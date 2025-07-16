using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JRSApplication.Components;
using JRSApplication.Data_Access_Layer;

namespace JRSApplication
{
    public partial class CheckProjectPayments : UserControl
    {

        public CheckProjectPayments()
        {
            InitializeComponent();
            CustomizeDataGridViewInvoice();
            LoadPaidInvoices();
        }
        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            string keyword = txtProjectID.Text.Trim(); // หรือจะใช้ TextBox ชื่ออื่นก็ได้

            SearchService service = new SearchService();
            DataTable dt = service.SearchData("Project", keyword);

            if (dt.Rows.Count > 0)
            {
                DataRow projectRow = dt.Rows[0];

                txtProjectID.Text = projectRow["รหัสโครงการ"].ToString();
                txtProjectname.Text = projectRow["ชื่อโครงการ"].ToString();
                txtCustomername.Text = projectRow["ลูกค้า"].ToString();
                txtProjectManager.Text = projectRow["พนักงานดูแล"].ToString();
            }
            else
            {
                MessageBox.Show("ไม่พบข้อมูลโครงการ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSearchPayment_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Invoice");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
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
                    txtPhase.Text = project.CurrentPhaseNumber.ToString();
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



    }
}
