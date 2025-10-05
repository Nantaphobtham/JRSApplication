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

namespace JRSApplication
{
    public partial class PurchaseOrderForm : UserControl
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
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

        public PurchaseOrderForm(string empId)
        {
            InitializeComponent();
            _empId = empId;

            // ✅ สร้าง column และตกแต่ง
            InitializePOGridColumns();
            InitializeMaterialGridColumns();

            CustomizeGridStyling(dtgvPurchaseOrderList);
            CustomizeGridStyling(dtgvMaterialList);

            dtgvMaterialList.DataSource = materialList;
            dtgvPurchaseOrderList.ClearSelection();
            dtgvMaterialList.ClearSelection();

            // ComboBox หน่วยนับ
            cmbUnit.DataSource = unitList;
            cmbUnit.SelectedIndex = 0;
            cmbUnit.AutoCompleteMode = AutoCompleteMode.None;
            cmbUnit.AutoCompleteSource = AutoCompleteSource.None;

            LoadAllPurchaseOrders();

            // Event
            dtgvPurchaseOrderList.CellDoubleClick += dtgvPurchaseOrderList_CellDoubleClick;
            dtgvPurchaseOrderList.CellFormatting += dtgvPurchaseOrderList_CellFormatting;
            dtgvMaterialList.CellClick += dtgvMaterialList_CellClick;
            txtQuantity.TextChanged += txtQuantity_TextChanged;
            txtUnitPrice.TextChanged += txtUnitPrice_TextChanged;
            cmbUnit.TextUpdate += txtUnit_TextUpdate;
            btnAddMaterial.Click += btnAddMaterial_Click;
            btnEditMaterial.Click += btnEditMaterial_Click;
            btnAddOrder.Click += btnAddOrder_Click;
            btnSaveOrder.Click += btnSaveOrder_Click;
            
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

        // ---------------- Event Handlers ----------------
        private void txtQuantity_TextChanged(object sender, EventArgs e) => CalculateTotalPrice();
        private void txtUnitPrice_TextChanged(object sender, EventArgs e) => CalculateTotalPrice();

        private void txtUnit_TextUpdate(object sender, EventArgs e)
        {
            string filterParam = cmbUnit.Text.Trim();
            var filtered = unitList.Skip(1).Where(x => x.Contains(filterParam)).ToList();
            filtered.Insert(0, "--เลือก--");
            if (filtered.Count == 1) filtered = new List<string>(unitList);
            cmbUnit.DataSource = null;
            cmbUnit.DataSource = filtered;
            cmbUnit.Text = filterParam;
            cmbUnit.SelectionStart = cmbUnit.Text.Length;
            cmbUnit.DroppedDown = true;
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
                    OrderStatus = "รอดำเนินการ",
                    ProId = int.Parse(txtProjectID.Text),
                    EmpId = _empId,
                    MaterialDetails = materialList.ToList()
                };

                var poDal = new PurchaseOrderDAL();
                int newOrderId = poDal.InsertFullPurchaseOrder(order);

                if (newOrderId <= 0)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกใบสั่งซื้อ", "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("บันทึกใบสั่งซื้อเรียบร้อยแล้ว", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearOrderForm();
                ClearMaterialForm();
                materialList.Clear();
                dtgvMaterialList.DataSource = null;
                lblSummation.Text = "รวม 0 รายการ\nรวมยอดเงิน 0.00 บาท";

                LoadAllPurchaseOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
        }

        private void dtgvMaterialList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dtgvMaterialList.Rows.Count)
            {
                DataGridViewRow row = dtgvMaterialList.Rows[e.RowIndex];
                txtMaterialName.Text = row.Cells["MatDetail"].Value?.ToString();
                txtUnitPrice.Text = row.Cells["MatPrice"].Value?.ToString();
                txtQuantity.Text = row.Cells["MatQuantity"].Value?.ToString();
                cmbUnit.Text = row.Cells["MatUnit"].Value?.ToString();

                CalculateTotalPrice();
                editingRowIndex = e.RowIndex;

                txtMaterialName.ReadOnly = true;
                txtUnitPrice.ReadOnly = true;
                txtQuantity.ReadOnly = true;
                cmbUnit.Enabled = false;

                btnAddMaterial.Text = "บันทึกแก้ไข";
                btnAddMaterial.BackColor = Color.Orange;
                btnEditMaterial.Enabled = true;
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

            txtOrderNO.Text = po.OrderNumber;
            txtOrderDetail.Text = po.OrderDetail;
            dtpOrderDate.Value = po.OrderDate == DateTime.MinValue ? DateTime.Today : po.OrderDate;

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

            txtMaterialName.ReadOnly = true;
            txtUnitPrice.ReadOnly = true;
            txtQuantity.ReadOnly = true;
            cmbUnit.Enabled = false;
            btnAddMaterial.Enabled = false;
            btnEditMaterial.Enabled = false;
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
            if (int.TryParse(cmbDueDate.SelectedItem.ToString(), out int days))
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
            dtgvPurchaseOrderList.DataSource = null;
            dtgvPurchaseOrderList.DataSource = orderList;
            dtgvPurchaseOrderList.ClearSelection();
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
        }

