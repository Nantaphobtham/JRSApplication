using JRSApplication.Components.Models;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class AdminForm : Form
    {
        private string userFullName;
        private string userRole;

        public AdminForm(string fullName, string role)
        {
            InitializeComponent();

            userFullName = fullName;
            userRole = role;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            txtName.Text = userFullName;
            txtPosition.Text = userRole;

            // 🔹 รูปโปรไฟล์ตาม role (ใช้โค้ดกลาง)
            Profile.Image = RoleIconHelper.GetProfileIcon(userRole);
            Profile.SizeMode = PictureBoxSizeMode.Zoom;
            Profile.BackColor = Color.Transparent;
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadUserControl(UserControl userControl)
        {
            if (userControl == null)
                return;

            if (PicLogo != null)
                PicLogo.Visible = false;

            Body.Controls.Clear();
            userControl.Dock = DockStyle.Fill;
            Body.Controls.Add(userControl);
        }

        private void btnManageUser_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UserManagementForm());
            txtFunctionname.Text = "จัดการบัญชีผู้ใช้";
            txtFunctionname.Location = new Point(857, 45);

            btnManageUser.BackColor = Color.White;
            btnRegisCustomer.BackColor = Color.Transparent;
            btnRegisSupplier.BackColor = Color.Transparent;
            btnManageProject.BackColor = Color.Transparent;
        }

        private void btnRegisCustomer_Click(object sender, EventArgs e)
        {
            LoadUserControl(new CustomerRegistration());
            txtFunctionname.Text = "ทะเบียนลูกค้า";
            txtFunctionname.Location = new Point(875, 45);

            btnRegisCustomer.BackColor = Color.White;
            btnManageUser.BackColor = Color.Transparent;
            btnRegisSupplier.BackColor = Color.Transparent;
            btnManageProject.BackColor = Color.Transparent;
        }

        private void btnRegisSupplier_Click(object sender, EventArgs e)
        {
            LoadUserControl(new SupplierRegistration());
            txtFunctionname.Text = "ทะเบียนซัพพลายเออร์";
            txtFunctionname.Location = new Point(875, 45);

            btnRegisSupplier.BackColor = Color.White;
            btnManageUser.BackColor = Color.Transparent;
            btnRegisCustomer.BackColor = Color.Transparent;
            btnManageProject.BackColor = Color.Transparent;
        }

        private void btnManageProject_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ManageProject(userFullName, userRole));
            txtFunctionname.Text = "จัดการข้อมูลโครงการ";
            txtFunctionname.Location = new Point(875, 45);

            btnManageProject.BackColor = Color.White;
            btnManageUser.BackColor = Color.Transparent;
            btnRegisCustomer.BackColor = Color.Transparent;
            btnRegisSupplier.BackColor = Color.Transparent;
        }
    }
}
