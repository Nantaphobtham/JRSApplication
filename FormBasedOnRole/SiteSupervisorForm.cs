using JRSApplication.Components.Models;
using JRSApplication.Sitesupervisor;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class SiteSupervisorForm : Form
    {
        private readonly string _fullName;
        private readonly string userRole;
        private readonly string _empId;

        public SiteSupervisorForm(string fullName, string role, string empId)
        {
            InitializeComponent();

            _fullName = fullName;
            userRole = role;
            _empId = empId;

            txtName.Text = _fullName;
            txtPosition.Text = userRole;

            // 🔹 รูปโปรไฟล์ตาม role
            Profile.Image = RoleIconHelper.GetProfileIcon(userRole);
            Profile.SizeMode = PictureBoxSizeMode.Zoom;
            Profile.BackColor = Color.Transparent;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void LoadUserControl(UserControl userControl, string functionName = "", bool showSubFunctionName = false)
        {
            if (userControl == null)
                return;

            if (PicLogo != null)
                PicLogo.Visible = false;

            Body.Controls.Clear();
            userControl.Dock = DockStyle.Fill;
            Body.Controls.Add(userControl);

            txtFunctionname.Text = functionName;
            txtFunctionname.Location = new Point(884, 45);

            txtsubFunctionname.Visible = showSubFunctionName;
        }

        private void btnCheckProjectInformation_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ProjectData(), "ตรวจสอบข้อมูลโครงการ");
        }

        private void btnPurchaseOrder_Click(object sender, EventArgs e)
        {
            LoadUserControl(new PurchaseOrderForm(_empId), "ออกใบสั่งซื้อ");
        }

        private void btnUpdatePhase_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UpdateProjectPhase(_empId, userRole), "ปรับปรุงข้อมูลเฟส");
        }

        private void btnWorkResponse_Click(object sender, EventArgs e)
        {
            LoadUserControl(new WorkResponse(), "ผลการอนุมัติใบสั่งซื้อ");
        }

        private void btnPhaseApprovalResult_Click(object sender, EventArgs e)
        {
            LoadUserControl(new PhaseApprovalResult(_empId, userRole), "ผลการอนุมัติเฟส");
        }
    }
}
