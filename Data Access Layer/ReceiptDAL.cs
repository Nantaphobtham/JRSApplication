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

        public int InsertReceipt(string receiptNo, DateTime receiptDate, string remark, int invId)
        {
            int result = 0;

            string query = @"
                INSERT INTO receipt (receipt_no, receipt_date, remark, inv_id)
                VALUES (@receipt_no, @receipt_date, @remark, @inv_id)";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@receipt_no", receiptNo);
                cmd.Parameters.AddWithValue("@receipt_date", receiptDate);
                cmd.Parameters.AddWithValue("@remark", remark);
                cmd.Parameters.AddWithValue("@inv_id", invId);

                conn.Open();
                result = cmd.ExecuteNonQuery();
            }

            return result;
        }
        public string GetReceiptNoByInvId(int invId)
        {
            string receiptNo = "";
            string query = "SELECT receipt_no FROM receipt WHERE inv_id = @inv_id";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@inv_id", invId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                    receiptNo = result.ToString();
            }

            return receiptNo;
        }

    }
}