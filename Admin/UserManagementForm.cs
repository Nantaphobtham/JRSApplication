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
        private bool isEditMode = false; // ✅ เช็คว่ากำลังแก้ไขหรือไม่
        private string selectedEmployeeID = ""; // ✅ เก็บรหัสพนักงานที่ถูกเลือก

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
            dtgvEmployee.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray; // ✅ แถวเว้นแถวสีเทา
            dtgvEmployee.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvEmployee.DefaultCellStyle.SelectionBackColor = Color.DarkBlue; // ✅ สีพื้นหลังของแถวที่เลือก
            dtgvEmployee.DefaultCellStyle.SelectionForeColor = Color.White; // ✅ สีตัวอักษรของแถวที่เลือก
            dtgvEmployee.BackgroundColor = Color.White;

            // ✅ ตั้งค่าหัวตาราง (Header)
            dtgvEmployee.EnableHeadersVisualStyles = false;
            dtgvEmployee.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvEmployee.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvEmployee.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvEmployee.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            dtgvEmployee.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // ✅ จัดหัวตารางให้อยู่กึ่งกลาง
            dtgvEmployee.ColumnHeadersHeight = 30; // ✅ ความสูงของหัวตาราง

            // ✅ ตั้งค่าแถวข้อมูล
            dtgvEmployee.DefaultCellStyle.Font = new Font("Segoe UI", 15);
            dtgvEmployee.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // ✅ จัดข้อมูลให้อยู่กึ่งกลาง
            dtgvEmployee.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3); // ✅ ปรับ Padding

            // ✅ ปรับขนาดคอลัมน์และแถว
            dtgvEmployee.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // ✅ ปรับให้คอลัมน์ขยายเต็ม
            dtgvEmployee.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // ✅ ปรับขนาดแถวอัตโนมัติ
            dtgvEmployee.RowTemplate.Height = 30; // ✅ กำหนดความสูงของแถวให้เหมาะสม

            // ✅ ซ่อนเส้นตารางแนวตั้ง
            dtgvEmployee.GridColor = Color.LightGray;
            dtgvEmployee.RowHeadersVisible = false; // ✅ ซ่อนหมายเลขแถว

            // ✅ ปิดการแก้ไขข้อมูลโดยตรง
            dtgvEmployee.ReadOnly = true;
            dtgvEmployee.AllowUserToAddRows = false;
            dtgvEmployee.AllowUserToResizeRows = false;
        }
        // ✅ ฟังก์ชันตรวจสอบค่าที่กรอกครบถ้วน
        private bool ValidateEmployeeData()
        {
            bool hasError = false;

            if (string.IsNullOrWhiteSpace(txtName.Text)) { starName.Visible = true; hasError = true; }
            else { starName.Visible = false; }

            if (string.IsNullOrWhiteSpace(txtLastname.Text)) { starLastname.Visible = true; hasError = true; }
            else { starLastname.Visible = false; }

            if (string.IsNullOrWhiteSpace(txtUsername.Text)) { starUsername.Visible = true; hasError = true; }
            else { starUsername.Visible = false; }

            if (string.IsNullOrWhiteSpace(txtPassword.Text)) { starPassword.Visible = true; hasError = true; }
            else { starPassword.Visible = false; }

            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text)) { starConfirmPassword.Visible = true; hasError = true; }
            else { starConfirmPassword.Visible = false; }

            if (string.IsNullOrWhiteSpace(txtPhone.Text)) { starPhone.Visible = true; hasError = true; }
            else { starPhone.Visible = false; }

            if (string.IsNullOrWhiteSpace(txtEmail.Text)) { starEmail.Visible = true; hasError = true; }
            else { starEmail.Visible = false; }

            if (cmbRole.SelectedIndex == -1) { starRole.Visible = true; hasError = true; }
            else { starRole.Visible = false; }

            return !hasError;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EnableControlsOn();
            ReadOnlyControlsOn();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateEmployeeData()) return;

            EmployeeDAL dal = new EmployeeDAL();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text);

            Employee emp = new Employee
            {
                FirstName = txtName.Text.Trim(),
                LastName = txtLastname.Text.Trim(),
                Username = txtUsername.Text.Trim(),
                Password = hashedPassword,
                Phone = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Role = cmbRole.SelectedItem.ToString()
            };

            bool success;
            if (isEditMode)
            {
                emp.EmployeeID = selectedEmployeeID;
                success = dal.UpdateEmployee(emp);
            }
            else
            {
                success = dal.InsertEmployee(emp);
            }

            if (success)
            {
                MessageBox.Show("บันทึกข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadEmployeeData();
                ClearForm();
            }
            else
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedEmployeeID))
            {
                MessageBox.Show("กรุณาเลือกพนักงานก่อนลบ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบข้อมูลนี้?", "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            EmployeeDAL dal = new EmployeeDAL();
            bool success = dal.DeleteEmployee(selectedEmployeeID);

            if (success)
            {
                MessageBox.Show("ลบข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadEmployeeData();
                ClearForm();
            }
            else
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการลบข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ เก็บค่าดั้งเดิมของ Username, Email, และ ID Card เพื่อตรวจสอบการเปลี่ยนแปลง
        private string originalUsername = "";
        private string originalEmail = "";
        private string originalIDCard = "";

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //แก้ไข
            // ✅ ตรวจสอบว่ามีการเลือกพนักงานก่อนหรือไม่
            if (string.IsNullOrEmpty(selectedEmployeeID))
            {
                MessageBox.Show("กรุณาเลือกพนักงานที่ต้องการแก้ไข", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ เปลี่ยนเป็นโหมดแก้ไข
            isEditMode = true;

            // ✅ เปิดให้แก้ไขฟอร์ม
            EnableControlsOn();
            ReadOnlyControlsOn();

            // ✅ โหลดข้อมูลพนักงานที่เลือกมาใส่ในฟอร์ม
            EmployeeDAL dal = new EmployeeDAL();
            DataTable dt = dal.GetEmployeeByID(selectedEmployeeID);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                txtName.Text = row["ชื่อ"].ToString();
                txtLastname.Text = row["นามสกุล"].ToString();
                txtUsername.Text = row["ชื่อผู้ใช้"].ToString();
                txtPhone.Text = row["เบอร์โทร"].ToString();
                txtEmail.Text = row["อีเมล"].ToString();
                txtIdcard.Text = row["เลขบัตรประชาชน"].ToString();
                txtAddress.Text = row["ที่อยู่"].ToString();
                cmbRole.SelectedItem = row["ตำแหน่ง"].ToString();

                // ✅ เก็บค่า Username & Email เดิมไว้เพื่อตรวจสอบว่ามีการเปลี่ยนแปลงหรือไม่
                originalUsername = txtUsername.Text.Trim();
                originalEmail = txtEmail.Text.Trim();
                originalIDCard = txtIdcard.Text.Trim();
            }

            txtName.Focus(); // ✅ ให้โฟกัสไปที่ช่องชื่อ
        }

        //เปิด ปิด ล้าง ฟอร์ม
        private void ReadOnlyControlsOn()
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
        private void EnableControlsOn()
        {
            txtName.Enabled = true;
            txtLastname.Enabled = true;
            txtIdcard.Enabled = true;
            txtAddress.Enabled = true;
            txtEmail.Enabled = true;
            txtPhone.Enabled = true;
            txtUsername.Enabled = true;
            txtPassword.Enabled = true;
            txtConfirmPassword.Enabled = true;
            cmbRole.Enabled = true;
        }
        private void ReadOnlyControlsOff()
        {
            txtName.ReadOnly = true;
            txtLastname.ReadOnly = true;
            txtIdcard.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtPhone.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtUsername.ReadOnly = true;
            txtPassword.ReadOnly = true;
            txtConfirmPassword.ReadOnly = true;
            cmbRole.SelectedIndex = -1;
        }
        private void EnableControlsOff()
        {
            txtName.Enabled = false;
            txtLastname.Enabled = false;
            txtIdcard.Enabled = false;
            txtAddress.Enabled = false;
            txtEmail.Enabled = false;
            txtPhone.Enabled = false;
            txtUsername.Enabled = false;
            txtPassword.Enabled = false;
            txtConfirmPassword.Enabled = false;
            cmbRole.Enabled = false;
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
            cmbRole.SelectedIndex = -1;
        }

    }
}
