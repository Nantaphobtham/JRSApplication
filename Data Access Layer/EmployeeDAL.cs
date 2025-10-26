using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace JRSApplication.Components
{
    public class EmployeeDAL
    {
        private readonly string connectionString;

        public EmployeeDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        // ====== Exists Helpers (กันข้อมูลซ้ำ) ======
        public bool ExistsEmail(string email, string excludeEmployeeId = null)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"
                SELECT 1
                FROM employee
                WHERE LOWER(TRIM(emp_email)) = LOWER(TRIM(@Email)) " +
                (excludeEmployeeId != null ? "AND emp_id <> @EmpID " : "") +
                "LIMIT 1;";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    if (excludeEmployeeId != null) cmd.Parameters.AddWithValue("@EmpID", excludeEmployeeId);
                    conn.Open();
                    return cmd.ExecuteScalar() != null;
                }
            }
        }


        public bool ExistsUsername(string username, string excludeEmployeeId = null)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"
                SELECT 1
                FROM employee
                WHERE LOWER(TRIM(emp_username)) = LOWER(TRIM(@Username)) " +
                (excludeEmployeeId != null ? "AND emp_id <> @EmpID " : "") +
                "LIMIT 1;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    if (excludeEmployeeId != null) cmd.Parameters.AddWithValue("@EmpID", excludeEmployeeId);
                    conn.Open();
                    return cmd.ExecuteScalar() != null;
                }
            }
        }

        public bool ExistsIDCard(string idCard, string excludeEmployeeId = null)
        {
            if (string.IsNullOrWhiteSpace(idCard)) return false;
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"
                SELECT 1
                FROM employee
                WHERE emp_identification = @IDCard " +
                (excludeEmployeeId != null ? "AND emp_id <> @EmpID " : "") +
                "LIMIT 1;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IDCard", idCard);
                    if (excludeEmployeeId != null) cmd.Parameters.AddWithValue("@EmpID", excludeEmployeeId);
                    conn.Open();
                    return cmd.ExecuteScalar() != null;
                }
            }
        }

        private bool EmpIdExists(string employeeID)
        {
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand("SELECT 1 FROM employee WHERE emp_id = @EmployeeID LIMIT 1;", conn))
            {
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                conn.Open();
                return cmd.ExecuteScalar() != null;
            }
        }

        private string GenerateEmployeeID(string role)
        {
            string prefix;
            switch (role)
            {
                case "Admin": prefix = "A"; break;
                case "Projectmanager":
                case "Sitesupervisor": prefix = "E"; break;
                case "Accountant": prefix = "F"; break;
                default: prefix = "X"; break;
            }

            string year2 = (DateTime.Now.Year % 100).ToString("D2");
            var rnd = new Random();
            string newID;
            do
            {
                string randoms = rnd.Next(0, 1000).ToString("D3"); // 000-999
                newID = prefix + year2 + randoms;
            } while (EmpIdExists(newID));
            return newID;
        }

        // ====== CRUD ======
        public bool InsertEmployee(Employee emp)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                string employeeID = GenerateEmployeeID(emp.Role);
                const string sql = @"
                INSERT INTO employee
                (emp_id, emp_username, emp_password, emp_name, emp_lname, emp_tel, emp_address, emp_email, emp_identification, emp_pos)
                VALUES
                (@EmployeeID, @Username, @Password, @FirstName, @LastName, @Phone, @Address, @Email, @IDCard, @Role);";
                using (var cmd = new MySqlCommand(sql, conn))
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
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateEmployee(Employee emp)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                const string sql = @"
                UPDATE employee
                SET
                    emp_name           = @FirstName,
                    emp_lname          = @LastName,
                    emp_username       = @Username,
                    emp_password       = @Password,
                    emp_tel            = @Phone,
                    emp_email          = @Email,
                    emp_pos            = @Role,
                    emp_identification = @IDCard,
                    emp_address        = @Address
                WHERE emp_id = @EmployeeID;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", emp.EmployeeID);
                    cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", emp.LastName);
                    cmd.Parameters.AddWithValue("@Username", emp.Username);
                    cmd.Parameters.AddWithValue("@Password", emp.Password);
                    cmd.Parameters.AddWithValue("@Phone", emp.Phone);
                    cmd.Parameters.AddWithValue("@Email", emp.Email);
                    cmd.Parameters.AddWithValue("@Role", emp.Role);
                    cmd.Parameters.AddWithValue("@IDCard", emp.IDCard);
                    cmd.Parameters.AddWithValue("@Address", emp.Address);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteEmployee(string employeeID, out string errorMessage)
        {
            errorMessage = "";

            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // ตรวจว่ามีข้อมูลลูกที่เกี่ยวข้องอยู่ไหม (project_employee_supervision)
                    string checkChildSql = "SELECT COUNT(*) FROM project_employee_supervision WHERE emp_id = @EmpID;";
                    using (var checkCmd = new MySqlCommand(checkChildSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@EmpID", employeeID);
                        long count = Convert.ToInt64(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            errorMessage = "ไม่สามารถลบพนักงานได้ เนื่องจากยังมีข้อมูลเชื่อมโยงในระบบโครงการ (project_employee_supervision)";
                            return false;
                        }
                    }

                    // ลบข้อมูลพนักงาน
                    string deleteSql = "DELETE FROM employee WHERE emp_id = @EmpID;";
                    using (var cmd = new MySqlCommand(deleteSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmpID", employeeID);
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                            return true;
                        else
                        {
                            errorMessage = "ไม่พบข้อมูลพนักงานที่ต้องการลบ";
                            return false;
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    if (ex.Message.Contains("foreign key constraint fails"))
                        errorMessage = "ไม่สามารถลบพนักงานได้ เนื่องจากยังมีข้อมูลที่เชื่อมโยงอยู่ในระบบ";
                    else
                        errorMessage = "เกิดข้อผิดพลาดจากฐานข้อมูล: " + ex.Message;

                    return false;
                }
                catch (Exception ex)
                {
                    errorMessage = "เกิดข้อผิดพลาด: " + ex.Message;
                    return false;
                }
            }
        }


        public DataTable GetEmployeeByIDtoUMNGT(string employeeID)
        {
            var dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                const string sql = @"
                SELECT
                    emp_id,
                    emp_name            AS 'ชื่อ',
                    emp_lname           AS 'นามสกุล',
                    emp_username        AS 'ชื่อผู้ใช้',
                    emp_tel             AS 'เบอร์โทร',
                    emp_email           AS 'อีเมล',
                    emp_pos             AS 'ตำแหน่ง',
                    emp_identification  AS 'เลขบัตรประชาชน',
                    emp_address         AS 'ที่อยู่',
                    emp_password        AS 'รหัสผ่าน'
                FROM employee
                WHERE emp_id = @EmployeeID;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    conn.Open();
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable GetAllEmployees()
        {
            var dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    const string sql = @"
                    SELECT
                        emp_id       AS 'รหัสพนักงาน',
                        emp_username AS 'ชื่อผู้ใช้',
                        emp_name     AS 'ชื่อ',
                        emp_lname    AS 'นามสกุล',
                        emp_identification AS 'เลขบัตรประชาชน',
                        emp_tel      AS 'เบอร์โทร',  
                        emp_email    AS 'อีเมล',
                        emp_pos      AS 'ตำแหน่ง',
                        emp_address  AS 'ที่อยู่'
                        
                    FROM employee;";
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

        public string GetEmployeeFullNameById(string empId)
        {
            string fullName = "";
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand("SELECT emp_name, emp_lname FROM employee WHERE emp_id = @EmpID;", conn))
            {
                cmd.Parameters.AddWithValue("@EmpID", empId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        fullName = $"{reader["emp_name"]} {reader["emp_lname"]}";
                }
            }
            return fullName;
        }

        // (ยังคงไว้ถ้าต้องการ)
        public bool CheckDuplicateEmployee(string employeeID, string username, string email, string idCard)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                const string sql = @"
                SELECT COUNT(*) FROM employee
                    WHERE emp_id = @EmployeeID
                    OR emp_username = @Username
                    OR emp_email = @Email
                    OR emp_identification = @IDCard;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@IDCard", idCard);
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }
    }
}
