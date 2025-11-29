using JRSApplication.Components;
using JRSApplication.Components.Models;
using JRSApplication.Components.Service;
using JRSApplication.Data_Access_Layer;
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
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class ProjectData : UserControl
    {
        private List<Project> _allProjects = new List<Project>();

        public ProjectData()
        {
            InitializeComponent();
            InitializeDataGridViewProject();
            InitializeDataGridViewPhase();
            LoadProjectData();

            // ✅ ผูก Searchbox สำหรับค้นหาโครงการ
            try
            {
                searchboxControl1.DefaultRole = "Projectmanager";
                searchboxControl1.DefaultFunction = "ข้อมูลโครงการ";
                searchboxControl1.SetRoleAndFunction("Projectmanager", "ตรวจสอบข้อมูลโครงการ2");

                searchboxControl1.SearchTriggered += SearchboxProject_SearchTriggered;
            }
            catch { }
        }

        // ============== Searchbox -> Filter Projects =================

        private void SearchboxProject_SearchTriggered(object sender, SearchEventArgs e)
        {
            ApplyProjectFilter(e.SearchBy, e.Keyword);
        }

        private void ApplyProjectFilter(string searchBy, string keyword)
        {
            if (_allProjects == null) return;

            string q = (keyword ?? "").Trim().ToLowerInvariant();
            IEnumerable<Project> baseList = _allProjects;

            if (string.IsNullOrEmpty(q))
            {
                FillProjectGrid(baseList);
                return;
            }

            IEnumerable<Project> filtered;

            switch (searchBy)
            {
                case "รหัสโครงการ":
                    filtered = baseList.Where(p =>
                        p.ProjectID.ToString().ToLowerInvariant().Contains(q));
                    break;

                case "ชื่อโครงการ":
                    filtered = baseList.Where(p =>
                        !string.IsNullOrEmpty(p.ProjectName) &&
                        p.ProjectName.ToLowerInvariant().Contains(q));
                    break;

                case "ชื่อลูกค้า":
                    filtered = baseList.Where(p =>
                        !string.IsNullOrEmpty(p.CustomerName) &&
                        p.CustomerName.ToLowerInvariant().Contains(q));
                    break;

                case "ผู้ดูแลโครงการ":
                    filtered = baseList.Where(p =>
                        !string.IsNullOrEmpty(p.EmployeeName) &&
                        p.EmployeeName.ToLowerInvariant().Contains(q));
                    break;

                default: // ทั้งหมด
                    filtered = baseList.Where(p =>
                        p.ProjectID.ToString().ToLowerInvariant().Contains(q) ||
                        (!string.IsNullOrEmpty(p.ProjectName) &&
                         p.ProjectName.ToLowerInvariant().Contains(q)) ||
                        (!string.IsNullOrEmpty(p.CustomerName) &&
                         p.CustomerName.ToLowerInvariant().Contains(q)) ||
                        (!string.IsNullOrEmpty(p.EmployeeName) &&
                         p.EmployeeName.ToLowerInvariant().Contains(q)));
                    break;
            }

            FillProjectGrid(filtered);
        }

        private void FillProjectGrid(IEnumerable<Project> projects)
        {
            dtgvProjectData.Rows.Clear();

            foreach (var project in projects)
            {
                dtgvProjectData.Rows.Add(
                    project.ProjectID,
                    project.ProjectName,
                    project.ProjectStart.ToString("dd/MM/yyyy"),
                    project.ProjectEnd.ToString("dd/MM/yyyy"),
                    project.ProjectBudget.ToString("N2"),
                    project.CurrentPhaseNumber,
                    project.CustomerName,
                    project.EmployeeName
                );
            }
        }

        // ============== โค้ดเดิมของ ProjectData ====================

        private void LoadProjectDetails(int projectId)
        {
            ProjectDAL projectDAL = new ProjectDAL();
            Project project = projectDAL.GetProjectDetailsById(projectId);

            if (project != null)
            {
                txtProjectID.Text = project.ProjectID.ToString();
                txtProjectname.Text = project.ProjectName;
                txtStartdate.Text = project.ProjectStart.ToString("dd/MM/yyyy");
                txtEnddate.Text = project.ProjectEnd.ToString("dd/MM/yyyy");
                txtContractnumber.Text = project.ProjectNumber;
                txtCustomername.Text = project.CustomerName;
                txtProjectManager.Text = project.EmployeeName;

                TimeSpan duration = project.ProjectEnd - project.ProjectStart;
                txtSumdate.Text = duration.TotalDays.ToString();

                byte[] blueprintBytes = project.ProjectFile?.ConstructionBlueprint;
                ShowPdfFromByteArray(blueprintBytes, axPdfBlueprint, pnlBlueprint, lblBlueprintNA);

                byte[] demolitionBytes = project.ProjectFile?.DemolitionModel;
                ShowPdfFromByteArray(demolitionBytes, axPdfDemolition, pnlDemolition, lblDemolitionNA);
            }
        }

        private void LoadProjectStatus(int projectID, int totalPhaseNumber)
        {
        }

        private void LoadProjectData()
        {
            InitializeDataGridViewProject();
            ProjectDAL dal = new ProjectDAL();
            _allProjects = dal.GetAllProjects();

            FillProjectGrid(_allProjects);
        }

        private void CustomizeDataGridViewProject()
        {
            dtgvProjectData.BorderStyle = BorderStyle.None;
            dtgvProjectData.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvProjectData.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvProjectData.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvProjectData.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvProjectData.BackgroundColor = Color.White;
            dtgvProjectData.EnableHeadersVisualStyles = false;
            dtgvProjectData.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvProjectData.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvProjectData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvProjectData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvProjectData.ColumnHeadersHeight = 30;
            dtgvProjectData.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvProjectData.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvProjectData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvProjectData.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);
            dtgvProjectData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvProjectData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvProjectData.RowTemplate.Height = 30;
            dtgvProjectData.GridColor = Color.LightGray;
            dtgvProjectData.RowHeadersVisible = false;
            dtgvProjectData.ReadOnly = true;
            dtgvProjectData.AllowUserToAddRows = false;
            dtgvProjectData.AllowUserToResizeRows = false;
        }

        private void InitializeDataGridViewProject()
        {
            if (dtgvProjectData.Columns.Count == 0)
            {
                dtgvProjectData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dtgvProjectData.MultiSelect = false;
                dtgvProjectData.AllowUserToAddRows = false;

                dtgvProjectData.Columns.Add("ProjectID", "รหัสโครงการ");
                dtgvProjectData.Columns.Add("ProjectName", "ชื่อโครงการ");
                dtgvProjectData.Columns.Add("ProjectStart", "วันที่เริ่มโครงการ");
                dtgvProjectData.Columns.Add("ProjectEnd", "วันที่สิ้นสุดโครงการ");
                dtgvProjectData.Columns.Add("ProjectBudget", "งบประมาณ (บาท)");
                dtgvProjectData.Columns.Add("CurrentPhaseNumber", "จำนวนเฟสงาน");
                dtgvProjectData.Columns.Add("CustomerName", "ชื่อลูกค้า");
                dtgvProjectData.Columns.Add("EmployeeName", "ชื่อผู้ดูแลโครงการ");

                foreach (DataGridViewColumn col in dtgvProjectData.Columns)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.ReadOnly = true;
                }

                dtgvProjectData.Columns["ProjectID"].Width = 80;

                dtgvProjectData.Columns["ProjectStart"].Width = 120;
                dtgvProjectData.Columns["ProjectStart"].DefaultCellStyle.Format = "dd/MM/yyyy";

                dtgvProjectData.Columns["ProjectEnd"].Width = 120;
                dtgvProjectData.Columns["ProjectEnd"].DefaultCellStyle.Format = "dd/MM/yyyy";

                dtgvProjectData.Columns["ProjectBudget"].Width = 150;
                dtgvProjectData.Columns["ProjectBudget"].DefaultCellStyle.Format = "N2";

                dtgvProjectData.Columns["CurrentPhaseNumber"].Width = 120;
                dtgvProjectData.Columns["CustomerName"].Width = 150;
                dtgvProjectData.Columns["EmployeeName"].Width = 150;

                CustomizeDataGridViewProject();
            }
        }

        private void dtgvProjectData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int projectId = Convert.ToInt32(dtgvProjectData.Rows[e.RowIndex].Cells["ProjectID"].Value);
                LoadProjectDetails(projectId);
                LoadPhaseData(projectId);
            }
        }

        private void LoadPhaseData(int projectId)
        {
            InitializeDataGridViewPhase();
            PhaseDAL phaseDAL = new PhaseDAL();
            List<PhaseWithStatus> phases = phaseDAL.GetPhasesWithStatus(projectId);

            dtgvPhaseDetail.Rows.Clear();
            foreach (var phase in phases)
            {
                dtgvPhaseDetail.Rows.Add(
                    phase.PhaseNumber,
                    phase.PhaseDetail,
                    phase.PhaseBudget.ToString("N2"),
                    phase.PhasePercent.ToString("N2") + " %",
                    WorkStatus.GetDisplayName(phase.PhaseStatus)
                );
            }

            decimal sumPercent = 0;
            bool hasInProgress = false;
            bool hasNotStarted = false;

            foreach (var phase in phases)
            {
                if (phase.PhaseStatus == WorkStatus.Completed)
                {
                    sumPercent += phase.PhasePercent;
                }
                else if (phase.PhaseStatus == WorkStatus.InProgress)
                {
                    hasInProgress = true;
                }
                else if (phase.PhaseStatus == WorkStatus.NotStarted)
                {
                    hasNotStarted = true;
                }
            }

            txtSumpercent.Text = $"{sumPercent:N2} %";

            string status;
            if (sumPercent == 100)
                status = "เสร็จสิ้น";
            else if (hasInProgress)
                status = "กำลังดำเนินการ";
            else if (hasNotStarted)
                status = "ยังไม่เริ่ม";
            else
                status = "ไม่ทราบสถานะ";

            txtStatus.Text = status;
        }

        private void CustomizeDataGridViewPhase()
        {
            dtgvPhaseDetail.BorderStyle = BorderStyle.None;
            dtgvPhaseDetail.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvPhaseDetail.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvPhaseDetail.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvPhaseDetail.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvPhaseDetail.BackgroundColor = Color.White;

            dtgvPhaseDetail.EnableHeadersVisualStyles = false;
            dtgvPhaseDetail.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dtgvPhaseDetail.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvPhaseDetail.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvPhaseDetail.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvPhaseDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvPhaseDetail.ColumnHeadersHeight = 40;

            dtgvPhaseDetail.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvPhaseDetail.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvPhaseDetail.DefaultCellStyle.Padding = new Padding(5, 3, 5, 3);

            dtgvPhaseDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvPhaseDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvPhaseDetail.RowTemplate.Height = 35;
            dtgvPhaseDetail.GridColor = Color.LightGray;
            dtgvPhaseDetail.RowHeadersVisible = false;
            dtgvPhaseDetail.ReadOnly = true;
            dtgvPhaseDetail.AllowUserToAddRows = false;
            dtgvPhaseDetail.AllowUserToResizeRows = false;
        }

        private void InitializeDataGridViewPhase()
        {
            if (dtgvPhaseDetail.Columns.Count == 0)
            {
                dtgvPhaseDetail.AllowUserToAddRows = false;

                dtgvPhaseDetail.Columns.Add("PhaseNumber", "เฟสที่");
                dtgvPhaseDetail.Columns.Add("PhaseDetail", "รายละเอียดการดำเนินงาน");
                dtgvPhaseDetail.Columns.Add("PhaseBudget", "จำนวนเงิน (บาท)");
                dtgvPhaseDetail.Columns.Add("PhasePercent", "เปอร์เซ็นต์ (%)");
                dtgvPhaseDetail.Columns.Add("PhaseStatus", "สถานะเฟส");

                dtgvPhaseDetail.Columns["PhaseNumber"].Width = 80;
                dtgvPhaseDetail.Columns["PhaseNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dtgvPhaseDetail.Columns["PhaseDetail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dtgvPhaseDetail.Columns["PhaseDetail"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dtgvPhaseDetail.Columns["PhaseBudget"].Width = 150;
                dtgvPhaseDetail.Columns["PhaseBudget"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvPhaseDetail.Columns["PhaseBudget"].DefaultCellStyle.Format = "N2";

                dtgvPhaseDetail.Columns["PhasePercent"].Width = 120;
                dtgvPhaseDetail.Columns["PhasePercent"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvPhaseDetail.Columns["PhasePercent"].DefaultCellStyle.Format = "N2";

                dtgvPhaseDetail.Columns["PhaseStatus"].Width = 150;
                dtgvPhaseDetail.Columns["PhaseStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                CustomizeDataGridViewPhase();
            }
        }

        private void ShowPdfFromByteArray(byte[] pdfBytes, AxAcroPDFLib.AxAcroPDF viewerControl, Panel panelToShow, Label labelIfNotAvailable)
        {
            labelIfNotAvailable.Visible = false;

            if (pdfBytes == null || pdfBytes.Length == 0)
            {
                viewerControl.Visible = false;
                labelIfNotAvailable.Text = "N/A";
                labelIfNotAvailable.TextAlign = ContentAlignment.MiddleCenter;
                labelIfNotAvailable.Dock = DockStyle.Fill;
                labelIfNotAvailable.Visible = true;
                return;
            }

            try
            {
                string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".pdf");
                File.WriteAllBytes(tempFile, pdfBytes);
                viewerControl.LoadFile(tempFile);
                viewerControl.setView("Fit");
                viewerControl.setShowToolbar(false);
                viewerControl.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการโหลด PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSumpercent_TextChanged(object sender, EventArgs e) { }
        private void txtStatus_TextChanged(object sender, EventArgs e) { }
    }
}
