using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JRSApplication.Data_Access_Layer;
using MySql.Data.MySqlClient;
using JRSApplication.Components;   // ถ้า SearchboxControl อยู่ namespace นี้

namespace JRSApplication.Accountant
{
    public partial class ConfirmInvoice : UserControl
    {
        private string _currentInvId;   // when we navigate directly from Invoice
        private string empId;
        private string fullName; // reserved for future use
        private string role;     // reserved for future use

        public ConfirmInvoice() : this(fullName: "", role: "", empId: "") { }

        public ConfirmInvoice(string fullName, string role, string empId)
        {
            InitializeComponent();
            CustomizeDataGridView();

            // 🔒 ล็อกให้แก้วันที่ไม่ได้
            MakeHeaderFieldsReadOnly();

            // 🔴 ปิดตารางก่อนจนกว่าจะมีข้อมูล
            dgvInvoices.Enabled = false;
            dgvInvoices.BackgroundColor = SystemColors.AppWorkspace;

            PopulatePaymentMethod();
            this.empId = empId;

            // ================== ตั้งค่า Searchbox ==================
            try
            {
                // role Accountant / function จัดการการเงิน
                searchboxControl1.DefaultRole = "Accountant";
                searchboxControl1.DefaultFunction = "จัดการการเงิน";
                searchboxControl1.SetRoleAndFunction("Accountant", "จัดการการเงิน");

                // ผูก event ยิงค้นหา → ไปกรอง dgvInvoices
                searchboxControl1.SearchTriggered += SearchboxConfirm_SearchTriggered;
            }
            catch
            {
                // กัน error ตอนออกแบบฟอร์มใน Designer
            }
        }

        // -------------------- Searchbox event --------------------
        private void SearchboxConfirm_SearchTriggered(object sender, SearchEventArgs e)
        {
            ApplyInvoiceGridFilter(e.SearchBy, e.Keyword);
        }

        private static string EscapeLikeValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            // escape สำหรับ RowFilter
            return value
                .Replace("[", "[[]")   // escape [
                                       // ไม่ escape ] ป้องกัน pattern พัง
                .Replace("%", "[%]")   // escape %
                .Replace("*", "[*]")   // escape *
                .Replace("'", "''");   // escape '
        }

        private void ApplyInvoiceGridFilter(string searchBy, string keyword)
        {
            if (!(dgvInvoices.DataSource is DataTable table))
                return;

            keyword = (keyword ?? "").Trim();
            keyword = EscapeLikeValue(keyword);

            var dv = table.DefaultView;

            if (string.IsNullOrEmpty(keyword))
            {
                dv.RowFilter = string.Empty;
                return;
            }

            string expr = null;

            switch (searchBy)
            {
                case "รหัสใบแจ้งหนี้":
                    if (table.Columns.Contains("inv_id"))
                        expr = $"CONVERT(inv_id, 'System.String') LIKE '%{keyword}%'";
                    break;

                case "ยอดชำระ":
                    if (table.Columns.Contains("inv_grand_total"))
                        expr = $"CONVERT(inv_grand_total, 'System.String') LIKE '%{keyword}%'";
                    else if (table.Columns.Contains("inv_total_amount"))
                        expr = $"CONVERT(inv_total_amount, 'System.String') LIKE '%{keyword}%'";
                    break;

                case "สถานะ":
                    if (table.Columns.Contains("inv_status"))
                        expr = $"CONVERT(inv_status, 'System.String') LIKE '%{keyword}%'";
                    break;
            }

            // ถ้าไม่ระบุ หรือคอลัมน์ไม่เจอ → ค้นทุกคอลัมน์
            if (expr == null)
            {
                var filters = new List<string>();
                foreach (DataColumn col in table.Columns)
                {
                    filters.Add(
                        $"CONVERT([{col.ColumnName}], 'System.String') LIKE '%{keyword}%'");
                }
                expr = string.Join(" OR ", filters);
            }

            dv.RowFilter = expr;
        }

        // -------------------- Helpers: money/decimal --------------------
        private static decimal ToMoney(object val)
        {
            if (val == null || val == DBNull.Value) return 0m;

            var s = new string(val.ToString()
                .Where(ch => char.IsDigit(ch) || ch == '.' || ch == ',').ToArray());
            if (string.IsNullOrWhiteSpace(s)) return 0m;

            if (decimal.TryParse(s,
                    NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                    CultureInfo.CurrentCulture, out var d)) return d;
            if (decimal.TryParse(s,
                    NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                    CultureInfo.InvariantCulture, out d)) return d;

            return 0m;
        }

