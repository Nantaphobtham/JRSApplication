using JRSApplication.Data_Access_Layer;
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


namespace JRSApplication.Components.Service
{
    public partial class CheckphaseWorking : Form
    {
        private string _workId; // Work ID ที่ส่งมาจาก ProjectPhaseListRequestsforApproval.cs
        private int _phaseId; // Phase ID ที่ส่งมาจาก ProjectPhaseListRequestsforApproval.cs
        private int _projectId; 
        public CheckphaseWorking(string workId, int phaseId, int projectId)
        {
            InitializeComponent();
            _workId = workId;
            _phaseId = phaseId;
            _projectId = projectId;
            LoadFullData();
        }
        private void LoadFullData()
        {
            var dal = new PhaseWorkDAL();
            var dataRows = dal.GetFullPhaseWorkingByPhaseId(_phaseId);

            if (dataRows == null || dataRows.Count == 0) return;

            // 👉 แสดง Project / Phase
            var first = dataRows[0];
            txtProjectID.Text = first.ProjectID.ToString();
            txtProjectname.Text = first.ProjectName;
            txtProjectNumber.Text = first.ProjectNumber;
            txtEmployee.Text = first.EmployeeName;
            txtCustomer.Text = first.CustomerName;
            txtPhaseNo.Text = first.PhaseNo.ToString();
            txtPhaseDetail.Text = first.PhaseDetail;

            // 👉 จัดกลุ่มงานจากข้อมูลแถวเดียว (group by work_id)
            var workGroups = dataRows
                .GroupBy(r => r.WorkID)
                .Select(g => new
                {
                    WorkID = g.Key,
                    Work = g.First(),
                    Pictures = g.Where(x => x.PicData != null).ToList()
                }).ToList();

            // 👉 Loop สร้าง UI
            int currentY = 10;
            foreach (var workItem in workGroups)
            {
                currentY += CreateWorkSection(workItem.Work, workItem.Pictures, currentY) + 20;
            }
        }
        private int CreateWorkSection(
                    JRSApplication.Data_Access_Layer.PhaseWorkDAL.PhaseWorkingFullRow work,
                    List<JRSApplication.Data_Access_Layer.PhaseWorkDAL.PhaseWorkingFullRow> pictures,
                    int yOffset)
        {
            int baseHeight = 540;
            int additionalHeightPerPicture = 250;
            int pictureCount = pictures.Count;
            int sectionHeight = baseHeight;

            if (pictureCount > 2)
            {
                sectionHeight += (pictureCount - 2) * additionalHeightPerPicture;
            }

            Panel container = new Panel
            {
                Location = new Point(0, yOffset),
                Size = new Size(787, sectionHeight),
                BorderStyle = BorderStyle.FixedSingle
            };

            Font f = new Font("Segoe UI", 15.75f, FontStyle.Regular);

            // 🔹 WorkID
            container.Controls.Add(CreateLabel("รหัสงาน", new Point(40, 10), f));
            container.Controls.Add(CreateTextbox(work.WorkID, new Point(121, 8), new Size(237, 35), f));

            // 🔹 Status
            container.Controls.Add(CreateLabel("สถานะงาน", new Point(20, 52), f));
            container.Controls.Add(CreateTextbox(work.WorkStatus, new Point(121, 50), new Size(237, 35), f));

            // 🔹 Start/End Dates
            container.Controls.Add(CreateLabel("วันที่เริ่ม", new Point(457, 10), f));
            container.Controls.Add(CreateTextbox(work.WorkDate.ToString("yyyy-MM-dd"), new Point(538, 8), new Size(237, 35), f));

            container.Controls.Add(CreateLabel("วันที่สิ้นสุด", new Point(439, 57), f));
            container.Controls.Add(CreateTextbox(work.WorkEndDate?.ToString("yyyy-MM-dd") ?? "", new Point(538, 52), new Size(237, 35), f));

            // 🔹 รายละเอียด
            container.Controls.Add(CreateLabel("รายละเอียด\nการดำเนินงาน", new Point(7, 102), f, ContentAlignment.MiddleRight));
            container.Controls.Add(CreateMultilineTextbox(work.WorkDetail, new Point(142, 99), new Size(633, 65), f));

            // 🔹 หมายเหตุ
            container.Controls.Add(CreateLabel("หมายเหตุ", new Point(48, 181), f));
            container.Controls.Add(CreateMultilineTextbox(work.WorkRemark, new Point(142, 164), new Size(633, 65), f));

            // 🔹 รูปภาพและคำอธิบาย
            int px = 17, py = 240;
            int count = 0;

            foreach (var pic in pictures)
            {
                PictureBox pb = new PictureBox
                {
                    Location = new Point(px, py),
                    Size = new Size(349, 170),
                    SizeMode = PictureBoxSizeMode.Zoom
                };

                using (var ms = new MemoryStream(pic.PicData))
                {
                    pb.Image = Image.FromStream(ms);
                }

                container.Controls.Add(pb);

                // Label
                container.Controls.Add(CreateLabel("คำอธิบายรูปภาพ", new Point(px - 5, py + 173), f));

                // Description
                container.Controls.Add(CreateMultilineTextbox(pic.PicDescription, new Point(px, py + 206), new Size(349, 62), f));

                // จัดวางแถว
                count++;
                if (count % 2 == 0)
                {
                    px = 17;
                    py += 250;
                }
                else
                {
                    px = 426;
                }
            }

            // 👉 Add เข้า panelWork
            Work.Controls.Add(container);
            return container.Height;
        }
        private Label CreateLabel(string text, Point location, Font font, ContentAlignment align = ContentAlignment.MiddleLeft)
        {
            return new Label
            {
                Text = text,
                Location = location,
                Font = font,
                AutoSize = true,
                TextAlign = align
            };
        }

        private TextBox CreateTextbox(string text, Point location, Size size, Font font)
        {
            return new TextBox
            {
                Text = text,
                Location = location,
                Size = size,
                Font = font,
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private TextBox CreateMultilineTextbox(string text, Point location, Size size, Font font)
        {
            return new TextBox
            {
                Text = text,
                Location = location,
                Size = size,
                Font = font,
                ReadOnly = true,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            var dal = new PhaseWorkDAL();
            dal.UpdateWorkStatus(_workId, WorkStatus.Completed, txtRemark.Text);
            MessageBox.Show("อนุมัติผลการทำงานเรียบร้อยแล้ว ✔️", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close(); // ปิดฟอร์มถ้าต้องการ
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            var dal = new PhaseWorkDAL();
            dal.UpdateWorkStatus(_workId, WorkStatus.Rejected, txtRemark.Text);
            MessageBox.Show("ไม่อนุมัติผลการทำงาน ❌", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            this.Close(); // ปิดฟอร์มถ้าต้องการ
        }
    }
}
