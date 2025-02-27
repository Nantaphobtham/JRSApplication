using JRSApplication.Components;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;

namespace JRSApplication
{
    public partial class UserManagementForm : UserControl
    {
        public UserManagementForm()
        {
            InitializeComponent();
            CustomizeDataGridView();
            LoadEmployeeData();

        }
        //ตาราง
        private void LoadEmployeeData()
        {
            EmployeeDAL dal = new EmployeeDAL();
            DataTable dt = dal.GetAllEmployees(); // ✅ ดึงข้อมูลจาก MySQL
            dtgvEmployee.DataSource = dt; // ✅ แสดงข้อมูลใน DataGridView
        }
        private void CustomizeDataGridView()
        {
            // ✅ ตั้งค่าพื้นฐาน
            dtgvEmployee.BorderStyle = BorderStyle.None;
            dtgvEmployee.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray; // แถวเว้นแถวสีเทา
            dtgvEmployee.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvEmployee.DefaultCellStyle.SelectionBackColor = Color.DarkBlue; // สีพื้นหลังของแถวที่เลือก
            dtgvEmployee.DefaultCellStyle.SelectionForeColor = Color.White; // สีตัวอักษรของแถวที่เลือก
            dtgvEmployee.BackgroundColor = Color.White;

            // ✅ ตั้งค่าหัวตาราง (Header)
            dtgvEmployee.EnableHeadersVisualStyles = false;
            dtgvEmployee.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvEmployee.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvEmployee.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvEmployee.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            dtgvEmployee.ColumnHeadersHeight = 30; // ความสูงของแถวหัวตาราง

            // ✅ ตั้งค่าแถวข้อมูล
            dtgvEmployee.DefaultCellStyle.Font = new Font("Segoe UI", 15);
            dtgvEmployee.DefaultCellStyle.Padding = new Padding(5); // ระยะห่างภายในเซลล์
            dtgvEmployee.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // ปรับขนาดคอลัมน์อัตโนมัติ
            dtgvEmployee.RowTemplate.Height = 30; // ความสูงของแถวข้อมูล

            // ✅ ซ่อนเส้นตารางแนวตั้งเพื่อให้ดูสะอาดตา
            dtgvEmployee.GridColor = Color.LightGray;
            dtgvEmployee.RowHeadersVisible = false; // ซ่อนแถวหมายเลขด้านซ้าย

            // ✅ ปิดการแก้ไขข้อมูลโดยตรง
            dtgvEmployee.ReadOnly = true;
            dtgvEmployee.AllowUserToAddRows = false;
            dtgvEmployee.AllowUserToResizeRows = false;
        }




        private void btnAdd_Click(object sender, EventArgs e)
        {
            EnableControls();
            ReadOnlyControls();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1️⃣ ดึงค่าจากฟอร์ม
            string role = cmbRole.SelectedItem?.ToString();
            string firstName = txtName.Text.Trim();
            string lastName = txtLastname.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtAddress.Text.Trim();
            string idCard = txtIdcard.Text.Trim();

            // 2️⃣ ตรวจสอบข้อมูล (Validation)
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3️⃣ ตรวจสอบรหัสผ่านตรงกัน
            if (password != confirmPassword)
            {
                MessageBox.Show("รหัสผ่านไม่ตรงกัน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4️⃣ ตรวจสอบว่า Username หรือ Email มีอยู่แล้วหรือไม่
            EmployeeDAL dal = new EmployeeDAL();
            if (dal.CheckDuplicateEmployee("", username, email, idCard))
            {
                MessageBox.Show("Username หรือ Email นี้ถูกใช้งานแล้ว", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 5️ เข้ารหัสรหัสผ่านก่อนบันทึก (ใช้ Hash)
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // 6️ สร้าง Employee Object
            Employee emp = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                IDCard = idCard,
                Phone = phone,
                Email = email,
                Address = address,
                Username = username,
                Password = hashedPassword,  // 🔥 บันทึกเป็นรหัสผ่านที่เข้ารหัสแล้ว
                Role = role
            };

            // 7️⃣ บันทึกลงฐานข้อมูล
            bool success = dal.InsertEmployee(emp);

            // 8️⃣ แสดงผลลัพธ์
            if (success)
            {
                MessageBox.Show("บันทึกข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm(); // ✅ ล้างข้อมูลฟอร์ม
            }
            else
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            //แก้ไข
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //ลบ
        }

        private void ReadOnlyControls()
        {
            txtName.ReadOnly = false;
            txtLastname.ReadOnly = false;
            txtIdcard.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtPhone.ReadOnly = false;
            txtAddress.ReadOnly = false;
            txtUsername.ReadOnly = false;
            txtPassword.ReadOnly = false;
            txtConfirmPassword.ReadOnly = false;
            cmbRole.SelectedIndex = 0;
        }
        private void EnableControls()
        {
            //btnAdd.Enabled = false;  ปิดปุ่มเพื่อป้องกันการกดซ้ำ

            txtName.Enabled = true;
            txtName.Text = string.Empty;    

            txtLastname.Enabled = true;
            txtLastname.Text = string.Empty;

            txtIdcard.Enabled = true;
            txtIdcard.Text = string.Empty;

            txtAddress.Enabled = true;
            txtAddress.Text = string.Empty;

            txtEmail.Enabled = true;
            txtEmail.Text = string.Empty;

            txtPhone.Enabled = true;
            txtPhone.Text = string.Empty;

            txtUsername.Enabled = true;
            txtUsername.Text = string.Empty;

            txtPassword.Enabled = true;
            txtPassword.Text = string.Empty;

            txtConfirmPassword.Enabled = true;
            txtConfirmPassword.Text = string.Empty;

            cmbRole.Enabled = true;
        }
        private void ClearForm()
        {
            txtName.Clear();
            txtLastname.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtIdcard.Clear();
            txtAddress.Clear();
            cmbRole.SelectedIndex = 0;
        }

    }
}
