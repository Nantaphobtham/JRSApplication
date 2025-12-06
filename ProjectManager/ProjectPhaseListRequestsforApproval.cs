using JRSApplication.Components;
using JRSApplication.Components.Service;
using JRSApplication.Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class ProjectPhaseListRequestsforApproval : UserControl
    {
        private List<PhaseWorking> allPhaseWorkingList;

        // เก็บค่าการค้นหาปัจจุบันของ Searchbox
        private string _currentSearchBy = "ทั้งหมด";
        private string _currentKeyword = "";

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

            // ------------------------------
            // ตั้งค่า Searchbox (ตามบทบาท Projectmanager)
            // ------------------------------
            try
            {
                searchboxControl1.DefaultRole = "Projectmanager";
                searchboxControl1.DefaultFunction = "รายการเฟสที่รออนุมัติ";
                searchboxControl1.SetRoleAndFunction("Projectmanager", "ผลการอนุมัติเฟส");
                searchboxControl1.SearchTriggered += SearchboxPhaseApproval_SearchTriggered;
            }
            catch
            {
            }

            CustomProjectPhaseGrid();   // ตั้ง style ของ grid
            LoadProjectPhasesWithWorkCount();
        }

        // ================== Event จาก Searchbox ==================

        private void SearchboxPhaseApproval_SearchTriggered(object sender, SearchEventArgs e)
        {
            _currentSearchBy = e.SearchBy;
            _currentKeyword = e.Keyword;

            ApplyPhaseListFilter(_currentSearchBy, _currentKeyword);
        }

        // =============== Helper: เตรียมคอลัมน์ / Bind ==================

        private void EnsureGridColumns()
        {
            var grid = dtgvRequestApproval;

            if (grid.Columns.Count > 0)
                return;    // เคยสร้างแล้ว

            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();

            // ซ่อน PhaseID แต่ต้องมีไว้ใช้
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhaseID",
                DataPropertyName = "PhaseID",
                HeaderText = "รหัสเฟส",
                Visible = false
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProID",
                DataPropertyName = "ProID",
                HeaderText = "รหัสโครงการ"
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhaseNo",
                DataPropertyName = "PhaseNo",
                HeaderText = "เฟสที่"
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhaseDetail",
                DataPropertyName = "PhaseDetail",
                HeaderText = "รายละเอียดเฟส"
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhaseBudget",
                DataPropertyName = "PhaseBudget",
                HeaderText = "งบประมาณ"
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhasePercent",
                DataPropertyName = "PhasePercent",
                HeaderText = "เปอร์เซ็นต์เฟส"
            });

            // ซ่อนสถานะอังกฤษ
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhaseStatus",
                DataPropertyName = "PhaseStatus",
                HeaderText = "สถานะ (อังกฤษ)",
                Visible = false
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WorkCount",
                DataPropertyName = "WorkCount",
                HeaderText = "จำนวนงานในเฟส"
            });

            // ซ่อน WorkID
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WorkID",
                DataPropertyName = "WorkID",
                HeaderText = "รหัสงาน",
                Visible = false
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhaseStatusThai",
                DataPropertyName = "PhaseStatusThai",
                HeaderText = "สถานะ"
            });
        }

        private void BindGrid(List<ProjectPhaseWithWorkCount> list)
        {
            EnsureGridColumns();
            dtgvRequestApproval.DataSource = null;
            dtgvRequestApproval.DataSource = list;
        }

        // =============== Apply Filter ให้ Grid ==================

        private void ApplyPhaseListFilter(string searchBy, string keyword)
        {
            if (_displayList == null) return;

            string q = (keyword ?? "").Trim().ToLowerInvariant();
            IEnumerable<ProjectPhaseWithWorkCount> baseList = _displayList;

            if (string.IsNullOrEmpty(q) || searchBy == "ทั้งหมด")
            {
                BindGrid(baseList.ToList());
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

                default: // ทั้งหมด/กรณีอื่น
                    filtered = baseList.Where(p =>
                        p.ProID.ToString().ToLowerInvariant().Contains(q) ||
                        p.PhaseNo.ToString().ToLowerInvariant().Contains(q) ||
                        (!string.IsNullOrEmpty(p.PhaseDetail) &&
                         p.PhaseDetail.ToLowerInvariant().Contains(q)) ||
                        (!string.IsNullOrEmpty(p.PhaseStatusThai) &&
                         p.PhaseStatusThai.ToLowerInvariant().Contains(q)));
                    break;
            }

            BindGrid(filtered.ToList());
        }

        // ================= Load Data =========================

        private void LoadProjectPhasesWithWorkCount()
        {
            var phaseDal = new PhaseDAL();
            var allPhases = phaseDal.GetAllProjectPhase();

            var workDal = new PhaseWorkDAL();
            var allWorkings = workDal.GetAllPhaseWorking();

            var displayList = allPhases
                .Select(p =>
                {
                    // งานทั้งหมดของเฟสนี้
                    var works = allWorkings
                        .Where(w => w.PhaseID == p.PhaseID)
                        .ToList();

                    // งานล่าสุด (ถ้ามี)
                    var lastWork = works
                        .OrderByDescending(w => w.WorkDate)
                        .FirstOrDefault();

                    // เอาสถานะจากงานล่าสุดก่อน ถ้าไม่มีให้ใช้ phase_status จากตาราง phase
                    string lastStatus = lastWork?.WorkStatus;
                    if (string.IsNullOrEmpty(lastStatus))
                        lastStatus = p.PhaseStatus;   // สมมติว่า PhaseModel มี property PhaseStatus แม็พกับคอลัมน์ phase_status

                    return new ProjectPhaseWithWorkCount
                    {
                        PhaseID = p.PhaseID,
                        ProID = p.ProID,
                        PhaseNo = p.PhaseNumber,
                        PhaseDetail = p.PhaseDetail,
                        PhaseBudget = p.PhaseBudget,
                        PhasePercent = p.PhasePercent,
                        WorkCount = works.Count,
                        WorkID = lastWork?.WorkID,
                        PhaseStatus = lastStatus ?? string.Empty,
                        PhaseStatusThai = !string.IsNullOrEmpty(lastStatus)
                                            ? WorkStatus.GetDisplayName(lastStatus)
                                            : string.Empty
                    };
                })
                // เรียงให้โครงการเหมือนเดิม แต่ให้ Completed ไปอยู่ล่างสุด
                .OrderBy(p => p.ProID)
                .ThenBy(p =>
                    string.Equals(p.PhaseStatus, WorkStatus.Completed,
                                  StringComparison.OrdinalIgnoreCase)
                        ? 1  // Completed → กลุ่มล่าง
                        : 0) // อื่น ๆ → กลุ่มบน
                .ThenBy(p => p.PhaseNo)
                .ToList();

            _displayList = displayList;

            // ถ้ายังไม่มีเงื่อนไขค้นหา → แสดงทั้งหมด
            if (string.IsNullOrWhiteSpace(_currentKeyword) || _currentSearchBy == "ทั้งหมด")
            {
                BindGrid(_displayList);
            }
            else
            {
                ApplyPhaseListFilter(_currentSearchBy, _currentKeyword);
            }
        }


        private void CustomProjectPhaseGrid()
        {
            var grid = dtgvRequestApproval;

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
                    MessageBox.Show("เฟสนี้ยังไม่มีข้อมูลการทำงาน",
                                    "แจ้งเตือน",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    return;
                }

                using (var detailForm = new CheckphaseWorking(workId, phaseId, proId))
                {
                    if (detailForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadProjectPhasesWithWorkCount();
                    }
                }
            }
        }
    }
}
