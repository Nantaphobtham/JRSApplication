﻿using JRSApplication.ProjectManager;
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
    public partial class ProjectManagerForm : Form
    {
        private string userFullName;
        private string userRole;
        private string _empID;
        public ProjectManagerForm(string fullName, string role, string empId)
        {
            InitializeComponent();

            // เก็บค่าที่รับมา
            userFullName = fullName;
            userRole = role;
            _empID = empId;
        }

        private void ProjectManagerForm_Load(object sender, EventArgs e)
        {
            txtName.Text = userFullName;
            txtPosition.Text = userRole;
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


        private void btnProjectInformation_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ProjectData());
            txtFunctionname.Text = "ตรวจสอบข้อมูลโครงการ";
            txtsubFunctionname.Visible = false;
        }

        private void btnPaymentsInfomation_Click(object sender, EventArgs e)
        {
            LoadUserControl(new CheckProjectPay());
            txtFunctionname.Text = "ตรวจสอบการชำระเงินโครงการ";
            txtsubFunctionname.Visible = false;
        }

        private void btnPurchaseOrder_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ApprovePurchaseOrder(_empID));
            txtFunctionname.Text = "อนุมัติใบสั่งซื้อ";
            txtsubFunctionname.Visible = false;
        }

        private void btnChooseSubcontractors_Click(object sender, EventArgs e)
        {
            LoadUserControl(new DetermineSubcontractors(_empID));
            txtFunctionname.Text = "กำหนดผู้รับเหมาช่วง";
            txtsubFunctionname.Visible = false;
        }

        private void btnAllocateEmployee_Click(object sender, EventArgs e)
        {
            LoadUserControl(new AllocatePersonnel());
            txtFunctionname.Text = "จัดสรรบุคลากร";
            txtsubFunctionname.Visible = false;
        }

        private void btnProjectPhaseUpdate_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UpdateProjectPhase(_empID, userRole));
            txtFunctionname.Text = "ปรับปรุงข้อมูลโครงการ";
            txtsubFunctionname.Visible = true;
            txtsubFunctionname.Text = "ปรับปรุงข้อมูลเฟส";
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
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit(); // ปิด Application ทั้งหมด
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; // ย่อหน้าต่าง
        }

        bool menuExpand = false;
        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (menuExpand == false)
            {
                menuContainer.Height += 10;
                if(menuContainer.Height >= 246)
                {
                    menuTransition.Stop();
                    menuExpand = true;
                }
            }
            else
            {
                menuContainer.Height -= 10;
                if(menuContainer.Height <= 80)
                {
                    menuTransition.Stop();
                    menuExpand = false;

                }
            }

        }

        

        
    }
}
