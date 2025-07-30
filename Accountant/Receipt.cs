using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using JRSApplication.Data_Access_Layer;

namespace JRSApplication.Accountant
{
    public partial class Receipt : UserControl
    {
        private InvoiceDAL invoiceDAL = new InvoiceDAL();
        private ReceiptDAL receiptDAL = new ReceiptDAL();
        private int currentInvId;


        public Receipt()
        {
            InitializeComponent();
            CustomizeDataGridView();
        }

        private void CustomizeDataGridView()
        {
            dtgvInvoice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvInvoice.MultiSelect = false;
            dtgvInvoice.BorderStyle = BorderStyle.None;
            dtgvInvoice.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvInvoice.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvInvoice.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvInvoice.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvInvoice.BackgroundColor = Color.White;

            dtgvInvoice.EnableHeadersVisualStyles = false;
            dtgvInvoice.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvInvoice.ColumnHeadersHeight = 30;

            dtgvInvoice.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvInvoice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvInvoice.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            dtgvInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvInvoice.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvInvoice.RowTemplate.Height = 30;
            dtgvInvoice.GridColor = Color.LightGray;
            dtgvInvoice.RowHeadersVisible = false;
            dtgvInvoice.ReadOnly = true;
            dtgvInvoice.AllowUserToAddRows = false;
            dtgvInvoice.AllowUserToResizeRows = false;
        }

        private void dtgvInvoice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvInvoice.Rows[e.RowIndex];

                int invId = Convert.ToInt32(row.Cells["inv_id"].Value);
                LoadInvoiceDetailForReceipt(invId);

                // 🧾 ข้อมูลการชำระเงิน
                txtInvNo.Text = row.Cells["inv_no"].Value?.ToString();
                txtEmpName.Text = row.Cells["emp_fullname"]?.Value?.ToString() ?? "";
                dtpPaidDate.Value = row.Cells["paid_date"].Value != DBNull.Value
                    ? Convert.ToDateTime(row.Cells["paid_date"].Value)
                    : DateTime.Now;
                textBox1.Text = row.Cells["inv_status"].Value?.ToString();


                // 👤 ข้อมูลลูกค้า
                txtCusName.Text = row.Cells["cus_fullname"]?.Value?.ToString();
                txtCusIDCard.Text = row.Cells["cus_id_card"].Value?.ToString();
                txtCusAddress.Text = row.Cells["cus_address"].Value?.ToString();

                // 🏗️ ข้อมูลโครงการ
                txtContractNo.Text = row.Cells["pro_id"].Value?.ToString();  // Contract No
                txtProName.Text = row.Cells["pro_name"].Value?.ToString();       // Project Name
                txtPhaseID.Text = row.Cells["phase_id"].Value?.ToString();       // Phase

                currentInvId = Convert.ToInt32(row.Cells["inv_id"].Value);

                SetupReceiptDetailGrid(); // call first to prepare columns
                
            }
        }
        private void LoadInvoiceDetailForReceipt(int invId)
        {
            SetupReceiptDetailGrid(); // Prepare the grid

            DataTable dt = InvoiceDAL.GetInvoiceDetail(invId); // Load detail from DB
            dtgvReceiptDetail.DataSource = dt;
        }

        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Project");  // use mode "Project"
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                string selectedProjectId = searchForm.SelectedID;  // this is the projectId you need
                LoadInvoiceDataByProject(selectedProjectId);       // send to your method
            }
        }
        private void LoadInvoiceDataByProject(string projectId)
        {
            DataTable dt = invoiceDAL.GetInvoicesByProjectId(projectId); // send projectId here
            dtgvInvoice.AutoGenerateColumns = true;
            dtgvInvoice.DataSource = dt;
        }
        private void SetupReceiptDetailGrid()
        {
            dtgvReceiptDetail.Columns.Clear();
            dtgvReceiptDetail.AutoGenerateColumns = false;
            dtgvReceiptDetail.RowHeadersVisible = false;
            dtgvReceiptDetail.AllowUserToAddRows = false;
            dtgvReceiptDetail.ReadOnly = true;
            dtgvReceiptDetail.BackgroundColor = Color.White;
            dtgvReceiptDetail.BorderStyle = BorderStyle.FixedSingle;
            dtgvReceiptDetail.GridColor = Color.LightGray;
            dtgvReceiptDetail.DefaultCellStyle.Font = new Font("Tahoma", 11);
            dtgvReceiptDetail.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            dtgvReceiptDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvReceiptDetail.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvReceiptDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dtgvReceiptDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "No",
                HeaderText = "No",
                Width = 50
            });

            dtgvReceiptDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "inv_detail",
                HeaderText = "รายละเอียด",
                DataPropertyName = "inv_detail",
                Width = 300,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });

            dtgvReceiptDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "inv_quantity",
                HeaderText = "จำนวน",
                DataPropertyName = "inv_quantity",
                Width = 80
            });

            dtgvReceiptDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "inv_price",
                HeaderText = "ราคา",
                DataPropertyName = "inv_price",
                Width = 100
            });

            dtgvReceiptDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "subtotal",
                HeaderText = "ราคารวม",
                Width = 120
            });

            dtgvReceiptDetail.CellFormatting += dtgvReceiptDetail_CellFormatting;
        }

        private void dtgvReceiptDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = dtgvReceiptDetail.Rows[e.RowIndex];

            if (dtgvReceiptDetail.Columns[e.ColumnIndex].Name == "subtotal")
            {
                if (row.Cells["inv_quantity"].Value != null && row.Cells["inv_price"].Value != null)
                {
                    decimal qty = Convert.ToDecimal(row.Cells["inv_quantity"].Value);
                    decimal price = Convert.ToDecimal(row.Cells["inv_price"].Value);
                    e.Value = (qty * price).ToString("N2");
                }
            }

            if (dtgvReceiptDetail.Columns[e.ColumnIndex].Name == "No")
            {
                e.Value = (e.RowIndex + 1).ToString();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string receiptNo = txtReceiptNo.Text.Trim();
            DateTime receiptDate = dtpReceiptDate.Value;
            string remark = txtReason.Text.Trim();

            if (string.IsNullOrEmpty(receiptNo))
            {
                MessageBox.Show("กรุณากรอกเลขที่ใบเสร็จรับเงิน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int result = receiptDAL.InsertReceipt(receiptNo, receiptDate, remark, currentInvId);

                if (result > 0)
                    MessageBox.Show("บันทึกใบเสร็จรับเงินสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("ไม่สามารถบันทึกข้อมูลได้", "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Duplicate entry"))
                {
                    MessageBox.Show("เลขที่ใบเสร็จรับเงินนี้มีอยู่แล้ว", "ข้อมูลซ้ำ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            // Create a form
            Form previewForm = new Form();
            previewForm.Text = "ใบเสร็จรับเงิน";
            previewForm.Size = new Size(850, 1100); // Set to desired paper size
            previewForm.StartPosition = FormStartPosition.CenterScreen;

            // Create the user control
            var receiptPrint = new ReceiptPrint();
            receiptPrint.Dock = DockStyle.Fill;

            // Add the user control to the form
            previewForm.Controls.Add(receiptPrint);

            // Show as modal
            previewForm.ShowDialog(); // Use .Show() for non-blocking
        }

    }
}
