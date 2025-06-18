using JRSApplication.Components;
using JRSApplication.Components.Models;
using JRSApplication.Data_Access_Layer;
using Mysqlx.Crud;
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
        //เก็บรูปภาพ 
        //private List<WorkingPicture> uploadedPictures = new List<WorkingPicture>();

        private SupplierWorkAssignmentDAL supplierWorkAssignmentDAL = new SupplierWorkAssignmentDAL();

        private string projectID; // ✅ add this line here

        private byte[] selectedImageBytes = null;
        private string selectedImagePath = null;
        private WorkingPictureDAL pictureDAL = new WorkingPictureDAL();



        public UpdateProjectPhase()
        {
            InitializeComponent();

            cmbStatus.DataSource = WorkStatus.AllStatuses
                .Select(status => new { Value = status, Display = WorkStatus.GetDisplayName(status) })
                .ToList();

            cmbStatus.DisplayMember = "Display";
            cmbStatus.ValueMember = "Value";

        }

        private void LoadSupplierAssignment(int phaseId)
        {
            SupplierWorkAssignment assignment = supplierWorkAssignmentDAL.GetAssignmentByPhaseId(phaseId);

            if (assignment != null)
            {
                // Fill Supplier fields
                txtSupplierName.Text = assignment.SupId; // You can JOIN to show Supplier Name if needed
                txtSupplierJuristic.Text = ""; // If you JOIN, show Personal ID here
                txtWorkingDescription.Text = assignment.AssignDescription;

                // Select "มี" if supplier is assigned
                checkeSupplier.ClearSelected();
                if (!string.IsNullOrEmpty(assignment.SupId))
                {
                    checkeSupplier.SetSelected(0, true); // Select "มี"
                }
                else
                {
                    checkeSupplier.SetSelected(1, true); // Select "ไม่"
                }
            }
            else
            {
                // Clear fields
                txtSupplierName.Text = "";
                txtSupplierJuristic.Text = "";
                txtWorkingDescription.Text = "";

                // Default to "ไม่"
                checkeSupplier.ClearSelected();
                checkeSupplier.SetSelected(1, true); // Select "ไม่"
            }
        }
        private PhaseWorkDAL phaseWorkDAL = new PhaseWorkDAL();

        private void LoadPhaseWorking(int projectId, int phaseId)
        {
            PhaseWorking phase = phaseWorkDAL.GetPhaseWorkingByPhaseId(phaseId);

            if (phase != null)
            {
                dtpkDate.Value = phase.WorkDate;
                cmbStatus.SelectedValue = phase.WorkStatus;
                txtWorkingDescription.Text = phase.WorkDetail;
                txtRemark.Text = phase.Remark;

                // ✅ แสดงสถานะเฟสในกล่องด้านขวาบน (TextBox สีเหลือง)
                txtPhaseStatus.Text = WorkStatus.GetDisplayName(phase.WorkStatus);
            }
            else
            {
                // Clear fields
                dtpkDate.Value = DateTime.Now;
                cmbStatus.SelectedIndex = -1;
                txtWorkingDescription.Text = "";
                txtRemark.Text = "";
                txtPhaseStatus.Text = "";
            }
        }

        private void cmbPhase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSelectPhase.SelectedValue != null && int.TryParse(cmbSelectPhase.SelectedValue.ToString(), out int phaseId))
            {
                // You need to also know projectId:
                PhaseWorking phase = phaseWorkDAL.GetPhaseWorkingByPhaseId(phaseId);

                LoadSupplierAssignment(phaseId);
                //LoadPhaseWorking(projectId, phaseId);
            }
        }
        private void LoadPhasesToComboBox(string projectId)
        {
            SearchService service = new SearchService();
            DataTable dt = service.GetPhasesByProjectId(projectId);

            cmbSelectPhase.DisplayMember = "phase_no";   // ✅ แสดงเลขเฟส
            cmbSelectPhase.ValueMember = "phase_id";     // ✅ ใช้ phase_id ตอนบันทึก
            cmbSelectPhase.DataSource = dt;
        }

        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm("Project");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                projectID = searchForm.SelectedID;

                txtProjectID.Text = searchForm.SelectedID;
                txtProjectName.Text = searchForm.SelectedName;

                LoadPhasesToComboBox(projectID);

                DataTable workHistory = phaseWorkDAL.GetWorkDetailsByProjectId(projectID);
                dtgvWhorHistory.DataSource = workHistory;
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 1️⃣ Prepare PhaseWorking object
                PhaseWorking phase = new PhaseWorking
                {
                    PhaseID = Convert.ToInt32(cmbSelectPhase.SelectedValue),
                    WorkDate = dtpkDate.Value,
                    WorkStatus = cmbStatus.SelectedValue.ToString(),  // or SelectedValue if you use ValueMember
                    WorkDetail = txtWorkingDescription.Text,
                    Remark = txtRemark.Text,
                    UpdateDate = DateTime.Now  // Optional: record update timestamp
                };

                // 2️⃣ Save to database
                PhaseWorkDAL phaseDAL = new PhaseWorkDAL();

                // Example: Check if record already exists (Update), else Insert
                bool success = phaseDAL.InsertPhaseWorking(phase);
                // 3️⃣ Show result
                if (success)
                {
                    MessageBox.Show("✅ บันทึกข้อมูลเรียบร้อย", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("❌ บันทึกข้อมูลไม่สำเร็จ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Select an Image"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                long maxSize = 20 * 1024 * 1024; // 20MB

                if (fileInfo.Length > maxSize)
                {
                    MessageBox.Show("ไฟล์รูปภาพต้องไม่เกิน 20MB", "ขนาดใหญ่เกินไป", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                selectedImagePath = openFileDialog.FileName;
                selectedImageBytes = File.ReadAllBytes(selectedImagePath);
                pictureBoxPreview.Image = Image.FromFile(selectedImagePath);
            }
        }


        private void btnAddImage_Click(object sender, EventArgs e)
        {
            if (selectedImageBytes == null)
            {
                MessageBox.Show("กรุณาเลือกรูปภาพก่อน", "ยังไม่ได้เลือกรูปภาพ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Step 1: Get selected phaseId
            if (cmbSelectPhase.SelectedValue == null || !int.TryParse(cmbSelectPhase.SelectedValue.ToString(), out int phaseId))
            {
                MessageBox.Show("กรุณาเลือกเฟสงานก่อน", "ไม่มีข้อมูลเฟส", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Step 2: Find work_id from phaseId
            int? workId = phaseWorkDAL.GetWorkIdByPhaseId(phaseId);
            if (workId == null)
            {
                MessageBox.Show("ไม่พบ work_id สำหรับ phase นี้", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Step 3: Create picture object
            WorkingPicture picture = new WorkingPicture
            {
                WorkID = workId.Value,
                PicNo = 1,
                PicName = Path.GetFileName(selectedImagePath),
                Description = txtPictureDescription.Text,
                PictureData = selectedImageBytes,
                CreatedAt = DateTime.Now
            };

            // Step 4: Save to database
            try
            {
                bool success = pictureDAL.InsertPicture(picture);

                if (success)
                {
                    MessageBox.Show("✅ บันทึกรูปภาพเรียบร้อย", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Step 5: Show image on the left
                    using (MemoryStream ms = new MemoryStream(picture.PictureData))
                    {
                        pictureBoxListDisplay.Image = Image.FromStream(ms);
                    }

                    // Step 6: Reset form
                    pictureBoxPreview.Image = null;
                    selectedImageBytes = null;
                    selectedImagePath = null;
                    txtPictureDescription.Clear();
                }
                else
                {
                    MessageBox.Show("❌ ไม่สามารถบันทึกรูปภาพได้", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





    }
}