        private static bool TryParseDecimal(object val, out decimal d)
        {
            d = 0m;
            if (val == null || val == DBNull.Value) return false;
            var s = val.ToString();
            if (s.Any(char.IsLetter)) return false;

            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out d)) return true;
            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out d)) return true;
            return false;
        }

        // -------------------- UI setup --------------------
        private void CustomizeDataGridView()
        {
            dgvInvoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInvoices.MultiSelect = false;
            dgvInvoices.BorderStyle = BorderStyle.None;
            dgvInvoices.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dgvInvoices.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvInvoices.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dgvInvoices.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvInvoices.BackgroundColor = Color.White;

            dgvInvoices.EnableHeadersVisualStyles = false;
            dgvInvoices.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvInvoices.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvInvoices.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvInvoices.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dgvInvoices.ColumnHeadersHeight = 30;

            dgvInvoices.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dgvInvoices.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInvoices.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInvoices.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            dgvInvoices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInvoices.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvInvoices.RowTemplate.Height = 30;
            dgvInvoices.GridColor = Color.LightGray;
            dgvInvoices.RowHeadersVisible = false;
            dgvInvoices.ReadOnly = true;
            dgvInvoices.AllowUserToAddRows = false;
            dgvInvoices.AllowUserToResizeRows = false;
        }

        // -------------------- Load invoices (top grid) --------------------
        private void LoadInvoiceData()
        {
            SearchService service = new SearchService();
            DataTable dt = service.GetAllInvoices();
            dgvInvoices.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                dgvInvoices.Enabled = true;
                dgvInvoices.BackgroundColor = Color.White;
            }
            else
            {
                dgvInvoices.Enabled = false;
                dgvInvoices.BackgroundColor = SystemColors.AppWorkspace;
            }

            if (dgvInvoices.Columns.Contains("inv_id")) dgvInvoices.Columns["inv_id"].HeaderText = "เลขที่ใบแจ้งหนี้";
            if (dgvInvoices.Columns.Contains("inv_date")) dgvInvoices.Columns["inv_date"].HeaderText = "วันที่ออกใบแจ้งหนี้";
            if (dgvInvoices.Columns.Contains("inv_duedate")) dgvInvoices.Columns["inv_duedate"].HeaderText = "กำหนดชำระ";
            if (dgvInvoices.Columns.Contains("cus_id")) dgvInvoices.Columns["cus_id"].HeaderText = "รหัสลูกค้า";
            if (dgvInvoices.Columns.Contains("pro_id")) dgvInvoices.Columns["pro_id"].HeaderText = "รหัสโครงการ";
            if (dgvInvoices.Columns.Contains("phase_id")) dgvInvoices.Columns["phase_id"].HeaderText = "รหัสเฟส";

            ApplyInvoiceGridStatusColumn();

            // หลังโหลดข้อมูล ให้ฟิลเตอร์ตามค่าปัจจุบันใน searchbox ด้วย
            ApplyInvoiceGridFilter(searchboxControl1.SelectedSearchBy, searchboxControl1.Keyword);
        }

        // -------------------- Customer / Project --------------------
        private void LoadCustomerDetails(string cusId)
        {
            SearchService service = new SearchService();
            DataTable dt = service.GetCustomerById(cusId);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string fullname = $"{row["cus_name"]} {row["cus_lname"]}";
                txtCustomerName.Text = fullname;
                txtCustomerIDCard.Text = row["cus_id_card"].ToString();
                txtCustomerAddress.Text = row["cus_address"].ToString();
            }
        }

        private void LoadProjectDetails(string proId)
        {
            SearchService service = new SearchService();
            DataTable dt = service.GetProjectById(proId);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtProjectName.Text = row["pro_name"].ToString();
            }
        }

        // -------------------- Click row on top grid --------------------
        private void dgvInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvInvoices.Rows[e.RowIndex];
            InvoiceDAL dal = new InvoiceDAL();

            string invId = row.Cells["inv_id"].Value?.ToString() ?? "";
            string invDate = row.Cells["inv_date"].Value?.ToString() ?? "";
            string invDueDate = row.Cells["inv_duedate"].Value?.ToString() ?? "";
            string proId = row.Cells["pro_id"].Value?.ToString() ?? "";
            string cusId = row.Cells["cus_id"].Value?.ToString() ?? "";

            int phaseId = 0;
            if (row.Cells["phase_id"].Value != null)
                int.TryParse(row.Cells["phase_id"].Value.ToString(), out phaseId);
            string phaseNo = dal.GetPhaseNoById(phaseId);

            txtInvoiceNumber.Text = invId;
            if (!string.IsNullOrWhiteSpace(invDate))
                dtpInvoiceDate.Value = Convert.ToDateTime(invDate);

            txtDueDate.Text = invDueDate;
            txtProjectID.Text = proId;
            textBox7.Text = phaseNo;

            LoadProjectDetails(proId);
            LoadCustomerDetails(cusId);

            if (string.IsNullOrWhiteSpace(invId))
            {
                MessageBox.Show("ไม่พบเลขที่ใบแจ้งหนี้ (inv_id).", "แจ้งเตือน",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadInvoiceDetails(invId);
        }

        // -------------------- Details grid (right) --------------------
        private void LoadInvoiceDetails(string invId)
        {
            SetupInvoiceDetailGrid();

            var dal = new InvoiceDetailDAL();
            DataTable dt = dal.GetInvoiceDetailByInvId(invId);

            decimal total = 0m;
            foreach (DataRow r in dt.Rows)
            {
                decimal price = ToMoney(r["inv_price"]);
                if (TryParseDecimal(r["inv_quantity"], out var qty))
                    total += qty * price;
                else
                    total += price;
            }

            DataRow totalRow = dt.NewRow();
            totalRow["inv_detail"] = "รวม";
            dt.Rows.Add(totalRow);

            dgvInvoiceDetails.DataSource = dt;
            dgvInvoiceDetails.Tag = total;
        }

        private void PopulatePaymentMethod()
        {
            comboPaymentMethod.Items.Clear();
            comboPaymentMethod.Items.Add("โอนผ่านธนาคาร");
            comboPaymentMethod.Items.Add("เช็ค");
            comboPaymentMethod.SelectedIndex = 0;
        }

        private void SaveProofOfPayment(string invoiceId, string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            byte[] fileData = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            string query = @"INSERT INTO payment_proof (inv_id, file_name, file_data)
                             VALUES (@inv_id, @file_name, @file_data)";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@inv_id", invoiceId);
                cmd.Parameters.AddWithValue("@file_name", fileName);
                cmd.Parameters.AddWithValue("@file_data", fileData);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateInvoice(string invId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            string query = @"UPDATE invoice 
                             SET inv_status = 'ชำระแล้ว', 
                                 inv_method = @method, 
                                 paid_date = @paid_date,
                                 emp_id = @emp_id
                             WHERE inv_id = @inv_id";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@method", comboPaymentMethod.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@paid_date", dtpPaymentDate.Value);
                cmd.Parameters.AddWithValue("@emp_id", this.empId);
                cmd.Parameters.AddWithValue("@inv_id", invId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            using (var searchForm = new SearchForm("Project"))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    string selectedProjectId = searchForm.SelectedID;

                    SearchService service = new SearchService();
                    DataTable filtered = service.GetDraftInvoicesByProject(selectedProjectId);
                    dgvInvoices.DataSource = filtered;

                    if (filtered.Rows.Count > 0)
                    {
                        dgvInvoices.Enabled = true;
                        dgvInvoices.BackgroundColor = Color.White;

                        dgvInvoices.Rows[0].Selected = true;
                        dgvInvoices_CellContentClick(dgvInvoices, new DataGridViewCellEventArgs(0, 0));
                    }
                    else
                    {
                        dgvInvoices.Enabled = false;
                        dgvInvoices.BackgroundColor = Color.LightGray;
                    }

                    if (dgvInvoices.Columns.Contains("inv_id")) dgvInvoices.Columns["inv_id"].HeaderText = "เลขที่ใบแจ้งหนี้";
                    if (dgvInvoices.Columns.Contains("inv_date")) dgvInvoices.Columns["inv_date"].HeaderText = "วันที่ออกใบแจ้งหนี้";
                    if (dgvInvoices.Columns.Contains("inv_duedate")) dgvInvoices.Columns["inv_duedate"].HeaderText = "กำหนดชำระ";
                    if (dgvInvoices.Columns.Contains("pro_id")) dgvInvoices.Columns["pro_id"].HeaderText = "รหัสโครงการ";
                    if (dgvInvoices.Columns.Contains("phase_id")) dgvInvoices.Columns["phase_id"].HeaderText = "รหัสเฟส";
                    if (dgvInvoices.Columns.Contains("cus_id")) dgvInvoices.Columns["cus_id"].HeaderText = "รหัสลูกค้า";

                    ApplyInvoiceGridStatusColumn();

                    // ให้เคารพค่าที่พิมพ์ใน searchbox ปัจจุบันด้วย
                    ApplyInvoiceGridFilter(searchboxControl1.SelectedSearchBy, searchboxControl1.Keyword);
                }
            }
        }

        private void SetupInvoiceDetailGrid()
        {
            dgvInvoiceDetails.Columns.Clear();
            dgvInvoiceDetails.AutoGenerateColumns = false;
            dgvInvoiceDetails.RowHeadersVisible = false;
            dgvInvoiceDetails.AllowUserToAddRows = false;
            dgvInvoiceDetails.ReadOnly = true;
            dgvInvoiceDetails.BackgroundColor = Color.White;
            dgvInvoiceDetails.BorderStyle = BorderStyle.FixedSingle;
            dgvInvoiceDetails.GridColor = Color.LightGray;
            dgvInvoiceDetails.DefaultCellStyle.Font = new Font("Tahoma", 11);
            dgvInvoiceDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            dgvInvoiceDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInvoiceDetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInvoiceDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvInvoiceDetails.ScrollBars = ScrollBars.Both;

            dgvInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "No",
                HeaderText = "No",
                Width = 50
            });

            dgvInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "inv_detail",
                HeaderText = "รายละเอียด",
                DataPropertyName = "inv_detail",
                Width = 260,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });

            dgvInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "inv_quantity",
                HeaderText = "จำนวน",
                DataPropertyName = "inv_quantity",
                Width = 80
            });

            dgvInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "inv_price",
                HeaderText = "ราคา",
                DataPropertyName = "inv_price",
                Width = 80
            });

            dgvInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "subtotal",
                HeaderText = "ราคารวม",
                Width = 120
            });

            dgvInvoiceDetails.CellFormatting += dgvInvoiceDetails_CellFormatting;
        }

        private void dgvInvoiceDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var grid = dgvInvoiceDetails;
            var row = grid.Rows[e.RowIndex];

            if (row.Cells["inv_detail"].Value?.ToString() == "รวม")
            {
                if (grid.Columns[e.ColumnIndex].Name == "subtotal")
                {
                    decimal total = grid.Tag is decimal d ? d : 0m;
                    e.Value = total.ToString("N2") + " บาท";
                    e.CellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (grid.Columns[e.ColumnIndex].Name == "inv_detail")
                {
                    e.CellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    e.Value = "";
                }

                e.CellStyle.BackColor = Color.LightYellow;
                e.FormattingApplied = true;
                return;
            }

            if (grid.Columns[e.ColumnIndex].Name == "subtotal")
            {
                var qObj = row.Cells["inv_quantity"].Value;
                var pObj = row.Cells["inv_price"].Value;

                decimal price = ToMoney(pObj);

                if (TryParseDecimal(qObj, out var qty))
                    e.Value = (qty * price).ToString("N2");
                else
                    e.Value = price.ToString("N2");

                e.FormattingApplied = true;
                return;
            }

            if (grid.Columns[e.ColumnIndex].Name == "No")
            {
                e.Value = (e.RowIndex + 1).ToString();
                e.FormattingApplied = true;
            }
        }

        // -------------------- Confirm payment --------------------
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dgvInvoices.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกใบแจ้งหนี้ก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtInvoiceNumber.Text) ||
                string.IsNullOrWhiteSpace(txtDueDate.Text) ||
                string.IsNullOrWhiteSpace(txtProjectID.Text) ||
                string.IsNullOrWhiteSpace(txtProjectName.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerIDCard.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerAddress.Text))
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนทำการบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFilePath.Text))
            {
                MessageBox.Show("กรุณาแนบไฟล์หลักฐานการชำระเงิน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("กรุณาเลือกวิธีการชำระเงิน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string invId =
                !string.IsNullOrWhiteSpace(_currentInvId) ? _currentInvId :
                (dgvInvoices.SelectedRows.Count > 0
                    ? dgvInvoices.SelectedRows[0].Cells["inv_id"].Value?.ToString()
                    : txtInvoiceNumber.Text?.Trim());

            if (string.IsNullOrWhiteSpace(invId))
            {
                MessageBox.Show("ไม่พบเลขที่ใบแจ้งหนี้", "แจ้งเตือน",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string filePath = txtFilePath.Text;

            UpdateInvoice(invId);
            SaveProofOfPayment(invId, filePath);

            MessageBox.Show("บันทึกข้อมูลเรียบร้อยแล้ว", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadInvoiceData();
            dgvInvoices.ClearSelection();
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All Files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = ofd.FileName;
            }
        }

        public void InitFromInvoiceId(string invId)
        {
            if (string.IsNullOrWhiteSpace(invId)) return;
            _currentInvId = invId;

            var dal = new InvoiceDAL();
            DataTable dt = dal.GetInvoiceID(invId);
            if (dt.Rows.Count == 0) return;

            var r = dt.Rows[0];

            txtInvoiceNumber.Text = invId;
            if (r.Table.Columns.Contains("inv_date") && r["inv_date"] != DBNull.Value)
                dtpInvoiceDate.Value = Convert.ToDateTime(r["inv_date"]);

            txtDueDate.Text = r["inv_duedate"]?.ToString() ?? "";

            txtProjectID.Text = r["pro_id"]?.ToString() ?? "";
            txtProjectName.Text = r["pro_name"]?.ToString() ?? "";

            if (r.Table.Columns.Contains("phase_no"))
                textBox7.Text = r["phase_no"]?.ToString() ?? "";
            else if (r.Table.Columns.Contains("phase_id"))
                textBox7.Text = r["phase_id"]?.ToString() ?? "";

            txtCustomerName.Text = r["cus_fullname"]?.ToString() ?? "";
            txtCustomerIDCard.Text = r["cus_id_card"]?.ToString() ?? "";
            txtCustomerAddress.Text = r["cus_address"]?.ToString() ?? "";

            LoadInvoiceDetails(invId);

            dgvInvoices.Enabled = true;
            dgvInvoices.BackgroundColor = Color.White;
        }

        private void MakeHeaderFieldsReadOnly()
        {
            dtpInvoiceDate.Enabled = false;
            dtpInvoiceDate.TabStop = false;

            txtDueDate.Enabled = false;
            txtDueDate.TabStop = false;
        }

        // ==================== Status column on dgvInvoices ====================
        private string NormalizeStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return "ไม่พบสถานะ";
            status = status.Trim();

            if (status.Equals("Draft", StringComparison.OrdinalIgnoreCase)) return "ค้างชำระ";
            if (status.Equals("Pending", StringComparison.OrdinalIgnoreCase)) return "รอดำเนินการ";
            if (status.Contains("ชำระแล้ว")) return "ชำระแล้ว";
            if (status.Contains("ยกเลิก")) return "ยกเลิก";
            return status;
        }

        private void ApplyInvoiceGridStatusColumn()
        {
            var g = dgvInvoices;
            if (g.DataSource == null) return;
            if (!g.Columns.Contains("inv_status")) return;

            var col = g.Columns["inv_status"];
            col.HeaderText = "สถานะการชำระเงิน";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            col.DisplayIndex = g.Columns.Count - 1;
            col.SortMode = DataGridViewColumnSortMode.Automatic;

            g.CellFormatting -= DgvInvoices_CellFormatting_StatusColor;
            g.CellFormatting += DgvInvoices_CellFormatting_StatusColor;
        }

        private void DgvInvoices_CellFormatting_StatusColor(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var grid = (DataGridView)sender;

            if (!grid.Columns.Contains("inv_status")) return;

            var row = grid.Rows[e.RowIndex];
            string raw = row.Cells["inv_status"]?.Value?.ToString() ?? "";
            string status = NormalizeStatus(raw);

            if (grid.Columns[e.ColumnIndex].Name == "inv_status")
            {
                e.Value = status;
                e.FormattingApplied = true;
            }

            if (status.Contains("ชำระแล้ว"))
            {
                row.DefaultCellStyle.BackColor = Color.Honeydew;
                row.DefaultCellStyle.ForeColor = Color.DarkGreen;
            }
            else if (status.Contains("ค้างชำระ") || status.Contains("รอดำเนินการ"))
            {
                row.DefaultCellStyle.BackColor = Color.MistyRose;
                row.DefaultCellStyle.ForeColor = Color.Maroon;
            }
            else if (status.Contains("ยกเลิก"))
            {
                row.DefaultCellStyle.BackColor = Color.Gainsboro;
                row.DefaultCellStyle.ForeColor = Color.DimGray;
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.White;
                row.DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        // -------------------- empty handlers from designer --------------------
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void txtCustomerName_TextChanged(object sender, EventArgs e) { }
        private void txtInvoiceNumber_TextChanged(object sender, EventArgs e) { }
        private void txtCustomerIDCard_TextChanged(object sender, EventArgs e) { }
        private void txtProjectID_TextChanged(object sender, EventArgs e) { }
        private void txtProjectName_TextChanged(object sender, EventArgs e) { }
        private void txtCustomerAddress_TextChanged(object sender, EventArgs e) { }
    }
}
