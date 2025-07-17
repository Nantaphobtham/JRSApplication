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



    }
}
