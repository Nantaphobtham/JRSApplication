using JRSApplication.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication.Data_Access_Layer
{
    public class InvoiceDetailDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        // InvoiceDetailDAL.cs
        public void InsertInvoiceDetail(string invId, string detail, decimal price, string quantityText, decimal vatRate)
        {
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(@"
        INSERT INTO invoice_detail (inv_id, inv_detail, inv_price, inv_quantity, inv_vat_rate)
        VALUES (@InvId, @Detail, @Price, @QtyText, @VatRate);", conn))
            {
                cmd.Parameters.AddWithValue("@InvId", invId);
                cmd.Parameters.AddWithValue("@Detail", detail);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@QtyText", string.IsNullOrWhiteSpace(quantityText) ? (object)DBNull.Value : quantityText);
                cmd.Parameters.AddWithValue("@VatRate", vatRate);

                conn.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    var dbName = Convert.ToString(new MySqlCommand("SELECT DATABASE();", conn).ExecuteScalar());
                    var dump =
                        "DB: " + dbName + Environment.NewLine +
                        "SQL: " + cmd.CommandText + Environment.NewLine +
                        $"@InvId={invId}, @Detail={detail}, @Price={price}, @QtyText={quantityText}, @VatRate={vatRate}" + Environment.NewLine +
                        $"MySQL #{ex.Number} SqlState={ex.SqlState}" + Environment.NewLine +
                        ex.Message;
                    MessageBox.Show(dump, "InsertInvoiceDetail failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
        public DataTable GetDetailsByInvId(string invId)
        {
            using (var conn = new MySqlConnection(connectionString))
            using (var da = new MySqlDataAdapter(@"
        SELECT inv_detail, inv_price
        FROM jrsconstruction.invoice_detail
        WHERE inv_id = @id
        ORDER BY inv_detail_id;", conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@id", invId);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public (string Detail, string Quantity, decimal Price)? GetFirstDetailForPrint(string invId)
        {
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(@"
        SELECT inv_detail, inv_quantity, inv_price
        FROM jrsconstruction.invoice_detail
        WHERE inv_id = @id
        ORDER BY inv_detail_id
        LIMIT 1;", conn))
            {
                cmd.Parameters.AddWithValue("@id", invId);
                conn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        string d = r["inv_detail"]?.ToString();
                        string q = r["inv_quantity"]?.ToString();
                        decimal p = 0m;
                        if (r["inv_price"] != DBNull.Value) p = Convert.ToDecimal(r["inv_price"]);
                        return (d, q, p);
                    }
                }
            }
            return null;
        }

    }
}
                      