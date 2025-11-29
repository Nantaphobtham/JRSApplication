using JRSApplication.Components;
using JRSApplication.Components.Service;
using JRSApplication.Data_Access_Layer;
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
    public partial class ProjectPhaseListRequestsforApproval : UserControl
    {
        private List<PhaseWorking> allPhaseWorkingList;

        // Model สำหรับ Grid
        private class ProjectPhaseWithWorkCount
        {
            public int PhaseID { get; set; }
            public int ProID { get; set; }
            public int PhaseNo { get; set; }
            public string PhaseDetail { get; set; }
            public decimal PhaseBudget { get; set; }
            public decimal PhasePercent { get; set; }
            public string PhaseStatus { get; set; }
            public int WorkCount { get; set; }
            public string WorkID { get; set; }
            public string PhaseStatusThai { get; set; }
        }

        // เก็บ list ที่ใช้แสดง + filter
        private List<ProjectPhaseWithWorkCount> _displayList = new List<ProjectPhaseWithWorkCount>();

        public ProjectPhaseListRequestsforApproval()
        {
            InitializeComponent();

            LoadProjectPhasesWithWorkCount();

            CustomProjectPhaseGrid();

            // ✅ ผูก Searchbox สำหรับ Project Manager
            try
            {
                // ชื่อ function นี้ต้องไปเพิ่มใน SearchboxControl ด้วย
                searchboxControl1.DefaultRole = "Projectmanager";
                searchboxControl1.DefaultFunction = "รายการเฟสที่รออนุมัติ";
                searchboxControl1.SetRoleAndFunction("Projectmanager", "รายการเฟสที่รออนุมัติ");

                searchboxControl1.SearchTriggered += SearchboxPhaseApproval_SearchTriggered;
            }
            catch { }
        }

        // =============== Searchbox -> Filter Grid ==================

        private void SearchboxPhaseApproval_SearchTriggered(object sender, SearchEventArgs e)
        {
            ApplyPhaseListFilter(e.SearchBy, e.Keyword);
        }

        private void ApplyPhaseListFilter(string searchBy, string keyword)
        {
            if (_displayList == null) return;

            string q = (keyword ?? "").Trim().ToLowerInvariant();

            IEnumerable<ProjectPhaseWithWorkCount> baseList = _displayList;

            if (string.IsNullOrEmpty(q))
            {
                dtgvRequestApproval.DataSource = null;
                dtgvRequestApproval.DataSource = baseList.ToList();
                return;
            }

            IEnumerable<ProjectPhaseWithWorkCount> filtered;

            switch (searchBy)
            {
                case "รหัสโครงการ":
                    filtered = baseList.Where(p =>
                        p.ProID.ToString().ToLowerInvariant().Contains(q));
                    break;

                case "เฟสที่":
                    filtered = baseList.Where(p =>
                        p.PhaseNo.ToString().ToLowerInvariant().Contains(q));
                    break;

                case "สถานะเฟส":
                    filtered = baseList.Where(p =>
                        (!string.IsNullOrEmpty(p.PhaseStatusThai) &&
                         p.PhaseStatusThai.ToLowerInvariant().Contains(q)) ||
                        (!string.IsNullOrEmpty(p.PhaseStatus) &&
                         p.PhaseStatus.ToLowerInvariant().Contains(q)));
                    break;

                default: // ทั้งหมด
                    filtered = baseList.Where(p =>
                        p.ProID.ToString().ToLowerInvariant().Contains(q) ||
                        p.PhaseNo.ToString().ToLowerInvariant().Contains(q) ||
                        (!string.IsNullOrEmpty(p.PhaseDetail) &&
                         p.PhaseDetail.ToLowerInvariant().Contains(q)) ||
                        (!string.IsNullOrEmpty(p.PhaseStatusThai) &&
                         p.PhaseStatusThai.ToLowerInvariant().Contains(q)));
                    break;
            }

            dtgvRequestApproval.DataSource = null;
            dtgvRequestApproval.DataSource = filtered.ToList();
        }

        // ================= Data load เดิม =========================

        private void LoadProjectPhasesWithWorkCount()
        {
            var phaseDal = new PhaseDAL();
            var allPhases = phaseDal.GetAllProjectPhase();
            var workDal = new PhaseWorkDAL();
            var allWorkings = workDal.GetAllPhaseWorking();

            var validStatuses = new[] {
                WorkStatus.InProgress,
                WorkStatus.Completed,
                WorkStatus.Rejected
            };

            var displayList = allPhases
                .Select(p =>
                {
                    var works = allWorkings.Where(w => w.PhaseID == p.PhaseID).ToList();
                    var firstWork = works.FirstOrDefault();
                    return new ProjectPhaseWithWorkCount
                    {
                        PhaseID = p.PhaseID,
                        ProID = p.ProID,
                        PhaseNo = p.PhaseNumber,
                        PhaseDetail = p.PhaseDetail,
                        PhaseBudget = p.PhaseBudget,
                        PhasePercent = p.PhasePercent,
                        PhaseStatus = p.PhaseStatus,
                        PhaseStatusThai = WorkStatus.GetDisplayName(p.PhaseStatus),
                        WorkCount = works.Count,
                        WorkID = firstWork != null ? firstWork.WorkID : null
                    };
                })
                .Where(x => x.WorkCount > 0 &&
                            validStatuses.Contains(
                                allWorkings.FirstOrDefault(w => w.WorkID == x.WorkID)?.WorkStatus
                            ))
                .OrderBy(p => p.PhaseNo)
                .ToList();

            _displayList = displayList;

            dtgvRequestApproval.DataSource = null;
            dtgvRequestApproval.DataSource = _displayList;
        }

        private void CustomProjectPhaseGrid()
        {
            var grid = dtgvRequestApproval;

            grid.AutoGenerateColumns = true;

            if (grid.Columns.Contains("PhaseID"))
                grid.Columns["PhaseID"].HeaderText = "รหัสเฟส";
            if (grid.Columns.Contains("ProID"))
                grid.Columns["ProID"].HeaderText = "รหัสโครงการ";
            if (grid.Columns.Contains("PhaseNo"))
                grid.Columns["PhaseNo"].HeaderText = "เฟสที่";
            if (grid.Columns.Contains("WorkCount"))
                grid.Columns["WorkCount"].HeaderText = "จำนวนงานในเฟส";
            if (grid.Columns.Contains("PhaseDetail"))
                grid.Columns["PhaseDetail"].HeaderText = "รายละเอียดเฟส";
            if (grid.Columns.Contains("PhaseBudget"))
                grid.Columns["PhaseBudget"].HeaderText = "งบประมาณ";
            if (grid.Columns.Contains("PhasePercent"))
                grid.Columns["PhasePercent"].HeaderText = "เปอร์เซ็นต์เฟส";
            if (grid.Columns.Contains("PhaseStatus"))
                grid.Columns["PhaseStatus"].HeaderText = "สถานะ (อังกฤษ)";
            if (grid.Columns.Contains("PhaseStatusThai"))
                grid.Columns["PhaseStatusThai"].HeaderText = "สถานะ";

            if (grid.Columns.Contains("PhaseID"))
                grid.Columns["PhaseID"].Visible = false;
            if (grid.Columns.Contains("PhaseStatus"))
                grid.Columns["PhaseStatus"].Visible = false;
            if (grid.Columns.Contains("WorkID"))
                grid.Columns["WorkID"].Visible = false;

            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;

            grid.BorderStyle = BorderStyle.None;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.BackgroundColor = Color.White;

            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            grid.ColumnHeadersHeight = 30;

            grid.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grid.RowTemplate.Height = 30;
            grid.GridColor = Color.LightGray;
            grid.RowHeadersVisible = false;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToResizeRows = false;
        }

        private void dtgvRequestApproval_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dtgvRequestApproval.Rows[e.RowIndex];
                var phaseId = Convert.ToInt32(selectedRow.Cells["PhaseID"].Value);
                var proId = Convert.ToInt32(selectedRow.Cells["ProID"].Value);
                var workIdObj = selectedRow.Cells["WorkID"].Value;
                string workId = workIdObj != null ? workIdObj.ToString() : null;

                if (string.IsNullOrEmpty(workId))
                {
                    MessageBox.Show("เฟสนี้ยังไม่มีข้อมูลการทำงาน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var detailForm = new CheckphaseWorking(workId, phaseId, proId);
                detailForm.ShowDialog();
            }
        }
    }
}
