using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace JRSApplication.Accountant
{
    public partial class InvoicePrintRDLC : Form
    {
        private DataTable reportTable;
        public InvoicePrintRDLC(DataTable table)
        {
            InitializeComponent();
            reportTable = table;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            // 👉 point to your InvoiceReport.rdlc (new file you design for invoices)
            reportViewer1.LocalReport.ReportPath = "Accountant\\InvoiceReport.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();

            // ✅ Bind to same dataset name you designed in RDLC
            reportViewer1.LocalReport.DataSources.Add(
                new ReportDataSource("ReceiptDataSet", reportTable));

            reportViewer1.RefreshReport();
        }
    }
}
