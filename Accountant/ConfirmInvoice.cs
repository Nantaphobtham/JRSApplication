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
using JRSApplication.Data_Access_Layer;
using MySql.Data.MySqlClient;

namespace JRSApplication.Accountant
{
    public partial class ConfirmInvoice : UserControl
    {

        private string empId;
        private string fullName;
        private string role;
        public ConfirmInvoice(string fullName, string role, string empId)
        {
            InitializeComponent();
            CustomizeDataGridView();
            //LoadInvoiceData();
            PopulatePaymentMethod();
            this.empId = empId;
        }

        private void CustomizeDataGridView()
        {
            dgvInvoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInvoices.MultiSelect = false;
            dgvInvoices.BorderStyle = BorderStyle.None;
            dgvInvoices.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dgvInvoices.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvInvoices.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dgvInvoices.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvInvoices.BackgroundColor = Color.White;

            dgvInvoices.EnableHeadersVisualStyles = false;
            dgvInvoices.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvInvoices.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvInvoices.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvInvoices.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dgvInvoices.ColumnHeadersHeight = 30;

            dgvInvoices.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dgvInvoices.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInvoices.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInvoices.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            dgvInvoices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInvoices.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvInvoices.RowTemplate.Height = 30;
            dgvInvoices.GridColor = Color.LightGray;
            dgvInvoices.RowHeadersVisible = false;
            dgvInvoices.ReadOnly = true;
            dgvInvoices.AllowUserToAddRows = false;
            dgvInvoices.AllowUserToResizeRows = false;
        }

        private void LoadInvoiceData()
        {
            SearchService service = new SearchService();
            DataTable dt = service.GetAllInvoices(); // ✅ make sure this returns `inv_id`, `inv_no`, `inv_date`, `inv_duedate`, etc.
            dgvInvoices.DataSource = dt;

            // Optional: Customize column headers (safe way with null checks)
            if (dgvInvoices.Columns.Contains("inv_id")) dgvInvoices.Columns["inv_id"].HeaderText = "รหัสใบแจ้งหนี้";
            if (dgvInvoices.Columns.Contains("inv_no")) dgvInvoices.Columns["inv_no"].HeaderText = "เลขที่ใบแจ้งหนี้";
            if (dgvInvoices.Columns.Contains("inv_date")) dgvInvoices.Columns["inv_date"].HeaderText = "วันที่ออกใบแจ้งหนี้";
            if (dgvInvoices.Columns.Contains("inv_duedate")) dgvInvoices.Columns["inv_duedate"].HeaderText = "กำหนดชำระ";
            if (dgvInvoices.Columns.Contains("cus_id")) dgvInvoices.Columns["cus_id"].HeaderText = "รหัสลูกค้า";
            if (dgvInvoices.Columns.Contains("pro_id")) dgvInvoices.Columns["pro_id"].HeaderText = "รหัสโครงการ";
            if (dgvInvoices.Columns.Contains("phase_id")) dgvInvoices.Columns["phase_id"].HeaderText = "รหัสเฟส";
        }


        private void LoadCustomerDetails(string cusId)
        {
            SearchService service = new SearchService();
            DataTable dt = service.GetCustomerById(cusId);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                // ✅ รวมชื่อ + นามสกุล
                string fullName = $"{row["cus_name"]} {row["cus_lname"]}";

                txtCustomerName.Text = fullName;
                txtCustomerIDCard.Text = row["cus_id_card"].ToString();
                txtCustomerAddress.Text = row["cus_address"].ToString();
            }
        }



