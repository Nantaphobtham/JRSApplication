using JRSApplication.Components;
using JRSApplication.Components.Models;
using JRSApplication.Components.Service;
using JRSApplication.Data_Access_Layer;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace JRSApplication
{
    public partial class PurchaseOrderForm : UserControl
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        private readonly string _empId;

        private BindingList<MaterialDetail> materialList = new BindingList<MaterialDetail>();
        private int? editingRowIndex = null;

        // รายการหน่วย
        List<string> unitList = new List<string>
        {
            "--เลือก--","เส้น","ก้อน","แผ่น","กล่อง","โหล","หลอด",
            "ถุง","ถัง","ชิ้น","กก.","ใบ","อัน","ขด",
            "บาน","แพ็ค","เครื่อง","กล.","ดอก","มัด","ม้วน",
            "ตัว","ท่อน","วง","เมตร","ตร.ม.","ลบ.ม.",
            "ลัง","แกลลอน","ถ้วย","คู่","เม็ด","ฟุต","ตัน","ชั้น","ช่อง"
        };

        // เก็บรายการใบสั่งซื้อทั้งหมด ไว้ใช้กับ Searchbox
        private List<PurchaseOrder> _allPurchaseOrders = new List<PurchaseOrder>();

        public PurchaseOrderForm(string empId)
        {
            InitializeComponent();
            _empId = empId;

            // สร้าง column และตกแต่ง
            InitializePOGridColumns();
            InitializeMaterialGridColumns();

            CustomizeGridStyling(dtgvPurchaseOrderList);
            CustomizeGridStyling(dtgvMaterialList);

            dtgvMaterialList.DataSource = materialList;
            dtgvPurchaseOrderList.ClearSelection();
            dtgvMaterialList.ClearSelection();

            btnEditOrder.Click += btnEditOrder_Click;

            // ComboBox หน่วยนับ
            cmbUnit.DataSource = unitList;
            cmbUnit.SelectedIndex = 0;
            cmbUnit.AutoCompleteMode = AutoCompleteMode.None;
            cmbUnit.AutoCompleteSource = AutoCompleteSource.None;

            // โหลดข้อมูลใบสั่งซื้อ (เก็บใน _allPurchaseOrders ด้วย)
            LoadAllPurchaseOrders();

            // ผูก Searchbox ให้ใช้ role Sitesupervisor / ฟังก์ชัน "ออกใบสั่งซื้อ"
            try
            {
                searchboxControl1.DefaultRole = "Sitesupervisor";
                searchboxControl1.DefaultFunction = "ออกใบสั่งซื้อ";
                searchboxControl1.SetRoleAndFunction("Sitesupervisor", "ออกใบสั่งซื้อ");

                searchboxControl1.SearchTriggered += SearchboxPO_SearchTriggered;
            }
            catch
            {
                // กัน error ตอนเปิดใน Designer
            }

            // Event เดิม
            dtgvPurchaseOrderList.CellDoubleClick += dtgvPurchaseOrderList_CellDoubleClick;
            dtgvPurchaseOrderList.CellFormatting += dtgvPurchaseOrderList_CellFormatting;
            dtgvPurchaseOrderList.CellMouseDown += dtgvPurchaseOrderList_CellMouseDown;
            dtgvPurchaseOrderList.CellMouseLeave += dtgvPurchaseOrderList_CellMouseLeave;

            dtgvMaterialList.CellClick += dtgvMaterialList_CellClick;

            txtQuantity.TextChanged += txtQuantity_TextChanged;
            txtUnitPrice.TextChanged += txtUnitPrice_TextChanged;
            cmbUnit.TextUpdate += txtUnit_TextUpdate;

            btnAddMaterial.Click += btnAddMaterial_Click;
            btnEditMaterial.Click += btnEditMaterial_Click;
            btnAddOrder.Click += btnAddOrder_Click;
            btnSaveOrder.Click += btnSaveOrder_Click;

            // ปุ่มยกเลิกใบสั่งซื้อ (btnDeleteOrder มีอยู่แล้วใน Designer)
            btnDeleteOrder.Click += btnDeleteOrder_Click;
        }

        // ================= Searchbox → filter dtgvPurchaseOrderList =================

        private void SearchboxPO_SearchTriggered(object sender, SearchEventArgs e)
        {
            ApplyPOFilter(e.SearchBy, e.Keyword);
        }

        private void ApplyPOFilter(string searchBy, string keyword)
        {
            if (_allPurchaseOrders == null || _allPurchaseOrders.Count == 0)
                return;

            var baseList = _allPurchaseOrders;

            string q = (keyword ?? "").Trim();
            if (string.IsNullOrEmpty(q))
            {
                dtgvPurchaseOrderList.DataSource = null;
                dtgvPurchaseOrderList.DataSource = baseList.ToList();
                dtgvPurchaseOrderList.ClearSelection();
                return;
            }

            string qLower = q.ToLowerInvariant();

            // แมปคำค้นสถานะ (ไทย/อังกฤษ) -> code ที่เก็บใน OrderStatus
            string MapStatusKeyword(string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return text;

                // ไทย
                if (text.Contains("อนุมัติแล้ว") || text.Contains("อนุมัติ"))
                    return "approved";
                if (text.Contains("รอดำเนินการ") || text.Contains("รออนุมัติ") || text.Contains("รอ"))
                    return "submitted";
                if (text.Contains("ไม่อนุมัติ") || text.Contains("ปฏิเสธ"))
                    return "rejected";
                if (text.Contains("ยกเลิก"))
                    return "canceled";

                // อังกฤษเดิม
                return text;
            }

            bool ContainsProp(PurchaseOrder o, string propName, string text)
            {
                var prop = typeof(PurchaseOrder).GetProperty(propName);
                if (prop == null) return false;

                var val = prop.GetValue(o, null)?.ToString();
                return !string.IsNullOrEmpty(val) &&
                       val.ToLowerInvariant().Contains(text);
            }

            IEnumerable<PurchaseOrder> filtered;

            switch (searchBy)
            {
                case "เลขที่ใบสั่งซื้อ":
                    filtered = baseList.Where(o => ContainsProp(o, "OrderNumber", qLower));
                    break;

                case "สถานะใบสั่งซื้อ":
                    string statusKey = MapStatusKeyword(qLower);
                    filtered = baseList.Where(o =>
                        !string.IsNullOrEmpty(o.OrderStatus) &&
                        o.OrderStatus.ToLowerInvariant().Contains(statusKey));
                    break;

                default:
                    // ค้นทุกช่องหลัก ๆ
                    filtered = baseList.Where(o =>
                           ContainsProp(o, "OrderNumber", qLower)
                        || ContainsProp(o, "OrderDetail", qLower)
                        || ContainsProp(o, "EmpName", qLower)
                        || ContainsProp(o, "ApprovedByName", qLower)
                        || o.OrderStatus != null &&
                           o.OrderStatus.ToLowerInvariant().Contains(MapStatusKeyword(qLower))
                    );
                    break;
            }

            var list = filtered.ToList();

            dtgvPurchaseOrderList.DataSource = null;
            dtgvPurchaseOrderList.DataSource = list;
            dtgvPurchaseOrderList.ClearSelection();
        }


        // ---------------- Grid Column ----------------
        private void InitializePOGridColumns()
        {
            dtgvPurchaseOrderList.AutoGenerateColumns = false;
            dtgvPurchaseOrderList.Columns.Clear();

            dtgvPurchaseOrderList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderId",
                HeaderText = "#",
                Visible = false
            });
            dtgvPurchaseOrderList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderNumber",
                HeaderText = "เลขที่ใบสั่งซื้อ",
                Width = 150
            });
            dtgvPurchaseOrderList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderDate",
                HeaderText = "วันที่สั่งซื้อ",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
            dtgvPurchaseOrderList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderDetail",
                HeaderText = "รายละเอียด",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dtgvPurchaseOrderList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderDueDate",
                HeaderText = "กำหนดส่งกลับ",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
            dtgvPurchaseOrderList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderStatus",
                HeaderText = "สถานะใบสั่งซื้อ",
                Width = 120
            });
            dtgvPurchaseOrderList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "EmpName",
                HeaderText = "ผู้สร้าง"
            });
            dtgvPurchaseOrderList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ApprovedByName",
                HeaderText = "ผู้อนุมัติ"
            });
            dtgvPurchaseOrderList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ApprovedDate",
                HeaderText = "วันที่อนุมัติ",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
        }

        private void InitializeMaterialGridColumns()
        {
            dtgvMaterialList.AutoGenerateColumns = false;
            dtgvMaterialList.Columns.Clear();

            dtgvMaterialList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatNo",
                Name = "MatNo",
                HeaderText = "ลำดับ",
                Width = 60
            });
            dtgvMaterialList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatDetail",
                Name = "MatDetail",
                HeaderText = "ชื่อวัสดุ",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dtgvMaterialList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatQuantity",
                Name = "MatQuantity",
                HeaderText = "จำนวน"
            });
            dtgvMaterialList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatUnit",
                Name = "MatUnit",
                HeaderText = "หน่วย"
            });
            dtgvMaterialList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatPrice",
                Name = "MatPrice",
                HeaderText = "ราคาต่อหน่วย",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });
            dtgvMaterialList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatAmount",
                Name = "MatAmount",
                HeaderText = "ราคารวม",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });
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

        // ---------------- Event Handlers (วัสดุ) ----------------
        private void txtQuantity_TextChanged(object sender, EventArgs e) => CalculateTotalPrice();
        private void txtUnitPrice_TextChanged(object sender, EventArgs e) => CalculateTotalPrice();

        private void txtUnit_TextUpdate(object sender, EventArgs e)
        {
            string filterParam = cmbUnit.Text.Trim();

            var filtered = unitList
                .Skip(1)
                .Where(x => x.Contains(filterParam))
                .ToList();

            filtered.Insert(0, "--เลือก--");

            if (filtered.Count == 1)
                filtered = new List<string>(unitList);

            cmbUnit.DataSource = null;
            cmbUnit.DataSource = filtered;
            cmbUnit.Text = filterParam;
            cmbUnit.SelectionStart = cmbUnit.Text.Length;
            cmbUnit.DroppedDown = true;
        }

        // ---------------- ปุ่ม / ฟังก์ชันใบสั่งซื้อ ----------------

        private void btnEditOrder_Click(object sender, EventArgs e)
        {
            // 1) ต้องมีการเลือกใบสั่งซื้อในตารางด้านซ้ายก่อน
            if (dtgvPurchaseOrderList.CurrentRow == null)
            {
                MessageBox.Show("กรุณาเลือกใบสั่งซื้อที่จะแก้ไข",
                                "แจ้งเตือน",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // 2) ดึง object PurchaseOrder จากแถวที่เลือก
            var po = dtgvPurchaseOrderList.CurrentRow.DataBoundItem as PurchaseOrder;
            if (po == null) return;

            // ถ้าไม่อนุญาตให้แก้ไขใบที่อนุมัติแล้ว / ยกเลิกแล้ว ให้เช็คสถานะที่นี่
            if (string.Equals(po.OrderStatus, "approved", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("ใบสั่งซื้อที่อนุมัติแล้ว ไม่สามารถแก้ไขได้",
                                "แจ้งเตือน",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }
            if (string.Equals(po.OrderStatus, "canceled", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("ใบสั่งซื้อที่ถูกยกเลิกแล้ว ไม่สามารถแก้ไขได้",
                                "แจ้งเตือน",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            // 3) โหลดข้อมูลใบสั่งซื้อ + รายการวัสดุเข้าฟอร์ม (ใช้เมธอดเดิมที่คุณมีอยู่)
            ShowPurchaseOrderDetails(po);

            // 4) เปิดโหมดแก้ไข: ให้ textboxes / comboboxs / ปุ่มวัสดุ แก้ไขได้
            EnableAllSections();          // เมธอดนี้ของคุณ enable textbox/combobox ต่าง ๆ อยู่แล้ว

            // ปรับปุ่มด้านวัสดุ
            btnAddMaterial.Enabled = true;         // ใช้บันทึกทั้งเพิ่มใหม่/แก้ไข
            btnEditMaterial.Enabled = false;      // จะ enable หลังจากเลือกแถววัสดุ
            btnAddMaterial.Text = "บันทึกแก้ไข";
            btnAddMaterial.BackColor = Color.Orange;

            // ปุ่มบันทึกใบสั่งซื้อ
            btnSaveOrder.Enabled = true;
            // ถ้าอยากให้ user เห็นว่าเป็นโหมดแก้ไขจริง ๆ
            //btnSaveOrder.Text = "บันทึกการแก้ไข";
        }


        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            EnableAllSections();
            ClearOrderForm();
            ClearMaterialForm();
            materialList.Clear();
            dtgvMaterialList.DataSource = null;
            dtgvMaterialList.DataSource = materialList;
            dtgvMaterialList.ClearSelection();
            lblSummation.Text = "รวม 0 รายการ\nรวมยอดเงิน 0.00 บาท";

            btnAddMaterial.Enabled = true;
            btnSaveOrder.Enabled = true;
            btnEditMaterial.Text = "แก้ไข";
            btnAddMaterial.Text = "เพิ่ม";
            editingRowIndex = null;

            txtOrderNO.Text = GenerateNextOrderNumber();
            dtpOrderDate.Value = DateTime.Today;
            cmbDueDate.SelectedIndex = -1;
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            if (!ValidatePO()) return;

            try
            {
                var order = new PurchaseOrder
                {
                    OrderNumber = txtOrderNO.Text.Trim(),
                    OrderDetail = txtOrderDetail.Text.Trim(),
                    OrderDate = dtpOrderDate.Value.Date,
                    OrderDueDate = CalculateDueDate(),
                    OrderStatus = "submitted",
                    ProId = int.Parse(txtProjectID.Text),
                    EmpId = _empId,
                    MaterialDetails = materialList.ToList()
                };

                var poDal = new PurchaseOrderDAL();
                int newOrderId = poDal.InsertFullPurchaseOrder(order);

                if (newOrderId <= 0)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกใบสั่งซื้อ", "ผิดพลาด",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("บันทึกใบสั่งซื้อเรียบร้อยแล้ว", "สำเร็จ",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearOrderForm();
                ClearMaterialForm();
                materialList.Clear();
                dtgvMaterialList.DataSource = null;
                lblSummation.Text = "รวม 0 รายการ\nรวมยอดเงิน 0.00 บาท";

                LoadAllPurchaseOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "Exception",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ปุ่ม “ยกเลิกใบสั่งซื้อ” ใช้ btnDeleteOrder
        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (dtgvPurchaseOrderList.CurrentRow == null)
            {
                MessageBox.Show("กรุณาเลือกใบสั่งซื้อที่ต้องการยกเลิก",
                                "แจ้งเตือน",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
            else
            {
                var po = dtgvPurchaseOrderList.CurrentRow.DataBoundItem as PurchaseOrder;
                if (po == null) return;

                // ถ้าอนุมัติแล้ว ไม่ให้ยกเลิก
                if (string.Equals(po.OrderStatus, "approved", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("ใบสั่งซื้อที่อนุมัติแล้ว ไม่สามารถยกเลิกได้",
                                    "แจ้งเตือน",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    return;
                }

                // ถ้ายกเลิกไปแล้ว
                if (string.Equals(po.OrderStatus, "canceled", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("ใบสั่งซื้อนี้ถูกยกเลิกไปแล้ว",
                                    "แจ้งเตือน",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    return;
                }

                var confirm = MessageBox.Show(
                    $"ต้องการยกเลิกใบสั่งซื้อเลขที่ {po.OrderNumber} หรือไม่?",
                    "ยืนยันการยกเลิก",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                try
                {
                    var dal = new PurchaseOrderDAL();
                    // ต้องมีเมทอดนี้ใน DAL (ดูตัวอย่างด้านล่าง)
                    dal.UpdateOrderStatus(
                        po.OrderId,
                        "canceled",
                        "ยกเลิกโดยผู้ใช้ ",
                        _empId
                        );

                    MessageBox.Show("ยกเลิกใบสั่งซื้อเรียบร้อยแล้ว",
                                    "สำเร็จ",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    LoadAllPurchaseOrders();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ไม่สามารถยกเลิกใบสั่งซื้อได้ : " + ex.Message,
                                    "ผิดพลาด",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        // ---------------- ปุ่ม / ฟังก์ชันวัสดุ ----------------

        private void btnAddMaterial_Click(object sender, EventArgs e)
        {
            if (!ValidateMaterialData()) return;

            string matName = txtMaterialName.Text.Trim();
            string unit = cmbUnit.Text.Trim();
            decimal qty = decimal.Parse(txtQuantity.Text.Trim());
            decimal price = decimal.Parse(txtUnitPrice.Text.Trim());
            decimal amount = qty * price;

            if (editingRowIndex != null)
            {
                var material = materialList[editingRowIndex.Value];
                material.MatDetail = matName;
                material.MatQuantity = qty;
                material.MatPrice = price;
                material.MatAmount = amount;
                material.MatUnit = unit;

                dtgvMaterialList.DataSource = null;
                dtgvMaterialList.DataSource = materialList;
                dtgvMaterialList.ClearSelection();

                editingRowIndex = null;
                btnAddMaterial.Text = "เพิ่ม";
                btnAddMaterial.BackColor = Color.LightGreen;
            }
            else
            {
                var material = new MaterialDetail
                {
                    MatNo = materialList.Count + 1,
                    MatDetail = matName,
                    MatQuantity = qty,
                    MatPrice = price,
                    MatAmount = amount,
                    MatUnit = unit
                };
                materialList.Add(material);

                dtgvMaterialList.DataSource = null;
                dtgvMaterialList.DataSource = materialList;
                dtgvMaterialList.ClearSelection();
            }

            ClearMaterialForm();
            UpdateMaterialSummary();
            cmbUnit.SelectedIndex = 0;
            txtMaterialName.ReadOnly = false;
            txtUnitPrice.ReadOnly = false;
            txtQuantity.ReadOnly = false;
            cmbUnit.Enabled = true;
            btnEditMaterial.Enabled = false;
        }

        private void btnEditMaterial_Click(object sender, EventArgs e)
        {
            txtMaterialName.ReadOnly = false;
            txtUnitPrice.ReadOnly = false;
            txtQuantity.ReadOnly = false;
            cmbUnit.Enabled = true;

            txtMaterialName.Focus();
        }

        private void dtgvMaterialList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dtgvMaterialList.Rows.Count)
            {
                DataGridViewRow row = dtgvMaterialList.Rows[e.RowIndex];

                // ดึงค่าจากแถวที่เลือกมาใส่ textbox
                txtMaterialName.Text = row.Cells["MatDetail"].Value?.ToString();
                txtUnitPrice.Text = row.Cells["MatPrice"].Value?.ToString();
                txtQuantity.Text = row.Cells["MatQuantity"].Value?.ToString();
                cmbUnit.Text = row.Cells["MatUnit"].Value?.ToString();

                CalculateTotalPrice();
                editingRowIndex = e.RowIndex;

                // สำคัญ: "อย่าล็อก" ช่องอีก ปล่อยให้สถานะ ReadOnly/Enabled
                // เป็นไปตามที่ ShowPurchaseOrderDetails / btnAddOrder ตั้งค่าไว้

                // แค่เปลี่ยนโหมดปุ่มให้รู้ว่าเป็นการแก้ไข
                if (btnAddMaterial.Enabled)
                {
                    btnAddMaterial.Text = "บันทึกแก้ไข";
                    btnAddMaterial.BackColor = Color.Orange;
                }

                // ถ้าอยากใช้ปุ่ม "แก้ไข" แยก ก็ enable ไว้ (จะใช้หรือไม่ใช้ก็ได้)
                btnEditMaterial.Enabled = btnAddMaterial.Enabled;
            }
        }


        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            var searchForm = new SearchForm("Project");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                txtProjectID.Text = searchForm.SelectedID;
                txtProjectNumber.Text = searchForm.SelectedContract;
                txtProjectName.Text = searchForm.SelectedName;
            }
        }

        private void dtgvPurchaseOrderList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dtgvPurchaseOrderList.Rows[e.RowIndex];
            var po = row.DataBoundItem as PurchaseOrder;
            if (po == null) return;

            ShowPurchaseOrderDetails(po);
        }

        private void ShowPurchaseOrderDetails(PurchaseOrder po)
        {
            if (po == null) return;

            // ── ข้อมูลหัวใบสั่งซื้อ ─────────────────────
            txtOrderNO.Text = po.OrderNumber;
            txtOrderDetail.Text = po.OrderDetail;
            dtpOrderDate.Value = po.OrderDate == DateTime.MinValue
                ? DateTime.Today
                : po.OrderDate;

            // ── โหลดรายการวัสดุจากฐานข้อมูล ─────────────
            var dal = new PurchaseOrderDAL();
            var materials = dal.GetMaterialDetailsByOrderId(po.OrderId);

            materialList.Clear();
            int seq = 1;
            foreach (var m in materials)
            {
                materialList.Add(new MaterialDetail
                {
                    MatNo = seq++,
                    MatDetail = m.MatDetail,
                    MatQuantity = m.MatQuantity,
                    MatPrice = m.MatPrice,
                    MatUnit = m.MatUnit,
                    MatAmount = m.MatAmount
                });
            }

            dtgvMaterialList.DataSource = null;
            dtgvMaterialList.DataSource = materialList;
            dtgvMaterialList.ClearSelection();
            UpdateMaterialSummary();

            // ── ตัดสินใจว่าใบนี้อนุญาตให้แก้ไขวัสดุได้ไหม ──
            // ถ้าไม่อยากเช็คสถานะเลย ให้ใช้ `bool canEditMaterials = true;`
            bool canEditMaterials =
                !string.Equals(po.OrderStatus, "approved", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(po.OrderStatus, "canceled", StringComparison.OrdinalIgnoreCase);

            // ช่องสำหรับแก้ไขวัสดุ (ด้านขวา)
            txtMaterialName.ReadOnly = !canEditMaterials;
            txtUnitPrice.ReadOnly = !canEditMaterials;
            txtQuantity.ReadOnly = !canEditMaterials;
            cmbUnit.Enabled = canEditMaterials;

            // ปุ่มวัสดุ
            btnAddMaterial.Enabled = canEditMaterials;   // ใช้ทั้งเพิ่มใหม่ + บันทึกแก้ไข
            btnEditMaterial.Enabled = false;             // จะ enable หลังจากเลือกแถว
            editingRowIndex = null;

            // เคลียร์ฟอร์มแก้ไขวัสดุ
            ClearMaterialForm();
        }



        // ---------------- Helpers ----------------
        private void CalculateTotalPrice()
        {
            if (decimal.TryParse(txtQuantity.Text, out decimal qty) &&
                decimal.TryParse(txtUnitPrice.Text, out decimal price))
            {
                txtTotalPrice.Text = (qty * price).ToString("N2");
            }
            else
            {
                txtTotalPrice.Text = "0.00";
            }
        }

        private void UpdateMaterialSummary()
        {
            int count = materialList.Count;
            decimal total = materialList.Sum(m => m.MatAmount);
            lblSummation.Text = $"รวม {count} รายการ\nรวมยอดเงิน {total:N2} บาท";
        }

        private void ClearOrderForm()
        {
            txtOrderNO.Clear();
            txtOrderDetail.Clear();
            dtpOrderDate.Value = DateTime.Today;
            cmbDueDate.SelectedIndex = -1;
        }

        private void ClearMaterialForm()
        {
            txtMaterialName.Clear();
            txtQuantity.Clear();
            txtUnitPrice.Clear();
            txtTotalPrice.Clear();
        }

        private void EnableAllSections()
        {
            txtOrderNO.Enabled = true;
            txtOrderDetail.Enabled = true;
            dtpOrderDate.Enabled = true;
            cmbDueDate.Enabled = true;

            txtMaterialName.Enabled = true;
            txtQuantity.Enabled = true;
            cmbUnit.Enabled = true;
            txtUnitPrice.Enabled = true;
            txtTotalPrice.Enabled = true;

            btnAddMaterial.Enabled = true;
            btnEditMaterial.Enabled = true;
            dtgvMaterialList.Enabled = true;
        }

        private bool ValidatePO()
        {
            if (string.IsNullOrWhiteSpace(txtOrderNO.Text))
            {
                MessageBox.Show("กรุณากรอกเลขที่ใบสั่งซื้อ"); return false;
            }
            if (string.IsNullOrWhiteSpace(txtOrderDetail.Text))
            {
                MessageBox.Show("กรุณากรอกรายละเอียดใบสั่งซื้อ"); return false;
            }
            if (cmbDueDate.SelectedItem == null)
            {
                MessageBox.Show("กรุณาเลือกกำหนดส่งกลับ"); return false;
            }
            if (materialList.Count == 0)
            {
                MessageBox.Show("กรุณาเพิ่มรายการวัสดุอย่างน้อย 1 รายการ"); return false;
            }
            if (string.IsNullOrWhiteSpace(txtProjectID.Text) || txtProjectID.Text == "0")
            {
                MessageBox.Show("กรุณาเลือกโครงการก่อนบันทึก"); return false;
            }
            return true;
        }

        private bool ValidateMaterialData()
        {
            if (string.IsNullOrWhiteSpace(txtMaterialName.Text))
            {
                MessageBox.Show("กรุณากรอกชื่อวัสดุ"); return false;
            }
            if (!decimal.TryParse(txtQuantity.Text, out decimal qty) || qty <= 0)
            {
                MessageBox.Show("กรุณากรอกจำนวนที่ถูกต้อง"); return false;
            }
            if (!decimal.TryParse(txtUnitPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("กรุณากรอกราคาต่อหน่วยที่ถูกต้อง"); return false;
            }
            if (string.IsNullOrWhiteSpace(cmbUnit.Text) || cmbUnit.SelectedIndex == 0)
            {
                MessageBox.Show("กรุณาเลือกหน่วยนับ"); return false;
            }
            return true;
        }

        private DateTime CalculateDueDate()
        {
            if (cmbDueDate.SelectedItem == null) return DateTime.Today;
            int days;
            if (int.TryParse(cmbDueDate.SelectedItem.ToString(), out days))
            {
                return dtpOrderDate.Value.AddDays(days);
            }
            return dtpOrderDate.Value;
        }

        private string GenerateNextOrderNumber()
        {
            return "PO-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void LoadAllPurchaseOrders()
        {
            var dal = new PurchaseOrderDAL();
            var orderList = dal.GetAllPurchaseOrders();

            if (orderList == null || orderList.Count == 0)
            {
                dtgvPurchaseOrderList.DataSource = null;
                _allPurchaseOrders = new List<PurchaseOrder>();
                return;
            }

            _allPurchaseOrders = orderList.ToList();

            dtgvPurchaseOrderList.AutoGenerateColumns = false;
            dtgvPurchaseOrderList.DataSource = _allPurchaseOrders.ToList();
            dtgvPurchaseOrderList.ClearSelection();

            // ถ้ามีคำค้นอยู่แล้ว ให้ฟิลเตอร์ตาม Searchbox
            if (searchboxControl1 != null)
            {
                ApplyPOFilter(searchboxControl1.SelectedSearchBy, searchboxControl1.Keyword);
            }
        }

        private void dtgvPurchaseOrderList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dtgvPurchaseOrderList.Columns[e.ColumnIndex].DataPropertyName == "ApprovedByName")
            {
                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Value = "รออนุมัติ";
                    e.CellStyle.ForeColor = Color.Gray;
                }
            }
            if (dtgvPurchaseOrderList.Columns[e.ColumnIndex].DataPropertyName == "ApprovedDate")
            {
                if (e.Value == null || e.Value == DBNull.Value)
                {
                    e.Value = "รออนุมัติ";
                    e.CellStyle.ForeColor = Color.Gray;
                }
            }
            if (dtgvPurchaseOrderList.Columns[e.ColumnIndex].DataPropertyName == "OrderStatus")
            {
                if (e.Value != null)
                {
                    switch (e.Value.ToString())
                    {
                        case "submitted":
                            e.Value = "รอดำเนินการ";
                            break;
                        case "approved":
                            e.Value = "อนุมัติแล้ว";
                            break;
                        case "rejected":
                            e.Value = "ไม่อนุมัติ";
                            break;
                        case "canceled":
                            e.Value = "ยกเลิกแล้ว";
                            break;
                    }
                }
            }
        }

        private Timer hoverTimer = new Timer();
        private int hoveredRowIndex = -1;

        private void StartHoverPreviewTimer(int rowIndex)
        {
            hoveredRowIndex = rowIndex;

            hoverTimer.Interval = 1000;
            hoverTimer.Tick -= HoverTimer_Tick;
            hoverTimer.Tick += HoverTimer_Tick;
            hoverTimer.Start();
        }

        private void HoverTimer_Tick(object sender, EventArgs e)
        {
            hoverTimer.Stop();

            if (hoveredRowIndex >= 0 && hoveredRowIndex < dtgvPurchaseOrderList.Rows.Count)
            {
                var row = dtgvPurchaseOrderList.Rows[hoveredRowIndex];
                var po = row.DataBoundItem as PurchaseOrder;

                if (po != null)
                {
                    var previewForm = new POForm(po.OrderId, _empId, true);
                    previewForm.ShowDialog();
                }
            }

            hoveredRowIndex = -1;
        }

        private void dtgvPurchaseOrderList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                StartHoverPreviewTimer(e.RowIndex);
            }
        }

        private void dtgvPurchaseOrderList_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            hoverTimer.Stop();
            hoveredRowIndex = -1;
        }

        private void ShowReport(PurchaseOrder po, List<MaterialDetail> materials)
        {
            var reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local
            };
            var rdlcPath = System.IO.Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory,
    "POreport.rdlc"
);
            reportViewer.LocalReport.ReportPath = rdlcPath;

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(
                new ReportDataSource("PurchaseOrderDataSet", new List<PurchaseOrder> { po }));
            reportViewer.LocalReport.DataSources.Add(
                new ReportDataSource("MaterialDetailDataSet", materials));

            reportViewer.RefreshReport();
        }

        private void btnPrintOrder_Click(object sender, EventArgs e)
        {
            if (dtgvPurchaseOrderList.CurrentRow == null)
            {
                MessageBox.Show("กรุณาเลือกใบสั่งซื้อก่อน", "แจ้งเตือน",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var po = dtgvPurchaseOrderList.CurrentRow.DataBoundItem as PurchaseOrder;
            if (po == null) return;

            var dal = new PurchaseOrderDAL();
            var materials = dal.GetMaterialDetailsByOrderId(po.OrderId);

            //var reportForm = new POReportForm(po, materials);
            //reportForm.ShowDialog();
        }
    }
}
