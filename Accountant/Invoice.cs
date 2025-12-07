using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JRSApplication.Components; // ✅ ใช้ Model ใหม่ + SearchboxControl
using JRSApplication.Data_Access_Layer;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;

namespace JRSApplication.Accountant
{
    public partial class Invoice : UserControl
    {
        public Invoice()
        {
            InitializeComponent();

            // -----------------------------
            // event เดิมของ Invoice
            // -----------------------------
            cmbPhase.SelectedIndexChanged += cmbPhase_SelectedIndexChanged;
            this.Load += Invoice_Load;

            panel1.Dock = DockStyle.Fill;

            CustomizeInvoiceGrid();
            BuildInvoiceGridMenu();
            dtgvInvoice.CellMouseDown += dtgvInvoice_CellMouseDown;

            dtpInvDate.ValueChanged += dtpInvDate_ValueChanged;
            dtpDueDate.ValueChanged += dtpDueDate_ValueChanged;

            // -----------------------------
            // NEW: ตั้งค่า Searchbox แบบเดียวกับฟอร์มอื่น
            // -----------------------------
            try
            {
                // หน้า Invoice ใช้สำหรับบัญชีอยู่แล้ว ฟิกเป็น Accountant + จัดการการเงิน
                searchboxControl1.DefaultRole = "Accountant";
                searchboxControl1.DefaultFunction = "จัดการการเงิน";

                searchboxControl1.SetRoleAndFunction("Accountant", "จัดการการเงิน");
            }
            catch
            {
                // กัน error เวลา designer โหลดคอนโทรล
            }

            // ผูกอีเวนต์ยิงค้นหา → ไปกรอง DataGridView
            searchboxControl1.SearchTriggered += SearchboxInvoice_SearchTriggered;
            dtgvInvoice.CellClick += dtgvInvoice_CellClick;
        }

        // --------------------------------------------------
        // Searchbox → Filter ตาราง dtgvInvoice แบบโค้ดเดิม (row.Visible)
        // --------------------------------------------------
        private void SearchboxInvoice_SearchTriggered(object sender, SearchEventArgs e)
        {
            ApplyInvoiceGridFilter(e.SearchBy, e.Keyword);
        }

