using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace JRSApplication.Components.Service
{
    public partial class ChangePassword1 : UserControl
    {
        private readonly string empId;
        private readonly string fullName;
        private readonly string role;

        public ChangePassword1(string empId, string fullName, string role)
        {
            InitializeComponent();
            this.empId = empId;
            this.fullName = fullName;
            this.role = role;

            // Set the UseSystemPasswordChar property on load
            txtOldPassword.UseSystemPasswordChar = true;
            txtNewPassword.UseSystemPasswordChar = true;
        }
        // This event runs when the control is loaded onto the form
        private void uc_ChangePassword_Load(object sender, EventArgs e)
        {
            // Populate the textboxes with the user's information
            txtUsername.Text = this.fullName;
            txtPosition.Text = this.role;

            // Make these fields read-only as the user should not change them here
            txtUsername.ReadOnly = true;
            txtPosition.ReadOnly = true;
        }

        // Event for the "Show Old Password" checkbox
        private void chkShowOldPassword_CheckedChanged(object sender, EventArgs e)
        {
            // If checked, show the password. If not, hide it.
            if (chkShowOldPassword.Checked)
            {
                txtOldPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtOldPassword.UseSystemPasswordChar = true;
            }
        }

        // Event for the "Show New Password" checkbox
        private void chkShowNewPassword_CheckedChanged(object sender, EventArgs e)
        {
            // If checked, show the password. If not, hide it.
            if (chkShowNewPassword.Checked)
            {
                txtNewPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtNewPassword.UseSystemPasswordChar = true;
            }
        }

        // Event for your "Save" or "Change Password" button
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;

            // Basic validation to ensure fields are not empty
            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("กรุณากรอกรหัสผ่านเก่าและรหัสผ่านใหม่", "ข้อมูลไม่ครบถ้วน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Step 1: Get the current hashed password from the database for this user
                    string currentHashedPassword = "";
                    string selectQuery = "SELECT emp_password FROM employee WHERE emp_id = @emp_id";

                    using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@emp_id", this.empId);

                        // ExecuteScalar is efficient for getting a single value
                        var result = selectCmd.ExecuteScalar();
                        if (result != null)
                        {
                            currentHashedPassword = result.ToString();
                        }
                    }

                    if (string.IsNullOrEmpty(currentHashedPassword))
                    {
                        MessageBox.Show("ไม่พบข้อมูลผู้ใช้งานในระบบ", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Step 2: Verify if the entered old password matches the stored hash
                    bool isOldPasswordCorrect = BCrypt.Net.BCrypt.Verify(oldPassword, currentHashedPassword);

                    if (isOldPasswordCorrect)
                    {
                        // Step 3: If correct, hash the new password and update the database
                        string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

                        string updateQuery = "UPDATE employee SET emp_password = @NewPassword WHERE emp_id = @emp_id";
                        using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@NewPassword", newHashedPassword);
                            updateCmd.Parameters.AddWithValue("@emp_id", this.empId);
                            updateCmd.ExecuteNonQuery();

                            MessageBox.Show("เปลี่ยนรหัสผ่านสำเร็จ!", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtOldPassword.Clear();
                            txtNewPassword.Clear();
                        }
                    }
                    else
                    {
                        // If the old password was incorrect
                        MessageBox.Show("รหัสผ่านเก่าไม่ถูกต้อง กรุณาลองอีกครั้ง", "ยืนยันตัวตนผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการเชื่อมต่อฐานข้อมูล: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}

