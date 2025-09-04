using JRSApplication.Components.Models;
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

namespace JRSApplication.Components.Service
{
    public partial class POForm : Form
    {
        private int _orderId;
        private readonly string currentUserId; // ✅ เพิ่มตัวแปร field
        private List<MaterialDetail> materialList = new List<MaterialDetail>();
        public POForm(int orderId, string empId)
        {
            InitializeComponent();
            _orderId = orderId;
            currentUserId = empId;  // ✅ เก็บไว้ใช้อัปเดตฐานข้อมูล
            InitializeGridColumns();
            LoadOrderData(); // โหลดข้อมูลใบสั่งซื้อ
            LoadMaterialData(); // โหลดวัสดุตาม orderId
            CustomizeMaterialGridStyling();

        }

        private void InitializeGridColumns()
        {
            dtgvMaterial.AutoGenerateColumns = false;
            dtgvMaterial.Columns.Clear(); // เผื่อกรณีรีโหลดหลายรอบ

            // ลำดับ
            dtgvMaterial.Columns.Add(new DataGridViewTextBoxColumn
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
            dtgvMaterial.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatDetail",
                HeaderText = "ชื่อวัสดุ",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            // จำนวน
            dtgvMaterial.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatQuantity",
                HeaderText = "จำนวน",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            // หน่วย
            dtgvMaterial.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatUnit",
                HeaderText = "หน่วย",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            // ราคาต่อหน่วย
            dtgvMaterial.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatPrice",
                HeaderText = "ราคาต่อหน่วย",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N2"
                }
            });

            // ราคารวม
            dtgvMaterial.Columns.Add(new DataGridViewTextBoxColumn
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
            dtgvMaterial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // ลบ border ด้านนอก
            dtgvMaterial.BorderStyle = BorderStyle.None;

            // แถวสลับสีเทา
            dtgvMaterial.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            // เส้นขอบแนวนอนแบบเรียบ
            dtgvMaterial.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // สีตอนถูกเลือก
            dtgvMaterial.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvMaterial.DefaultCellStyle.SelectionForeColor = Color.White;

            // หัวตาราง
            dtgvMaterial.EnableHeadersVisualStyles = false;
            dtgvMaterial.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvMaterial.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvMaterial.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvMaterial.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvMaterial.ColumnHeadersHeight = 32;

            // ฟอนต์ข้อมูลในตาราง
            dtgvMaterial.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvMaterial.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvMaterial.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            // ฟอนต์หัวตาราง
            dtgvMaterial.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // ขนาดแถวและคอลัมน์
            dtgvMaterial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvMaterial.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvMaterial.RowTemplate.Height = 32;

            // สีเส้นตาราง และซ่อนหัวแถว
            dtgvMaterial.GridColor = Color.LightGray;
            dtgvMaterial.RowHeadersVisible = false;

            // ป้องกันการแก้ไขข้อมูล
            dtgvMaterial.ReadOnly = true;

            // ไม่อนุญาตให้เพิ่มแถวเอง
            dtgvMaterial.AllowUserToAddRows = false;
            dtgvMaterial.AllowUserToResizeRows = false;

            // อนุญาต resize คอลัมน์ด้วยมือ
            dtgvMaterial.AllowUserToResizeColumns = true;
        }
        //โหลดข้อมูมใบ PO มาแสดงที่ textbox ต่าง ๆ จากฐานข้อมูล

