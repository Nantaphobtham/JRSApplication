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

            // Show system print dialog
            PrintDialog dlg = new PrintDialog();
            dlg.Document = pd;

            // If user presses OK
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
                    pd.Print(); // trigger print to selected printer
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Printing failed: " + ex.Message);
                }
            }
        }




    }
}
