using JRSApplication.Components.Models;
using JRSApplication.Components.Service;
using JRSApplication.ProjectManager;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using JRSApplication.Components.Service;

namespace JRSApplication
{
    public partial class ProjectManagerForm : Form
    {
        private string userFullName;
        private string userRole;
        private string _empID;

        public ProjectManagerForm(string fullName, string role, string empId)
        {
            InitializeComponent();

            userFullName = fullName;
            userRole = role;
            _empID = empId;
        }

        private void ProjectManagerForm_Load(object sender, EventArgs e)
        {
            txtName.Text = userFullName;
            txtPosition.Text = userRole;

            // 🔹 รูปโปรไฟล์ตาม role
            Profile.Image = RoleIconHelper.GetProfileIcon(userRole);
            Profile.SizeMode = PictureBoxSizeMode.Zoom;
            Profile.BackColor = Color.Transparent;
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

        private void btnProjectInformation_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ProjectData());
            txtFunctionname.Text = "ตรวจสอบข้อมูลโครงการ";
            txtsubFunctionname.Visible = false;

            btnProjectInformation.BackColor = Color.White;
            btnPaymentsInfomation.BackColor = Color.Transparent;
            btnPurchaseOrder.BackColor = Color.Transparent;
            btnProjectPhaseUpdate.BackColor = Color.Transparent;
            btnChooseSubcontractors.BackColor = Color.Transparent;
            btnRequestsforApproval.BackColor = Color.Transparent;
            btnAllocateEmployee.BackColor = Color.Transparent;
        }

        private void btnPaymentsInfomation_Click(object sender, EventArgs e)
        {
            LoadUserControl(new CheckProjectPay());
            txtFunctionname.Text = "ตรวจสอบการชำระเงินโครงการ";
            txtsubFunctionname.Visible = false;

            btnProjectInformation.BackColor = Color.Transparent;
            btnPaymentsInfomation.BackColor = Color.White;
            btnPurchaseOrder.BackColor = Color.Transparent;
            btnProjectPhaseUpdate.BackColor = Color.Transparent;
            btnChooseSubcontractors.BackColor = Color.Transparent;
            btnRequestsforApproval.BackColor = Color.Transparent;
            btnAllocateEmployee.BackColor = Color.Transparent;
        }

        private void btnPurchaseOrder_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ApprovePurchaseOrder(_empID));
            txtFunctionname.Text = "อนุมัติใบสั่งซื้อ";
            txtsubFunctionname.Visible = false;

            btnProjectInformation.BackColor = Color.Transparent;
            btnPaymentsInfomation.BackColor = Color.Transparent;
            btnPurchaseOrder.BackColor = Color.White;
            btnProjectPhaseUpdate.BackColor = Color.Transparent;
            btnChooseSubcontractors.BackColor = Color.Transparent;
            btnRequestsforApproval.BackColor = Color.Transparent;
            btnAllocateEmployee.BackColor = Color.Transparent;
        }

        private void btnChooseSubcontractors_Click(object sender, EventArgs e)
        {
            LoadUserControl(new DetermineSubcontractors(_empID));
            txtFunctionname.Text = "กำหนดผู้รับเหมา";
            txtsubFunctionname.Visible = false;

            btnProjectInformation.BackColor = Color.Transparent;
            btnPaymentsInfomation.BackColor = Color.Transparent;
            btnPurchaseOrder.BackColor = Color.Transparent;
            btnProjectPhaseUpdate.BackColor = Color.Transparent;
            btnChooseSubcontractors.BackColor = Color.White;
            btnRequestsforApproval.BackColor = Color.Transparent;
            btnAllocateEmployee.BackColor = Color.Transparent;
        }

        private void btnAllocateEmployee_Click(object sender, EventArgs e)
        {
            LoadUserControl(new AllocatePersonnel());
            txtFunctionname.Text = "จัดสรรบุคลากร";
            txtsubFunctionname.Visible = false;

            btnProjectInformation.BackColor = Color.Transparent;
            btnPaymentsInfomation.BackColor = Color.Transparent;
            btnPurchaseOrder.BackColor = Color.Transparent;
            btnProjectPhaseUpdate.BackColor = Color.Transparent;
            btnChooseSubcontractors.BackColor = Color.Transparent;
            btnRequestsforApproval.BackColor = Color.Transparent;
            btnAllocateEmployee.BackColor = Color.White;
        }

        private void btnProjectPhaseUpdate_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UpdateProjectPhase(_empID, userRole));
            txtFunctionname.Text = "ปรับปรุงข้อมูลโครงการ";
            txtsubFunctionname.Visible = true;
            txtsubFunctionname.Text = "ปรับปรุงข้อมูลเฟส";

            btnProjectInformation.BackColor = Color.Transparent;
            btnPaymentsInfomation.BackColor = Color.Transparent;
            btnPurchaseOrder.BackColor = Color.Transparent;
            btnProjectPhaseUpdate.BackColor = Color.White;
            btnChooseSubcontractors.BackColor = Color.Transparent;
            btnRequestsforApproval.BackColor = Color.Transparent;
        }

        private void btnHeadmenu_Click(object sender, EventArgs e)
        {
            menuTransition.Start();
            txtFunctionname.Text = "ปรับปรุงข้อมูลโครงการ";
            txtsubFunctionname.Visible = true;
            txtsubFunctionname.Text = "";
        }

        private void btnRequestsforApproval_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ProjectPhaseListRequestsforApproval());
            txtFunctionname.Text = "ปรับปรุงข้อมูลโครงการ";
            txtsubFunctionname.Visible = true;
            txtsubFunctionname.Text = "รายการคำขออนุมัติผลการดำเนินงาน";

            btnProjectInformation.BackColor = Color.Transparent;
            btnPaymentsInfomation.BackColor = Color.Transparent;
            btnPurchaseOrder.BackColor = Color.Transparent;
            btnProjectPhaseUpdate.BackColor = Color.Transparent;
            btnChooseSubcontractors.BackColor = Color.Transparent;
            btnRequestsforApproval.BackColor = Color.White;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        bool menuExpand = false;

        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (!menuExpand)
            {
                menuContainer.Height += 10;
                if (menuContainer.Height >= 246)
                {
                    menuTransition.Stop();
                    menuExpand = true;
                }
            }
            else
            {
                menuContainer.Height -= 10;
                if (menuContainer.Height <= 80)
                {
                    menuTransition.Stop();
                    menuExpand = false;
                }
            }
        }
        private void Profile_Click(object sender, EventArgs e)
        {
            // Create an instance of your new user control, passing the required data
            var changePasswordControl = new ChangePassword1(this._empID, this.userFullName, this.userRole);

            // Use your existing method to load it into the body
            LoadUserControl(changePasswordControl);

            // Optionally update the header text
            txtFunctionname.Text = "เปลี่ยนรหัสผ่าน";
        }
    }
}
