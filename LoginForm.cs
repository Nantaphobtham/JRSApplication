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

            txtPassword.PasswordChar = '*';
            txtPassword.KeyDown += TxtPassword_KeyDown;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            PerformLogin();
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
                MessageBox.Show("กรุณากรอกชื่อผู้ใช้และรหัสผ่าน",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString =
                    ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT emp_id, emp_password, emp_name, emp_lname, emp_pos
                        FROM employee
                        WHERE emp_username = @Username";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hashedPassword = reader["emp_password"].ToString();
                                string fullName = $"{reader["emp_name"]} {reader["emp_lname"]}";
                                string role = reader["emp_pos"].ToString();
                                string empId = reader["emp_id"].ToString();

                                if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                                {
                                    MessageBox.Show(
                                        $"ยินดีต้อนรับเข้าสู่ระบบ {fullName} ตำแหน่ง : {role}",
                                        "เข้าสู่ระบบสำเร็จ",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                                    NavigateToDashboard(role, fullName, empId);
                                }
                                else
                                {
                                    MessageBox.Show(
                                        "ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้องกรุณาลองใหม่อีกครั้ง",
                                        "เข้าสู่ระบบล้มเหลว",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                                    txtUsername.Clear();
                                    txtPassword.Clear();
                                    txtUsername.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show(
                                    "ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้องกรุณาลองใหม่อีกครั้ง",
                                    "เข้าสู่ระบบล้มเหลว",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"เกิดข้อผิดพลาด: {ex.Message}",
                    "เข้าสู่ระบบล้มเหลว",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void NavigateToDashboard(string role, string fullName, string empId)
        {
            Form dashboard;

            switch (role)
            {
                case "Admin":
                    dashboard = new AdminForm(fullName, role);
                    break;

                case "Projectmanager":
                    dashboard = new ProjectManagerForm(fullName, role, empId);
                    break;

                case "Sitesupervisor":
                    dashboard = new SiteSupervisorForm(fullName, role, empId);
                    break;

                case "Accountant":
                    dashboard = new AccountantForm(fullName, role, empId);
                    break;

                default:
                    throw new InvalidOperationException("ตำแหน่งงานไม่ถูกต้อง");
            }

            dashboard.Show();
            this.Hide();
        }
    }
}
