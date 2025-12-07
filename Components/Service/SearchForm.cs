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
    public partial class SearchForm : Form
    {
        private SearchService searchService = new SearchService();
        public string SearchMode { get; set; }
        public string SelectedID { get; private set; } = "";
        public string SelectedName { get; private set; } = "";
        public string SelectedContract { get; private set; } = "";
        public string SelectedLastName { get; private set; } = "";
        public string SelectedIDCardOrRole { get; private set; } = "";
        public string SelectedPhone { get; private set; } = "";
        public string SelectedEmail { get; private set; } = "";
        public string SelectedCusID { get; private set; } = "";
        private string optionalProjectId = null;

        public SearchForm(string mode, string optionalProjectId = null)
        {
            InitializeComponent();
            this.SearchMode = mode;
            this.optionalProjectId = optionalProjectId;

            this.Load += SearchForm_Load;

            // Title
            if (SearchMode == "Customer")
                lblTitle.Text = "ค้นหาลูกค้า";
            else if (SearchMode == "Employee")
                lblTitle.Text = "ค้นหาพนักงาน";
            else if (SearchMode == "Supplier")
                lblTitle.Text = "ค้นหาซัพพลายเออร์";
            else if (SearchMode == "Project")
                lblTitle.Text = "ค้นหาโครงการ";
            else if (SearchMode == "Invoice")
                lblTitle.Text = "ค้นหาใบแจ้งหนี้";
            else if (SearchMode == "UnpaidInvoiceByProject")
                lblTitle.Text = "เลือกใบแจ้งหนี้ที่ยังไม่ชำระ";
            else if (SearchMode == "PaidInvoiceByProject") // New mode
                lblTitle.Text = "เลือกใบแจ้งหนี้ที่ชำระแล้ว";
            else
                lblTitle.Text = "ค้นหา";

            // โหลดข้อมูลเริ่มต้น
            if (SearchMode == "UnpaidInvoiceByProject" || SearchMode == "PaidInvoiceByProject")
                LoadSearchData(optionalProjectId ?? "");
            else
                LoadSearchData("");

            CustomizeDataGridViewAlldata();

            dtgvAlldata.CellDoubleClick += dtgvAlldata_CellDoubleClick;
        }

        private void LoadSearchData(string keywordOrProjectId = "")
        {
            dtgvAlldata.AutoGenerateColumns = true;

            if (SearchMode == "UnpaidInvoiceByProject" || SearchMode == "PaidInvoiceByProject")
            {
                dtgvAlldata.DataSource = searchService.SearchData(SearchMode, keywordOrProjectId);

                // *** ตั้งหัวคอลัมน์เป็นภาษาไทยสำหรับใบแจ้งหนี้ที่ยังไม่ชำระ
                SetupUnpaidInvoiceGridHeaders();
            }
            else
            {
                dtgvAlldata.DataSource = searchService.SearchData(SearchMode, "");
            }
        }

        // *** ฟังก์ชันตั้ง HeaderText ภาษาไทย (ใช้กับ inv_* จากฐานข้อมูล)
        private void SetupUnpaidInvoiceGridHeaders()
        {
            if (dtgvAlldata.Columns.Contains("inv_id"))
                dtgvAlldata.Columns["inv_id"].HeaderText = "เลขที่ใบแจ้งหนี้";

            if (dtgvAlldata.Columns.Contains("inv_date"))
                dtgvAlldata.Columns["inv_date"].HeaderText = "วันที่ออก";

            if (dtgvAlldata.Columns.Contains("inv_duedate"))
                dtgvAlldata.Columns["inv_duedate"].HeaderText = "กำหนดชำระ";

            if (dtgvAlldata.Columns.Contains("pro_id"))
                dtgvAlldata.Columns["pro_id"].HeaderText = "รหัสโครงการ";

            if (dtgvAlldata.Columns.Contains("cus_id"))
                dtgvAlldata.Columns["cus_id"].HeaderText = "รหัสลูกค้า";

            if (dtgvAlldata.Columns.Contains("emp_id"))
            {
                dtgvAlldata.Columns["emp_id"].HeaderText = "รหัสพนักงาน";
                // ถ้าไม่อยากให้โชว์ก็ซ่อน
                // dtgvAlldata.Columns["emp_id"].Visible = false;
            }

            if (dtgvAlldata.Columns.Contains("inv_method"))
                dtgvAlldata.Columns["inv_method"].HeaderText = "วิธีการชำระเงิน";

            if (dtgvAlldata.Columns.Contains("inv_status"))
                dtgvAlldata.Columns["inv_status"].HeaderText = "สถานะใบแจ้งหนี้";
        }

        // textbox ค้นหา
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (SearchMode == "UnpaidInvoiceByProject" || SearchMode == "PaidInvoiceByProject")
            {
                if (dtgvAlldata.DataSource is DataTable table)
                {
                    var dv = table.DefaultView;
                    string q = EscapeLikeValue(txtSearch.Text.Trim());

                    if (string.IsNullOrEmpty(q))
                    {
                        dv.RowFilter = string.Empty;
                    }
                    else
                    {
                        dv.RowFilter =
                            $"CONVERT(inv_id, 'System.String') LIKE '%{q}%' " +
                            $"OR CONVERT(inv_status, 'System.String') LIKE '%{q}%' " +
                            $"OR CONVERT(cus_id, 'System.String') LIKE '%{q}%'";
                    }
                }
            }
            else
            {
                LoadSearchData(txtSearch.Text);
            }
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            txtSearch_TextChanged(sender, e);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            ConfirmSelection();
        }

        private void dtgvAlldata_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ConfirmSelection();
            }
        }

        private void ConfirmSelection()
        {
            if (dtgvAlldata.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dtgvAlldata.SelectedRows[0];

                if (SearchMode == "Customer")
                {
                    SelectedID = selectedRow.Cells["ID"].Value?.ToString() ?? "";
                    SelectedName = selectedRow.Cells["ชื่อ"].Value?.ToString() ?? "";
                    SelectedLastName = selectedRow.Cells["นามสกุล"].Value?.ToString() ?? "";
                    SelectedIDCardOrRole = selectedRow.Cells["เลขบัตรประชาชน"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["เบอร์โทร"].Value?.ToString() ?? "";
                    SelectedEmail = selectedRow.Cells["อีเมล"].Value?.ToString() ?? "";
                }
                else if (SearchMode == "Employee")
                {
                    SelectedID = selectedRow.Cells["ID"].Value?.ToString() ?? "";
                    SelectedName = selectedRow.Cells["ชื่อ"].Value?.ToString() ?? "";
                    SelectedLastName = selectedRow.Cells["นามสกุล"].Value?.ToString() ?? "";
                    SelectedIDCardOrRole = selectedRow.Cells["ตำแหน่ง"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["เบอร์โทร"].Value?.ToString() ?? "";
                }
                else if (SearchMode == "Supplier")
                {
                    SelectedID = selectedRow.Cells["ID"].Value?.ToString() ?? "";
                    SelectedName = selectedRow.Cells["ชื่อบริษัท"].Value?.ToString() ?? "";
                    SelectedIDCardOrRole = selectedRow.Cells["เลขทะเบียนนิติบุคคล"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["เบอร์โทร"].Value?.ToString() ?? "";
                }
                else if (SearchMode == "Project")
                {
                    SelectedID = selectedRow.Cells["รหัสโครงการ"].Value?.ToString() ?? "";
                    SelectedContract = selectedRow.Cells["เลขที่สัญญา"].Value?.ToString() ?? "";
                    SelectedName = selectedRow.Cells["ชื่อโครงการ"].Value?.ToString() ?? "";
                    SelectedLastName = selectedRow.Cells["ลูกค้า"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["เบอร์โทร"].Value?.ToString() ?? "";
                    SelectedEmail = selectedRow.Cells["อีเมล"].Value?.ToString() ?? "";
                    SelectedIDCardOrRole = selectedRow.Cells["พนักงานดูแล"].Value?.ToString() ?? "";
                    SelectedCusID = selectedRow.Cells["รหัสลูกค้า"].Value?.ToString() ?? "";
                }
                else if (SearchMode == "Invoice")
                {
                    SelectedID = selectedRow.Cells["เลขที่ใบแจ้งหนี้"].Value?.ToString() ?? "";
                    SelectedCusID = selectedRow.Cells["รหัสลูกค้า"].Value?.ToString() ?? "";
                    SelectedLastName = selectedRow.Cells["รหัสโครงการ"].Value?.ToString() ?? "";
                    SelectedIDCardOrRole = selectedRow.Cells["รหัสพนักงาน"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["วิธีชำระเงิน"].Value?.ToString() ?? "";
                    SelectedEmail = selectedRow.Cells["สถานะ"].Value?.ToString() ?? "";
                }
                else if (SearchMode == "UnpaidInvoiceByProject" || SearchMode == "PaidInvoiceByProject")
                {
                    // หา key ของใบแจ้งหนี้ให้ได้แน่ ๆ
                    string invKey = "";

                    // 1) ถ้ามีคอลัมน์ inv_id ใช้อันนี้ก่อน
                    if (dtgvAlldata.Columns.Contains("inv_id"))
                    {
                        invKey = selectedRow.Cells["inv_id"].Value?.ToString() ?? "";
                    }
                    // 2) บางกรณี query อาจตั้งชื่อว่า inv_no
                    else if (dtgvAlldata.Columns.Contains("inv_no"))
                    {
                        invKey = selectedRow.Cells["inv_no"].Value?.ToString() ?? "";
                    }
                    // 3) เผื่อในอนาคตไปเปลี่ยนชื่อคอลัมน์เป็นภาษาไทย
                    else if (dtgvAlldata.Columns.Contains("เลขที่ใบแจ้งหนี้"))
                    {
                        invKey = selectedRow.Cells["เลขที่ใบแจ้งหนี้"].Value?.ToString() ?? "";
                    }

                    SelectedID = invKey;

                    // อื่น ๆ เหมือนเดิม แต่ใส่เงื่อนไขเผื่อคอลัมน์ไม่มี
                    if (dtgvAlldata.Columns.Contains("cus_id"))
                        SelectedCusID = selectedRow.Cells["cus_id"].Value?.ToString() ?? "";

                    if (dtgvAlldata.Columns.Contains("pro_id"))
                        SelectedLastName = selectedRow.Cells["pro_id"].Value?.ToString() ?? "";

                    if (dtgvAlldata.Columns.Contains("inv_method"))
                        SelectedPhone = selectedRow.Cells["inv_method"].Value?.ToString() ?? "";

                    if (dtgvAlldata.Columns.Contains("inv_status"))
                        SelectedEmail = selectedRow.Cells["inv_status"].Value?.ToString() ?? "";
                }


                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("กรุณาเลือกข้อมูลก่อน", "แจ้งเตือน",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CustomizeDataGridViewAlldata()
        {
            dtgvAlldata.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvAlldata.MultiSelect = false;
            dtgvAlldata.BorderStyle = BorderStyle.None;
            dtgvAlldata.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvAlldata.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvAlldata.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvAlldata.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvAlldata.BackgroundColor = Color.White;
            dtgvAlldata.EnableHeadersVisualStyles = false;
            dtgvAlldata.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dtgvAlldata.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvAlldata.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvAlldata.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvAlldata.ColumnHeadersHeight = 30;
            dtgvAlldata.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvAlldata.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvAlldata.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvAlldata.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);
            dtgvAlldata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvAlldata.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvAlldata.RowTemplate.Height = 30;
            dtgvAlldata.GridColor = Color.LightGray;
            dtgvAlldata.RowHeadersVisible = false;
            dtgvAlldata.ReadOnly = true;
            dtgvAlldata.AllowUserToAddRows = false;
            dtgvAlldata.AllowUserToResizeRows = false;
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            cmbSearchBy.Items.Clear();

            if (SearchMode == "Project")
            {
                cmbSearchBy.Items.Add("ชื่อโครงการ");
                cmbSearchBy.Items.Add("ลูกค้า");
                cmbSearchBy.Items.Add("รหัสโครงการ");
            }
            else if (SearchMode == "Employee")
            {
                cmbSearchBy.Items.Add("ชื่อ");
                cmbSearchBy.Items.Add("นามสกุล");
                cmbSearchBy.Items.Add("ID");
            }
            else if (SearchMode == "Customer")
            {
                cmbSearchBy.Items.Add("ชื่อ");
                cmbSearchBy.Items.Add("นามสกุล");
                cmbSearchBy.Items.Add("เลขบัตรประชาชน");
            }
            else if (SearchMode == "Supplier")
            {
                cmbSearchBy.Items.Add("ชื่อบริษัท");
                cmbSearchBy.Items.Add("เลขทะเบียนนิติบุคคล");
                cmbSearchBy.Items.Add("เบอร์โทรศัพท์");
                cmbSearchBy.Items.Add("อีเมล");
            }
            else if (SearchMode == "Invoice")
            {
                cmbSearchBy.Items.Add("เลขที่ใบแจ้งหนี้");
            }
            // --- We now check for BOTH invoice modes here ---
            else if (SearchMode == "UnpaidInvoiceByProject" || SearchMode == "PaidInvoiceByProject")
            {
                // Now, this will run for both paid and unpaid invoice searches.
                cmbSearchBy.Items.Add("เลขที่ใบแจ้งหนี้");
                cmbSearchBy.Items.Add("สถานะใบแจ้งหนี้");
                cmbSearchBy.Items.Add("วันที่ออก");
                cmbSearchBy.Items.Add("กำหนดชำระ");
                cmbSearchBy.Items.Add("รหัสลูกค้า");
            }
            if (cmbSearchBy.Items.Count > 0)
                cmbSearchBy.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (dtgvAlldata.DataSource is DataTable table)
            {
                string keyword = EscapeLikeValue(txtSearch.Text.Trim());

                if (string.IsNullOrEmpty(keyword))
                {
                    table.DefaultView.RowFilter = string.Empty;
                    return;
                }

                if (SearchMode == "UnpaidInvoiceByProject")
                {
                    // ใช้เกณฑ์เดียวกับการพิมพ์ค้นหา
                    table.DefaultView.RowFilter =
                        $"CONVERT(inv_id, 'System.String') LIKE '%{keyword}%'" +
                        $" OR CONVERT(inv_status, 'System.String') LIKE '%{keyword}%'" +
                        $" OR CONVERT(cus_id, 'System.String') LIKE '%{keyword}%'";
                    return;
                }

                string selectedColumn = cmbSearchBy.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedColumn))
                {
                    MessageBox.Show("กรุณาเลือกคอลัมน์สำหรับค้นหา", "Search",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                table.DefaultView.RowFilter =
                    $"CONVERT([{selectedColumn}], 'System.String') LIKE '%{keyword}%'";
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private static string EscapeLikeValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("*", "[*]")
                .Replace("'", "''");
        }
    }
}
