using JRSApplication.Components;
using JRSApplication.Components.Models;
using JRSApplication.Data_Access_Layer;
using Mysqlx.Crud;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class UpdateProjectPhase : UserControl
    {
        private List<WorkingPicture> pictures = new List<WorkingPicture>();
        private List<PhaseWithStatus> phaseList = new List<PhaseWithStatus>();
        // ตัวแปรระดับคลาส
        private List<PhaseWorking> allPhaseWorkingList;
        private PhaseWorkDAL dal = new PhaseWorkDAL();
        private readonly string _empId;
        private readonly string _userRole;
        public class ComboBoxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
            public override string ToString() => Text;  // โชว์ Text ใน ComboBox
        }
        public UpdateProjectPhase(string empId, string userRole)
        {
            InitializeComponent();
            _empId = empId;
            _userRole = userRole;
            LoadAllPhaseWorking();
            CustomPhaseWorkingGrid();
            LoadStatusComboBox();
            InitGrid();
            //CustomSubcontractorGrid();
            CustomDataGridView();
            // ตั้งค่า Placeholder ให้ TextBox
            txtPictureDescription.Text = "คำอธิบายรูปภาพ";
            txtPictureDescription.ForeColor = Color.Gray;


            // Events สำหรับ Placeholder
            txtPictureDescription.Enter += (s, e) =>
            {
                if (txtPictureDescription.Text == "คำอธิบายรูปภาพ")
                {
                    txtPictureDescription.Text = "";
                    txtPictureDescription.ForeColor = Color.Black;
                }
            };
            txtPictureDescription.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtPictureDescription.Text))
                {
                    txtPictureDescription.Text = "คำอธิบายรูปภาพ";
                    txtPictureDescription.ForeColor = Color.Gray;
                }
            };

        }
        private void LoadAllPhaseWorking()
        {
            var dal = new PhaseWorkDAL();
            allPhaseWorkingList = dal.GetAllPhaseWorking(); // คืน List<PhaseWorking>
            List<PhaseWorking> allWorkings = dal.GetAllPhaseWorking();
            dtgvPhaseWorkingHistory.DataSource = null;
            dtgvPhaseWorkingHistory.DataSource = allWorkings;
        }
        //สำหรับ Filter project จะใช้ที่จุดไหน
        private void FilterPhaseWorkingByProject(IEnumerable<int> phaseIds)
        {
            var filtered = allPhaseWorkingList
                .Where(pw => phaseIds.Contains(pw.PhaseID))
                .ToList();

            dtgvPhaseWorkingHistory.DataSource = filtered;
        }
        private void CustomPhaseWorkingGrid()
        {
            var grid = dtgvPhaseWorkingHistory;
            var grid2 = dtgvDetailSubcontractorWork;

            // อย่าลืม! set AutoGenerateColumns = true ถ้า bind DataSource เป็น List/Datatable
            grid.AutoGenerateColumns = true;

            // --- ตั้งชื่อ Column Header ภาษาไทย ---
            if (grid.Columns.Contains("WorkID"))
                grid.Columns["WorkID"].HeaderText = "รหัสทำงาน";
            if (grid.Columns.Contains("ProjectID"))
                grid.Columns["ProjectID"].HeaderText = "รหัสโครงการ";
            if (grid.Columns.Contains("PhaseNo"))
                grid.Columns["PhaseNo"].HeaderText = "เฟส";
            if (grid.Columns.Contains("WorkDetail"))
                grid.Columns["WorkDetail"].HeaderText = "รายละเอียดการทำงาน";
            if (grid.Columns.Contains("WorkStatus"))
                grid.Columns["WorkStatus"].HeaderText = "สถานะ";
            if (grid.Columns.Contains("WorkDate"))
                grid.Columns["WorkDate"].HeaderText = "วันที่เริ่ม";
            if (grid.Columns.Contains("EndDate"))
                grid.Columns["EndDate"].HeaderText = "วันที่สิ้นสุด";
            if (grid.Columns.Contains("UpdateDate"))
                grid.Columns["UpdateDate"].HeaderText = "วันที่แก้ไขล่าสุด";
            if (grid.Columns.Contains("Remark"))
                grid.Columns["Remark"].HeaderText = "ปัญหาการดำเนินงาน";
            if (grid.Columns.Contains("SupplierAssignmentId"))
                grid.Columns["SupplierAssignmentId"].HeaderText = "ใบสั่งจ้าง";

            // --- ซ่อน Column ที่ไม่ต้องการให้แสดง ---
            if (grid.Columns.Contains("PhaseID"))
                grid.Columns["PhaseID"].Visible = false;

            // กำหนด Selection
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;

            // เส้นขอบ + สีพื้นหลัง + cell
            grid.BorderStyle = BorderStyle.None;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.BackgroundColor = Color.White;

            // Header
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            grid.ColumnHeadersHeight = 30;

            // Cell font, alignment, padding
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            // ขนาดคอลัมน์/แถว
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grid.RowTemplate.Height = 30;

            // Grid line สีเทา
            grid.GridColor = Color.LightGray;

            // ซ่อนแถวหัวแถวซ้ายมือ
            grid.RowHeadersVisible = false;

            // ReadOnly + ไม่อนุญาตเพิ่ม/ขยายแถว
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToResizeRows = false;
            //--------------------------------------------
            grid2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid2.MultiSelect = false;

            grid2.BorderStyle = BorderStyle.None;
            grid2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            grid2.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid2.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            grid2.DefaultCellStyle.SelectionForeColor = Color.White;
            grid2.BackgroundColor = Color.White;

            grid2.EnableHeadersVisualStyles = false;
            grid2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid2.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            grid2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid2.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            grid2.ColumnHeadersHeight = 30;

            grid2.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            grid2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid2.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            grid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grid2.RowTemplate.Height = 30;

            grid2.GridColor = Color.LightGray;
            grid2.RowHeadersVisible = false;

            grid2.ReadOnly = true;
            grid2.AllowUserToAddRows = false;
            grid2.AllowUserToResizeRows = false;
            
        }

        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            using (var searchForm = new SearchForm("Project"))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    txtProjectID.Text = searchForm.SelectedID;
                    txtProjectName.Text = searchForm.SelectedName;

                    // โหลด Phase ทั้งหมดของโครงการ
                    LoadPhaseForProject(searchForm.SelectedID);
                    // เพิ่มตรงนี้เพื่อ filter ข้อมูลเฉพาะของ Project ที่เลือก
                    // 1. phaseList จะมี Phase ของ Project นั้นๆ
                    var phaseIds = phaseList.Select(p => p.PhaseId); // ดึง PhaseId ทั้งหมดของ project
                    FilterPhaseWorkingByProject(phaseIds);
                }
            }
        }

        private void LoadPhaseForProject(string projectId)
        {
            PhaseWorkDAL phaseWorkDAL = new PhaseWorkDAL();
            phaseList = phaseWorkDAL.GetPhasesWithStatusAndAssignments(Convert.ToInt32(projectId));

            var comboSource = new List<object>
                        {
                            new { phase_id = 0, phase_no = "-- เลือกเฟส --" }
                        };
            comboSource.AddRange(
                phaseList.Select(p => new { phase_id = p.PhaseId, phase_no = $"เฟส {p.PhaseNumber}" })
                );
            cmbSelectPhase.DataSource = comboSource;
            cmbSelectPhase.DisplayMember = "phase_no";
            cmbSelectPhase.ValueMember = "phase_id";
            cmbSelectPhase.SelectedIndex = 0;
        }
        private void cmbSelectPhase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSelectPhase.SelectedIndex <= 0 || cmbSelectPhase.SelectedValue == null)
            {
                txtPhaseStatus.Text = "";
                txtPhaseStatus.BackColor = Color.Yellow;
                dtgvDetailSubcontractorWork.DataSource = null;
                return;
            }
            int phaseId = Convert.ToInt32(cmbSelectPhase.SelectedValue);

            // --- ดึง work_status จาก phase_working ---
            var phaseWorkDal = new PhaseWorkDAL();
            string workStatus = phaseWorkDal.GetWorkStatusByPhaseId(phaseId);

            if (!string.IsNullOrEmpty(workStatus))
            {
                txtPhaseStatus.Text = WorkStatus.GetDisplayName(workStatus);
                txtPhaseStatus.BackColor = WorkStatus.GetStatusColor(workStatus);
                txtPhaseStatus.ForeColor = Color.Black;
            }
            else
            {
                // ถ้าไม่มี work_status ใช้ PhaseStatus ปกติ
                var selectedPhase = phaseList.FirstOrDefault(p => p.PhaseId == phaseId);
                if (selectedPhase != null)
                {
                    txtPhaseStatus.Text = WorkStatus.GetDisplayName(selectedPhase.PhaseStatus);
                    txtPhaseStatus.BackColor = WorkStatus.GetStatusColor(selectedPhase.PhaseStatus);
                    txtPhaseStatus.ForeColor = Color.Black;
                }
                else
                {
                    txtPhaseStatus.Text = "ไม่พบสถานะ";
                    txtPhaseStatus.BackColor = Color.Gray;
                    txtPhaseStatus.ForeColor = Color.White;
                }
            }

            // --- โหลดงานของซัพพลายเออร์ ---
            LoadSupplierAssignmentsForPhase(phaseId);
        }
        //สำคัญ 
        private void LoadSupplierAssignmentsForPhase(int phaseId)
        {
            var dal = new PhaseWorkDAL();
            DataTable dt = dal.GetAssignmentsByPhase(phaseId);
            dtgvDetailSubcontractorWork.DataSource = dt;
            dtgvDetailSubcontractorWork.ClearSelection(); // <-- ไม่มีแถวถูกเลือกจนกว่าจะคลิก
        }
        //โหลด status
        private void LoadStatusComboBox()
        {
            cmbWorkstatusOfSupplier.Items.Clear();
            cmbWorkStatus.Items.Clear();

            var defaultItem = new ComboBoxItem { Text = "-- เลือกสถานะ --", Value = "" };
            cmbWorkstatusOfSupplier.Items.Add(defaultItem);
            cmbWorkStatus.Items.Add(defaultItem);

            // เฉพาะของ PM
            var pmStatuses = new List<string> { WorkStatus.InProgress, WorkStatus.Completed };

            foreach (var status in pmStatuses)
            {
                cmbWorkstatusOfSupplier.Items.Add(new ComboBoxItem
                {
                    Text = WorkStatus.GetDisplayName(status),
                    Value = status
                });
                cmbWorkStatus.Items.Add(new ComboBoxItem
                {
                    Text = WorkStatus.GetDisplayName(status),
                    Value = status
                });
            }

            cmbWorkstatusOfSupplier.SelectedIndex = 0;
            cmbWorkStatus.SelectedIndex = 0;
            cmbWorkstatusOfSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWorkStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private string GetSelectedStatus(ComboBox cmb)
        {
            if (cmb.SelectedItem is ComboBoxItem item)
                return item.Value;
            return null;
        }
        //------------------------------------------------------------------------------------------------------------------------------
        //บันทึกข้อมูล
        private bool ValidateBeforeSave()
        {
            if (pictures == null || pictures.Count == 0)
            {
                MessageBox.Show("กรุณาแนบรูปภาพอย่างน้อย 1 รูป", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (dtgvDetailSubcontractorWork.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกงานที่ต้องการบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtProjectID.Text))
            {
                MessageBox.Show("กรุณาเลือกโครงการ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cmbSelectPhase.SelectedIndex <= 0 || cmbSelectPhase.SelectedValue == null)
            {
                MessageBox.Show("กรุณาเลือกเฟส", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtWorkingDescription.Text))
            {
                MessageBox.Show("กรุณากรอกรายละเอียดการดำเนินงาน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (dtpkDate.Value == null)
            {
                MessageBox.Show("กรุณาเลือกวันที่", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cmbWorkstatusOfSupplier.SelectedIndex <= 0)
            {
                MessageBox.Show("กรุณาเลือกสถานะงานผู้รับเหมาช่วง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cmbWorkStatus.SelectedIndex <= 0)
            {
                MessageBox.Show("กรุณาเลือกสถานะเฟสงาน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // เพิ่ม validate กรณีเปลี่ยน Completed → InProgress
            string oldAssignStatus = ""; // อ่านค่าจาก grid หรือ field ที่เก็บไว้
            if (dtgvDetailSubcontractorWork.SelectedRows.Count > 0)
            {
                oldAssignStatus = dtgvDetailSubcontractorWork.SelectedRows[0].Cells["สถานะงาน"].Value?.ToString();
            }
            string newAssignStatus = GetSelectedStatus(cmbWorkstatusOfSupplier);

            if (oldAssignStatus == WorkStatus.Completed && newAssignStatus == WorkStatus.InProgress)
            {
                var result = MessageBox.Show(
                    "ยืนยันการเปลี่ยนสถานะจาก 'เสร็จสมบูรณ์' (Completed) กลับไปเป็น 'กำลังดำเนินการ' (InProgress) หรือไม่?\n\nกรณีนี้ควรใช้เมื่อแก้ไขข้อมูลย้อนหลังเท่านั้น!",
                    "ยืนยันการเปลี่ยนสถานะ",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (result == DialogResult.No)
                {
                    return false;
                }
            }

            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateBeforeSave()) return;

            var dal = new PhaseWorkDAL();

            // เตรียมข้อมูล
            int phaseId = Convert.ToInt32(cmbSelectPhase.SelectedValue);
            string workStatus = GetSelectedStatus(cmbWorkStatus);
            string supplierAssignmentId = dtgvDetailSubcontractorWork.SelectedRows[0].Cells["เลขที่งาน"].Value.ToString();
            string assignStatus = GetSelectedStatus(cmbWorkstatusOfSupplier);

            // เช็ค role
            string insertStatus = workStatus;
            if (_userRole == "Sitesupervisor" && workStatus == WorkStatus.Completed)
                insertStatus = "Waiting";

            // 1. Update supplier_work_assignment
            dal.UpdateSupplierWorkAssignmentStatus(supplierAssignmentId, assignStatus);

            // 2. Insert phase_working
            var phaseWork = new PhaseWorking
            {
                //WorkID = $"HW{DateTime.Now:yyMM}{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}",
                PhaseID = phaseId,
                WorkDetail = txtWorkingDescription.Text,
                WorkStatus = insertStatus,
                WorkDate = dtpkDate.Value.Date,
                EndDate = (insertStatus == WorkStatus.Completed || insertStatus == "Waiting") ? dtpkDate.Value.Date : (DateTime?)null,
                Remark = txtRemark.Text,
                SupplierAssignmentId = supplierAssignmentId
            };
            dal.InsertPhaseWorking(phaseWork);

            // --- เรียก ClearFormAfterSave ---
            ClearFormAfterSave();

            // --- Reload ประวัติการทำงาน ---
            LoadAllPhaseWorking();

            MessageBox.Show("บันทึกข้อมูลสำเร็จ!");
        }

        private void dtgvDetailSubcontractorWork_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // อ่านข้อมูลของ row ที่ถูกคลิก
                var selectedRow = dtgvDetailSubcontractorWork.Rows[e.RowIndex];
                // นำค่าไป process ตามที่ต้องการ เช่น แก้ status
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------
        //path Edit
        private void btnEdit_Click(object sender, EventArgs e)
        {

        }


        //------------------------------------------------------------------------------------------------------------------------------
        //ข้อมูล insrt picture
        private void btnInsertPicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "เลือกไฟล์รูปภาพ";
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    pictureBoxPreview.Image = Image.FromFile(dlg.FileName);
                    pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        }
        private void InitGrid()
        {
            dtgvPicturelist.AutoGenerateColumns = false;
            dtgvPicturelist.Columns.Clear();

            // ลำดับ
            dtgvPicturelist.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PicNo",
                HeaderText = "ลำดับ",
                DataPropertyName = "PicNo",
                Width = 50
            });

            // รูปภาพ (Thumbnail)
            DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
            imgCol.Name = "PictureData";
            imgCol.HeaderText = "รูปภาพ";
            imgCol.DataPropertyName = "PictureThumbnail";
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imgCol.Width = 100;
            dtgvPicturelist.Columns.Add(imgCol);

            // คำอธิบาย
            dtgvPicturelist.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "คำอธิบาย",
                DataPropertyName = "Description",
                Width = 200
            });

            dtgvPicturelist.DataSource = null;
        }
        private void btnAddPicture_Click(object sender, EventArgs e)
        {
            if (pictureBoxPreview.Image == null)
            {
                MessageBox.Show("กรุณาเลือกรูปภาพก่อน");
                return;
            }

            // แปลงภาพเป็น byte[]
            byte[] imageBytes;
            using (var ms = new MemoryStream())
            {
                pictureBoxPreview.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imageBytes = ms.ToArray();
            }

            // เพิ่มข้อมูลลง List
            var pic = new WorkingPicture
            {
                PicNo = pictures.Count + 1,
                PictureData = imageBytes,
                Description = txtPictureDescription.Text == "คำอธิบายรูปภาพ" ? "" : txtPictureDescription.Text,
                // PicName, CreatedAt ใส่เพิ่มได้
            };
            pictures.Add(pic);

            // เคลียร์รูปตัวอย่าง
            if (pictureBoxPreview.Image != null)
            {
                pictureBoxPreview.Image.Dispose();
                pictureBoxPreview.Image = null;
            }

            // เคลียร์กล่องคำอธิบาย (รีเซ็ต placeholder)
            txtPictureDescription.Text = "คำอธิบายรูปภาพ";
            txtPictureDescription.ForeColor = Color.Gray;
            // อัปเดต Grid
            RefreshPictureGrid();
        }

        private void RefreshPictureGrid()
        {
            var listForGrid = pictures.Select(p => new
            {
                p.PicNo,
                PictureThumbnail = ByteArrayToImage(p.PictureData),
                p.Description
            }).ToList();

            dtgvPicturelist.DataSource = null;
            dtgvPicturelist.DataSource = listForGrid;
        }

        private Image ByteArrayToImage(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }

        private void CustomDataGridView()
        {
            dtgvPicturelist.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvPicturelist.BorderStyle = BorderStyle.None;
            dtgvPicturelist.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvPicturelist.EnableHeadersVisualStyles = false;

            // Header
            dtgvPicturelist.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvPicturelist.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvPicturelist.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvPicturelist.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvPicturelist.ColumnHeadersHeight = 42;

            // Row สีหลัก (ขาว) + สีสลับ (LightGray)
            dtgvPicturelist.DefaultCellStyle.BackColor = Color.White;
            dtgvPicturelist.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // ตัวอักษร Row
            dtgvPicturelist.DefaultCellStyle.ForeColor = Color.Black;
            dtgvPicturelist.DefaultCellStyle.Font = new Font("Segoe UI", 13, FontStyle.Regular);
            dtgvPicturelist.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // สีตอนเลือก
            dtgvPicturelist.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvPicturelist.DefaultCellStyle.SelectionForeColor = Color.White;

            // กรอบ/ตาราง
            dtgvPicturelist.GridColor = Color.LightGray;

            // ขนาด Column/Row
            dtgvPicturelist.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvPicturelist.RowTemplate.Height = 42;

            // อื่นๆ
            dtgvPicturelist.RowHeadersVisible = false;
            dtgvPicturelist.ReadOnly = true;
            dtgvPicturelist.MultiSelect = false;
            dtgvPicturelist.AllowUserToAddRows = false;
            dtgvPicturelist.AllowUserToResizeRows = false;
        }
        //--------------------------------------------------------------------------------------------------
        // clear form 
        private void ClearFormAfterSave()
        {
            // ล้างข้อมูลใน controls ที่เกี่ยวข้อง
            txtProjectID.Text = "";
            txtProjectName.Text = "";
            cmbSelectPhase.SelectedIndex = 0;
            txtPhaseStatus.Text = "";
            txtPhaseStatus.BackColor = Color.Yellow;

            dtgvDetailSubcontractorWork.DataSource = null;
            cmbWorkstatusOfSupplier.SelectedIndex = 0;
            cmbWorkStatus.SelectedIndex = 0;

            txtWorkingDescription.Text = "";
            dtpkDate.Value = DateTime.Now;
            txtRemark.Text = "";

            // ล้างรูปภาพ
            pictures.Clear();
            RefreshPictureGrid();

            // ล้างรูปตัวอย่าง (Preview)
            if (pictureBoxPreview.Image != null)
            {
                pictureBoxPreview.Image.Dispose();
                pictureBoxPreview.Image = null;
            }

            txtPictureDescription.Text = "คำอธิบายรูปภาพ";
            txtPictureDescription.ForeColor = Color.Gray;

            // เคลียร์ selection ใน DataGridView
            dtgvPhaseWorkingHistory.ClearSelection();
        }

    }
}
