using JRSApplication.Components;
using JRSApplication.Data_Access_Layer;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net.Mail;
using MySql.Data.MySqlClient; // <- สำหรับ catch MySqlException

namespace JRSApplication
{
    public partial class CustomerRegistration : UserControl
    {
        private bool isEditMode = false;  // ตรวจสอบโหมด เพิ่ม/แก้ไข
        private string selectedCustomerID = "";  // เก็บรหัสลูกค้าที่เลือก
        private string originalEmail = "";
        private string originalPhone = "";   // เผื่อเก็บค่าเดิม
        private string originalAddress = ""; // เผื่อเก็บค่าเดิม
        private string originalName = "";    // เผื่อเก็บค่าเดิม
        private string originalIdCard = "";

        public CustomerRegistration()
        {
            InitializeComponent();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            // ถ้าต้องการให้เปลี่ยนหน้า/ปิดฟอร์มได้แม้ Validate ไม่ผ่าน ให้ใช้บรรทัดนี้
            // this.AutoValidate = AutoValidate.EnableAllowFocusChange;

            // ✅ ให้แน่ใจว่า CellClick ถูกผูกอีเวนต์
            dtgvCustomer.CellClick += dtgvCustomer_CellClick;

            // ===== Input filters/Validating (ตามสเปกเดียวกับตัวอย่าง) =====

            // --- ID Card: ตัวเลขล้วน 13 หลัก (ไม่ตรวจ checksum) ---
            txtIdcard.MaxLength = 13;
            txtIdcard.KeyPress += (s, e) =>
            {
                e.Handled = !(char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar));
            };

            // --- Phone: ยอม +66 หรือ 0 นำหน้า -> normalize เป็น 0XXXXXXXXX 10 หลัก ---
            txtPhone.Validating += (s, e) =>
            {
                // แค่ normalize เฉย ๆ ถ้าถูก; ถ้าไม่ถูกก็ปล่อยไว้ให้ไปเช็กตอนกดบันทึก
                string normalized = NormalizeThaiMobile10(txtPhone.Text);
                if (!string.IsNullOrEmpty(normalized))
                    txtPhone.Text = normalized;
            };


            // --- Email: ตรวจรูปแบบเข้ม + normalize โดเมนเป็นตัวพิมพ์เล็ก ---
            txtEmail.Validating += (s, e) =>
            {
                string email = (txtEmail.Text ?? "").Trim();
                // แค่ normalize ถ้ารูปแบบถูกต้อง ไม่ขึ้น error ที่นี่
                if (!string.IsNullOrEmpty(email) && IsValidEmailStrict(email))
                    txtEmail.Text = NormalizeEmail(email);
            };


            CustomizeDataGridView(); // ✅ ปรับแต่ง DataGridView
            LoadCustomerData();      // ✅ โหลดข้อมูลเมื่อฟอร์มเปิด

            searchboxCustomer.SetRoleAndFunction("Admin", "ทะเบียนลูกค้า");
            searchboxCustomer.SearchTriggered += searchboxCustomer_SearchTriggered;
        }

        private void searchboxCustomer_SearchTriggered(object sender, SearchEventArgs e)
        {
            LoadCustomerData(e.SearchBy, e.Keyword);
        }

        private void LoadCustomerData(string searchBy = "", string keyword = "")
        {
            CustomerDAL dal = new CustomerDAL();
            DataTable dt = (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(keyword))
                ? dal.SearchCustomer(searchBy, keyword)
                : dal.GetAllCustomers();

            dtgvCustomer.DataSource = dt;
        }



