using JRSApplication.Components;
using JRSApplication.Components.Models;
using JRSApplication.Components.Service;
using JRSApplication.Data_Access_Layer;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class DetermineSubcontractors : UserControl
    {
        //  เก็บ ID ต่าง ๆ ที่ใช้ขณะทำงาน
        private string supplierID = "";
        private string projectID = "";
        private string currentAssignmentId = ""; // ใช้สำหรับ Edit/Delete
        private bool isEditing = false; // ตรวจสอบว่าอยู่ในโหมดแก้ไขหรือไม่
        private SupplierAssignmentFile currentFile; // เก็บไฟล์ PDF ที่แนบล่าสุด
        private string _empId; // เก็บ empId ของ user ที่ login อยู่

        private FormPDFPreview pdfPreviewForm = null; // ✅ ฟอร์ม Preview
        private Timer hoverCheckTimer;                // ✅ Timer เช็กเมาส์ออก

        public DetermineSubcontractors(string empId)
        {
            InitializeComponent();
            _empId = empId;
            CustomizeDataGridViewAssignment();
            LoadAssignments(); // ปิดฟิลด์ทั้งหมดไม่ให้แก้ไขก่อน
        }

        //  โหลดข้อมูลเฟสของโครงการลง ComboBox
        private void LoadPhasesToComboBox(string projectId)
        {
            SearchService service = new SearchService();
            DataTable dt = service.GetPhasesByProjectId(projectId);

            // เพิ่ม row สำหรับ 'เลือกเฟส'
            DataRow dr = dt.NewRow();
            dr["phase_id"] = DBNull.Value;     // หรือ 0 ก็ได้ (แต่ DBNull.Value จะปลอดภัยกว่า)
            dr["phase_no"] = "-- เลือกเฟส --";
            dt.Rows.InsertAt(dr, 0);           // เพิ่มเป็น index 0

            cmbSelectPhase.DisplayMember = "phase_no";    // แสดงหมายเลขเฟส
            cmbSelectPhase.ValueMember = "phase_id";      // เก็บค่า id ตอน save
            cmbSelectPhase.DataSource = dt;               // ใส่ data ลง combobox
            cmbSelectPhase.SelectedIndex = 0;             // ให้ชี้มาที่ "เลือกเฟส" ทันที
        }

        // โหลดรายการทั้งหมดจาก DB
        private void LoadAssignments()
        {
            SupplierWorkAssignmentDAL dal = new SupplierWorkAssignmentDAL();
            dtgvAssignment.DataSource = dal.GetAllAssignmentsWithPhase();
        }

        // ปุ่มค้นหาโครงการ
        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Project");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                projectID = searchForm.SelectedID; // เก็บ id ไว้ใช้ตอน save
                txtPorjectID.Text = searchForm.SelectedID;
                txtProjectName.Text = searchForm.SelectedName;
                txtContractnumber.Text = searchForm.SelectedContract;
                LoadPhasesToComboBox(projectID); // โหลด phase มาให้เลือก
            }
        }

        // ปุ่มค้นหาผู้รับเหมา
        private void btnSearchSupplier_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Supplier");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                supplierID = searchForm.SelectedID; // เก็บ id ไว้ใช้ตอน save
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
            if (cmbSelectPhase.SelectedIndex == -1 || cmbSelectPhase.SelectedValue == null)
            {
                MessageBox.Show("กรุณาเลือกเฟสที่ต้องการดำเนินงาน", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        // กดปุ่มบันทึกข้อมูล (ทั้งเพิ่มใหม่ และ แก้ไข)
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateBeforeSave()) return;

            SupplierWorkAssignment model = new SupplierWorkAssignment
            {
                AssignmentId = isEditing ? currentAssignmentId : "",   // ถ้าแก้ไขให้ใช้รหัสเดิม, ถ้าเพิ่มใหม่ให้ว่างไว้ เดี๋ยว generate
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

                    // ✅ จัดการไฟล์แนบ (AssignmentId เป็น string)
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
                    // ✅ สร้างรหัสใหม่ (SWO6807001...) แล้วเซ็ตให้ AssignmentId
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


        // ปุ่ม Add เปิดให้กรอกข้อมูลใหม่
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearandClossForm();
            EnableFormFields();
        }

        // ปุ่ม Edit → เปิดให้แก้ไขฟิลด์
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

        // ปุ่ม นี้จะเป็นยกเลิกการจ้างแทน
        private void btnDelete_Click(object sender, EventArgs e)
        {

            //if (string.IsNullOrEmpty(currentAssignmentId))
            //{
            //    MessageBox.Show("กรุณาเลือกรายการที่ต้องการลบ", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //DialogResult result = MessageBox.Show("คุณต้องการลบรายการนี้หรือไม่?", "ยืนยัน", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (result == DialogResult.Yes)
            //{
            //    SupplierWorkAssignmentDAL dal = new SupplierWorkAssignmentDAL();
            //    cmd.Parameters.AddWithValue("@AssignmentId", assignmentId);
            //    MessageBox.Show("ลบข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    ClearandClossForm();
            //    LoadAssignments();
            //}
        }

        // เมื่อคลิกตาราง เพื่อโหลดข้อมูลขึ้นฟอร์ม (แต่ยังไม่ให้แก้ไข)
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
                foreach (DataRowView item in cmbSelectPhase.Items)
                {
                    if (item["phase_no"].ToString() == phaseNo)
                    {
                        cmbSelectPhase.SelectedValue = item["phase_id"];
                        break;
                    }
                }

                DisableFormFields(); // ห้ามแก้ไขทันที ต้องกดปุ่ม Edit ก่อน
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
                    long maxSizeInBytes = 20 * 1024 * 1024; // 20 MB

                    if (fileInfo.Length > maxSizeInBytes)
                    {
                        MessageBox.Show("ไฟล์มีขนาดใหญ่เกิน 20 MB", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // ✅ โหลดไฟล์เป็น byte[]
                    byte[] fileData = File.ReadAllBytes(filePath);

                    // ✅ สร้าง object
                    SupplierAssignmentFile fileModel = new SupplierAssignmentFile
                    {
                        FileName = Path.GetFileName(filePath),
                        FileType = "application/pdf",
                        FileData = fileData,
                        UploadedAt = DateTime.Now,
                        UploadedBy = _empId // ใช้ empId ที่ส่งมาจาก Formณ
                                            // AssignmentId ยังไม่ต้องใส่ถ้ายังไม่ได้เลือก assignment
                    };

                    // ✅ ตัวอย่าง: แสดงชื่อไฟล์ใน TextBox หรือ Label
                    MessageBox.Show("ไฟล์เตรียมข้อมูลเรียบร้อย: " + fileModel.FileName, "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 📌 ถ้าอยากเก็บ object นี้ไว้ใช้ตอนกด Save, สามารถเก็บไว้ใน class-level field เช่น
                     this.currentFile = fileModel;
                    // 👉 เปลี่ยนชื่อปุ่มเป็นชื่อไฟล์ที่เลือก
                    btnInsertFile.Text = fileModel.FileName;
                }
            }
        }

        private void cmbSelectPhase_SelectedIndexChanged(object sender, EventArgs e)
        {
            // กรองเฉพาะถ้าเลือก phase จริง (index 0 เป็น "-- เลือกเฟส --" ไม่ต้องโชว์ detail)
            if (cmbSelectPhase.SelectedIndex > 0 && cmbSelectPhase.SelectedItem is DataRowView row)
            {
                txtPhaseDetail.Text = row["phase_detail"].ToString();
            }
            else
            {
                txtPhaseDetail.Text = ""; // หรือ "--" ถ้าอยากให้มีค่า default
            }
        }

        //เมธอดเปิดฟอร์ม Preview
        private void ShowPDFPreviewFromBytes(byte[] pdfBytes, Control targetControl)
        {
            if (pdfBytes == null) return;

            // ถ้า Form ยังไม่ได้เปิด
            if (pdfPreviewForm == null || pdfPreviewForm.IsDisposed)
            {
                // ✅ สร้างและเปิด Form ใหม่
                pdfPreviewForm = new FormPDFPreview(pdfBytes);

                // ✅ วางตำแหน่ง Form ใต้ปุ่ม
                var location = targetControl.PointToScreen(new Point(0, targetControl.Height));
                pdfPreviewForm.Location = location;

                pdfPreviewForm.Show();
            }

            StartHoverTimer(targetControl); // เริ่มเช็กเมื่อเมาส์ออก
        }
        //Timer เช็กเมาส์ "ออก" แล้วปิด Form
        private void StartHoverTimer(Control buttonControl)
        {
            if (hoverCheckTimer == null)
            {
                hoverCheckTimer = new Timer();
                hoverCheckTimer.Interval = 300;
                hoverCheckTimer.Tick += (s, e) =>
                {
                    Point mousePos = Cursor.Position;

                    // ✅ เช็กเมาส์อยู่บนปุ่ม
                    bool overButton = buttonControl.Bounds.Contains(buttonControl.Parent.PointToClient(mousePos));

                    // ✅ เช็กเมาส์อยู่บนฟอร์ม Preview
                    bool overPreview = pdfPreviewForm != null && !pdfPreviewForm.IsDisposed && pdfPreviewForm.Bounds.Contains(mousePos);

                    // ❌ ถ้าเมาส์อยู่นอกทั้ง 2 จุด ให้ปิดฟอร์ม
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

            // ถ้า dueDate ตรงกับวันอาทิตย์ → ขยับเป็นวันจันทร์
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
                dueDate.Value = due;
            }
            else
            {
                dueDate.Value = startDate.Value;
            }
        }
        private void txtDate_TextChanged(object sender, EventArgs e)
        {
            UpdateDueDate();
        }

        private void startDate_ValueChanged(object sender, EventArgs e)
        {
            UpdateDueDate();
        }
    }
}
