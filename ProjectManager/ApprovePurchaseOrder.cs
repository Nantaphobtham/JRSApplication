using JRSApplication.Components.Service;
using JRSApplication.Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication.ProjectManager
{
    public partial class ApprovePurchaseOrder : UserControl
    {
        private readonly string _empId;

        // เก็บรายการ PO ทั้งหมดไว้สำหรับฟิลเตอร์
        private List<object> _allPurchaseOrders = new List<object>();

        public ApprovePurchaseOrder(string empId)
        {
            InitializeComponent();
            _empId = empId;

            InitializePOGridColumns();          // ✅ สร้างคอลัมน์
            CustomizePOGridStyling();           // ✅ ตั้งค่ารูปแบบ DataGridView
            dtgvListofPO.CellFormatting += dtgvListofPO_CellFormatting; // ✅ กำหนด event

            // ✅ ผูก SearchboxControl ให้ใช้ role Projectmanager / อนุมัติใบสั่งซื้อ
            try
            {
                searchboxControl1.DefaultRole = "Projectmanager";
                searchboxControl1.DefaultFunction = "อนุมัติใบสั่งซื้อ";
                searchboxControl1.SetRoleAndFunction("Projectmanager", "อนุมัติใบสั่งซื้อ");

                searchboxControl1.SearchTriggered += SearchboxPO_SearchTriggered;
            }
            catch
            {
                // กัน error ตอนออกแบบ
            }

            LoadAllPurchaseOrders();            // ✅ โหลดข้อมูลหลังตั้งคอลัมน์แล้ว
        }

        // ================= Searchbox → filter dtgvListofPO =================

        private void SearchboxPO_SearchTriggered(object sender, SearchEventArgs e)
        {
            ApplyPOGridFilter(e.SearchBy, e.Keyword);
        }

        private void ApplyPOGridFilter(string searchBy, string keyword)
        {
            if (_allPurchaseOrders == null)
                return;

            var baseList = _allPurchaseOrders;

            string q = (keyword ?? "").Trim();
            string qLower = q.ToLowerInvariant();

            if (string.IsNullOrEmpty(q))
            {
                dtgvListofPO.DataSource = null;
                dtgvListofPO.DataSource = baseList.ToList();
                return;
            }

            bool ContainsProp(object obj, string propName, string textLower)
            {
                var prop = obj.GetType().GetProperty(propName);
                if (prop == null) return false;

                var val = prop.GetValue(obj, null)?.ToString();
                return !string.IsNullOrEmpty(val) &&
                       val.ToLowerInvariant().Contains(textLower);
            }

            bool MatchStatus(object obj, string keywordText)
            {
                var prop = obj.GetType().GetProperty("OrderStatus");
                if (prop == null) return false;

                var raw = (prop.GetValue(obj, null)?.ToString() ?? "")
                            .Trim().ToLowerInvariant();

                if (string.IsNullOrEmpty(raw))
                    raw = "submitted";

                string thai;
                switch (raw)
                {
                    case "approved":
                        thai = "อนุมัติ";
                        break;
                    case "rejected":
                        thai = "ไม่อนุมัติ";
                        break;
                    case "submitted":
                        thai = "รออนุมัติ";
                        break;
                    case "draft":
                        thai = "แบบร่าง";
                        break;
                    case "canceled":
                        thai = "ยกเลิก";
                        break;
                    default:
                        thai = string.Empty;
                        break;
                }

                var k = (keywordText ?? "").Trim().ToLowerInvariant();

                // เทียบแบบตรงตัว ไม่ใช้ Contains
                return k == raw || k == thai.ToLowerInvariant();
            }

            var filtered = baseList.Where(o =>
            {
                switch (searchBy)
                {
                    case "เลขที่ใบสั่งซื้อ":
                        return ContainsProp(o, "OrderNumber", qLower);

                    case "สถานะใบสั่งซื้อ":
                    case "สถานะ":
                        return MatchStatus(o, q);

                    case "ผู้ออกใบสั่งซื้อ":
                        return ContainsProp(o, "EmpName", qLower);

                    case "ผู้อนุมัติ":
                        return ContainsProp(o, "ApprovedByName", qLower);

                    case "รหัสโครงการ":
                        return ContainsProp(o, "ProId", qLower);

                    default:
                        return ContainsProp(o, "OrderNumber", qLower)
                               || MatchStatus(o, q)
                               || ContainsProp(o, "EmpName", qLower)
                               || ContainsProp(o, "ApprovedByName", qLower)
                               || ContainsProp(o, "OrderDetail", qLower);
                }
            }).ToList();

            dtgvListofPO.DataSource = null;
            dtgvListofPO.DataSource = filtered;
        }





        // -----------------------------
        // Grid: Define Columns
        // -----------------------------
        private void InitializePOGridColumns()
        {
            dtgvListofPO.AutoGenerateColumns = false;
            dtgvListofPO.Columns.Clear();

            // ซ่อน OrderId
            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderId",
                DataPropertyName = "OrderId",
                HeaderText = "#",
                Visible = false
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colOrderNumber",
                DataPropertyName = "OrderNumber",
                HeaderText = "เลขที่ใบสั่งซื้อ",
                Width = 150
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colOrderDate",
                DataPropertyName = "OrderDate",
                HeaderText = "วันที่สั่งซื้อ",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" },
                Width = 110
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colOrderDetail",
                DataPropertyName = "OrderDetail",
                HeaderText = "รายละเอียด",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDueDate",
                DataPropertyName = "OrderDueDate",
                HeaderText = "กำหนดส่งกลับ",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" },
                Width = 120
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                DataPropertyName = "OrderStatus",
                HeaderText = "สถานะใบสั่งซื้อ",
                Width = 130
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCreator",
                DataPropertyName = "EmpName",
                HeaderText = "ผู้ออกใบสั่งซื้อ",
                Width = 160
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colApprover",
                DataPropertyName = "ApprovedByName",
                HeaderText = "ผู้อนุมัติ",
                Width = 160
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colApprovedDate",
                DataPropertyName = "ApprovedDate",
                HeaderText = "วันที่อนุมัติ",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" },
                Width = 120
            });
        }

        // -----------------------------
        // Grid: Styling
        // -----------------------------
        private void CustomizePOGridStyling()
        {
            dtgvListofPO.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvListofPO.BorderStyle = BorderStyle.None;
            dtgvListofPO.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvListofPO.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dtgvListofPO.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvListofPO.DefaultCellStyle.SelectionForeColor = Color.White;

            dtgvListofPO.EnableHeadersVisualStyles = false;
            dtgvListofPO.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvListofPO.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvListofPO.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvListofPO.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvListofPO.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvListofPO.ColumnHeadersHeight = 32;

            dtgvListofPO.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvListofPO.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvListofPO.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            dtgvListofPO.RowTemplate.Height = 30;
            dtgvListofPO.AllowUserToResizeRows = false;
            dtgvListofPO.AllowUserToAddRows = false;
            dtgvListofPO.ReadOnly = true;
            dtgvListofPO.RowHeadersVisible = false;
            dtgvListofPO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvListofPO.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        // -----------------------------
        // Load Data
        // -----------------------------
        private void LoadAllPurchaseOrders()
        {
            try
            {
                var dal = new PurchaseOrderDAL();
                var orderList = dal.GetAllPurchaseOrders();

                if (orderList == null || orderList.Count == 0)
                {
                    _allPurchaseOrders = new List<object>();
                    dtgvListofPO.DataSource = null;
                    return;
                }

                _allPurchaseOrders = orderList.Cast<object>().ToList();

                dtgvListofPO.AutoGenerateColumns = false;
                dtgvListofPO.DataSource = _allPurchaseOrders.ToList();
                dtgvListofPO.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("โหลดข้อมูลใบสั่งซื้อไม่สำเร็จ: " + ex.Message,
                                "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -----------------------------
        // CellFormatting: แสดงภาษาไทย + สี
        // -----------------------------
        private void dtgvListofPO_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string col = dtgvListofPO.Columns[e.ColumnIndex].Name;

            // ✅ แสดงสถานะใบสั่งซื้อเป็นภาษาไทย + สีพื้นหลัง
            if (col == "colStatus")
            {
                string status = "";

                // ป้องกัน null และตัดช่องว่าง
                if (e.Value != null && e.Value != DBNull.Value)
                    status = e.Value.ToString().Trim().ToLower();

                // ถ้าเป็นค่าว่างให้ default = “รออนุมัติ”
                if (string.IsNullOrEmpty(status))
                    status = "submitted";

                // ตรวจค่าทุกกรณี
                switch (status)
                {
                    case "approved":
                    case "อนุมัติ":
                        e.Value = "อนุมัติ";
                        e.CellStyle.BackColor = Color.FromArgb(46, 204, 113); // เขียว
                        e.CellStyle.ForeColor = Color.White;
                        break;

                    case "rejected":
                    case "ไม่อนุมัติ":
                        e.Value = "ไม่อนุมัติ";
                        e.CellStyle.BackColor = Color.FromArgb(231, 76, 60); // แดง
                        e.CellStyle.ForeColor = Color.White;
                        break;

                    case "submitted":
                    case "รออนุมัติ":
                        e.Value = "รออนุมัติ";
                        e.CellStyle.BackColor = Color.FromArgb(52, 152, 219); // ฟ้า
                        e.CellStyle.ForeColor = Color.White;
                        break;

                    case "draft":
                    case "แบบร่าง":
                        e.Value = "แบบร่าง";
                        e.CellStyle.BackColor = Color.FromArgb(149, 165, 166); // เทา
                        e.CellStyle.ForeColor = Color.White;
                        break;

                    case "canceled":
                    case "ยกเลิก":
                        e.Value = "ยกเลิก";
                        e.CellStyle.BackColor = Color.FromArgb(230, 126, 34); // ส้ม
                        e.CellStyle.ForeColor = Color.White;
                        break;

                    default:
                        e.Value = "ไม่ทราบสถานะ";
                        e.CellStyle.BackColor = Color.LightGray;
                        e.CellStyle.ForeColor = Color.Black;
                        break;
                }

                // ให้สีคงเดิมเวลาเลือก
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
                e.FormattingApplied = true;
            }

            // ✅ ถ้าไม่มีชื่อผู้อนุมัติ → "รออนุมัติ"
            if (col == "colApprover" && (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString())))
            {
                e.Value = "รออนุมัติ";
                e.CellStyle.ForeColor = Color.Gray;
                e.FormattingApplied = true;
            }

            // ✅ ถ้ายังไม่มีวันที่อนุมัติ → "รออนุมัติ"
            if (col == "colApprovedDate" && (e.Value == null || e.Value == DBNull.Value))
            {
                e.Value = "รออนุมัติ";
                e.CellStyle.ForeColor = Color.Gray;
                e.FormattingApplied = true;
            }
        }



        // -----------------------------
        // Double Click -> เปิด POForm
        // -----------------------------
        private void dtgvListofPO_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var selectedRow = dtgvListofPO.Rows[e.RowIndex];
            if (selectedRow.Cells["OrderId"].Value == null) return;

            int orderId = Convert.ToInt32(selectedRow.Cells["OrderId"].Value);

            using (var poForm = new POForm(orderId, _empId))
            {
                poForm.ShowDialog();
            }

            // ✅ Reload หลังปิดฟอร์ม เพื่ออัปเดตสถานะล่าสุด
            LoadAllPurchaseOrders();
        }
    }
}