        private void CustomizeDataGridView()
        {
            // ✅ ตั้งค่าพื้นฐาน
            dtgvCustomer.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dtgvCustomer.BorderStyle = BorderStyle.None;
            dtgvCustomer.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray; // แถวเว้นแถวสีเทา
            dtgvCustomer.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvCustomer.DefaultCellStyle.SelectionBackColor = Color.DarkBlue; // สีพื้นหลังของแถวที่เลือก
            dtgvCustomer.DefaultCellStyle.SelectionForeColor = Color.White;    // สีตัวอักษรของแถวที่เลือก
            dtgvCustomer.BackgroundColor = Color.White;

            // ✅ ตั้งค่าหัวตาราง (Header)
            dtgvCustomer.EnableHeadersVisualStyles = false;
            dtgvCustomer.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvCustomer.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvCustomer.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvCustomer.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            dtgvCustomer.ColumnHeadersHeight = 30; // ความสูงของแถวหัวตาราง
            dtgvCustomer.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // ✅ ตั้งค่าแถวข้อมูล
            dtgvCustomer.DefaultCellStyle.Font = new Font("Segoe UI", 15);
            dtgvCustomer.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // ✅ จัดข้อความให้อยู่กึ่งกลาง
            dtgvCustomer.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3); // ✅ ปรับ Padding

            // ✅ ปรับขนาดคอลัมน์และแถว
            dtgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // ✅ ปรับให้คอลัมน์ขยายเต็ม
            dtgvCustomer.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;   // ✅ ปรับขนาดแถวอัตโนมัติ
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

