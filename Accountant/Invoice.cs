using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using JRSApplication.Components; // ✅ ใช้ Model ใหม่
using JRSApplication.Data_Access_Layer;

namespace JRSApplication.Accountant
{
    public partial class Invoice : UserControl
    {
        public Invoice()
        {
            InitializeComponent();
        }

        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Project");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                txtProjectID.Text = searchForm.SelectedID;                  // รหัสโครงการ
                txtContractNumber.Text = searchForm.SelectedContract;       // เลขที่สัญญา
                txtProjectName.Text = searchForm.SelectedName;              // ชื่อโครงการ
                txtCusID.Text = searchForm.SelectedCusID;  // ✅ แก้ให้ใช้ cus_id โดยตรง                                                          
                txtCusName.Text = searchForm.SelectedLastName;             // ชื่อ-นามสกุล ลูกค้า


                LoadPhasesToComboBox(searchForm.SelectedID);
            }
        }

        private void LoadPhasesToComboBox(string projectId)
        {
            SearchService service = new SearchService();
            DataTable dt = service.GetPhasesByProjectId(projectId);

            DataRow dr = dt.NewRow();
            dr["phase_id"] = DBNull.Value;
            dr["phase_no"] = "-- เลือกเฟส --";
            dt.Rows.InsertAt(dr, 0);

            cmbPhase.DisplayMember = "phase_no";
            cmbPhase.ValueMember = "phase_id";
            cmbPhase.DataSource = dt;
            cmbPhase.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ เตรียม model หลัก
                InvoiceModel model = new InvoiceModel
                {
                    InvNo = txtInvNo.Text.Trim(),
                    InvDate = dtpInvDate.Value,
                    InvDueDate = dtpDueDate.Value,
                    CusId = txtCusID.Text.Trim(),
                    CusName = txtCusName.Text.Trim(),
                    ProId = txtProjectID.Text.Trim(),
                    ProNumber = txtContractNumber.Text.Trim(),
                    ProName = txtProjectName.Text.Trim(),
                    PhaseId = cmbPhase.SelectedValue?.ToString(),
                    PhaseBudget = txtPhaseBudget.Text.Trim(),
                    PhaseDetail = txtPhaseDetail.Text.Trim()
                };

                InvoiceDAL dal = new InvoiceDAL();
                int newId = dal.InsertInvoice(model);

                if (newId > 0)
                {
                    // ✅ ถ้า insert invoice สำเร็จ → ทำต่อ insert detail
                    string detail = txtDetail.Text.Trim();
                    int quantity = int.Parse(txtQuantity.Text.Trim());
                    decimal price = decimal.Parse(txtPrice.Text.Trim());
                    decimal vatRate = 7; // ปรับได้ถ้าต้องการ

                    InvoiceDetailDAL detailDal = new InvoiceDetailDAL();
                    detailDal.InsertInvoiceDetail(newId, detail, price, quantity, vatRate);

                    MessageBox.Show("บันทึกข้อมูลสำเร็จ! รหัสใบแจ้งหนี้: " + newId, "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ ล้างค่า
                    txtInvNo.Text = "";
                    txtCusID.Text = "";
                    txtCusName.Text = "";
                    txtProjectID.Text = "";
                    txtContractNumber.Text = "";
                    txtProjectName.Text = "";
                    txtPhaseBudget.Text = "";
                    txtPhaseDetail.Text = "";
                    txtDetail.Text = "";
                    txtQuantity.Text = "";
                    txtPrice.Text = "";
                }
                else
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกใบแจ้งหนี้", "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            Form invoiceForm = new Form();
            invoiceForm.Text = "ใบแจ้งหนี้";
            invoiceForm.Size = new Size(850, 1100);
            invoiceForm.StartPosition = FormStartPosition.CenterScreen;

            InvoicePrint invoicePrint = new InvoicePrint();
            invoicePrint.Dock = DockStyle.Fill;

            invoiceForm.Controls.Add(invoicePrint);
            invoiceForm.ShowDialog();
        }


    }
}
