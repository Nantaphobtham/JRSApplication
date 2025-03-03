using JRSApplication.Components;
using JRSApplication.Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class ManageProject : UserControl
    {
        // สำหรับเก็บ ID ของลูกค้า และพนักงาน
        private string selectedCustomerID = "";
        private string selectedEmployeeID = "";
        private string loggedInUser = "Admin"; // ✅ เก็บชื่อผู้ใช้ที่ล็อกอินมา
        private List<EmployeeAssignment> assignedEmployees = new List<EmployeeAssignment>(); // ✅ เก็บพนักงานที่เลือกไว้ก่อนบันทึก
        private List<ProjectPhase> projectPhases = new List<ProjectPhase>();



        public ManageProject()
        {
            InitializeComponent();
            LoadPhaseNumberDropdown();
            LoadProjectData();
            InitializePhaseDataGridView(); //ของ phase
        }
        private void LoadProjectData()
        {
            ProjectDAL dal = new ProjectDAL();
            List<Project> projects = dal.GetAllProjects();

            dtgvProject.Rows.Clear();
            foreach (var project in projects)
            {
                dtgvProject.Rows.Add(project.ProjectID, project.ProjectName, project.ProjectStart.ToShortDateString(),
                                      project.ProjectEnd.ToShortDateString(), project.ProjectBudget);
            }
        }

        //กำหนดค่าลงใน cmbCurrentPhaseNumber
        private void LoadPhaseNumberDropdown()
        {
            cmbCurrentPhaseNumber.Items.Clear();
            for (int i = 3; i <= 15; i++)
            {
                cmbCurrentPhaseNumber.Items.Add(i.ToString());
            }
        }
        //เมื่อเลือก cmbCurrentPhaseNumber ต้องอัปเดต cmbPhaseNumber
        private void cmbCurrentPhaseNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCurrentPhaseNumber.SelectedItem != null)
            {
                int totalPhases = int.Parse(cmbCurrentPhaseNumber.SelectedItem.ToString());

                // ✅ เคลียร์ค่าเก่าก่อน
                cmbPhaseNumber.Items.Clear();

                // ✅ เพิ่มค่า 1 ถึง totalPhases (รวม totalPhases ด้วย)
                for (int i = 1; i <= totalPhases; i++)
                {
                    cmbPhaseNumber.Items.Add(i.ToString());
                }

                // ✅ ตั้งค่าเริ่มต้นให้เลือกอันแรก
                if (cmbPhaseNumber.Items.Count > 0)
                {
                    cmbPhaseNumber.SelectedIndex = 0;
                }
            }
        }



        private void btnSearchEmployee_Click(object sender, EventArgs e)
        {
            using (SearchForm searchForm = new SearchForm("Employee"))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    // ✅ เก็บค่าที่ได้จาก SearchForm
                    selectedEmployeeID = searchForm.SelectedID;
                    txtEmployeeName.Text = searchForm.SelectedName;
                    txtEmployeeLastName.Text = searchForm.SelectedLastName;
                    txtEmployeeRole.Text = searchForm.SelectedIDCardOrRole; // ✅ ใช้ตำแหน่งเป็น Role

                    // ✅ เพิ่มพนักงานเข้า List (แต่ยังไม่ลงฐานข้อมูล)
                    EmployeeAssignment newAssign = new EmployeeAssignment
                    {
                        EmployeeID = int.Parse(selectedEmployeeID),
                        EmployeeName = txtEmployeeName.Text,
                        EmployeeLastName = txtEmployeeLastName.Text,
                        AssignRole = txtEmployeeRole.Text,
                        AssignBy = loggedInUser, // ✅ Admin ที่ล็อกอิน
                        AssignDate = DateTime.Now
                    };

                    // ✅ ตรวจสอบว่า Employee ถูก Assign ไปแล้วหรือไม่
                    if (!assignedEmployees.Any(a => a.EmployeeID == newAssign.EmployeeID))
                    {
                        assignedEmployees.Add(newAssign);
                        //RefreshAssignedEmployeesGrid(); // ✅ อัปเดต DataGridView
                    }
                    else
                    {
                        MessageBox.Show("พนักงานคนนี้ถูกมอบหมายไปแล้ว!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            OpenSearchForm("Customer", txtCustomerName, txtCustomerLastName, txtCustomerIDCard, txtCustomerPhone, txtCustomerEmail);
        }

        private void OpenSearchForm(string searchType, TextBox nameTextBox, TextBox lastNameTextBox,
                            TextBox idCardOrRoleTextBox, TextBox phoneTextBox = null, TextBox emailTextBox = null)
        {
            using (SearchForm searchForm = new SearchForm(searchType))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    // ✅ เก็บค่า ID ของลูกค้าหรือพนักงาน (แต่ไม่แสดงใน UI)
                    if (searchType == "Customer")
                    {
                        selectedCustomerID = searchForm.SelectedID;
                    }
                    else if (searchType == "Employee")
                    {
                        selectedEmployeeID = searchForm.SelectedID;
                    }

                    // ✅ แสดงเฉพาะข้อมูลที่ต้องการใน TextBox
                    nameTextBox.Text = searchForm.SelectedName;
                    lastNameTextBox.Text = searchForm.SelectedLastName;
                    idCardOrRoleTextBox.Text = searchForm.SelectedIDCardOrRole;

                    if (phoneTextBox != null) phoneTextBox.Text = searchForm.SelectedPhone;
                    if (emailTextBox != null) emailTextBox.Text = searchForm.SelectedEmail;
                }
            }
        }

        //Action button
        private void btnSave_Click(object sender, EventArgs e)
        {
            
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //เพิ่ม
            EnableControls_open();
            ReadOnlyControls_open();
            txtProjectName.Focus();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //แก้ไข
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //ลบ
            if (dtgvProject.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกโครงการที่ต้องการลบ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int projectID = Convert.ToInt32(dtgvProject.SelectedRows[0].Cells[0].Value);

            DialogResult result = MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบโครงการนี้?", "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                ProjectDAL dal = new ProjectDAL();
                bool success = dal.DeleteProject(projectID);

                if (success)
                {
                    MessageBox.Show("ลบข้อมูลโครงการสำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProjectData();
                    ClearForm();
                    EnableControls_close();
                    ReadOnlyControls_close();
                }
                else
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการลบข้อมูล!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        //ฟังก์ชันคำนวณจำนวนวัน
        private void CalculateProjectDuration()
        {
            // ตรวจสอบว่า วันที่เริ่มต้นและวันที่สิ้นสุดถูกต้องหรือไม่
            if (dtpkEndDate.Value >= dtpkStartDate.Value)
            {
                // คำนวณจำนวนวัน
                int totalDays = (dtpkEndDate.Value - dtpkStartDate.Value).Days;

                // แสดงผลที่ txtSumDate
                txtSumDate.Text = totalDays + " วัน";
            }
            else
            {
                // ถ้าผู้ใช้เลือกวันสิ้นสุดที่น้อยกว่าวันเริ่มต้น
                txtSumDate.Text = "กรุณาเลือกวันที่ให้ถูกต้อง";
            }
        }
        private void dtpkStartDate_ValueChanged(object sender, EventArgs e)
        {
            CalculateProjectDuration(); // คำนวณใหม่เมื่อเปลี่ยนวันที่เริ่มต้น
        }
        private void dtpkEndDate_ValueChanged(object sender, EventArgs e)
        {
            CalculateProjectDuration(); // คำนวณใหม่เมื่อเปลี่ยนวันที่สิ้นสุด
        }

        //เปิด ปิด ล้าง ฟอร์ม
        private void EnableControls_open()
        {
            txtProjectName.Enabled = true;
            txtNumber.Enabled = true;
            txtBudget.Enabled = true;
            txtRemark.Enabled = true;
            txtProjectDetail.Enabled = true;
            txtProjectAddress.Enabled = true;
            txtPhaseDetail.Enabled = true;
            txtPercentPhase.Enabled = true;

            dtpkStartDate.Enabled = true;
            dtpkEndDate.Enabled = true;

            cmbCurrentPhaseNumber.Enabled = true;
            cmbPhaseNumber.Enabled = true;

            btnSearchCustomer.Enabled = true;
            btnSearchEmployee.Enabled = true;

            btnInsertBlueprintFile.Enabled = true;
            btnInsertDemolitionFile.Enabled = true;

            btnAddPhase.Enabled = true;
            btnEditPhase.Enabled = true;
        }
        private void EnableControls_close()
        {
            txtProjectName.Enabled = false;
            txtNumber.Enabled = false;
            txtBudget.Enabled = false;
            txtRemark.Enabled = false;
            txtProjectDetail.Enabled = false;
            txtProjectAddress.Enabled = false;
            txtPhaseDetail.Enabled = false;
            txtPercentPhase.Enabled = false;

            dtpkStartDate.Enabled = false;
            dtpkEndDate.Enabled = false;

            cmbCurrentPhaseNumber.Enabled = false;
            cmbPhaseNumber.Enabled = false;

            btnSearchCustomer.Enabled = false;
            btnSearchEmployee.Enabled = false;

            btnInsertBlueprintFile.Enabled = false;
            btnInsertDemolitionFile.Enabled = false;

            btnAddPhase.Enabled = false;
            btnEditPhase.Enabled = false;
        }
        private void ReadOnlyControls_open()
        {
            txtProjectName.ReadOnly = false;
            txtNumber.ReadOnly = false;
            txtBudget.ReadOnly = false;
            txtRemark.ReadOnly = false;
            txtProjectDetail.ReadOnly = false;
            txtProjectAddress.ReadOnly = false;
            txtPhaseDetail.ReadOnly = false;
            txtPercentPhase.ReadOnly = false;
        }
        private void ReadOnlyControls_close()
        {
            txtProjectName.ReadOnly = true;
            txtNumber.ReadOnly = true;
            txtBudget.ReadOnly = true;
            txtRemark.ReadOnly = true;
            txtProjectDetail.ReadOnly = true;
            txtProjectAddress.ReadOnly = true;
            txtPhaseDetail.ReadOnly = true;
            txtPercentPhase.ReadOnly = true;
        }
        private void ClearForm()
        {
            txtProjectName.Clear();
            txtNumber.Clear();
            txtBudget.Clear();
            txtRemark.Clear();
            txtProjectDetail.Clear();
            txtProjectAddress.Clear();
            txtPhaseDetail.Clear();
            txtPercentPhase.Clear();

            dtpkStartDate.Value = DateTime.Now;
            dtpkEndDate.Value = DateTime.Now;

            cmbCurrentPhaseNumber.SelectedIndex = -1;
            cmbPhaseNumber.SelectedIndex = -1;

            selectedCustomerID = "";
            selectedEmployeeID = "";
        }

        private void dtgvProject_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int projectID = Convert.ToInt32(dtgvProject.Rows[e.RowIndex].Cells[0].Value);

                // ✅ โหลดเฟสของโครงการที่เลือก
                LoadPhaseData(projectID);
            }
        }

        private void LoadPhaseData(int projectID)
        {
            ProjectPhaseDAL dal = new ProjectPhaseDAL();
            List<ProjectPhase> phases = dal.GetPhasesByProject(projectID);

            dtgvPhase.Rows.Clear();
            foreach (var phase in phases)
            {
                dtgvPhase.Rows.Add(phase.PhaseNumber, phase.PhaseDetail, phase.PhaseBudget);
            }
        }

        //นำเข้าไฟล์ PDF
        private void SelectFileAndSetButtonText(Button button)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "เลือกไฟล์ PDF";
                openFileDialog.Filter = "PDF Files|*.pdf"; // จำกัดให้เลือกเฉพาะ PDF
                openFileDialog.Multiselect = false; // เลือกได้ทีละไฟล์

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = openFileDialog.FileName;

                    // ✅ ตรวจสอบว่าเป็น PDF หรือไม่
                    if (Path.GetExtension(selectedFile).ToLower() == ".pdf")
                    {
                        string fileName = Path.GetFileName(selectedFile); // ดึงแค่ชื่อไฟล์ เช่น "โครงการ.pdf"
                        button.Text = fileName; // แสดงชื่อไฟล์บนปุ่ม
                        button.Tag = selectedFile; // เก็บพาธไฟล์ใน `Tag`
                    }
                    else
                    {
                        MessageBox.Show("กรุณาเลือกไฟล์ PDF เท่านั้น!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        private void btnInsertBlueprintFile_Click(object sender, EventArgs e)
        {
            SelectFileAndSetButtonText(btnInsertBlueprintFile);
        }
        private void btnInsertDemolitionFile_Click(object sender, EventArgs e)
        {
            SelectFileAndSetButtonText(btnInsertDemolitionFile);
        }

        //ฟังก์ชันสำหรับ txtBudget ที่รองรับจุดทศนิยม
        private void txtBudget_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ✅ อนุญาตเฉพาะตัวเลข จุดทศนิยม และปุ่ม Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '.')
            {
                e.Handled = true; // ป้องกันการพิมพ์ตัวอักษรหรืออักขระพิเศษ
            }

            // ✅ ป้องกันไม่ให้มีมากกว่า 1 จุดทศนิยม
            if (e.KeyChar == '.' && txtBudget.Text.Contains("."))
            {
                e.Handled = true;
            }
        }
        private void txtBudget_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtBudget.Text, out decimal budget))
            {
                txtBudget.Text = budget.ToString("#,##0.00"); // ✅ ใส่ `,` คั่นหลักพัน และบังคับทศนิยม 2 ตำแหน่ง
            }
        }
        private void txtBudget_Enter(object sender, EventArgs e)
        {
            txtBudget.Text = txtBudget.Text.Replace(",", ""); // ✅ ลบ `,` ออกเมื่อแก้ไข
        }


        // phase data
        private void InitializePhaseDataGridView()
        {
            // ✅ ป้องกันการเพิ่มคอลัมน์ซ้ำ
            if (dtgvPhase.Columns.Count == 0)
            {
                dtgvPhase.AllowUserToAddRows = false;

                // ✅ เพิ่มคอลัมน์
                dtgvPhase.Columns.Add("PhaseNumber", "เฟสที่");
                dtgvPhase.Columns.Add("PhaseDetail", "รายละเอียดการดำเนินงาน");
                dtgvPhase.Columns.Add("PhaseBudget", "งบประมาณเฟส (บาท)"); // ✅ เปลี่ยนจาก "เปอร์เซ็นต์งาน"

                // ✅ ปรับแต่งคอลัมน์
                dtgvPhase.Columns["PhaseNumber"].Width = 60;
                dtgvPhase.Columns["PhaseNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvPhase.Columns["PhaseNumber"].ReadOnly = true;

                dtgvPhase.Columns["PhaseDetail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dtgvPhase.Columns["PhaseDetail"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dtgvPhase.Columns["PhaseDetail"].ReadOnly = true;

                dtgvPhase.Columns["PhaseBudget"].Width = 120;
                dtgvPhase.Columns["PhaseBudget"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dtgvPhase.Columns["PhaseBudget"].DefaultCellStyle.Format = "N2"; // ✅ แสดงเป็น 1,200.00
                dtgvPhase.Columns["PhaseBudget"].ReadOnly = true;

                // ✅ ปรับแต่ง UI
                CustomizeDataGridViewPhase();
            }
        }
        private void CustomizeDataGridViewPhase()
        {
            dtgvPhase.BorderStyle = BorderStyle.None;
            dtgvPhase.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvPhase.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvPhase.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvPhase.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvPhase.BackgroundColor = Color.White;

            dtgvPhase.EnableHeadersVisualStyles = false;
            dtgvPhase.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvPhase.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvPhase.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvPhase.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvPhase.ColumnHeadersHeight = 30;

            dtgvPhase.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvPhase.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvPhase.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvPhase.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            dtgvPhase.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvPhase.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvPhase.RowTemplate.Height = 30;

            dtgvPhase.GridColor = Color.LightGray;
            dtgvPhase.RowHeadersVisible = false;

            dtgvPhase.ReadOnly = true;
            dtgvPhase.AllowUserToAddRows = false;
            dtgvPhase.AllowUserToResizeRows = false;
        }
        private void btnAddPhase_Click(object sender, EventArgs e)
        {
            // ✅ ตรวจสอบค่าซ้ำ
            int phaseNumber = int.Parse(cmbPhaseNumber.SelectedItem.ToString());
            if (projectPhases.Any(p => p.PhaseNumber == phaseNumber))
            {
                MessageBox.Show("เฟสนี้มีอยู่แล้ว กรุณาเลือกเฟสใหม่!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ ตรวจสอบว่ากรอกข้อมูลครบ
            if (cmbPhaseNumber.SelectedItem == null || string.IsNullOrWhiteSpace(txtPhaseDetail.Text) || string.IsNullOrWhiteSpace(txtPercentPhase.Text))
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ แปลงค่าเปอร์เซ็นต์
            if (!decimal.TryParse(txtPercentPhase.Text, out decimal phasePercent) || phasePercent <= 0)
            {
                MessageBox.Show("กรุณากรอกเปอร์เซ็นต์งานให้ถูกต้อง!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ คำนวณ phase_budget (budget * phasePercent / 100)
            decimal totalBudget = decimal.Parse(txtBudget.Text.Replace(",", ""));
            decimal phaseBudget = (totalBudget * phasePercent) / 100;

            // ✅ สร้างอ็อบเจ็กต์ของเฟส (ใช้ตัวแปร phaseNumber ที่มีอยู่แล้ว)
            ProjectPhase newPhase = new ProjectPhase
            {
                PhaseNumber = phaseNumber, // ❌ ห้ามใช้ `int phaseNumber` ซ้ำ
                PhaseDetail = txtPhaseDetail.Text.Trim(),
                PhaseBudget = phaseBudget
            };

            // ✅ เพิ่มเข้าไปใน List
            projectPhases.Add(newPhase);

            // ✅ โหลดข้อมูลลง DataGridView
            LoadPhaseToGridView();

            // ✅ ล้างค่าฟอร์มเพื่อให้กรอกเฟสถัดไป
            clearPhaseForm();
        }
        private void LoadPhaseToGridView()
        {
            dtgvPhase.Rows.Clear();

            foreach (var phase in projectPhases)
            {
                dtgvPhase.Rows.Add(phase.PhaseNumber, phase.PhaseDetail, phase.PhaseBudget.ToString("N2")); // ✅ แสดงงบประมาณแทนเปอร์เซ็นต์
            }

            // ✅ คำนวณงบประมาณรวม
            CalculateTotalPhasePercentage();
        }
        private void CalculateTotalPhasePercentage()
        {
            decimal totalPercent = projectPhases.Sum(p => (p.PhaseBudget * 100) / decimal.Parse(txtBudget.Text.Replace(",", "")));

            // ✅ แสดงผลรวมที่ตารางด้านล่าง
            lblTotalPercentage.Text = $"รวม  {totalPercent:0.00}%";

            if (totalPercent != 100)
            {
                lblTotalPercentage.ForeColor = Color.Red;
            }
            else
            {
                lblTotalPercentage.ForeColor = Color.Green;
            }
        }
        private void clearPhaseForm()
        {
            txtPhaseDetail.Clear();
            txtPercentPhase.Clear();
            cmbPhaseNumber.SelectedIndex = -1; // ✅ ล้างค่าเลือกเฟส
        }


    }
}
