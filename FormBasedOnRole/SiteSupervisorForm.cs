using JRSApplication.Sitesupervisor;
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
    public partial class SiteSupervisorForm : Form
    {
        // เก็บค่าที่ส่งมา
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

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit(); // ปิด Application ทั้งหมด
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            //minimize window
            this.WindowState = FormWindowState.Minimized; // ย่อหน้าต่าง
        }

        private void LoadUserControl(UserControl userControl, string functionName = "", bool showSubFunctionName = false)
        {
            if (userControl == null)
            {
                return; // **ถ้าไม่มี UserControl ส่งมา ก็ไม่ต้องทำอะไร**
            }

            if (PicLogo != null)
            {
                PicLogo.Visible = false; // ซ่อนโลโก้
            }

            // เคลียร์ Panel Body
            Body.Controls.Clear();

            // ตั้งค่า Dock
            userControl.Dock = DockStyle.Fill;

            // เพิ่ม UserControl เข้า Panel
            Body.Controls.Add(userControl);

            // ตั้งชื่อ function name
            txtFunctionname.Text = functionName;
            txtFunctionname.Location = new Point(884, 45);

            // ตั้งค่า SubFunctionName visible
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
            LoadUserControl(new WorkResponse(), "ผลการอนุมัติ"); 
        }

        
    }
}
