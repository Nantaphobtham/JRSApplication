using JRSApplication.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JRSApplication.Data_Access_Layer
{
    public class CustomerDAL
    {
        private readonly string connectionString;
        private readonly Random rnd = new Random();

        public CustomerDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        // ✅ ดึงข้อมูลลูกค้าทั้งหมด (คอลัมน์ alias เป็นภาษาไทย ใช้กับ Grid ได้ทันที)
        public DataTable GetAllCustomers()
        {
            DataTable dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    string sql =
                        @"SELECT 
                            cus_id      AS 'รหัสลูกค้า',
                            cus_name    AS 'ชื่อ',
                            cus_lname   AS 'นามสกุล',
                            cus_id_card AS 'เลขบัตรประชาชน',
                            cus_tel     AS 'เบอร์โทร',
                            cus_email   AS 'อีเมล',
                            cus_address AS 'ที่อยู่'
                          FROM customer";

                    using (var adapter = new MySqlDataAdapter(sql, conn))
                    {
                        conn.Open();
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการดึงข้อมูล: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dt;
        }

        // ✅ เช็คว่ามี cus_id นี้อยู่แล้วหรือไม่ (ใช้กับการออกเลขไอดีใหม่)
        private bool CustomerIdExists(string customerID)
        {
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM customer WHERE cus_id = @id";
                cmd.Parameters.AddWithValue("@id", customerID);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        // ✅ ออกเลขรหัสลูกค้า: YY + MM + (สุ่ม 3 หลัก) และเช็คไม่ให้ซ้ำจริง ๆ ที่ cus_id
        public string GenerateCustomerID()
        {
            string year = (DateTime.Now.Year - 2000).ToString("D2"); // 2025 -> 25
            string month = DateTime.Now.Month.ToString("D2");        // 09
            string newID;

            do
            {
                string randoms = rnd.Next(0, 1000).ToString("D3");    // 000-999
                newID = year + month + randoms;
            } while (CustomerIdExists(newID));

            return newID;
        }

        // ✅ ตรวจซ้ำ Email หรือ ID Card (ข้ามเงื่อนไขที่เป็นค่าว่าง) – เทียบอีเมลแบบ case-insensitive
        public bool CheckDuplicateCustomer(string customerID, string email, string idCard)
        {
            bool emailGiven = !string.IsNullOrWhiteSpace(email);
            bool idGiven = !string.IsNullOrWhiteSpace(idCard);

            if (!emailGiven && !idGiven) return false;

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = conn.CreateCommand())
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT COUNT(*) FROM customer WHERE 1=1");

                // ยกเว้นแถวของตัวเองเมื่อแก้ไข
                if (!string.IsNullOrWhiteSpace(customerID))
                {
                    sb.AppendLine("  AND cus_id <> @CustomerID");
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                }

                // (email OR idcard)
                sb.Append("  AND (0");
                if (emailGiven)
                {
                    sb.Append(" OR LOWER(cus_email) = LOWER(@Email)");
                    cmd.Parameters.AddWithValue("@Email", email);
                }
                if (idGiven)
                {
                    sb.Append(" OR cus_id_card = @IDCard");
                    cmd.Parameters.AddWithValue("@IDCard", idCard);
                }
                sb.Append(")");

                cmd.CommandText = sb.ToString();
                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        // ✅ เพิ่มลูกค้า
        public bool InsertCustomer(Customer cus)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                using (var cmd = conn.CreateCommand())
                {
                    string customerID = GenerateCustomerID();

                    // เช็คซ้ำ (email OR idcard) – ไม่ต้องยกเว้น cus_id เพราะยังไม่มีใน DB
                    if (CheckDuplicateCustomer(null, cus.Email, cus.IDCard))
                    {
                        MessageBox.Show("อีเมลหรือเลขบัตรประชาชนซ้ำ!", "แจ้งเตือน",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    cmd.CommandText =
                        @"INSERT INTO customer
                          (cus_id, cus_name, cus_lname, cus_id_card, cus_tel, cus_address, cus_email)
                          VALUES
                          (@CustomerID, @FirstName, @LastName, @IDCard, @Phone, @Address, @Email)";

                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    cmd.Parameters.AddWithValue("@FirstName", cus.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", cus.LastName);
                    cmd.Parameters.AddWithValue("@IDCard", cus.IDCard);
                    cmd.Parameters.AddWithValue("@Phone", cus.Phone);
                    cmd.Parameters.AddWithValue("@Address", cus.Address);
                    cmd.Parameters.AddWithValue("@Email", cus.Email);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (MySqlException mex) when (mex.Number == 1062) // Duplicate entry
            {
                string msg = "ข้อมูลซ้ำในระบบ";
                if (mex.Message.Contains("cus_email")) msg = "อีเมลนี้มีอยู่แล้วในระบบ";
                else if (mex.Message.Contains("cus_id_card")) msg = "เลขบัตรประชาชนนี้มีอยู่แล้วในระบบ";
                else if (mex.Message.Contains("PRIMARY") || mex.Message.Contains("cus_id")) msg = "รหัสลูกค้าซ้ำในระบบ";

                MessageBox.Show(msg, "ข้อมูลซ้ำ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("InsertCustomer Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ✅ แก้ไขลูกค้า
        public bool UpdateCustomer(Customer cus)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                using (var cmd = conn.CreateCommand())
                {
                    // ยกเว้นแถวตัวเองตอนเช็คซ้ำ
                    if (CheckDuplicateCustomer(cus.CustomerID.ToString(), cus.Email, cus.IDCard))
                    {
                        MessageBox.Show("อีเมลหรือเลขบัตรประชาชนซ้ำ!", "แจ้งเตือน",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    cmd.CommandText =
                        @"UPDATE customer
                             SET cus_name    = @FirstName,
                                 cus_lname   = @LastName,
                                 cus_id_card = @IDCard,
                                 cus_tel     = @Phone,
                                 cus_address = @Address,
                                 cus_email   = @Email
                           WHERE cus_id = @CustomerID";

                    cmd.Parameters.AddWithValue("@CustomerID", cus.CustomerID);
                    cmd.Parameters.AddWithValue("@FirstName", cus.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", cus.LastName);
                    cmd.Parameters.AddWithValue("@IDCard", cus.IDCard);
                    cmd.Parameters.AddWithValue("@Phone", cus.Phone);
                    cmd.Parameters.AddWithValue("@Address", cus.Address);
                    cmd.Parameters.AddWithValue("@Email", cus.Email);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (MySqlException mex) when (mex.Number == 1062) // Duplicate entry
            {
                string msg = "ข้อมูลซ้ำในระบบ";
                if (mex.Message.Contains("cus_email")) msg = "อีเมลนี้มีอยู่แล้วในระบบ";
                else if (mex.Message.Contains("cus_id_card")) msg = "เลขบัตรประชาชนนี้มีอยู่แล้วในระบบ";
                MessageBox.Show(msg, "ข้อมูลซ้ำ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("UpdateCustomer Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ✅ ลบลูกค้า (แนะนำให้เรียก IsCustomerReferenced ก่อนเสมอ)
        public bool DeleteCustomer(int customerID)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM customer WHERE cus_id = @CustomerID";
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (MySqlException mex)
            {
                // ถ้ามี FK constraint จะมาลงเคสนี้
                MessageBox.Show("DeleteCustomer Error: " + mex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DeleteCustomer Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ✅ ค้นหาลูกค้า: คืนนิยามคอลัมน์ alias ให้เหมือน GetAllCustomers
        public DataTable SearchCustomer(string searchBy, string keyword)
        {
            DataTable dt = new DataTable();
            string where = "";
            string col;

            // รองรับชื่อจากหลายหน้าจอ
            switch (searchBy)
            {
                case "ชื่อ":
                case "ชื่อลูกค้า":              // จาก searchboxCustomer
                    col = "cus_name";
                    break;

                case "นามสกุล":
                case "นามสกูลลูกค้า":           // เผื่ออนาคตมีใช้
                    col = "cus_lname";
                    break;

                case "เลขบัตรประชาชน":
                case "เลขประจำตัว":            // จาก searchboxCustomer
                    col = "cus_id_card";
                    break;

                case "อีเมล":
                    col = "cus_email";
                    break;

                default:
                    col = ""; // ไม่มี column -> จะดึงทั้งหมด
                    break;
            }

            if (!string.IsNullOrWhiteSpace(col) && !string.IsNullOrWhiteSpace(keyword))
            {
                where = $" WHERE {col} LIKE @kw ";
            }

            string sql =
                $@"SELECT 
                    cus_id      AS 'รหัสลูกค้า',
                    cus_name    AS 'ชื่อ',
                    cus_lname   AS 'นามสกุล',
                    cus_id_card AS 'เลขบัตรประชาชน',
                    cus_tel     AS 'เบอร์โทร',
                    cus_email   AS 'อีเมล',
                    cus_address AS 'ที่อยู่'
                  FROM customer
                  {where}";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            using (var adapter = new MySqlDataAdapter(cmd))
            {
                if (!string.IsNullOrWhiteSpace(where))
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                conn.Open();
                adapter.Fill(dt);
            }

            return dt;
        }

        // ===== Helper: อ่านชื่อฐานข้อมูลจากคอนเนคชันปัจจุบัน =====
        private string GetCurrentDatabaseName()
        {
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT DATABASE()";
                conn.Open();
                return Convert.ToString(cmd.ExecuteScalar());
            }
        }

        // ===== Helper: หา "ชื่อคอลัมน์ลูกค้า" ที่มีอยู่จริงในตาราง =====
        private string TryGetCustomerColumn(string tableName)
        {
            string[] candidates = { "cus_id", "customer_id", "CustomerID", "cusid", "customerid" };

            string db = GetCurrentDatabaseName();
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT COLUMN_NAME
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_SCHEMA = @db AND TABLE_NAME = @table 
                          AND COLUMN_NAME IN (" + string.Join(",", candidates.Select((_, i) => "@c" + i)) + ")";
                cmd.Parameters.AddWithValue("@db", db);
                cmd.Parameters.AddWithValue("@table", tableName);
                for (int i = 0; i < candidates.Length; i++)
                    cmd.Parameters.AddWithValue("@c" + i, candidates[i]);

                conn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                        return r.GetString(0); // คืนชื่อคอลัมน์ที่พบตัวแรก
                }
            }
            return null; // ไม่พบคอลัมน์ลูกค้าในตารางนี้
        }

        // ✅ เช็คการอ้างอิงแบบยืดหยุ่นชื่อคอลัมน์ (project/invoice/purchaseorder)
        public bool IsCustomerReferenced(int customerId)
        {
            string[] tables = { "project", "invoice", "purchaseorder" };

            var sqlParts = new List<string>();
            var parameters = new List<MySqlParameter>();

            for (int i = 0; i < tables.Length; i++)
            {
                string table = tables[i];
                string col = TryGetCustomerColumn(table);
                if (!string.IsNullOrEmpty(col))
                {
                    string p = "@cid_" + i;
                    sqlParts.Add($"(SELECT COUNT(*) FROM `{table}` WHERE `{col}` = {p})");
                    parameters.Add(new MySqlParameter(p, customerId));
                }
            }

            if (sqlParts.Count == 0) return false;

            string finalSql = "SELECT " + string.Join(" + ", sqlParts) + " AS ref_count;";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = finalSql;
                cmd.Parameters.AddRange(parameters.ToArray());
                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        // ✅ ดึงลูกค้าตามรหัส (ใช้ในฟอร์ม)
        public Customer GetCustomerById(string customerId)
        {
            Customer customer = null;
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM customer WHERE cus_id = @CustomerID";
                cmd.Parameters.AddWithValue("@CustomerID", customerId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        customer = new Customer
                        {
                            CustomerID = reader.GetInt32("cus_id"),
                            FirstName = reader.GetString("cus_name"),
                            LastName = reader.GetString("cus_lname"),
                            Email = reader.GetString("cus_email"),
                            Phone = reader.GetString("cus_tel"),
                            Address = reader.GetString("cus_address"),
                            IDCard = reader.GetString("cus_id_card")
                        };
                    }
                }
            }
            return customer;
        }
    }
}
