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
    public partial class SupplierRegistration : UserControl
    {
        public SupplierRegistration()
        {
            InitializeComponent();
            CustomizeDataGridView(); // ✅ ปรับแต่ง DataGridView
            LoadSupplierData(); // ✅ โหลดข้อมูลเมื่อฟอร์มเปิด
            searchboxSuppiler.SetRoleAndFunction("Admin", "ทะเบียนลูกค้า");
            searchboxSuppiler.SearchTriggered += searchboxSuppiler_SearchTriggered;
        }
        private void searchboxSuppiler_SearchTriggered(object sender, SearchEventArgs e)
        {
            LoadSupplierData(e.SearchBy, e.Keyword); // โหลดข้อมูลที่ค้นหา
        }

        private void LoadSupplierData(string searchBy = "", string keyword = "")
        {
            SupplierDAL dal = new SupplierDAL();
            DataTable dt;

            // ถ้ามีเงื่อนไขการค้นหา
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(keyword))
            {
                dt = dal.SearchSuppliers(searchBy, keyword); // ค้นหาตามเงื่อนไข
            }
            else
            {
                dt = dal.GetAllSuppliers(); // โหลดข้อมูลทั้งหมด
            }

            dtgvSupplier.DataSource = dt; // แสดงข้อมูลใน DataGridView
        }



        private void CustomizeDataGridView()
        {
            dtgvSupplier.BorderStyle = BorderStyle.None;
            dtgvSupplier.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvSupplier.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvSupplier.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvSupplier.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvSupplier.BackgroundColor = Color.White;

            dtgvSupplier.EnableHeadersVisualStyles = false;
            dtgvSupplier.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvSupplier.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvSupplier.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvSupplier.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvSupplier.ColumnHeadersHeight = 30;

            dtgvSupplier.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvSupplier.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvSupplier.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvSupplier.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            dtgvSupplier.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvSupplier.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvSupplier.RowTemplate.Height = 30;

            dtgvSupplier.GridColor = Color.LightGray;
            dtgvSupplier.RowHeadersVisible = false;

            dtgvSupplier.ReadOnly = true;
            dtgvSupplier.AllowUserToAddRows = false;
            dtgvSupplier.AllowUserToResizeRows = false;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            //บันทึก
            // ✅ 1️⃣ ดึงค่าจากฟอร์ม
            string name = txtName.Text.Trim();
            string juristic = txtJuristic.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtAddress.Text.Trim();

            // ✅ 2️⃣ ตรวจสอบข้อมูล (Validation)
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(juristic) ||
                string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ 3️⃣ ตรวจสอบว่า Email ซ้ำหรือไม่
            SupplierDAL dal = new SupplierDAL();
            if (dal.CheckDuplicateSupplier(email))
            {
                MessageBox.Show("อีเมลนี้ถูกใช้งานแล้ว!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ 4️⃣ สร้าง `Supplier` Object
            Supplier sup = new Supplier
            {
                Name = name,
                Juristic = juristic,
                Phone = phone,
                Address = address,
                Email = email
            };

            // ✅ 5️⃣ บันทึกลงฐานข้อมูล
            bool success = dal.InsertSupplier(sup);

            // ✅ 6️⃣ แสดงผลลัพธ์
            if (success)
            {
                MessageBox.Show("บันทึกข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSupplierData(); // ✅ โหลดข้อมูลใหม่หลังบันทึก
                ClearForm(); // ✅ ล้างข้อมูลฟอร์ม
                ReadOnlyControls(); // ✅ ปิดการแก้ไข
            }
            else
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //เพื่ม
            ReadOnlyControls();
            EnableControls();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //แก้ไข
        }
        private void btDelete_Click(object sender, EventArgs e)
        {
            //ลบ
        }

        private void ReadOnlyControls()
        {
            txtName.ReadOnly = false;
            txtJuristic.ReadOnly = false;
            txtPhone.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtAddress.ReadOnly = false;
        }
        private void EnableControls()
        {
            txtName.Enabled = true;
            txtJuristic.Enabled = true;
            txtPhone.Enabled = true;
            txtEmail.Enabled = true;
            txtAddress.Enabled = true;
        }
        private void ClearForm()
        {
            txtName.Clear();
            txtJuristic.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
        }
    }
}