                // ✅ เก็บค่าเดิมสำหรับเช็คซ้ำตอนแก้ไข
                originalEmail = (txtEmail.Text ?? "").Trim();
                originalIdCard = (txtIdcard.Text ?? "").Trim();
            }
            EnableControlsOff();
            ReadOnlyControlsOff();
            btnEdit.Text = "แก้ไข";
            isEditMode = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CustomerDAL dal = new CustomerDAL();

            // ✅ เช็คทีละช่องและแจ้งเตือนทันที (รวม normalize)
            if (!CheckRequiredFieldsSequential()) return;

            // ✅ ตรวจสอบข้อมูลที่จำเป็น (หลัง normalize แล้ว)
            string firstName = (txtName.Text ?? "").Trim();
            string lastName = (txtLastname.Text ?? "").Trim();
            string idCard = (txtIdcard.Text ?? "").Trim(); // ตอนนี้เป็นตัวเลข 13 หลัก
            string phone = (txtPhone.Text ?? "").Trim();  // ตอนนี้รูปแบบ 0XXXXXXXXX 10 หลัก
            string email = NormalizeEmail((txtEmail.Text ?? "").Trim());
            string address = (txtAddress.Text ?? "").Trim();

            Customer cus = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                IDCard = idCard,
                Phone = phone,
                Address = address,
                Email = email
            };

            bool success = false;

            try
            {
                if (isEditMode)
                {
                    if (email != originalEmail || idCard != originalIdCard)
                    {
                        if (dal.CheckDuplicateCustomer(selectedCustomerID, email, idCard))
                        {
                            MessageBox.Show("อีเมลหรือเลขบัตรประชาชนนี้มีอยู่แล้ว!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    if (int.TryParse(selectedCustomerID, out int customerId))
                    {
                        cus.CustomerID = customerId;
                        success = dal.UpdateCustomer(cus);
                    }
                    else
                    {
                        MessageBox.Show("รหัสลูกค้าไม่ถูกต้อง!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    if (dal.CheckDuplicateCustomer("", email, idCard))
                    {
                        MessageBox.Show("อีเมลหรือเลขบัตรประชาชนนี้มีอยู่แล้ว!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    success = dal.InsertCustomer(cus);
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
            {
                string msg = "ข้อมูลซ้ำในระบบ!";
                if (ex.Message.Contains("cus_email")) { msg = "อีเมลนี้มีอยู่แล้วในระบบ!"; txtEmail.Focus(); txtEmail.SelectAll(); }
                else if (ex.Message.Contains("cus_id_card")) { msg = "เลขบัตรประชาชนนี้มีอยู่แล้วในระบบ!"; txtIdcard.Focus(); txtIdcard.SelectAll(); }
                else if (ex.Message.Contains("PRIMARY") || ex.Message.Contains("cus_id")) { msg = "รหัสลูกค้าซ้ำในระบบ!"; }

                MessageBox.Show(msg, "ข้อมูลซ้ำ!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด!: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
                btnEdit.Text = "แก้ไข";
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
            ClearForm();
            isEditMode = false;
            selectedCustomerID = "";
            btnEdit.Text = "แก้ไข";
            txtName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedCustomerID))
            {
                MessageBox.Show("กรุณาเลือกลูกค้าก่อนแก้ไข!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!isEditMode)
            {
                // เข้าสู่โหมดแก้ไข
                isEditMode = true;
                ReadOnlyControlsOn();
                EnableControlsOn();
                txtName.Focus();

                btnEdit.Text = "ยกเลิกแก้ไข"; // 🟢
            }
            else
            {
                // ออกจากโหมดแก้ไข
                isEditMode = false;
                ReadOnlyControlsOff();
                EnableControlsOff();          // ← ปิดการแก้ไขให้เหมือนตอนแรก
                btnEdit.Text = "แก้ไข";       // 🔵
            }
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
            int customerId = Convert.ToInt32(selectedCustomerID);

            // ตรวจสอบว่ามีการอ้างอิงอยู่หรือไม่
            if (dal.IsCustomerReferenced(customerId))
            {
                MessageBox.Show("ไม่สามารถลบลูกค้าได้ เพราะยังมีโครงการที่เกี่ยวข้องอยู่", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ดำเนินการลบถ้าไม่มีการอ้างอิง
            bool success = dal.DeleteCustomer(customerId);

            if (success)
            {
                MessageBox.Show("ลบข้อมูลสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomerData();
                ClearForm();
                selectedCustomerID = "";
                isEditMode = false;
                ReadOnlyControlsOff();
                EnableControlsOff();
                btnEdit.Text = "แก้ไข";
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

            starName.Visible = string.IsNullOrWhiteSpace(txtName.Text);
            starLastname.Visible = string.IsNullOrEmpty(txtLastname.Text);
            starIdcard.Visible = string.IsNullOrEmpty(txtIdcard.Text);
            starPhone.Visible = string.IsNullOrWhiteSpace(txtPhone.Text);
            starEmail.Visible = string.IsNullOrWhiteSpace(txtEmail.Text);
            starAddress.Visible = string.IsNullOrWhiteSpace(txtAddress.Text);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            starName.Visible = string.IsNullOrWhiteSpace(txtName.Text);
        }
        private void txtLastname_TextChanged(object sender, EventArgs e)
        {
            starLastname.Visible = string.IsNullOrEmpty(txtLastname.Text);
        }
        private void txtIdcard_TextChanged(object sender, EventArgs e)
        {
            starIdcard.Visible = string.IsNullOrEmpty(txtIdcard.Text);
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

        // ===== Validation & Helpers =====

        // แจ้งเตือน + โฟกัส + โชว์ดอกจัน ของช่องที่ยังไม่กรอก/ไม่ถูกต้อง
        private bool AlertField(string message, TextBox tb, Label star)
        {
            if (star != null) star.Visible = true;
            MessageBox.Show(message, "แจ้งเตือน!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (tb != null) { tb.Focus(); tb.SelectAll(); }
            return false;
        }

        // เช็คทีละช่องตามลำดับ และหยุดทันทีเมื่อพบข้อผิดพลาด
        private bool CheckRequiredFieldsSequential()
        {
            // ชื่อ
            if (string.IsNullOrWhiteSpace((txtName.Text ?? "").Trim()))
                return AlertField("กรุณากรอกชื่อ!", txtName, starName);

            // นามสกุล
            if (string.IsNullOrWhiteSpace((txtLastname.Text ?? "").Trim()))
                return AlertField("กรุณากรอกนามสกุล!", txtLastname, starLastname);

            // เลขบัตรประชาชน: ต้องเป็นตัวเลข 13 หลัก
            var idCard = Regex.Replace((txtIdcard.Text ?? "").Trim(), @"\D", "");
            if (string.IsNullOrWhiteSpace(idCard))
                return AlertField("กรุณากรอกเลขบัตรประชาชน!", txtIdcard, starIdcard);
            if (idCard.Length != 13)
                return AlertField("เลขบัตรประชาชนต้องเป็นตัวเลข 13 หลัก!", txtIdcard, starIdcard);
            txtIdcard.Text = idCard; // normalize

            // เบอร์โทร: ต้อง normalize ได้เป็น 0XXXXXXXXX (10 หลัก)
            var phoneRaw = (txtPhone.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(phoneRaw))
                return AlertField("กรุณากรอกเบอร์โทร!", txtPhone, starPhone);
            var phoneNorm = NormalizeThaiMobile10(phoneRaw);
            if (string.IsNullOrEmpty(phoneNorm))
                return AlertField("เบอร์โทรศัพท์ต้องเป็นตัวเลข 10 หลัก!", txtPhone, starPhone);
            txtPhone.Text = phoneNorm;

            // อีเมล
            var emailRaw = (txtEmail.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(emailRaw))
                return AlertField("กรุณากรอกอีเมล!", txtEmail, starEmail);
            if (!IsValidEmailStrict(emailRaw))
                return AlertField("รูปแบบอีเมลไม่ถูกต้อง!", txtEmail, starEmail);
            txtEmail.Text = NormalizeEmail(emailRaw);

            // ที่อยู่
            if (string.IsNullOrWhiteSpace((txtAddress.Text ?? "").Trim()))
                return AlertField("กรุณากรอกที่อยู่!", txtAddress, starAddress);

            // ผ่านทุกช่อง -> ซ่อนดอกจัน
            starName.Visible = starLastname.Visible = starIdcard.Visible =
                starPhone.Visible = starEmail.Visible = starAddress.Visible = false;

            return true;
        }

        // รับเบอร์มือถือไทยในรูปแบบต่าง ๆ แล้วคืนเป็น 0XXXXXXXXX (10 หลัก) เท่านั้น
        // ตัวอย่างที่ยอมรับได้:
        //   0812345678
        //   0 81-234-5678
        //   +66812345678
        //   66812345678
        //   0066812345678
        private string NormalizeThaiMobile10(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return string.Empty;

            // เอาเฉพาะตัวเลขออกมา
            string digits = new string(raw.Where(char.IsDigit).ToArray());

            // กรณีเบอร์ภายในประเทศปกติ 0XXXXXXXXX (10 หลัก)
            if (Regex.IsMatch(digits, @"^0\d{9}$"))
                return digits;

            // กรณีมีรหัสประเทศ 0066 นำหน้า
            if (digits.StartsWith("0066"))
            {
                digits = digits.Substring(4); // ตัด 0066 ออก
            }
            else if (digits.StartsWith("66"))
            {
                digits = digits.Substring(2); // ตัด 66 ออก
            }

            // หลังตัดรหัสประเทศแล้ว ต้องเหลือ 9 หลัก (ไม่ขึ้นต้นด้วย 0) -> เติม 0 ข้างหน้า
            if (Regex.IsMatch(digits, @"^[1-9]\d{8}$"))
                return "0" + digits;      // 9 หลัก -> 0XXXXXXXXX

            // รูปแบบอื่นถือว่าไม่ถูกต้อง
            return string.Empty;
        }

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
                var a = MailAddress(email);
                return string.Equals(a.Address, email, StringComparison.OrdinalIgnoreCase);
            }
            catch { return false; }
        }

        private MailAddress MailAddress(string email) => new MailAddress(email);

        private string NormalizeEmail(string email)
        {
            email = (email ?? "").Trim();
            int at = email.LastIndexOf('@');
            if (at <= 0 || at == email.Length - 1) return email;
            string local = email.Substring(0, at);
            string domain = email.Substring(at + 1).ToLowerInvariant();
            return $"{local}@{domain}";
        }
    }
}
