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
using JRSApplication.Components;
using JRSApplication.Data_Access_Layer;
using MySql.Data.MySqlClient;

namespace JRSApplication
{
    public partial class CheckProjectPayments : UserControl
    {
        private SearchService searchService = new SearchService();

        public CheckProjectPayments()
        {
            InitializeComponent();
            CustomizeDataGridViewInvoice();
            //LoadPaidInvoices();
        }
        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Project");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                txtProjectID.Text = searchForm.SelectedID;
                txtProjectname.Text = searchForm.SelectedName;
                txtCustomername.Text = searchForm.SelectedLastName;
                txtProjectManager.Text = searchForm.SelectedIDCardOrRole;

                // 🔹 Load invoice data for this project
                LoadPaidInvoicesByProject(searchForm.SelectedID);
            }
        }
        private void LoadPaidInvoicesByProject(string projectId)
        {
            DataTable dt = searchService.GetPaidInvoicesByProject(projectId);
            dtgvInvoice.DataSource = dt;
        }

        private void btnSearchPayment_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Invoice");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                InvoiceDAL dal = new InvoiceDAL();

                string phaseNo = dal.GetPhaseNoById(phaseId);
                // ดึงค่าที่เลือก
                string invNo = searchForm.SelectedID;
                string cusId = searchForm.SelectedName;
                string proId = searchForm.SelectedLastName;
                string empId = searchForm.SelectedIDCardOrRole;
                string paymentMethod = searchForm.SelectedPhone;
                string status = searchForm.SelectedEmail;

                // =========================
                // 🟢 โหลดข้อมูลลูกค้า
                // =========================
                CustomerDAL customerDAL = new CustomerDAL();
                var customer = customerDAL.GetCustomerById(cusId);
                if (customer != null)
                {
                    txtCustomer.Text = $"{customer.FirstName} {customer.LastName}";
                    txtIDCard.Text = customer.IDCard;
                    txtAddress.Text = customer.Address;
                }

                // =========================
                // 🔵 โหลดข้อมูลโครงการ
                // =========================
                ProjectDAL projectDAL = new ProjectDAL();
                var project = projectDAL.GetProjectDetailsById(Convert.ToInt32(proId));
                if (project != null)
                {
                    txtContractNumber.Text = project.ProjectNumber;
                    txtProjectName2.Text = project.ProjectName;
                    txtPhase.Text = phaseNo;
                }

                // =========================
                // 🟡 โหลดข้อมูลการชำระเงิน
                // =========================
                txtInvoiceNo.Text = invNo;
                txtEmpName.Text = new EmployeeDAL().GetEmployeeFullNameById(empId);
                txtPaymentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtPaymentMethod.Text = paymentMethod;
            }
        }

        private void CustomizeDataGridViewInvoice()
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
            dtgvInvoice.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            dtgvInvoice.ColumnHeadersHeight = 30;

            dtgvInvoice.DefaultCellStyle.Font = new Font("Segoe UI", 14);
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

        private void LoadPaidInvoices()
        {
            SearchService service = new SearchService();
            DataTable dt = service.GetPaidInvoices();
            dtgvInvoice.DataSource = dt;
        }

        private void dtgvInvoice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvInvoice.Rows[e.RowIndex];

                // 🔹 Invoice info
                txtInvoiceNo.Text = row.Cells["inv_no"].Value?.ToString();
                txtPaymentMethod.Text = row.Cells["inv_method"].Value?.ToString();

                // 🔹 Payment date
                if (row.Cells["paid_date"].Value != DBNull.Value)
                {
                    DateTime paidDate = Convert.ToDateTime(row.Cells["paid_date"].Value);
                    txtPaymentDate.Text = paidDate.ToString("yyyy-MM-dd");
                }

                // 🔹 Customer info
                txtCustomer.Text = row.Cells["cus_name"].Value?.ToString();
                txtIDCard.Text = row.Cells["cus_id_card"].Value?.ToString();
                txtAddress.Text = row.Cells["cus_address"].Value?.ToString();

                // 🔹 Project info
                txtContractNumber.Text = row.Cells["pro_number"].Value?.ToString();
                txtProjectName2.Text = row.Cells["pro_name"].Value?.ToString();

                // 🔹 Employee info
                string empName = row.Cells["emp_name"].Value?.ToString();
                string empLname = row.Cells["emp_lname"].Value?.ToString();
                txtEmpName.Text = $"{empName} {empLname}";

                // 🔹 Phase (optional)
                txtPhase.Text = row.Cells["phase_id"].Value?.ToString();

                // 🔹 Load Image
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
                        MessageBox.Show("No payment proof found for this invoice.", "No Image", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

    }
}
