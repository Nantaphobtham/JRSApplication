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
        private readonly string phaseNo;

        public ReceiptPrintRDLC(DataTable table, string phaseNo)   // << เปลี่ยน ctor
        {
            InitializeComponent();
            this.reportTable = table;
            this.phaseNo = phaseNo ?? "";
        }

        private void ReceiptPrintRDLC_Load(object sender, EventArgs e)
        {
            reportViewer1.Reset();
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            var reportPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Accountant",
                "ReceiptReport.rdlc"
            );

            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(
                new ReportDataSource("ReceiptDataSet", reportTable));

            // 🔑 ส่งพารามิเตอร์ phase_no เข้า RDLC
            reportViewer1.LocalReport.SetParameters(new[]
            {
                new ReportParameter("phase_no", phaseNo)
            });

            reportViewer1.RefreshReport();
        }

    }
}
