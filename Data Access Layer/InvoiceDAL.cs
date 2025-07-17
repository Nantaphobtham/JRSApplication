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




    }
}
