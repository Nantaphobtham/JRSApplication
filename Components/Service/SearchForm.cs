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

            // ✅ Title
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
            else
                lblTitle.Text = "ค้นหา";

            // ✅ Initial data load
            if (SearchMode == "UnpaidInvoiceByProject")
                LoadSearchData(optionalProjectId ?? "");
            else
                LoadSearchData("");

            CustomizeDataGridViewAlldata();

            // ✅ DoubleClick = เลือก
            dtgvAlldata.CellDoubleClick += dtgvAlldata_CellDoubleClick;
        }

        private void LoadSearchData(string keywordOrProjectId = "")
        {
            dtgvAlldata.AutoGenerateColumns = true;

            if (SearchMode == "UnpaidInvoiceByProject")
            {
                dtgvAlldata.DataSource = searchService.SearchData(SearchMode, keywordOrProjectId);
            }
            else
            {
                dtgvAlldata.DataSource = searchService.SearchData(SearchMode, "");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (SearchMode == "UnpaidInvoiceByProject")
            {
                if (dtgvAlldata.DataSource is DataTable table)
                {
                    var dv = table.DefaultView;
                    string q = txtSearch.Text.Trim().Replace("'", "''");

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

        // ✅ ใช้ฟังก์ชันกลาง
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
                else if (SearchMode == "UnpaidInvoiceByProject")
                {
                    SelectedID = selectedRow.Cells["inv_id"].Value?.ToString() ?? "";
                    SelectedCusID = selectedRow.Cells["cus_id"].Value?.ToString() ?? "";
                    SelectedLastName = selectedRow.Cells["pro_id"].Value?.ToString() ?? "";
                    SelectedPhone = selectedRow.Cells["inv_method"].Value?.ToString() ?? "";
                    SelectedEmail = selectedRow.Cells["inv_status"].Value?.ToString() ?? "";
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("กรุณาเลือกข้อมูลก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            dtgvAlldata.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
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
                // ✅ ปรับชื่อฟิลด์ค้นหาให้เหมาะกับซัพพลายเออร์
                cmbSearchBy.Items.Add("ชื่อบริษัท");
                cmbSearchBy.Items.Add("เลขทะเบียนนิติบุคคล");
                cmbSearchBy.Items.Add("เบอร์โทรศัพท์");
                cmbSearchBy.Items.Add("อีเมล");
            }
            else if (SearchMode == "Invoice")
            {
                cmbSearchBy.Items.Add("เลขที่ใบแจ้งหนี้");
            }

            if (cmbSearchBy.Items.Count > 0)
                cmbSearchBy.SelectedIndex = 0;
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            if (dtgvAlldata.DataSource is DataTable table)
            {
                string keyword = txtSearch.Text.Trim().Replace("'", "''");

                if (string.IsNullOrEmpty(keyword))
                {
                    table.DefaultView.RowFilter = string.Empty;
                }
                else
                {
                    if (SearchMode == "Project")
                    {
                        table.DefaultView.RowFilter =
                            $"[ชื่อโครงการ] LIKE '%{keyword}%' OR [ลูกค้า] LIKE '%{keyword}%'";
                    }
                    else if (SearchMode == "Employee")
                    {
                        table.DefaultView.RowFilter =
                            $"[ชื่อ] LIKE '%{keyword}%' OR [นามสกุล] LIKE '%{keyword}%'";
                    }
                    else if (SearchMode == "Customer")
                    {
                        table.DefaultView.RowFilter =
                            $"[ชื่อ] LIKE '%{keyword}%' OR [นามสกุล] LIKE '%{keyword}%'";
                    }
                    else if (SearchMode == "Supplier")
                    {
                        // ✅ ให้ค้นพร้อมกันจากชื่อบริษัท / เลขทะเบียน / เบอร์ / อีเมล
                        table.DefaultView.RowFilter =
                            $"[ชื่อบริษัท] LIKE '%{keyword}%' " +
                            $"OR [เลขทะเบียนนิติบุคคล] LIKE '%{keyword}%' " +
                            $"OR [เบอร์โทรศัพท์] LIKE '%{keyword}%' " +
                            $"OR [อีเมล] LIKE '%{keyword}%'";
                    }
                    else if (SearchMode == "Invoice")
                    {
                        table.DefaultView.RowFilter =
                            $"CONVERT([เลขที่ใบแจ้งหนี้], 'System.String') LIKE '%{keyword}%'";
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (dtgvAlldata.DataSource is DataTable table)
            {
                string keyword = txtSearch.Text.Trim().Replace("'", "''");

                if (string.IsNullOrEmpty(keyword))
                {
                    table.DefaultView.RowFilter = string.Empty;
                    return;
                }

                string selectedColumn = cmbSearchBy.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedColumn))
                {
                    MessageBox.Show("⚠️ Please select a search column.", "Search",
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
    }
}
