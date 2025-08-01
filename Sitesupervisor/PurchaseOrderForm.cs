using JRSApplication.Components;
using JRSApplication.Components.Models;
using JRSApplication.Data_Access_Layer;
using Mysqlx.Crud;
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
    public partial class PurchaseOrderForm : UserControl
    {
        private readonly string _empId; // 👈 เก็บ empId จาก Form หลัก

        //ให้ DataGrid รู้จักการเปลี่ยนแปลงอัตโนมัติ
        private BindingList<MaterialDetail> materialList = new BindingList<MaterialDetail>(); // mat_list

        private MaterialDetail currentEditingMaterial = null; //เพิ่มตัวแปรและโหมดแก้ไข
        public PurchaseOrderForm(string empId)
        {
            InitializeComponent();
            _empId = empId; // 📌 เก็บไว้ใช้ภายใน UserControl
            InitializePOGridColumns();
            CustomizePOGridStyling();
            InitializeGridColumns();            // 🧱 สร้างคอลัมน์
            CustomizeMaterialGridStyling();
            dtgvPurchaseOrderList.Columns.Clear();  // ✅ ปลอดภัยที่นี่


            dtgvMaterialList.DataSource = materialList;
            dtgvMaterialList.ClearSelection(); // ❌ ไม่ให้เลือกแถวทันที

            LoadAllPurchaseOrders();
        }

        private void InitializeGridColumns()
        {
            dtgvMaterialList.AutoGenerateColumns = false;
            dtgvMaterialList.Columns.Clear(); // เผื่อกรณีรีโหลดหลายรอบ

            // ลำดับ
            dtgvMaterialList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatNo",
                HeaderText = "ลำดับ",
                Width = 60,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            // ชื่อวัสดุ
            dtgvMaterialList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatDetail",
                HeaderText = "ชื่อวัสดุ",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // ราคาต่อหน่วย
            dtgvMaterialList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatPrice",
                HeaderText = "ราคาต่อหน่วย",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N2"
                }
            });

            // จำนวน
            dtgvMaterialList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatQuantity",
                HeaderText = "จำนวน",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            // หน่วย
            dtgvMaterialList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatUnit",
                HeaderText = "หน่วย",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            // ราคารวม
            dtgvMaterialList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatAmount",
                HeaderText = "ราคารวม",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N2"
                }
            });
        }


        private void CustomizeMaterialGridStyling()
        {
            // ตั้งค่าเลือกทั้งแถว
            dtgvMaterialList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // ลบ border ด้านนอก
            dtgvMaterialList.BorderStyle = BorderStyle.None;

            // แถวสลับสีเทา
            dtgvMaterialList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            // เส้นขอบแนวนอนแบบเรียบ
            dtgvMaterialList.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // สีตอนถูกเลือก
            dtgvMaterialList.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvMaterialList.DefaultCellStyle.SelectionForeColor = Color.White;

            // หัวตาราง
            dtgvMaterialList.EnableHeadersVisualStyles = false;
            dtgvMaterialList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvMaterialList.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvMaterialList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvMaterialList.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvMaterialList.ColumnHeadersHeight = 32;

            // ฟอนต์ข้อมูลในตาราง
            dtgvMaterialList.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvMaterialList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvMaterialList.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            // ฟอนต์หัวตาราง
            dtgvMaterialList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // ขนาดแถวและคอลัมน์
            dtgvMaterialList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvMaterialList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvMaterialList.RowTemplate.Height = 32;

            // สีเส้นตาราง และซ่อนหัวแถว
            dtgvMaterialList.GridColor = Color.LightGray;
            dtgvMaterialList.RowHeadersVisible = false;

            // ป้องกันการแก้ไขข้อมูล
            dtgvMaterialList.ReadOnly = true;

            // ไม่อนุญาตให้เพิ่มแถวเอง
            dtgvMaterialList.AllowUserToAddRows = false;
            dtgvMaterialList.AllowUserToResizeRows = false;

            // อนุญาต resize คอลัมน์ด้วยมือ
            dtgvMaterialList.AllowUserToResizeColumns = true;
        }
        //Purchase Order validate
        private bool ValidatePO()
        {
            // ✅ ตรวจสอบเลขที่ใบสั่งซื้อ
            if (string.IsNullOrWhiteSpace(txtOrderNO.Text))
            {
                MessageBox.Show("กรุณากรอกเลขที่ใบสั่งซื้อ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOrderNO.Focus();
                return false;
            }

            // ✅ ตรวจสอบรายละเอียดใบสั่งซื้อ
            if (string.IsNullOrWhiteSpace(txtOrderDetail.Text))
            {
                MessageBox.Show("กรุณากรอกรายละเอียดใบสั่งซื้อ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOrderDetail.Focus();
                return false;
            }

            // ✅ ตรวจสอบกำหนดส่งกลับ
            if (cmbDueDate.SelectedItem == null)
            {
                MessageBox.Show("กรุณาเลือกกำหนดส่งกลับ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDueDate.Focus();
                return false;
            }

            // ✅ ตรวจสอบผู้ใช้งานที่ล็อกอิน
            //if (string.IsNullOrWhiteSpace(loggedInUserEmpId))
            //{
            //    MessageBox.Show("ไม่พบข้อมูลผู้ใช้งาน กรุณาล็อกอินใหม่", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
            //}

            // ✅ ตรวจสอบว่ามีรายการวัสดุอย่างน้อย 1 รายการ
            if (materialList.Count == 0)
            {
                MessageBox.Show("กรุณาเพิ่มรายการวัสดุก่อสร้างหรือวัสดุไฟฟ้าอย่างน้อย 1 รายการ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProjectID.Text) || txtProjectID.Text == "0")
            {
                MessageBox.Show("กรุณาเลือกโครงการก่อนบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProjectID.Focus();
                return false;
            }

            return true; // 🟢 ทุกอย่างถูกต้อง
        }


        // material validate
        private bool ValidateMaterialData()
        {
            if (string.IsNullOrWhiteSpace(txtMaterialName.Text))
            {
                MessageBox.Show("กรุณากรอกชื่อวัสดุ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaterialName.Focus();
                return false;
            }

            if (!decimal.TryParse(txtQuantity.Text.Trim(), out decimal qty) || qty <= 0)
            {
                MessageBox.Show("กรุณาระบุจำนวนวัสดุให้ถูกต้อง (มากกว่า 0)", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }

            if (!decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal price) || price < 0)
            {
                MessageBox.Show("กรุณาระบุราคาต่อหน่วยให้ถูกต้อง (ต้องไม่ติดลบ)", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnitPrice.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUnit.Text))
            {
                MessageBox.Show("กรุณาระบุหน่วยของวัสดุ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnit.Focus();
                return false;
            }

            return true;
        }

        private void CalculateTotalPrice()
        {
            decimal qty = 0;
            decimal price = 0;

            // แปลงจำนวน
            if (!string.IsNullOrWhiteSpace(txtQuantity.Text) &&
                decimal.TryParse(txtQuantity.Text.Trim(), out decimal parsedQty))
            {
                qty = parsedQty;
            }

            // แปลงราคาต่อหน่วย
            if (!string.IsNullOrWhiteSpace(txtUnitPrice.Text) &&
                decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal parsedPrice))
            {
                price = parsedPrice;
            }

            // คำนวณราคารวม
            decimal total = qty * price;
            txtTotalPrice.Text = total.ToString("N2");
        }

        private void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }


        // รวม btn
        private void btnAddMaterial_Click(object sender, EventArgs e)
        {
            if (!ValidateMaterialData())
                return;

            // ✅ แปลงค่าจาก TextBox
            string matName = txtMaterialName.Text.Trim();
            string unit = txtUnit.Text.Trim();
            decimal qty = decimal.Parse(txtQuantity.Text.Trim());
            decimal price = decimal.Parse(txtUnitPrice.Text.Trim());
            decimal amount = qty * price;

            // ✅ สร้าง MaterialDetail ใหม่
            var material = new MaterialDetail
            {
                MatNo = materialList.Count + 1,
                MatDetail = matName,
                MatQuantity = qty,
                MatPrice = price,
                MatAmount = amount,
                MatUnit = unit
            };

            // ✅ เพิ่มเข้ารายการ
            materialList.Add(material);

            // ✅ แสดงผลใน DataGridView
            dtgvMaterialList.DataSource = null;
            dtgvMaterialList.DataSource = materialList;
            dtgvMaterialList.ClearSelection();

            // ✅ ล้างช่อง input
            ClearMaterialForm();

            // ✅ สรุปรวมยอดรายการ
            UpdateMaterialSummary();
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            // ✅ เปิดการใช้งานทุกส่วนของฟอร์ม
            EnableAllSections();

            // ✅ ล้างข้อมูลเดิมทั้งหมด
            ClearOrderForm();
            ClearMaterialForm();

            // ✅ เคลียร์รายการวัสดุใน List และ Grid
            materialList.Clear();
            dtgvMaterialList.DataSource = null;
            dtgvMaterialList.DataSource = materialList;
            dtgvMaterialList.ClearSelection();

            // ✅ เคลียร์สรุปยอด
            lblSummation.Text = "รวม 0 รายการ\nรวมยอดเงิน 0.00 บาท";

            // ✅ รีเซ็ตปุ่มและสถานะภายใน
            btnAddMaterial.Enabled = true;
            btnSaveOrder.Enabled = true;
            btnEditMaterial.Text = "แก้ไข";
            btnAddMaterial.Text = "เพิ่ม";
            currentEditingMaterial = null;

            // ✅ กำหนดค่า default เช่น วันที่
            dtpOrderDate.Value = DateTime.Today;
            cmbDueDate.SelectedIndex = -1;

            // ✅ [Option] ถ้าคุณใช้เลขใบสั่งอัตโนมัติ
            // txtOrderNO.Text = GenerateNextOrderNumber();
        }

        private void ClearMaterialForm()
        {
            txtMaterialName.Clear();
            txtQuantity.Clear();
            txtUnit.Clear();
            txtUnitPrice.Clear();
            txtTotalPrice.Clear();
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
            //txtOrderRemark.Clear();
            dtpOrderDate.Value = DateTime.Today;
            cmbDueDate.SelectedIndex = -1;
        }

        private void EnableAllSections()
        {
            // ฟอร์มใบสั่งซื้อ
            txtOrderNO.Enabled = true;
            txtOrderDetail.Enabled = true;
            //txtOrderRemark.Enabled = true;
            dtpOrderDate.Enabled = true;
            cmbDueDate.Enabled = true;

            // ฟอร์มวัสดุ
            txtMaterialName.Enabled = true;
            txtQuantity.Enabled = true;
            txtUnit.Enabled = true;
            txtUnitPrice.Enabled = true;
            txtTotalPrice.Enabled = true;

            btnAddMaterial.Enabled = true;
            btnEditMaterial.Enabled = true;
            dtgvMaterialList.Enabled = true;
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            if (!ValidatePO())
                return;

            try
            {
                var order = new JRSApplication.Components.Models.PurchaseOrder
                {
                    OrderNumber = txtOrderNO.Text.Trim(),
                    OrderDetail = txtOrderDetail.Text.Trim(),
                    OrderDate = dtpOrderDate.Value.Date,
                    OrderDueDate = CalculateDueDate(),
                    OrderStatus = PurchaseOrderStatus.Submitted,
                    ProId = int.Parse(txtProjectID.Text),
                    EmpId = _empId

                };

                // ✅ แนบวัสดุ
                order.MaterialDetails = materialList.ToList();

                var poDal = new PurchaseOrderDAL();
                int newOrderId = poDal.InsertFullPurchaseOrder(order);  // ✅ insert แบบ Transaction

                if (newOrderId <= 0)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกใบสั่งซื้อ", "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ ถ้า Success แล้วเคลียร์หน้าจอ
                MessageBox.Show("บันทึกใบสั่งซื้อเรียบร้อยแล้ว", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearOrderForm();
                ClearMaterialForm();
                materialList.Clear();
                dtgvMaterialList.DataSource = null;
                lblSummation.Text = "รวม 0 รายการ\nรวมยอดเงิน 0.00 บาท";
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoadAllPurchaseOrders();
        }



        private DateTime CalculateDueDate()
        {
            // ดึงค่าที่เลือกจาก ComboBox (ควรเป็น string ของตัวเลข)
            if (cmbDueDate.SelectedItem == null)
                return dtpOrderDate.Value; // ถ้ายังไม่เลือก → return วันที่เดิม

            // แปลงเป็นจำนวนวัน
            int offsetDays = int.Parse(cmbDueDate.SelectedItem.ToString());

            // บวกจากวันที่สั่งซื้อ
            DateTime dueDate = dtpOrderDate.Value.Date.AddDays(offsetDays);

            // ถ้าตรงกับวันอาทิตย์ → เลื่อนเป็นวันจันทร์
            if (dueDate.DayOfWeek == DayOfWeek.Sunday)
            {
                dueDate = dueDate.AddDays(1);
            }

            return dueDate;
        }
        private void CustomizePOGridStyling()
        {
            dtgvPurchaseOrderList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvPurchaseOrderList.BorderStyle = BorderStyle.None;

            // 🪄 แถวสลับสี
            dtgvPurchaseOrderList.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // 🖌️ เส้นกรอบเซลล์
            dtgvPurchaseOrderList.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // 🎨 สีเมื่อเลือกแถว
            dtgvPurchaseOrderList.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvPurchaseOrderList.DefaultCellStyle.SelectionForeColor = Color.White;

            // 🎯 หัวตาราง
            dtgvPurchaseOrderList.EnableHeadersVisualStyles = false;
            dtgvPurchaseOrderList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvPurchaseOrderList.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvPurchaseOrderList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvPurchaseOrderList.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvPurchaseOrderList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvPurchaseOrderList.ColumnHeadersHeight = 32;

            // 📄 เซลล์ทั่วไป
            dtgvPurchaseOrderList.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvPurchaseOrderList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvPurchaseOrderList.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            // 📏 ขนาดแถว
            dtgvPurchaseOrderList.RowTemplate.Height = 30;

            // ❌ ไม่ให้ resize
            dtgvPurchaseOrderList.AllowUserToResizeRows = false;
            dtgvPurchaseOrderList.AllowUserToAddRows = false;
            dtgvPurchaseOrderList.ReadOnly = true;

            // 🔲 ซ่อน Row Header
            dtgvPurchaseOrderList.RowHeadersVisible = false;

            // 🧲 ขนาดอัตโนมัติ
            dtgvPurchaseOrderList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvPurchaseOrderList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void InitializePOGridColumns()
        {
            dtgvPurchaseOrderList.AutoGenerateColumns = false;

            // 🔒 ป้องกันซ้ำ: ถ้ามี column แล้ว ไม่ต้อง Add ซ้ำ
            if (dtgvPurchaseOrderList.Columns.Count > 0)
                return;

            // ✅ OrderId (ซ่อนไว้)
            var colOrderId = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderId",
                HeaderText = "#",
                Width = 50,
                Visible = false
            };
            dtgvPurchaseOrderList.Columns.Add(colOrderId);

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
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" },
                Width = 100
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
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" },
                Width = 120
            });

            dtgvPurchaseOrderList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderStatus",
                HeaderText = "สถานะใบสั่งซื้อ",
                Width = 100
            });

            dtgvPurchaseOrderList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "EmpId",
                HeaderText = "ผู้สร้าง",
                Width = 100
            });

            dtgvPurchaseOrderList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ApprovedByEmpId",
                HeaderText = "ผู้อนุมัติ",
                Width = 100
            });

            dtgvPurchaseOrderList.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ApprovedDate",
                HeaderText = "วันที่อนุมัติ",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" },
                Width = 120
            });
        }

        //รอเรียกใช้งาน
        private void dtgvPurchaseOrderList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dtgvPurchaseOrderList.Columns[e.ColumnIndex].DataPropertyName == "ApprovedByEmpId")
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
    }
}
