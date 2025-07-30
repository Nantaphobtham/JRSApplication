using JRSApplication.Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication.Components.Service
{
    public partial class CheckphaseWorking : Form
    {
        private string _workId;
        private int _phaseId;
        private int _projectId;
        public CheckphaseWorking(string workId, int phaseId, int projectId)
        {
            InitializeComponent();
            _workId = workId;
            _phaseId = phaseId;
            _projectId = projectId;
        }
        private void LoadAllData()
        {
            // 👉 โหลดข้อมูล Project
            Project project = new ProjectDAL().GetProjectById(_projectId);
            if (project != null)
            {
                txtProjectID.Text = project.ProjectID.ToString();
                txtProjectname.Text = project.ProjectName;
                txtProjectNumber.Text = project.ProjectNumber;
                txtCustomer.Text = $"{project.CustomerName}";
                txtEmployee.Text = $"{project.EmployeeName}";
            }

            // 👉 โหลดข้อมูล Phase
            ProjectPhase phase = new ProjectPhaseDAL().GetPhaseById(_phaseId);
            if (phase != null)
            {
                txtPhaseNo.Text = phase.PhaseNumber.ToString();
                txtPhaseDetail.Text = phase.PhaseDetail;
            }

            // 👉 โหลดรายการงานทั้งหมดในเฟส
            List<PhaseWorking> works = new PhaseWorkingDAL().GetWorksByPhaseId(_phaseId);
            foreach (var work in works)
            {
                // โหลดรูปภาพของงาน
                work.Pictures = new WorkingPictureDAL().GetPicturesByWorkId(work.WorkID);
            }

            // 👉 แสดงใน panel3
            RenderWorks(works);
        }


    }
}
