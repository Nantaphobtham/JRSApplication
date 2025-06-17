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
        private readonly string _role;
        private readonly string _empId;

        public SiteSupervisorForm(string fullName, string role, string empId)
        {
            InitializeComponent();

            _fullName = fullName;
            _role = role;
            _empId = empId;

            txtName.Text = _fullName;
            txtPosition.Text = _role;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // ปิดโปรแกรม
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
    }
}
