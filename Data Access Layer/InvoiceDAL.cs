using JRSApplication.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace JRSApplication.Data_Access_Layer
{
    public class InvoiceDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        // InvoiceDAL.cs
        public string InsertInvoice(InvoiceModel invoice)
        {
            string newInvId = "INV_0001";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    // 1) Get last id and lock it to avoid race conditions
                    const string sqlGetLast = @"
                SELECT inv_id
                FROM invoice
                WHERE inv_id LIKE 'INV\_%'
                ORDER BY CAST(SUBSTRING(inv_id, 5) AS UNSIGNED) DESC
                LIMIT 1
                FOR UPDATE;";
                    using (var cmdGet = new MySqlCommand(sqlGetLast, conn, tx))
                    {
                        var last = cmdGet.ExecuteScalar();
                        if (last != null)
                        {
                            int n = int.Parse(last.ToString().Substring(4));
                            newInvId = $"INV_{(n + 1):D4}";
                        }
                    }

                    // 2) Insert
                    const string sqlInsert = @"
                INSERT INTO invoice (inv_id, inv_date, inv_duedate, inv_remark,  cus_id, pro_id, phase_id)
                VALUES (@Id, @InvDate, @DueDate, @Remark, @CusId, @ProId, @PhaseId);";
                    using (var cmd = new MySqlCommand(sqlInsert, conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@Id", newInvId);
                        cmd.Parameters.AddWithValue("@InvDate", invoice.InvDate);
                        cmd.Parameters.AddWithValue("@DueDate", invoice.InvDueDate);
                        cmd.Parameters.AddWithValue("@Remark", invoice.InvRemark ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CusId", invoice.CusId);
                        cmd.Parameters.AddWithValue("@ProId", invoice.ProId);
                        cmd.Parameters.AddWithValue("@PhaseId", string.IsNullOrEmpty(invoice.PhaseId) ? (object)DBNull.Value : invoice.PhaseId);
                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
            }
            return newInvId;
        }
        public void UpdateInvoiceAmounts(string invId, decimal total, decimal vat, decimal grand)
        {
            const string sql = @"
        UPDATE invoice
           SET inv_total_amount = @total,
               inv_vat_amount   = @vat,
               inv_grand_total  = @grand
         WHERE inv_id = @inv_id;";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@vat", vat);
                cmd.Parameters.AddWithValue("@grand", grand);
                cmd.Parameters.AddWithValue("@inv_id", invId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public DataTable GetAllInvoices()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT inv_no AS 'เลขที่ใบแจ้งหนี้',
                            inv_date AS 'วันที่ออกใบแจ้งหนี้',
                            inv_duedate AS 'กำหนดชำระ',
                            cus_id AS 'รหัสลูกค้า',
                            pro_id AS 'รหัสโครงการ',
                            phase_id AS 'เฟสงาน'
                     FROM invoice
                     ORDER BY inv_date DESC";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
            {
                conn.Open();
                adapter.Fill(dt);
            }

            return dt;
        }

        public DataTable GetUnpaidInvoicesByProject(string proId)
        {
            var dt = new DataTable();

            const string sql = @"
        SELECT
            i.inv_id,
            i.inv_date,
            i.inv_duedate,
            i.pro_id,
            i.cus_id,
            i.emp_id,
            i.inv_method,
            i.inv_status
        FROM invoice i
        WHERE i.pro_id = @ProId
          AND (
               i.paid_date IS NULL
               OR i.inv_status IS NULL
               OR i.inv_status NOT IN ('ชำระแล้ว','paid','ชำระเงินแล้ว')
          )
        ORDER BY i.inv_date DESC, i.inv_id DESC;";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@ProId", proId);
                conn.Open();
                da.Fill(dt);
            }
            return dt;
        }

        public Image GetPaymentProofImage(int invId)
        {
            string query = "SELECT file_data FROM payment_proof WHERE inv_id = @invId";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@invId", invId);
                conn.Open();

                var result = cmd.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                {
                    byte[] imageBytes = (byte[])result;
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        return Image.FromStream(ms);
                    }
                }
            }

            return null; // not found
        }

        public DataTable GetInvoicesByProjectId(string projectId)
        {
            DataTable dt = new DataTable();
            string query = @"
                            SELECT 
                            invoice.inv_id,
                            invoice.inv_date,
                            invoice.inv_duedate,
                            invoice.inv_status,
                            invoice.inv_method,
                            invoice.paid_date,
                            invoice.emp_id,
                            CONCAT(employee.emp_name, ' ', employee.emp_lname) AS emp_fullname,
                            invoice.pro_id,
                            project.pro_name,
                            CONCAT(customer.cus_name, ' ', customer.cus_lname) AS cus_fullname,
                            customer.cus_id_card,
                            customer.cus_address,
                            invoice.phase_id,
                            project_phase.phase_no      as phase_no
                            FROM invoice
                            JOIN project ON invoice.pro_id = project.pro_id
                            JOIN customer ON project.cus_id = customer.cus_id
                            JOIN project_phase ON project_phase.phase_id = invoice.phase_id
                            LEFT JOIN employee ON invoice.emp_id = employee.emp_id
                            WHERE invoice.inv_status = 'ชำระแล้ว'
                            AND invoice.pro_id = @ProjectId";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ProjectId", projectId);
                conn.Open();
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        public static DataTable GetInvoiceDetail(string invoiceId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            DataTable dt = new DataTable();

            string query = @"SELECT inv_detail, inv_quantity, inv_price 
                     FROM invoice_detail 
                     WHERE inv_id = @inv_id";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@inv_id", invoiceId);
                conn.Open();

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        public int InsertReceipt(string receiptNo, DateTime receiptDate, string remark, int invId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"INSERT INTO receipt (receipt_no, receipt_date, remark, inv_id)
                             VALUES (@receipt_no, @receipt_date, @remark, @inv_id)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@receipt_no", receiptNo);
                cmd.Parameters.AddWithValue("@receipt_date", receiptDate);
                cmd.Parameters.AddWithValue("@remark", remark);
                cmd.Parameters.AddWithValue("@inv_id", invId);

                conn.Open();
                return cmd.ExecuteNonQuery(); // return rows affected
            }
        }
        public string GetInvoiceRemark(string invId)
        {
            string remark = "";
            string sql = "SELECT inv_remark FROM invoice WHERE inv_id = @invId";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@invId", invId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    remark = result.ToString();
                }
            }

            return remark;
        }
        public DataTable GetInvoiceID(string invId)
        {
            DataTable dt = new DataTable();
            string query = @"
        SELECT 
            i.inv_id,
            i.inv_id AS inv_no,
            i.inv_date,
            i.inv_duedate,
            i.inv_remark,
            p.pro_id,
            p.pro_name,
            p.pro_number,
            c.cus_id,
            CONCAT(c.cus_name, ' ', c.cus_lname) AS cus_fullname,
            c.cus_id_card,
            c.cus_address,
            ph.phase_id,
            ph.phase_no        AS phase_no,
            ph.phase_budget,
            ph.phase_detail,
            d.inv_detail,
            d.inv_quantity,
            d.inv_price
        FROM invoice i
        JOIN project p ON i.pro_id = p.pro_id
        JOIN customer c ON p.cus_id = c.cus_id
        JOIN project_phase ph ON i.phase_id = ph.phase_id
        LEFT JOIN invoice_detail d ON i.inv_id = d.inv_id
        WHERE i.inv_id = @invId
        LIMIT 1";  // Return only first invoice_detail row

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@invId", invId);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }
        public string PeekNextInvoiceId()
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                const string sql = @"
            SELECT inv_id
            FROM invoice
            WHERE inv_id LIKE 'INV\_%'
            ORDER BY CAST(SUBSTRING(inv_id, 5) AS UNSIGNED) DESC
            LIMIT 1;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    var last = cmd.ExecuteScalar();
                    if (last == null || last == DBNull.Value) return "INV_0001";
                    int n = 0; int.TryParse(last.ToString().Substring(4), out n);
                    return $"INV_{(n + 1):D4}";
                }
            }
        }



        public DataTable GetAllInvoicesByProjectId(string projectId)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT 
                    invoice.inv_id,
                    invoice.inv_date,
                    invoice.inv_duedate,
                    invoice.inv_status,
                    invoice.inv_method,
                    invoice.paid_date,
                    invoice.emp_id,
                    CONCAT(employee.emp_name, ' ', employee.emp_lname) AS emp_fullname,
                    invoice.pro_id,
                    project.pro_name,
                    CONCAT(customer.cus_name, ' ', customer.cus_lname) AS cus_fullname,
                    customer.cus_id_card,
                    customer.cus_address,
                    invoice.phase_id,
                    project_phase.phase_no      as phase_no
                FROM invoice
                JOIN project ON invoice.pro_id = project.pro_id
                JOIN customer ON project.cus_id = customer.cus_id
                JOIN project_phase ON project_phase.phase_id = invoice.phase_id 
                LEFT JOIN employee ON invoice.emp_id = employee.emp_id
                WHERE invoice.pro_id = @ProjectId
                ORDER BY invoice.inv_date DESC";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ProjectId", projectId);
                conn.Open();
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        public string GetPhaseNoById(int phaseId)
        {
            string phaseNo = "";
            string query = "SELECT phase_no FROM project_phase WHERE phase_id = @PhaseId";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PhaseId", phaseId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                    phaseNo = result.ToString();
            }

            return phaseNo;
        }



    }
}
