using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static JRSApplication.Sitesupervisor.WorkResponse;

namespace JRSApplication.Sitesupervisor
{
    public partial class PhaseApprovalResult : UserControl
    {
        private readonly string _empId;
        private readonly string _role;
        public PhaseApprovalResult()
        {
            InitializeComponent();
            SetupGrid();
        }

        public PhaseApprovalResult(string empId, string role) : this()
        {
            _empId = empId;
            _role = role;

            // ถ้าต้องใช้ค่า ก็เซ็ต/โหลดข้อมูลที่นี่
            // e.g., LoadDataFor(empId, role);
        }

        private void SetupGrid()
        {
            dtgvPhaseApprovalResult.AutoGenerateColumns = false;
            dtgvPhaseApprovalResult.AllowUserToAddRows = false;
            dtgvPhaseApprovalResult.ReadOnly = true;
            dtgvPhaseApprovalResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvPhaseApprovalResult.MultiSelect = false;
            dtgvPhaseApprovalResult.RowHeadersVisible = false;
            dtgvPhaseApprovalResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dtgvPhaseApprovalResult.Columns.Clear();

            // 🆔 ลำดับ
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIndex",
                HeaderText = "ลำดับ"
            });

            // 📦 รหัสโครงการ
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProId",
                HeaderText = "รหัสโครงการ",
                DataPropertyName = "ProjectId"
            });

            // 📑 เลขที่สัญญา
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProNumber",
                HeaderText = "เลขที่สัญญา",
                DataPropertyName = "ProjectNumber"
            });

            // 🧩 เฟสที่
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPhase",
                HeaderText = "เฟสที่",
                DataPropertyName = "PhaseNo"
            });

            // 📎 รหัสรายการ / รหัสงาน
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colItemCode",
                HeaderText = "รหัสงาน",
                DataPropertyName = "OrderNumber"
            });

            // 📝 รายละเอียด
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDetail",
                HeaderText = "รายละเอียดคำสั่งซื้อ",
                DataPropertyName = "OrderDetail"
            });

            // 🕒 วันที่เริ่ม
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStartDate",
                HeaderText = "วันที่ออกใบสังซื้อ",
                DataPropertyName = "OrderDate",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });

            // 📅 วันที่ครบกำหนด / เสร็จสิ้น
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEndDate",
                HeaderText = "วันที่ครบกำหนด",
                DataPropertyName = "DueDate",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });

            // ✅ วันที่อนุมัติ / วันที่เสร็จสิ้น
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colApproved",
                HeaderText = "วันที่อนุมัติ",
                DataPropertyName = "ApproveDate",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });

            // 📌 สถานะรวม
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "สถานะ",
                DataPropertyName = "CombinedStatus"
            });

            // 🗒️ หมายเหตุ
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colRemark",
                HeaderText = "หมายเหตุ",
                DataPropertyName = "CombinedRemark"
            });

            // 🔢 ลำดับอัตโนมัติ
            dtgvPhaseApprovalResult.RowPostPaint += (s, e) =>
            {
                dtgvPhaseApprovalResult.Rows[e.RowIndex].Cells["colIndex"].Value = (e.RowIndex + 1).ToString();
            };

            // 🎨 แยกสีระหว่าง Order / Work
            dtgvPhaseApprovalResult.CellFormatting += (s, e) =>
            {
                if (dtgvPhaseApprovalResult.Rows[e.RowIndex].DataBoundItem is Response row)
                {
                    if (row.RowType == RowType.Work)
                    {
                        if (dtgvPhaseApprovalResult.Columns[e.ColumnIndex].Name == "colItemCode")
                            e.Value = row.WorkId;
                        if (dtgvPhaseApprovalResult.Columns[e.ColumnIndex].Name == "colDetail")
                            e.Value = row.WorkDetail;
                        if (dtgvPhaseApprovalResult.Columns[e.ColumnIndex].Name == "colStartDate")
                            e.Value = row.WorkDate;
                        if (dtgvPhaseApprovalResult.Columns[e.ColumnIndex].Name == "colEndDate")
                            e.Value = row.WorkendDate;
                        if (dtgvPhaseApprovalResult.Columns[e.ColumnIndex].Name == "colApproved")
                            e.Value = "";

                        e.CellStyle.BackColor = Color.WhiteSmoke;
                    }
                }
            };
            CustomizeGridStyling(dtgvPhaseApprovalResult);
        }

        private void CustomizeGridStyling(DataGridView grid)
        {
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.BorderStyle = BorderStyle.None;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersHeight = 32;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);
            grid.RowTemplate.Height = 32;
            grid.AllowUserToResizeRows = false;
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }


    }
}
