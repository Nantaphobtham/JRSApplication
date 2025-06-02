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
    public partial class AdminForm : Form
    {

        //name & role
        private string userFullName;
        private string userRole;

        public AdminForm(string fullName, string role)
        {
            InitializeComponent();

            // เก็บค่าที่รับมา
            userFullName = fullName;
            userRole = role;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            txtName.Text = userFullName;
            txtPosition.Text = userRole;
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; // ย่อหน้าต่าง
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // ปิดโปรแกรม
        }

        private void LoadUserControl(UserControl userControl)
        {
            if (userControl == null)
            {
                return; // **ถ้าไม่มี UserControl ส่งเข้ามา ก็ไม่ต้องทำอะไร**
            }

            if (PicLogo != null)
            {
                PicLogo.Visible = false; // ซ่อนโลโก้
            }

            // เคลียร์เนื้อหาทั้งหมดใน Panel Body
            Body.Controls.Clear();
                
            // ตั้งค่าให้ UserControl เต็ม Panel
            userControl.Dock = DockStyle.Fill;

            // เพิ่ม UserControl ลงใน Panel Body
            Body.Controls.Add(userControl);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UserManagementForm());
        }

        private void btnRegisCustomer_Click(object sender, EventArgs e)
        {
            LoadUserControl(new CustomerRegistration());
        }

        private void btnRegisSupplier_Click(object sender, EventArgs e)
        {
            LoadUserControl(new SupplierRegistration());
        }

        private void btnManageProject_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ManageProject(userFullName, userRole));
        }
    }
}
