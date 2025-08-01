using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class SearchForm : Form
    {
        private SearchService searchService = new SearchService();
        public string SearchMode { get; set; }  // "Customer" หรือ "Employee" หรือ "Supplier" หรือ "Project" หรือ "Invoice"
        public string SelectedID { get; private set; } = "";
        public string SelectedName { get; private set; } = "";
        public string SelectedContract { get; private set; } = "";
        public string SelectedLastName { get; private set; } = "";
        public string SelectedIDCardOrRole { get; private set; } = "";
        public string SelectedPhone { get; private set; } = "";
        public string SelectedEmail { get; private set; } = "";
        public string SelectedCusID { get; private set; } = "";
        private string optionalProjectId = null;


        public SearchForm(string mode, string optionalProjectId = null)
        {
            InitializeComponent();
            this.SearchMode = mode;
            this.optionalProjectId = optionalProjectId;

            // ✅ ตั้งชื่อหัวเรื่อง
            if (SearchMode == "Customer")
                lblTitle.Text = "ค้นหาลูกค้า";
            else if (SearchMode == "Employee")
                lblTitle.Text = "ค้นหาพนักงาน";
            else if (SearchMode == "Supplier")
                lblTitle.Text = "ค้นหาซัพพลายเออร์";
            else if (SearchMode == "Project")
                lblTitle.Text = "ค้นหาโครงการ";
            else if (SearchMode == "Invoice")
                lblTitle.Text = "ค้นหาใบแจ้งหนี้";
            else if (SearchMode == "UnpaidInvoiceByProject")
                lblTitle.Text = "เลือกใบแจ้งหนี้ที่ยังไม่ชำระ";
            else
                lblTitle.Text = "ค้นหา";

            // ✅ โหลดข้อมูล: กรณีพิเศษใช้ projectId
            if (SearchMode == "UnpaidInvoiceByProject")
                LoadSearchData(optionalProjectId ?? "");
            else
                LoadSearchData("");  // default

            CustomizeDataGridViewAlldata(); // ✅ ปรับแต่ง DataGridView
        }
        private void LoadSearchData(string keywordOrProjectId)
        {
            // For unpaid invoices by project, we use the project ID as input
            if (SearchMode == "UnpaidInvoiceByProject")
                dtgvAlldata.DataSource = searchService.SearchData(SearchMode, keywordOrProjectId);
            else
                dtgvAlldata.DataSource = searchService.SearchData(SearchMode, keywordOrProjectId);
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadSearchData(txtSearch.Text);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dtgvAlldata.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dtgvAlldata.SelectedRows[0];

                if (SearchMode == "Customer")
                {
                    SelectedID = selectedRow.Cells["ID"].Value?.ToString() ?? "";
                    SelectedName = selectedRow.Cells["ชื่อ"].Value?.ToString() ?? "";
                    SelectedLastName = selectedRow.Cells["นามสกุล"].Value?.ToString() ?? "";
                    SelectedIDCardOrRole = selectedRow.Cells["เลขบัตรประชาชน"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["เบอร์โทร"].Value?.ToString() ?? "";
                    SelectedEmail = selectedRow.Cells["อีเมล"].Value?.ToString() ?? "";
                }
                else if (SearchMode == "Employee")
                {
                    SelectedID = selectedRow.Cells["ID"].Value?.ToString() ?? "";
                    SelectedName = selectedRow.Cells["ชื่อ"].Value?.ToString() ?? "";
                    SelectedLastName = selectedRow.Cells["นามสกุล"].Value?.ToString() ?? "";
                    SelectedIDCardOrRole = selectedRow.Cells["ตำแหน่ง"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["เบอร์โทร"].Value?.ToString() ?? "";

                }
                else if (SearchMode == "Supplier")
                {
                    SelectedID = selectedRow.Cells["ID"].Value?.ToString() ?? "";
                    SelectedName = selectedRow.Cells["ชื่อบริษัท"].Value?.ToString() ?? "";
                    SelectedIDCardOrRole = selectedRow.Cells["เลขทะเบียนนิติบุคคล"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["เบอร์โทร"].Value?.ToString() ?? "";
                }
                else if (SearchMode == "Project")
                {
                    SelectedID = selectedRow.Cells["รหัสโครงการ"].Value?.ToString() ?? "";
                    SelectedContract = selectedRow.Cells["เลขที่สัญญา"].Value?.ToString() ?? "";
                    SelectedName = selectedRow.Cells["ชื่อโครงการ"].Value?.ToString() ?? "";
                    SelectedLastName = selectedRow.Cells["ลูกค้า"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["เบอร์โทร"].Value?.ToString() ?? "";    // ✅ ใช้ "เบอร์โทร"
                    SelectedEmail = selectedRow.Cells["อีเมล"].Value?.ToString() ?? "";        // ✅ ใช้ "อีเมล"
                    SelectedIDCardOrRole = selectedRow.Cells["พนักงานดูแล"].Value?.ToString() ?? "";
                    SelectedCusID = selectedRow.Cells["รหัสลูกค้า"].Value?.ToString() ?? "";
                }
                else if (SearchMode == "Invoice")
                {
                    SelectedID = selectedRow.Cells["เลขที่ใบแจ้งหนี้"].Value?.ToString() ?? "";
                    SelectedCusID = selectedRow.Cells["รหัสลูกค้า"].Value.ToString();  // ✅ ค่าของ cus_id
                    SelectedLastName = selectedRow.Cells["รหัสโครงการ"].Value?.ToString() ?? "";
                    SelectedIDCardOrRole = selectedRow.Cells["รหัสพนักงาน"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["วิธีชำระเงิน"].Value?.ToString() ?? "";
                    SelectedEmail = selectedRow.Cells["สถานะ"].Value?.ToString() ?? "";
                }
                else if (SearchMode == "UnpaidInvoiceByProject")
                {
                    SelectedID = selectedRow.Cells["inv_id"].Value?.ToString() ?? "";
                    SelectedCusID = selectedRow.Cells["cus_id"].Value?.ToString() ?? "";
                    SelectedLastName = selectedRow.Cells["pro_id"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["inv_method"].Value?.ToString() ?? "";
                    SelectedEmail = selectedRow.Cells["inv_status"].Value?.ToString() ?? "";
                }
                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            else
            {
                MessageBox.Show("กรุณาเลือกข้อมูลก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CustomizeDataGridViewAlldata()
        {
            dtgvAlldata.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvAlldata.MultiSelect = false;

            dtgvAlldata.BorderStyle = BorderStyle.None;
            dtgvAlldata.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvAlldata.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvAlldata.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvAlldata.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvAlldata.BackgroundColor = Color.White;

            dtgvAlldata.EnableHeadersVisualStyles = false;
            dtgvAlldata.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvAlldata.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvAlldata.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvAlldata.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvAlldata.ColumnHeadersHeight = 30;

            dtgvAlldata.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvAlldata.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvAlldata.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvAlldata.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            dtgvAlldata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvAlldata.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvAlldata.RowTemplate.Height = 30;

            dtgvAlldata.GridColor = Color.LightGray;
            dtgvAlldata.RowHeadersVisible = false;

            dtgvAlldata.ReadOnly = true;
            dtgvAlldata.AllowUserToAddRows = false;
            dtgvAlldata.AllowUserToResizeRows = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadSearchData(txtSearch.Text.Trim());
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
