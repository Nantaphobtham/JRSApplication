using JRSApplication.Components;
using JRSApplication.Components.Service;
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
        //private string loggedInUser = "Admin"; // ✅ เก็บชื่อผู้ใช้ที่ล็อกอินมา

        private string _loggedInUser;
        private string _loggedInRole;

        private List<EmployeeAssignment> assignedEmployees = new List<EmployeeAssignment>(); // ✅ เก็บพนักงานที่เลือกไว้ก่อนบันทึก

        // 🟢 เก็บรายการเฟสทั้งหมด
        private List<ProjectPhase> projectPhases = new List<ProjectPhase>();

        // 🟢 ใช้เก็บเฟสที่กำลังแก้ไข
        private ProjectPhase currentEditingPhase = null;

        //file PDF
        private byte[] fileConstructionBytes;
        private byte[] fileDemolitionBytes;
        //PDFPreview
        private FormPDFPreview pdfPreviewForm = null; // ✅ ฟอร์ม Preview
        private Timer hoverCheckTimer;                // ✅ Timer เช็กเมาส์ออก
        private bool isPreviewingDemolition = false;  // ✅ Flag บอกว่าปุ่มไหน trigger

        private int? selectedProjectID = null; // null = ยังไม่มีการเลือก



        public ManageProject(string fullName, string role)
        {
            InitializeComponent();

            _loggedInUser = fullName;  // ✅ รับค่าชื่อผู้ใช้ที่ล็อกอิน
            _loggedInRole = role;      // ✅ รับค่าตำแหน่ง

            LoadPhaseNumberDropdown();
            
            InitializePhaseDataGridView(); //ของ phase
            InitializeDataGridViewProject(); // ✅ กำหนดโครงสร้าง DataGridView
            
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
            // ✅ ตรวจสอบว่า DataGridView ถูกตั้งค่าแล้วหรือยัง
            InitializeDataGridViewProject(); // ป้องกันการเพิ่มคอลัมน์ซ้ำ

            // ✅ ดึงข้อมูลจากฐานข้อมูล
            ProjectDAL dal = new ProjectDAL();
            List<Project> projects = dal.GetAllProjects();

            // ✅ เคลียร์ข้อมูลเก่า
            dtgvProject.Rows.Clear();

            // ✅ เพิ่มข้อมูลใหม่ลง DataGridView
            foreach (var project in projects)
            {
                dtgvProject.Rows.Add(
                    project.ProjectID,
                    project.ProjectName,
                    project.ProjectStart.ToString("dd/MM/yyyy"),
                    project.ProjectEnd.ToString("dd/MM/yyyy"),
                    project.ProjectBudget.ToString("N2"),
                    project.CurrentPhaseNumber,
                    project.CustomerName ?? "ไม่ระบุ",
                    project.EmployeeName ?? "ไม่ระบุ"
                );
            }
        }


        private void LoadFullProjectByID(int projectId)
        {
            //  ดึงข้อมูลเต็มจาก ProjectDAL
            ProjectDAL dal = new ProjectDAL();
            Project project = dal.GetProjectDetailsById(projectId);

            selectedProjectID = project.ProjectID;


            if (project == null)
            {
                MessageBox.Show("ไม่พบข้อมูลโครงการที่เลือก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ แสดงข้อมูล Project ลง TextBox / ComboBox / DatePicker
            txtProjectName.Text = project.ProjectName;
            txtProjectDetail.Text = project.ProjectDetail;
            txtProjectAddress.Text = project.ProjectAddress;
            txtBudget.Text = project.ProjectBudget.ToString("N2");
            txtRemark.Text = project.Remark;
            txtNumber.Text = project.ProjectNumber;

            dtpkStartDate.Value = project.ProjectStart;
            dtpkEndDate.Value = project.ProjectEnd;

            // ✅ นับวันทำงาน และแสดง
            int workingDays = CalculateWorkingDays(project.ProjectStart, project.ProjectEnd);
            txtWorkingDate.Text = workingDays.ToString();

            cmbCurrentPhaseNumber.SelectedItem = project.CurrentPhaseNumber.ToString();

            // ✅ ชื่อลูกค้า/พนักงาน
            txtCustomerName.Text = project.CustomerName;
            txtEmployeeName.Text = project.EmployeeName;

            // 🟢 TODO: เก็บ ID ลูกค้า/พนักงาน (ในตัวแปร global ถ้ามี)
            //selectedCustomerID = project.CustomerID;
            //selectedEmployeeID = project.EmployeeID;

            // ✅ โหลดไฟล์ (ถ้ามี)
            fileConstructionBytes = project.ProjectFile?.ConstructionBlueprint;
            fileDemolitionBytes = project.ProjectFile?.DemolitionModel;

            if (fileConstructionBytes != null)
                btnInsertBlueprintFile.Text = "ไฟล์แนบแล้ว";
            else
                btnInsertBlueprintFile.Text = "เลือกไฟล์";

            if (fileDemolitionBytes != null)
                btnInsertDemolitionFile.Text = "ไฟล์แนบแล้ว";
            else
                btnInsertDemolitionFile.Text = "เลือกไฟล์";

            // ✅ โหลด Phase ลง dtgvPhase และ projectPhases list
            projectPhases = project.Phases ?? new List<ProjectPhase>();
            dtgvPhase.Rows.Clear();

            foreach (var phase in projectPhases)
            {
                dtgvPhase.Rows.Add(
                    phase.PhaseNumber,
                    phase.PhaseDetail,
                    phase.PhaseBudget.ToString("N2"),
                    phase.PhasePercent.ToString("0.00") + " %"
                );
            }

            // ✅ อัปเดตยอดรวม %
            CalculateTotalPhasePercentage();
        }

        private void dtgvProject_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int projectId = Convert.ToInt32(dtgvProject.Rows[e.RowIndex].Cells["ProjectID"].Value);
                LoadFullProjectByID(projectId);
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
                dtgvPhase.Columns.Add("PhasePercent", "% ความก้าวหน้า");   // จาก txtcompletionPercentage
                dtgvPhase.Columns.Add("BoqPercent", "% BOQ");   // ย้อนคำนวณจากงบ

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

                dtgvPhase.Columns["BoqPercent"].Width = 100;
                dtgvPhase.Columns["BoqPercent"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dtgvPhase.Columns["BoqPercent"].DefaultCellStyle.Format = "0.00";
                dtgvPhase.Columns["BoqPercent"].ReadOnly = true;

                CustomizeDataGridViewPhase();
            }
        }
        private void CustomizeDataGridViewPhase()
        {
            dtgvPhase.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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

        private void LoadPhaseToGridView()
        {
            dtgvPhase.Rows.Clear();

            decimal totalBudget = 0;
            decimal.TryParse(txtBudget.Text.Replace(",", ""), out totalBudget);

            foreach (var phase in projectPhases)
            {
                // 🧮 คำนวณ % BOQ จาก Budget
                decimal boqPercent = 0;
                if (totalBudget > 0)
                {
                    boqPercent = (phase.PhaseBudget * 100) / totalBudget;
                }

                dtgvPhase.Rows.Add(
                    phase.PhaseNumber,
                    phase.PhaseDetail,
                    phase.PhaseBudget.ToString("N2"),
                    phase.PhasePercent.ToString("0.00"),     // % ความก้าวหน้า
                    boqPercent.ToString("0.00")              // % BOQ
                );
            }

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
                    txtboqPercentage.Text = currentEditingPhase.PhasePercent.ToString("0.00");
                    txtcompletionPercentage.Text = currentEditingPhase.PhasePercent.ToString("0.00");



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
                string.IsNullOrWhiteSpace(txtboqPercentage.Text) ||
                string.IsNullOrWhiteSpace(txtcompletionPercentage.Text))
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ แปลงค่า % BOQ และ ความก้าวหน้า
            if (!decimal.TryParse(txtboqPercentage.Text, out decimal boqPercent) || boqPercent <= 0)
            {
                MessageBox.Show("กรุณากรอกเปอร์เซ็นต์ BOQ ให้ถูกต้อง!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtcompletionPercentage.Text, out decimal completionPercent) ||
                completionPercent < 0 || completionPercent > 100)
            {
                MessageBox.Show("กรุณากรอกเปอร์เซ็นต์ความก้าวหน้าให้ถูกต้อง (0-100%)", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ ตรวจสอบว่ากรอกงบประมาณแล้ว
            if (string.IsNullOrWhiteSpace(txtBudget.Text))
            {
                MessageBox.Show("กรุณากรอกงบประมาณก่อนเพิ่มเฟส!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ คำนวณงบประมาณจาก BOQ %
            decimal totalBudget = 0;
            decimal.TryParse(txtBudget.Text.Replace(",", ""), out totalBudget);
            decimal phaseBudget = (totalBudget * boqPercent) / 100;

            int phaseNumber = int.Parse(cmbPhaseNumber.SelectedItem.ToString());

            // ✅ ตรวจสอบยอดรวมเปอร์เซ็นต์ก่อนเพิ่ม
            decimal totalCompletionPercent = projectPhases.Sum(p => p.PhasePercent);
            decimal totalBoqPercent = 0;

            foreach (var p in projectPhases)
            {
                totalBoqPercent += (p.PhaseBudget * 100) / totalBudget;
            }

            // ✅ ถ้าแก้ไขเฟสเดิม ให้นำค่าเดิมออกจากยอดรวมก่อนเช็ค
            if (btnAddPhase.Text == "บันทึก" && currentEditingPhase != null)
            {
                totalCompletionPercent -= currentEditingPhase.PhasePercent;
                totalBoqPercent -= (currentEditingPhase.PhaseBudget * 100) / totalBudget;
            }

            // ✅ เช็คว่าไม่เกิน 100%
            if (totalCompletionPercent + completionPercent > 100)
            {
                MessageBox.Show($"เปอร์เซ็นต์ความก้าวหน้ารวมเกิน 100%! (รวม {totalCompletionPercent + completionPercent:0.00}%)",
                                "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (totalBoqPercent + boqPercent > 100)
            {
                MessageBox.Show($"เปอร์เซ็นต์งวดงาน (BOQ) รวมเกิน 100%! (รวม {totalBoqPercent + boqPercent:0.00}%)",
                                "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ ดำเนินการเพิ่มหรือบันทึก
            if (btnAddPhase.Text == "บันทึก" && currentEditingPhase != null)
            {
                currentEditingPhase.PhaseDetail = txtPhaseDetail.Text.Trim();
                currentEditingPhase.PhasePercent = completionPercent;
                currentEditingPhase.PhaseBudget = phaseBudget;
            }
            else
            {
                if (projectPhases.Any(p => p.PhaseNumber == phaseNumber))
                {
                    MessageBox.Show("เฟสนี้มีอยู่แล้ว กรุณาเลือกเฟสใหม่!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ProjectPhase newPhase = new ProjectPhase
                {
                    PhaseNumber = phaseNumber,
                    PhaseDetail = txtPhaseDetail.Text.Trim(),
                    PhasePercent = completionPercent,
                    PhaseBudget = phaseBudget
                };

                projectPhases.Add(newPhase);
            }

            // ✅ อัปเดต UI และ Reset
            LoadPhaseToGridView();
            clearPhaseForm();
            PhaseAbleOn();
            btnTurnoffEditing.Visible = false;
            btnAddPhase.Text = "เพิ่ม";
            currentEditingPhase = null;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------

        //การคำนวณและรีเซ็ต Phase


        private void CalculateTotalPhasePercentage()
        {
            decimal totalCompletion = projectPhases.Sum(p => p.PhasePercent);
            decimal totalBoqPercent = 0;
            decimal totalBoqBudget = 0;

            decimal totalBudget = 0;
            decimal.TryParse(txtBudget.Text.Replace(",", ""), out totalBudget);

            foreach (var phase in projectPhases)
            {
                // 🟡 ย้อนคำนวณ boqPercent จาก budget
                if (totalBudget > 0)
                {
                    decimal boqPercent = (phase.PhaseBudget * 100) / totalBudget;
                    totalBoqPercent += boqPercent;
                    totalBoqBudget += phase.PhaseBudget;
                }
            }

            lblTotalPercentage.Text = $"รวม เปอร์เซ็นต์ความก้าวหน้างาน % : {totalCompletion:0.00}%\n" +
                                      $"รวม เปอร์เซ็นต์งวดงาน (BOQ) % : {totalBoqPercent:0.00}%\n" +
                                      $"รวม งบประมาณ: {totalBoqBudget:N2} บาท";

            lblTotalPercentage.ForeColor = totalCompletion == 100 ? Color.Green : Color.Blue;
        }

        //เปิดปิด การทำงาน phase
        private void clearPhaseForm()
        {   //ล้างข้อมูล
            txtPhaseDetail.Clear();
            txtboqPercentage.Clear();
            cmbPhaseNumber.SelectedIndex = -1;
            txtcompletionPercentage.Clear();
        }
        private void ClearPhaseData()
        {
            projectPhases.Clear();               // เคลียร์ List<ProjectPhase>
            dtgvPhase.Rows.Clear();              // เคลียร์แถวใน DataGridView
            cmbPhaseNumber.SelectedIndex = -1;   // รีเซ็ต dropdown
            txtPhaseDetail.Clear();
            txtboqPercentage.Clear();
            txtcompletionPercentage.Clear();

        }
        private void PhaseDisableEditing()
        {   //ปิดการทำงาน
            txtPhaseDetail.ReadOnly = true;
            txtboqPercentage.ReadOnly = true;
            cmbPhaseNumber.Enabled = false;
            txtPhaseDetail.Enabled = false;
            txtboqPercentage.Enabled = false;
            txtcompletionPercentage.Enabled = false;
        }
        private void PhaseAbleOn()
        {//เปิดการทำงาน
            txtPhaseDetail.ReadOnly = false;
            txtboqPercentage.ReadOnly = false;
            cmbPhaseNumber.Enabled = true;
            txtPhaseDetail.Enabled = true;
            txtboqPercentage.Enabled = true;
            txtcompletionPercentage.Enabled = true;
        }
        private void btnEditPhase_Click(object sender, EventArgs e)
        {
            if (currentEditingPhase != null)  // ✅ ตรวจสอบว่ามีเฟสที่ถูกเลือกหรือไม่
            {
                // ✅ เปิดการแก้ไขเฉพาะรายละเอียดงาน & เปอร์เซ็นต์
                txtPhaseDetail.ReadOnly = false;
                txtboqPercentage.ReadOnly = false;
                txtPhaseDetail.Enabled = true;
                txtboqPercentage.Enabled = true;

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
                // ✅ ตรวจสอบความถูกต้องก่อน
                if (!ValidateProjectData())
                    return;

                ProjectDAL dal = new ProjectDAL();
                EmployeeAssignmentDAL assignDal = new EmployeeAssignmentDAL();
                int projectID;

                bool isUpdate = selectedProjectID != null;

                if (isUpdate)
                {
                    // ✅ แก้ไขโครงการเดิม
                    projectID = selectedProjectID.Value;
                }
                else
                {
                    // ✅ สร้างโครงการใหม่
                    projectID = dal.GenerateProjectID();
                }

                // ✅ เตรียมข้อมูล Project
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
                    EmployeeID = selectedEmployeeID,   // ✅ ใช้ string ตรง ๆ
                    CustomerID = int.Parse(selectedCustomerID)
                };

                bool success;

                if (isUpdate)
                {
                    // ✅ UPDATE Project + Phases + Files
                    success = dal.UpdateProjectWithPhases(project, projectPhases, fileConstructionBytes, fileDemolitionBytes);

                    if (success)
                    {
                        // ✅ ลบ Assignments เก่า แล้วเพิ่มใหม่
                        assignDal.DeleteAssignmentsByProjectID(project.ProjectID);
                        foreach (var assign in assignedEmployees)
                        {
                            assign.ProjectID = project.ProjectID;
                            assignDal.InsertAssignment(assign);
                        }

                        MessageBox.Show("อัปเดตโครงการเรียบร้อย", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // ✅ INSERT Project + Phases + Files
                    success = dal.InsertProjectWithPhases(project, projectPhases, fileConstructionBytes, fileDemolitionBytes);

                    if (success)
                    {
                        foreach (var assign in assignedEmployees)
                        {
                            assign.ProjectID = projectID;
                            assignDal.InsertAssignment(assign);
                        }

                        MessageBox.Show("บันทึกโครงการเรียบร้อย", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                if (!success)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึก", "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ รีเซ็ต + โหลดใหม่
                LoadProjectData();
                ClearForm();
                ReadOnlyControls_close();
                EnableControls_close();
                selectedProjectID = null; // 🟢 รีเซ็ตโหมดหลังบันทึก

                // ✅ สลับปุ่มกลับเป็นโหมด Save (ถ้าคุณใช้ toggle UI)
                btnSave.Text = "บันทึก";
            }
            catch (Exception ex)
            {
                MessageBox.Show("ข้อผิดพลาด: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            if (selectedProjectID == null)
            {
                MessageBox.Show("กรุณาเลือกโครงการก่อนแก้ไข", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EnableControls_open();     // เปิด control
            ReadOnlyControls_open();   // เปิดให้พิมพ์ได้
            //txtProjectName.Focus();    // โฟกัสชื่อ

            // 👉 ถ้ามีการเปลี่ยนปุ่ม Save เป็น "อัปเดต" ก็ทำตรงนี้
            btnSave.Text = "อัปเดต";
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //ลบ
            if (dtgvProject.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกโครงการที่ต้องการลบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔍 ดึง ProjectID จากแถวที่เลือก
            int selectedProjectID = Convert.ToInt32(dtgvProject.SelectedRows[0].Cells["ProjectID"].Value);

            var confirm = MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบโครงการนี้?", "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                ProjectDAL dal = new ProjectDAL();
                bool success = dal.DeleteProjectWithPhases(selectedProjectID);

                if (success)
                {
                    MessageBox.Show("ลบโครงการเรียบร้อย", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProjectData(); // โหลดข้อมูลใหม่
                }
                else
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการลบโครงการ", "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
            txtboqPercentage.Enabled = true;
            //เพิ่มใหม่
            txtcompletionPercentage.Enabled = true;


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
            txtboqPercentage.Enabled = false;
            //
            txtcompletionPercentage.Enabled= false;

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
            txtboqPercentage.ReadOnly = false;
            //แก้ไขใหม่
            txtWorkingDate.ReadOnly = false;
            txtcompletionPercentage.ReadOnly = false;
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
            txtboqPercentage.ReadOnly = true;
            //แก้ไขใหม่
            txtWorkingDate.ReadOnly = true;
            txtcompletionPercentage.ReadOnly = true;
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
            txtboqPercentage.Clear();
            txtWorkingDate.Clear();
            txtcompletionPercentage.Clear();

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
            txtcompletionPercentage.Text = "";


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
        // การเลือกข้อมูลพนักงาน
        private void btnSearchEmployee_Click(object sender, EventArgs e)
        {
            using (SearchForm searchForm = new SearchForm("Employee"))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    selectedEmployeeID = searchForm.SelectedID;
                    txtEmployeeName.Text = searchForm.SelectedName;
                    txtEmployeeLastName.Text = searchForm.SelectedLastName;
                    txtEmployeeRole.Text = searchForm.SelectedIDCardOrRole;

                    // ✅ สร้างอ็อบเจกต์พนักงานใหม่ (ใช้ string แทน int)
                    EmployeeAssignment newAssign = new EmployeeAssignment
                    {
                        EmployeeID = selectedEmployeeID,
                        EmployeeName = txtEmployeeName.Text,
                        EmployeeLastName = txtEmployeeLastName.Text,
                        AssignRole = _loggedInRole,
                        AssignBy = _loggedInUser,
                        AssignDate = DateTime.Now
                    };

                    // ✅ ตรวจสอบว่าถูกเพิ่มไปแล้วหรือยัง
                    if (!assignedEmployees.Any(a => a.EmployeeID == newAssign.EmployeeID))
                    {
                        assignedEmployees.Add(newAssign);
                        // RefreshAssignedEmployeesGrid(); // ถ้ามี
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

        private int CalculateWorkingDays(DateTime startDate, DateTime endDate)
        {
            int workingDays = 0;
            DateTime currentDate = startDate;

            while (currentDate <= endDate)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }

                currentDate = currentDate.AddDays(1);
            }

            return workingDays;
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
                MessageBox.Show("กรุณากรอกหมายเหตุที่เกี่ยวข้องกับโครงการ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            // ✅ ตรวจสอบความถูกต้องของ % และงบประมาณรวมของเฟส
            decimal totalBudget = 0;
            decimal.TryParse(txtBudget.Text.Replace(",", ""), out totalBudget);

            decimal totalCompletion = projectPhases.Sum(p => p.PhasePercent);
            decimal totalBoqBudget = projectPhases.Sum(p => p.PhaseBudget);
            decimal totalBoqPercent = 0;

            // คำนวณ BOQ % จากงบแต่ละเฟส
            if (totalBudget > 0)
            {
                foreach (var phase in projectPhases)
                {
                    totalBoqPercent += (phase.PhaseBudget * 100) / totalBudget;
                }
            }

            // ตรวจสอบทั้ง 3 เงื่อนไข
            List<string> errors = new List<string>();

            if (totalCompletion != 100)
                errors.Add($"เปอร์เซ็นต์ความก้าวหน้ารวมต้องเท่ากับ 100% (ปัจจุบัน: {totalCompletion:0.00}%)");

            if (totalBoqPercent != 100)
                errors.Add($"เปอร์เซ็นต์ BOQ รวมต้องเท่ากับ 100% (ปัจจุบัน: {totalBoqPercent:0.00}%)");

            if (Math.Round(totalBoqBudget, 2) != Math.Round(totalBudget, 2))
                errors.Add($"งบประมาณรวมของเฟส ({totalBoqBudget:N2}) ต้องเท่ากับงบประมาณโครงการ ({totalBudget:N2})");

            if (errors.Any())
            {
                MessageBox.Show(string.Join("\n", errors), "ข้อผิดพลาดในการตรวจสอบเฟส", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true; // ✅ ผ่านทุกเงื่อนไข
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //PDFPreview

        //เมธอดเปิดฟอร์ม Preview
        private void ShowPDFPreviewFromBytes(byte[] pdfBytes, Control targetControl)
        {
            if (pdfBytes == null) return;

            // ถ้า Form ยังไม่ได้เปิด
            if (pdfPreviewForm == null || pdfPreviewForm.IsDisposed)
            {
                // ✅ สร้างและเปิด Form ใหม่
                pdfPreviewForm = new FormPDFPreview(pdfBytes);

                // ✅ วางตำแหน่ง Form ใต้ปุ่ม
                var location = targetControl.PointToScreen(new Point(0, targetControl.Height));
                pdfPreviewForm.Location = location;

                pdfPreviewForm.Show();
            }

            StartHoverTimer(targetControl); // เริ่มเช็กเมื่อเมาส์ออก
        }
        //Timer เช็กเมาส์ "ออก" แล้วปิด Form
        private void StartHoverTimer(Control buttonControl)
        {
            if (hoverCheckTimer == null)
            {
                hoverCheckTimer = new Timer();
                hoverCheckTimer.Interval = 300;
                hoverCheckTimer.Tick += (s, e) =>
                {
                    Point mousePos = Cursor.Position;

                    // ✅ เช็กเมาส์อยู่บนปุ่ม
                    bool overButton = buttonControl.Bounds.Contains(buttonControl.Parent.PointToClient(mousePos));

                    // ✅ เช็กเมาส์อยู่บนฟอร์ม Preview
                    bool overPreview = pdfPreviewForm != null && !pdfPreviewForm.IsDisposed && pdfPreviewForm.Bounds.Contains(mousePos);

                    // ❌ ถ้าเมาส์อยู่นอกทั้ง 2 จุด ให้ปิดฟอร์ม
                    if (!overButton && !overPreview)
                    {
                        if (pdfPreviewForm != null && !pdfPreviewForm.IsDisposed)
                        {
                            pdfPreviewForm.Close();
                            pdfPreviewForm = null;
                        }

                        hoverCheckTimer.Stop();
                    }
                };
            }

            hoverCheckTimer.Start();
        }

        private void btnInsertBlueprintFile_MouseEnter(object sender, EventArgs e)
        {
            isPreviewingDemolition = false; // บอกว่าเป็น Blueprint
            if (fileConstructionBytes != null)
            {
                ShowPDFPreviewFromBytes(fileConstructionBytes, btnInsertBlueprintFile);
            }
        }

        private void btnInsertDemolitionFile_MouseEnter(object sender, EventArgs e)
        {
            isPreviewingDemolition = true; // บอกว่าเป็น Demolition
            if (fileDemolitionBytes != null)
            {
                ShowPDFPreviewFromBytes(fileDemolitionBytes, btnInsertDemolitionFile);
            }
        }


    }
}
