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
    public class ReceiptDAL
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public int InsertReceipt(string receiptNo /* now receiptId */, DateTime receiptDate, string remark, string invId)
        {
            int result = 0;

            const string query = @"
                INSERT INTO receipt (receipt_id, receipt_date, remark, inv_id)
                VALUES (@receipt_id, @receipt_date, @remark, @inv_id);";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@receipt_id", receiptNo ?? string.Empty); // pass your auto-generated REC_0001
                cmd.Parameters.AddWithValue("@receipt_date", receiptDate);
                cmd.Parameters.AddWithValue("@remark", string.IsNullOrWhiteSpace(remark) ? (object)DBNull.Value : remark);
                cmd.Parameters.AddWithValue("@inv_id", invId);

                conn.Open();
                result = cmd.ExecuteNonQuery();
            }

            return result;
        }
        public string GetReceiptNoByInvId(string invId)
        {
            const string query = @"
                SELECT receipt_id
                FROM receipt
                WHERE inv_id = @inv_id
                ORDER BY receipt_date DESC, receipt_id DESC
                LIMIT 1;";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@inv_id", invId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result?.ToString() ?? string.Empty;
            }
        }
        public string GetReceiptRemarkByInvId(string invId)
        {
            const string sql = @"
        SELECT remark
        FROM receipt
        WHERE inv_id = @invId
        ORDER BY receipt_date DESC
        LIMIT 1;";
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@invId", invId);
                conn.Open();
                var o = cmd.ExecuteScalar();
                return (o == null || o == DBNull.Value) ? "" : o.ToString();
            }
        }

        // In ReceiptDAL.cs
        public DataTable GetReceiptDetailsByInvoiceId(string invoiceId)
        {
            var dt = new DataTable();
            // THE FIX IS HERE: We added a LEFT JOIN to project_phase to get phase_no
            const string query = @"
        SELECT 
            r.receipt_id,
            r.receipt_date,
            r.remark AS receipt_remark,
            i.inv_id,
            i.inv_method,
            i.paid_date,
            ph.phase_no,  -- <-- Changed from i.phase_id to ph.phase_no
            CONCAT(c.cus_name, ' ', c.cus_lname) AS cus_fullname,
            c.cus_id_card,
            c.cus_address,
            p.pro_id,
            p.pro_name,
            CONCAT(e.emp_name, ' ', e.emp_lname) AS emp_fullname
        FROM 
            receipt r
        LEFT JOIN 
            invoice i ON r.inv_id = i.inv_id
        LEFT JOIN 
            customer c ON i.cus_id = c.cus_id
        LEFT JOIN 
            project p ON i.pro_id = p.pro_id
        LEFT JOIN 
            employee e ON i.emp_id = e.emp_id
        LEFT JOIN
            project_phase ph ON i.phase_id = ph.phase_id -- <-- Added this join
        WHERE 
            r.inv_id = @InvoiceId
        LIMIT 1;";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }
    }
}