        private void ApplyInvoiceGridFilter(string searchBy, string keyword)
        {
            keyword = (keyword ?? "").Trim();

            if (dtgvInvoice.Rows.Count == 0)
                return;

            // ====== จุดสำคัญอยู่ตรงนี้ ======
            CurrencyManager cm = null;

            if (dtgvInvoice.DataSource != null)
            {
                // อย่าลืมมี this. และมี dtgvInvoice.DataSource อยู่ใน []
                cm = (CurrencyManager)this.BindingContext[dtgvInvoice.DataSource];
            }
            // =================================

            cm?.SuspendBinding();   // ถ้า cm ไม่ null ให้หยุด binding ก่อน

            try
            {
                foreach (DataGridViewRow row in dtgvInvoice.Rows)
                {
                    if (row.IsNewRow) continue;

                    // ถ้า keyword ว่างให้โชว์ทุกแถว
                    if (string.IsNullOrEmpty(keyword))
                    {
                        row.Visible = true;
                        continue;
                    }

                    string cellValue = "";

                    switch (searchBy)
                    {
                        case "รหัสใบแจ้งหนี้":
                            if (dtgvInvoice.Columns.Contains("inv_id"))
                                cellValue = row.Cells["inv_id"].Value?.ToString() ?? "";
                            else if (dtgvInvoice.Columns.Contains("inv_no"))
                                cellValue = row.Cells["inv_no"].Value?.ToString() ?? "";
                            break;

                        case "ยอดชำระ":
                            if (dtgvInvoice.Columns.Contains("inv_grand_total"))
                                cellValue = row.Cells["inv_grand_total"].Value?.ToString() ?? "";
                            else if (dtgvInvoice.Columns.Contains("inv_total_amount"))
                                cellValue = row.Cells["inv_total_amount"].Value?.ToString() ?? "";
                            break;

                        case "สถานะ":
                            if (dtgvInvoice.Columns.Contains("inv_status"))
                                cellValue = row.Cells["inv_status"].Value?.ToString() ?? "";
                            break;

                        default:
                            var sb = new StringBuilder();
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.Value != null)
                                    sb.Append(cell.Value.ToString()).Append(" ");
                            }
                            cellValue = sb.ToString();
                            break;
                    }

                    bool match = cellValue.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
                    row.Visible = match;
                }
            }
            finally
            {
                cm?.ResumeBinding();  // เปิด binding กลับ
            }
        }

        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Project");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                txtProjectID.Text = searchForm.SelectedID;                  // รหัสโครงการ
                txtContractNumber.Text = searchForm.SelectedContract;       // เลขที่สัญญา
                txtProjectName.Text = searchForm.SelectedName;              // ชื่อโครงการ
                txtCusID.Text = searchForm.SelectedCusID;                   // ใช้ cus_id
                txtCusName.Text = searchForm.SelectedLastName;             // ชื่อลูกค้า

                LoadPhasesToComboBox(searchForm.SelectedID);

                // โหลดตาราง dtgvInvoice จาก project id
                LoadInvoiceTableByProject(searchForm.SelectedID);
            }
        }

        private void LoadInvoiceTableByProject(string projectId)
        {
            InvoiceDAL dal = new InvoiceDAL();
            DataTable dt = dal.GetAllInvoicesByProjectId(projectId);
            dtgvInvoice.DataSource = dt;

            // === Step 1: Set all column headers first ===
            if (dtgvInvoice.Columns.Contains("inv_id")) dtgvInvoice.Columns["inv_id"].HeaderText = "รหัสใบแจ้งหนี้";
            if (dtgvInvoice.Columns.Contains("pro_id")) dtgvInvoice.Columns["pro_id"].HeaderText = "รหัสโครงการ";
            if (dtgvInvoice.Columns.Contains("pro_name")) dtgvInvoice.Columns["pro_name"].HeaderText = "ชื่อโครงการ";
            if (dtgvInvoice.Columns.Contains("phase_no")) dtgvInvoice.Columns["phase_no"].HeaderText = "เฟสที่";
            if (dtgvInvoice.Columns.Contains("inv_date")) dtgvInvoice.Columns["inv_date"].HeaderText = "วันที่ออก";
            if (dtgvInvoice.Columns.Contains("inv_duedate")) dtgvInvoice.Columns["inv_duedate"].HeaderText = "กำหนดชำระ";
            if (dtgvInvoice.Columns.Contains("inv_status")) dtgvInvoice.Columns["inv_status"].HeaderText = "สถานะใบแจ้งหนี้";
            if (dtgvInvoice.Columns.Contains("inv_method")) dtgvInvoice.Columns["inv_method"].HeaderText = "วิธีการชำระเงิน";
            if (dtgvInvoice.Columns.Contains("paid_date")) dtgvInvoice.Columns["paid_date"].HeaderText = "วันที่ชำระเงิน";
            if (dtgvInvoice.Columns.Contains("emp_fullname")) dtgvInvoice.Columns["emp_fullname"].HeaderText = "ผู้รับเงิน";


            // === Step 2: Hide the columns you don't want (marked with 'X') ===
            if (dtgvInvoice.Columns.Contains("cus_fullname")) dtgvInvoice.Columns["cus_fullname"].Visible = false;
            if (dtgvInvoice.Columns.Contains("cus_id_card")) dtgvInvoice.Columns["cus_id_card"].Visible = false;
            if (dtgvInvoice.Columns.Contains("cus_address")) dtgvInvoice.Columns["cus_address"].Visible = false;
            if (dtgvInvoice.Columns.Contains("phase_id")) dtgvInvoice.Columns["phase_id"].Visible = false;
            if (dtgvInvoice.Columns.Contains("emp_id")) dtgvInvoice.Columns["emp_id"].Visible = false;

            // === Step 3: Set the DisplayIndex for each VISIBLE column based on your image ===
            int i = 0; // A counter for the display order
            if (dtgvInvoice.Columns.Contains("inv_id")) dtgvInvoice.Columns["inv_id"].DisplayIndex = i++;             // 1. รหัสใบแจ้งหนี้
            if (dtgvInvoice.Columns.Contains("pro_id")) dtgvInvoice.Columns["pro_id"].DisplayIndex = i++;             // 2. รหัสโครงการ
            if (dtgvInvoice.Columns.Contains("pro_name")) dtgvInvoice.Columns["pro_name"].DisplayIndex = i++;         // 3. ชื่อโครงการ
            if (dtgvInvoice.Columns.Contains("phase_no")) dtgvInvoice.Columns["phase_no"].DisplayIndex = i++;
            if (dtgvInvoice.Columns.Contains("inv_date")) dtgvInvoice.Columns["inv_date"].DisplayIndex = i++;         // 5. วันที่ออก
            if (dtgvInvoice.Columns.Contains("inv_duedate")) dtgvInvoice.Columns["inv_duedate"].DisplayIndex = i++;   // 6. กำหนดชำระ
            if (dtgvInvoice.Columns.Contains("inv_status")) dtgvInvoice.Columns["inv_status"].DisplayIndex = i++;     // 7. สถานะใบแจ้งหนี้
            if (dtgvInvoice.Columns.Contains("inv_method")) dtgvInvoice.Columns["inv_method"].DisplayIndex = i++;     // 8. วิธีการชำระเงิน
            if (dtgvInvoice.Columns.Contains("paid_date")) dtgvInvoice.Columns["paid_date"].DisplayIndex = i++;       // 9. วันที่ชำระเงิน
            if (dtgvInvoice.Columns.Contains("emp_fullname")) dtgvInvoice.Columns["emp_fullname"].DisplayIndex = i++; // 10. ผู้รับเงิน


            // หลังโหลดข้อมูลแล้ว ให้ฟิลเตอร์ตามค่าปัจจุบันใน Searchbox (เหมือน ManageProject)
            ApplyInvoiceGridFilter(searchboxControl1.SelectedSearchBy, searchboxControl1.Keyword);
        }

        private void LoadPhasesToComboBox(string projectId)
        {
            SearchService service = new SearchService();
            DataTable dt = service.GetPhasesByProjectId(projectId);

            DataRow dr = dt.NewRow();
            dr["phase_id"] = DBNull.Value;
            dr["phase_no"] = "-- เลือกเฟส --";
            dt.Rows.InsertAt(dr, 0);

            cmbPhase.DisplayMember = "phase_no";
            cmbPhase.ValueMember = "phase_id";
            cmbPhase.DataSource = dt;
            cmbPhase.SelectedIndex = 0;
        }

        private void Invoice_Load(object sender, EventArgs e)
        {
            try
            {
                var dal = new InvoiceDAL();
                txtInvNo.Text = dal.PeekNextInvoiceId();
            }
            catch
            {
                txtInvNo.Text = "";
            }

            txtInvNo.Text = new InvoiceDAL().PeekNextInvoiceId();
            ApplyDueDateFloor();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjectID.Text) || string.IsNullOrWhiteSpace(txtCusID.Text))
            {
                MessageBox.Show("กรุณาเลือกโครงการและลูกค้าก่อนบันทึก", "แจ้งเตือน",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpDueDate.Value.Date < dtpInvDate.Value.Date)
            {
                MessageBox.Show("กำหนดชำระเงินต้องไม่น้อยกว่าวันที่ออกใบแจ้งหนี้",
                    "วันที่ไม่ถูกต้อง", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDueDate.Value = dtpInvDate.Value.Date;
                return;
            }

            try
            {
                string proId = txtProjectID.Text.Trim();

                string phaseIdFromUI = cmbPhase.SelectedValue?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(phaseIdFromUI))
                {
                    MessageBox.Show("กรุณาเลือกเฟสงานก่อนบันทึกใบแจ้งหนี้", "แจ้งเตือน",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbPhase.Focus();
                    cmbPhase.DroppedDown = true;
                    return;
                }

                var searchSvc = new SearchService();
                var phases = searchSvc.GetPhasesByProjectId(proId);
                bool belongs = phases.AsEnumerable().Any(r => r["phase_id"]?.ToString() == phaseIdFromUI);
                if (!belongs)
                {
                    MessageBox.Show("เฟสงานที่เลือกไม่ตรงกับโครงการ กรุณาเลือกใหม่", "แจ้งเตือน",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbPhase.Focus();
                    cmbPhase.DroppedDown = true;
                    return;
                }

                InvoiceModel model = new InvoiceModel
                {
                    InvDate = dtpInvDate.Value,
                    InvDueDate = dtpDueDate.Value,
                    CusId = txtCusID.Text.Trim(),
                    CusName = txtCusName.Text.Trim(),
                    ProId = proId,
                    ProNumber = txtContractNumber.Text.Trim(),
                    ProName = txtProjectName.Text.Trim(),
                    PhaseId = phaseIdFromUI,
                    PhaseBudget = txtPhaseBudget.Text.Trim(),
                    PhaseDetail = txtPhaseDetail.Text.Trim(),
                    InvRemark = txtRemark.Text.Trim(),
                    Quantity = txtQuantity.Text.Trim()
                };

                var dal = new InvoiceDAL();
                string newInvId = dal.InsertInvoice(model);

                if (!string.IsNullOrWhiteSpace(newInvId))
                {
                    txtInvNo.Text = newInvId;

                    string detail = txtDetail.Text.Trim();
                    string quantityText = txtQuantity.Text.Trim();
                    if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price)) price = 0m;
                    decimal vatRate = 7m;

                    var detailDal = new InvoiceDetailDAL();
                    detailDal.InsertInvoiceDetail(newInvId, detail, price, quantityText, vatRate);

                    decimal ParseMoney(string s)
                    {
                        if (string.IsNullOrWhiteSpace(s)) return 0m;

                        if (decimal.TryParse(s,
                            NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                            CultureInfo.CurrentCulture, out var v)) return v;

                        if (decimal.TryParse(s,
                            NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                            CultureInfo.InvariantCulture, out v)) return v;

                        return 0m;
                    }

                    decimal phaseBudget = ParseMoney(txtPhaseBudget.Text);
                    decimal extraPrice = price;
                    decimal subtotal = phaseBudget + extraPrice;
                    decimal vat = Math.Round(subtotal * 0.07m, 2, MidpointRounding.AwayFromZero);
                    decimal grand = subtotal + vat;

                    dal.UpdateInvoiceAmounts(newInvId, subtotal, vat, grand);

                    MessageBox.Show("บันทึกข้อมูลสำเร็จ! เลขที่ใบแจ้งหนี้: " + newInvId,
                        "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadInvoiceTableByProject(proId);

                    txtCusID.Text = "";
                    txtCusName.Text = "";
                    txtProjectID.Text = "";
                    txtContractNumber.Text = "";
                    txtProjectName.Text = "";
                    txtPhaseBudget.Text = "";
                    txtPhaseDetail.Text = "";
                    txtDetail.Text = "";
                    txtQuantity.Text = "";
                    txtPrice.Text = "";
                    txtRemark.Text = "";
                }
                else
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกใบแจ้งหนี้",
                        "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message,
                    "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInvNo.Text))
            {
                MessageBox.Show("ยังไม่มีเลขที่ใบแจ้งหนี้", "แจ้งเตือน",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string invId = txtInvNo.Text.Trim();
            decimal phaseBudget = 0m;
            string phaseDetail = "งบประมาณของเฟสงาน";
            string invDate = "";
            string remark = txtRemark.Text.Trim();
            string cusAddress = "";

            try
            {
                DataTable headTable = new InvoiceDAL().GetInvoiceID(invId);
                if (headTable.Rows.Count > 0)
                {
                    var r = headTable.Rows[0];
                    if (headTable.Columns.Contains("phase_budget") && r["phase_budget"] != DBNull.Value)
                        phaseBudget = Convert.ToDecimal(r["phase_budget"]);
                    if (headTable.Columns.Contains("phase_detail") && r["phase_detail"] != DBNull.Value)
                        phaseDetail = Convert.ToString(r["phase_detail"]);
                    if (headTable.Columns.Contains("inv_date") && r["inv_date"] != DBNull.Value)
                        invDate = Convert.ToDateTime(r["inv_date"]).ToString("d/M/yyyy");
                    if (headTable.Columns.Contains("cus_address") && r["cus_address"] != DBNull.Value)
                        cusAddress = r["cus_address"].ToString();
                }
            }
            catch { }

            var table = new DataTable();
            table.Columns.Add("receipt_id");
            table.Columns.Add("receipt_date");
            table.Columns.Add("inv_id");
            table.Columns.Add("inv_date");
            table.Columns.Add("cus_fullname");
            table.Columns.Add("cus_address");
            table.Columns.Add("pro_name");
            table.Columns.Add("phase_detail");
            table.Columns.Add("phase_no");
            table.Columns.Add("phase_budget", typeof(decimal));
            table.Columns.Add("inv_remark");
            table.Columns.Add("subtotal", typeof(decimal));
            table.Columns.Add("vat", typeof(string)); // Keep as string
            table.Columns.Add("grand_total", typeof(decimal));
            table.Columns.Add("ToDate");
            table.Columns.Add("inv_detail");
            table.Columns.Add("inv_quantity");
            table.Columns.Add("inv_price");

            decimal subtotal = phaseBudget;
            string invDetail = txtDetail.Text.Trim();
            string invQty = string.IsNullOrWhiteSpace(txtQuantity.Text) ? "1" : txtQuantity.Text.Trim();
            string invPrice = txtPrice.Text.Trim();

            if (decimal.TryParse(invPrice, out decimal extraPrice))
                subtotal += extraPrice;

            // --- MODIFIED CODE STARTS HERE ---

            // 1. Force the VAT display text to always be a dash.
            string vatDisplayText = "-";

            // 2. Make the grand total equal to the subtotal.
            decimal grand = subtotal;

            // --- MODIFIED CODE ENDS HERE ---

            var thaiCulture = new CultureInfo("th-TH");
            string toDate = DateTime.Now.ToString("d MMMM yyyy", thaiCulture);

            table.Rows.Add(
                DBNull.Value,
                DBNull.Value,
                invId,
                dtpInvDate.Value.ToString("d/M/yyyy"),
                txtCusName.Text,
                cusAddress,
                txtProjectName.Text,
                phaseDetail,
                cmbPhase.Text,
                phaseBudget,
                remark,
                subtotal,
                vatDisplayText, // This will now always be "-"
                grand,          // This will now be the same as subtotal
                toDate,
                string.IsNullOrWhiteSpace(invDetail) ? "" : invDetail,
                invQty,
                string.IsNullOrWhiteSpace(invPrice) ? "0.00" : invPrice
            );

            var frm = new InvoicePrintRDLC(table);
            frm.ShowDialog();
        }

        private void cmbPhase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPhase.SelectedValue == null || cmbPhase.SelectedIndex == 0)
            {
                txtPhaseBudget.Text = "";
                txtPhaseDetail.Text = "";
                return;
            }

            string phaseId = cmbPhase.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(phaseId))
            {
                try
                {
                    PhaseDAL phaseDAL = new PhaseDAL();
                    var result = phaseDAL.GetPhaseBudgetAndDetail(phaseId);

                    txtPhaseBudget.Text = result.budget.ToString("N2");
                    txtPhaseDetail.Text = result.detail;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการดึงข้อมูลเฟสงาน: " + ex.Message);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var projectId = txtProjectID.Text?.Trim();
            if (string.IsNullOrWhiteSpace(projectId))
            {
                MessageBox.Show("กรุณาเลือกโครงการก่อน", "แจ้งเตือน",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var searchForm = new SearchForm("UnpaidInvoiceByProject", projectId))
            {
                if (searchForm.ShowDialog() != DialogResult.OK) return;

                var selectedInvoiceId = searchForm.SelectedID;
                if (string.IsNullOrWhiteSpace(selectedInvoiceId))
                {
                    MessageBox.Show("ไม่พบเลขที่ใบแจ้งหนี้ที่เลือก", "แจ้งเตือน",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    if (cmbPhase.DataSource == null ||
                        (cmbPhase.DataSource as DataTable)?.Rows.Count == 0)
                    {
                        LoadPhasesToComboBox(projectId);
                    }

                    LoadInvoiceById(selectedInvoiceId);
                    txtRemark.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("โหลดข้อมูลใบแจ้งหนี้ไม่สำเร็จ: " + ex.Message,
                        "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CustomizeInvoiceGrid()
        {
            dtgvInvoice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvInvoice.MultiSelect = false;
            dtgvInvoice.BorderStyle = BorderStyle.None;

            dtgvInvoice.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvInvoice.DefaultCellStyle.BackColor = Color.White;
            dtgvInvoice.DefaultCellStyle.ForeColor = Color.Black;

            dtgvInvoice.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvInvoice.DefaultCellStyle.SelectionForeColor = Color.White;

            dtgvInvoice.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvInvoice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvInvoice.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            dtgvInvoice.EnableHeadersVisualStyles = false;
            dtgvInvoice.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvInvoice.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvInvoice.ColumnHeadersHeight = 30;

            dtgvInvoice.RowTemplate.Height = 30;
            dtgvInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvInvoice.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvInvoice.RowHeadersVisible = false;
            dtgvInvoice.ReadOnly = true;
            dtgvInvoice.AllowUserToAddRows = false;
            dtgvInvoice.AllowUserToResizeRows = false;
            dtgvInvoice.GridColor = Color.LightGray;
        }

        private ContextMenuStrip _invoiceMenu;

        private void BuildInvoiceGridMenu()
        {
            _invoiceMenu = new ContextMenuStrip();
            var miPrint = new ToolStripMenuItem("พิมพ์ใบแจ้งหนี้");
            miPrint.Click += (s, e) => PrintSelectedInvoiceFromGrid();
            _invoiceMenu.Items.Add(miPrint);

            var miConfirm = new ToolStripMenuItem("ยืนยันการรับชำระเงิน");
            miConfirm.Click += (s, e) => GoToConfirmPaymentForSelected();
            _invoiceMenu.Items.Add(miConfirm);
        }

        private void dtgvInvoice_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // เลือกแถวที่คลิก
            dtgvInvoice.ClearSelection();
            dtgvInvoice.Rows[e.RowIndex].Selected = true;
            dtgvInvoice.CurrentCell = dtgvInvoice.Rows[e.RowIndex].Cells[e.ColumnIndex];

            var row = dtgvInvoice.Rows[e.RowIndex];
            string status = row.Cells["inv_status"]?.Value?.ToString() ?? "";

            // ถ้าเป็นใบที่ "ชำระแล้ว" ให้ปิดเมนูพิมพ์
            if (_invoiceMenu != null && _invoiceMenu.Items.Count > 0)
            {
                _invoiceMenu.Items[0].Enabled =
                    !status.Equals("ชำระแล้ว", StringComparison.OrdinalIgnoreCase);
            }

            // แสดงเมนูเฉพาะตอนคลิกขวาเท่านั้น (ไม่ต้องมี popup ฟอร์มแล้ว)
            if (e.Button == MouseButtons.Right && _invoiceMenu != null)
            {
                _invoiceMenu.Show(dtgvInvoice, new Point(e.X, e.Y));
            }

            // ❌ ลบบรรทัดนี้ออก (หรือคอมเมนต์ทิ้ง)
            // if (e.RowIndex >= 0 && e.Button == MouseButtons.Left)
            // {
            //     dtgvInvoice.ClearSelection();
            //     dtgvInvoice.Rows[e.RowIndex].Selected = true;
            //     ShowInvoiceActionPopup();
            // }
        }
        private void dtgvInvoice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Make sure the click is on a valid row (not the header)
            if (e.RowIndex < 0)
            {
                return;
            }

            // Get the row that was clicked
            var row = dtgvInvoice.Rows[e.RowIndex];

            // Get the invoice ID from the clicked row's "inv_id" column.
            string selectedInvoiceId = row.Cells["inv_id"]?.Value?.ToString();

            // If we couldn't get an ID, stop.
            if (string.IsNullOrWhiteSpace(selectedInvoiceId))
            {
                return;
            }

            // Use your existing method to load all the data into the textboxes.
            try
            {
                LoadInvoiceById(selectedInvoiceId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูล: " + ex.Message, "ผิดพลาด",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintSelectedInvoiceFromGrid()
        {
            if (dtgvInvoice.SelectedRows.Count == 0) return;
            var row = dtgvInvoice.SelectedRows[0];

            string status = row.Cells["inv_status"]?.Value?.ToString() ?? "";
            if (status.Equals("ชำระแล้ว", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("รายการนี้ชำระแล้ว", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string invId = row.Cells["inv_id"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(invId)) return;

            LoadInvoiceById(invId);
            btnPrintInvoice_Click(null, EventArgs.Empty);
        }

        private void LoadInvoiceById(string invId)
        {
            var invoiceDAL = new InvoiceDAL();
            DataTable dt = invoiceDAL.GetInvoiceID(invId);

            if (dt.Rows.Count == 0) return;
            DataRow r = dt.Rows[0];

            string GetStr(string col)
                => (r.Table.Columns.Contains(col) && r[col] != DBNull.Value) ? r[col].ToString() : "";

            txtInvNo.Text = r.Table.Columns.Contains("inv_no") && r["inv_no"] != DBNull.Value
                ? r["inv_no"].ToString()
                : GetStr("inv_id");

            if (r.Table.Columns.Contains("inv_date") && r["inv_date"] != DBNull.Value)
                dtpInvDate.Value = Convert.ToDateTime(r["inv_date"]);
            if (r.Table.Columns.Contains("inv_duedate") && r["inv_duedate"] != DBNull.Value)
                dtpDueDate.Value = Convert.ToDateTime(r["inv_duedate"]);

            txtProjectID.Text = GetStr("pro_id");
            txtContractNumber.Text = GetStr("pro_number");
            txtProjectName.Text = GetStr("pro_name");

            string phaseIdStr = GetStr("phase_id");
            if (!string.IsNullOrWhiteSpace(phaseIdStr))
            {
                try { cmbPhase.SelectedValue = phaseIdStr; } catch { }
            }

            if (r.Table.Columns.Contains("phase_budget") && r["phase_budget"] != DBNull.Value)
                txtPhaseBudget.Text = Convert.ToDecimal(r["phase_budget"]).ToString("N2");
            else if (!string.IsNullOrWhiteSpace(phaseIdStr))
            {
                try
                {
                    var phDal = new PhaseDAL();
                    var ph = phDal.GetPhaseBudgetAndDetail(phaseIdStr);
                    txtPhaseBudget.Text = ph.budget.ToString("N2");
                    if (string.IsNullOrWhiteSpace(txtPhaseDetail.Text))
                        txtPhaseDetail.Text = ph.detail ?? "";
                }
                catch { }
            }

            if (r.Table.Columns.Contains("phase_detail") && r["phase_detail"] != DBNull.Value)
                txtPhaseDetail.Text = r["phase_detail"].ToString();

            txtCusID.Text = GetStr("cus_id");
            txtCusName.Text = GetStr("cus_fullname");

            txtRemark.Text = GetStr("inv_remark");

            bool hasMergedDetailCols =
                r.Table.Columns.Contains("inv_detail") ||
                r.Table.Columns.Contains("inv_quantity") ||
                r.Table.Columns.Contains("inv_price");

            if (hasMergedDetailCols)
            {
                txtDetail.Text = GetStr("inv_detail");
                txtQuantity.Text = GetStr("inv_quantity");
                txtPrice.Text = GetStr("inv_price");
            }

            if (string.IsNullOrWhiteSpace(txtDetail.Text) &&
                string.IsNullOrWhiteSpace(txtPrice.Text) &&
                !string.IsNullOrWhiteSpace(invId))
            {
                try
                {
                    var dDal = new InvoiceDetailDAL();
                    var first = dDal.GetFirstDetailForPrint(invId);
                    if (first.HasValue)
                    {
                        if (string.IsNullOrWhiteSpace(txtDetail.Text))
                            txtDetail.Text = first.Value.Detail ?? "";
                        if (string.IsNullOrWhiteSpace(txtQuantity.Text))
                            txtQuantity.Text = first.Value.Quantity ?? "";
                        if (string.IsNullOrWhiteSpace(txtPrice.Text))
                            txtPrice.Text = first.Value.Price.ToString("N2");
                    }
                }
                catch { }
            }
        }

        private void GoToConfirmPaymentForSelected()
        {
            if (dtgvInvoice.SelectedRows.Count == 0) return;

            var invId = dtgvInvoice.SelectedRows[0].Cells["inv_id"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(invId)) return;

            var host = this.FindForm() as AccountantForm;
            if (host != null)
            {
                host.ShowConfirmInvoice(invId);
            }
        }

        private void ShowInvoiceActionPopup()
        {
            if (dtgvInvoice.SelectedRows.Count == 0) return;
            var row = dtgvInvoice.SelectedRows[0];
            string status = row.Cells["inv_status"]?.Value?.ToString() ?? "";

            if (status.Equals("ชำระแล้ว", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("รายการนี้ชำระแล้ว", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Form popup = new Form();
            popup.Text = "เลือกการดำเนินการ";
            popup.StartPosition = FormStartPosition.CenterParent;
            popup.Size = new Size(400, 180);
            popup.FormBorderStyle = FormBorderStyle.FixedDialog;
            popup.MaximizeBox = false;
            popup.MinimizeBox = false;

            Label lbl = new Label()
            {
                Text = "คุณต้องการทำอะไรกับใบแจ้งหนี้นี้?",
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            Button btnPrint = new Button() { Text = "พิมพ์ใบแจ้งหนี้", Width = 120, Height = 35 };
            btnPrint.Click += (s, e) => { popup.Tag = "print"; popup.DialogResult = DialogResult.OK; };

            Button btnConfirm = new Button() { Text = "ยืนยันการรับชำระเงิน", Width = 150, Height = 35 };
            btnConfirm.Click += (s, e) => { popup.Tag = "confirm"; popup.DialogResult = DialogResult.OK; };

            Button btnCancel = new Button() { Text = "ยกเลิก", Width = 80, Height = 35 };
            btnCancel.Click += (s, e) => { popup.Tag = "cancel"; popup.DialogResult = DialogResult.Cancel; };

            FlowLayoutPanel panel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(20),
                AutoSize = true
            };
            panel.Controls.Add(btnPrint);
            panel.Controls.Add(btnConfirm);
            panel.Controls.Add(btnCancel);

            popup.Controls.Add(panel);
            popup.Controls.Add(lbl);

            if (popup.ShowDialog() == DialogResult.OK)
            {
                string choice = popup.Tag?.ToString();
                if (choice == "print")
                    PrintSelectedInvoiceFromGrid();
                else if (choice == "confirm")
                    GoToConfirmPaymentForSelected();
            }
        }

        private void ApplyDueDateFloor()
        {
            var inv = dtpInvDate.Value.Date;

            if (dtpDueDate.Value.Date < inv)
                dtpDueDate.Value = inv;

            if (dtpDueDate.MinDate != inv)
                dtpDueDate.MinDate = inv;
        }

        private void dtpInvDate_ValueChanged(object sender, EventArgs e)
        {
            ApplyDueDateFloor();
        }

        private void dtpDueDate_ValueChanged(object sender, EventArgs e)
        {
            var inv = dtpInvDate.Value.Date;
            if (dtpDueDate.Value.Date < inv)
            {
                dtpDueDate.Value = inv;
                MessageBox.Show("กำหนดชำระเงินต้องไม่ก่อนวันที่ออกใบแจ้งหนี้",
                    "วันที่ไม่ถูกต้อง", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
