using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication
{
    public class SearchService
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public DataTable SearchData(string searchType, string keyword)
        {
            DataTable dt = new DataTable();
            string query = "";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                if (searchType == "Customer")
                {
                    query = "SELECT cus_id AS 'รหัสลูกค้า', cus_name AS 'ชื่อ', cus_lname AS 'นามสกุล', " +
                            "cus_tel AS 'เบอร์โทร', cus_email AS 'อีเมล' FROM customers " +
                            "WHERE cus_name LIKE @Keyword OR cus_lname LIKE @Keyword";
                }
                else if (searchType == "Employee")
                {
                    query = "SELECT emp_id AS 'รหัสพนักงาน', emp_name AS 'ชื่อ', emp_lname AS 'นามสกุล', " +
                            "emp_role AS 'ตำแหน่ง' FROM employees " +
                            "WHERE emp_role = 'Project Manager' AND (emp_name LIKE @Keyword OR emp_lname LIKE @Keyword)";
                }

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }
    }
}