        private void LoadProjectDetails(string proId)
        {
            SearchService service = new SearchService();
            DataTable dt = service.GetProjectById(proId); // You need to implement this if not exists

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtProjectName.Text = row["pro_name"].ToString(); // Project name
            }
        }



        private void dgvInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvInvoices.Rows[e.RowIndex];

                string invNo = row.Cells["inv_no"].Value?.ToString() ?? "";
                string invDate = row.Cells["inv_date"].Value?.ToString() ?? "";
                string invDueDate = row.Cells["inv_duedate"].Value?.ToString() ?? "";
                string proId = row.Cells["pro_id"].Value?.ToString() ?? "";
                string cusId = row.Cells["cus_id"].Value?.ToString() ?? ""; // still needed for lookup

                // Fill invoice section
                txtInvoiceNumber.Text = invNo;
                dtpInvoiceDate.Value = Convert.ToDateTime(invDate);
                txtDueDate.Text = invDueDate;
                txtProjectID.Text = proId;

                // Fill project & customer details
                LoadProjectDetails(proId);
                LoadCustomerDetails(cusId);
                int invId = Convert.ToInt32(row.Cells["inv_id"].Value);
                LoadInvoiceDetails(invId);  // Show invoice detail in bottom-right

            }
        }

        private void LoadInvoiceDetails(int invId)
        {
            SetupInvoiceDetailGrid();

            DataTable dt = InvoiceDAL.GetInvoiceDetail(invId);

            decimal total = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (decimal.TryParse(row["inv_quantity"].ToString(), out decimal qty) &&
                    decimal.TryParse(row["inv_price"].ToString(), out decimal price))
                {
                    total += qty * price;
                }
            }

            // ✅ Add final "total row"
            DataRow totalRow = dt.NewRow();
            totalRow["inv_detail"] = "รวม";
            dt.Rows.Add(totalRow);

            dgvInvoiceDetails.DataSource = dt;

            // ✅ Pass total into Tag so we can format later
            dgvInvoiceDetails.Tag = total;
        }


        private void PopulatePaymentMethod()
        {
            comboPaymentMethod.Items.Clear();
            comboPaymentMethod.Items.Add("เงินสด");
            comboPaymentMethod.Items.Add("โอนผ่านธนาคาร");
            comboPaymentMethod.Items.Add("เช็ค");
            comboPaymentMethod.Items.Add("QR Code");
            comboPaymentMethod.SelectedIndex = 0; // Default to first
        }
        private void SaveProofOfPayment(int invoiceId, string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            byte[] fileData = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            string query = @"INSERT INTO payment_proof (inv_id, file_name, file_data)
                     VALUES (@inv_id, @file_name, @file_data)";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@inv_id", invoiceId);
                cmd.Parameters.AddWithValue("@file_name", fileName);
                cmd.Parameters.AddWithValue("@file_data", fileData);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateInvoice(int invId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            string query = @"UPDATE invoice 
                     SET inv_status = 'ชำระแล้ว', 
                         inv_method = @method, 
                         paid_date = @paid_date,
                         emp_id = @emp_id
                     WHERE inv_id = @inv_id";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@method", comboPaymentMethod.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@paid_date", dtpPaymentDate.Value);
                cmd.Parameters.AddWithValue("@emp_id", this.empId); // ✅ ใส่ empId ของผู้ที่ล็อกอิน
                cmd.Parameters.AddWithValue("@inv_id", invId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            using (var searchForm = new SearchForm("Project"))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    string selectedProjectId = searchForm.SelectedID;

                    SearchService service = new SearchService();
                    DataTable filtered = service.GetDraftInvoicesByProject(selectedProjectId);

                    dgvInvoices.DataSource = filtered;

                    // Optional: Customize columns again
                    if (dgvInvoices.Columns.Contains("inv_id")) dgvInvoices.Columns["inv_id"].HeaderText = "รหัสใบแจ้งหนี้";
                    if (dgvInvoices.Columns.Contains("inv_no")) dgvInvoices.Columns["inv_no"].HeaderText = "เลขที่ใบแจ้งหนี้";
                    if (dgvInvoices.Columns.Contains("inv_date")) dgvInvoices.Columns["inv_date"].HeaderText = "วันที่ออกใบแจ้งหนี้";
                    if (dgvInvoices.Columns.Contains("inv_duedate")) dgvInvoices.Columns["inv_duedate"].HeaderText = "กำหนดชำระ";

                    // Optional: auto-select first row
                    if (dgvInvoices.Rows.Count > 0)
                    {
                        dgvInvoices.Rows[0].Selected = true;
                        dgvInvoices_CellContentClick(dgvInvoices, new DataGridViewCellEventArgs(0, 0));
                    }
                }
            }
        }


        private void SetupInvoiceDetailGrid()
        {
            dgvInvoiceDetails.Columns.Clear();
            dgvInvoiceDetails.AutoGenerateColumns = false;
            dgvInvoiceDetails.RowHeadersVisible = false;
            dgvInvoiceDetails.AllowUserToAddRows = false;
            dgvInvoiceDetails.ReadOnly = true;
            dgvInvoiceDetails.BackgroundColor = Color.White;
            dgvInvoiceDetails.BorderStyle = BorderStyle.FixedSingle;
            dgvInvoiceDetails.GridColor = Color.LightGray;
            dgvInvoiceDetails.DefaultCellStyle.Font = new Font("Tahoma", 11);
            dgvInvoiceDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            dgvInvoiceDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInvoiceDetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInvoiceDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvInvoiceDetails.ScrollBars = ScrollBars.Both;

            dgvInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "No",
                HeaderText = "No",
                Width = 50
            });

            dgvInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "inv_detail",
                HeaderText = "รายละเอียด",
                DataPropertyName = "inv_detail",
                Width = 260,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });

            dgvInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "inv_quantity",
                HeaderText = "จำนวน",
                DataPropertyName = "inv_quantity",
                Width = 80
            });

            dgvInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "inv_price",
                HeaderText = "ราคา",
                DataPropertyName = "inv_price",
                Width = 80
            });

            dgvInvoiceDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "subtotal",
                HeaderText = "ราคารวม",
                Width = 120
            });

            dgvInvoiceDetails.CellFormatting += dgvInvoiceDetails_CellFormatting;
        }



        private void dgvInvoiceDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = dgvInvoiceDetails.Rows[e.RowIndex];

            // ✅ Detect last row: "รวม"
            if (row.Cells["inv_detail"].Value?.ToString() == "รวม")
            {
                if (dgvInvoiceDetails.Columns[e.ColumnIndex].Name == "subtotal")
                {
                    decimal total = dgvInvoiceDetails.Tag != null ? (decimal)dgvInvoiceDetails.Tag : 0;
                    e.Value = total.ToString("N2") + " บาท";
                    e.CellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (dgvInvoiceDetails.Columns[e.ColumnIndex].Name == "inv_detail")
                {
                    e.CellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    e.Value = ""; // Empty other cells
                }

                e.CellStyle.BackColor = Color.LightYellow;
                return;
            }

            // 🧮 Calculate Subtotal Normally
            if (dgvInvoiceDetails.Columns[e.ColumnIndex].Name == "subtotal")
            {
                if (row.Cells["inv_quantity"].Value != null && row.Cells["inv_price"].Value != null)
                {
                    decimal qty = Convert.ToDecimal(row.Cells["inv_quantity"].Value);
                    decimal price = Convert.ToDecimal(row.Cells["inv_price"].Value);
                    e.Value = (qty * price).ToString("N2");
                }
            }

            // ➕ Add "No" column auto numbering
            if (dgvInvoiceDetails.Columns[e.ColumnIndex].Name == "No")
            {
                e.Value = (e.RowIndex + 1).ToString();
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dgvInvoices.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกใบแจ้งหนี้ก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // ตรวจสอบช่องว่าง
            if (string.IsNullOrWhiteSpace(txtInvoiceNumber.Text) ||
                string.IsNullOrWhiteSpace(txtDueDate.Text) ||
                string.IsNullOrWhiteSpace(txtProjectID.Text) ||
                string.IsNullOrWhiteSpace(txtProjectName.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerIDCard.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerAddress.Text))
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนทำการบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // ตรวจสอบไฟล์แนบ
            if (string.IsNullOrWhiteSpace(txtFilePath.Text))
            {
                MessageBox.Show("กรุณาแนบไฟล์หลักฐานการชำระเงิน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ตรวจสอบการเลือกวิธีการชำระเงิน
            if (comboPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("กรุณาเลือกวิธีการชำระเงิน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int invId = Convert.ToInt32(dgvInvoices.SelectedRows[0].Cells["inv_id"].Value);
            string filePath = txtFilePath.Text; // Set by file dialog when uploading proof

            UpdateInvoice(invId);
            SaveProofOfPayment(invId, filePath);

            MessageBox.Show("บันทึกข้อมูลเรียบร้อยแล้ว", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadInvoiceData(); // Refresh grid
            dgvInvoices.ClearSelection();
        }
        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF Files|*.pdf|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = ofd.FileName;
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtInvoiceNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCustomerIDCard_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtProjectID_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtProjectName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCustomerAddress_TextChanged(object sender, EventArgs e)
        {

        }
    }
}



