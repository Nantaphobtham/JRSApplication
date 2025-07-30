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

        public int InsertInvoice(InvoiceModel invoice)
        {
            int insertedId = 0;

            string sql = @"
        INSERT INTO invoice (inv_no, inv_date, inv_duedate, cus_id, pro_id, phase_id)
        VALUES
        (@InvNo, @InvDate, @DueDate, @CusId, @ProId, @PhaseId);
        SELECT LAST_INSERT_ID();
    ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@InvNo", invoice.InvNo);
                cmd.Parameters.AddWithValue("@InvDate", invoice.InvDate);
                cmd.Parameters.AddWithValue("@DueDate", invoice.InvDueDate);
                cmd.Parameters.AddWithValue("@CusId", invoice.CusId);
                cmd.Parameters.AddWithValue("@ProId", invoice.ProId);
                cmd.Parameters.AddWithValue("@PhaseId", invoice.PhaseId);

                conn.Open();
                insertedId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return insertedId;
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
        invoice.inv_no,
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
        invoice.phase_id
    FROM invoice
    JOIN project ON invoice.pro_id = project.pro_id
    JOIN customer ON project.cus_id = customer.cus_id
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

        public static DataTable GetInvoiceDetail(int invoiceId)
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
        public string GetInvoiceRemark(int invId)
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




    }
}