        private void LoadOrderData()
        {
            var dal = new PurchaseOrderDAL();
            var order = dal.GetPurchaseOrderById(_orderId); // ต้องมีฟังก์ชันนี้ใน DAL

            if (order != null)
            {
                txtPONumber.Text = order.OrderNumber;          // เลขที่ใบสั่งซื้อ
                txtProjectNumber.Text = order.ProId.ToString(); // หรือจะแสดง ProjectNumber จริงก็ได้ถ้ามี JOIN
                // ผู้สร้างใบสั่งซื้อ (โชว์ชื่อ ถ้าไม่มีค่อยโชว์รหัส)
                txtEmpName.Text = !string.IsNullOrWhiteSpace(order.EmpName)
                    ? order.EmpName
                    : (order.EmpId ?? "-");

                // ✅ ผู้อนุมัติ (แสดงชื่อ; ถ้ายังไม่อนุมัติให้โชว์ "รออนุมัติ")
                txtEmpApprove.Text = !string.IsNullOrWhiteSpace(order.ApprovedByName)
                    ? order.ApprovedByName
                    : "รออนุมัติ";

                txtDate.Text = order.OrderDate.ToString("dd/MM/yyyy"); // วันที่ใบสั่งซื้อ
                txtApproveDate.Text = order.ApprovedDate.HasValue 
                    ? order.ApprovedDate.Value.ToString("dd/MM/yyyy") 
                    : "ยังไม่อนุมัติ"; // วันที่อนุมัติ ถ้าไม่มีให้แสดงข้อความว่า "ยังไม่อนุมัติ"
                txtDetail.Text = order.OrderDetail; // รายละเอียดใบสั่งซื้อ
               
            }
        }
        private void LoadMaterialData()
        {
            MaterialDetailDAL dal = new MaterialDetailDAL();
            materialList = dal.LoadMaterialData(_orderId);  // โหลดจาก DAL

            dtgvMaterial.DataSource = materialList;         // แสดงผล
            UpdateMaterialSummary();                        // อัปเดตยอดรวม
        }

        private void UpdateMaterialSummary()
        {
            int count = materialList.Count;
            decimal total = materialList.Sum(m => m.MatAmount);

            lblSummary.Text = $"รวม {count} รายการ\nรวมยอดเงิน {total:N2} บาท";
        }
        private void UpdateOrderStatus(string status, string remark)
        {
            var dal = new PurchaseOrderDAL();
            dal.UpdateOrderStatus(_orderId, status, remark, currentUserId);

            MessageBox.Show("บันทึกข้อมูลสำเร็จแล้ว", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private string PromptForRemark(string title, string label)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.White
            };

            Label lblText = new Label()
            {
                Left = 20,
                Top = 20,
                Width = 440,
                Text = label,
                Font = new Font("Segoe UI", 12)
            };

            TextBox txtInput = new TextBox()
            {
                Left = 20,
                Top = 60,
                Width = 440,
                Height = 40,
                Font = new Font("Segoe UI", 12)
            };

            Button btnOk = new Button()
            {
                Text = "ตกลง",
                Left = 250,
                Width = 100,
                Height = 40,
                Top = 120,
                DialogResult = DialogResult.OK,
                Font = new Font("Segoe UI", 12)
            };

            Button btnCancel = new Button()
            {
                Text = "ยกเลิก",
                Left = 360,
                Width = 100,
                Height = 40, 
                Top = 120,
                DialogResult = DialogResult.Cancel,
                Font = new Font("Segoe UI", 12)
            };

            prompt.Controls.Add(lblText);
            prompt.Controls.Add(txtInput);
            prompt.Controls.Add(btnOk);
            prompt.Controls.Add(btnCancel);
            prompt.AcceptButton = btnOk;
            prompt.CancelButton = btnCancel;

            return prompt.ShowDialog() == DialogResult.OK ? txtInput.Text : null;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string remark = txtRemark.Text.Trim();

            if (radioApproved.Checked)
            {
                DialogResult result = MessageBox.Show(
                    "คุณยืนยันที่จะอนุมัติใบสั่งซื้อนี้หรือไม่?",
                    "ยืนยันการอนุมัติ",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    UpdateOrderStatus("อนุมัติ", remark);
                }
            }
            else if (radioRejected.Checked)
            {
                DialogResult result = MessageBox.Show(
                    "คุณแน่ใจหรือไม่ที่จะ 'ไม่อนุมัติ' ใบสั่งซื้อนี้?",
                    "ยืนยันไม่อนุมัติ",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    UpdateOrderStatus("ไม่อนุมัติ", remark);
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกสถานะการอนุมัติใบสั่งซื้อก่อนบันทึก",
                                "แจ้งเตือน",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }


        //private void btnRejected_Click(object sender, EventArgs e)
        //{
        //    // รับ remark ได้ทั้งกรอกหรือไม่กรอก
        //    string remark = txtRemark.Text.Trim();

        //    DialogResult result = MessageBox.Show(
        //        "คุณแน่ใจหรือไม่ที่จะ 'ไม่อนุมัติ' ใบสั่งซื้อนี้?",
        //        "ยืนยันไม่อนุมัติ",
        //        MessageBoxButtons.YesNo,
        //        MessageBoxIcon.Warning);

        //    if (result == DialogResult.Yes)
        //    {
        //        UpdateOrderStatus("rejected", remark);
        //    }
        //}
    }
}
