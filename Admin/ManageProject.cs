using JRSApplication.Components;
using JRSApplication.Components.Service;
using JRSApplication.Data_Access_Layer;
using MySql.Data.MySqlClient;
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
        // ---------- ใช้ใน GenerateNextOrderNumber ----------
        private readonly string connectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        // ---------- แฟลกกัน event ระหว่างโปรแกรม set ค่าเอง ----------
        private bool _suppressUIEvents = false;

        // ตัวแปรระดับคลาส
        private string selectedCustomerID = "";
        private string selectedEmployeeID = "";

        private string _loggedInUser;
        private string _loggedInRole;

        private List<EmployeeAssignment> assignedEmployees = new List<EmployeeAssignment>(); // เก็บพนักงานที่เลือกไว้ก่อนบันทึก

        // เก็บรายการเฟสทั้งหมด
        private List<ProjectPhase> projectPhases = new List<ProjectPhase>();

        // ใช้เก็บเฟสที่กำลังแก้ไข
        private ProjectPhase currentEditingPhase = null;

        // file PDF
        private byte[] fileConstructionBytes;
        private byte[] fileDemolitionBytes;

        // PDFPreview
        private FormPDFPreview pdfPreviewForm = null; // ฟอร์ม Preview
        private Timer hoverCheckTimer;                // Timer เช็กเมาส์ออก
        private bool isPreviewingDemolition = false;  // Flag

        private int? selectedProjectID = null; // null = ยังไม่มีการเลือก

        protected override Point ScrollToControl(Control activeControl)
        {
            // ไม่ต้องเลื่อนจออัตโนมัติเมื่อ focus เปลี่ยน
            return this.DisplayRectangle.Location;
            // หรือใช้ return this.AutoScrollPosition; ก็ได้เช่นกัน
        }

        public ManageProject(string fullName, string role)
        {
            InitializeComponent();

            _loggedInUser = fullName;
            _loggedInRole = role;

            // -------------------------------
            // ตั้งค่าตัวเลือกค้นหาใน searchboxProject
            // -------------------------------
            try
            {
                string roleKey = string.IsNullOrWhiteSpace(role)
                    ? searchboxProject.DefaultRole
                    : role;

                searchboxProject.SetRoleAndFunction(roleKey, "จัดการข้อมูลโครงการ");
            }
            catch
            {
                // กัน error เผื่อคอนโทรลยังไม่พร้อมหรือ role ไม่ถูกต้อง
            }

            // ผูกอีเวนต์ SearchTriggered จาก SearchboxControl
            searchboxProject.SearchTriggered += SearchboxProject_SearchTriggered;

            LoadPhaseNumberDropdown();
            InitializePhaseDataGridView();
            InitializeDataGridViewProject();

            // โหลดข้อมูลโปรเจกต์
            LoadProjectData();
            this.Load += (s, e) => LoadProjectData();

            // ป้องกัน loop คำนวณตอน initialize
            _suppressUIEvents = true;
            try
            {
                txtWorkingDate.Text = "";
                dtpkStartDate.Value = DateTime.Now;
                dtpkEndDate.Value = DateTime.Now;
            }
            finally
            {
                _suppressUIEvents = false;
            }

            // ผูกอีเวนต์ (ถ้ายังไม่ได้ผูกใน Designer)
            this.dtpkStartDate.ValueChanged += dtpkStartDate_ValueChanged;
            this.txtWorkingDate.TextChanged += txtWorkingDate_TextChanged;
            this.dtgvProject.CellClick += dtgvProject_CellClick;
        }

        // ------------------------------------------------------------
        // การโหลดและจัดการตาราง Project
        // ------------------------------------------------------------
        private void InitializeDataGridViewProject()
        {
            if (dtgvProject.Columns.Count == 0)
            {
                dtgvProject.AllowUserToAddRows = false;

                dtgvProject.Columns.Add("ProjectID", "รหัสโครงการ");
                dtgvProject.Columns.Add("ProjectName", "ชื่อโครงการ");

                // คอลัมน์เลขที่สัญญา
                dtgvProject.Columns.Add("ProjectNumber", "เลขที่สัญญา");

                dtgvProject.Columns.Add("ProjectStart", "วันที่เริ่มโครงการ");
                dtgvProject.Columns.Add("ProjectEnd", "วันที่สิ้นสุดโครงการ");
                dtgvProject.Columns.Add("ProjectBudget", "งบประมาณ (บาท)");
                dtgvProject.Columns.Add("CurrentPhaseNumber", "จำนวนเฟสงาน");
                dtgvProject.Columns.Add("CustomerFullName", "ลูกค้า");
                dtgvProject.Columns.Add("EmployeeFullName", "ผู้ดูแลโครงการ");

                dtgvProject.Columns["ProjectID"].Width = 80;
                dtgvProject.Columns["ProjectID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProject.Columns["ProjectID"].ReadOnly = true;

                dtgvProject.Columns["ProjectName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dtgvProject.Columns["ProjectName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProject.Columns["ProjectName"].ReadOnly = true;

                dtgvProject.Columns["ProjectNumber"].Width = 120;
                dtgvProject.Columns["ProjectNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProject.Columns["ProjectNumber"].ReadOnly = true;

                dtgvProject.Columns["ProjectStart"].Width = 120;
                dtgvProject.Columns["ProjectStart"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProject.Columns["ProjectStart"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dtgvProject.Columns["ProjectStart"].ReadOnly = true;

                dtgvProject.Columns["ProjectEnd"].Width = 120;
                dtgvProject.Columns["ProjectEnd"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProject.Columns["ProjectEnd"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dtgvProject.Columns["ProjectEnd"].ReadOnly = true;

                dtgvProject.Columns["ProjectBudget"].Width = 150;
                dtgvProject.Columns["ProjectBudget"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProject.Columns["ProjectBudget"].DefaultCellStyle.Format = "N2";
                dtgvProject.Columns["ProjectBudget"].ReadOnly = true;

                dtgvProject.Columns["CurrentPhaseNumber"].Width = 120;
                dtgvProject.Columns["CurrentPhaseNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProject.Columns["CurrentPhaseNumber"].ReadOnly = true;

                dtgvProject.Columns["CustomerFullName"].Width = 150;
                dtgvProject.Columns["CustomerFullName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProject.Columns["CustomerFullName"].ReadOnly = true;

                dtgvProject.Columns["EmployeeFullName"].Width = 150;
                dtgvProject.Columns["EmployeeFullName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProject.Columns["EmployeeFullName"].ReadOnly = true;

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
            InitializeDataGridViewProject();

            ProjectDAL dal = new ProjectDAL();
            List<Project> projects = dal.GetAllProjects();

            _suppressUIEvents = true;
            try
            {
                dtgvProject.Rows.Clear();

                foreach (var project in projects)
                {
                    dtgvProject.Rows.Add(
                        project.ProjectID,
                        project.ProjectName,
                        project.ProjectNumber, // เลขที่สัญญา
                        project.ProjectStart.ToString("dd/MM/yyyy"),
                        project.ProjectEnd.ToString("dd/MM/yyyy"),
                        project.ProjectBudget.ToString("N2"),
                        project.CurrentPhaseNumber,
                        project.CustomerName ?? "ไม่ระบุ",
                        project.EmployeeName ?? "ไม่ระบุ"
                    );
                }
            }
            finally
            {
                _suppressUIEvents = false;
            }

            // รีเฟรชฟิลเตอร์ให้สัมพันธ์กับค่าปัจจุบันใน Searchbox
            ApplyProjectGridFilter(searchboxProject.SelectedSearchBy, searchboxProject.Keyword);
        }

        private void LoadFullProjectByID(int projectId)
        {
            _suppressUIEvents = true;
            try
            {
                ProjectDAL dal = new ProjectDAL();
                Project project = dal.GetProjectDetailsById(projectId);

                if (project == null)
                {
                    MessageBox.Show("ไม่พบข้อมูลโครงการที่เลือก!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                selectedProjectID = project.ProjectID;

                // ข้อมูล Project
                txtProjectName.Text = project.ProjectName;
                txtProjectDetail.Text = project.ProjectDetail;
                txtProjectAddress.Text = project.ProjectAddress;
                txtBudget.Text = project.ProjectBudget.ToString("N2");
                txtRemark.Text = project.Remark;
                txtNumber.Text = project.ProjectNumber;

                dtpkStartDate.Value = project.ProjectStart;
                dtpkEndDate.Value = project.ProjectEnd;

                int workingDays = CalculateWorkingDays(project.ProjectStart, project.ProjectEnd);
                txtWorkingDate.Text = workingDays.ToString();

                cmbCurrentPhaseNumber.SelectedItem = project.CurrentPhaseNumber.ToString();

                txtCustomerName.Text = project.CustomerName;
                txtEmployeeName.Text = project.EmployeeName;

                // ไฟล์
                fileConstructionBytes = project.ProjectFile?.ConstructionBlueprint;
                fileDemolitionBytes = project.ProjectFile?.DemolitionModel;

                btnInsertBlueprintFile.Text = (fileConstructionBytes != null) ? "ไฟล์แนบแล้ว" : "เลือกไฟล์";
                btnInsertDemolitionFile.Text = (fileDemolitionBytes != null) ? "ไฟล์แนบแล้ว" : "เลือกไฟล์";

                // เฟสงาน
                projectPhases = project.Phases ?? new List<ProjectPhase>();
                dtgvPhase.Rows.Clear();

                foreach (var phase in projectPhases)
                {
                    dtgvPhase.Rows.Add(
                        phase.PhaseNumber,
                        phase.PhaseDetail,
                        phase.PhaseBudget.ToString("N2"),
                        phase.PhasePercent.ToString("0.00") + " %",
                        (project.ProjectBudget > 0 ? (phase.PhaseBudget * 100m / project.ProjectBudget) : 0m).ToString("0.00")
                    );
                }

                CalculateTotalPhasePercentage();
            }
            finally
            {
                _suppressUIEvents = false;
            }
        }

        // นับจำนวน "วันทำงาน" แบบไม่รวมวันอาทิตย์
        private int CalculateWorkingDays(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate) return 0;

            int workingDays = 0;
            DateTime d = startDate.Date;
            DateTime last = endDate.Date;

            while (d <= last)
            {
                if (d.DayOfWeek != DayOfWeek.Sunday)
                    workingDays++;

                d = d.AddDays(1);
            }
            return workingDays;
        }

        private void dtgvProject_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int projectId = Convert.ToInt32(dtgvProject.Rows[e.RowIndex].Cells["ProjectID"].Value);
                LoadFullProjectByID(projectId);
            }
        }

        // ------------------------------------------------------------
        // การจัดการข้อมูล Phase
        // ------------------------------------------------------------
        private void InitializePhaseDataGridView()
        {
            if (dtgvPhase.Columns.Count == 0)
            {
                dtgvPhase.AllowUserToAddRows = false;

                dtgvPhase.Columns.Add("PhaseNumber", "เฟสที่");
                dtgvPhase.Columns.Add("PhaseDetail", "รายละเอียดการดำเนินงาน");
                dtgvPhase.Columns.Add("PhaseBudget", "งบประมาณเฟส (บาท)");
                dtgvPhase.Columns.Add("PhasePercent", "% ความก้าวหน้า");
                dtgvPhase.Columns.Add("BoqPercent", "% BOQ");

                dtgvPhase.Columns["PhaseNumber"].Width = 60;
                dtgvPhase.Columns["PhaseNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvPhase.Columns["PhaseNumber"].ReadOnly = true;

                dtgvPhase.Columns["PhaseDetail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dtgvPhase.Columns["PhaseDetail"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dtgvPhase.Columns["PhaseDetail"].ReadOnly = true;

                dtgvPhase.Columns["PhaseBudget"].Width = 120;
                dtgvPhase.Columns["PhaseBudget"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dtgvPhase.Columns["PhaseBudget"].DefaultCellStyle.Format = "N2";
                dtgvPhase.Columns["PhaseBudget"].ReadOnly = true;

                dtgvPhase.Columns["PhasePercent"].Width = 100;
                dtgvPhase.Columns["PhasePercent"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dtgvPhase.Columns["PhasePercent"].DefaultCellStyle.Format = "0.00";
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
            dtgvPhase.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
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
            _suppressUIEvents = true;
            try
            {
                dtgvPhase.Rows.Clear();

                decimal totalBudget = 0;
                decimal.TryParse(txtBudget.Text.Replace(",", ""), out totalBudget);

                foreach (var phase in projectPhases)
                {
                    decimal boqPercent = 0;
                    if (totalBudget > 0)
                    {
                        boqPercent = (phase.PhaseBudget * 100) / totalBudget;
                    }

                    dtgvPhase.Rows.Add(
                        phase.PhaseNumber,
                        phase.PhaseDetail,
                        phase.PhaseBudget.ToString("N2"),
                        phase.PhasePercent.ToString("0.00"),
                        boqPercent.ToString("0.00")
                    );
                }

                CalculateTotalPhasePercentage();
            }
            finally
            {
                _suppressUIEvents = false;
            }
        }

        private void dtgvPhase_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvPhase.Rows[e.RowIndex];

                int selectedPhaseNumber = int.Parse(row.Cells["PhaseNumber"].Value.ToString());
                currentEditingPhase = projectPhases.FirstOrDefault(p => p.PhaseNumber == selectedPhaseNumber);
                if (currentEditingPhase != null)
                {
                    _suppressUIEvents = true;
                    try
                    {
                        cmbPhaseNumber.SelectedItem = currentEditingPhase.PhaseNumber.ToString();
                        txtPhaseDetail.Text = currentEditingPhase.PhaseDetail;
                        txtboqPercentage.Text = currentEditingPhase.PhasePercent.ToString("0.00");
                        txtcompletionPercentage.Text = currentEditingPhase.PhasePercent.ToString("0.00");
                    }
                    finally
                    {
                        _suppressUIEvents = false;
                    }

                    PhaseDisableEditing();
                    btnTurnoffEditing.Visible = true;
                }
            }
        }

        private void btnAddPhase_Click(object sender, EventArgs e)
        {
            if (cmbPhaseNumber.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtPhaseDetail.Text) ||
                string.IsNullOrWhiteSpace(txtboqPercentage.Text) ||
                string.IsNullOrWhiteSpace(txtcompletionPercentage.Text))
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            if (string.IsNullOrWhiteSpace(txtBudget.Text))
            {
                MessageBox.Show("กรุณากรอกงบประมาณก่อนเพิ่มเฟส!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalBudget = 0;
            decimal.TryParse(txtBudget.Text.Replace(",", ""), out totalBudget);
            decimal phaseBudget = (totalBudget * boqPercent) / 100;

            int phaseNumber = int.Parse(cmbPhaseNumber.SelectedItem.ToString());

            decimal totalCompletionPercent = projectPhases.Sum(p => p.PhasePercent);
            decimal totalBoqPercent = 0;

            foreach (var p in projectPhases)
            {
                totalBoqPercent += (p.PhaseBudget * 100) / totalBudget;
            }

            if (btnAddPhase.Text == "บันทึกข้อมูล" && currentEditingPhase != null)
            {
                totalCompletionPercent -= currentEditingPhase.PhasePercent;
                totalBoqPercent -= (currentEditingPhase.PhaseBudget * 100) / totalBudget;
            }

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

            if (btnAddPhase.Text == "บันทึกข้อมูล" && currentEditingPhase != null)
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

            LoadPhaseToGridView();
            clearPhaseForm();
            PhaseAbleOn();
            btnTurnoffEditing.Visible = false;
            btnAddPhase.Text = "เพิ่ม";
            currentEditingPhase = null;
        }

        // ------------------------------------------------------------
        // การคำนวณและรีเซ็ต Phase
        // ------------------------------------------------------------
        private void CalculateTotalPhasePercentage()
        {
            decimal totalCompletion = projectPhases.Sum(p => p.PhasePercent);
            decimal totalBoqPercent = 0;
            decimal totalBoqBudget = 0;

            decimal totalBudget = 0;
            decimal.TryParse(txtBudget.Text.Replace(",", ""), out totalBudget);

            foreach (var phase in projectPhases)
            {
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

        private void clearPhaseForm()
        {
            txtPhaseDetail.Clear();
            txtboqPercentage.Clear();
            cmbPhaseNumber.SelectedIndex = -1;
            txtcompletionPercentage.Clear();
        }

        private void ClearPhaseData()
        {
            projectPhases.Clear();
            dtgvPhase.Rows.Clear();
            cmbPhaseNumber.SelectedIndex = -1;
            txtPhaseDetail.Clear();
            txtboqPercentage.Clear();
            txtcompletionPercentage.Clear();
        }

        private void PhaseDisableEditing()
        {
            txtPhaseDetail.ReadOnly = true;
            txtboqPercentage.ReadOnly = true;
            cmbPhaseNumber.Enabled = false;
            txtPhaseDetail.Enabled = false;
            txtboqPercentage.Enabled = false;
            txtcompletionPercentage.Enabled = false;
        }

        private void PhaseAbleOn()
        {
            txtPhaseDetail.ReadOnly = false;
            txtboqPercentage.ReadOnly = false;
            cmbPhaseNumber.Enabled = true;
            txtPhaseDetail.Enabled = true;
            txtboqPercentage.Enabled = true;
            txtcompletionPercentage.Enabled = true;
        }

        private void btnEditPhase_Click(object sender, EventArgs e)
        {
            if (currentEditingPhase != null)
            {
                txtPhaseDetail.ReadOnly = false;
                txtboqPercentage.ReadOnly = false;
                txtPhaseDetail.Enabled = true;
                txtboqPercentage.Enabled = true;

                cmbPhaseNumber.Enabled = false;

                btnAddPhase.Text = "บันทึก";
            }
            else
            {
                MessageBox.Show("กรุณาเลือกเฟสที่ต้องการแก้ไข!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnTurnoffEditing_Click(object sender, EventArgs e)
        {
            PhaseAbleOn();
            clearPhaseForm();
            btnTurnoffEditing.Visible = false;
        }

        // ------------------------------------------------------------
        // การจัดการ ComboBox(เฟสงาน)
        // ------------------------------------------------------------
        private void LoadPhaseNumberDropdown()
        {
            cmbCurrentPhaseNumber.Items.Clear();
            for (int i = 3; i <= 15; i++)
            {
                cmbCurrentPhaseNumber.Items.Add(i.ToString());
            }
        }

        private void cmbCurrentPhaseNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCurrentPhaseNumber.SelectedItem != null)
            {
                int totalPhases = int.Parse(cmbCurrentPhaseNumber.SelectedItem.ToString());

                cmbPhaseNumber.Items.Clear();
                for (int i = 1; i <= totalPhases; i++)
                {
                    cmbPhaseNumber.Items.Add(i.ToString());
                }

                if (cmbPhaseNumber.Items.Count > 0)
                {
                    cmbPhaseNumber.SelectedIndex = 0;
                }
            }
        }

        // ------------------------------------------------------------
        // ปุ่ม Action หลัก
        // ------------------------------------------------------------
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateProjectData())
                    return;

                ProjectDAL dal = new ProjectDAL();
                EmployeeAssignmentDAL assignDal = new EmployeeAssignmentDAL();
                int projectID;

                bool isUpdate = selectedProjectID != null;

                if (isUpdate)
                {
                    projectID = selectedProjectID.Value;
                }
                else
                {
                    projectID = dal.GenerateProjectID();
                }

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
                    EmployeeID = selectedEmployeeID,
                    CustomerID = int.Parse(selectedCustomerID)
                };

                bool success;

                if (isUpdate)
                {
                    success = dal.UpdateProjectWithPhases(project, projectPhases, fileConstructionBytes, fileDemolitionBytes);

                    if (success)
                    {
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
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึก!", "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                LoadProjectData();
                ClearForm();
                ReadOnlyControls_close();
                EnableControls_close();
                selectedProjectID = null;

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("ข้อผิดพลาด: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _suppressUIEvents = true;
            try
            {
                ClearForm();
                assignedEmployees.Clear();
                projectPhases.Clear();
                dtgvPhase.Rows.Clear();
                currentEditingPhase = null;
                selectedProjectID = null;

                EnableControls_open();
                ReadOnlyControls_open();

                txtNumber.Text = GenerateNextOrderNumber();
                txtNumber.ReadOnly = false;
                txtNumber.Enabled = true;

                

                txtProjectName.Focus();
            }
            finally
            {
                _suppressUIEvents = false;
            }

            if (!string.IsNullOrWhiteSpace(txtWorkingDate.Text))
            {
                CalculateEndDateFromWorkingDays(false);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedProjectID == null)
            {
                MessageBox.Show("กรุณาเลือกโครงการก่อนแก้ไข!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EnableControls_open();
            ReadOnlyControls_open();
            btnSave.Text = "อัปเดต";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dtgvProject.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกโครงการที่ต้องการลบ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedProjectID = Convert.ToInt32(dtgvProject.SelectedRows[0].Cells["ProjectID"].Value);

            var confirm = MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบโครงการนี้?", "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                ProjectDAL dal = new ProjectDAL();
                bool success = dal.DeleteProjectWithPhases(selectedProjectID);

                if (success)
                {
                    MessageBox.Show("ลบโครงการเรียบร้อย!", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProjectData();
                }
                else
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการลบโครงการ!", "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ------------------------------------------------------------
        // การจัดการฟอร์ม
        // ------------------------------------------------------------
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
            txtcompletionPercentage.Enabled = true;

            txtWorkingDate.Enabled = true;
            dtpkStartDate.Enabled = true;

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
            txtcompletionPercentage.Enabled = false;

            txtWorkingDate.Enabled = true;
            dtpkStartDate.Enabled = false;

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
            txtWorkingDate.ReadOnly = true;
            txtcompletionPercentage.ReadOnly = true;
        }

        private void ClearForm()
        {
            _suppressUIEvents = true;
            try
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

                if (txtNumber != null)
                {
                    txtNumber.Clear();
                    txtNumber.ReadOnly = false;
                    txtNumber.Enabled = true;
                }

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

                btnInsertBlueprintFile.Text = "เลือกไฟล์";
                btnInsertDemolitionFile.Text = "เลือกไฟล์";
                fileConstructionBytes = null;
                fileDemolitionBytes = null;

                ClearPhaseData();
            }
            finally
            {
                _suppressUIEvents = false;
            }
        }

        // ------------------------------------------------------------
        // การเลือกผู้ดูแล/ลูกค้า
        // ------------------------------------------------------------
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

                    EmployeeAssignment newAssign = new EmployeeAssignment
                    {
                        EmployeeID = selectedEmployeeID,
                        EmployeeName = txtEmployeeName.Text,
                        EmployeeLastName = txtEmployeeLastName.Text,
                        AssignRole = _loggedInRole,
                        AssignBy = _loggedInUser,
                        AssignDate = DateTime.Now
                    };

                    if (!assignedEmployees.Any(a => a.EmployeeID == newAssign.EmployeeID))
                    {
                        assignedEmployees.Add(newAssign);
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
            OpenSearchForm("Customer",
                txtCustomerName,      // ชื่อ
                txtCustomerLastName,  // นามสกุล
                txtCustomerIDCard,    // เลขบัตรประชาชน
                txtCustomerPhone,     // เบอร์โทร
                txtCustomerEmail);    // อีเมล
        }



        private void OpenSearchForm(string searchType, TextBox nameTextBox, TextBox lastNameTextBox, TextBox idCardOrRoleTextBox, TextBox phoneTextBox = null, TextBox emailTextBox = null)
        {
            using (SearchForm searchForm = new SearchForm(searchType))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    if (searchType == "Customer")
                    {
                        selectedCustomerID = searchForm.SelectedID;
                    }
                    else if (searchType == "Employee")
                    {
                        selectedEmployeeID = searchForm.SelectedID;
                    }

                    nameTextBox.Text = searchForm.SelectedName;
                    lastNameTextBox.Text = searchForm.SelectedLastName;
                    idCardOrRoleTextBox.Text = searchForm.SelectedIDCardOrRole;

                    if (phoneTextBox != null) phoneTextBox.Text = searchForm.SelectedPhone;
                    if (emailTextBox != null) emailTextBox.Text = searchForm.SelectedEmail;
                }
            }
        }

        // ------------------------------------------------------------
        // Budget textbox handlers
        // ------------------------------------------------------------
        private void txtBudget_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && txtBudget.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txtBudget_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtBudget.Text, out decimal budget))
            {
                txtBudget.Text = budget.ToString("#,##0.00");
            }
        }

        private void txtBudget_Enter(object sender, EventArgs e)
        {
            txtBudget.Text = txtBudget.Text.Replace(",", "");
        }

        // ------------------------------------------------------------
        // คำนวณจำนวนวันของโครงการ
        // ------------------------------------------------------------
        private void CalculateEndDateFromWorkingDays(bool isUserAction = false)
        {
            if (_suppressUIEvents) return;

            if (!int.TryParse(txtWorkingDate.Text.Trim(), out int workingDays) || workingDays <= 0)
            {
                if (isUserAction && (txtWorkingDate.Focused || this.ContainsFocus))
                {
                    MessageBox.Show("กรุณากรอกจำนวนวันทำงานให้ถูกต้อง!", "แจ้งเตือน",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
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

            if (currentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                if (isUserAction)
                {
                    DialogResult result = MessageBox.Show(
                        "วันที่สิ้นสุดโครงการตรงกับวันอาทิตย์ คุณต้องการขยายระยะเวลาอีก 1 วันหรือไม่?",
                        "ยืนยันการเปลี่ยนแปลง",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        _suppressUIEvents = true;
                        try
                        {
                            txtWorkingDate.Text = (workingDays + 1).ToString();
                        }
                        finally
                        {
                            _suppressUIEvents = false;
                        }
                        CalculateEndDateFromWorkingDays(true);
                        return;
                    }
                }
                else
                {
                    currentDate = currentDate.AddDays(1);
                }
            }

            _suppressUIEvents = true;
            try
            {
                dtpkEndDate.Value = currentDate;
            }
            finally
            {
                _suppressUIEvents = false;
            }
        }

        private void dtpkStartDate_ValueChanged(object sender, EventArgs e)
        {
            if (_suppressUIEvents) return;
            CalculateEndDateFromWorkingDays(true);
        }

        private void txtWorkingDate_TextChanged(object sender, EventArgs e)
        {
            if (_suppressUIEvents) return;
            CalculateEndDateFromWorkingDays(true);
        }

        // ------------------------------------------------------------
        // PDFPreview
        // ------------------------------------------------------------
        private void ShowPDFPreviewFromBytes(byte[] pdfBytes, Control targetControl)
        {
            if (pdfBytes == null) return;

            if (pdfPreviewForm == null || pdfPreviewForm.IsDisposed)
            {
                pdfPreviewForm = new FormPDFPreview(pdfBytes);

                var location = targetControl.PointToScreen(new Point(0, targetControl.Height));
                pdfPreviewForm.Location = location;

                pdfPreviewForm.Show();
            }

            StartHoverTimer(targetControl);
        }

        private void StartHoverTimer(Control buttonControl)
        {
            if (hoverCheckTimer == null)
            {
                hoverCheckTimer = new Timer();
                hoverCheckTimer.Interval = 300;
                hoverCheckTimer.Tick += (s, e) =>
                {
                    Point mousePos = Cursor.Position;

                    Rectangle buttonScreenBounds = new Rectangle(
                        buttonControl.PointToScreen(Point.Empty),
                        buttonControl.Size
                    );

                    bool overButton = buttonScreenBounds.Contains(mousePos);
                    bool overPreview = pdfPreviewForm != null && !pdfPreviewForm.IsDisposed && pdfPreviewForm.Bounds.Contains(mousePos);

                    if (!overButton && !overPreview)
                    {
                        hoverCheckTimer.Stop();
                        Task.Delay(300).ContinueWith(_ =>
                        {
                            if (pdfPreviewForm != null && !pdfPreviewForm.IsDisposed)
                            {
                                pdfPreviewForm.Invoke((Action)(() =>
                                {
                                    pdfPreviewForm.Close();
                                    pdfPreviewForm = null;
                                }));
                            }
                        });
                    }
                };
            }

            hoverCheckTimer.Start();
        }

        private void btnInsertBlueprintFile_MouseEnter(object sender, EventArgs e)
        {
            isPreviewingDemolition = false;
            if (fileConstructionBytes != null)
            {
                ShowPDFPreviewFromBytes(fileConstructionBytes, btnInsertBlueprintFile);
            }
        }

        private void btnInsertDemolitionFile_MouseEnter(object sender, EventArgs e)
        {
            isPreviewingDemolition = true;
            if (fileDemolitionBytes != null)
            {
                ShowPDFPreviewFromBytes(fileDemolitionBytes, btnInsertDemolitionFile);
            }
        }

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

                    if (Path.GetExtension(selectedFile).ToLower() == ".pdf")
                    {
                        FileInfo fileInfo = new FileInfo(selectedFile);

                        if (fileInfo.Length <= 50 * 1024 * 1024)
                        {
                            string fileName = Path.GetFileName(selectedFile);
                            button.Text = fileName;
                            Console.WriteLine("ไฟล์ที่เลือก: " + fileName);

                            return File.ReadAllBytes(selectedFile);
                        }
                        else
                        {
                            MessageBox.Show("ขนาดไฟล์เกิน 50MB!", "ขนาดไฟล์ไม่ถูกต้อง", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        // ------------------------------------------------------------
        // Validate ก่อนบันทึก
        // ------------------------------------------------------------
        private bool ValidateProjectData()
        {
            if (string.IsNullOrWhiteSpace(txtProjectName.Text))
            {
                MessageBox.Show("กรุณากรอกชื่อโครงการ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProjectName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNumber.Text))
            {
                MessageBox.Show("กรุณากรอกเลขที่สัญญา!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNumber.Focus();
                return false;
            }

            if (cmbCurrentPhaseNumber.SelectedItem == null)
            {
                MessageBox.Show("กรุณาระบุจำนวนเฟส!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCurrentPhaseNumber.Focus();
                return false;
            }

            if (dtpkStartDate.Value > dtpkEndDate.Value)
            {
                MessageBox.Show("วันที่สิ้นสุดโครงการต้องไม่น้อยกว่าวันที่เริ่มต้น!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpkEndDate.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtBudget.Text) || !decimal.TryParse(txtBudget.Text.Replace(",", ""), out _))
            {
                MessageBox.Show("กรุณากรอกจำนวนเงินจ้างที่ถูกต้อง!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBudget.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(selectedCustomerID))
            {
                MessageBox.Show("กรุณาเลือกข้อมูลลูกค้า!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(selectedEmployeeID))
            {
                MessageBox.Show("กรุณาเลือกข้อมูลผู้ดูแลโครงการ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProjectAddress.Text))
            {
                MessageBox.Show("กรุณากรอกที่อยู่โครงการ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProjectAddress.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtRemark.Text))
            {
                MessageBox.Show("กรุณากรอกหมายเหตุที่เกี่ยวข้องกับโครงการ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRemark.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProjectDetail.Text))
            {
                MessageBox.Show("กรุณากรอกรายละเอียดโครงการ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProjectDetail.Focus();
                return false;
            }

            if (fileConstructionBytes == null)
            {
                MessageBox.Show(
                    "กรุณาเลือกไฟล์แบบแปลนโครงการ (Blueprint) ก่อนบันทึก!",
                    "ข้อมูลผิดพลาด!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            decimal totalBudget = 0;
            decimal.TryParse(txtBudget.Text.Replace(",", ""), out totalBudget);

            decimal totalCompletion = projectPhases.Sum(p => p.PhasePercent);
            decimal totalBoqBudget = projectPhases.Sum(p => p.PhaseBudget);
            decimal totalBoqPercent = 0;

            if (totalBudget > 0)
            {
                foreach (var phase in projectPhases)
                {
                    totalBoqPercent += (phase.PhaseBudget * 100) / totalBudget;
                }
            }

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

            return true;
        }

        // ------------------------------------------------------------
        // เลขที่ใบสั่งซื้อ/เลขที่สัญญาอัตโนมัติ
        // ------------------------------------------------------------
        private string GenerateNextOrderNumber()
        {
            string prefix = "CT";
            var now = DateTime.Now;

            // ปี พ.ศ. 2 หลักท้าย
            int yearBE = now.Year + 543;
            string yy = (yearBE % 100).ToString("D2");
            string mm = now.Month.ToString("D2");
            string baseKey = $"{prefix}{yy}{mm}";
            string pattern = $"{baseKey}-%";

            string sql = @"
                SELECT num FROM (
                    SELECT order_number AS num
                    FROM purchaseorder
                    WHERE order_number LIKE @pattern
                    UNION ALL
                    SELECT pro_number AS num
                    FROM project
                    WHERE pro_number LIKE @pattern
                ) x
                ORDER BY num DESC
                LIMIT 1;";

            string lastOrderNo = null;

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@pattern", pattern);
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    lastOrderNo = result.ToString();
            }

            string nextRunning = "0001";
            string dash = "-";

            if (!string.IsNullOrEmpty(lastOrderNo))
            {
                // รูปแบบ: CTyyMM-0001 หรือ CTyyMM-A0001
                var parts = lastOrderNo.Split('-');
                if (parts.Length == 2)
                {
                    string runningPart = parts[1];

                    if (runningPart.StartsWith("A"))
                    {
                        int num = int.Parse(runningPart.Substring(1));
                        if (num >= 9999)
                        {
                            dash = "-A";
                            nextRunning = "0001";
                        }
                        else
                        {
                            dash = "-A";
                            nextRunning = (num + 1).ToString("D4");
                        }
                    }
                    else
                    {
                        int num = int.Parse(runningPart);
                        if (num >= 9999)
                        {
                            dash = "-A";
                            nextRunning = "0001";
                        }
                        else
                        {
                            nextRunning = (num + 1).ToString("D4");
                        }
                    }
                }
            }

            return $"{baseKey}{dash}{nextRunning}";
        }

        // ------------------------------------------------------------
        // Handler SearchboxControl + ฟิลเตอร์ใน DataGridView
        // ------------------------------------------------------------
        private void SearchboxProject_SearchTriggered(object sender, SearchEventArgs e)
        {
            ApplyProjectGridFilter(e.SearchBy, e.Keyword);
        }

        private void ApplyProjectGridFilter(string searchBy, string keyword)
        {
            keyword = (keyword ?? "").Trim();

            if (dtgvProject.Rows.Count == 0)
                return;

            foreach (DataGridViewRow row in dtgvProject.Rows)
            {
                if (row.IsNewRow) continue;

                if (string.IsNullOrEmpty(keyword))
                {
                    row.Visible = true;
                    continue;
                }

                string cellValue = "";

                switch (searchBy)
                {
                    case "รหัสโครงการ":
                        cellValue = row.Cells["ProjectID"].Value?.ToString() ?? "";
                        break;
                    case "ชื่อโครงการ":
                        cellValue = row.Cells["ProjectName"].Value?.ToString() ?? "";
                        break;
                    case "เลขที่สัญญา":
                        cellValue = row.Cells["ProjectNumber"].Value?.ToString() ?? "";
                        break;
                    default:
                        var sb = new StringBuilder();
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null)
                                sb.Append(cell.Value.ToString()).Append(" ");
                        }
                        cellValue = sb.ToString();
                        break;
                }

                bool match = cellValue.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
                row.Visible = match;
            }
        }
    }
}
