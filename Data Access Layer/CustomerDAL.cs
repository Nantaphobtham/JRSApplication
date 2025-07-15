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
    public class CustomerDAL
    {
        private string connectionString;

        public CustomerDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        // ✅  ดึงข้อมูลลูกค้าทั้งหมด
        public DataTable GetAllCustomers()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    string sql = "SELECT cus_id AS 'รหัสลูกค้า', cus_name AS 'ชื่อ', cus_lname AS 'นามสกุล', " +
                                 "cus_id_card AS 'เลขบัตรประชาชน', cus_tel AS 'เบอร์โทร', cus_email AS 'อีเมล', cus_address AS 'ที่อยู่' " +
                                 "FROM customer";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn))
                    {
                        conn.Open();
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการดึงข้อมูล: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dt;
        }
        //รอเรียก
        public Customer GetCustomerById(string customerId)
        {
            Customer customer = null;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM customer WHERE cus_id = @CustomerID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customer = new Customer
                            {
                                CustomerID = reader.GetString("cus_id"),
                                FirstName = reader.GetString("cus_name"),
                                LastName = reader.GetString("cus_lname"),
                                IDCard = reader.GetString("cus_id_card"),
                                Address = reader.GetString("cus_address"),
                                Email = reader.GetString("cus_email"),
                                Phone = reader.GetString("cus_tel")
                            };
                        }
                    }
                }
            }
            return customer;
        }





        public string GenerateCustomerID()
        {
            string year = (DateTime.Now.Year - 2000).ToString(); // ปี 2 หลัก เช่น 2025 -> "25"
            string month = DateTime.Now.Month.ToString("D2"); // เดือน 2 หลัก เช่น "09"
            string newID;
            Random rnd = new Random();
            CustomerDAL dal = new CustomerDAL(); // ✅ สร้างอ็อบเจ็กต์เพื่อเช็คซ้ำ

            do
            {
                string randoms = rnd.Next(0, 999).ToString("D3"); // ✅ เลขสุ่ม 3 หลัก
                newID = year + month + randoms;
            } while (dal.CheckDuplicateCustomer(newID,"","")); // ✅ ตรวจสอบว่ามีในฐานข้อมูลหรือไม่

            return newID;
        }


        // ✅  ตรวจสอบว่า Email หรือ ID Card ซ้ำหรือไม่
        public bool CheckDuplicateCustomer(string customerID, string email, string idCard)
        {
            bool exists = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT COUNT(*) FROM customer 
                       WHERE (cus_email = @Email OR cus_id_card = @IDCard) 
                       AND cus_id != @CustomerID";  // ✅ ยกเว้นตัวเอง

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@IDCard", idCard);

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    exists = count > 0;
                }
            }
            return exists;
        }


        // ✅  เพิ่มลูกค้าใหม่
        public bool InsertCustomer(Customer cus)
        {
            bool isSuccess = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string customerID = GenerateCustomerID(); // ✅ สร้างรหัสลูกค้าอัตโนมัติ

                // ✅ ตรวจสอบว่ามีข้อมูลซ้ำหรือไม่
                if (CheckDuplicateCustomer(customerID, cus.Email, cus.IDCard))
                {
                    MessageBox.Show("หมายเลขลูกค้า, อีเมล หรือเลขบัตรประชาชนซ้ำ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string sql = "INSERT INTO customer (cus_id, cus_name, cus_lname, cus_id_card, cus_tel, cus_address, cus_email) " +
                             "VALUES (@CustomerID, @FirstName, @LastName, @IDCard, @Phone, @Address, @Email)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    cmd.Parameters.AddWithValue("@FirstName", cus.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", cus.LastName);
                    cmd.Parameters.AddWithValue("@IDCard", cus.IDCard);
                    cmd.Parameters.AddWithValue("@Phone", cus.Phone);
                    cmd.Parameters.AddWithValue("@Address", cus.Address);
                    cmd.Parameters.AddWithValue("@Email", cus.Email);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    isSuccess = rows > 0;
                }
            }
            return isSuccess;
        }


        // ✅  แก้ไขข้อมูลลูกค้า 
        public bool UpdateCustomer(Customer cus)
        {
            bool isSuccess = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "UPDATE customer SET cus_name = @FirstName, cus_lname = @LastName, " +
                             "cus_id_card = @IDCard, cus_tel = @Phone, cus_address = @Address, cus_email = @Email " +
                             "WHERE cus_id = @CustomerID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", cus.CustomerID);
                    cmd.Parameters.AddWithValue("@FirstName", cus.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", cus.LastName);
                    cmd.Parameters.AddWithValue("@IDCard", cus.IDCard);
                    cmd.Parameters.AddWithValue("@Phone", cus.Phone);
                    cmd.Parameters.AddWithValue("@Address", cus.Address);
                    cmd.Parameters.AddWithValue("@Email", cus.Email);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    isSuccess = rows > 0;
                }
            }
            return isSuccess;
        }

        // ✅  ลบลูกค้า  
        public bool DeleteCustomer(int customerID)
        {
            bool isSuccess = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "DELETE FROM customer WHERE cus_id = @CustomerID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    isSuccess = rows > 0;
                }
            }
            return isSuccess;
        }
        public DataTable SearchCustomer(string searchBy, string keyword)
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM customer WHERE ";

            // ตรวจสอบตัวเลือกที่ใช้ค้นหา
            switch (searchBy)
            {
                case "ชื่อ":
                    query += "cus_name LIKE @Keyword";
                    break;
                case "เลขบัตรประชาชน":
                    query += "cus_id_card LIKE @Keyword";
                    break;
                case "อีเมล":
                    query += "cus_email LIKE @Keyword";
                    break;
                default:
                    query = "SELECT * FROM customer";
                    break;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
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

        public bool IsCustomerReferenced(int customerId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) FROM Projects WHERE CustomerID = @CustomerID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

    }
}
