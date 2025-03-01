using JRSApplication.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication.Data_Access_Layer
{
    public class SupplierDAL
    {
        private string connectionString;

        public SupplierDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        // ✅ 1️⃣ ดึงข้อมูลซัพพลายเออร์ทั้งหมด
        public DataTable GetAllSuppliers()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    string sql = "SELECT sup_id AS 'รหัสซัพพลายเออร์', sup_name AS 'ชื่อบริษัท', " +
                                 "sup_juristic AS 'นิติบุคคล', sup_tel AS 'เบอร์โทรศัพท์', " +
                                 "sup_email AS 'อีเมล', sup_address AS 'ที่อยู่' FROM supplier";

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

        // ✅ 2️⃣ สร้างรหัสซัพพลายเออร์อัตโนมัติ (10 หลัก)
        public string GenerateSupplierID()
        {
            string year = DateTime.Now.Year.ToString().Substring(2); // ปี 2 หลัก เช่น 2025 -> "25"
            string month = DateTime.Now.Month.ToString("D2"); // เดือน 2 หลัก เช่น "09"
            string newID;
            Random rnd = new Random();
            SupplierDAL dal = new SupplierDAL();

            do
            {
                string randoms = rnd.Next(1000, 9999).ToString(); // สุ่มเลข 4 หลัก
                newID = year + month + randoms;
            } while (dal.CheckDuplicateSupplierID(newID));  // ✅ ตรวจสอบว่าซ้ำหรือไม่

            return newID;
        }

        public bool CheckDuplicateSupplierID(string supplierID)
        {
            bool exists = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) FROM supplier WHERE sup_id = @SupplierID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    exists = count > 0;
                }
            }
            return exists;
        }

        // ✅ 3️⃣ ตรวจสอบว่า ID, Email ซ้ำหรือไม่
        public bool CheckDuplicateSupplier(string email, string phone, string juristic, string name, string supplierID)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM supplier WHERE "
                             + "(sup_email = @Email OR sup_tel = @Phone OR sup_juristic = @Juristic OR sup_name = @Name) ";

                // ✅ ถ้ามี `supplierID` แสดงว่าเป็นการอัปเดต ไม่ต้องเช็คตัวเอง
                if (!string.IsNullOrEmpty(supplierID))
                {
                    query += " AND sup_id != @SupplierID";
                }

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Juristic", juristic);
                    cmd.Parameters.AddWithValue("@Name", name);

                    if (!string.IsNullOrEmpty(supplierID))
                    {
                        cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                    }

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0; // ✅ คืนค่า true ถ้าข้อมูลซ้ำ
                }
            }
        }


        // ✅ 4️⃣ เพิ่มข้อมูลซัพพลายเออร์
        public bool InsertSupplier(Supplier sup)
        {
            bool isSuccess = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string supplierID = GenerateSupplierID(); // ✅ สร้างรหัสซัพพลายเออร์ใหม่

                if (CheckDuplicateSupplier(sup.Email, sup.Phone, sup.Juristic, sup.Name, ""))
                {
                    MessageBox.Show("อีเมลซ้ำในระบบ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string sql = "INSERT INTO supplier (sup_id, sup_name, sup_juristic, sup_tel, sup_address, sup_email) " +
                             "VALUES (@SupplierID, @Name, @Juristic, @Phone, @Address, @Email)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                    cmd.Parameters.AddWithValue("@Name", sup.Name);
                    cmd.Parameters.AddWithValue("@Juristic", sup.Juristic);
                    cmd.Parameters.AddWithValue("@Phone", sup.Phone);
                    cmd.Parameters.AddWithValue("@Address", sup.Address);
                    cmd.Parameters.AddWithValue("@Email", sup.Email);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    isSuccess = rows > 0;
                }
            }
            return isSuccess;
        }

        // ✅ 5️⃣ อัปเดตข้อมูลซัพพลายเออร์
        public bool UpdateSupplier(string supplierID, string name, string juristic, string phone, string email, string address)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE supplier SET sup_name = @Name, sup_juristic = @Juristic, " +
                               "sup_tel = @Phone, sup_email = @Email, sup_address = @Address " +
                               "WHERE sup_id = @SupplierID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Juristic", juristic);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Address", address);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0; // ✅ คืนค่า true ถ้ามีแถวถูกอัปเดต
                }
            }
        }


        // ✅ 6️⃣ ลบซัพพลายเออร์
        public bool DeleteSupplier(string supplierID)
        {
            bool isSuccess = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "DELETE FROM supplier WHERE sup_id = @SupplierID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    isSuccess = rows > 0;
                }
            }
            return isSuccess;
        }

        public DataTable SearchSuppliers(string searchBy, string keyword)
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM suppliers WHERE ";

            // ตรวจสอบตัวเลือกที่ใช้ค้นหา
            switch (searchBy)
            {
                case "ชื่อบริษัท":
                    query += "supplier_name LIKE @Keyword";
                    break;
                case "เลขทะเบียนนิติบุคคล":
                    query += "supplier_idcard LIKE @Keyword";
                    break;
                case "อีเมล":
                    query += "supplier_email LIKE @Keyword";
                    break;
                default:
                    query = "SELECT * FROM suppliers"; // ถ้าไม่มีตัวเลือกให้ดึงข้อมูลทั้งหมด
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
    }
}
