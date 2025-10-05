using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace JRSApplication.Accountant
{
    public partial class ReceiptPrintRDLC : Form
    {
        private DataTable reportTable;

        public ReceiptPrintRDLC(DataTable table)
        {
            InitializeComponent();
            reportTable = table;
        }

        private void ReceiptPrintRDLC_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.ReportPath = "Accountant\\ReceiptReport.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();

            // ✅ only one dataset
            reportViewer1.LocalReport.DataSources.Add(
                new ReportDataSource("ReceiptDataSet", reportTable));

            reportViewer1.RefreshReport();
        }
    }
}
