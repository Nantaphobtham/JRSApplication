using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace JRSApplication.Sitesupervisor
{
    public partial class PhaseApprovalResult : UserControl
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        private readonly string _empId;
        private readonly string _role;
        public PhaseApprovalResult()
        {
            InitializeComponent();
            SetupGrid();
            EnableThaiStatusDisplay();
            LoadPhaseSummaryToGrid();
            //LoadPhaseSummaryToGrid(proIdSelected);
        }

        public PhaseApprovalResult(string empId, string role) : this()
        {
            _empId = empId;
            _role = role;
        }
        private DataTable GetPhaseSummaryDataTable(int? proId = null)
        {
            var dt = new DataTable();

            string sql = @"
                        WITH pw_latest AS (
                          SELECT
                            pw.*,
                            ROW_NUMBER() OVER (
                              PARTITION BY pw.phase_id
                              ORDER BY COALESCE(pw.work_update_date, pw.work_end_date, pw.work_date, DATE('1000-01-01')) DESC,
                                       pw.work_id DESC
                            ) AS rn
                          FROM phase_working pw
                        ),
                        pw_agg AS (
                          SELECT
                            phase_id,
                            COUNT(*)                                  AS work_count,
                            MIN(work_date)                            AS start_date,
                            MAX(work_end_date)                        AS end_date
                          FROM phase_working
                          GROUP BY phase_id
                        )
                        SELECT *
                        FROM (
                          SELECT
                            p.pro_id                                  AS ProjectId,
                            p.pro_number                              AS ProjectNumber,
                            pp.phase_no                               AS PhaseNo,
                            COALESCE(pa.work_count, 0)                AS WorkCount,
                            pp.phase_detail                           AS PhaseDetail,
                            pa.start_date                             AS OrderDate,
                            pa.end_date                               AS DueDate,
                            CASE
                              WHEN pp.phase_status = 'Completed'
                                   AND COALESCE(lw.work_status, 'Completed') = 'Completed'
                                THEN 'Completed'
                              WHEN pp.phase_status = 'InProgress'
                                   AND COALESCE(lw.work_status, 'Waiting') = 'Waiting'
                                THEN 'รออนุมัติ'
                              ELSE CONCAT(pp.phase_status, ' / ', COALESCE(lw.work_status, '-'))
                            END                                       AS CombinedStatus,
                            lw.work_remark                            AS CombinedRemark
                          FROM project_phase pp
                          JOIN project p        ON p.pro_id    = pp.pro_id
                          LEFT JOIN pw_agg pa   ON pa.phase_id = pp.phase_id
                          LEFT JOIN pw_latest lw ON lw.phase_id = pp.phase_id AND lw.rn = 1
                        ) t
                        WHERE
                          t.WorkCount > 0
                          AND (t.CombinedStatus = 'Completed' OR t.CombinedStatus = 'รออนุมัติ')
                        " + (proId.HasValue ? "  AND t.ProjectId = @proId" : "") + @"
                        ORDER BY t.ProjectId, t.PhaseNo;";

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn))
            using (var da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd))
            {
                if (proId.HasValue)
                    cmd.Parameters.AddWithValue("@proId", proId.Value);

                da.Fill(dt);
            }

            return dt;
        }

        public void LoadPhaseSummaryToGrid(int? proId = null)
        {
            var table = GetPhaseSummaryDataTable(proId);
            dtgvPhaseApprovalResult.DataSource = table;
        }

        // ====== ตั้งค่าตาราง/คอลัมน์และสไตล์ ======
        private void SetupGrid()
        {
            dtgvPhaseApprovalResult.AutoGenerateColumns = false;
            dtgvPhaseApprovalResult.AllowUserToAddRows = false;
            dtgvPhaseApprovalResult.ReadOnly = true;
            dtgvPhaseApprovalResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvPhaseApprovalResult.MultiSelect = false;
            dtgvPhaseApprovalResult.RowHeadersVisible = false;

            // ใช้ Fill เพื่อคุมสัดส่วน แต่ล็อกบางคอลัมน์ด้วย MinimumWidth
            dtgvPhaseApprovalResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // แก้อาการแถวสูงเวอร์เพราะ wrap: ตั้งความสูงคงที่และไม่ auto-size แถว
            dtgvPhaseApprovalResult.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dtgvPhaseApprovalResult.RowTemplate.Height = 32;

            dtgvPhaseApprovalResult.Columns.Clear();

            // ลำดับ
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIndex",
                HeaderText = "ลำดับ",
                FillWeight = 6,
                MinimumWidth = 60
            });

            // รหัสโครงการ (pro_id)
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProId",
                HeaderText = "รหัสโครงการ",
                DataPropertyName = "ProjectId",
                FillWeight = 10,
                MinimumWidth = 90
            });

            // เลขที่สัญญา (pro_number)
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProNumber",
                HeaderText = "เลขที่สัญญา",
                DataPropertyName = "ProjectNumber",
                FillWeight = 12,
                MinimumWidth = 110
            });

            // เฟสที่
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPhaseNo",
                HeaderText = "เฟสที่",
                DataPropertyName = "PhaseNo",
                FillWeight = 7,
                MinimumWidth = 70
            });

            // จำนวนงานที่อยู่ในขั้นตอน
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colWorkCount",
                HeaderText = "จำนวนงานที่อยู่ในขั้นตอน",
                DataPropertyName = "WorkCount",
                FillWeight = 9,
                MinimumWidth = 120
            });

            // รายละเอียดเฟสที่ต้องดำเนินการ (ตัวหลักให้กินพื้นที่)
            var colDetail = new DataGridViewTextBoxColumn
            {
                Name = "colPhaseDetail",
                HeaderText = "รายละเอียดที่ต้องดำเนินการให้แล้วเสร็จ",
                DataPropertyName = "PhaseDetail",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 28,
                MinimumWidth = 260,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft, WrapMode = DataGridViewTriState.False }
            };
            dtgvPhaseApprovalResult.Columns.Add(colDetail);

            // วันที่เริ่ม
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStartDate",
                HeaderText = "วันที่เริ่ม",
                DataPropertyName = "OrderDate",
                DefaultCellStyle = { Format = "dd/MM/yyyy" },
                FillWeight = 8,
                MinimumWidth = 100
            });

            // วันที่สิ้นสุด
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEndDate",
                HeaderText = "วันที่สิ้นสุด",
                DataPropertyName = "DueDate",
                DefaultCellStyle = { Format = "dd/MM/yyyy" },
                FillWeight = 8,
                MinimumWidth = 100
            });

            // สถานะ
            dtgvPhaseApprovalResult.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "สถานะ",
                DataPropertyName = "CombinedStatus",
                FillWeight = 8,
                MinimumWidth = 100
            });

            // หมายเหตุ (ตั้งให้เล็กลงแบบคงที่)
            var colRemark = new DataGridViewTextBoxColumn
            {
                Name = "colRemark",
                HeaderText = "หมายเหตุ",
                DataPropertyName = "CombinedRemark",
                FillWeight = 6,
                MinimumWidth = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,   // ไม่ให้ขยาย
                Width = 140,                                           // คุมขนาดจริง
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft, WrapMode = DataGridViewTriState.False }
            };
            dtgvPhaseApprovalResult.Columns.Add(colRemark);

            // ลำดับอัตโนมัติ
            dtgvPhaseApprovalResult.RowPostPaint += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < dtgvPhaseApprovalResult.Rows.Count)
                    dtgvPhaseApprovalResult.Rows[e.RowIndex].Cells["colIndex"].Value = (e.RowIndex + 1).ToString();
            };

            // สไตล์รวม
            CustomizeGridStyling(dtgvPhaseApprovalResult);

            // ให้ header สูงขึ้นนิดนึงดูอ่านง่าย
            dtgvPhaseApprovalResult.ColumnHeadersHeight = 40;
            dtgvPhaseApprovalResult.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // โชว์ข้อความยาวเต็มผ่าน tooltip แทนการขยายแถว
            dtgvPhaseApprovalResult.ShowCellToolTips = true;
            dtgvPhaseApprovalResult.CellToolTipTextNeeded += (s, e) =>
            {
                if (e.RowIndex >= 0 &&
                    (e.ColumnIndex == dtgvPhaseApprovalResult.Columns["colPhaseDetail"].Index ||
                     e.ColumnIndex == dtgvPhaseApprovalResult.Columns["colRemark"].Index))
                {
                    var val = dtgvPhaseApprovalResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    e.ToolTipText = val?.ToString();
                }
            };
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
        // 1) ฟังก์ชันแมปสถานะ => ไทย (เพิ่ม/แก้ได้ภายหลัง)
        private static string MapStatusToThai(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return string.Empty;

            switch (status.Trim())
            {
                case "Completed": return "เสร็จสิ้น";
                case "InProgress": return "กำลังดำเนินการ";
                case "Waiting": return "รอดำเนินการ";
                case "รออนุมัติ": return "รออนุมัติ"; // เผื่อกรณี SQL ส่งมาเป็นไทยอยู่แล้ว
                default: return status;       // คงเดิมถ้ายังไม่ครอบคลุม
            }
        }
        // 2) เปิดการแปลสถานะเฉพาะคอลัมน์ "สถานะ" บน UI
        private void EnableThaiStatusDisplay()
        {
            // ป้องกันการสมัครซ้ำ
            dtgvPhaseApprovalResult.CellFormatting -= DtgvPhaseApprovalResult_CellFormatting;
            dtgvPhaseApprovalResult.CellFormatting += DtgvPhaseApprovalResult_CellFormatting;
        }
        // 3) ตัวจัดการ CellFormatting (เรนเดอร์เป็นไทยเฉพาะตอนแสดงผล)
        private void DtgvPhaseApprovalResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var grid = (DataGridView)sender;
            var col = grid.Columns[e.ColumnIndex];

            // แปลเฉพาะคอลัมน์ "สถานะ"
            if (col != null && col.Name == "colStatus")
            {
                // Convert.ToString จะคืน "" ถ้า e.Value เป็น null (รองรับ C# 7.3)
                var original = Convert.ToString(e.Value);
                var display = MapStatusToThai(original);

                if (!string.Equals(original, display, StringComparison.Ordinal))
                {
                    e.Value = display;
                    e.FormattingApplied = true;
                }
            }
        }



    }
}
