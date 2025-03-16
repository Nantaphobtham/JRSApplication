using JRSApplication.Components;
using JRSApplication.Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class CustomerRegistration : UserControl
    {
        private bool isEditMode = false;  // ตรวจสอบโหมด เพิ่ม/แก้ไข
        private string selectedCustomerID = "";  // เก็บรหัสลูกค้าที่เลือก

        public CustomerRegistration()
        {
            InitializeComponent();
            CustomizeDataGridView(); // ✅ ปรับแต่ง DataGridView
            LoadCustomerData(); // ✅ โหลดข้อมูลเมื่อฟอร์มเปิด
        }


        private void LoadCustomerData()
        {
            CustomerDAL dal = new CustomerDAL();
            DataTable dt = dal.GetAllCustomers(); // ✅ ดึงข้อมูลจาก MySQL
            dtgvCustomer.DataSource = dt; // ✅ แสดงข้อมูลใน DataGridView
        }

        private void CustomizeDataGridView()
        {
            // ✅ ตั้งค่าพื้นฐาน
            dtgvCustomer.BorderStyle = BorderStyle.None;
            dtgvCustomer.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray; // แถวเว้นแถวสีเทา
            dtgvCustomer.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvCustomer.DefaultCellStyle.SelectionBackColor = Color.DarkBlue; // สีพื้นหลังของแถวที่เลือก
            dtgvCustomer.DefaultCellStyle.SelectionForeColor = Color.White; // สีตัวอักษรของแถวที่เลือก
            dtgvCustomer.BackgroundColor = Color.White;

            // ✅ ตั้งค่าหัวตาราง (Header)
            dtgvCustomer.EnableHeadersVisualStyles = false;
            dtgvCustomer.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvCustomer.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvCustomer.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvCustomer.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            dtgvCustomer.ColumnHeadersHeight = 30; // ความสูงของแถวหัวตาราง

            // ✅ ตั้งค่าแถวข้อมูล
            dtgvCustomer.DefaultCellStyle.Font = new Font("Segoe UI", 15);
            dtgvCustomer.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // ✅ จัดข้อความให้อยู่กึ่งกลาง
            dtgvCustomer.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // ✅ จัดหัวตารางให้อยู่กึ่งกลาง
            dtgvCustomer.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3); // ✅ ปรับ Padding

            // ✅ ปรับขนาดคอลัมน์และแถว
            dtgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // ✅ ปรับให้คอลัมน์ขยายเต็ม
            dtgvCustomer.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // ✅ ปรับขนาดแถวอัตโนมัติ
            dtgvCustomer.RowTemplate.Height = 30; // ✅ กำหนดความสูงของแถวให้เหมาะสม

            // ✅ ซ่อนเส้นตารางแนวตั้ง
            dtgvCustomer.GridColor = Color.LightGray;
            dtgvCustomer.RowHeadersVisible = false; // ✅ ซ่อนหมายเลขแถว

            // ✅ ปิดการแก้ไขข้อมูลโดยตรง
            dtgvCustomer.ReadOnly = true;
            dtgvCustomer.AllowUserToAddRows = false;
            dtgvCustomer.AllowUserToResizeRows = false;
        }
        private void dtgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvCustomer.Rows[e.RowIndex];

                selectedCustomerID = row.Cells["รหัสลูกค้า"].Value?.ToString();
                txtName.Text = row.Cells["ชื่อ"].Value?.ToString();
                txtLastname.Text = row.Cells["นามสกุล"].Value?.ToString();
                txtIdcard.Text = row.Cells["เลขบัตรประชาชน"].Value?.ToString();
                txtPhone.Text = row.Cells["เบอร์โทร"].Value?.ToString();
                txtEmail.Text = row.Cells["อีเมล"].Value?.ToString();
                txtAddress.Text = row.Cells["ที่อยู่"].Value?.ToString();
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            CustomerDAL dal = new CustomerDAL();
            string firstName = txtName.Text.Trim();
            string lastName = txtLastname.Text.Trim();
            string idCard = txtIdcard.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtAddress.Text.Trim();

            // ✅ ตรวจสอบว่ากรอกข้อมูลครบหรือไม่
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(idCard) || string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ ตรวจสอบอีเมล และ เลขบัตรประชาชนซ้ำ
            if (dal.CheckDuplicateCustomer(selectedCustomerID, email, idCard))
            {
                MessageBox.Show("อีเมลหรือเลขบัตรประชาชนนี้มีอยู่แล้ว!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Customer cus = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                IDCard = idCard,
                Phone = phone,
                Address = address,
                Email = email
            };

            bool success;
            if (isEditMode)
            {
                // ✅ ถ้าเป็นโหมดแก้ไข ให้ Update
                cus.CustomerID = int.Parse(selectedCustomerID);
                success = dal.UpdateCustomer(cus);
            }
            else
            {
                // ✅ ถ้าเป็นโหมดเพิ่มข้อมูลใหม่
                success = dal.InsertCustomer(cus);
            }

            if (success)
            {
                MessageBox.Show("บันทึกข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomerData();
                ClearForm();
                ReadOnlyControlsOff();
                EnableControlsOff();
                isEditMode = false;
                selectedCustomerID = "";
            }
            else
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //เพิ่ม
            ReadOnlyControlsOn();
            EnableControlsOn();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //แก้ไข
            if (string.IsNullOrEmpty(selectedCustomerID))
            {
                MessageBox.Show("กรุณาเลือกลูกค้าก่อนแก้ไข", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isEditMode = true; // ✅ เข้าโหมดแก้ไข
            ReadOnlyControlsOn(); // ✅ เปิดให้แก้ไข
            EnableControlsOn();
            txtName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //ลบ
            if (string.IsNullOrEmpty(selectedCustomerID))
            {
                MessageBox.Show("กรุณาเลือกลูกค้าก่อนลบ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบลูกค้านี้?", "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
            {
                return;
            }

            CustomerDAL dal = new CustomerDAL();
            bool success = dal.DeleteCustomer(Convert.ToInt32(selectedCustomerID));

            if (success)
            {
                MessageBox.Show("ลบข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomerData();
                ClearForm();
                selectedCustomerID = "";
            }
            else
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการลบข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //เปิด ปิด ล้าง ฟอร์ม
        private void ReadOnlyControlsOn()
        {
            txtName.ReadOnly = false;
            txtLastname.ReadOnly = false;
            txtIdcard.ReadOnly = false;
            txtPhone.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtAddress.ReadOnly = false;
        }
        private void EnableControlsOn()
        {
            txtName.Enabled = true;
            txtLastname.Enabled = true;
            txtIdcard.Enabled = true;
            txtPhone.Enabled = true;
            txtEmail.Enabled = true;
            txtAddress.Enabled = true;
        }
        private void ReadOnlyControlsOff()
        {
            txtName.ReadOnly = true;
            txtLastname.ReadOnly = true;
            txtIdcard.ReadOnly = true;
            txtPhone.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtAddress.ReadOnly = true;
        }
        private void EnableControlsOff()
        {
            txtName.Enabled = false;
            txtLastname.Enabled = false;
            txtIdcard.Enabled = false;
            txtPhone.Enabled = false;
            txtEmail.Enabled = false;
            txtAddress.Enabled = false;
        }
        private void ClearForm()
        {
            txtName.Clear();
            txtLastname.Clear();
            txtIdcard.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
        }


    }
}
