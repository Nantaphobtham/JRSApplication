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
                SelectedID = dtgvAlldata.SelectedRows[0].Cells["ID"].Value.ToString();
                SelectedName = dtgvAlldata.SelectedRows[0].Cells["ชื่อ"].Value.ToString();
                SelectedLastName = dtgvAlldata.SelectedRows[0].Cells["นามสกุล"].Value.ToString();

                if (SearchMode == "Customer")
                {
                    SelectedIDCardOrRole = dtgvAlldata.SelectedRows[0].Cells["เลขบัตรประชาชน"].Value.ToString();
                    SelectedPhone = dtgvAlldata.SelectedRows[0].Cells["เบอร์โทร"].Value.ToString();
                    SelectedEmail = dtgvAlldata.SelectedRows[0].Cells["อีเมล"].Value.ToString();
                }
                else if (SearchMode == "Employee")
                {
                    SelectedIDCardOrRole = dtgvAlldata.SelectedRows[0].Cells["ตำแหน่ง"].Value.ToString();
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("กรุณาเลือกข้อมูลก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
