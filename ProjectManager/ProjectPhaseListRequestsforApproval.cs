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
        public ProjectPhaseListRequestsforApproval()
        {
            InitializeComponent();
            LoadAllPhaseWorking();
            //CustomPhaseWorkingGrid();
        }

        private void LoadAllPhaseWorking()
        {
            var dal = new PhaseWorkDAL();
            allPhaseWorkingList = dal.GetAllPhaseWorking(); // คืน List<PhaseWorking>
            List<PhaseWorking> allWorkings = dal.GetAllPhaseWorking();
            dtgvRequestApproval.DataSource = null;
            dtgvRequestApproval.DataSource = allWorkings;
            CustomPhaseWorkingGrid();
        }

        private void CustomPhaseWorkingGrid()
        {
            var grid = dtgvRequestApproval;
            // อย่าลืม! set AutoGenerateColumns = true ถ้า bind DataSource เป็น List/Datatable
            grid.AutoGenerateColumns = true;

            // --- ตั้งชื่อ Column Header ภาษาไทย ---
            if (grid.Columns.Contains("WorkID"))
                grid.Columns["WorkID"].HeaderText = "รหัสทำงาน";
            if (grid.Columns.Contains("ProjectID"))
                grid.Columns["ProjectID"].HeaderText = "รหัสโครงการ";
            if (grid.Columns.Contains("PhaseNo"))
                grid.Columns["PhaseNo"].HeaderText = "เฟส";
            if (grid.Columns.Contains("WorkDetail"))
                grid.Columns["WorkDetail"].HeaderText = "รายละเอียดการทำงาน";
            if (grid.Columns.Contains("WorkStatus"))
                grid.Columns["WorkStatus"].HeaderText = "สถานะ";
            if (grid.Columns.Contains("WorkDate"))
                grid.Columns["WorkDate"].HeaderText = "วันที่เริ่ม";
            if (grid.Columns.Contains("EndDate"))
                grid.Columns["EndDate"].HeaderText = "วันที่สิ้นสุด";
            if (grid.Columns.Contains("UpdateDate"))
                grid.Columns["UpdateDate"].HeaderText = "วันที่แก้ไขล่าสุด";
            if (grid.Columns.Contains("Remark"))
                grid.Columns["Remark"].HeaderText = "ปัญหาการดำเนินงาน";
            if (grid.Columns.Contains("SupplierAssignmentId"))
                grid.Columns["SupplierAssignmentId"].HeaderText = "ใบสั่งจ้าง";

            // --- ซ่อน Column ที่ไม่ต้องการให้แสดง ---
            if (grid.Columns.Contains("PhaseID"))
                grid.Columns["PhaseID"].Visible = false;
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
                var workId = selectedRow.Cells["WorkID"].Value.ToString();

                var phaseId = Convert.ToInt32(selectedRow.Cells["PhaseID"].Value);
                var projectId = Convert.ToInt32(selectedRow.Cells["ProjectID"].Value);

                // ส่งข้อมูลไปยังฟอร์ม CheckphaseWorking
                var detailForm = new CheckphaseWorking(workId, phaseId, projectId);
                detailForm.ShowDialog();  // หรือ Show()
            }
        }
    }
}
