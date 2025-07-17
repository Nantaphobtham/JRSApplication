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
        public ConfirmInvoice()
        {
            InitializeComponent();
            CustomizeDataGridView();
            LoadInvoiceData();
            PopulatePaymentMethod();
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
                txtCustomerName.Text = row["cus_name"].ToString();
                txtCustomerIDCard.Text = row["cus_id_card"].ToString(); // <-- National ID card
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
            }
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
                         paid_date = @paid_date
                     WHERE inv_id = @inv_id";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@method", comboPaymentMethod.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@paid_date", dtpPaymentDate.Value);
                cmd.Parameters.AddWithValue("@inv_id", invId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dgvInvoices.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกใบแจ้งหนี้ก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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


    }
}



