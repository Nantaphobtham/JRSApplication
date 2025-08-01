using JRSApplication.Components;
using JRSApplication.Data_Access_Layer;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace JRSApplication.ProjectManager
{
    public partial class CheckProjectPay : UserControl
    {
        private SearchService searchService = new SearchService();

        public CheckProjectPay()
        {
            InitializeComponent();
            dtgvInvoice.CellClick += dtgvInvoice_CellClick;

        }

        private void btnSearchProject_Click_1(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Project");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                txtProjectID.Text = searchForm.SelectedID;
                txtProjectname.Text = searchForm.SelectedName;
                txtCustomername.Text = searchForm.SelectedLastName;
                txtProjectManager.Text = searchForm.SelectedIDCardOrRole;

                LoadPaidInvoicesByProject(searchForm.SelectedID);
            }
        }

        private void LoadPaidInvoicesByProject(string projectId)
        {
            DataTable dt = searchService.GetPaidInvoicesByProject(projectId);
            dtgvInvoice.DataSource = dt;
            RenameInvoiceGridHeaders();
            CustomizeInvoiceGridStyle();
        }

        private void btnSearchPayment_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Invoice");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                string invNo = searchForm.SelectedID;
                string cusId = searchForm.SelectedName;
                string proId = searchForm.SelectedLastName;
                string empId = searchForm.SelectedIDCardOrRole;
                string paymentMethod = searchForm.SelectedPhone;

                // 🟢 ลูกค้า
                CustomerDAL customerDAL = new CustomerDAL();
                var customer = customerDAL.GetCustomerById(cusId);
                if (customer != null)
                {
                    txtCustomer.Text = $"{customer.FirstName} {customer.LastName}";
                    txtIDCard.Text = customer.IDCard;
                    txtAddress.Text = customer.Address;
                }

                // 🔵 โครงการ
                ProjectDAL projectDAL = new ProjectDAL();
                var project = projectDAL.GetProjectDetailsById(Convert.ToInt32(proId));
                if (project != null)
                {
                    txtContractNumber.Text = project.ProjectNumber;
                    txtProjectName2.Text = project.ProjectName;
                    txtPhase.Text = project.CurrentPhaseNumber.ToString();
                }

                // 🟡 ชำระเงิน
                txtInvoiceNo.Text = invNo;
                txtEmpName.Text = new EmployeeDAL().GetEmployeeFullNameById(empId);
                txtPaymentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtPaymentMethod.Text = paymentMethod;
            }
        }

        private void dtgvInvoice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvInvoice.Rows[e.RowIndex];

                txtInvoiceNo.Text = row.Cells["inv_no"].Value?.ToString();
                txtPaymentMethod.Text = row.Cells["inv_method"].Value?.ToString();

                if (row.Cells["paid_date"].Value != DBNull.Value)
                {
                    DateTime paidDate = Convert.ToDateTime(row.Cells["paid_date"].Value);
                    txtPaymentDate.Text = paidDate.ToString("yyyy-MM-dd");
                }

                txtCustomer.Text = row.Cells["cus_name"].Value?.ToString();
                txtIDCard.Text = row.Cells["cus_id_card"].Value?.ToString();
                txtAddress.Text = row.Cells["cus_address"].Value?.ToString();

                txtContractNumber.Text = row.Cells["pro_number"].Value?.ToString();
                txtProjectName2.Text = row.Cells["pro_name"].Value?.ToString();

                string empName = row.Cells["emp_name"].Value?.ToString();
                string empLname = row.Cells["emp_lname"].Value?.ToString();
                txtEmpName.Text = $"{empName} {empLname}";

                txtPhase.Text = row.Cells["phase_id"].Value?.ToString();

                int invId = Convert.ToInt32(row.Cells["inv_id"].Value);
                LoadPaymentProofImage(invId);
            }
        }

        private void LoadPaymentProofImage(int invId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT file_data FROM payment_proof WHERE inv_id = @invId LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@invId", invId);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        byte[] imageBytes = (byte[])result;
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            pictureBoxProof.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        pictureBoxProof.Image = null;
                        MessageBox.Show("ไม่พบหลักฐานการชำระเงิน", "ไม่มีไฟล์", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        // ✅ เปลี่ยนชื่อหัวตารางให้เป็นไทย
        private void RenameInvoiceGridHeaders()
        {
            
            if (dtgvInvoice.Columns.Contains("inv_id")) dtgvInvoice.Columns["inv_id"].HeaderText = "รหัสงาน";
            if (dtgvInvoice.Columns.Contains("inv_no")) dtgvInvoice.Columns["inv_no"].HeaderText = "เลขที่ใบแจ้งหนี้";
            if (dtgvInvoice.Columns.Contains("inv_date")) dtgvInvoice.Columns["inv_date"].HeaderText = "วันที่ออกใบแจ้งหนี้";
            if (dtgvInvoice.Columns.Contains("inv_duedate")) dtgvInvoice.Columns["inv_duedate"].HeaderText = "วันครบกำหนด";
            if (dtgvInvoice.Columns.Contains("inv_status")) dtgvInvoice.Columns["inv_status"].HeaderText = "สถานะใบแจ้งหนี้";
            if (dtgvInvoice.Columns.Contains("inv_method")) dtgvInvoice.Columns["inv_method"].HeaderText = "วิธีชำระเงิน";
            if (dtgvInvoice.Columns.Contains("paid_date")) dtgvInvoice.Columns["paid_date"].HeaderText = "วันที่บันทึก";

            
            if (dtgvInvoice.Columns.Contains("phase_id")) dtgvInvoice.Columns["phase_id"].HeaderText = "เฟสงาน";
            if (dtgvInvoice.Columns.Contains("cus_name")) dtgvInvoice.Columns["cus_name"].HeaderText = "ชื่อลูกค้า";
            if (dtgvInvoice.Columns.Contains("cus_id_card")) dtgvInvoice.Columns["cus_id_card"].HeaderText = "เลขบัตรประชาชน";
            if (dtgvInvoice.Columns.Contains("cus_address")) dtgvInvoice.Columns["cus_address"].HeaderText = "ที่อยู่";
            if (dtgvInvoice.Columns.Contains("pro_name")) dtgvInvoice.Columns["pro_name"].HeaderText = "ชื่อโครงการ";
            if (dtgvInvoice.Columns.Contains("pro_number")) dtgvInvoice.Columns["pro_number"].HeaderText = "เลขที่สัญญา";
            if (dtgvInvoice.Columns.Contains("emp_name")) dtgvInvoice.Columns["emp_name"].HeaderText = "ชื่อพนักงาน";

            
            if (dtgvInvoice.Columns.Contains("cus_id")) dtgvInvoice.Columns["cus_id"].HeaderText = "รหัสลูกค้า";
            if (dtgvInvoice.Columns.Contains("pro_id")) dtgvInvoice.Columns["pro_id"].HeaderText = "รหัสโครงการ";
            if (dtgvInvoice.Columns.Contains("emp_id")) dtgvInvoice.Columns["emp_id"].HeaderText = "รหัสพนักงาน";
            if (dtgvInvoice.Columns.Contains("emp_lname")) dtgvInvoice.Columns["emp_lname"].HeaderText = "นามสกุลพนักงาน";
        }


        // ✅ ปรับสไตล์ DataGridView
        private void CustomizeInvoiceGridStyle()
        {
            dtgvInvoice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvInvoice.MultiSelect = false;
            dtgvInvoice.BorderStyle = BorderStyle.None;
            dtgvInvoice.BackgroundColor = Color.White;

            dtgvInvoice.DefaultCellStyle.BackColor = Color.White;
            dtgvInvoice.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(217, 217, 217);
            dtgvInvoice.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvInvoice.GridColor = Color.LightGray;

            dtgvInvoice.EnableHeadersVisualStyles = false;
            dtgvInvoice.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvInvoice.ColumnHeadersHeight = 35;
            dtgvInvoice.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtgvInvoice.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvInvoice.DefaultCellStyle.ForeColor = Color.Black;
            dtgvInvoice.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvInvoice.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvInvoice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvInvoice.DefaultCellStyle.Padding = new Padding(2);

            dtgvInvoice.RowTemplate.Height = 30;
            dtgvInvoice.RowHeadersVisible = false;
            dtgvInvoice.ReadOnly = true;
            dtgvInvoice.AllowUserToAddRows = false;
            dtgvInvoice.AllowUserToResizeRows = false;

            dtgvInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dtgvInvoice.ScrollBars = ScrollBars.Both;

            // กำหนดความกว้างแต่ละคอลัมน์
            foreach (DataGridViewColumn col in dtgvInvoice.Columns)
            {
                col.Width = 200;
            }
        }

        private void dtgvInvoice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // ไม่ใช่ Header row
            {
                DataGridViewRow row = dtgvInvoice.Rows[e.RowIndex];

                // ✅ กรอกข้อมูลลูกค้า
                txtCustomer.Text = row.Cells["cus_name"].Value?.ToString();
                txtIDCard.Text = row.Cells["cus_id_card"].Value?.ToString();
                txtAddress.Text = row.Cells["cus_address"].Value?.ToString();

                // ✅ กรอกข้อมูลโครงการ
                txtContractNumber.Text = row.Cells["pro_number"].Value?.ToString();
                txtProjectName2.Text = row.Cells["pro_name"].Value?.ToString();
                txtPhase.Text = row.Cells["phase_id"].Value?.ToString();

                // ✅ กรอกข้อมูลการชำระเงิน
                txtInvoiceNo.Text = row.Cells["inv_no"].Value?.ToString();
                txtPaymentMethod.Text = row.Cells["inv_method"].Value?.ToString();

                if (row.Cells["paid_date"].Value != DBNull.Value)
                {
                    DateTime paidDate = Convert.ToDateTime(row.Cells["paid_date"].Value);
                    txtPaymentDate.Text = paidDate.ToString("dd/MM/yyyy HH:mm");
                }

                string empName = row.Cells["emp_name"].Value?.ToString();
                string empLname = row.Cells["emp_lname"].Value?.ToString();
                txtEmpName.Text = $"{empName} {empLname}";

                // ✅ โหลดรูปภาพ
                int invId = Convert.ToInt32(row.Cells["inv_id"].Value);
                LoadPaymentProofImage(invId);
            }
        }


    }
}
