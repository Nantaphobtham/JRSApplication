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


        private void InvoicePrintRDLC_Load(object sender, EventArgs e)
        {
            string reportPath = System.IO.Path.Combine(
                Application.StartupPath, "Accountant\\InvoiceReport.rdlc");

            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();

            // ✅ ใช้ชื่อ DataSet เดียวกับใบเสร็จได้เลย
            reportViewer1.LocalReport.DataSources.Add(
                new ReportDataSource("ReceiptDataSet", reportTable));

            reportViewer1.RefreshReport();
        }

    }
}
