using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;                  // <— added
using MySql.Data.MySqlClient;               // <— added
using JRSApplication.Data_Access_Layer;
using System.Globalization;
using System.Linq;
using System.Drawing.Printing;

namespace JRSApplication.Accountant
{
    public partial class Receipt : UserControl
    {
        private InvoiceDAL invoiceDAL = new InvoiceDAL();
        private ReceiptDAL receiptDAL = new ReceiptDAL();
        private string currentInvId;


        // for GenerateNextReceiptId()
        private readonly string _cs = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public Receipt()
        {
            InitializeComponent();
            CustomizeDataGridView();
        }

        private void CustomizeDataGridView()
        {
            dtgvInvoice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvInvoice.MultiSelect = false;
            dtgvInvoice.BorderStyle = BorderStyle.None;
            dtgvInvoice.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvInvoice.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvInvoice.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvInvoice.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvInvoice.BackgroundColor = Color.White;

            dtgvInvoice.EnableHeadersVisualStyles = false;
            dtgvInvoice.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvInvoice.ColumnHeadersHeight = 30;

            dtgvInvoice.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvInvoice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvInvoice.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            dtgvInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvInvoice.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvInvoice.RowTemplate.Height = 30;
            dtgvInvoice.GridColor = Color.LightGray;
            dtgvInvoice.RowHeadersVisible = false;
            dtgvInvoice.ReadOnly = true;
            dtgvInvoice.AllowUserToAddRows = false;
            dtgvInvoice.AllowUserToResizeRows = false;
        }

        // 🔹 UI-side generator: next REC_XXXX (no new DAL methods)
        private string GenerateNextReceiptId()
        {
            using (var conn = new MySqlConnection(_cs))
            {
                conn.Open();

                const string sql = @"
            SELECT COALESCE(
                       MAX(CAST(SUBSTRING(receipt_id, 5) AS UNSIGNED)), 0
                   )
            FROM receipt
            WHERE receipt_id LIKE 'REC\_%' ESCAPE '\\';";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    object o = cmd.ExecuteScalar();
                    int last = 0;
                    if (o != null && o != DBNull.Value)
                        int.TryParse(o.ToString(), out last);

                    return $"REC_{(last + 1):D4}";
                }
            }
        }
            

        private void dtgvInvoice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dtgvInvoice.Rows[e.RowIndex];

