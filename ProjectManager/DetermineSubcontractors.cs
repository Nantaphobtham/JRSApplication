using JRSApplication.Components;
using JRSApplication.Components.Models;
using JRSApplication.Components.Service;
using JRSApplication.Data_Access_Layer;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class DetermineSubcontractors : UserControl
    {
        private string supplierID = "";
        private string projectID = "";
        private string currentAssignmentId = "";
        private bool isEditing = false;
        private SupplierAssignmentFile currentFile;
        private string _empId;

        private FormPDFPreview pdfPreviewForm = null;
        private Timer hoverCheckTimer;

        // เก็บ DataTable ทั้งหมดสำหรับ filter
        private DataTable _allAssignments;

        public DetermineSubcontractors(string empId)
        {
            InitializeComponent();
            _empId = empId;

            CustomizeDataGridViewAssignment();
            LoadAssignments();

            startDate.ValueChanged += startDate_ValueChanged;
            dueDate.ValueChanged += dueDate_ValueChanged;
            ApplyDateGuards();

            // ✅ ผูก Searchbox สำหรับค้นหาใบมอบหมายงาน
            try
            {
                searchboxControl1.DefaultRole = "Projectmanager";
                searchboxControl1.DefaultFunction = "กำหนดผู้รับเหมาช่วง";
                searchboxControl1.SetRoleAndFunction("Projectmanager", "กำหนดผู้รับเหมา");

                searchboxControl1.SearchTriggered += SearchboxAssignment_SearchTriggered;
            }
            catch { }
        }

        // ============== Searchbox -> Filter Assignments ===============

        private void SearchboxAssignment_SearchTriggered(object sender, SearchEventArgs e)
        {
            ApplyAssignmentFilter(e.SearchBy, e.Keyword);
        }

        private void ApplyAssignmentFilter(string searchBy, string keyword)
        {
            if (_allAssignments == null) return;

            string q = (keyword ?? "").Trim().ToLowerInvariant();

            if (string.IsNullOrEmpty(q))
            {
                dtgvAssignment.DataSource = _allAssignments;
                return;
            }

            var rows = _allAssignments.AsEnumerable();

            bool HasCol(string col) => _allAssignments.Columns.Contains(col);

            Func<DataRow, bool> containsIn = r =>
            {
                bool any = false;

                if (HasCol("รหัสงาน"))
                    any |= (r["รหัสงาน"]?.ToString() ?? "").ToLowerInvariant().Contains(q);
                if (HasCol("รหัสโครงการ"))
                    any |= (r["รหัสโครงการ"]?.ToString() ?? "").ToLowerInvariant().Contains(q);
                if (HasCol("เฟสที่"))
                    any |= (r["เฟสที่"]?.ToString() ?? "").ToLowerInvariant().Contains(q);
                if (HasCol("รหัสผู้รับเหมา"))
                    any |= (r["รหัสผู้รับเหมา"]?.ToString() ?? "").ToLowerInvariant().Contains(q);
                if (HasCol("ชื่อผู้รับเหมา"))
                    any |= (r["ชื่อผู้รับเหมา"]?.ToString() ?? "").ToLowerInvariant().Contains(q);
                if (HasCol("รายละเอียดงาน"))
                    any |= (r["รายละเอียดงาน"]?.ToString() ?? "").ToLowerInvariant().Contains(q);

                return any;
            };

            switch (searchBy)
            {
                case "รหัสงาน":
                    if (HasCol("รหัสงาน"))
                        rows = rows.Where(r => (r["รหัสงาน"]?.ToString() ?? "").ToLowerInvariant().Contains(q));
                    break;

                case "รหัสโครงการ":
                    if (HasCol("รหัสโครงการ"))
                        rows = rows.Where(r => (r["รหัสโครงการ"]?.ToString() ?? "").ToLowerInvariant().Contains(q));
                    break;

                case "เฟสที่":
                    if (HasCol("เฟสที่"))
                        rows = rows.Where(r => (r["เฟสที่"]?.ToString() ?? "").ToLowerInvariant().Contains(q));
                    break;

                case "รหัสผู้รับเหมา":          // ✅ เพิ่มตรงนี้
                    if (HasCol("รหัสผู้รับเหมา"))
                        rows = rows.Where(r => (r["รหัสผู้รับเหมา"]?.ToString() ?? "").ToLowerInvariant().Contains(q));
                    break;

                case "ชื่อผู้รับเหมา":
                    if (HasCol("ชื่อผู้รับเหมา"))
                        rows = rows.Where(r => (r["ชื่อผู้รับเหมา"]?.ToString() ?? "").ToLowerInvariant().Contains(q));
                    break;

                default: // ค้นทุกคอลัมน์หลัก
                    rows = rows.Where(containsIn);
                    break;
            }

            DataTable filtered;
            if (rows.Any())
                filtered = rows.CopyToDataTable();
            else
                filtered = _allAssignments.Clone();

            dtgvAssignment.DataSource = filtered;
        }


        // ============== โค้ดเดิมของ DetermineSubcontractors ==============

        private void LoadPhasesToComboBox(string projectId)
        {
            SearchService service = new SearchService();
            DataTable dt = service.GetPhasesByProjectId(projectId);

            DataRow dr = dt.NewRow();
            dr["phase_id"] = DBNull.Value;
            dr["phase_no"] = "-- เลือกเฟส --";
            dt.Rows.InsertAt(dr, 0);

            cmbSelectPhase.DisplayMember = "phase_no";
            cmbSelectPhase.ValueMember = "phase_id";
            cmbSelectPhase.DataSource = dt;
            cmbSelectPhase.SelectedIndex = 0;
        }

        private void LoadAssignments()
        {
            SupplierWorkAssignmentDAL dal = new SupplierWorkAssignmentDAL();
            var dt = dal.GetAllAssignmentsWithPhase();

            _allAssignments = dt;
            dtgvAssignment.DataSource = dt;
        }

        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Project");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                projectID = searchForm.SelectedID;
                txtPorjectID.Text = searchForm.SelectedID;
                txtProjectName.Text = searchForm.SelectedName;
                txtContractnumber.Text = searchForm.SelectedContract;
                LoadPhasesToComboBox(projectID);
            }
        }

        private void btnSearchSupplier_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Supplier");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                supplierID = searchForm.SelectedID;
                txtSupplierName.Text = searchForm.SelectedName;
                txtSupplierJuristic.Text = searchForm.SelectedIDCardOrRole;
                txtSupplierPhone.Text = searchForm.SelectedPhone;
            }
        }

        private void CustomizeDataGridViewAssignment()
        {
            dtgvAssignment.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvAssignment.BorderStyle = BorderStyle.None;
            dtgvAssignment.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvAssignment.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvAssignment.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvAssignment.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvAssignment.BackgroundColor = Color.White;
            dtgvAssignment.EnableHeadersVisualStyles = false;
            dtgvAssignment.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvAssignment.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvAssignment.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvAssignment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            dtgvAssignment.ColumnHeadersHeight = 30;
            dtgvAssignment.DefaultCellStyle.Font = new Font("Segoe UI", 15);
            dtgvAssignment.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvAssignment.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvAssignment.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);
            dtgvAssignment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvAssignment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvAssignment.RowTemplate.Height = 30;
            dtgvAssignment.GridColor = Color.LightGray;
            dtgvAssignment.RowHeadersVisible = false;
            dtgvAssignment.ReadOnly = true;
            dtgvAssignment.AllowUserToAddRows = false;
            dtgvAssignment.AllowUserToResizeRows = false;
        }

        private void EnableFormFields()
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
            txtDate.Enabled = true;
            cmbSelectPhase.Enabled = true;
            startDate.Enabled = true;
            dueDate.Enabled = true;
            btnInsertFile.Enabled = true;
        }

        private void DisableFormFields()
        {
            btnSearchSupplier.Enabled = false;
            btnSearchProject.Enabled = false;
            txtSupplierName.Enabled = false;
            txtSupplierJuristic.Enabled = false;
            txtSupplierPhone.Enabled = false;
            txtProjectName.Enabled = false;
            txtPorjectID.Enabled = false;
            txtRemark.Enabled = false;
            txtAssignDescription.Enabled = false;
            txtDate.Enabled = false;
            cmbSelectPhase.Enabled = false;
            startDate.Enabled = false;
            dueDate.Enabled = false;
            btnInsertFile.Enabled = false;
        }

        private void ClearandClossForm()
        {
            txtSupplierName.Text = "";
            txtSupplierJuristic.Text = "";
            txtSupplierPhone.Text = "";
            txtProjectName.Text = "";
            txtPorjectID.Text = "";
            txtAssignDescription.Text = "";
            txtRemark.Text = "";
            cmbSelectPhase.DataSource = null;
            cmbSelectPhase.Items.Clear();
            startDate.Value = DateTime.Now;
            dueDate.Value = DateTime.Now;
            currentAssignmentId = "";
            isEditing = false;
            DisableFormFields();
            supplierID = "";
            projectID = "";
            txtDate.Text = "";
            txtContractnumber.Text = "";
            currentFile = null;
            btnInsertFile.Text = "แนบไฟล์ PDF";
        }

        private bool ValidateBeforeSave()
        {
            if (cmbSelectPhase.SelectedIndex <= 0 || cmbSelectPhase.SelectedValue == null || cmbSelectPhase.SelectedValue == DBNull.Value)
            {
                MessageBox.Show("กรุณาเลือกเฟสที่ต้องการดำเนินงาน", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrEmpty(supplierID))
            {
                MessageBox.Show("กรุณาเลือกผู้รับเหมา", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (startDate.Value > dueDate.Value)
            {
                MessageBox.Show("วันที่เริ่มต้นต้องไม่มากกว่าวันที่สิ้นสุด", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtAssignDescription.Text))
            {
                MessageBox.Show("กรุณาระบุรายละเอียดงาน", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (currentFile == null)
            {
                MessageBox.Show("กรุณาแนบไฟล์ก่อนบันทึก", "เตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateBeforeSave()) return;

            SupplierWorkAssignment model = new SupplierWorkAssignment
            {
                AssignmentId = isEditing ? currentAssignmentId : "",
                SupId = supplierID,
                StartDate = startDate.Value,
                DueDate = dueDate.Value,
                AssignDescription = txtAssignDescription.Text.Trim(),
                AssignRemark = txtRemark.Text.Trim(),
                PhaseId = Convert.ToInt32(cmbSelectPhase.SelectedValue),
                AssignStatus = "InProgress",
                EmployeeID = _empId
            };

            SupplierWorkAssignmentDAL dal = new SupplierWorkAssignmentDAL();

            try
            {
                if (isEditing && !string.IsNullOrEmpty(model.AssignmentId))
                {
                    dal.Update(model);

                    if (currentFile != null)
                    {
                        var fileDal = new SupplierAssignmentFileDAL();
                        fileDal.DeleteByAssignmentId(model.AssignmentId);
                        currentFile.AssignmentId = model.AssignmentId;
                        fileDal.Insert(currentFile);
                        currentFile = null;
                    }
                }
                else
                {
                    model.AssignmentId = dal.GenerateWorkOrderId();
                    dal.Insert(model);

                    if (currentFile != null)
                    {
                        var fileDal = new SupplierAssignmentFileDAL();
                        fileDal.DeleteByAssignmentId(model.AssignmentId);
                        currentFile.AssignmentId = model.AssignmentId;
                        fileDal.Insert(currentFile);
                        currentFile = null;
                    }
                }

                MessageBox.Show("บันทึกข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearandClossForm();
                LoadAssignments();
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการบันทึก: " + ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearandClossForm();
            EnableFormFields();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentAssignmentId))
            {
                MessageBox.Show("กรุณาเลือกรายการก่อนแก้ไข", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isEditing = true;
            EnableFormFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentAssignmentId))
            {
                MessageBox.Show("กรุณาเลือกรายการก่อน", "เตือน",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("ยืนยันการลบรายการนี้หรือไม่?", "ยืนยัน",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                var fileDal = new SupplierAssignmentFileDAL();
                fileDal.DeleteByAssignmentId(currentAssignmentId);

                var dal = new SupplierWorkAssignmentDAL();
                dal.Delete(currentAssignmentId);

                MessageBox.Show("ลบข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearandClossForm();
                LoadAssignments();
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการลบ: " + ex.Message, "ข้อผิดพลาด",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtgvAssignment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvAssignment.Rows[e.RowIndex];
                currentAssignmentId = row.Cells["รหัสงาน"].Value?.ToString() ?? "";
                supplierID = row.Cells["รหัสผู้รับเหมา"].Value?.ToString();
                string startDateStr = row.Cells["วันที่เริ่ม"].Value?.ToString();
                string dueDateStr = row.Cells["วันที่สิ้นสุด"].Value?.ToString();
                string assignDesc = row.Cells["รายละเอียดงาน"].Value?.ToString();
                string assignRemark = row.Cells["หมายเหตุ"].Value?.ToString();
                string phaseNo = row.Cells["เฟสที่"].Value?.ToString();

                txtAssignDescription.Text = assignDesc;
                txtRemark.Text = assignRemark;
                if (DateTime.TryParse(startDateStr, out DateTime sDate)) startDate.Value = sDate;
                if (DateTime.TryParse(dueDateStr, out DateTime dDate)) dueDate.Value = dDate;

                LoadSupplierDetail(supplierID);

                if (cmbSelectPhase.Items.Count > 0)
                {
                    foreach (DataRowView item in cmbSelectPhase.Items)
                    {
                        if (item["phase_no"].ToString() == phaseNo)
                        {
                            cmbSelectPhase.SelectedValue = item["phase_id"];
                            break;
                        }
                    }
                }

                DisableFormFields();
            }
        }

        private void LoadSupplierDetail(string supId)
        {
            SupplierWorkAssignmentDAL dal = new SupplierWorkAssignmentDAL();
            DataTable dt = dal.GetSupplierInfoFromAssignment(supId);

            if (dt.Rows.Count > 0)
            {
                txtSupplierName.Text = dt.Rows[0]["Name"].ToString();
                txtSupplierJuristic.Text = dt.Rows[0]["Juristic"].ToString();
                txtSupplierPhone.Text = dt.Rows[0]["Phone"].ToString();
                txtPorjectID.Text = dt.Rows[0]["ProjectID"].ToString();
                txtProjectName.Text = dt.Rows[0]["ProjectName"].ToString();

                string projectId = dt.Rows[0]["ProjectID"].ToString();
                string phaseId = dt.Rows[0]["PhaseID"].ToString();
                LoadPhasesToComboBox(projectId);
                cmbSelectPhase.BeginInvoke((Action)(() =>
                {
                    cmbSelectPhase.SelectedValue = phaseId;
                }));
            }
        }

        private void btnInsertFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "เลือกไฟล์ PDF";
                openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    if (Path.GetExtension(filePath).ToLower() != ".pdf")
                    {
                        MessageBox.Show("กรุณาเลือกไฟล์ PDF เท่านั้น", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    FileInfo fileInfo = new FileInfo(filePath);
                    long maxSizeInBytes = 20 * 1024 * 1024;

                    if (fileInfo.Length > maxSizeInBytes)
                    {
                        MessageBox.Show("ไฟล์มีขนาดใหญ่เกิน 20 MB", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    byte[] fileData = File.ReadAllBytes(filePath);

                    SupplierAssignmentFile fileModel = new SupplierAssignmentFile
                    {
                        FileName = Path.GetFileName(filePath),
                        FileType = "application/pdf",
                        FileData = fileData,
                        UploadedAt = DateTime.Now,
                        UploadedBy = _empId
                    };

                    this.currentFile = fileModel;
                    btnInsertFile.Text = fileModel.FileName;
                }
            }
        }

        private void cmbSelectPhase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSelectPhase.SelectedIndex > 0 && cmbSelectPhase.SelectedItem is DataRowView row)
                txtPhaseDetail.Text = row["phase_detail"].ToString();
            else
                txtPhaseDetail.Text = "";
        }

        private void ShowPDFPreviewFromBytes(byte[] pdfBytes, Control targetControl)
        {
            if (pdfBytes == null) return;

            if (pdfPreviewForm == null || pdfPreviewForm.IsDisposed)
            {
                pdfPreviewForm = new FormPDFPreview(pdfBytes);
                var location = targetControl.PointToScreen(new Point(0, targetControl.Height));
                pdfPreviewForm.Location = location;
                pdfPreviewForm.Show();
            }

            StartHoverTimer(targetControl);
        }

        private void StartHoverTimer(Control buttonControl)
        {
            if (hoverCheckTimer == null)
            {
                hoverCheckTimer = new Timer();
                hoverCheckTimer.Interval = 300;
                hoverCheckTimer.Tick += (s, e) =>
                {
                    Point mousePos = Cursor.Position;

                    bool overButton = buttonControl.Bounds.Contains(buttonControl.Parent.PointToClient(mousePos));
                    bool overPreview = pdfPreviewForm != null && !pdfPreviewForm.IsDisposed && pdfPreviewForm.Bounds.Contains(mousePos);

                    if (!overButton && !overPreview)
                    {
                        if (pdfPreviewForm != null && !pdfPreviewForm.IsDisposed)
                        {
                            pdfPreviewForm.Close();
                            pdfPreviewForm = null;
                        }
                        hoverCheckTimer.Stop();
                    }
                };
            }
            hoverCheckTimer.Start();
        }

        private void btnInsertFile_MouseEnter(object sender, EventArgs e)
        {
            if (currentFile?.FileData != null)
            {
                ShowPDFPreviewFromBytes(currentFile.FileData, btnInsertFile);
            }
        }

        private DateTime CalculateDueDate(DateTime start, int workingDays)
        {
            int added = 0;
            DateTime current = start;

            while (added < workingDays)
            {
                if (current.DayOfWeek != DayOfWeek.Sunday)
                {
                    added++;
                    if (added == workingDays) break;
                }
                current = current.AddDays(1);
            }

            if (current.DayOfWeek == DayOfWeek.Sunday)
                current = current.AddDays(1);

            return current;
        }

        private void UpdateDueDate()
        {
            int workDays;
            if (int.TryParse(txtDate.Text, out workDays) && workDays > 0)
            {
                DateTime start = startDate.Value.Date;
                DateTime due = CalculateDueDate(start, workDays);

                if (due < start) due = start;

                dueDate.MinDate = start;
                dueDate.Value = due;
            }
            else
            {
                dueDate.MinDate = startDate.Value.Date;
                dueDate.Value = startDate.Value.Date;
            }
        }

        private void ApplyDateGuards()
        {
            dueDate.MinDate = startDate.Value.Date;

            if (dueDate.Value.Date < dueDate.MinDate.Date)
                dueDate.Value = dueDate.MinDate;
        }

        private void startDate_ValueChanged(object sender, EventArgs e)
        {
            ApplyDateGuards();
            UpdateDueDate();
        }

        private void dueDate_ValueChanged(object sender, EventArgs e)
        {
            if (dueDate.Value.Date < startDate.Value.Date)
                dueDate.Value = startDate.Value.Date;
        }

        private void txtDate_TextChanged(object sender, EventArgs e)
        {
            UpdateDueDate();
        }
    }
}
