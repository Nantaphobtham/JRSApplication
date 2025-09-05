using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using JRSApplication.Accountant;

namespace JRSApplication
{
    public partial class AccountantForm : Form
    {
        private readonly string fullName;
        private readonly string role;
        private readonly string empId;

        public AccountantForm(string fullName, string role, string empId)
        {
            InitializeComponent();
            // ✅ fix shadowing: store the values from login
            this.fullName = fullName;
            this.role = role;
            this.empId = empId;

            this.Load += AccountantForm_Load;
        }
        private void AccountantForm_Load(object sender, EventArgs e)
        {
            // แสดงชื่อ-ตำแหน่งจากค่าที่ส่งมาจาก Login
            txtName.Text = this.fullName;
            txtPosition.Text = this.role;
        }

        private void btnReceivePaymentMain_Click(object sender, EventArgs e)
        {
            // Toggle การซ่อน/แสดง
            panelReceivePaymentSub.Visible = !panelReceivePaymentSub.Visible;
        }

        // Generic loader for any page
        private void LoadUserControl(UserControl uc)
        {
            Body.Controls.Clear();     // ล้างเนื้อหาเก่าออก
            uc.Dock = DockStyle.Fill;  // ขยายเต็ม panel
            Body.Controls.Add(uc);     // เพิ่ม UserControl
            uc.BringToFront();         // ดันไปข้างหน้า
        }

        // 🔹 Public navigation API used by Invoice page
        public void ShowConfirmInvoice(string invId)
        {
            var page = new ConfirmInvoice(this.fullName, this.role, this.empId);
            page.InitFromInvoiceId(invId);   // <-- preload controls
            LoadUserControl(page);
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            LoadUserControl(new Invoice());
            txtFunctionname.Text = "เรียกชำระเงิน";
        }

        private void btnConfirmInvoice_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ConfirmInvoice(this.fullName, this.role, this.empId));
            txtFunctionname.Text = "ยืนยันการรับชำระเงิน";
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            LoadUserControl(new Receipt());
            txtFunctionname.Text = "พิมพ์ใบเสร็จรับเงิน";
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // ปิดโปรแกรม
        }
    }
}