        private void LoadAllPurchaseOrders()
        {
            var dal = new PurchaseOrderDAL();
            var orderList = dal.GetAllPurchaseOrders();

            // 🔐 ป้องกันกรณีไม่มีข้อมูล
            if (orderList == null || orderList.Count == 0)
            {
                MessageBox.Show("ไม่พบข้อมูลใบสั่งซื้อ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtgvPurchaseOrderList.DataSource = null;
                return;
            }

            // 🧱 ใช้ BindingSource เพื่อความยืดหยุ่น
            var bindingSource = new BindingSource();
            bindingSource.DataSource = orderList;

            // 🧼 เคลียร์คอลัมน์เก่าหากมี (สำคัญมาก ถ้ามี reload หลายรอบ)
            dtgvPurchaseOrderList.Columns.Clear();
            InitializePOGridColumns(); // 🏗️ สร้างคอลัมน์ใหม่

            dtgvPurchaseOrderList.AutoGenerateColumns = false;
            dtgvPurchaseOrderList.DataSource = bindingSource;
            dtgvPurchaseOrderList.ClearSelection();
        }

        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            // เปิดฟอร์มค้นหาโครงการแบบ Dialog
            var searchForm = new SearchForm("Project");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                // ดึงค่าที่เลือกกลับมา
                txtProjectID.Text = searchForm.SelectedID;
                txtProjectNumber.Text = searchForm.SelectedContract; // หรือใช้ชื่อ property ที่คุณส่งกลับ
                txtProjectName.Text = searchForm.SelectedName;
            }
        }

        private void txtUnit_TextUpdate(object sender, EventArgs e)
        {
            string filterParam = cmbUnit.Text.Trim();

            // อย่า filter "--เลือก--"
            var filtered = unitList
                .Skip(1) // ข้าม index 0 ("--เลือก--")
                .Where(x => x.Contains(filterParam))
                .ToList();

            // เพิ่ม "--เลือก--" กลับไปที่ index 0
            filtered.Insert(0, "--เลือก--");

            // ถ้าไม่เหลือรายการเลย ให้แสดง "--เลือก--" กับทั้งหมด
            if (filtered.Count == 1)
                filtered = new List<string>(unitList);

            // suspend event เพื่อไม่ให้เกิด loop
            cmbUnit.DataSource = null;
            cmbUnit.DataSource = filtered;
            cmbUnit.Text = filterParam;
            cmbUnit.SelectionStart = cmbUnit.Text.Length;
            cmbUnit.DroppedDown = true;
        }

        private void dtgvMaterialList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dtgvMaterialList.Rows.Count)
            {
                // ดึงข้อมูลจาก DataGridViewRow ที่ถูกเลือก
                DataGridViewRow row = dtgvMaterialList.Rows[e.RowIndex];

                // สมมุติ ColumnNames ตาม code คุณ: MatDetail, MatPrice, MatQuantity, MatUnit
                txtMaterialName.Text = row.Cells["MatDetail"].Value?.ToString();
                txtUnitPrice.Text = row.Cells["MatPrice"].Value?.ToString();
                txtQuantity.Text = row.Cells["MatQuantity"].Value?.ToString();
                cmbUnit.Text = row.Cells["MatUnit"].Value?.ToString();

                // คำนวณราคาสุทธิใหม่ (option)
                CalculateTotalPrice();
                // 🟡 จำ index ไว้
                editingRowIndex = e.RowIndex;

                // เก็บ material ที่เลือกไว้สำหรับแก้ไข (option)
                // currentEditingMaterial = ... หาได้จาก BindingList ถ้ามี Primary Key หรือ index
                txtMaterialName.ReadOnly = true; // ป้องกันการแก้ไขโดยตรง
                txtUnitPrice.ReadOnly = true; // ป้องกันการแก้ไขโดยตรง
                txtQuantity.ReadOnly = true; // ป้องกันการแก้ไขโดยตรง
                cmbUnit.Enabled = false; // ป้องกันการเปลี่ยนแปลงหน่วย

                btnAddMaterial.Text = "บันทึกแก้ไข";
                btnAddMaterial.BackColor = Color.Orange; // สีแยก (option)
                btnEditMaterial.Enabled = true; // ให้ user ปลด ReadOnly ได้
            }
        }
        private int? editingRowIndex = null; // index ของแถวที่กำลังแก้ไข (null = เพิ่มใหม่)

        private void btnEditMaterial_Click(object sender, EventArgs e)
        {
            txtMaterialName.ReadOnly = false; // เปลี่ยนแปลงหน่วย
            txtUnitPrice.ReadOnly = false; // เปลี่ยนแปลงหน่วย
            txtQuantity.ReadOnly = false; // เปลี่ยนแปลงหน่วย
            cmbUnit.Enabled = true; // เปลี่ยนแปลงหน่วย
        }

        private void dtgvPurchaseOrderList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dtgvPurchaseOrderList.Rows[e.RowIndex];
            var po = row.DataBoundItem as JRSApplication.Components.Models.PurchaseOrder;
            if (po == null) return;

            ShowPurchaseOrderDetails(po);
        }


        private void ShowPurchaseOrderDetails(JRSApplication.Components.Models.PurchaseOrder po)
        {
            if (po == null) return;

            // เติมหัวฟอร์ม
            txtOrderNO.Text = po.OrderNumber;
            txtOrderDetail.Text = po.OrderDetail;
            dtpOrderDate.Value = po.OrderDate == DateTime.MinValue ? DateTime.Today : po.OrderDate;

            // ถ้า cmbDueDate ของคุณเก็บ “จำนวนวัน” อาจไม่ต้องเซ็ตตรง ๆ
            // ถ้าอยากโชว์วันที่กำหนดส่งกลับเป็น text ก็ทำได้ เช่น:
            // txtDueDateDisplay.Text = po.OrderDueDate.ToString("dd/MM/yyyy"); // ถ้ามี textbox สำหรับโชว์

            // โหลดรายการวัสดุตาม order id
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
        }

        private Timer hoverTimer = new Timer();
        private int hoveredRowIndex = -1;
        private void StartHoverPreviewTimer(int rowIndex)
        {
            hoveredRowIndex = rowIndex;

            hoverTimer.Interval = 1000; // วินาที
            hoverTimer.Tick -= HoverTimer_Tick; // ป้องกันซ้ำ
            hoverTimer.Tick += HoverTimer_Tick;
            hoverTimer.Start();
        }
        private void HoverTimer_Tick(object sender, EventArgs e)
        {
            hoverTimer.Stop();

            if (hoveredRowIndex >= 0 && hoveredRowIndex < dtgvPurchaseOrderList.Rows.Count)
            {
                var row = dtgvPurchaseOrderList.Rows[hoveredRowIndex];
                var po = row.DataBoundItem as JRSApplication.Components.Models.PurchaseOrder;

                if (po != null)
                {
                    // เรียก POForm แบบ preview
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

    }
}
