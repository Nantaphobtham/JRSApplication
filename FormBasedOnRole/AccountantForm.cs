using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using JRSApplication.Accountant;
using JRSApplication.Components.Models;
using JRSApplication.Components.Service;

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

            this.fullName = fullName;
            this.role = role;
            this.empId = empId;

            this.Load += AccountantForm_Load;
        }

        private void AccountantForm_Load(object sender, EventArgs e)
        {
            txtName.Text = this.fullName;
            txtPosition.Text = this.role;

            // 🔹 รูปโปรไฟล์ตาม role
            Profile.Image = RoleIconHelper.GetProfileIcon(this.role);
            Profile.SizeMode = PictureBoxSizeMode.Zoom;
            Profile.BackColor = Color.Transparent;
        }

        private void btnReceivePaymentMain_Click(object sender, EventArgs e)
        {
            panelReceivePaymentSub.Visible = !panelReceivePaymentSub.Visible;
        }

        private void LoadUserControl(UserControl uc)
        {
            Body.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            Body.Controls.Add(uc);
            uc.BringToFront();
        }

        public void ShowConfirmInvoice(string invId)
        {
            var page = new ConfirmInvoice(this.fullName, this.role, this.empId);
            page.InitFromInvoiceId(invId);
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
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void Profile_Click(object sender, EventArgs e)
        {
            // Create an instance of your new user control, passing the required data
            var changePasswordControl = new ChangePassword1(this.empId, this.fullName, this.role);

            // Use your existing method to load it into the body
            LoadUserControl(changePasswordControl);

            // Optionally update the header text
            txtFunctionname.Text = "เปลี่ยนรหัสผ่าน";
        }
    }
}
