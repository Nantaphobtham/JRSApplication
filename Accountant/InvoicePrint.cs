using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication.Accountant
{
    public partial class InvoicePrint : UserControl
    {

        public InvoicePrint()
        {
            InitializeComponent();
        }
        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            PrintDialog dlg = new PrintDialog();
            dlg.Document = pd;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pd.PrintPage += (s, args) =>
                {
                    using (Bitmap bmp = new Bitmap(this.Width, this.Height))
                    {
                        this.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                        args.Graphics.DrawImage(bmp, 0, 0);
                    }
                };

                try
                {
                    pd.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Printing failed: " + ex.Message);
                }
            }
        }
        public void SetCustomerBox(string customerName, string customerAddress)
        {
            string content = $"ลูกค้า: {customerName}\nที่อยู่: {customerAddress}";

            Label lbl = new Label
            {
                Text = content,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Tahoma", 10),
                Padding = new Padding(5),
                AutoSize = false
            };

            lblCustomer.Controls.Clear(); // Your left panel name
            lblCustomer.Controls.Add(lbl);
        }
        public void SetInvoiceHeader(string invNo, DateTime invDate)
        {
            AddLabelToTable(tblReceiptHeader, "lblInvNo", invNo, 1, 0);
            AddLabelToTable(tblReceiptHeader, "lblDate", invDate.ToString("dd/MM/yyyy"), 1, 1);
        }
        private void AddLabelToTable(TableLayoutPanel table, string name, string text, int col, int row)
        {
            var old = table.GetControlFromPosition(col, row);
            if (old != null)
                table.Controls.Remove(old);

            Label lbl = new Label
            {
                Name = name,
                Text = text,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Tahoma", 10),
                Padding = new Padding(5),
            };

            table.Controls.Add(lbl, col, row);
        }
        // REPLACE your SetInvoiceDetails with this version
        public void SetInvoiceDetails(DataTable invoiceDetailTable)
        {
            // we’ll rebuild the layout for consistent sizing
            tblReceiptItems.SuspendLayout();

            tblReceiptItems.Controls.Clear();
            tblReceiptItems.RowStyles.Clear();
            tblReceiptItems.ColumnStyles.Clear();
            tblReceiptItems.RowCount = 0;

            // 5 columns: No | Detail | Qty | Price | Total
            tblReceiptItems.ColumnCount = 5;
            tblReceiptItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f)); // No
            tblReceiptItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f)); // Detail
            tblReceiptItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f)); // Qty
            tblReceiptItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f)); // Price
            tblReceiptItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f)); // Total

            const int RH = 38;

            // Header
            AddCell(tblReceiptItems, "ลำดับ", 0, 0, true, ContentAlignment.MiddleCenter, RH);
            AddCell(tblReceiptItems, "รายการ", 1, 0, true, ContentAlignment.MiddleLeft, RH);
            AddCell(tblReceiptItems, "จำนวน", 2, 0, true, ContentAlignment.MiddleCenter, RH);
            AddCell(tblReceiptItems, "ราคา", 3, 0, true, ContentAlignment.MiddleRight, RH);
            AddCell(tblReceiptItems, "ราคารวม", 4, 0, true, ContentAlignment.MiddleRight, RH);

            int row = 1;

            foreach (DataRow dr in invoiceDetailTable.Rows)
            {
                string detail = dr["inv_detail"]?.ToString() ?? string.Empty;
                string qtyText = dr["inv_quantity"]?.ToString() ?? string.Empty;    // show as typed

                decimal price = 0m;
                decimal.TryParse(
                    CleanNumericString(dr["inv_price"]),
                    NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                    CultureInfo.CurrentCulture,
                    out price);

                // skip completely empty rows (optional)
                if (string.IsNullOrWhiteSpace(detail) &&
                    string.IsNullOrWhiteSpace(qtyText) &&
                    price == 0m)
                {
                    continue;
                }

                // table's "ราคารวม" = qty × price  (display wise)
                // if you still want "ignore qty" behavior, set total = price;
                decimal qtyForTotal = 1m;
                decimal.TryParse(CleanNumericString(qtyText), NumberStyles.Number, CultureInfo.CurrentCulture, out qtyForTotal);
                if (qtyForTotal <= 0) qtyForTotal = 1m;

                decimal total = price;

                AddCell(tblReceiptItems, row.ToString(), 0, row, false, ContentAlignment.MiddleCenter, RH);
                AddCell(tblReceiptItems, detail, 1, row, false, ContentAlignment.MiddleLeft, RH);
                AddCell(tblReceiptItems, qtyText, 2, row, false, ContentAlignment.MiddleCenter, RH);
                AddCell(tblReceiptItems, price.ToString("N2"), 3, row, false, ContentAlignment.MiddleRight, RH);
                AddCell(tblReceiptItems, total.ToString("N2"), 4, row, false, ContentAlignment.MiddleRight, RH);

                row++;
            }

            tblReceiptItems.ResumeLayout();
        }

        decimal SafeMoney(object v)
        {
            if (v == null || v == DBNull.Value) return 0m;
            var s = v.ToString();
            decimal d;
            if (decimal.TryParse(s, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, CultureInfo.CurrentCulture, out d)) return d;
            if (decimal.TryParse(s, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, CultureInfo.InvariantCulture, out d)) return d;
            return 0m;
        }
        // then: decimal price = SafeMoney(dr["inv_price"]);

        private string CleanNumericString(object value)
        {
            return value?.ToString().Replace(",", "").Replace("บาท", "").Trim() ?? "0";
        }

        // REPLACE your AddCell with this version
        private void AddCell(
            TableLayoutPanel tbl,
            string text,
            int col,
            int row,
            bool bold = false,
            ContentAlignment align = ContentAlignment.MiddleCenter,
            int rowHeight = 38)   // <- unified row height
        {
            var lbl = new Label
            {
                Text = text,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = align,
                Font = new Font("Tahoma", 10, bold ? FontStyle.Bold : FontStyle.Regular),
                Padding = new Padding(6),
                Margin = Padding.Empty,
                BorderStyle = BorderStyle.FixedSingle
            };

            while (tbl.RowCount <= row)
            {
                tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
                tbl.RowCount++;
            }

            tbl.Controls.Add(lbl, col, row);
        }


        // REPLACE your SetInvoiceSummary + AddRowToSummary
        public void SetInvoiceSummary(decimal subtotal, decimal vat, decimal grandTotal)
        {
            tblBottomRight.SuspendLayout();

            tblBottomRight.Controls.Clear();
            tblBottomRight.RowStyles.Clear();
            tblBottomRight.ColumnStyles.Clear();
            tblBottomRight.RowCount = 0;

            // Two columns: left label 60%, right value 40%
            tblBottomRight.ColumnCount = 2;
            tblBottomRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
            tblBottomRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));

            const int RH = 36;

            AddSummaryRow("รวมเป็นเงิน", subtotal.ToString("N2"), 0, RH, boldLeft: true);
            AddSummaryRow("จำนวนภาษีมูลค่าเพิ่ม 7%", vat.ToString("N2"), 1, RH);
            AddSummaryRow("จำนวนเงินทั้งสิ้น", grandTotal.ToString("N2"), 2, RH, boldLeft: true, boldRight: true);

            tblBottomRight.ResumeLayout();
        }

        private void AddSummaryRow(string leftText, string rightText, int row, int rowHeight, bool boldLeft = false, bool boldRight = false)
        {
            AddCell(tblBottomRight, leftText, 0, row, boldLeft, ContentAlignment.MiddleLeft, rowHeight);
            AddCell(tblBottomRight, rightText, 1, row, boldRight, ContentAlignment.MiddleRight, rowHeight);
        }

        private string ConvertToThaiBahtText(decimal amount)
        {
            return new ThaiBahtTextConverter().GetBahtText(amount);
        }

        private void AddRowToSummary(TableLayoutPanel tbl, string leftText, string rightText, int row, bool isBahtText = false)
        {
            tbl.RowCount++;
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33f));

            Label lblLeft = new Label
            {
                Text = leftText,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Tahoma", 10),
                Padding = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblRight = new Label
            {
                Text = rightText,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Tahoma", 10, isBahtText ? FontStyle.Italic : FontStyle.Regular),
                Padding = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            tbl.Controls.Add(lblLeft, 0, row);
            tbl.Controls.Add(lblRight, 1, row);
        }

        public void SetInvoiceRemark(string remark)
        {
            Label lbl = new Label
            {
                Text = "หมายเหตุ: " + remark,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Tahoma", 10),
                Padding = new Padding(10),
                AutoSize = false
            };

            tblLeftBottom.Controls.Clear(); // Left bottom panel
            tblLeftBottom.Controls.Add(lbl);
        }

        // ✅ ฟังก์ชันแสดงงวดงาน (phase_no)
        public void SetPhaseNo(string phaseNo)
        {
            // ล้างค่าเดิมใน tableTextPhase_no (ชื่อ TableLayoutPanel ของคุณ)
            tableTextPhase_no.Controls.Clear();

            Label lblPhase = new Label
            {
                Text = phaseNo,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 10, FontStyle.Bold),
                Padding = new Padding(5),
                AutoSize = false
            };

            tableTextPhase_no.Controls.Add(lblPhase, 0, 0);
        }



    }
}
