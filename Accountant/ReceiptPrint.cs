using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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

        // ✅ ซ้ายบน: ข้อมูลลูกค้า
        public void SetCustomerBox(string name, string address, string invNo)
        {
            string content = $"ลูกค้า: {name}\nที่อยู่: {address}\nใบแจ้งหนี้: {invNo}";

            Label lbl = new Label
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
        public void SetInvoiceDetails(DataTable invoiceDetailTable)
        {
            tblReceiptItems.Controls.Clear();
            tblReceiptItems.RowStyles.Clear();
            tblReceiptItems.RowCount = 0;

            // Header row
            string[] headers = { "ลำดับ", "รายละเอียด", "จำนวน", "ราคา", "ราคารวม" };
            for (int i = 0; i < headers.Length; i++)
            {
                AddCell(tblReceiptItems, headers[i], i, 0, true);
            }

            int row = 1;
            foreach (DataRow dr in invoiceDetailTable.Rows)
            {
                string detail = dr["inv_detail"].ToString();
                decimal quantity = Convert.ToDecimal(dr["inv_quantity"]);
                decimal price = Convert.ToDecimal(dr["inv_price"]);
                decimal total = quantity * price;

                AddCell(tblReceiptItems, row.ToString(), 0, row);
                AddCell(tblReceiptItems, detail, 1, row);
                AddCell(tblReceiptItems, quantity.ToString(), 2, row);
                AddCell(tblReceiptItems, price.ToString("N2"), 3, row);
                AddCell(tblReceiptItems, total.ToString("N2"), 4, row);

                row++;
            }
        }

        // ✅ สรุปรายการด้านขวาล่าง - ใส่ข้อความบาทไทยใต้จำนวนเงินใน cell เดียวกัน
        public void SetReceiptSummary(decimal subtotal, decimal vat, decimal grandTotal)
        {
            tblBottomRight.Controls.Clear();
            tblBottomRight.RowStyles.Clear();
            tblBottomRight.RowCount = 0;

            tblBottomRight.ColumnCount = 2;
            tblBottomRight.ColumnStyles.Clear();
            tblBottomRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblBottomRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            AddRowToSummary(tblBottomRight, "รวมเป็นเงิน", subtotal.ToString("N2") + " บาท", 0);
            AddRowToSummary(tblBottomRight, "จำนวนภาษีมูลค่าเพิ่ม 7%", vat.ToString("N2") + " บาท", 1);
            AddRowToSummary(tblBottomRight, "จำนวนเงินทั้งสิ้น", $"( {ConvertToThaiBahtText(grandTotal)} )", 2, isBahtText: true);
        } 



        // ✅ เพิ่ม row โดยจัดชิดบนเพื่อรองรับข้อความหลายบรรทัดใน cell เดียว
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
                TextAlign = isBahtText ? ContentAlignment.MiddleRight : ContentAlignment.MiddleRight,
                Font = new Font("Tahoma", 10, isBahtText ? FontStyle.Italic : FontStyle.Regular),
                Padding = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            tbl.Controls.Add(lblLeft, 0, row);
            tbl.Controls.Add(lblRight, 1, row);
        }

        // ✅ แปลงตัวเลขเป็นข้อความบาทไทย
        private string ConvertToThaiBahtText(decimal amount)
        {
            // คุณต้องมีคลาส ThaiBahtTextConverter เองหรือเขียนเพิ่ม
            return new ThaiBahtTextConverter().GetBahtText(amount);
        }


        private void AddCell(TableLayoutPanel tbl, string text, int col, int row, bool bold = false)
        {
            Label lbl = new Label
            {
                Text = text,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 10, bold ? FontStyle.Bold : FontStyle.Regular),
                Padding = new Padding(2),
                Margin = new Padding(0),
                BorderStyle = BorderStyle.FixedSingle
            };

            if (tbl.RowCount <= row)
            {
                tbl.RowCount = row + 1;
                tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
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




    }
}
