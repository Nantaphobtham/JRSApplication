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
        private static readonly string connectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        // เก็บรายการทั้งหมดไว้ใช้ฟิลเตอร์
        private List<Response> _allResponses = new List<Response>();

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

            // ✅ เพิ่ม 2 บรรทัดนี้ (ยังไม่ได้ใช้ในโค้ดนี้ แต่เผื่อใช้ต่อ)
            public string OrderByEmpId { get; set; }
            public string ApprovedByEmpId { get; set; }

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

            // ✅ ผูก SearchboxControl ให้ใช้ role Sitesupervisor / ผลการอนุมัติ
            try
            {
                searchboxControl1.DefaultRole = "Sitesupervisor";
                searchboxControl1.DefaultFunction = "ผลการอนุมัติ";
                searchboxControl1.SetRoleAndFunction("Sitesupervisor", "ผลการอนุมัติใบสั่งซื้อ");

                searchboxControl1.SearchTriggered += SearchboxWork_SearchTriggered;
            }
            catch
            {
                // กัน error ตอนออกแบบ (Designer)
            }

            LoadWorkResponse();
        }

        // ================= Searchbox → filter dtgvWorkResponse =================

        private void SearchboxWork_SearchTriggered(object sender, SearchEventArgs e)
        {
            ApplyWorkResponseFilter(e.SearchBy, e.Keyword);
        }

        private void ApplyWorkResponseFilter(string searchBy, string keyword)
        {
            if (_allResponses == null || _allResponses.Count == 0)
                return;

            var baseList = _allResponses;

            string q = (keyword ?? "").Trim();
            if (string.IsNullOrEmpty(q))
            {
                // ไม่มีคำค้น → แสดงทั้งหมด
                dtgvWorkResponse.DataSource = null;
                dtgvWorkResponse.DataSource = baseList.ToList();
                return;
            }

            q = q.ToLowerInvariant();

            bool ContainsProp(Response r, string propName, string text)
            {
                var prop = typeof(Response).GetProperty(propName);
                if (prop == null) return false;

                var val = prop.GetValue(r, null)?.ToString();
                return !string.IsNullOrEmpty(val) &&
                       val.ToLowerInvariant().Contains(text);
            }

            IEnumerable<Response> filtered;

            switch (searchBy)
            {
                case "รหัสโครงการ":
                    filtered = baseList.Where(r => ContainsProp(r, nameof(Response.ProjectId), q));
                    break;

                case "เลขที่สัญญา":
                    filtered = baseList.Where(r => ContainsProp(r, nameof(Response.ProjectNumber), q));
                    break;

                case "เฟสที่":
                    filtered = baseList.Where(r => ContainsProp(r, nameof(Response.PhaseNo), q));
                    break;

                default:
                    // ค้นหลายช่องหลัก
                    filtered = baseList.Where(r =>
                           ContainsProp(r, nameof(Response.ProjectId), q)
                        || ContainsProp(r, nameof(Response.ProjectNumber), q)
                        || ContainsProp(r, nameof(Response.PhaseNo), q)
                        || ContainsProp(r, nameof(Response.OrderNumber), q)
                        || ContainsProp(r, nameof(Response.OrderDetail), q)
                        || ContainsProp(r, nameof(Response.WorkId), q)
                        || ContainsProp(r, nameof(Response.WorkDetail), q)
                        || ContainsProp(r, nameof(Response.CombinedStatus), q)
                        || ContainsProp(r, nameof(Response.CombinedRemark), q));
                    break;
            }

            dtgvWorkResponse.DataSource = null;
            dtgvWorkResponse.DataSource = filtered.ToList();
        }

        // ================= โหลดข้อมูล =================

        private void LoadWorkResponse()
        {
            var responses = LoadResponses();
            _allResponses = responses ?? new List<Response>();

            dtgvWorkResponse.DataSource = null;
            dtgvWorkResponse.DataSource = _allResponses.ToList();

            // หลังโหลดแล้ว ถ้าใน searchbox มีค่าอยู่แล้ว ให้ฟิลเตอร์ตามนั้น
            if (searchboxControl1 != null)
            {
                ApplyWorkResponseFilter(searchboxControl1.SelectedSearchBy, searchboxControl1.Keyword);
            }
        }

        private List<Response> LoadResponses()
        {
            // ตอนนี้ดึงเฉพาะใบสั่งซื้อ (Order) ถ้าจะรวมงาน PhaseWorking ให้เอา
            // LoadPhaseWorkings() มา Concat เพิ่มได้
            return LoadPurchaseOrders();
            // ตัวอย่างถ้าจะรวม:
            // var orders = LoadPurchaseOrders();
            // var works = LoadPhaseWorkings();
            // return orders.Concat(works).ToList();
        }

        private List<Response> LoadPurchaseOrders()
        {
            var list = new List<Response>();
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
                    po.pro_id,
                    p.pro_number,
                    (SELECT MIN(pp.phase_no)
                     FROM project_phase pp
                     WHERE pp.pro_id = po.pro_id) AS phase_no
                FROM purchaseorder po
                INNER JOIN project p ON po.pro_id = p.pro_id
                WHERE po.order_status IN ('approved','submitted')
                ORDER BY po.order_id;";

            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, con))
            using (var da = new MySqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                var groupKeys = new HashSet<string>();

                foreach (DataRow row in dt.Rows)
                {
                    string orderNumber = row["order_number"]?.ToString();
                    string groupKey = orderNumber;

                    if (!groupKeys.Contains(groupKey))
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
                            OrderDate = row["order_date"] != DBNull.Value
                                ? Convert.ToDateTime(row["order_date"])
                                : (DateTime?)null,
                            DueDate = row["order_duedate"] != DBNull.Value
                                ? Convert.ToDateTime(row["order_duedate"])
                                : (DateTime?)null,
                            ApproveDate = row["approved_date"] != DBNull.Value
                                ? Convert.ToDateTime(row["approved_date"])
                                : (DateTime?)null,
                            OrderStatus = row["order_status"]?.ToString(),
                            OrderRemark = row["order_remark"]?.ToString()
                        });

                        groupKeys.Add(groupKey);
                    }
                }
            }

            return list;
        }

        public static List<Response> LoadPhaseWorkings()
        {
            var list = new List<Response>();
            string sql = @"
                SELECT 
                    pw.work_id,
                    pw.work_detail,
                    pw.work_date,
                    pw.work_end_date,
                    pw.work_status,
                    pw.work_remark,
                    pp.phase_id,
                    pp.phase_no,
                    pp.pro_id,
                    p.pro_number
                FROM phase_working pw
                INNER JOIN project_phase pp ON pw.phase_id = pp.phase_id
                INNER JOIN project p ON pp.pro_id = p.pro_id
                WHERE pw.work_status IN ('Completed', 'Waiting')
                ORDER BY pw.work_id;";

            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, con))
            using (var da = new MySqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new Response
                    {
                        RowType = RowType.Work,
                        ProjectId = row["pro_id"]?.ToString(),
                        ProjectNumber = row["pro_number"]?.ToString(),
                        PhaseNo = row["phase_no"]?.ToString(),
                        WorkId = row["work_id"]?.ToString(),
                        WorkDetail = row["work_detail"]?.ToString(),
                        WorkDate = row["work_date"] != DBNull.Value
                            ? Convert.ToDateTime(row["work_date"])
                            : (DateTime?)null,
                        WorkendDate = row["work_end_date"] != DBNull.Value
                            ? Convert.ToDateTime(row["work_end_date"])
                            : (DateTime?)null,
                        WorkStatus = row["work_status"]?.ToString(),
                        WorkRemark = row["work_remark"]?.ToString()
                    });
                }
            }

            return list;
        }

        private void PrintApprovedOrder(Response row)
        {
            try
            {
                var frm = new PurchaseOrderPrintForm(row.OrderId);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดระหว่างพิมพ์: " + ex.Message,
                    "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            //// 🧩 เฟสที่ (ถ้าต้องการแสดง ให้เอา comment ออก)
            //dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    Name = "colPhase",
            //    HeaderText = "เฟสที่",
            //    DataPropertyName = "PhaseNo"
            //});

            // 📎 เลขที่ใบสั่งซื้อ / รหัสงาน
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colItemCode",
                HeaderText = "เลขที่ใบสั่งซื้อ",
                DataPropertyName = "OrderNumber"
            });

            // 📝 รายละเอียด
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDetail",
                HeaderText = "รายละเอียดคำสั่งซื้อ",
                DataPropertyName = "OrderDetail"
            });

            // 🕒 วันที่ออกใบสั่งซื้อ / เริ่มงาน
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStartDate",
                HeaderText = "วันที่ออกใบสั่งซื้อ",
                DataPropertyName = "OrderDate",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });

            // 📅 วันที่ครบกำหนด / วันที่สิ้นสุด
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEndDate",
                HeaderText = "วันที่ครบกำหนด",
                DataPropertyName = "DueDate",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });

            // ✅ วันที่อนุมัติ
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colApproved",
                HeaderText = "วันที่อนุมัติ",
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
            var printButtonColumn = new DataGridViewButtonColumn
            {
                Name = "colPrint",
                HeaderText = "พิมพ์",
                Text = "พิมพ์เอกสาร",
                UseColumnTextForButtonValue = true,
                Width = 100
            };
            dtgvWorkResponse.Columns.Add(printButtonColumn);

            // 🔢 ลำดับอัตโนมัติ
            dtgvWorkResponse.RowPostPaint += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    dtgvWorkResponse.Rows[e.RowIndex].Cells["colIndex"].Value =
                        (e.RowIndex + 1).ToString();
            };

            // 🎨 แยกสีและ mapping สำหรับแถว Work
            dtgvWorkResponse.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;

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
                if (e.RowIndex >= 0 &&
                    e.ColumnIndex == dtgvWorkResponse.Columns["colPrint"].Index)
                {
                    var row = dtgvWorkResponse.Rows[e.RowIndex].DataBoundItem as Response;
                    if (row != null)
                    {
                        bool canPrint =
                            (row.RowType == RowType.Order &&
                             row.OrderStatus?.Equals("approved", StringComparison.OrdinalIgnoreCase) == true);
                        // ถ้าจะให้ print ได้จากงานที่ Completed ด้วย ให้ uncomment ด้านล่าง
                        // ||
                        // (row.RowType == RowType.Work &&
                        //  row.WorkStatus?.Equals("Completed", StringComparison.OrdinalIgnoreCase) == true);

                        if (canPrint)
                        {
                            PrintApprovedOrder(row);
                        }
                        else
                        {
                            MessageBox.Show(
                                "อนุญาตให้พิมพ์ได้เฉพาะรายการที่ 'อนุมัติแล้ว' เท่านั้น",
                                "ไม่สามารถพิมพ์ได้",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }
                    }
                }
            };

            CustomizeGridStyling(dtgvWorkResponse);
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
