using System;
using System.Configuration;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace JRSApplication
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            // ✅ ตั้งค่า Password ให้เป็น ***
            txtPassword.PasswordChar = '*';

            // ✅ ตั้งค่าการทำงานของปุ่ม Enter
            txtPassword.KeyDown += TxtPassword_KeyDown;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            PerformLogin(); // ✅ เรียกฟังก์ชันตรวจสอบการเข้าสู่ระบบ
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformLogin();
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void PerformLogin()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("กรุณากรอกชื่อผู้ใช้และรหัสผ่าน", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // ✅ ใช้ Parameterized Query เพื่อป้องกัน SQL Injection
                    string query = @"
                        SELECT emp_name, emp_lname, emp_pos 
                        FROM employee 
                        WHERE emp_username = @Username 
                        AND emp_password = @Password";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string fullName = $"{reader["emp_name"]} {reader["emp_lname"]}";
                                string role = reader["emp_pos"].ToString();

                                MessageBox.Show($"เข้าสู่ระบบสำเร็จ! ตำแหน่ง: {role}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // ✅ ส่งค่าชื่อและตำแหน่งไป AdminForm
                                NavigateToDashboard(role, fullName);
                            }
                            else
                            {
                                MessageBox.Show("ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NavigateToDashboard(string role, string fullName)
        {
            Form dashboard;

            switch (role)
            {
                case "Admin":
                    dashboard = new AdminForm(fullName, role); // ✅ ส่งค่าชื่อและตำแหน่งไป AdminForm
                    break;
                case "Projectmanager":
                    dashboard = new ProjectManagerForm();
                    break;
                case "Sitesupervisor":
                    dashboard = new SiteSupervisorForm();
                    break;
                case "Accountant":
                    dashboard = new AccountantForm();
                    break;
                default:
                    throw new InvalidOperationException("ตำแหน่งงานไม่ถูกต้อง");
            }

            dashboard.Show();
            this.Hide(); // ✅ ซ่อนหน้า Login
        }
    }
}
