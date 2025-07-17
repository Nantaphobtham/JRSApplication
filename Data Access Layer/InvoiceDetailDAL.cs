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

        public void InsertInvoiceDetail(int invId, string detail, decimal price, int quantity, decimal vatRate)
        {
            string query = @"
                INSERT INTO invoice_detail (inv_id, inv_detail, inv_price, inv_quantity, inv_vat_rate)
                VALUES (@InvId, @Detail, @Price, @Quantity, @VatRate)";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@InvId", invId);
                cmd.Parameters.AddWithValue("@Detail", detail);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@VatRate", vatRate);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
