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
        public string SearchMode { get; set; }  // "Customer" หรือ "Employee"
        public string SelectedID { get; private set; } = "";
        public string SelectedName { get; private set; } = "";
        public string SelectedLastName { get; private set; } = "";
        public string SelectedIDCardOrRole { get; private set; } = "";
        public string SelectedPhone { get; private set; } = "";
        public string SelectedEmail { get; private set; } = "";

        public SearchForm(string mode)
        {
            InitializeComponent();
            SearchMode = mode;
            lblTitle.Text = SearchMode == "Customer" ? "ค้นหาลูกค้า" : "ค้นหาพนักงาน"; // ✅ เปลี่ยนชื่อหัวข้อ
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
            if (dtgvAlldata.SelectedRows.Count > 0) // ✅ ตรวจสอบว่ามีการเลือกแถว
            {
                DataGridViewRow selectedRow = dtgvAlldata.SelectedRows[0]; // ✅ ดึงแถวที่เลือก

                SelectedID = selectedRow.Cells["ID"].Value?.ToString() ?? "";
                SelectedName = selectedRow.Cells["ชื่อ"].Value?.ToString() ?? "";
                SelectedLastName = selectedRow.Cells["นามสกุล"].Value?.ToString() ?? "";

                if (SearchMode == "Customer")
                {
                    SelectedIDCardOrRole = selectedRow.Cells["เลขบัตรประชาชน"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["เบอร์โทร"].Value?.ToString() ?? "";
                    SelectedEmail = selectedRow.Cells["อีเมล"].Value?.ToString() ?? "";
                }
                else if (SearchMode == "Employee")
                {
                    SelectedIDCardOrRole = selectedRow.Cells["ตำแหน่ง"].Value?.ToString() ?? "";
                }

                this.DialogResult = DialogResult.OK; // ✅ ส่งผลลัพธ์กลับ
                this.Close(); // ✅ ปิดฟอร์ม
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
