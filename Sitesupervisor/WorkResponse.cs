using JRSApplication.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace JRSApplication.Sitesupervisor
{
    public partial class WorkResponse : UserControl
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public enum RowType
        {
            Order,
            Work
        }

        public class Response
        {
            public RowType RowType { get; set; }

            // 🔹 Project
            public string ProjectId { get; set; }
            public string ProjectNumber { get; set; }

            // 🔹 Phase
            public string PhaseNo { get; set; }

            // 🔹 Order (purchaseorder)
            public int OrderId { get; set; }
            public string OrderNumber { get; set; }
            public string OrderDetail { get; set; }
            public DateTime? OrderDate { get; set; }
            public DateTime? DueDate { get; set; }
            public DateTime? ApproveDate { get; set; }
            public string OrderStatus { get; set; }

            // 🔹 Work (phase_working)
            public string WorkId { get; set; }
            public string WorkDetail { get; set; }
            public DateTime? WorkDate { get; set; }
            public DateTime? WorkendDate { get; set; }
            public string WorkStatus { get; set; }
            public string OrderRemark { get; set; }
            public string WorkRemark { get; set; }

            // 🔹 หมายเหตุรวม
            public string CombinedRemark
            {
                get
                {
                    if (RowType == RowType.Order)
                    {
                        bool isApproved = string.Equals(OrderStatus, "approved", StringComparison.OrdinalIgnoreCase);
                        bool isSubmitted = string.Equals(OrderStatus, "submitted", StringComparison.OrdinalIgnoreCase);

                        if (isSubmitted && DueDate.HasValue && DateTime.Today > DueDate.Value.Date)
                            return "เกินกำหนดส่งกลับ";
                        return OrderRemark;
                    }

                    if (RowType == RowType.Work)
                        return WorkRemark;

                    return string.Empty;
                }
            }

            // 🔹 สถานะรวม
            public string CombinedStatus
            {
                get
                {
                    if (RowType == RowType.Order)
                        return PurchaseOrderStatus.GetDisplayName(OrderStatus);
                    else if (RowType == RowType.Work)
                        return JRSApplication.Components.WorkStatus.GetDisplayName(WorkStatus);
                    return "";
                }
            }
        }

        public WorkResponse()
        {
            InitializeComponent();
            SetupGrid();
            LoadWorkResponse();
        }

        private void SetupGrid()
        {
            dtgvWorkResponse.AutoGenerateColumns = false;
            dtgvWorkResponse.AllowUserToAddRows = false;
            dtgvWorkResponse.ReadOnly = true;
            dtgvWorkResponse.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvWorkResponse.MultiSelect = false;
            dtgvWorkResponse.RowHeadersVisible = false;
            dtgvWorkResponse.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dtgvWorkResponse.Columns.Clear();

            // 🆔 ลำดับ
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIndex",
                HeaderText = "ลำดับ"
            });

            // 📦 รหัสโครงการ
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProId",
                HeaderText = "รหัสโครงการ",
                DataPropertyName = "ProjectId"
            });

            // 📑 เลขที่สัญญา
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProNumber",
                HeaderText = "เลขที่สัญญา",
                DataPropertyName = "ProjectNumber"
            });

            // 🧩 เฟสที่
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPhase",
                HeaderText = "เฟสที่",
                DataPropertyName = "PhaseNo"
            });

            // 📎 รหัสรายการ / รหัสงาน
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colItemCode",
                HeaderText = "รหัสรายการ",
                DataPropertyName = "OrderNumber"
            });

            // 📝 รายละเอียด
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDetail",
                HeaderText = "รายละเอียด",
                DataPropertyName = "OrderDetail"
            });

            // 🕒 วันที่เริ่ม
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStartDate",
                HeaderText = "วันที่เริ่มต้น / วันที่ทำงาน",
                DataPropertyName = "OrderDate",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });

            // 📅 วันที่ครบกำหนด / เสร็จสิ้น
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEndDate",
                HeaderText = "วันที่ครบกำหนด / วันที่เสร็จ",
                DataPropertyName = "DueDate",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });

            // ✅ วันที่อนุมัติ / วันที่เสร็จสิ้น
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colApproved",
                HeaderText = "วันที่อนุมัติ / เสร็จงาน",
                DataPropertyName = "ApproveDate",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });

            // 📌 สถานะรวม
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "สถานะ",
                DataPropertyName = "CombinedStatus"
            });

            // 🗒️ หมายเหตุ
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colRemark",
                HeaderText = "หมายเหตุ",
                DataPropertyName = "CombinedRemark"
            });

            // 🖨️ ปุ่มปริ้น
            DataGridViewButtonColumn printButtonColumn = new DataGridViewButtonColumn();
            printButtonColumn.Name = "colPrint";
            printButtonColumn.HeaderText = "พิมพ์";
            printButtonColumn.Text = "พิมพ์เอกสาร";
            printButtonColumn.UseColumnTextForButtonValue = true;
            printButtonColumn.Width = 100;
            dtgvWorkResponse.Columns.Add(printButtonColumn);

            // 🔢 ลำดับอัตโนมัติ
            dtgvWorkResponse.RowPostPaint += (s, e) =>
            {
                dtgvWorkResponse.Rows[e.RowIndex].Cells["colIndex"].Value = (e.RowIndex + 1).ToString();
            };

            // 🎨 แยกสีระหว่าง Order / Work
            dtgvWorkResponse.CellFormatting += (s, e) =>
            {
                if (dtgvWorkResponse.Rows[e.RowIndex].DataBoundItem is Response row)
                {
                    if (row.RowType == RowType.Work)
                    {
                        if (dtgvWorkResponse.Columns[e.ColumnIndex].Name == "colItemCode")
                            e.Value = row.WorkId;
                        if (dtgvWorkResponse.Columns[e.ColumnIndex].Name == "colDetail")
                            e.Value = row.WorkDetail;
                        if (dtgvWorkResponse.Columns[e.ColumnIndex].Name == "colStartDate")
                            e.Value = row.WorkDate;
                        if (dtgvWorkResponse.Columns[e.ColumnIndex].Name == "colEndDate")
                            e.Value = row.WorkendDate;
                        if (dtgvWorkResponse.Columns[e.ColumnIndex].Name == "colApproved")
                            e.Value = "";

                        e.CellStyle.BackColor = Color.WhiteSmoke;
                    }
                }
            };

            // 🎯 คลิกปุ่มปริ้น
            dtgvWorkResponse.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dtgvWorkResponse.Columns["colPrint"].Index)
                {
                    var row = dtgvWorkResponse.Rows[e.RowIndex].DataBoundItem as Response;
                    if (row != null)
                    {
                        bool canPrint =
                            (row.RowType == RowType.Order &&
                             row.OrderStatus?.Equals("approved", StringComparison.OrdinalIgnoreCase) == true)
                            ||
                            (row.RowType == RowType.Work &&
                             row.WorkStatus?.Equals("completed", StringComparison.OrdinalIgnoreCase) == true);

                        if (canPrint)
                        {
                            PrintApprovedOrder(row);
                        }
                        else
                        {
                            MessageBox.Show("อนุญาตให้พิมพ์ได้เฉพาะรายการที่ 'อนุมัติแล้ว' หรือ 'เสร็จสมบูรณ์' เท่านั้น",
                                "ไม่สามารถพิมพ์ได้", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            };


            CustomizeGridStyling(dtgvWorkResponse);
        }

        private void LoadWorkResponse()
        {
            var responses = LoadResponses();
            dtgvWorkResponse.DataSource = responses;
        }

        private List<Response> LoadResponses()
        {
            string sql = @"
            SELECT 
                po.order_id,
                po.order_number,
                po.order_detail,
                po.order_date,
                po.order_status,
                po.order_duedate,
                po.approved_date,
                po.order_remark,

                pp.phase_id,
                pp.phase_no,
                pp.pro_id,

                p.pro_number,

                pw.work_id,
                pw.work_detail,
                pw.work_date,
                pw.work_end_date,
                pw.work_status,
                pw.work_remark

            FROM purchaseorder po
            INNER JOIN project_phase pp ON po.pro_id = pp.pro_id
            INNER JOIN project p ON pp.pro_id = p.pro_id
            LEFT JOIN phase_working pw 
                ON pp.phase_id = pw.phase_id 
               AND pw.work_status IN ('Completed','Waiting')
            WHERE po.order_status IN ('approved','submitted')
                AND EXISTS (
                    SELECT 1 FROM phase_working x WHERE x.phase_id = pp.phase_id
                )
            ORDER BY po.order_id, pp.phase_id, pw.work_id;";

            var list = new List<Response>();
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, con))
            using (var da = new MySqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                var orderGroupKeys = new HashSet<string>();

                foreach (DataRow row in dt.Rows)
                {
                    string orderNumber = row["order_number"]?.ToString();
                    string groupKey = $"{orderNumber}_{row["phase_id"]}";

                    if (!orderGroupKeys.Contains(groupKey))
                    {
                        list.Add(new Response
                        {
                            RowType = RowType.Order,
                            ProjectId = row["pro_id"]?.ToString(),
                            ProjectNumber = row["pro_number"]?.ToString(),
                            PhaseNo = row["phase_no"]?.ToString(),
                            OrderId = Convert.ToInt32(row["order_id"]),
                            OrderNumber = orderNumber,
                            OrderDetail = row["order_detail"]?.ToString(),
                            OrderDate = Convert.ToDateTime(row["order_date"]),
                            DueDate = row["order_duedate"] != DBNull.Value ? Convert.ToDateTime(row["order_duedate"]) : (DateTime?)null,
                            ApproveDate = row["approved_date"] != DBNull.Value ? Convert.ToDateTime(row["approved_date"]) : (DateTime?)null,
                            OrderStatus = row["order_status"]?.ToString(),
                            OrderRemark = row["order_remark"]?.ToString()
                        });
                        orderGroupKeys.Add(groupKey);
                    }

                    if (row["work_id"] != DBNull.Value)
                    {
                        var workStatus = row["work_status"]?.ToString();
                        if (workStatus == "Completed" || workStatus == "Waiting")
                        {
                            list.Add(new Response
                            {
                                RowType = RowType.Work,
                                ProjectId = row["pro_id"]?.ToString(),
                                ProjectNumber = row["pro_number"]?.ToString(),
                                PhaseNo = row["phase_no"]?.ToString(),
                                WorkId = row["work_id"]?.ToString(),
                                WorkDetail = row["work_detail"]?.ToString(),
                                WorkDate = row["work_date"] != DBNull.Value ? Convert.ToDateTime(row["work_date"]) : (DateTime?)null,
                                WorkendDate = row["work_end_date"] != DBNull.Value ? Convert.ToDateTime(row["work_end_date"]) : (DateTime?)null,
                                WorkStatus = workStatus,
                                WorkRemark = row["work_remark"]?.ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        private void PrintApprovedOrder(Response row)
        {
            try
            {
                // 🔹 เปิดหน้ารายงานใบสั่งซื้อ (เชื่อมกับ RDLC)
                var frm = new PurchaseOrderPrintForm(row.OrderId);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดระหว่างพิมพ์: " + ex.Message,
                    "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
