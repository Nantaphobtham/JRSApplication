using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JRSApplication.Components;
using JRSApplication.Data_Access_Layer;

namespace JRSApplication
{
    public partial class CheckProjectPayments : UserControl
    {
        public CheckProjectPayments()
        {
            InitializeComponent();
        }
    

    private void btnSearchProject_Click(object sender, EventArgs e)
        {
            string keyword = txtProjectID.Text.Trim(); // หรือจะใช้ TextBox ชื่ออื่นก็ได้

            SearchService service = new SearchService();
            DataTable dt = service.SearchData("Project", keyword);

            if (dt.Rows.Count > 0)
            {
                DataRow projectRow = dt.Rows[0];

                txtProjectID.Text = projectRow["รหัสโครงการ"].ToString();
                txtProjectname.Text = projectRow["ชื่อโครงการ"].ToString();
                txtCustomername.Text = projectRow["ลูกค้า"].ToString();
                txtProjectManager.Text = projectRow["พนักงานดูแล"].ToString();
            }
            else
            {
                MessageBox.Show("ไม่พบข้อมูลโครงการ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnSearchInvoice_Click(object sender, EventArgs e)
        {
            // เปิดฟอร์มค้นหา Invoice
            SearchForm form = new SearchForm("InvoiceFull");

            if (form.ShowDialog() == DialogResult.OK)
            {
                // ดึงข้อมูลที่เลือกจาก SearchForm
                string invoiceNo = form.SelectedID;               // เลขที่ใบแจ้งหนี้
                string customerName = form.SelectedName;          // ชื่อลูกค้า
                string idCard = form.SelectedLastName;            // เลขบัตรประชาชน
                string address = form.SelectedIDCardOrRole;       // ที่อยู่
                string projectId = form.SelectedPhone;            // รหัสโครงการ
                string projectName = form.SelectedEmail;          // ชื่อโครงการ

                // 👇 ตั้งค่า textbox ของคุณตามนี้ (แก้ชื่อตามที่คุณตั้งใน Designer)
                //txtInvoiceNo.Text = invoiceNo;
                txtCustomerName2.Text = customerName;
                txtIDCard.Text = idCard;
                txtAddress.Text = address;
                txtProjectID.Text = projectId;
                //txtProjectName.Text = projectName;

                // ถ้ามี textbox อื่นๆ เช่น วันที่ หรือสถานะ สามารถเพิ่มได้
                // txtStatus.Text = form.YourAdditionalField;
                // txtPaymentMethod.Text = form.YourAdditionalField2;
            }
        }




    }
}
