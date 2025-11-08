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
        private List<PhaseWorking> allPhaseWorkingList; //ไม่ได้ใช้ในที่นี้ แต่สามารถเก็บข้อมูลทั้งหมดได้ถ้าต้องการ
        // สร้างคลาส Model สำหรับแสดงข้อมูลใน Grid
        private class ProjectPhaseWithWorkCount
        {
            public int PhaseID { get; set; }
            public int ProID { get; set; }
            public int PhaseNo { get; set; }
            public string PhaseDetail { get; set; }
            public decimal PhaseBudget { get; set; }
            public decimal PhasePercent { get; set; }
            public string PhaseStatus { get; set; }
            public int WorkCount { get; set; } // จำนวนงานในเฟส
            public string WorkID { get; set; } // เก็บ WorkID แรก (หรือเลือกวิธีอื่น)
            public string PhaseStatusThai { get; set; } 
        }
        public ProjectPhaseListRequestsforApproval()
        {
            InitializeComponent();
            LoadProjectPhasesWithWorkCount();
            //CustomPhaseWorkingGrid();
        }

        private void LoadProjectPhasesWithWorkCount()
        {
            var phaseDal = new PhaseDAL();
            var allPhases = phaseDal.GetAllProjectPhase();
            var workDal = new PhaseWorkDAL();
            var allWorkings = workDal.GetAllPhaseWorking();

            // ใช้ค่านิยามจาก WorkStatus class ที่มีอยู่
            var validStatuses = new[] {
                WorkStatus.InProgress,
                WorkStatus.Completed,
                WorkStatus.Rejected
            };

            var displayList = allPhases
                .Select(p => {
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
                        PhaseStatusThai = WorkStatus.GetDisplayName(p.PhaseStatus), // 🟢 Map ภาษาไทย
                        WorkCount = works.Count,
                        WorkID = firstWork != null ? firstWork.WorkID : null
                    };
                })
                .Where(x => x.WorkCount > 0 &&
                validStatuses.Contains(
                    allWorkings.FirstOrDefault(w => w.WorkID == x.WorkID)?.WorkStatus
                )
    )
                .OrderBy(p => p.PhaseNo)
                .ToList();

            dtgvRequestApproval.DataSource = null;
            dtgvRequestApproval.DataSource = displayList;
            CustomProjectPhaseGrid();
        }


        private void CustomProjectPhaseGrid()
        {
            var grid = dtgvRequestApproval;
            // อย่าลืม! set AutoGenerateColumns = true ถ้า bind DataSource เป็น List/Datatable
            grid.AutoGenerateColumns = true;

            if (grid.Columns.Contains("PhaseID"))
                grid.Columns["PhaseID"].HeaderText = "รหัสเฟส";
            if (grid.Columns.Contains("ProID"))
                grid.Columns["ProID"].HeaderText = "รหัสโครงการ";
            if (grid.Columns.Contains("PhaseNo"))
                grid.Columns["PhaseNo"].HeaderText = "เฟสที่";
            if (grid.Columns.Contains("WorkCount"))
                grid.Columns["WorkCount"].HeaderText = "จำนวนงานในเฟส"; // หรือ "จำนวนกิจกรรม"
            if (grid.Columns.Contains("PhaseDetail"))
                grid.Columns["PhaseDetail"].HeaderText = "รายละเอียดเฟส";
            if (grid.Columns.Contains("PhaseBudget"))
                grid.Columns["PhaseBudget"].HeaderText = "งบประมาณ";
            if (grid.Columns.Contains("PhasePercent"))
                grid.Columns["PhasePercent"].HeaderText = "เปอร์เซ็นต์เฟส";
            if (grid.Columns.Contains("PhaseStatus"))
                grid.Columns["PhaseStatus"].HeaderText = "สถานะ";
            if (grid.Columns.Contains("PhaseStatusThai"))
                grid.Columns["PhaseStatusThai"].HeaderText = "สถานะ";


            // --- ซ่อน Column ที่ไม่ต้องการให้แสดง ---
            if (grid.Columns.Contains("PhaseID"))
                grid.Columns["PhaseID"].Visible = false;
            if (grid.Columns.Contains("PhaseStatus"))
                grid.Columns["PhaseStatus"].Visible = false; // ซ่อนภาษาอังกฤษ
            if (grid.Columns.Contains("WorkID"))
                grid.Columns["WorkID"].Visible = false; // ซ่อน WorkID
            // กำหนด Selection
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;

            // เส้นขอบ + สีพื้นหลัง + cell
            grid.BorderStyle = BorderStyle.None;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.BackgroundColor = Color.White;

            // Header
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            grid.ColumnHeadersHeight = 30;

            // Cell font, alignment, padding
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            // ขนาดคอลัมน์/แถว
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grid.RowTemplate.Height = 30;

            // Grid line สีเทา
            grid.GridColor = Color.LightGray;

            // ซ่อนแถวหัวแถวซ้ายมือ
            grid.RowHeadersVisible = false;

            // ReadOnly + ไม่อนุญาตเพิ่ม/ขยายแถว
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToResizeRows = false;
        }
        //สำหรับส่งค่าข้อมูลเพื่อโหลดมาแสดงที่หน้าจอรายละเอียดการทำงาน ที่โหลด form CheckphaseWorking

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

                // ส่งค่า workId, phaseId, projectId ไปฟอร์ม CheckphaseWorking
                var detailForm = new CheckphaseWorking(workId, phaseId, proId);
                detailForm.ShowDialog();
            }
        }

        
    }
}
