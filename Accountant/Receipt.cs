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

                // 🧾 ข้อมูลการชำระเงิน
                txtInvNo.Text = row.Cells["inv_no"].Value?.ToString();
                txtEmpName.Text = row.Cells["emp_fullname"]?.Value?.ToString() ?? "";
                dtpPaidDate.Value = row.Cells["paid_date"].Value != DBNull.Value
                    ? Convert.ToDateTime(row.Cells["paid_date"].Value)
                    : DateTime.Now;
                string status = row.Cells["inv_status"]?.Value?.ToString() ?? "";


                // 👤 ข้อมูลลูกค้า
                txtCusName.Text = row.Cells["cus_fullname"]?.Value?.ToString();
                txtCusIDCard.Text = row.Cells["cus_id_card"].Value?.ToString();
                txtCusAddress.Text = row.Cells["cus_address"].Value?.ToString();

                // 🏗️ ข้อมูลโครงการ
                txtContractNo.Text = row.Cells["pro_id"].Value?.ToString();  // Contract No
                txtProName.Text = row.Cells["pro_name"].Value?.ToString();       // Project Name
                txtPhaseID.Text = row.Cells["phase_id"].Value?.ToString();       // Phase
            }
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


    }
}
