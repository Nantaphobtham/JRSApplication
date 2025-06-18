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
            _empId = empId; // 📌 เก็บไว้ใช้ภายใน UserControl
            LoadAllPurchaseOrders();
            InitializePOGridColumns();
            CustomizePOGridStyling();
        }


        private void InitializePOGridColumns()
        {
            dtgvListofPO.AutoGenerateColumns = false;

            // 🔒 ป้องกันซ้ำ: ถ้ามี column แล้ว ไม่ต้อง Add ซ้ำ
            if (dtgvListofPO.Columns.Count > 0)
                return;

            // ✅ OrderId (ซ่อนไว้)
            var colOrderId = new DataGridViewTextBoxColumn
            {
                Name = "OrderId", // 👈 สำคัญมาก!
                DataPropertyName = "OrderId",
                HeaderText = "#",
                Width = 50,
                Visible = false
            };
            dtgvListofPO.Columns.Add(colOrderId);

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderNumber",
                HeaderText = "เลขที่ใบสั่งซื้อ",
                Width = 150
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderDate",
                HeaderText = "วันที่สั่งซื้อ",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" },
                Width = 100
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderDetail",
                HeaderText = "รายละเอียด",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderDueDate",
                HeaderText = "กำหนดส่งกลับ",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" },
                Width = 120
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderStatus",
                HeaderText = "สถานะใบสั่งซื้อ",
                Width = 100
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "EmpId",
                HeaderText = "รหัสผู้ออกใบสั่งซื้อ",
                Width = 100
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ApprovedByEmpId",
                HeaderText = "ผู้อนุมัติ",
                Width = 100
            });

            dtgvListofPO.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ApprovedDate",
                HeaderText = "วันที่อนุมัติ",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" },
                Width = 120
            });
        }

        private void dtgvListofPO_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dtgvListofPO.Columns[e.ColumnIndex].DataPropertyName == "ApprovedByEmpId")
            {
                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Value = "รออนุมัติ";
                    e.CellStyle.ForeColor = Color.Gray;
                }
            }

            if (dtgvListofPO.Columns[e.ColumnIndex].DataPropertyName == "ApprovedDate")
            {
                if (e.Value == null || e.Value == DBNull.Value)
                {
                    e.Value = "รออนุมัติ";
                    e.CellStyle.ForeColor = Color.Gray;
                }
            }
        }

        private void LoadAllPurchaseOrders()
        {
            var dal = new PurchaseOrderDAL();
            var orderList = dal.GetAllPurchaseOrders();

            // 🔐 ป้องกันกรณีไม่มีข้อมูล
            if (orderList == null || orderList.Count == 0)
            {
                MessageBox.Show("ไม่พบข้อมูลใบสั่งซื้อ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtgvListofPO.DataSource = null;
                return;
            }

            // 🧱 ใช้ BindingSource เพื่อความยืดหยุ่น
            var bindingSource = new BindingSource();
            bindingSource.DataSource = orderList;

            // 🧼 เคลียร์คอลัมน์เก่าหากมี (สำคัญมาก ถ้ามี reload หลายรอบ)
            dtgvListofPO.Columns.Clear();
            InitializePOGridColumns(); // 🏗️ สร้างคอลัมน์ใหม่

            dtgvListofPO.AutoGenerateColumns = false;
            dtgvListofPO.DataSource = bindingSource;
            dtgvListofPO.ClearSelection();
        }
        private void CustomizePOGridStyling()
        {
            dtgvListofPO.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvListofPO.BorderStyle = BorderStyle.None;

            // 🪄 แถวสลับสี
            dtgvListofPO.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // 🖌️ เส้นกรอบเซลล์
            dtgvListofPO.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // 🎨 สีเมื่อเลือกแถว
            dtgvListofPO.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvListofPO.DefaultCellStyle.SelectionForeColor = Color.White;

            // 🎯 หัวตาราง
            dtgvListofPO.EnableHeadersVisualStyles = false;
            dtgvListofPO.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvListofPO.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvListofPO.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvListofPO.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvListofPO.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvListofPO.ColumnHeadersHeight = 32;

            // 📄 เซลล์ทั่วไป
            dtgvListofPO.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvListofPO.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvListofPO.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            // 📏 ขนาดแถว
            dtgvListofPO.RowTemplate.Height = 30;

            // ❌ ไม่ให้ resize
            dtgvListofPO.AllowUserToResizeRows = false;
            dtgvListofPO.AllowUserToAddRows = false;
            dtgvListofPO.ReadOnly = true;

            // 🔲 ซ่อน Row Header
            dtgvListofPO.RowHeadersVisible = false;

            // 🧲 ขนาดอัตโนมัติ
            dtgvListofPO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvListofPO.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void dtgvListofPO_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dtgvListofPO.Rows[e.RowIndex];
                int orderId = Convert.ToInt32(selectedRow.Cells["OrderId"].Value);

                // เรียกฟอร์ม PO พร้อมส่ง orderId ไป
                POForm poForm = new POForm(orderId, _empId);
                poForm.ShowDialog();
            }
        }



    }
}
