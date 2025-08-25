using JRSApplication.Components;
using JRSApplication.Data_Access_Layer;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class SupplierRegistration : UserControl
    {
        private bool isEditMode = false;           // โหมดแก้ไข?
        private string selectedSupplierID = "";    // ID ที่เลือกใน grid

        // เก็บค่าเดิมไว้เทียบตอนแก้ไข (ป้องกันตรวจซ้ำถ้าไม่ได้แก้จริง)
        private string originalEmail = "";
        private string originalPhone = "";
        private string originalName = "";
        private string originalJuristic = "";

        // ErrorProvider สำหรับแสดงข้อผิดพลาดที่ช่องอินพุต
        private readonly ErrorProvider ep = new ErrorProvider();

        public SupplierRegistration()
        {
            InitializeComponent();

            // ===== Validation behavior =====
            // ให้คอนโทรลในฟอร์มรัน Validating เวลาย้ายโฟกัส/กดปุ่ม
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;

            // ปุ่มบันทึกให้ทริกเกอร์ validating ของทุกช่อง
            // (ต้องมีปุ่มชื่อ btnSave ในฟอร์ม)
            btnSave.CausesValidation = true;

            // ===== Input filters =====
            // นิติบุคคล/เลขผู้เสียภาษี: ตัวเลขล้วน 13 หลัก
            txtJuristic.MaxLength = 13;
            txtJuristic.KeyPress += (s, e) =>
            {
                e.Handled = !(char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar));
            };

            // โทรศัพท์: ยอมให้กดเฉพาะตัวเลข/สัญลักษณ์ทั่วไป และ normalize ตอน validate
            txtPhone.MaxLength = 12;
            txtPhone.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && "+-() ".IndexOf(e.KeyChar) < 0)
                    e.Handled = true;
            };

            // อีเมล: เมื่อเลิกโฟกัส normalize โดเมนให้เป็นตัวพิมพ์เล็ก (ถ้ารูปแบบถูก)
            txtEmail.Validating += (s, e) =>
            {
                var email = (txtEmail.Text ?? "").Trim();
                if (!string.IsNullOrEmpty(email) && IsValidEmailStrict(email))
                    txtEmail.Text = NormalizeEmail(email);
            };

            // ErrorProvider
            ep.ContainerControl = this;
            ep.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // grid + data
            CustomizeDataGridView();
            dtgvSupplier.CellClick += dtgvSupplier_CellClick;

            LoadSupplierData();

            // search box (คัสตอมของคุณ)
            searchboxSuppiler.SetRoleAndFunction("Admin", "ทะเบียนลูกค้า");
            searchboxSuppiler.SearchTriggered += searchboxSuppiler_SearchTriggered;

            // ปิดการแก้ไขเริ่มต้น
            ReadOnlyControls_close();
            EnableControls_close();
        }

        // ===== Search =====
        private void searchboxSuppiler_SearchTriggered(object sender, SearchEventArgs e)
        {
            LoadSupplierData(e.SearchBy, e.Keyword);
        }

        private void LoadSupplierData(string searchBy = "", string keyword = "")
        {
            SupplierDAL dal = new SupplierDAL();
            DataTable dt = (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(keyword))
                ? dal.SearchSuppliers(searchBy, keyword)
                : dal.GetAllSuppliers();

            dtgvSupplier.DataSource = dt;
        }

        // ===== Grid click -> fill form =====
        private void dtgvSupplier_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // header

            DataGridViewRow row = dtgvSupplier.Rows[e.RowIndex];

            selectedSupplierID = row.Cells["รหัสซัพพลายเออร์"].Value?.ToString();

            txtName.Text = row.Cells["ชื่อบริษัท"].Value?.ToString();
            txtJuristic.Text = row.Cells["นิติบุคคล"].Value?.ToString();
            txtPhone.Text = row.Cells["เบอร์โทรศัพท์"].Value?.ToString();
            txtEmail.Text = row.Cells["อีเมล"].Value?.ToString();
            txtAddress.Text = row.Cells["ที่อยู่"].Value?.ToString();

            // เก็บค่าเดิมไว้เทียบตอนแก้ไข
            originalEmail = txtEmail.Text.Trim();
            originalPhone = txtPhone.Text.Trim();
            originalJuristic = txtJuristic.Text.Trim();
            originalName = txtName.Text.Trim();

            // เข้าโหมดดูข้อมูล
            isEditMode = false;
            ReadOnlyControls_close();
            EnableControls_close();
            ClearErrors();
        }

        // ===== UI: Grid style =====
        private void CustomizeDataGridView()
        {
            dtgvSupplier.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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
            dtgvSupplier.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            dtgvSupplier.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvSupplier.ColumnHeadersHeight = 30;

            dtgvSupplier.DefaultCellStyle.Font = new Font("Segoe UI", 15);
            dtgvSupplier.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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

        // ===== Buttons =====
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // โหมดเพิ่ม
            isEditMode = false;
            selectedSupplierID = "";

            ClearForm();
            ClearErrors();

            ReadOnlyControls_Open();
            EnableControls_Open();
            txtName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedSupplierID))
            {
                MessageBox.Show("กรุณาเลือกซัพพลายเออร์ก่อนแก้ไข", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isEditMode = true;
            ClearErrors();
            ReadOnlyControls_Open();
            EnableControls_Open();
            txtName.Focus();
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedSupplierID))
            {
                MessageBox.Show("กรุณาเลือกซัพพลายเออร์ก่อนลบ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบข้อมูลนี้?", "ยืนยันการลบ",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            SupplierDAL dal = new SupplierDAL();
            bool success = dal.DeleteSupplier(selectedSupplierID);

            if (success)
            {
                MessageBox.Show("ลบข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSupplierData();
                ClearForm();
                ClearErrors();
                selectedSupplierID = "";
                isEditMode = false;
                ReadOnlyControls_close();
                EnableControls_close();
            }
            else
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการลบข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1) รัน Validating ของทุก control (รวม txtEmail.Validating)
            if (!this.ValidateChildren(ValidationConstraints.Enabled))
                return;

            // 2) ตรวจความถูกต้องรวม + Normalize
            if (!ValidateSupplierData()) return;

            // 3) ค่าที่ผ่านการ Normalize แล้ว
            string name = txtName.Text.Trim();
            string juristic = txtJuristic.Text.Trim(); // 13 digits
            string phone = txtPhone.Text.Trim();    // 0XXXXXXXXX
            string email = txtEmail.Text.Trim();    // normalized domain
            string address = txtAddress.Text.Trim();

            SupplierDAL dal = new SupplierDAL();

            if (isEditMode)
            {
                // ถ้ามีการแก้ไขฟิลด์สำคัญ ค่อยเช็คซ้ำ
                if (email != originalEmail || phone != originalPhone ||
                    juristic != originalJuristic || name != originalName)
                {
                    if (dal.CheckDuplicateSupplier(email, phone, juristic, name, selectedSupplierID))
                    {
                        MessageBox.Show("ข้อมูลที่กรอกมีอยู่แล้วในระบบ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                bool ok = dal.UpdateSupplier(selectedSupplierID, name, juristic, phone, email, address);
                if (ok)
                {
                    MessageBox.Show("อัปเดตข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadSupplierData();
                    ClearForm();
                    ClearErrors();
                    ReadOnlyControls_close();
                    EnableControls_close();
                    isEditMode = false;
                    selectedSupplierID = "";

                    // อัปเดตค่าเดิม
                    originalEmail = email;
                    originalPhone = phone;
                    originalJuristic = juristic;
                    originalName = name;
                }
                else
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการอัปเดตข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // โหมดเพิ่ม: เช็คซ้ำทั้งหมด
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
                    Email = email,
                    Address = address
                };

                bool ok = dal.InsertSupplier(sup);
                if (ok)
                {
                    MessageBox.Show("บันทึกข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSupplierData();
                    ClearForm();
                    ClearErrors();
                    ReadOnlyControls_close();
                    EnableControls_close();
                    isEditMode = false;
                    selectedSupplierID = "";
                }
                else
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ===== UI enable/disable helpers =====
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

            // อัปเดตสตาร์ให้ถูกต้อง (เริ่มต้นเป็นต้องกรอก)
            starCompanyName.Visible = true;
            starIdCompany.Visible = true;
            starPhone.Visible = true;
            starEmail.Visible = true;
            starAddress.Visible = true;
        }
        private void ClearErrors()
        {
            ep.SetError(txtName, "");
            ep.SetError(txtJuristic, "");
            ep.SetError(txtPhone, "");
            ep.SetError(txtEmail, "");
            ep.SetError(txtAddress, "");
        }

        // ===== Live star indicators (optional UI) =====
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            starCompanyName.Visible = string.IsNullOrWhiteSpace(txtName.Text);
        }
        private void txtJuristic_TextChanged(object sender, EventArgs e)
        {
            string digits = Regex.Replace(txtJuristic.Text ?? "", @"\D", "");
            starIdCompany.Visible = string.IsNullOrWhiteSpace(txtJuristic.Text) || digits.Length != 13;
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

        // ===== Validation แบบเจาะจงทีละช่อง =====
        private bool ValidateSupplierData()
        {
            // Trim ก่อน
            txtName.Text = (txtName.Text ?? "").Trim();
            txtJuristic.Text = (txtJuristic.Text ?? "").Trim();
            txtPhone.Text = (txtPhone.Text ?? "").Trim();
            txtEmail.Text = (txtEmail.Text ?? "").Trim();
            txtAddress.Text = (txtAddress.Text ?? "").Trim();

            // 1) ชื่อบริษัท
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                ep.SetError(txtName, "กรอกชื่อบริษัท");
                starCompanyName.Visible = true;
                return ShowFieldError(txtName, "กรุณากรอกชื่อบริษัท");
            }
            ep.SetError(txtName, "");
            starCompanyName.Visible = false;

            // 2) เลขนิติบุคคล/เลขผู้เสียภาษี: ตัวเลข 13 หลัก
            string juristicDigits = Regex.Replace(txtJuristic.Text, @"\D", "");
            if (juristicDigits.Length != 13)
            {
                ep.SetError(txtJuristic, "เลขนิติบุคคล/ภาษี ต้องมี 13 หลัก");
                starIdCompany.Visible = true;
                return ShowFieldError(txtJuristic, "กรุณากรอกเลขนิติบุคคล/ผู้เสียภาษีให้ถูกต้อง (13 หลัก)");
            }
            txtJuristic.Text = juristicDigits; // normalize
            ep.SetError(txtJuristic, "");
            starIdCompany.Visible = false;

            // 3) โทรศัพท์ไทย: รองรับ 0XXXXXXXXX หรือ +66XXXXXXXXX -> normalize เป็น 0XXXXXXXXX
            string normalizedPhone = NormalizeThaiMobile10(txtPhone.Text);
            if (string.IsNullOrEmpty(normalizedPhone))
            {
                ep.SetError(txtPhone, "เบอร์มือถือไทย 10 หลัก");
                starPhone.Visible = true;
                return ShowFieldError(txtPhone, "กรุณากรอกเบอร์โทรให้ถูกต้อง เช่น 0812345678 หรือ +66812345678");
            }
            txtPhone.Text = normalizedPhone;
            ep.SetError(txtPhone, "");
            starPhone.Visible = false;

            // 4) อีเมล
            if (!IsValidEmailStrict(txtEmail.Text))
            {
                ep.SetError(txtEmail, "อีเมลไม่ถูกต้อง");
                starEmail.Visible = true;
                return ShowFieldError(txtEmail, "กรุณากรอกอีเมลให้ถูกต้อง เช่น name@example.com");
            }
            txtEmail.Text = NormalizeEmail(txtEmail.Text); // normalize โดเมนเป็นตัวพิมพ์เล็ก
            ep.SetError(txtEmail, "");
            starEmail.Visible = false;

            // 5) ที่อยู่
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                ep.SetError(txtAddress, "กรอกที่อยู่");
                starAddress.Visible = true;
                return ShowFieldError(txtAddress, "กรุณากรอกที่อยู่");
            }
            ep.SetError(txtAddress, "");
            starAddress.Visible = false;

            return true; // ผ่านหมด
        }

        // โชว์ข้อความผิดเฉพาะช่อง + โฟกัส + เลือกข้อความให้แก้ทันที
        private bool ShowFieldError(TextBox tb, string message)
        {
            MessageBox.Show(message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (tb != null)
            {
                tb.Focus();
                tb.SelectAll();
                tb.BringToFront();
            }
            return false;
        }


        // ===== Helpers: Email =====
        private bool IsValidEmailStrict(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            email = email.Trim();
            if (email.Length > 254) return false;
            if (email.Contains(" ") || email.Contains("..")) return false;

            const string pattern =
                @"^(?!\.)[A-Za-z0-9!#$%&'*+/=?^_`{|}~.-]{1,64}@" +
                @"(?!-)(?:[A-Za-z0-9-]{1,63}\.)+[A-Za-z]{2,63}$";
            if (!Regex.IsMatch(email, pattern)) return false;

            try
            {
                var a = new System.Net.Mail.MailAddress(email);
                return string.Equals(a.Address, email, StringComparison.OrdinalIgnoreCase);
            }
            catch { return false; }
        }

        private string NormalizeEmail(string email)
        {
            email = (email ?? "").Trim();
            int at = email.LastIndexOf('@');
            if (at <= 0 || at == email.Length - 1) return email;
            string local = email.Substring(0, at);
            string domain = email.Substring(at + 1).ToLowerInvariant();
            return $"{local}@{domain}";
        }

        // ===== Helpers: Phone (ไทย) =====
        // ยอมรับ 0XXXXXXXXX (10 หลัก) หรือ +66XXXXXXXXX แล้ว Normalize -> 0XXXXXXXXX
        private string NormalizeThaiMobile10(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";
            string digits = Regex.Replace(raw, @"\D", "");
            if (digits.StartsWith("66"))
            {
                string after = digits.Substring(2);
                if (after.Length == 9) return "0" + after;                     // +66 + 9 หลัก -> 0XXXXXXXXX
                if (after.Length == 10 && after.StartsWith("0")) return after; // เผื่อ +660xxxxxxxx
                return "";
            }
            return Regex.IsMatch(digits, @"^0\d{9}$") ? digits : "";
        }
    }
}
