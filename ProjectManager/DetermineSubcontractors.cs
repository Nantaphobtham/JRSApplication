using JRSApplication.Components;
using JRSApplication.Components.Models;
using JRSApplication.Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class DetermineSubcontractors : UserControl
    {
        private string supplierID = "";  // เก็บ ID สำหรับ supplier
        private string projectID = "";   // เก็บ ID สำหรับ project


        public DetermineSubcontractors()
        {
            InitializeComponent();
            CustomizeDataGridViewAssignment();
            LoadAssignments();
            
        }

        private void LoadPhasesToComboBox(string projectId)
        {
            SearchService service = new SearchService();
            DataTable dt = service.GetPhasesByProjectId(projectId);

            cmbSelectPhase.DisplayMember = "phase_no";   // ✅ แสดงเลขเฟส
            cmbSelectPhase.ValueMember = "phase_id";     // ✅ ใช้ phase_id ตอนบันทึก
            cmbSelectPhase.DataSource = dt;
        }
        private void LoadAssignments()
        {
            SupplierWorkAssignmentDAL dal = new SupplierWorkAssignmentDAL();
            dtgvAssignment.DataSource = dal.GetAllAssignmentsWithPhase();
        }

        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Project");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                projectID = searchForm.SelectedID; // เก็บไว้ใช้ตอนบันทึก

                txtPorjectID.Text = searchForm.SelectedID;     // หรือ txtProjectID ถ้าพิมพ์ผิด
                txtProjectName.Text = searchForm.SelectedName;

                LoadPhasesToComboBox(projectID); // ✅ โหลดข้อมูลลง ComboBox
            }
        }

        private void btnSearchSupplier_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Supplier");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                supplierID = searchForm.SelectedID; // เก็บไว้ใช้ตอนบันทึก

                txtSupplierName.Text = searchForm.SelectedName;
                txtSupplierJuristic.Text = searchForm.SelectedIDCardOrRole;  // เลขทะเบียนนิติบุคคล
                txtSupplierPhone.Text = searchForm.SelectedPhone;
            }
        }

        //private void InitializeDataGridViewAssignment()   ยังไม่จำเป็นในตอนนี้
        //{
        //    if (dtgvAssignment.Columns.Count == 0)
        //    {
        //        dtgvAssignment.AllowUserToAddRows = false;

        //        // ✅ เพิ่มคอลัมน์
        //        dtgvAssignment.Columns.Add("AssignmentID", "รหัสงาน");
        //        dtgvAssignment.Columns.Add("SupplierID", "รหัสผู้รับเหมา");
        //        dtgvAssignment.Columns.Add("StartDate", "วันที่เริ่ม");
        //        dtgvAssignment.Columns.Add("DueDate", "วันที่สิ้นสุด");
        //        dtgvAssignment.Columns.Add("AssignDescription", "รายละเอียดงาน");
        //        dtgvAssignment.Columns.Add("AssignRemark", "หมายเหตุ");
        //        dtgvAssignment.Columns.Add("PhaseNo", "เฟสที่");

        //        // ✅ ตั้งค่ารูปแบบ + การจัดชิด
        //        dtgvAssignment.Columns["AssignmentID"].Width = 100;
        //        dtgvAssignment.Columns["AssignmentID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        dtgvAssignment.Columns["AssignmentID"].ReadOnly = true;

        //        dtgvAssignment.Columns["SupplierID"].Width = 120;
        //        dtgvAssignment.Columns["SupplierID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        dtgvAssignment.Columns["SupplierID"].ReadOnly = true;

        //        dtgvAssignment.Columns["StartDate"].Width = 120;
        //        dtgvAssignment.Columns["StartDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        dtgvAssignment.Columns["StartDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
        //        dtgvAssignment.Columns["StartDate"].ReadOnly = true;

        //        dtgvAssignment.Columns["DueDate"].Width = 120;
        //        dtgvAssignment.Columns["DueDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        dtgvAssignment.Columns["DueDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
        //        dtgvAssignment.Columns["DueDate"].ReadOnly = true;

        //        dtgvAssignment.Columns["AssignDescription"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        //        dtgvAssignment.Columns["AssignDescription"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        //        dtgvAssignment.Columns["AssignDescription"].ReadOnly = true;

        //        dtgvAssignment.Columns["AssignRemark"].Width = 200;
        //        dtgvAssignment.Columns["AssignRemark"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        //        dtgvAssignment.Columns["AssignRemark"].ReadOnly = true;

        //        dtgvAssignment.Columns["PhaseNo"].Width = 80;
        //        dtgvAssignment.Columns["PhaseNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        dtgvAssignment.Columns["PhaseNo"].ReadOnly = true;

        //        // ✅ เรียกใช้การตกแต่งรวม
        //        CustomizeDataGridViewAssignment();
        //    }
        //}

        private void CustomizeDataGridViewAssignment()
        {
            // 🔧 ตั้งค่าพื้นฐาน
            dtgvAssignment.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dtgvAssignment.BorderStyle = BorderStyle.None;
            dtgvAssignment.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvAssignment.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvAssignment.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvAssignment.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvAssignment.BackgroundColor = Color.White;

            // 🔧 ตั้งค่าหัวตาราง (Header)
            dtgvAssignment.EnableHeadersVisualStyles = false;
            dtgvAssignment.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvAssignment.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvAssignment.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvAssignment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            dtgvAssignment.ColumnHeadersHeight = 30;

            // 🔧 ตั้งค่าข้อมูล
            dtgvAssignment.DefaultCellStyle.Font = new Font("Segoe UI", 15);
            dtgvAssignment.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvAssignment.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvAssignment.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            // 🔧 ปรับขนาดอัตโนมัติ
            dtgvAssignment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvAssignment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvAssignment.RowTemplate.Height = 30;

            // 🔧 ปรับ Grid และแถวหัวตาราง
            dtgvAssignment.GridColor = Color.LightGray;
            dtgvAssignment.RowHeadersVisible = false;

            // 🔒 ปิดการแก้ไขข้อมูลโดยตรง
            dtgvAssignment.ReadOnly = true;
            dtgvAssignment.AllowUserToAddRows = false;
            dtgvAssignment.AllowUserToResizeRows = false;
        }




        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!ValidateBeforeSave())
                return; // ❌ ไม่ผ่านการตรวจสอบ

            SupplierWorkAssignment model = new SupplierWorkAssignment
            {
                SupId = supplierID,
                StartDate = startDate.Value,
                DueDate = dueDate.Value,
                AssignDescription = txtAssignDescription.Text.Trim(),
                AssignRemark = txtRemark.Text.Trim(),
                PhaseId = Convert.ToInt32(cmbSelectPhase.SelectedValue)
            };

            SupplierWorkAssignmentDAL dal = new SupplierWorkAssignmentDAL();
            dal.Insert(model);

            MessageBox.Show("บันทึกข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearandClossForm();
            LoadAssignments(); // ✅ รีโหลดข้อมูลล่าสุด
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            openForm();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }


        private void openForm()
        {
            btnSearchSupplier.Enabled = true;
            btnSearchProject.Enabled = true;
            txtSupplierName.Enabled = true;
            txtSupplierJuristic.Enabled = true;
            txtSupplierPhone.Enabled = true;
            txtProjectName.Enabled = true;
            txtPorjectID.Enabled = true;
            txtRemark.Enabled = true;
            txtAssignDescription.Enabled = true;
            cmbSelectPhase.Enabled = true;
            startDate.Enabled = true;
            dueDate.Enabled = true;
        }

        private void ClearandClossForm()
        {
            // 🔄 ล้างค่า TextBox
            txtSupplierName.Text = "";
            txtSupplierJuristic.Text = "";
            txtSupplierPhone.Text = "";

            txtProjectName.Text = "";
            txtPorjectID.Text = "";

            txtAssignDescription.Text = "";
            txtRemark.Text = "";

            // 🔄 รีเซ็ต ComboBox
            cmbSelectPhase.DataSource = null;
            cmbSelectPhase.Items.Clear();

            // 🔄 รีเซ็ต DateTimePicker
            startDate.Value = DateTime.Now;
            dueDate.Value = DateTime.Now;

            // 🔒 ปิดการใช้งานทุก control
            btnSearchSupplier.Enabled = false;
            btnSearchProject.Enabled = false;

            txtSupplierName.Enabled = false;
            txtSupplierJuristic.Enabled = false;
            txtSupplierPhone.Enabled = false;

            txtProjectName.Enabled = false;
            txtPorjectID.Enabled = false;

            txtAssignDescription.Enabled = false;
            txtRemark.Enabled = false;

            cmbSelectPhase.Enabled = false;
            startDate.Enabled = false;
            dueDate.Enabled = false;

            // 💡 ล้างตัวแปรที่ใช้เก็บ ID ต่าง ๆ ถ้าต้องการ
            supplierID = "";
            projectID = "";
        }

        private bool ValidateBeforeSave()
        {
            // ตรวจสอบ supplierID
            if (string.IsNullOrEmpty(supplierID))
            {
                MessageBox.Show("กรุณาเลือกผู้รับเหมา", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // ตรวจสอบเลือกเฟส
            if (cmbSelectPhase.SelectedIndex == -1 || cmbSelectPhase.SelectedValue == null)
            {
                MessageBox.Show("กรุณาเลือกเฟสที่ต้องการดำเนินงาน", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // ตรวจสอบวันที่เริ่มต้น
            if (startDate.Value == DateTime.MinValue)
            {
                MessageBox.Show("กรุณาระบุวันที่เริ่มต้น", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // ตรวจสอบวันที่สิ้นสุด
            if (dueDate.Value == DateTime.MinValue)
            {
                MessageBox.Show("กรุณาระบุวันที่สิ้นสุด", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // ตรวจสอบว่ากำหนดวันเริ่มไม่มากกว่าวันสิ้นสุด
            if (startDate.Value > dueDate.Value)
            {
                MessageBox.Show("วันที่เริ่มต้นต้องไม่มากกว่าวันที่สิ้นสุด", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // ตรวจสอบรายละเอียดงาน
            if (string.IsNullOrWhiteSpace(txtAssignDescription.Text))
            {
                MessageBox.Show("กรุณาระบุรายละเอียดงาน", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true; // ✅ ผ่านทุกเงื่อนไข
        }


        private void LoadSupplierDetail(string supId)
        {
            SupplierWorkAssignmentDAL dal = new SupplierWorkAssignmentDAL();
            DataTable dt = dal.GetSupplierInfoFromAssignment(supId); // ✔️ ดึงข้อมูล DAL

            if (dt.Rows.Count > 0)
            {
                // Set ข้อมูล Supplier
                txtSupplierName.Text = dt.Rows[0]["Name"].ToString();
                txtSupplierJuristic.Text = dt.Rows[0]["Juristic"].ToString();
                txtSupplierPhone.Text = dt.Rows[0]["Phone"].ToString();
                txtPorjectID.Text = dt.Rows[0]["ProjectID"].ToString();
                txtProjectName.Text = dt.Rows[0]["ProjectName"].ToString();

                // Load ComboBox Phase
                string projectId = dt.Rows[0]["ProjectID"].ToString();
                string phaseId = dt.Rows[0]["PhaseID"].ToString();

                LoadPhasesToComboBox(projectId);

                // ⭐ Trick ใช้ BeginInvoke เพื่อให้ ComboBox Ready ก่อน Set SelectedValue
                cmbSelectPhase.BeginInvoke((Action)(() =>
                {
                    cmbSelectPhase.SelectedValue = phaseId;
                }));
            }
        }


        private void dtgvAssignment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // ไม่ใช่ header
            {
                DataGridViewRow row = dtgvAssignment.Rows[e.RowIndex];

                // 🔁 ดึงค่าแต่ละ column ตามชื่อหัวตารางภาษาไทย
                string supplierID = row.Cells["รหัสผู้รับเหมา"].Value?.ToString();
                string startDateStr = row.Cells["วันที่เริ่ม"].Value?.ToString();
                string dueDateStr = row.Cells["วันที่สิ้นสุด"].Value?.ToString();
                string assignDesc = row.Cells["รายละเอียดงาน"].Value?.ToString();
                string assignRemark = row.Cells["หมายเหตุ"].Value?.ToString();
                string phaseNo = row.Cells["เฟสที่"].Value?.ToString();

                // ✅ ตั้งค่าควบคุมบนฟอร์ม
                txtAssignDescription.Text = assignDesc;
                txtRemark.Text = assignRemark;

                if (DateTime.TryParse(startDateStr, out DateTime sDate))
                    startDate.Value = sDate;

                if (DateTime.TryParse(dueDateStr, out DateTime dDate))
                    dueDate.Value = dDate;

                // ✅ เลือก combobox โดยเทียบจาก phase_no
                foreach (DataRowView item in cmbSelectPhase.Items)
                {
                    if (item["phase_no"].ToString() == phaseNo)
                    {
                        cmbSelectPhase.SelectedValue = item["phase_id"];
                        break;
                    }
                }

                // ✅ ดึงข้อมูล supplier เพิ่มเติม
                LoadSupplierDetail(supplierID);
            }
        }

    }
}
