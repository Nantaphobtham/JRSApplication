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

        public UpdateProjectPhase()
        {
            InitializeComponent();
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
            PhaseWorking phase = phaseWorkDAL.GetPhaseWorkingByPhaseId( phaseId);

            if (phase != null)
            {
                dtpkDate.Value = phase.WorkDate;
                cmbStatus.Text = phase.WorkStatus;
                txtWorkingDescription.Text = phase.WorkDetail;
                txtRemark.Text = phase.Remark;
            }
            else
            {
                // Clear fields
                dtpkDate.Value = DateTime.Now;
                cmbStatus.Text = "";
                txtWorkingDescription.Text = "";
                txtRemark.Text = "";
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
                projectID = searchForm.SelectedID; // เก็บไว้ใช้ตอนบันทึก

                txtProjectID.Text = searchForm.SelectedID;
                txtProjectName.Text = searchForm.SelectedName;

                LoadPhasesToComboBox(projectID);
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
                    WorkStatus = cmbStatus.Text,  // or SelectedValue if you use ValueMember
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
    }
}
