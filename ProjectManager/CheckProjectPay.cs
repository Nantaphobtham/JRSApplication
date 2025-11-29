using JRSApplication.Components;
using JRSApplication.Data_Access_Layer;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
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

            // ================== ตั้งค่า SearchboxControl ให้เป็น Projectmanager ==================
            try
            {
                // role หน้านี้คือ Projectmanager
                searchboxControl1.DefaultRole = "Projectmanager";
                searchboxControl1.DefaultFunction = "ตรวจสอบข้อมูลการเงินโครงการ";

                searchboxControl1.SetRoleAndFunction("Projectmanager", "ตรวจสอบข้อมูลการเงินโครงการ");

                // ผูก event → ฟิลเตอร์ dtgvInvoice
                searchboxControl1.SearchTriggered += SearchboxInvoice_SearchTriggered;
            }
            catch
            {
                // กัน error ตอนเปิดใน Designer
            }
        }

        // ================== Searchbox → Filter dtgvInvoice ==================
        private void SearchboxInvoice_SearchTriggered(object sender, SearchEventArgs e)
        {
            ApplyInvoiceGridFilter(e.SearchBy, e.Keyword);
        }

        private void ApplyInvoiceGridFilter(string searchBy, string keyword)
        {
            if (!(dtgvInvoice.DataSource is DataTable table))
                return;

            string q = (keyword ?? "").Trim();

            if (string.IsNullOrEmpty(q))
            {
                table.DefaultView.RowFilter = string.Empty;
                return;
            }

            q = EscapeLikeValue(q);
            string filter = "";

            switch (searchBy)
            {
                case "รหัสใบแจ้งหนี้":
                    if (table.Columns.Contains("inv_id"))
                        filter = $"CONVERT(inv_id, 'System.String') LIKE '%{q}%'";
                    else if (table.Columns.Contains("inv_no"))
                        filter = $"CONVERT(inv_no, 'System.String') LIKE '%{q}%'";
                    break;

                case "ยอดชำระ":
                    if (table.Columns.Contains("inv_grand_total"))
                        filter = $"CONVERT(inv_grand_total, 'System.String') LIKE '%{q}%'";
                    else if (table.Columns.Contains("inv_total_amount"))
                        filter = $"CONVERT(inv_total_amount, 'System.String') LIKE '%{q}%'";
                    break;

                case "สถานะ":
                    if (table.Columns.Contains("inv_status"))
                        filter = $"CONVERT(inv_status, 'System.String') LIKE '%{q}%'";
                    break;

                default:
                    // fallback: ค้นทุกคอลัมน์
                    var sb = new StringBuilder();
                    foreach (DataColumn col in table.Columns)
                    {
                        if (sb.Length > 0) sb.Append(" OR ");
                        sb.Append($"CONVERT([{col.ColumnName}], 'System.String') LIKE '%{q}%'");
                    }
                    filter = sb.ToString();
                    break;
            }

            table.DefaultView.RowFilter = filter;
        }

        private static string EscapeLikeValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            // escape ตัวอักษรพิเศษของ RowFilter: [, ], %, *
            return value
                .Replace("[", "[[]")
                .Replace("]", "[]]")
                .Replace("%", "[%]")
                .Replace("*", "[*]")
                .Replace("'", "''");
        }

        // ================== ปุ่มค้นหาโครงการ ==================
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

            // ensure we have phase_no; if not, derive it from phase_id
            if (!dt.Columns.Contains("phase_no") && dt.Columns.Contains("phase_id"))
            {
                dt.Columns.Add("phase_no", typeof(string));
                var dal = new InvoiceDAL();

                foreach (DataRow r in dt.Rows)
                {
                    if (r["phase_id"] != DBNull.Value)
                    {
                        if (int.TryParse(Convert.ToString(r["phase_id"]), out int pid))
                        {
                            r["phase_no"] = dal.GetPhaseNoById(pid);
                        }
                    }
                }
            }

            dtgvInvoice.DataSource = dt;
            RenameInvoiceGridHeaders();
            CustomizeInvoiceGridStyle();

            // หลังโหลดข้อมูลให้ฟิลเตอร์ตามค่าปัจจุบันใน Searchbox
            ApplyInvoiceGridFilter(searchboxControl1.SelectedSearchBy, searchboxControl1.Keyword);
        }

        // ================== ปุ่มค้นหาใบชำระ (ด้วย SearchForm Invoice เดิม) ==================
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

                // ลูกค้า
                CustomerDAL customerDAL = new CustomerDAL();
                var customer = customerDAL.GetCustomerById(cusId);
                if (customer != null)
                {
                    txtCustomer.Text = $"{customer.FirstName} {customer.LastName}";
                    txtIDCard.Text = customer.IDCard;
                    txtAddress.Text = customer.Address;
                }

                // โครงการ
                ProjectDAL projectDAL = new ProjectDAL();
                var project = projectDAL.GetProjectDetailsById(Convert.ToInt32(proId));
                if (project != null)
                {
                    txtContractNumber.Text = project.ProjectNumber;
                    txtProjectName2.Text = project.ProjectName;
                    txtPhase.Text = project.CurrentPhaseNumber.ToString();
                }

                // ชำระเงิน
                txtInvoiceNo.Text = invNo;
                txtEmpName.Text = new EmployeeDAL().GetEmployeeFullNameById(empId);
                txtPaymentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtPaymentMethod.Text = paymentMethod;
            }
        }

        // ================== คลิกแถวใน dtgvInvoice ==================
        private void dtgvInvoice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // ไม่ใช่ Header row
            {
                DataGridViewRow row = dtgvInvoice.Rows[e.RowIndex];

                // ลูกค้า
                txtCustomer.Text = row.Cells["cus_name"].Value?.ToString();
                txtIDCard.Text = row.Cells["cus_id_card"].Value?.ToString();
                txtAddress.Text = row.Cells["cus_address"].Value?.ToString();

                // โครงการ
                txtContractNumber.Text = row.Cells["pro_number"].Value?.ToString();
                txtProjectName2.Text = row.Cells["pro_name"].Value?.ToString();
                txtPhase.Text = row.Cells["phase_no"].Value?.ToString();

                // การชำระเงิน
                txtInvoiceNo.Text = row.Cells["inv_id"].Value?.ToString();
                txtPaymentMethod.Text = row.Cells["inv_method"].Value?.ToString();

                if (row.Cells["paid_date"].Value != DBNull.Value)
                {
                    DateTime paidDate = Convert.ToDateTime(row.Cells["paid_date"].Value);
                    txtPaymentDate.Text = paidDate.ToString("dd/MM/yyyy HH:mm");
                }

                string empName = row.Cells["emp_name"].Value?.ToString();
                string empLname = row.Cells["emp_lname"].Value?.ToString();
                txtEmpName.Text = $"{empName} {empLname}";

                // โหลดรูปหลักฐาน
                string invId = row.Cells["inv_id"]?.Value?.ToString();
                if (!string.IsNullOrWhiteSpace(invId))
                {
                    LoadPaymentProofImage(invId);
                }
                else
                {
                    pictureBoxProof.Image = null;
                }
            }
        }

        private void LoadPaymentProofImage(string invId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                const string query = "SELECT file_data FROM payment_proof WHERE inv_id = @invId LIMIT 1";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@invId", invId ?? (object)DBNull.Value);

                    var result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        var imageBytes = (byte[])result;
                        using (var ms = new MemoryStream(imageBytes))
                        {
                            pictureBoxProof.Image = Image.FromStream(ms);
                            pictureBoxProof.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                    }
                    else
                    {
                        pictureBoxProof.Image = null;
                        MessageBox.Show("ไม่พบหลักฐานการชำระเงิน", "ไม่มีไฟล์",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        // เปลี่ยนชื่อหัวตารางให้เป็นไทย
        private void RenameInvoiceGridHeaders()
        {
            if (dtgvInvoice.Columns.Contains("inv_id")) dtgvInvoice.Columns["inv_id"].HeaderText = "รหัสงาน";
            if (dtgvInvoice.Columns.Contains("inv_date")) dtgvInvoice.Columns["inv_date"].HeaderText = "วันที่ออกใบแจ้งหนี้";
            if (dtgvInvoice.Columns.Contains("inv_duedate")) dtgvInvoice.Columns["inv_duedate"].HeaderText = "วันครบกำหนด";
            if (dtgvInvoice.Columns.Contains("inv_status")) dtgvInvoice.Columns["inv_status"].HeaderText = "สถานะใบแจ้งหนี้";
            if (dtgvInvoice.Columns.Contains("inv_method")) dtgvInvoice.Columns["inv_method"].HeaderText = "วิธีชำระเงิน";
            if (dtgvInvoice.Columns.Contains("paid_date")) dtgvInvoice.Columns["paid_date"].HeaderText = "วันที่บันทึก";

            if (dtgvInvoice.Columns.Contains("phase_no")) dtgvInvoice.Columns["phase_no"].HeaderText = "เฟสงาน";
            if (dtgvInvoice.Columns.Contains("phase_id")) dtgvInvoice.Columns["phase_id"].Visible = false;

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

        // ปรับสไตล์ DataGridView
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

            foreach (DataGridViewColumn col in dtgvInvoice.Columns)
            {
                col.Width = 200;
            }
        }
    }
}
