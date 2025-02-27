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


        private void btnSave_Click(object sender, EventArgs e)
        {
            CustomerDAL dal = new CustomerDAL(); // ✅ ประกาศตัวแปรก่อนใช้
            //บันทึก
            // ✅ 1️⃣ ดึงค่าจากฟอร์ม
            string customerID = dal.GenerateCustomerID();
            string firstName = txtName.Text.Trim();
            string lastName = txtLastname.Text.Trim();
            string idCard = txtIdcard.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string email = txtEmail.Text.Trim();

            // ✅ 2️⃣ ตรวจสอบข้อมูล (Validation)
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(idCard) || string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(email))
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ 3️⃣ ตรวจสอบว่า Email หรือ ID Card ซ้ำหรือไม่
            if (dal.CheckDuplicateCustomer(customerID, email, idCard))
            {
                MessageBox.Show("เลขบัตรประชาชน หรืออีเมลนี้ถูกใช้งานแล้ว!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ 4️⃣ สร้าง `Customer` Object
            Customer cus = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                IDCard = idCard,
                Phone = phone,
                Address = address,
                Email = email
            };

            // ✅ 5️⃣ บันทึกลงฐานข้อมูล
            bool success = dal.InsertCustomer(cus);

            // ✅ 6️⃣ แสดงผลลัพธ์
            if (success)
            {
                MessageBox.Show("บันทึกข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
               // LoadCustomerData(); // ✅ โหลดข้อมูลใหม่หลังบันทึก
               // ReadOnlyControls(); // ✅ ปิดการแก้ไข
               ClearForm();
            }
            else
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //เพิ่ม
            ReadOnlyControls();
            EnableControls();
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
            txtPhone.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtAddress.ReadOnly = false;
        }
        private void EnableControls()
        {
            txtName.Enabled = true;
            txtLastname.Enabled = true;
            txtIdcard.Enabled = true;
            txtPhone.Enabled = true;
            txtEmail.Enabled = true;
            txtAddress.Enabled = true;
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
