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
        public string SearchMode { get; set; }  // "Customer" หรือ "Employee" หรือ "Supplier"
        public string SelectedID { get; private set; } = "";
        public string SelectedName { get; private set; } = "";
        public string SelectedContract { get; private set; } = "";
        public string SelectedLastName { get; private set; } = "";
        public string SelectedIDCardOrRole { get; private set; } = "";
        public string SelectedPhone { get; private set; } = "";
        public string SelectedEmail { get; private set; } = "";

        public SearchForm(string mode)
        {
            InitializeComponent();
            SearchMode = mode;
            if (SearchMode == "Customer")
                lblTitle.Text = "ค้นหาลูกค้า";
            else if (SearchMode == "Employee")
                lblTitle.Text = "ค้นหาพนักงาน";
            else if (SearchMode == "Supplier")
                lblTitle.Text = "ค้นหาซัพพลายเออร์";
            else if (SearchMode == "Project")
                lblTitle.Text = "ค้นหาโครงการ";
            else
                lblTitle.Text = "ค้นหา";
            LoadSearchData(""); // ✅ โหลดข้อมูลเริ่มต้น
            CustomizeDataGridViewAlldata(); // ✅ ปรับแต่ง DataGridView
        }

        // โหลดข้อมูลตามประเภทที่เลือก (ลูกค้าหรือพนักงาน)
        private void LoadSearchData(string keyword)
        {
            dtgvAlldata.DataSource = searchService.SearchData(SearchMode, keyword);
        }

        // ค้นหาแบบ Real-time เมื่อพิมพ์ใน TextBox
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadSearchData(txtSearch.Text);
        }

        // กดปุ่มยืนยัน (btnConfirm) เพื่อนำข้อมูลไปใช้
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dtgvAlldata.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dtgvAlldata.SelectedRows[0];

                // ✅ ย้ายเข้าแต่ละ if แทนที่จะเรียกแบบ hard-coded ก่อน if
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
                    SelectedIDCardOrRole = selectedRow.Cells["พนักงานดูแล"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["สถานที่"].Value?.ToString() ?? "";
                    SelectedEmail = selectedRow.Cells["งบประมาณ"].Value?.ToString() ?? "";
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
            dtgvAlldata.MultiSelect = false; // ✅ เลือกได้ทีละแถวเท่านั้น

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
            LoadSearchData(txtSearch.Text.Trim()); // ✅ ค้นหาตาม Keyword
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            //ปิดหน้าจอ
            this.Close();
        }

        
    }
}
