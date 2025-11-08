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

    }
}