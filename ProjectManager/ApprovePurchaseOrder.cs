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

namespace JRSApplication.ProjectManager
{
    public partial class ApprovePurchaseOrder : UserControl
    {
        private readonly string _empId;

        public ApprovePurchaseOrder(string empId)
        {
            InitializeComponent();
            _empId = empId;

            InitializePOGridColumns();          // ✅ สร้างคอลัมน์ให้ชี้ DataPropertyName ถูกตั้งแต่แรก
            CustomizePOGridStyling();
            dtgvListofPO.CellFormatting += dtgvListofPO_CellFormatting; // ✅ ใส่ fallback "รออนุมัติ"

            LoadAllPurchaseOrders();            // ✅ โหลดข้อมูลหลังตั้งคอลัมน์แล้ว
        }

        // -----------------------------
        // Grid: Define Columns (ชื่อแทนรหัส)
        // -----------------------------
        private void InitializePOGridColumns()
        {
            dtgvListofPO.AutoGenerateColumns = false;

            dtgvListofPO.Columns.Clear();

            // OrderId (ซ่อน)
            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderId",
                DataPropertyName = "OrderId",
                HeaderText = "#",
                Visible = false,
                Width = 50
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
                Width = 120
            });

            // 👇 เปลี่ยนเป็นชื่อผู้ออกใบสั่งซื้อ
            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCreator",
                DataPropertyName = "EmpName",            // ✅ ชื่อผู้สร้าง (แทน EmpId)
                HeaderText = "ผู้ออกใบสั่งซื้อ",
                Width = 160
            });

            // 👇 เปลี่ยนเป็นชื่อผู้อนุมัติ
            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colApprover",
                DataPropertyName = "ApprovedByName",     // ✅ ชื่อผู้อนุมัติ (แทน ApprovedByEmpId)
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
                var orderList = dal.GetAllPurchaseOrders(); // ✅ มี EmpName และ ApprovedByName แล้ว

                if (orderList == null || orderList.Count == 0)
                {
                    dtgvListofPO.DataSource = null;
                    return;
                }

                dtgvListofPO.AutoGenerateColumns = false;
                dtgvListofPO.DataSource = orderList;
                dtgvListofPO.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("โหลดข้อมูลใบสั่งซื้อไม่สำเร็จ: " + ex.Message,
                                "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -----------------------------
        // Fallback: "รออนุมัติ"
        // -----------------------------
        private void dtgvListofPO_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var col = dtgvListofPO.Columns[e.ColumnIndex].Name;

            if (col == "colApprover")
            {
                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Value = "รออนุมัติ";
                    e.CellStyle.ForeColor = Color.Gray;
                    e.FormattingApplied = true;
                }
            }

            if (col == "colApprovedDate")
            {
                if (e.Value == null || e.Value == DBNull.Value)
                {
                    e.Value = "รออนุมัติ";
                    e.CellStyle.ForeColor = Color.Gray;
                    e.FormattingApplied = true;
                }
            }
        }

        // -----------------------------
        // Double Click -> Open POForm
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

            // reload เพื่ออัปเดตสถานะ/ชื่อผู้อนุมัติทันทีหลังปิดฟอร์ม
            LoadAllPurchaseOrders();
        }
    }
}
