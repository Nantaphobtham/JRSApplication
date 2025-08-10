using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using JRSApplication.Components; // ✅ ใช้ Model ใหม่
using JRSApplication.Data_Access_Layer;
using Org.BouncyCastle.Asn1.Cmp;

namespace JRSApplication.Accountant
{
    public partial class Invoice : UserControl
    {
        public Invoice()
        {
            InitializeComponent();
            cmbPhase.SelectedIndexChanged += cmbPhase_SelectedIndexChanged;

            // ✅ ทำให้ panel1 ขยายเต็มขอบ UserControl ทุกด้าน
            panel1.Dock = DockStyle.Fill;

            CustomizeInvoiceGrid(); // ✅ เรียกตกแต่ง


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
            if (dtgvInvoice.Columns.Contains("phase_id")) dtgvInvoice.Columns["phase_id"].HeaderText = "เฟสที่";

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ เตรียม model หลัก
                InvoiceModel model = new InvoiceModel
                {
                    InvNo = txtInvNo.Text.Trim(),
                    InvDate = dtpInvDate.Value,
                    InvDueDate = dtpDueDate.Value,
                    CusId = txtCusID.Text.Trim(),
                    CusName = txtCusName.Text.Trim(),
                    ProId = txtProjectID.Text.Trim(),
                    ProNumber = txtContractNumber.Text.Trim(),
                    ProName = txtProjectName.Text.Trim(),
                    PhaseId = cmbPhase.SelectedValue?.ToString(),
                    PhaseBudget = txtPhaseBudget.Text.Trim(),
                    PhaseDetail = txtPhaseDetail.Text.Trim()
                };

                InvoiceDAL dal = new InvoiceDAL();
                int newId = dal.InsertInvoice(model);

                if (newId > 0)
                {
                    // ✅ ถ้า insert invoice สำเร็จ → ทำต่อ insert detail
                    string detail = txtDetail.Text.Trim();
                    int quantity = int.Parse(txtQuantity.Text.Trim());
                    decimal price = decimal.Parse(txtPrice.Text.Trim());
                    decimal vatRate = 7; // ปรับได้ถ้าต้องการ

                    InvoiceDetailDAL detailDal = new InvoiceDetailDAL();
                    detailDal.InsertInvoiceDetail(newId, detail, price, quantity, vatRate);

                    MessageBox.Show("บันทึกข้อมูลสำเร็จ! รหัสใบแจ้งหนี้: " + newId, "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ ล้างค่า
                    txtInvNo.Text = "";
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
                }
                else
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกใบแจ้งหนี้", "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            Form invoiceForm = new Form();
            invoiceForm.Text = "ใบแจ้งหนี้";
            invoiceForm.StartPosition = FormStartPosition.CenterScreen;
            invoiceForm.Size = new Size(1620, 1080); // consistent size
            invoiceForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            invoiceForm.MaximizeBox = false;

            InvoicePrint invoicePrint = new InvoicePrint();
            invoicePrint.Dock = DockStyle.Fill;

            // ✅ 1. Set Customer Box (Left)
            string customerName = txtCusName.Text;
            string customerId = txtCusID.Text;
            invoicePrint.SetCustomerBox(customerName, "รหัสลูกค้า: " + customerId);

            // ✅ 2. Set Header (Right)
            string invNo = txtInvNo.Text;
            string invDate = dtpInvDate.Value.ToString("dd/MM/yyyy");
            invoicePrint.SetInvoiceHeader(invNo, dtpInvDate.Value); // You may update SetInvoiceHeader to accept DateTime

            // ✅ 3. Prepare and Set Item Table
            DataTable invoiceItems = new DataTable();
            invoiceItems.Columns.Add("inv_detail");
            invoiceItems.Columns.Add("inv_quantity");
            invoiceItems.Columns.Add("inv_price");

            DataRow row = invoiceItems.NewRow();
            row["inv_detail"] = txtDetail.Text;
            row["inv_quantity"] = txtQuantity.Text;
            row["inv_price"] = txtPrice.Text;
            invoiceItems.Rows.Add(row);

            invoicePrint.SetInvoiceDetails(invoiceItems);

            // ✅ 4. Summary (calculate subtotal, VAT, grand total)
            decimal qty = decimal.TryParse(txtQuantity.Text, out var q) ? q : 0;
            decimal price = decimal.TryParse(txtPrice.Text, out var p) ? p : 0;
            decimal subtotal = qty * price;
            decimal vat = subtotal * 0.07m;
            decimal grandTotal = subtotal + vat;

            invoicePrint.SetInvoiceSummary(subtotal, vat, grandTotal);

            // ✅ 5. Remark (optional)
            invoicePrint.SetInvoiceRemark(txtPhaseDetail.Text);

            // ✅ Show
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
            if (string.IsNullOrEmpty(txtProjectID.Text))
            {
                MessageBox.Show("กรุณาเลือกโครงการก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var searchForm = new SearchForm("UnpaidInvoiceByProject", txtProjectID.Text))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    string selectedInvoiceId = searchForm.SelectedID;
                    InvoiceDAL invoiceDAL = new InvoiceDAL();
                    DataTable dt = invoiceDAL.GetInvoiceID(selectedInvoiceId); // ✅ Use new merged method

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];

                        // Header
                        txtInvNo.Text = row["inv_no"].ToString();
                        dtpInvDate.Value = Convert.ToDateTime(row["inv_date"]);
                        dtpDueDate.Value = Convert.ToDateTime(row["inv_duedate"]);

                        // Project
                        txtProjectID.Text = row["pro_number"].ToString();
                        txtContractNumber.Text = row["pro_id"].ToString();
                        txtProjectName.Text = row["pro_name"].ToString();
                        cmbPhase.SelectedValue = row["phase_id"];
                        txtPhaseBudget.Text = row["phase_budget"].ToString();
                        txtPhaseDetail.Text = row["phase_detail"].ToString();

                        // Customer
                        txtCusID.Text = row["cus_id"].ToString();
                        txtCusName.Text = row["cus_fullname"].ToString();

                        // Detail (first row)
                        txtDetail.Text = row["inv_detail"].ToString();
                        txtQuantity.Text = row["inv_quantity"].ToString();
                        txtPrice.Text = row["inv_price"].ToString();
                    }
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



    }
}
