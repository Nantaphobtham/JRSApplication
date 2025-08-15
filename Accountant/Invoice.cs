using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using JRSApplication.Components; // ✅ ใช้ Model ใหม่
using JRSApplication.Data_Access_Layer;
using Org.BouncyCastle.Asn1.Cmp;
using System.Globalization;
using MySql.Data.MySqlClient;
using System.Linq;

namespace JRSApplication.Accountant
{
    public partial class Invoice : UserControl
    {
        public Invoice()
        {
            InitializeComponent();
            cmbPhase.SelectedIndexChanged += cmbPhase_SelectedIndexChanged;
            this.Load += Invoice_Load;

            // ✅ ทำให้ panel1 ขยายเต็มขอบ UserControl ทุกด้าน
            panel1.Dock = DockStyle.Fill;

            CustomizeInvoiceGrid(); // ✅ เรียกตกแต่ง

            // 👇 menu for grid
            BuildInvoiceGridMenu();

            // ensure row selects on right/left click for menu
            dtgvInvoice.CellMouseDown += dtgvInvoice_CellMouseDown;

        }


        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Project");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                txtProjectID.Text = searchForm.SelectedID;                  // รหัสโครงการ
                txtContractNumber.Text = searchForm.SelectedContract;       // เลขที่สัญญา
                txtProjectName.Text = searchForm.SelectedName;              // ชื่อโครงการ
                txtCusID.Text = searchForm.SelectedCusID;  // ✅ แก้ให้ใช้ cus_id โดยตรง                                                          
                txtCusName.Text = searchForm.SelectedLastName;             // ชื่อ-นามสกุล ลูกค้า


                LoadPhasesToComboBox(searchForm.SelectedID);

