using JRSApplication.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace JRSApplication
{
    public partial class UserManagementForm : UserControl
    {
        private bool isEditMode = false;
        private string selectedEmployeeID = "";
        private string currentHashedPassword = "";
        private bool passwordEdited = false;

        private string originalUsername = "";
        private string originalEmail = "";
        private string originalIDCard = "";

        // ===== ค้นหา/กรองตาราง =====
        private DataTable employeeTable;
        private readonly BindingSource employeeBS = new BindingSource();

        public UserManagementForm()
        {
            InitializeComponent();

            // ===== Input filters =====
            txtIdcard.MaxLength = 13;
            txtIdcard.KeyPress += (s, e) =>
            {
                e.Handled = !(char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar));
            };
            txtIdcard.Validating += (s, e) =>
            {
                string digits = Regex.Replace(txtIdcard.Text ?? "", @"\D", "");
                if (digits.Length != 13)
                {
                    e.Cancel = true;
                    MessageBox.Show("เลขบัตรประชาชนต้องเป็นตัวเลข 13 หลัก!",
                        "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtIdcard.SelectAll();
                }
                else
                {
                    txtIdcard.Text = digits;
                }
            };

            txtPhone.MaxLength = 12;
            txtPhone.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && "+-() ".IndexOf(e.KeyChar) < 0)
                    e.Handled = true;
            };
            txtPhone.Validating += (s, e) =>
            {
                string normalized = NormalizeThaiMobile10(txtPhone.Text);
                if (string.IsNullOrEmpty(normalized))
                {
                    e.Cancel = true;
                    MessageBox.Show("เบอร์โทรต้องเป็นตัวเลข 10 หลักเท่านั้น\nตัวอย่าง: 0812345678 หรือ +66812345678",
                        "กรุณากรอกเบอร์โทรให้ถูกต้อง!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhone.SelectAll();
                }
                else
                {
                    txtPhone.Text = normalized;
                }
            };

            txtEmail.Validating += (s, e) =>
            {
                string email = (txtEmail.Text ?? "").Trim();
                if (!IsValidEmailStrict(email))
                {
                    e.Cancel = true;
                    MessageBox.Show("อีเมลไม่ถูกต้อง\nตัวอย่างที่ถูกต้อง: name@example.com",
                        "กรุณากรอกอีเมลให้ถูกต้อง!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.SelectAll();
                }
                else
                {
                    txtEmail.Text = NormalizeEmail(email);
                }
            };

            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;
            txtPassword.TextChanged += (s, e) => { if (isEditMode) passwordEdited = true; };
            txtConfirmPassword.TextChanged += (s, e) => { if (isEditMode) passwordEdited = true; };

            // grid + data
            CustomizeDataGridView();
            dtgvEmployee.CellClick += dtgvEmployee_CellClick;

            LoadEmployeeData();
            LoadRoles();

            // ===== ผูก SearchboxControl =====
            // (กันคอนโทรลยังไม่พร้อม: ย้ายไป this.Load)
            this.Load += (s, e) =>
            {
                // เซ็ตค่าเริ่มต้น (เผื่อคอนโทรลยังไม่ได้อัดเอง)
                searchboxUserData.SetRoleAndFunction("Admin", "จัดการบัญชีผู้ใช้");

                // เมื่อผู้ใช้เปลี่ยนบทบาท ให้สลับชุดตัวเลือกในกล่องค้นหา
                cmbRole.SelectedIndexChanged += (s2, e2) =>
                {
                    var role = cmbRole.SelectedItem?.ToString();
                    if (!string.IsNullOrWhiteSpace(role))
                        searchboxUserData.SetRoleAndFunction(role);
                };

                // รับอีเวนต์ค้นหาแล้วกรองตาราง
                searchboxUserData.SearchTriggered += OnSearchTriggered;
            };

            ReadOnlyControlsOff();
            EnableControlsOff();
        }

        private void LoadRoles()
        {
            cmbRole.Items.Clear();
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Projectmanager");
            cmbRole.Items.Add("Sitesupervisor");
            cmbRole.Items.Add("Accountant");
            cmbRole.SelectedIndex = -1;
        }

        private void LoadEmployeeData()
        {
            var dal = new EmployeeDAL();
            employeeTable = dal.GetAllEmployees();

            // คง filter เดิมไว้ถ้ามี
            string keep = employeeBS.Filter;

            employeeBS.DataSource = employeeTable;
            dtgvEmployee.DataSource = employeeBS;

            if (!string.IsNullOrEmpty(keep))
                employeeBS.Filter = keep;
        }

        private void dtgvEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dtgvEmployee.Rows[e.RowIndex];
            selectedEmployeeID = row.Cells["รหัสพนักงาน"].Value?.ToString();

            var dal = new EmployeeDAL();
            DataTable dt = dal.GetEmployeeByIDtoUMNGT(selectedEmployeeID);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("ไม่สามารถโหลดข้อมูลพนักงานได้!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow dr = dt.Rows[0];
            txtName.Text = dr["ชื่อ"].ToString();
            txtLastname.Text = dr["นามสกุล"].ToString();
            txtUsername.Text = dr["ชื่อผู้ใช้"].ToString();
            txtPhone.Text = dr["เบอร์โทร"].ToString();
            txtEmail.Text = dr["อีเมล"].ToString();
            txtIdcard.Text = dr["เลขบัตรประชาชน"].ToString();
            txtAddress.Text = dr["ที่อยู่"].ToString();

            string role = dr["ตำแหน่ง"].ToString().Trim();
            cmbRole.SelectedItem = cmbRole.Items.Contains(role) ? role : null;

            originalUsername = txtUsername.Text.Trim();
            originalEmail = txtEmail.Text.Trim();
            originalIDCard = txtIdcard.Text.Trim();

            ReadOnlyControlsOff();
            EnableControlsOff();
        }

        private void CustomizeDataGridView()
        {
            dtgvEmployee.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvEmployee.BorderStyle = BorderStyle.None;
            dtgvEmployee.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvEmployee.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvEmployee.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvEmployee.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvEmployee.BackgroundColor = Color.White;

            dtgvEmployee.EnableHeadersVisualStyles = false;
            dtgvEmployee.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvEmployee.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvEmployee.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvEmployee.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            dtgvEmployee.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvEmployee.ColumnHeadersHeight = 30;

            dtgvEmployee.DefaultCellStyle.Font = new Font("Segoe UI", 15);
            dtgvEmployee.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvEmployee.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            dtgvEmployee.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvEmployee.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvEmployee.RowTemplate.Height = 30;

            dtgvEmployee.GridColor = Color.LightGray;
            dtgvEmployee.RowHeadersVisible = false;
            dtgvEmployee.ReadOnly = true;
            dtgvEmployee.AllowUserToAddRows = false;
            dtgvEmployee.AllowUserToResizeRows = false;
        }

        // ===== Validation รวม =====
        private bool ValidateEmployeeData()
        {
            // required
            if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("กรุณากรอกชื่อก่อนบันทึก!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtLastname.Text)) { MessageBox.Show("กรุณากรอกนามสกุลก่อนบันทึก!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtIdcard.Text)) { MessageBox.Show("กรุณากรอกเลขบัตรประชาชนก่อนบันทึก!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtPhone.Text)) { MessageBox.Show("กรุณากรอกเบอร์โทรศัพท์ก่อนบันทึก!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtAddress.Text)) { MessageBox.Show("กรุณากรอกที่อยู่ก่อนบันทึก!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtEmail.Text)) { MessageBox.Show("กรุณากรอกอีเมลก่อนบันทึก!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtUsername.Text)) { MessageBox.Show("กรุณากรอกชื่อผู้ใช้ก่อนบันทึก!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (cmbRole.SelectedIndex == -1)
            {
                if (starRole != null) starRole.Visible = true;
                MessageBox.Show("กรุณาเลือกบทบาทก่อนบันทึก!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbRole.Focus(); cmbRole.DroppedDown = true; return false;
            }
            else if (starRole != null) starRole.Visible = false;

            if (!isEditMode)
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text)) { MessageBox.Show("กรุณากรอกรหัสผ่านก่อนบันทึก!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
                if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text)) { MessageBox.Show("ยืนยันรหัสผ่านก่อนบันทึก!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
                if (txtPassword.Text.Length < 8) { MessageBox.Show("รหัสผ่านอย่างน้อย 8 ตัวอักษร", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
                if (txtPassword.Text != txtConfirmPassword.Text) { MessageBox.Show("รหัสผ่านและยืนยันรหัสผ่านไม่ตรงกัน!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            }
            else
            {
                bool anyFilled = !string.IsNullOrWhiteSpace(txtPassword.Text) || !string.IsNullOrWhiteSpace(txtConfirmPassword.Text);
                if (anyFilled)
                {
                    if (string.IsNullOrWhiteSpace(txtPassword.Text)) { MessageBox.Show("กรอกรหัสผ่านใหม่", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
                    if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text)) { MessageBox.Show("ยืนยันรหัสผ่านใหม่", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
                    if (txtPassword.Text.Length < 8) { MessageBox.Show("รหัสผ่านอย่างน้อย 8 ตัวอักษร", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
                    if (txtPassword.Text != txtConfirmPassword.Text) { MessageBox.Show("รหัสผ่านใหม่และยืนยันไม่ตรงกัน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
                    passwordEdited = true;
                }
                else passwordEdited = false;
            }

            string id = Regex.Replace(txtIdcard.Text ?? "", @"\D", "");
            if (id.Length != 13)
            {
                MessageBox.Show("เลขบัตรประชาชนต้องเป็นตัวเลข 13 หลัก",
                    "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdcard.Focus(); txtIdcard.SelectAll(); return false;
            }
            txtIdcard.Text = id;

            string phone10 = NormalizeThaiMobile10(txtPhone.Text);
            if (string.IsNullOrEmpty(phone10))
            {
                MessageBox.Show("เบอร์โทรต้องเป็นตัวเลข 10 หลักเท่านั้น\nตัวอย่าง: 0812345678 หรือ +66812345678",
                    "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus(); txtPhone.SelectAll(); return false;
            }
            txtPhone.Text = phone10;

            string email = (txtEmail.Text ?? "").Trim();
            if (!IsValidEmailStrict(email))
            {
                MessageBox.Show("รูปแบบอีเมลไม่ถูกต้อง!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus(); txtEmail.SelectAll(); return false;
            }
            txtEmail.Text = NormalizeEmail(email);

            return true;
        }

        // ===== Buttons =====
        private void btnAdd_Click(object sender, EventArgs e)
        {
            isEditMode = false;
            ClearForm();
            EnableControlsOn();
            ReadOnlyControlsOn();

            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;
            cbxShowPassword1.Checked = false;
            cbxShowPassword2.Checked = false;
            passwordEdited = false;

            txtName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedEmployeeID))
            {
                MessageBox.Show("กรุณาเลือกพนักงานที่ต้องการแก้ไข!", "แจ้งเตือน",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isEditMode = true;
            EnableControlsOn();
            ReadOnlyControlsOn();

            var dal = new EmployeeDAL();
            DataTable dt = dal.GetEmployeeByIDtoUMNGT(selectedEmployeeID);
            if (dt.Rows.Count > 0)
            {
                currentHashedPassword = dt.Rows[0]["รหัสผ่าน"].ToString();
            }

            txtPassword.Clear();
            txtConfirmPassword.Clear();
            passwordEdited = false;

            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;
            cbxShowPassword1.Checked = false;
            cbxShowPassword2.Checked = false;

            txtName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateEmployeeData()) return;

            var dal = new EmployeeDAL();
            string excludeId = (isEditMode && !string.IsNullOrEmpty(selectedEmployeeID))
                                ? selectedEmployeeID : null;

            string emailToSave = NormalizeEmail((txtEmail.Text ?? "").Trim());
            string usernameToSave = (txtUsername.Text ?? "").Trim();
            string idcardToSave = (txtIdcard.Text ?? "").Trim();

            if (dal.ExistsEmail(emailToSave, excludeId))
            {
                MessageBox.Show("อีเมลนี้ถูกใช้งานแล้วในระบบ!", "ข้อมูลซ้ำ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus(); txtEmail.SelectAll(); return;
            }
            if (dal.ExistsUsername(usernameToSave, excludeId))
            {
                MessageBox.Show("ชื่อผู้ใช้นี้ถูกใช้งานแล้วในระบบ!", "ข้อมูลซ้ำ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus(); txtUsername.SelectAll(); return;
            }
            if (dal.ExistsIDCard(idcardToSave, excludeId))
            {
                MessageBox.Show("เลขบัตรประชาชนนี้ถูกใช้งานแล้วในระบบ!", "ข้อมูลซ้ำ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdcard.Focus(); txtIdcard.SelectAll(); return;
            }

            string finalPassword = !isEditMode
                ? BCrypt.Net.BCrypt.HashPassword(txtPassword.Text)
                : (passwordEdited ? BCrypt.Net.BCrypt.HashPassword(txtPassword.Text) : currentHashedPassword);

            var emp = new Employee
            {
                FirstName = txtName.Text.Trim(),
                LastName = txtLastname.Text.Trim(),
                Username = usernameToSave,
                Password = finalPassword,
                Phone = txtPhone.Text.Trim(),
                Email = emailToSave,
                Address = txtAddress.Text.Trim(),
                IDCard = idcardToSave,
                Role = cmbRole.SelectedItem?.ToString() ?? ""
            };
            if (isEditMode) emp.EmployeeID = selectedEmployeeID;

            try
            {
                bool ok = isEditMode ? dal.UpdateEmployee(emp) : dal.InsertEmployee(emp);

                if (ok)
                {
                    MessageBox.Show("บันทึกข้อมูลสำเร็จ!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadEmployeeData();   // รีโหลดข้อมูล (คงฟิลเตอร์เดิมไว้แล้ว)

                    ReadOnlyControlsOff();
                    EnableControlsOff();
                    ClearForm();
                    isEditMode = false;
                    passwordEdited = false;
                    currentHashedPassword = "";
                }
                else
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                string msg = "ข้อมูลซ้ำกับรายการเดิมในระบบ!";
                if (ex.Message.Contains("emp_email"))
                {
                    msg = "อีเมลนี้ถูกใช้งานแล้วในระบบ!";
                    txtEmail.Focus(); txtEmail.SelectAll();
                }
                else if (ex.Message.Contains("emp_username"))
                {
                    msg = "ชื่อผู้ใช้นี้ถูกใช้งานแล้วในระบบ!";
                    txtUsername.Focus(); txtUsername.SelectAll();
                }
                else if (ex.Message.Contains("emp_identification") || ex.Message.Contains("uq_employee_idcard"))
                {
                    msg = "เลขบัตรประชาชนนี้ถูกใช้งานแล้วในระบบ!";
                    txtIdcard.Focus(); txtIdcard.SelectAll();
                }

                MessageBox.Show(msg, "ข้อมูลซ้ำ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedEmployeeID))
            {
                MessageBox.Show("กรุณาเลือกพนักงานก่อนลบ!", "แจ้งเตือน",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบข้อมูลนี้?",
                                         "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            var dal = new EmployeeDAL();
            string errorMessage;

            bool success = dal.DeleteEmployee(selectedEmployeeID, out errorMessage);

            if (success)
            {
                MessageBox.Show("ลบข้อมูลสำเร็จ!", "สำเร็จ",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadEmployeeData(); // คงฟิลเตอร์เดิมไว้

                ReadOnlyControlsOff();
                EnableControlsOff();
                ClearForm();
                isEditMode = false;
                passwordEdited = false;
                currentHashedPassword = "";
            }
            else
            {
                MessageBox.Show(errorMessage, "ไม่สามารถลบข้อมูลได้",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ===== Search Handler =====
        private void OnSearchTriggered(object sender, SearchEventArgs ev)
        {
            if (employeeTable == null) return;

            string keyword = (ev.Keyword ?? "").Trim();

            // คีย์เวิร์ดว่าง = ล้างฟิลเตอร์ (โชว์ทั้งหมด)
            if (string.IsNullOrEmpty(keyword))
            {
                employeeBS.RemoveFilter();
                return;
            }

            // mapping display name → ชื่อคอลัมน์จริงใน DataTable
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["ชื่อ"] = "ชื่อ",
                ["นามสกุล"] = "นามสกุล",
                ["ชื่อผู้ใช้"] = "ชื่อผู้ใช้",
                ["เบอร์โทร"] = "เบอร์โทร",
                ["อีเมล"] = "อีเมล",
                ["ตำแหน่ง"] = "ตำแหน่ง",
                ["รหัสพนักงาน"] = "รหัสพนักงาน",
                ["ที่อยู่"] = "ที่อยู่",

                // เผื่อใช้ข้ามหน้าอื่นในอนาคต
                ["ชื่อลูกค้า"] = "ชื่อลูกค้า",
                ["เลขประจำตัว"] = "เลขประจำตัว",
                ["ชื่อบริษัทซัพพลายเออร์"] = "ชื่อบริษัทซัพพลายเออร์",
                ["รหัสซัพพลายเออร์"] = "รหัสซัพพลายเออร์",
                ["รหัสโครงการ"] = "รหัสโครงการ",
                ["ชื่อโครงการ"] = "ชื่อโครงการ",
                ["เลขที่สัญญา"] = "เลขที่สัญญา",
            };

            string display = ev.SearchBy ?? "";
            string column = map.ContainsKey(display) ? map[display] : display;

            string term = EscapeLikeValue(keyword);
            employeeBS.Filter = $"CONVERT([{column}], System.String) LIKE '%{term}%'";
        }

        private static string EscapeLikeValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("*", "[*]")
                .Replace("_", "[_]")
                .Replace("'", "''");
        }

        // ===== Helpers =====
        private string NormalizeThaiMobile10(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";
            string digits = Regex.Replace(raw, @"\D", "");
            if (digits.StartsWith("66"))
            {
                string after = digits.Substring(2);
                if (after.Length == 9) return "0" + after;
                if (after.Length == 10 && after.StartsWith("0")) return after;
                return "";
            }
            return Regex.IsMatch(digits, @"^0\\d{9}$") ? digits : "";
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
                var a = new MailAddress(email);
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

        // ===== UI enable/disable helpers =====
        private void ReadOnlyControlsOn()
        {
            txtName.ReadOnly = false; txtLastname.ReadOnly = false; txtIdcard.ReadOnly = false;
            txtEmail.ReadOnly = false; txtPhone.ReadOnly = false; txtAddress.ReadOnly = false;
            txtUsername.ReadOnly = false; txtPassword.ReadOnly = false; txtConfirmPassword.ReadOnly = false;
        }
        private void EnableControlsOn()
        {
            txtName.Enabled = true; txtLastname.Enabled = true; txtIdcard.Enabled = true; txtAddress.Enabled = true;
            txtEmail.Enabled = true; txtPhone.Enabled = true; txtUsername.Enabled = true; txtPassword.Enabled = true;
            txtConfirmPassword.Enabled = true; cmbRole.Enabled = true;
        }
        private void ReadOnlyControlsOff()
        {
            txtName.ReadOnly = true; txtLastname.ReadOnly = true; txtIdcard.ReadOnly = true;
            txtEmail.ReadOnly = true; txtPhone.ReadOnly = true; txtAddress.ReadOnly = true;
            txtUsername.ReadOnly = true; txtPassword.ReadOnly = true; txtConfirmPassword.ReadOnly = true;
            cmbRole.SelectedIndex = -1;
        }
        private void EnableControlsOff()
        {
            txtName.Enabled = false; txtLastname.Enabled = false; txtIdcard.Enabled = false; txtAddress.Enabled = false;
            txtEmail.Enabled = false; txtPhone.Enabled = false; txtUsername.Enabled = false; txtPassword.Enabled = false;
            txtConfirmPassword.Enabled = false; cmbRole.Enabled = false;
        }
        private void ClearForm()
        {
            txtName.Clear(); txtLastname.Clear(); txtUsername.Clear();
            txtPassword.Clear(); txtConfirmPassword.Clear();
            txtPhone.Clear(); txtEmail.Clear(); txtIdcard.Clear(); txtAddress.Clear();
            cmbRole.SelectedIndex = -1;
        }

        private void cbxShowPassword1_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !cbxShowPassword1.Checked;
        }
        private void cbxShowPassword2_CheckedChanged(object sender, EventArgs e)
        {
            txtConfirmPassword.UseSystemPasswordChar = !cbxShowPassword2.Checked;
        }
    }
}