            // inv_id is a string (e.g. "INV_0005")
            currentInvId = row.Cells["inv_id"]?.Value?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(currentInvId))
            {
                MessageBox.Show("ไม่พบเลขที่ใบแจ้งหนี้ (inv_id)", "แจ้งเตือน",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🧾 ชำระเงิน/หัวตาราง
            txtInvNo.Text = row.Cells["inv_id"]?.Value?.ToString() ?? "";
            txtEmpName.Text = row.Cells["emp_fullname"]?.Value?.ToString() ?? "";
            dtpPaidDate.Value = row.Cells["paid_date"]?.Value is DateTime dt
                                ? dt
                                : DateTime.Now;
            textBox1.Text = row.Cells["inv_method"]?.Value?.ToString();

            // 👤 ลูกค้า
            txtCusName.Text = row.Cells["cus_fullname"]?.Value?.ToString();
            txtCusIDCard.Text = row.Cells["cus_id_card"]?.Value?.ToString();
            txtCusAddress.Text = row.Cells["cus_address"]?.Value?.ToString();

            // 🏗️ โครงการ
            txtContractNo.Text = row.Cells["pro_id"]?.Value?.ToString();
            txtProName.Text = row.Cells["pro_name"]?.Value?.ToString();
            txtPhaseID.Text = row.Cells["phase_no"]?.Value?.ToString()
                   ?? row.Cells["phase_id"]?.Value?.ToString()
                   ?? "";

            // 🔸 Auto-generate receipt_id into the textbox and lock it
            try
            {
                string nextRec = GenerateNextReceiptId();
                txtReceiptNo.Text = nextRec;
                txtReceiptNo.ReadOnly = true;     // or txtReceiptNo.Enabled = false;
            }
            catch (Exception ex)
            {
                txtReceiptNo.Text = "";
                MessageBox.Show("สร้างเลขที่ใบเสร็จไม่สำเร็จ: " + ex.Message,
                    "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // เตรียม columns และโหลดรายละเอียดจาก DB ด้วย inv_id (string)
            SetupReceiptDetailGrid();
            LoadInvoiceDetailForReceipt(currentInvId);
        }

        private void LoadInvoiceDetailForReceipt(string invId)
        {
            SetupReceiptDetailGrid(); // Prepare the grid

            // Keep your existing call signature (string invId)
            DataTable dt = InvoiceDAL.GetInvoiceDetail(invId);
            dtgvReceiptDetail.DataSource = dt;
        }

        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Project");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                string selectedProjectId = searchForm.SelectedID;
                LoadInvoiceDataByProject(selectedProjectId);
            }
        }

        private void LoadInvoiceDataByProject(string projectId)
        {
            DataTable dt = invoiceDAL.GetInvoicesByProjectId(projectId);
            dtgvInvoice.AutoGenerateColumns = true;
            dtgvInvoice.DataSource = dt;
        }

        private void SetupReceiptDetailGrid()
        {
            dtgvReceiptDetail.Columns.Clear();
            dtgvReceiptDetail.AutoGenerateColumns = false;
            dtgvReceiptDetail.RowHeadersVisible = false;
            dtgvReceiptDetail.AllowUserToAddRows = false;
            dtgvReceiptDetail.ReadOnly = true;
            dtgvReceiptDetail.BackgroundColor = Color.White;
            dtgvReceiptDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dtgvReceiptDetail.GridColor = Color.LightGray;
            dtgvReceiptDetail.DefaultCellStyle.Font = new Font("Tahoma", 11);
            dtgvReceiptDetail.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            dtgvReceiptDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvReceiptDetail.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvReceiptDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dtgvReceiptDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "No",
                HeaderText = "No",
                Width = 50
            });

            dtgvReceiptDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "inv_detail",
                HeaderText = "รายละเอียด",
                DataPropertyName = "inv_detail",
                Width = 300,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });

            dtgvReceiptDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "inv_quantity",
                HeaderText = "จำนวน",
                DataPropertyName = "inv_quantity",
                Width = 80
            });

            dtgvReceiptDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "inv_price",
                HeaderText = "ราคา",
                DataPropertyName = "inv_price",
                Width = 100
            });

            dtgvReceiptDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "subtotal",
                HeaderText = "ราคารวม",
                Width = 120
            });

            dtgvReceiptDetail.CellFormatting += dtgvReceiptDetail_CellFormatting;
        }

        private void dtgvReceiptDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dtgvReceiptDetail.Rows[e.RowIndex];

            if (dtgvReceiptDetail.Columns[e.ColumnIndex].Name == "subtotal")
            {
                decimal price = 0m;
                decimal.TryParse(Convert.ToString(row.Cells["inv_price"]?.Value), out price);

                decimal qty = 1m; // fallback 1 if not numeric
                if (decimal.TryParse(Convert.ToString(row.Cells["inv_quantity"]?.Value), out var q))
                    qty = q;

                e.Value = (qty * price).ToString("N2");
                e.FormattingApplied = true;
                return;
            }

            if (dtgvReceiptDetail.Columns[e.ColumnIndex].Name == "No")
            {
                e.Value = (e.RowIndex + 1).ToString();
                e.FormattingApplied = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(currentInvId))
            {
                MessageBox.Show("กรุณาเลือกใบแจ้งหนี้ก่อน", "แจ้งเตือน",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get or generate receipt_id
            string receiptId = txtReceiptNo.Text.Trim();
            if (string.IsNullOrWhiteSpace(receiptId))
            {
                try
                {
                    receiptId = GenerateNextReceiptId();
                    txtReceiptNo.Text = receiptId;
                    txtReceiptNo.ReadOnly = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("สร้างเลขที่ใบเสร็จไม่สำเร็จ: " + ex.Message,
                        "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            DateTime receiptDate = dtpReceiptDate.Value;
            string remark = txtReason.Text.Trim();

            try
            {
                // InsertReceipt now expects receipt_id (we already modified DAL)
                int result = receiptDAL.InsertReceipt(receiptId, receiptDate, remark, currentInvId);

                if (result > 0)
                {
                    MessageBox.Show("บันทึกใบเสร็จรับเงินสำเร็จ", "สำเร็จ",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ไม่สามารถบันทึกข้อมูลได้", "ผิดพลาด",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (MySqlException ex) when (ex.Message.Contains("Duplicate"))
            {
                // Rare edge case if two users saved concurrently
                MessageBox.Show("เลขที่ใบเสร็จนี้ถูกใช้งานแล้ว กำลังสร้างใหม่ให้...",
                    "ข้อมูลซ้ำ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                string fresh = GenerateNextReceiptId();
                txtReceiptNo.Text = fresh; txtReceiptNo.ReadOnly = true;

                // Try once more
                int retry = receiptDAL.InsertReceipt(fresh, receiptDate, remark, currentInvId);
                if (retry > 0)
                    MessageBox.Show("บันทึกใบเสร็จรับเงินสำเร็จ", "สำเร็จ",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("ไม่สามารถบันทึกข้อมูลได้", "ผิดพลาด",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "ผิดพลาด",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private static decimal ParseMoney(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0m;

            if (decimal.TryParse(s,
                NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                CultureInfo.CurrentCulture, out var v)) return v;

            if (decimal.TryParse(s,
                NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                CultureInfo.InvariantCulture, out v)) return v;

            // last resort: keep only digits/.,,
            var cleaned = new string(s.Where(ch => char.IsDigit(ch) || ch == '.' || ch == ',').ToArray());
            return decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out v) ? v : 0m;
        }

        // Returns the first row from dtgvReceiptDetail as (detail, qtyText, priceText)
        private (string detail, string qtyText, string priceText)? GetFirstDetailFromGrid()
        {
            foreach (DataGridViewRow row in dtgvReceiptDetail.Rows)
            {
                if (row.IsNewRow) continue;

                string detail = Convert.ToString(row.Cells["inv_detail"]?.Value) ?? "";
                string qty = Convert.ToString(row.Cells["inv_quantity"]?.Value) ?? "";
                string price = Convert.ToString(row.Cells["inv_price"]?.Value) ?? "";
                return (detail, qty, price);
            }
            return null;
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            string receiptNo = receiptDAL.GetReceiptNoByInvId(currentInvId);
            if (string.IsNullOrWhiteSpace(receiptNo))
            {
                MessageBox.Show("ยังไม่พบเลขที่ใบเสร็จสำหรับใบแจ้งหนี้นี้ กรุณาบันทึกข้อมูลก่อน",
                    "ไม่พบข้อมูล", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal phaseBudget = 0m;
            string phaseDetail = "งบประมาณของเฟสงาน";
            string paidDate = "";
            string remark = receiptDAL.GetReceiptRemarkByInvId(currentInvId);

            try
            {
                DataTable headTable = invoiceDAL.GetInvoiceID(currentInvId);
                if (headTable.Rows.Count > 0)
                {
                    var r = headTable.Rows[0];
                    if (headTable.Columns.Contains("phase_budget") && r["phase_budget"] != DBNull.Value)
                        phaseBudget = Convert.ToDecimal(r["phase_budget"]);

                    if (headTable.Columns.Contains("phase_detail") && r["phase_detail"] != DBNull.Value)
                        phaseDetail = Convert.ToString(r["phase_detail"]);

                    if (headTable.Columns.Contains("paid_date") && r["paid_date"] != DBNull.Value)
                        paidDate = Convert.ToDateTime(r["paid_date"]).ToString("d/M/yyyy");
                }
            }
            catch { }

            // --- Build ONE DataTable for RDLC ---
            var table = new DataTable();
            table.Columns.Add("receipt_id");
            table.Columns.Add("receipt_date");
            table.Columns.Add("inv_id");
            table.Columns.Add("cus_fullname");
            table.Columns.Add("cus_address");
            table.Columns.Add("pro_name");
            table.Columns.Add("phase_detail");
            table.Columns.Add("phase_budget", typeof(decimal));
            table.Columns.Add("inv_remark");
            table.Columns.Add("subtotal", typeof(decimal));
            table.Columns.Add("vat", typeof(decimal));
            table.Columns.Add("grand_total", typeof(decimal));
            table.Columns.Add("ToDate");

            // ✅ Add the item columns too (so RDLC finds them)
            table.Columns.Add("inv_detail");
            table.Columns.Add("inv_quantity");
            table.Columns.Add("inv_price");

            // --- Calculate totals ---
            decimal subtotal = phaseBudget;
            string invDetail = null, invQty = null, invPrice = null;

            var first = GetFirstDetailFromGrid();
            if (first.HasValue)
            {
                invDetail = first.Value.detail ?? "";
                invQty = string.IsNullOrWhiteSpace(first.Value.qtyText) ? "1" : first.Value.qtyText;
                invPrice = first.Value.priceText ?? "0.00";

                if (decimal.TryParse(first.Value.priceText, out decimal extraPrice))
                    subtotal += extraPrice;
            }

            decimal vat = Math.Round(subtotal * 0.07m, 2, MidpointRounding.AwayFromZero);
            decimal grand = subtotal + vat;

            var thaiCulture = new System.Globalization.CultureInfo("th-TH");
            string toDate = DateTime.Now.ToString("d MMMM yyyy", thaiCulture);
            // --- Add row ---
            table.Rows.Add(
                receiptNo,
                paidDate,
                txtInvNo.Text.Trim(),
                txtCusName.Text,
                txtCusAddress.Text,
                txtProName.Text,
                phaseDetail,
                phaseBudget,
                remark,
                subtotal,
                vat,
                grand,
                toDate, 
                invDetail ?? DBNull.Value.ToString(),
                invQty ?? DBNull.Value.ToString(),
                invPrice ?? DBNull.Value.ToString()
            );

            // --- Show RDLC ---
            var frm = new ReceiptPrintRDLC(table);
            frm.ShowDialog();
        }




    }
}
