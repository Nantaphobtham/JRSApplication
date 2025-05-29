using JRSApplication.Components;
using JRSApplication.Data_Access_Layer;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class ManageProject : UserControl
    {
        //ตัวแปรระดับคลาส
        private string selectedCustomerID = "";
        private string selectedEmployeeID = "";
        private string loggedInUser = "Admin"; // ✅ เก็บชื่อผู้ใช้ที่ล็อกอินมา

        private List<EmployeeAssignment> assignedEmployees = new List<EmployeeAssignment>(); // ✅ เก็บพนักงานที่เลือกไว้ก่อนบันทึก

        // 🟢 เก็บรายการเฟสทั้งหมด
        private List<ProjectPhase> projectPhases = new List<ProjectPhase>();

        // 🟢 ใช้เก็บเฟสที่กำลังแก้ไข
        private ProjectPhase currentEditingPhase = null;

        //file PDF
        private byte[] fileConstructionBytes;
        private byte[] fileDemolitionBytes;


        public ManageProject()
        {
            InitializeComponent();
            LoadPhaseNumberDropdown();
            LoadProjectData();
            InitializePhaseDataGridView(); //ของ phase
            InitializeDataGridViewProject(); // ✅ กำหนดโครงสร้าง DataGridView
            LoadProjectData(); // ✅ โหลดข้อมูลจากฐานข้อมูล
        }

        //การโหลดและจัดการตาราง Project
        private void InitializeDataGridViewProject()
        {
            // ✅ ป้องกันการเพิ่มคอลัมน์ซ้ำ
            if (dtgvProject.Columns.Count == 0)
            {
                dtgvProject.AllowUserToAddRows = false;

                // ✅ เพิ่มคอลัมน์
                dtgvProject.Columns.Add("ProjectID", "รหัสโครงการ");
                dtgvProject.Columns.Add("ProjectName", "ชื่อโครงการ");
                dtgvProject.Columns.Add("ProjectStart", "วันที่เริ่มโครงการ");
                dtgvProject.Columns.Add("ProjectEnd", "วันที่สิ้นสุดโครงการ");
                dtgvProject.Columns.Add("ProjectBudget", "งบประมาณ (บาท)");
                dtgvProject.Columns.Add("CurrentPhaseNumber", "จำนวนเฟสงาน");
                dtgvProject.Columns.Add("CustomerFullName", "ลูกค้า");
                dtgvProject.Columns.Add("EmployeeFullName", "ผู้ดูแลโครงการ");

                // ✅ ปรับแต่งคอลัมน์
                dtgvProject.Columns["ProjectID"].Width = 80;
                dtgvProject.Columns["ProjectID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProject.Columns["ProjectID"].ReadOnly = true;

                dtgvProject.Columns["ProjectName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dtgvProject.Columns["ProjectName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dtgvProject.Columns["ProjectName"].ReadOnly = true;

                dtgvProject.Columns["ProjectStart"].Width = 120;
                dtgvProject.Columns["ProjectStart"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProject.Columns["ProjectStart"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dtgvProject.Columns["ProjectStart"].ReadOnly = true;

                dtgvProject.Columns["ProjectEnd"].Width = 120;
                dtgvProject.Columns["ProjectEnd"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProject.Columns["ProjectEnd"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dtgvProject.Columns["ProjectEnd"].ReadOnly = true;

                dtgvProject.Columns["ProjectBudget"].Width = 150;
                dtgvProject.Columns["ProjectBudget"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dtgvProject.Columns["ProjectBudget"].DefaultCellStyle.Format = "N2"; // แสดงเป็น 1,200.00
                dtgvProject.Columns["ProjectBudget"].ReadOnly = true;

                dtgvProject.Columns["CurrentPhaseNumber"].Width = 120;
                dtgvProject.Columns["CurrentPhaseNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProject.Columns["CurrentPhaseNumber"].ReadOnly = true;

                dtgvProject.Columns["CustomerFullName"].Width = 150;
                dtgvProject.Columns["CustomerFullName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dtgvProject.Columns["CustomerFullName"].ReadOnly = true;

                dtgvProject.Columns["EmployeeFullName"].Width = 150;
                dtgvProject.Columns["EmployeeFullName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dtgvProject.Columns["EmployeeFullName"].ReadOnly = true;

                // ✅ ใช้ฟังก์ชันตกแต่ง
                CustomizeDataGridViewProject();
            }
        }

        private void CustomizeDataGridViewProject()
        {
            dtgvProject.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvProject.BorderStyle = BorderStyle.None;
            dtgvProject.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvProject.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvProject.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvProject.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvProject.BackgroundColor = Color.White;

            dtgvProject.EnableHeadersVisualStyles = false;
            dtgvProject.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvProject.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvProject.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvProject.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvProject.ColumnHeadersHeight = 30;

            dtgvProject.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvProject.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvProject.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvProject.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            dtgvProject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvProject.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvProject.RowTemplate.Height = 30;

            dtgvProject.GridColor = Color.LightGray;
            dtgvProject.RowHeadersVisible = false;

            dtgvProject.ReadOnly = true;
            dtgvProject.AllowUserToAddRows = false;
            dtgvProject.AllowUserToResizeRows = false;
        }
        private void LoadProjectData()
        {
            InitializeDataGridViewProject(); // ✅ ตรวจสอบตารางก่อนโหลดข้อมูล

            ProjectDAL dal = new ProjectDAL();
            List<Project> projects = dal.GetAllProjects();

            dtgvProject.Rows.Clear();
            foreach (var project in projects)
            {
                dtgvProject.Rows.Add(
                    project.ProjectID,
                    project.ProjectName,
                    project.ProjectStart.ToString("dd/MM/yyyy"),
                    project.ProjectEnd.ToString("dd/MM/yyyy"),
                    project.ProjectBudget.ToString("N2"),
                    project.CurrentPhaseNumber,  // ✅ จำนวนเฟส
                    project.CustomerName,  // ✅ ชื่อลูกค้า
                    project.EmployeeName   // ✅ ชื่อผู้ดูแลโครงการ
                );
            }
        }
        private void LoadProjectDetails(int projectId)
        {
            //ProjectDAL projectDAL = new ProjectDAL();
            //Project project = projectDAL.GetProjectDetailsById(projectId);

            //if (project != null)
            //{
            //    //ต้องเรียกอะไรบ้าง
            //}
        }
        private void dtgvProject_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // ✅ ดึงค่า `ProjectID` จากตาราง
                int projectId = Convert.ToInt32(dtgvProject.Rows[e.RowIndex].Cells["ProjectID"].Value);

                // ✅ โหลดข้อมูลโครงการไปยัง TextBox ต่างๆ
                LoadProjectDetails(projectId);

                // ✅ โหลดข้อมูล Phase ที่เกี่ยวข้อง
                LoadPhaseData(projectId);
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------
        
        // การจัดการข้อมูล Phase
        private void InitializePhaseDataGridView()
        {
            // ✅ ป้องกันการเพิ่มคอลัมน์ซ้ำ
            if (dtgvPhase.Columns.Count == 0)
            {
                dtgvPhase.AllowUserToAddRows = false;

                // ✅ เพิ่มคอลัมน์
                dtgvPhase.Columns.Add("PhaseNumber", "เฟสที่");
                dtgvPhase.Columns.Add("PhaseDetail", "รายละเอียดการดำเนินงาน");
                dtgvPhase.Columns.Add("PhaseBudget", "งบประมาณเฟส (บาท)");
                dtgvPhase.Columns.Add("PhasePercent", "เปอร์เซ็นต์งาน (%)"); // 🟢 เพิ่มคอลัมน์

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

                dtgvPhase.Columns["PhasePercent"].Width = 100;
                dtgvPhase.Columns["PhasePercent"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dtgvPhase.Columns["PhasePercent"].DefaultCellStyle.Format = "0.00"; // ✅ แสดงเป็น 2 ตำแหน่ง
                dtgvPhase.Columns["PhasePercent"].ReadOnly = true;

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

        private void LoadPhaseData(int projectId)
        {
            InitializePhaseDataGridView();

            PhaseDAL phaseDAL = new PhaseDAL();
            List<ProjectPhase> phases = phaseDAL.GetAllPhasesByPrjectID(projectId);

            dtgvPhase.Rows.Clear();
            foreach (var phase in phases)
            {
                dtgvPhase.Rows.Add(
                    phase.PhaseNumber, // เฟสที่
                    phase.PhaseDetail, // รายละเอียดการดำเนินงาน
                    phase.PhaseBudget.ToString("N2"), // จำนวนเงิน (บาท) -> แสดงเป็น 1,200.00
                    phase.PhasePercent.ToString("N2") + " %" // เปอร์เซ็นต์ (%)
                );
            }
        }

        private void LoadPhaseToGridView()
        {
            dtgvPhase.Rows.Clear();

            foreach (var phase in projectPhases)
            {
                dtgvPhase.Rows.Add(phase.PhaseNumber, phase.PhaseDetail,
                                   phase.PhaseBudget.ToString("N2"),
                                   phase.PhasePercent.ToString("0.00"));
            }

            // ✅ คำนวณเปอร์เซ็นต์รวม
            CalculateTotalPhasePercentage();
        }
        private void dtgvPhase_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // ✅ ตรวจสอบว่าคลิกที่แถวที่มีข้อมูล
            {
                DataGridViewRow row = dtgvPhase.Rows[e.RowIndex];

                // ✅ ค้นหาเฟสที่ถูกเลือกจาก `List<ProjectPhase>`
                int selectedPhaseNumber = int.Parse(row.Cells["PhaseNumber"].Value.ToString());
                currentEditingPhase = projectPhases.FirstOrDefault(p => p.PhaseNumber == selectedPhaseNumber);
                if (currentEditingPhase != null)
                {
                    // ✅ แสดงค่าลงในช่องป้อนข้อมูล
                    cmbPhaseNumber.SelectedItem = currentEditingPhase.PhaseNumber.ToString();
                    txtPhaseDetail.Text = currentEditingPhase.PhaseDetail;
                    txtPercentPhase.Text = currentEditingPhase.PhasePercent.ToString("0.00");

                    // ห้ามแก้ไข
                    PhaseDisableEditing();
                    btnTurnoffEditing.Visible = true;
                }
            }

        }

        private void btnAddPhase_Click(object sender, EventArgs e)
        {
            // ✅ ตรวจสอบว่ากรอกข้อมูลครบ
            if (cmbPhaseNumber.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtPhaseDetail.Text) ||
                string.IsNullOrWhiteSpace(txtPercentPhase.Text))
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

            if (string.IsNullOrWhiteSpace(txtBudget.Text))
            {
                MessageBox.Show("กรุณากรอกงบประมาณก่อนเพิ่มเฟส!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ คำนวณ phase_budget (budget * phasePercent / 100)
            decimal totalBudget = decimal.Parse(txtBudget.Text.Replace(",", ""));
            decimal phaseBudget = (totalBudget * phasePercent) / 100;
            int phaseNumber = int.Parse(cmbPhaseNumber.SelectedItem.ToString());
            // ✅ รวมเปอร์เซ็นต์ทั้งหมดก่อนเพิ่ม
            decimal totalPhasePercent = projectPhases.Sum(p => p.PhasePercent);

            if (btnAddPhase.Text == "บันทึก" && currentEditingPhase != null)
            {
                // ถ้าแก้ไขเฟสเดิม ให้นำเปอร์เซ็นต์เดิมออกก่อนรวม
                totalPhasePercent -= currentEditingPhase.PhasePercent;
            }

            // ✅ ตรวจสอบว่าไม่เกิน 100%
            if (totalPhasePercent + phasePercent > 100)
            {
                MessageBox.Show($"เปอร์เซ็นต์รวมของเฟสเกิน 100%! ({totalPhasePercent + phasePercent}%)",
                                "แจ้งเตือน",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (btnAddPhase.Text == "บันทึก" && currentEditingPhase != null)
            {
                // ✅ แก้ไขข้อมูลเดิม
                currentEditingPhase.PhaseDetail = txtPhaseDetail.Text.Trim();
                currentEditingPhase.PhasePercent = phasePercent;
                currentEditingPhase.PhaseBudget = phaseBudget;
            }
            else
            {
                // ✅ ตรวจสอบว่ามีเฟสนี้แล้วหรือยัง
                if (projectPhases.Any(p => p.PhaseNumber == phaseNumber))
                {
                    MessageBox.Show("เฟสนี้มีอยู่แล้ว กรุณาเลือกเฟสใหม่!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ เพิ่มข้อมูลใหม่
                ProjectPhase newPhase = new ProjectPhase
                {
                    PhaseNumber = phaseNumber,
                    PhaseDetail = txtPhaseDetail.Text.Trim(),
                    PhasePercent = phasePercent,
                    PhaseBudget = phaseBudget
                };

                projectPhases.Add(newPhase);
            }

            // ✅ โหลดข้อมูลใหม่ลงตาราง
            LoadPhaseToGridView();

            // ✅ ล้างค่าฟอร์ม
            clearPhaseForm();
            PhaseAbleOn();
            btnTurnoffEditing.Visible = false;

            // ✅ เปลี่ยนปุ่มกลับเป็น "เพิ่ม"
            btnAddPhase.Text = "เพิ่ม";
            currentEditingPhase = null;
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------

        //การคำนวณและรีเซ็ต Phase

        
        private void CalculateTotalPhasePercentage()
        {
            decimal totalPercent = projectPhases.Sum(p => p.PhasePercent);

            lblTotalPercentage.Text = $"รวม {totalPercent:0.00}%";

            // ✅ เปลี่ยนสีตัวเลขถ้าครบ 100%
            lblTotalPercentage.ForeColor = totalPercent == 100 ? Color.Green : Color.Red;
        }
        //เปิดปิด การทำงาน phase
        private void clearPhaseForm()
        {   //ล้างข้อมูล
            txtPhaseDetail.Clear();
            txtPercentPhase.Clear();
            cmbPhaseNumber.SelectedIndex = -1;
        }
        private void ClearPhaseData()
        {
            projectPhases.Clear();               // เคลียร์ List<ProjectPhase>
            dtgvPhase.Rows.Clear();              // เคลียร์แถวใน DataGridView
            cmbPhaseNumber.SelectedIndex = -1;   // รีเซ็ต dropdown
            txtPhaseDetail.Clear();
            txtPercentPhase.Clear();

        }
        private void PhaseDisableEditing()
        {   //ปิดการทำงาน
            txtPhaseDetail.ReadOnly = true;
            txtPercentPhase.ReadOnly = true;
            cmbPhaseNumber.Enabled = false;
            txtPhaseDetail.Enabled = false;
            txtPercentPhase.Enabled = false;
        }
        private void PhaseAbleOn()
        {//เปิดการทำงาน
            txtPhaseDetail.ReadOnly = false;
            txtPercentPhase.ReadOnly = false;
            cmbPhaseNumber.Enabled = true;
            txtPhaseDetail.Enabled = true;
            txtPercentPhase.Enabled = true;
        }
        private void btnEditPhase_Click(object sender, EventArgs e)
        {
            if (currentEditingPhase != null)  // ✅ ตรวจสอบว่ามีเฟสที่ถูกเลือกหรือไม่
            {
                // ✅ เปิดการแก้ไขเฉพาะรายละเอียดงาน & เปอร์เซ็นต์
                txtPhaseDetail.ReadOnly = false;
                txtPercentPhase.ReadOnly = false;
                txtPhaseDetail.Enabled = true;
                txtPercentPhase.Enabled = true;

                // ✅ ปิดการแก้ไขหมายเลขเฟส เพื่อป้องกันการเปลี่ยนหมายเลขเฟส
                cmbPhaseNumber.Enabled = false;

                // ✅ เปลี่ยนปุ่ม "เพิ่ม" เป็น "บันทึก"
                btnAddPhase.Text = "บันทึก";
            }
            else
            {
                MessageBox.Show("กรุณาเลือกเฟสที่ต้องการแก้ไข", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnTurnoffEditing_Click(object sender, EventArgs e)
        {
            PhaseAbleOn();
            clearPhaseForm();
            btnTurnoffEditing.Visible = false;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------

        //การจัดการ ComboBox(เฟสงาน)
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

        //-----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------

        //ปุ่ม Action หลัก
        //Action button
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                // ✅ ตรวจสอบข้อมูลก่อนบันทึก
                if (!ValidateProjectData())
                {
                    return; // ❌ ถ้าข้อมูลไม่ครบ จะไม่ดำเนินการต่อ
                }

                ProjectDAL dal = new ProjectDAL();
                int projectID = dal.GenerateProjectID(); // ✅ สร้าง `pro_id` ก่อน

                Project project = new Project
                {
                    ProjectID = projectID,
                    ProjectName = txtProjectName.Text.Trim(),
                    ProjectDetail = txtProjectDetail.Text.Trim(),
                    ProjectAddress = txtProjectAddress.Text.Trim(),
                    ProjectBudget = decimal.Parse(txtBudget.Text.Replace(",", "")),
                    ProjectStart = dtpkStartDate.Value,
                    ProjectEnd = dtpkEndDate.Value,
                    CurrentPhaseNumber = int.Parse(cmbCurrentPhaseNumber.SelectedItem.ToString()),
                    Remark = txtRemark.Text.Trim(),
                    ProjectNumber = txtNumber.Text.Trim(),
                    EmployeeID = int.Parse(selectedEmployeeID),
                    CustomerID = int.Parse(selectedCustomerID)
                };

                // ✅ สร้าง ProjectFile แยกต่างหาก
                ProjectFile projectFile = new ProjectFile
                {
                    ProjectID = projectID,
                    ConstructionBlueprint = fileConstructionBytes, // สมมุติคุณมีข้อมูลไฟล์ในตัวแปรนี้
                    DemolitionModel = fileDemolitionBytes         // หรือ null ได้
                };

                bool success = dal.InsertProjectWithPhases(project, projectPhases, fileConstructionBytes, fileDemolitionBytes);

                if (success)
                {
                    MessageBox.Show("บันทึกโครงการเรียบร้อย", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProjectData(); // ✅ โหลดข้อมูลใหม่
                }
                else
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึก", "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ข้อผิดพลาด: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //เคลียฟอร์ม
            ClearForm();
            ReadOnlyControls_close();
            EnableControls_close();

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //เพิ่ม เปิดการทำงาน
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
            //if (dtgvProject.SelectedRows.Count == 0)
            //{
            //    MessageBox.Show("กรุณาเลือกโครงการที่ต้องการลบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //// 🔍 ดึง ProjectID จากแถวที่เลือก
            //int selectedProjectID = Convert.ToInt32(dtgvProject.SelectedRows[0].Cells["ProjectID"].Value);

            //var confirm = MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบโครงการนี้?", "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (confirm == DialogResult.Yes)
            //{
            //    ProjectDAL dal = new ProjectDAL();
            //    bool success = dal.DeleteProjectWithPhases(selectedProjectID);

            //    if (success)
            //    {
            //        MessageBox.Show("ลบโครงการเรียบร้อย", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        LoadProjectData(); // โหลดข้อมูลใหม่
            //    }
            //    else
            //    {
            //        MessageBox.Show("เกิดข้อผิดพลาดในการลบโครงการ", "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------

        //การจัดการฟอร์ม
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

            txtWorkingDate.Enabled = true;
            dtpkStartDate.Enabled = true;
            //dtpkEndDate.Enabled = true;

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

            txtWorkingDate.Enabled = true;
            dtpkStartDate.Enabled = false;
            //dtpkEndDate.Enabled = false;

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
            //แก้ไขใหม่
            txtWorkingDate.ReadOnly = false;
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
            //แก้ไขใหม่
            txtWorkingDate.ReadOnly = true;
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
            txtCustomerName.Text = "";
            txtCustomerLastName.Text = "";
            txtCustomerIDCard.Text = "";
            txtCustomerPhone.Text = "";
            txtCustomerEmail.Text = "";

            selectedEmployeeID = "";
            txtEmployeeName.Text = "";
            txtEmployeeLastName.Text = "";
            txtEmployeeRole.Text = "";
            //แก้ไขใหม่
            txtWorkingDate.Text = "";


            btnInsertBlueprintFile.Text = "เลือกไฟล์";
            btnInsertDemolitionFile.Text = "เลือกไฟล์";
            //projectFile = new ProjectFile(); // รีเซ็ตตัวแปรเก็บไฟล์
            fileConstructionBytes = null;
            fileDemolitionBytes = null;

            // ✔️ ล้าง phase
            ClearPhaseData();
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------

        //การเลือกผู้ดูแล/ลูกค้า
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
        private void OpenSearchForm(string searchType, TextBox nameTextBox, TextBox lastNameTextBox, TextBox idCardOrRoleTextBox, TextBox phoneTextBox = null, TextBox emailTextBox = null)
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
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------

        //จัดการ Budget (KeyPress/Enter/Leave)
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

        //-----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------


        //คำนวณจำนวนวันของโครงการ 
        private void CalculateEndDateFromWorkingDays()
        {
            // ตรวจสอบค่าที่ป้อน
            if (!int.TryParse(txtWorkingDate.Text.Trim(), out int workingDays) || workingDays <= 0)
            {
                MessageBox.Show("กรุณากรอกจำนวนวันทำงานให้ถูกต้อง");
                return;
            }

            DateTime startDate = dtpkStartDate.Value;
            DateTime currentDate = startDate;
            int countedDays = 0;

            while (countedDays < workingDays)
            {
                currentDate = currentDate.AddDays(1);

                if (currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    countedDays++;
                }
            }

            // 🔔 ตรวจสอบว่าตรงกับวันอาทิตย์พอดีไหม
            if (currentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                DialogResult result = MessageBox.Show(
                    "วันที่สิ้นสุดโครงการตรงกับวันอาทิตย์ คุณต้องการขยายระยะเวลาอีก 1 วันหรือไม่?",
                    "ยืนยันการเปลี่ยนแปลง",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    //  ยืนยัน -> เพิ่มวันทำงานอีก 1 วัน และคำนวณใหม่
                    txtWorkingDate.Text = (workingDays + 1).ToString();
                    CalculateEndDateFromWorkingDays(); // รีคำนวณใหม่
                    return;
                }
                else
                {
                    // ❌ ไม่ยืนยัน -> ผู้ใช้แก้ไขเอง
                    MessageBox.Show("กรุณาแก้ไขระยะเวลาทำงานเพื่อหลีกเลี่ยงวันอาทิตย์ก่อนบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // ✅ ตั้งค่า EndDate ที่คำนวณได้
            dtpkEndDate.Value = currentDate;
        }

        private void dtpkStartDate_ValueChanged(object sender, EventArgs e)
        {
            CalculateEndDateFromWorkingDays();
        }
        private void txtWorkingDate_TextChanged(object sender, EventArgs e)
        {
            CalculateEndDateFromWorkingDays();
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------

        private byte[] SelectFileAndSetButtonText(Button button)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "เลือกไฟล์ PDF";
                openFileDialog.Filter = "PDF Files|*.pdf";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = openFileDialog.FileName;

                    // ✅ ตรวจสอบไฟล์ PDF หรือไม่
                    if (Path.GetExtension(selectedFile).ToLower() == ".pdf")
                    {
                        FileInfo fileInfo = new FileInfo(selectedFile);

                        // ✅ ตรวจสอบขนาดไฟล์ไม่เกิน 50MB
                        if (fileInfo.Length <= 50 * 1024 * 1024)
                        {
                            string fileName = Path.GetFileName(selectedFile);
                            button.Text = fileName;
                            Console.WriteLine("ไฟล์ที่เลือก: " + fileName); // ☑️ Debug Log

                            return File.ReadAllBytes(selectedFile); // ☑️ แปลงไฟล์เป็น byte[]
                        }
                        else
                        {
                            MessageBox.Show("ขนาดไฟล์เกิน 50MB", "ขนาดไฟล์ไม่ถูกต้อง", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("กรุณาเลือกไฟล์ PDF เท่านั้น!", "ชนิดไฟล์ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            return null; 
        }

        private void btnInsertBlueprintFile_Click(object sender, EventArgs e)
        {
            fileConstructionBytes = SelectFileAndSetButtonText(btnInsertBlueprintFile);
        }

        private void btnInsertDemolitionFile_Click(object sender, EventArgs e)
        {
            fileDemolitionBytes = SelectFileAndSetButtonText(btnInsertDemolitionFile);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------

        //Validate ก่อนบันทึก
        private bool ValidateProjectData()
        {
            // ✅ ตรวจสอบค่าที่จำเป็นต้องกรอก
            if (string.IsNullOrWhiteSpace(txtProjectName.Text))
            {
                MessageBox.Show("กรุณากรอกชื่อโครงการ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProjectName.Focus();
                starProjectName.Visible = true;
                return false;
            }
            else
            {
                starProjectName.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(txtNumber.Text))
            {
                MessageBox.Show("กรุณากรอกเลขที่สัญญา", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNumber.Focus();
                starNumber.Visible = true;
                return false;
            }
            else
            {

            }

            if (cmbCurrentPhaseNumber.SelectedItem == null)
            {
                MessageBox.Show("กรุณาระบุจำนวนเฟส", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCurrentPhaseNumber.Focus();
                starPhase.Visible = true;
                return false;
            }
            else
            {

            }

            if (dtpkStartDate.Value > dtpkEndDate.Value)
            {
                MessageBox.Show("วันที่สิ้นสุดโครงการต้องไม่น้อยกว่าวันที่เริ่มต้น", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpkEndDate.Focus();
                starStartDate.Visible = true;
                starWorkingDate.Visible = true;
                return false;
            }
            else
            {

            }

            if (string.IsNullOrWhiteSpace(txtBudget.Text) || !decimal.TryParse(txtBudget.Text.Replace(",", ""), out _))
            {
                MessageBox.Show("กรุณากรอกจำนวนเงินจ้างที่ถูกต้อง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBudget.Focus();
                starBudget.Visible = true;
                return false;
            }
            else
            {
                starBudget.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(selectedCustomerID))
            {
                MessageBox.Show("กรุณาเลือกข้อมูลลูกค้า", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                starCustomer.Visible = true;
                return false;
            }
            else
            {
                starCustomer.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(selectedEmployeeID))
            {
                MessageBox.Show("กรุณาเลือกข้อมูลผู้ดูแลโครงการ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                starProjectManager.Visible = true;
                return false;
            }
            else
            {
                starProjectManager.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(txtProjectDetail.Text))
            {
                MessageBox.Show("กรุณากรอกข้อมูลโครงการ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProjectDetail.Focus();
                starProjectDetail.Visible = true;
                return false;
            }
            else
            {
                starProjectDetail.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(txtProjectAddress.Text))
            {
                MessageBox.Show("กรุณากรอกที่อยู่โครงการ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProjectAddress.Focus();
                starProjectAddress.Visible = true;
                return false;
            }
            else 
            {
                starProjectAddress.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(txtRemark.Text))
            {
                MessageBox.Show("", "" , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRemark.Focus();
                starRemark.Visible = true;
                return false;
            }
            else
            {
                starRemark.Visible = false;
            }
            // ✅ ตรวจสอบว่ามีไฟล์ blueprint หรือไม่
            if (fileConstructionBytes == null)
            {
                MessageBox.Show(
                    "กรุณาเลือกไฟล์แบบแปลนโครงการ (Blueprint) ก่อนบันทึก!",
                    "ข้อมูลผิดพลาด",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            else
            {
                starBlueprint.Visible = false;
            }

            // ✅ ตรวจสอบเปอร์เซ็นต์รวมของเฟส (ต้องเท่ากับ 100%)
            decimal totalPercent = projectPhases.Sum(p => p.PhasePercent);
            if (totalPercent != 100)
            {
                MessageBox.Show("เปอร์เซ็นต์รวมของเฟสต้องเท่ากับ 100%", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return false;
            }

            return true; // ✅ ผ่านทุกเงื่อนไข
        }

        
    }
}
