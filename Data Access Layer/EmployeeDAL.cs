using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace JRSApplication.Components
{
    public class EmployeeDAL
    {
        private string connectionString;

        public EmployeeDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        private string GenerateEmployeeID(string role)
        {
            string prefix = "";

            switch (role)
            {
                case "Admin":
                    prefix = "A";
                    break;
                case "Projectmanager":
                case "Sitesupervisor":
                    prefix = "E";
                    break;
                case "Accountant":
                    prefix = "F";
                    break;
                default:
                    prefix = "X";
                    break;
            }
            

            // ปีปัจจุบันเป็น 2 หลัก (เช่น 2024 -> 64)
            string year = (DateTime.Now.Year - 2000).ToString();

            // สุ่มเลข 3 หลัก
            string newID;
            Random rnd = new Random();
            EmployeeDAL dal = new EmployeeDAL();  // ✅ สร้างอ็อบเจ็กต์ EmployeeDAL

            do
            {
                string randoms = rnd.Next(0, 999).ToString("D3");
                newID = prefix + year + randoms;
            } while (dal.CheckDuplicateEmployee(newID, "", "", ""));  // ✅ เรียกใช้ผ่าน `dal`

            return newID;
        }

        public bool CheckDuplicateEmployee(string employeeID, string username, string email, string idCard)
        {
            bool exists = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) FROM employee WHERE emp_id = @EmployeeID OR emp_username = @Username OR emp_email = @Email OR emp_identification = @IDCard";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@IDCard", idCard);

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    exists = count > 0;
                }
            }
            return exists;
        }


        public bool InsertEmployee(Employee emp)
        {
            bool isSuccess = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string employeeID = GenerateEmployeeID(emp.Role); // ✅ สร้าง emp_id แบบไม่ซ้ำ

                string sql = "INSERT INTO employee (emp_id, emp_username, emp_password, emp_name, emp_lname, emp_tel, emp_address, emp_email, emp_identification, emp_pos) " +
                             "VALUES (@EmployeeID, @Username, @Password, @FirstName, @LastName, @Phone, @Address, @Email, @IDCard, @Role)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.Parameters.AddWithValue("@Username", emp.Username);
                    cmd.Parameters.AddWithValue("@Password", emp.Password);
                    cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", emp.LastName);
                    cmd.Parameters.AddWithValue("@Phone", emp.Phone);
                    cmd.Parameters.AddWithValue("@Address", emp.Address);
                    cmd.Parameters.AddWithValue("@Email", emp.Email);
                    cmd.Parameters.AddWithValue("@IDCard", emp.IDCard);
                    cmd.Parameters.AddWithValue("@Role", emp.Role);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    isSuccess = rows > 0;
                }
            }
            return isSuccess;
        }
        public bool UpdateEmployee(Employee emp)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "SELECT emp_id, emp_name AS 'ชื่อ', emp_lname AS 'นามสกุล', emp_username AS 'ชื่อผู้ใช้', " +
                             "emp_password AS 'รหัสผ่าน', emp_tel AS 'เบอร์โทร', emp_email AS 'อีเมล', emp_pos AS 'ตำแหน่ง', " +
                             "emp_identification AS 'เลขบัตรประชาชน', emp_address AS 'ที่อยู่' " +
                             "FROM employee WHERE emp_id = @EmployeeID";


                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", emp.EmployeeID);
                    cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", emp.LastName);
                    cmd.Parameters.AddWithValue("@Username", emp.Username);
                    cmd.Parameters.AddWithValue("@Password", emp.Password);
                    cmd.Parameters.AddWithValue("@Phone", emp.Phone);
                    cmd.Parameters.AddWithValue("@Email", emp.Email);
                    cmd.Parameters.AddWithValue("@Role", emp.Role);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public bool DeleteEmployee(string employeeID)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "DELETE FROM employee WHERE emp_id = @EmployeeID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        //GetEmployeeByIDtoUsermanagementForm
        public DataTable GetEmployeeByIDtoUMNGT(string employeeID)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "SELECT emp_id, emp_name AS 'ชื่อ', emp_lname AS 'นามสกุล', emp_username AS 'ชื่อผู้ใช้', " +
                             "emp_tel AS 'เบอร์โทร', emp_email AS 'อีเมล', emp_pos AS 'ตำแหน่ง', emp_identification AS 'เลขบัตรประชาชน', " +
                             "emp_address AS 'ที่อยู่', emp_password AS 'รหัสผ่าน' " +  
                             "FROM employee WHERE emp_id = @EmployeeID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    conn.Open();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable GetAllEmployees()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    string sql = "SELECT emp_id AS 'รหัสพนักงาน', emp_username AS 'ชื่อผู้ใช้', emp_name AS 'ชื่อ', emp_lname AS 'นามสกุล', " +
                                 "emp_tel AS 'เบอร์โทร', emp_email AS 'อีเมล', emp_pos AS 'ตำแหน่ง' FROM employee";

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

        public string GetEmployeeFullNameById(string empId)
        {
            string fullName = "";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "SELECT emp_name, emp_lname FROM employee WHERE emp_id = @EmpID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmpID", empId);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string firstName = reader["emp_name"].ToString();
                            string lastName = reader["emp_lname"].ToString();
                            fullName = $"{firstName} {lastName}";
                        }
                    }
                }
            }
            return fullName;
        }


    }
}
