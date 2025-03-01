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
        private bool isEditMode = false; // เช็คว่ากำลังแก้ไขหรือไม่
        private string selectedSupplierID = ""; // เก็บรหัสซัพพลายเออร์ที่ถูกเลือก

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

        private void dtgvSupplier_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // เช็คว่าแถวที่คลิกไม่ใช่ Header และเป็นแถวที่มีข้อมูล
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvSupplier.Rows[e.RowIndex];

                // ✅ เก็บค่า ID ของซัพพลายเออร์ที่ถูกเลือก
                selectedSupplierID = row.Cells["รหัสซัพพลายเออร์"].Value?.ToString();

                // ดึงข้อมูลจากแต่ละคอลัมน์ของ DataGridView และใส่ลงใน TextBox
                txtName.Text = row.Cells["ชื่อบริษัท"].Value?.ToString();
                txtJuristic.Text = row.Cells["นิติบุคคล"].Value?.ToString();
                txtPhone.Text = row.Cells["เบอร์โทรศัพท์"].Value?.ToString();
                txtEmail.Text = row.Cells["อีเมล"].Value?.ToString();
                txtAddress.Text = row.Cells["ที่อยู่"].Value?.ToString();

                // ✅ เก็บค่า Email เดิมที่เลือก
                originalEmail = txtEmail.Text.Trim();
                originalPhone = txtPhone.Text.Trim();
                originalJuristic = txtJuristic.Text.Trim();
                originalName = txtName.Text.Trim();
            }
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

        // ✅ เก็บค่า Email เดิมก่อนแก้ไข
        private string originalEmail = ""; 
        private string originalPhone = "";
        private string originalName = "";
        private string originalJuristic = "";

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private bool IsValidPhoneNumber(string phone)
        {
            return phone.All(char.IsDigit) && phone.Length >= 9 && phone.Length <= 15;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // ✅ 1️⃣ ดึงค่าจากฟอร์ม
            string name = txtName.Text.Trim();
            string juristic = txtJuristic.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtAddress.Text.Trim();

            // ✅ 2️⃣ ตรวจสอบข้อมูล (Validation)
            bool hasError = false;

            if (string.IsNullOrWhiteSpace(name))
            {
                starCompanyName.Visible = true;
                hasError = true;
            }
            else
            {
                starCompanyName.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(juristic))
            {
                starIdCompany.Visible = true;
                hasError = true;
            }
            else
            {
                starIdCompany.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(phone) || !IsValidPhoneNumber(phone))
            {
                starPhone.Visible = true;
                hasError = true;
            }
            else
            {
                starPhone.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                starEmail.Visible = true;
                hasError = true;
            }
            else
            {
                starEmail.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                starAddress.Visible = true;
                hasError = true;
            }
            else
            {
                starAddress.Visible = false;
            }

            // ✅ หยุดทำงานหากมีช่องที่ยังไม่ได้กรอก
            if (hasError)
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            SupplierDAL dal = new SupplierDAL();

            if (isEditMode) // ✅ 3️⃣ ถ้าเป็นโหมดแก้ไข ให้ทำ Update
            {
                if (email != originalEmail || phone != originalPhone || juristic != originalJuristic || name != originalName)
                {
                    // ✅ ตรวจสอบว่าข้อมูลซ้ำหรือไม่ (ยกเว้นตัวเอง)
                    if (dal.CheckDuplicateSupplier(email, phone, juristic, name, selectedSupplierID))
                    {
                        MessageBox.Show("ข้อมูลที่กรอกมีอยู่แล้วในระบบ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                Supplier sup = new Supplier
                {
                    SupplierID = selectedSupplierID,
                    Name = name,
                    Juristic = juristic,
                    Phone = phone,
                    Address = address,
                    Email = email
                };

                bool success = dal.UpdateSupplier(sup.SupplierID, sup.Name, sup.Juristic, sup.Phone, sup.Email, sup.Address);
                if (success)
                {
                    MessageBox.Show("อัปเดตข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ ล้างค่าที่ใช้เปรียบเทียบข้อมูลเก่า
                    originalEmail = email;
                    originalPhone = phone;
                    originalJuristic = juristic;
                    originalName = name;

                    LoadSupplierData();
                    ClearForm();
                    ReadOnlyControls_close();
                    EnableControls_close();
                    isEditMode = false;
                    selectedSupplierID = "";
                }
                else
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการอัปเดตข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // ✅ 4️⃣ ถ้าไม่ใช่โหมดแก้ไข แสดงว่าเป็นการเพิ่มข้อมูลใหม่
            {
                if (dal.CheckDuplicateSupplier(email, phone, juristic, name, ""))
                {
                    MessageBox.Show("ข้อมูลที่กรอกมีอยู่แล้วในระบบ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Supplier sup = new Supplier
                {
                    Name = name,
                    Juristic = juristic,
                    Phone = phone,
                    Address = address,
                    Email = email
                };

                bool success = dal.InsertSupplier(sup);
                if (success)
                {
                    MessageBox.Show("บันทึกข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            LoadSupplierData();
            ClearForm();
            ReadOnlyControls_close();
            EnableControls_close();
            isEditMode = false;
            selectedSupplierID = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //เพื่ม
            ReadOnlyControls_Open();
            EnableControls_Open();
            ClearForm();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //แก้ไข
            if (string.IsNullOrEmpty(selectedSupplierID))
            {
                MessageBox.Show("กรุณาเลือกซัพพลายเออร์ก่อนแก้ไข", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            isEditMode = true; // ✅ ตั้งค่าให้เป็นโหมดแก้ไข
            EnableControls_Open(); // ✅ เปิดให้แก้ไข TextBox
            ReadOnlyControls_Open(); // ✅ เปิดให้แก้ไข TextBox
            txtName.Focus(); // ✅ ให้โฟกัสไปที่ชื่อบริษัท
        }
        private void btDelete_Click(object sender, EventArgs e)
        {
            //ลบ
            // ✅ ตรวจสอบว่ามีการเลือกซัพพลายเออร์หรือไม่
            if (string.IsNullOrEmpty(selectedSupplierID))
            {
                MessageBox.Show("กรุณาเลือกซัพพลายเออร์ก่อนลบ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ ยืนยันก่อนลบ
            DialogResult result = MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบข้อมูลนี้?", "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
            {
                return;
            }

            // ✅ เรียกใช้ฟังก์ชันลบจาก DAL
            SupplierDAL dal = new SupplierDAL();
            bool success = dal.DeleteSupplier(selectedSupplierID);

            if (success)
            {
                MessageBox.Show("ลบข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // ✅ โหลดข้อมูลใหม่หลังลบ
                LoadSupplierData();
                ClearForm();

                // ✅ รีเซ็ตค่าตัวแปรที่ใช้เก็บข้อมูล
                selectedSupplierID = "";
            }
            else
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการลบข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReadOnlyControls_Open()
        {
            txtName.ReadOnly = false;
            txtJuristic.ReadOnly = false;
            txtPhone.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtAddress.ReadOnly = false;
        }
        private void EnableControls_Open()
        {
            txtName.Enabled = true;
            txtJuristic.Enabled = true;
            txtPhone.Enabled = true;
            txtEmail.Enabled = true;
            txtAddress.Enabled = true;
        }
        private void ReadOnlyControls_close()
        {
            txtName.ReadOnly = true;
            txtJuristic.ReadOnly = true;
            txtPhone.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtAddress.ReadOnly = true;
        }
        private void EnableControls_close()
        {
            txtName.Enabled = false;
            txtJuristic.Enabled = false;
            txtPhone.Enabled = false;
            txtEmail.Enabled = false;
            txtAddress.Enabled = false;
        }
        private void ClearForm()
        {
            txtName.Clear();
            txtJuristic.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            starCompanyName.Visible = string.IsNullOrWhiteSpace(txtName.Text);
        }

        private void txtJuristic_TextChanged(object sender, EventArgs e)
        {
            starIdCompany.Visible = string.IsNullOrWhiteSpace(txtJuristic.Text);
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            starPhone.Visible = string.IsNullOrWhiteSpace(txtPhone.Text);
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            starEmail.Visible = string.IsNullOrWhiteSpace(txtEmail.Text);
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            starAddress.Visible = string.IsNullOrWhiteSpace(txtAddress.Text);
        }
    }
}