                // ✅ โหลดตาราง dtgvInvoice จาก project id
                LoadInvoiceTableByProject(searchForm.SelectedID);
            }
        }

        private void LoadInvoiceTableByProject(string projectId)
        {
            InvoiceDAL dal = new InvoiceDAL();
            DataTable dt = dal.GetAllInvoicesByProjectId(projectId); // ✅ ใช้เมธอดใหม่
            dtgvInvoice.DataSource = dt;

            // ✅ เปลี่ยนชื่อหัวคอลัมน์
            if (dtgvInvoice.Columns.Contains("inv_id")) dtgvInvoice.Columns["inv_id"].HeaderText = "รหัสใบแจ้งหนี้";
            if (dtgvInvoice.Columns.Contains("inv_no")) dtgvInvoice.Columns["inv_no"].HeaderText = "เลขที่ใบแจ้งหนี้";
            if (dtgvInvoice.Columns.Contains("inv_date")) dtgvInvoice.Columns["inv_date"].HeaderText = "วันที่ออก";
            if (dtgvInvoice.Columns.Contains("inv_duedate")) dtgvInvoice.Columns["inv_duedate"].HeaderText = "กำหนดชำระ";
            if (dtgvInvoice.Columns.Contains("inv_status")) dtgvInvoice.Columns["inv_status"].HeaderText = "สถานะใบแจ้งหนี้";
            if (dtgvInvoice.Columns.Contains("inv_method")) dtgvInvoice.Columns["inv_method"].HeaderText = "วิธีการชำระเงิน";
            if (dtgvInvoice.Columns.Contains("paid_date")) dtgvInvoice.Columns["paid_date"].HeaderText = "วันที่ชำระเงิน";
            if (dtgvInvoice.Columns.Contains("emp_fullname")) dtgvInvoice.Columns["emp_fullname"].HeaderText = "ผู้รับเงิน";
            if (dtgvInvoice.Columns.Contains("pro_id")) dtgvInvoice.Columns["pro_id"].HeaderText = "รหัสโครงการ";
            if (dtgvInvoice.Columns.Contains("pro_name")) dtgvInvoice.Columns["pro_name"].HeaderText = "ชื่อโครงการ";
            if (dtgvInvoice.Columns.Contains("cus_fullname")) dtgvInvoice.Columns["cus_fullname"].HeaderText = "ชื่อลูกค้า";
            if (dtgvInvoice.Columns.Contains("cus_id_card")) dtgvInvoice.Columns["cus_id_card"].HeaderText = "เลขบัตรประชาชน";
            if (dtgvInvoice.Columns.Contains("cus_address")) dtgvInvoice.Columns["cus_address"].HeaderText = "ที่อยู่ลูกค้า";
            if (dtgvInvoice.Columns.Contains("phase_no")) dtgvInvoice.Columns["phase_no"].HeaderText = "เฟสที่";
            if (dtgvInvoice.Columns.Contains("phase_id")) dtgvInvoice.Columns["phase_id"].Visible = false;  // keep id but hide it

            // ✅ ซ่อน emp_id ไม่ให้แสดง
            if (dtgvInvoice.Columns.Contains("emp_id"))
                dtgvInvoice.Columns["emp_id"].Visible = false;
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
                txtInvNo.Text = dal.PeekNextInvoiceId();   // shows next INV_000x
            }
            catch
            {
                txtInvNo.Text = "";
            }

            // if you clear the form and want to show the next number immediately:
            txtInvNo.Text = new InvoiceDAL().PeekNextInvoiceId();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // ต้องเลือกโครงการและลูกค้าก่อนบันทึก
            if (string.IsNullOrWhiteSpace(txtProjectID.Text) || string.IsNullOrWhiteSpace(txtCusID.Text))
            {
                MessageBox.Show("กรุณาเลือกโครงการและลูกค้าก่อนบันทึก", "แจ้งเตือน",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // ✅ เตรียม model หลัก (ไม่ต้องใช้ InvNo แล้ว)
                InvoiceModel model = new InvoiceModel
                {
                    InvDate = dtpInvDate.Value,
                    InvDueDate = dtpDueDate.Value,
                    CusId = txtCusID.Text.Trim(),
                    CusName = txtCusName.Text.Trim(),
                    ProId = txtProjectID.Text.Trim(),
                    ProNumber = txtContractNumber.Text.Trim(),
                    ProName = txtProjectName.Text.Trim(),
                    PhaseId = cmbPhase.SelectedValue?.ToString(),
                    PhaseBudget = txtPhaseBudget.Text.Trim(),
                    PhaseDetail = txtPhaseDetail.Text.Trim(),
                    InvRemark = txtRemark.Text.Trim(),
                    Quantity = txtQuantity.Text.Trim()
                };

                InvoiceDAL dal = new InvoiceDAL();

                // 🔁 ตอนนี้ InsertInvoice() จะคืนค่า inv_id แบบ "INV_0001"
                string newInvId = dal.InsertInvoice(model);

                if (!string.IsNullOrWhiteSpace(newInvId))
                {
                    // ✅ เก็บ inv_id ที่สร้างไว้เพื่อแสดง/ใช้งานต่อ
                    txtInvNo.Text = newInvId;

                    // ✅ จากฝั่งขวา: บันทึกรายการแรกลง invoice_detail (FK = inv_id แบบสตริง)
                    string detail = txtDetail.Text.Trim();
                    string Quantity = txtQuantity.Text.Trim();
                    if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price)) price = 0m;
                    decimal vatRate = 7m; // ปรับได้

                    InvoiceDetailDAL detailDal = new InvoiceDetailDAL();
                    detailDal.InsertInvoiceDetail(newInvId, detail, price, Quantity, vatRate);

                    MessageBox.Show("บันทึกข้อมูลสำเร็จ! เลขที่ใบแจ้งหนี้: " + newInvId,
                        "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ ล้างค่าอื่น ๆ แต่คงเลขที่ใบแจ้งหนี้ไว้ให้ผู้ใช้เห็น
                    // (ถ้าต้องการล้างด้วย ให้ใส่ txtInvNo.Text = "" เพิ่มเองได้)
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

            // Load address from DB so the customer box shows real address
            string custAddress = "";
            try
            {
                var invDal = new InvoiceDAL();
                var dtInv = invDal.GetInvoiceID(invId);
                if (dtInv.Rows.Count > 0 && dtInv.Columns.Contains("cus_address"))
                    custAddress = dtInv.Rows[0]["cus_address"]?.ToString() ?? "";
            }
            catch { /* ignore */ }

            // If right-panel fields are empty, pull first invoice_detail row
            string detailText = (txtDetail.Text ?? "").Trim();
            string quantityText = (txtQuantity.Text ?? "").Trim();   // keep AS-TYPED for display
            string priceText = (txtPrice.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(detailText) &&
                string.IsNullOrWhiteSpace(quantityText) &&
                string.IsNullOrWhiteSpace(priceText))
            {
                try
                {
                    var detDal = new InvoiceDetailDAL();
                    var first = detDal.GetFirstDetailForPrint(invId);
                    if (first.HasValue)
                    {
                        detailText = first.Value.Detail ?? "";
                        quantityText = first.Value.Quantity ?? "";
                        priceText = first.Value.Price.ToString("N2");
                    }
                }
                catch { /* ignore */ }
            }

            // UI scaffold
            Form invoiceForm = new Form
            {
                Text = "ใบแจ้งหนี้",
                StartPosition = FormStartPosition.CenterScreen,
                Size = new Size(1620, 1080),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false
            };

            var invoicePrint = new InvoicePrint { Dock = DockStyle.Fill };

            // Customer box + header (address fixed)
            string line2 = string.IsNullOrWhiteSpace(custAddress)
                ? ("รหัสลูกค้า: " + txtCusID.Text)
                : ("ที่อยู่: " + custAddress);
            invoicePrint.SetCustomerBox(txtCusName.Text, line2);
            invoicePrint.SetInvoiceHeader(invId, dtpInvDate.Value);

            // Helpers
            decimal ParseMoney(string s)
            {
                if (string.IsNullOrWhiteSpace(s)) return 0m;
                if (decimal.TryParse(s, NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                                     CultureInfo.CurrentCulture, out var v)) return v;
                if (decimal.TryParse(s, NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                                     CultureInfo.InvariantCulture, out v)) return v;
                return 0m;
            }

            // 3) Build items
            var items = new DataTable();
            items.Columns.Add("inv_detail");
            items.Columns.Add("inv_quantity");
            items.Columns.Add("inv_price");

            // ----- Right row (optional) -----
            bool hasRightRow = !string.IsNullOrWhiteSpace(detailText);
            decimal priceNum = ParseMoney(priceText);

            if (hasRightRow)
            {
                var rRight = items.NewRow();
                rRight["inv_detail"] = detailText;
                rRight["inv_quantity"] = quantityText;        // show EXACT text
                rRight["inv_price"] = priceNum.ToString("N2");
                items.Rows.Add(rRight);
            }

            // ----- Phase row (always) -----
            decimal phaseBudget = ParseMoney(txtPhaseBudget.Text);
            string phaseName = string.IsNullOrWhiteSpace(txtPhaseDetail.Text)
                ? "งบประมาณของเฟสงาน"
                : txtPhaseDetail.Text.Trim();

            var rPhase = items.NewRow();
            rPhase["inv_detail"] = phaseName;
            rPhase["inv_quantity"] = "1";                    // display only
            rPhase["inv_price"] = phaseBudget.ToString("N2");
            items.Rows.Add(rPhase);

            invoicePrint.SetInvoiceDetails(items);

            // 4) Summary: **ignore quantity** (treat each line as 1×price)
            decimal subtotal = 0m;
            if (hasRightRow) subtotal += priceNum;
            subtotal += phaseBudget;

            decimal vat = subtotal * 0.07m;
            decimal grandTotal = subtotal + vat;
            invoicePrint.SetInvoiceSummary(subtotal, vat, grandTotal);

            // 5) Remark
            invoicePrint.SetInvoiceRemark(txtRemark.Text);

            invoiceForm.Controls.Add(invoicePrint);
            invoiceForm.ShowDialog();
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
            // ต้องมีรหัสโครงการก่อน
            var projectId = txtProjectID.Text?.Trim();
            if (string.IsNullOrWhiteSpace(projectId))
            {
                MessageBox.Show("กรุณาเลือกโครงการก่อน", "แจ้งเตือน",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // เปิดหน้าค้นหา: ใบแจ้งหนี้สถานะ 'รอชำระเงิน' ของโครงการนี้
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

                // โหลดข้อมูลใบแจ้งหนี้ลงฟอร์ม
                try
                {
                    // ถ้าคอมโบเฟสยังไม่ได้โหลด (หรือโครงการเปลี่ยน) ให้โหลดเฟสของโครงการก่อน
                    if (cmbPhase.DataSource == null ||
                        (cmbPhase.DataSource as DataTable)?.Rows.Count == 0)
                    {
                        LoadPhasesToComboBox(projectId);
                    }

                    // โหลดหัวใบแจ้งหนี้ + รายการ มาใส่กล่อง (ใช้เมธอดที่คุณมีอยู่)
                    LoadInvoiceById(selectedInvoiceId);

                    // ถ้าต้องการ…โฟกัสไปช่อง remark เพื่อแก้ไขต่อ
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

            // ✅ สีสลับแถว
            dtgvInvoice.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvInvoice.DefaultCellStyle.BackColor = Color.White;
            dtgvInvoice.DefaultCellStyle.ForeColor = Color.Black;

            // ✅ สีเมื่อเลือกแถว
            dtgvInvoice.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvInvoice.DefaultCellStyle.SelectionForeColor = Color.White;

            // ✅ ฟอนต์และการจัดข้อความภายในเซลล์
            dtgvInvoice.DefaultCellStyle.Font = new Font("Segoe UI", 12); // ขนาดใหญ่เหมือน ConfirmInvoice
            dtgvInvoice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvInvoice.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            // ✅ หัวตาราง
            dtgvInvoice.EnableHeadersVisualStyles = false;
            dtgvInvoice.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy; // ใช้สีเดียวกับ ConfirmInvoice
            dtgvInvoice.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvInvoice.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvInvoice.ColumnHeadersHeight = 30;

            // ✅ ขนาดแถว
            dtgvInvoice.RowTemplate.Height = 30;

            // ✅ การจัดขนาดคอลัมน์
            dtgvInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvInvoice.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvInvoice.RowHeadersVisible = false;
            dtgvInvoice.ReadOnly = true;
            dtgvInvoice.AllowUserToAddRows = false;
            dtgvInvoice.AllowUserToResizeRows = false;

            // ✅ สีเส้นตาราง
            dtgvInvoice.GridColor = Color.LightGray;
        }



        private ContextMenuStrip _invoiceMenu;

        private void BuildInvoiceGridMenu()
        {
            _invoiceMenu = new ContextMenuStrip();
            var miPrint = new ToolStripMenuItem("พิมพ์ใบแจ้งหนี้");
            miPrint.Click += (s, e) => PrintSelectedInvoiceFromGrid();
            _invoiceMenu.Items.Add(miPrint);

            // (ไว้ล่วงหน้า) เมนูไปหน้ายืนยันการรับชำระเงิน
            var miConfirm = new ToolStripMenuItem("ยืนยันการรับชำระเงิน");
            miConfirm.Click += (s, e) => GoToConfirmPaymentForSelected();
            _invoiceMenu.Items.Add(miConfirm);
        }

        private void dtgvInvoice_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // เลือกแถวก่อน
            dtgvInvoice.ClearSelection();
            dtgvInvoice.Rows[e.RowIndex].Selected = true;
            dtgvInvoice.CurrentCell = dtgvInvoice.Rows[e.RowIndex].Cells[e.ColumnIndex];

            // ตรวจสถานะ
            var row = dtgvInvoice.Rows[e.RowIndex];
            string status = row.Cells["inv_status"]?.Value?.ToString() ?? "";

            // ถ้า “ชำระแล้ว” ไม่ให้พิมพ์ (หรือจะซ่อนไว้ก็ได้)
            _invoiceMenu.Items[0].Enabled = !status.Equals("ชำระแล้ว", StringComparison.OrdinalIgnoreCase);

            // โชว์เมนู
            if (e.RowIndex >= 0 && e.Button == MouseButtons.Left)
            {
                dtgvInvoice.ClearSelection();
                dtgvInvoice.Rows[e.RowIndex].Selected = true;
                ShowInvoiceActionPopup();
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

            // 1) ดึง inv_id จากแถว
            string invId = row.Cells["inv_id"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(invId)) return;

            // 2) โหลดข้อมูลเต็มของใบแจ้งหนี้ (รวม phase_budget/phase_detail)
            LoadInvoiceById(invId);

            // 3) เรียกพิมพ์ (ใช้โค้ดปุ่มพิมพ์ที่คุณมีอยู่แล้ว)
            btnPrintInvoice_Click(null, EventArgs.Empty);
        }

        private void LoadInvoiceById(string invId)
        {
            var invoiceDAL = new InvoiceDAL();
            DataTable dt = invoiceDAL.GetInvoiceID(invId); // merged query

            if (dt.Rows.Count == 0) return;
            DataRow r = dt.Rows[0];

            string GetStr(string col)
                => (r.Table.Columns.Contains(col) && r[col] != DBNull.Value) ? r[col].ToString() : "";

            // Header
            txtInvNo.Text = r.Table.Columns.Contains("inv_no") && r["inv_no"] != DBNull.Value
                ? r["inv_no"].ToString()
                : GetStr("inv_id");

            if (r.Table.Columns.Contains("inv_date") && r["inv_date"] != DBNull.Value)
                dtpInvDate.Value = Convert.ToDateTime(r["inv_date"]);
            if (r.Table.Columns.Contains("inv_duedate") && r["inv_duedate"] != DBNull.Value)
                dtpDueDate.Value = Convert.ToDateTime(r["inv_duedate"]);

            // Project
            txtProjectID.Text = GetStr("pro_id");
            txtContractNumber.Text = GetStr("pro_number");
            txtProjectName.Text = GetStr("pro_name");

            // Phase (id + budget/detail)
            string phaseIdStr = GetStr("phase_id");
            if (!string.IsNullOrWhiteSpace(phaseIdStr))
            {
                try { cmbPhase.SelectedValue = phaseIdStr; } catch { /* ignore type mismatch */ }
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
                catch { /* ignore */ }
            }

            if (r.Table.Columns.Contains("phase_detail") && r["phase_detail"] != DBNull.Value)
                txtPhaseDetail.Text = r["phase_detail"].ToString();

            // Customer
            txtCusID.Text = GetStr("cus_id");
            txtCusName.Text = GetStr("cus_fullname");

            // Remark
            txtRemark.Text = GetStr("inv_remark");

            // Right-panel (detail/qty/price)
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

            // Fallback: if still empty, get first detail row from invoice_detail
            if (string.IsNullOrWhiteSpace(txtDetail.Text) &&
                string.IsNullOrWhiteSpace(txtPrice.Text) &&
                !string.IsNullOrWhiteSpace(invId))
            {
                try
                {
                    var dDal = new InvoiceDetailDAL();
                    var first = dDal.GetFirstDetailForPrint(invId); // see helper below
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
                catch { /* ignore */ }
            }
        }


        private void GoToConfirmPaymentForSelected()
        {
            if (dtgvInvoice.SelectedRows.Count == 0) return;
            var row = dtgvInvoice.SelectedRows[0];
            string invId = row.Cells["inv_id"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(invId)) return;

            // TODO: เปิดฟอร์ม/หน้า ยืนยันการรับชำระเงิน พร้อมส่ง invId ไป
            // new ConfirmPaymentForm(invId).ShowDialog();
            MessageBox.Show($"(ตัวอย่าง) ไปหน้ายืนยันการรับชำระเงินสำหรับใบแจ้งหนี้ #{invId}");
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

            // Create custom form
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


    }
}
