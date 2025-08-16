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
    public partial class ReceiptPrint : UserControl
    {
        public ReceiptPrint()
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
        private decimal SafeParse(object value, decimal fallback = 0m)
        {
            var s = Convert.ToString(value);
            if (string.IsNullOrWhiteSpace(s)) return fallback;

            var cleaned = new string(s.Where(ch => char.IsDigit(ch) || ch == '.' || ch == ',').ToArray());
            if (string.IsNullOrWhiteSpace(cleaned)) return fallback;

            if (decimal.TryParse(cleaned,
                NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                CultureInfo.CurrentCulture, out var v)) return v;

            if (decimal.TryParse(cleaned,
                NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                CultureInfo.InvariantCulture, out v)) return v;

            return fallback;
        }

        // ✅ LEFT: customer box (name + address only)
        public void SetCustomerBox(string name, string address)
        {
            string content = $"ลูกค้า: {name}\nที่อยู่: {address}";

            var lbl = new Label
            {
                Text = content,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Tahoma", 10),
                Padding = new Padding(5),
                AutoSize = false
            };

            tblHeader.Controls.Clear();
            tblHeader.Controls.Add(lbl, 0, 0);
        }

        // keep compatibility but IGNORE invNo
        public void SetCustomerBox(string name, string address, string invNo)
            => SetCustomerBox(name, address);


        // ✅ ขวาบน: ReceiptNo, วันที่, InvoiceNo
        public void SetReceiptHeader(string receiptNo, string date, string invoiceNo)
        {
            AddLabelToTable(tblReceiptHeader, "lblReceiptNo", receiptNo, 1, 0);
            AddLabelToTable(tblReceiptHeader, "lblDate", date, 1, 1);
            AddLabelToTable(tblReceiptHeader, "lblInvoiceNo", invoiceNo, 1, 2);
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

        // ✅ กลาง: ตารางรายการสินค้า
        public void SetInvoiceDetails(DataTable items)
        {
            tblReceiptItems.SuspendLayout();

            tblReceiptItems.Controls.Clear();
            tblReceiptItems.ColumnStyles.Clear();
            tblReceiptItems.RowStyles.Clear();

            // 5 columns: No | Detail | Qty | Price | Total
            tblReceiptItems.ColumnCount = 5;
            tblReceiptItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f)); // No
            tblReceiptItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f)); // Detail
            tblReceiptItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f)); // Qty
            tblReceiptItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f)); // Price
            tblReceiptItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f)); // Total
            tblReceiptItems.RowCount = 0;

            const int RH = 38; // uniform row height

            // Header row (row 0)
            AddCell(tblReceiptItems, "ลำดับ", 0, 0, ContentAlignment.MiddleCenter, true, RH);
            AddCell(tblReceiptItems, "รายละเอียด", 1, 0, ContentAlignment.MiddleLeft, true, RH);
            AddCell(tblReceiptItems, "จำนวน", 2, 0, ContentAlignment.MiddleCenter, true, RH);
            AddCell(tblReceiptItems, "ราคา", 3, 0, ContentAlignment.MiddleRight, true, RH);
            AddCell(tblReceiptItems, "ราคารวม", 4, 0, ContentAlignment.MiddleRight, true, RH);

            // data rows start at 1
            int r = 1;
            foreach (DataRow dr in items.Rows)
            {
                string detail = Convert.ToString(dr["inv_detail"]);
                decimal qty = SafeParse(dr["inv_quantity"], 1m);
                decimal price = SafeParse(dr["inv_price"], 0m);
                decimal total = qty * price;

                AddCell(tblReceiptItems, r.ToString(), 0, r, ContentAlignment.MiddleCenter, false, RH);
                AddCell(tblReceiptItems, detail, 1, r, ContentAlignment.MiddleLeft, false, RH);
                AddCell(tblReceiptItems,
                        (qty == Math.Truncate(qty) ? ((int)qty).ToString() : qty.ToString("N2")),
                                                                               2, r, ContentAlignment.MiddleCenter, false, RH);
                AddCell(tblReceiptItems, price.ToString("N2"), 3, r, ContentAlignment.MiddleRight, false, RH);
                AddCell(tblReceiptItems, total.ToString("N2"), 4, r, ContentAlignment.MiddleRight, false, RH);
                r++;
            }

            tblReceiptItems.ResumeLayout();
        }


        // ✅ แปลงตัวเลขเป็นข้อความบาทไทย
        private string ConvertToThaiBahtText(decimal amount)
        {
            // คุณต้องมีคลาส ThaiBahtTextConverter เองหรือเขียนเพิ่ม
            return new ThaiBahtTextConverter().GetBahtText(amount);
        }


        // One place to create uniform bordered cells
        private void AddCell(
            TableLayoutPanel tbl,
            string text,
            int col,
            int row,
            ContentAlignment align = ContentAlignment.MiddleCenter,
            bool bold = false,
            int rowHeight = 38)   // <- uniform height here
        {
            var lbl = new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                AutoSize = false,
                TextAlign = align,
                Font = new Font("Tahoma", 10, bold ? FontStyle.Bold : FontStyle.Regular),
                Padding = new Padding(6),
                Margin = Padding.Empty,
                BorderStyle = BorderStyle.FixedSingle
            };

            // make sure the row exists and has a fixed height
            while (tbl.RowCount <= row)
            {
                tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
                tbl.RowCount++;
            }

            tbl.Controls.Add(lbl, col, row);
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

            tblLeftBottom.Controls.Clear(); // 🠔 Your left-bottom panel name
            tblLeftBottom.Controls.Add(lbl);
        }

        // ReceiptPrint.cs  (inside the ReceiptPrint user control class)
        public void SetReceiptSummary(decimal subtotal, decimal vat, decimal grandTotal)
        {
            tblBottomRight.SuspendLayout();

            tblBottomRight.Controls.Clear();
            tblBottomRight.ColumnStyles.Clear();
            tblBottomRight.RowStyles.Clear();

            tblBottomRight.ColumnCount = 2;
            tblBottomRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
            tblBottomRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));

            const int RH = 36;
            tblBottomRight.RowCount = 0;

            AddCell(tblBottomRight, "รวมเป็นเงิน", 0, 0, ContentAlignment.MiddleLeft, true, RH);
            AddCell(tblBottomRight, subtotal.ToString("N2"), 1, 0, ContentAlignment.MiddleRight, true, RH);

            AddCell(tblBottomRight, "จำนวนภาษีมูลค่าเพิ่ม 7%", 0, 1, ContentAlignment.MiddleLeft, false, RH);
            AddCell(tblBottomRight, vat.ToString("N2"), 1, 1, ContentAlignment.MiddleRight, false, RH);

            AddCell(tblBottomRight, "จำนวนเงินทั้งสิ้น", 0, 2, ContentAlignment.MiddleLeft, true, RH);
            AddCell(tblBottomRight, grandTotal.ToString("N2"), 1, 2, ContentAlignment.MiddleRight, true, RH);

            tblBottomRight.ResumeLayout();
        }

    }
}
