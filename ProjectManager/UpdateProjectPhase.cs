using JRSApplication.Components;
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

namespace JRSApplication
{
    public partial class UpdateProjectPhase : UserControl
    {


        public UpdateProjectPhase()
        {
            InitializeComponent();
            LoadProjectData();
            LoadWorkStatuses();
        }

        private void LoadPhaseStatus(int projectId, int phaseNo)
        {
            PhaseWorkDAL dal = new PhaseWorkDAL();
            var phase = dal.GetPhaseWorking(projectId, phaseNo); // ✅ ฟังก์ชันนี้คุณต้องมีใน DAL (จะเขียนให้ด้านล่าง)

            if (phase != null)
            {
                txtPhaseStatus.Text = WorkStatus.GetDisplayName(phase.WorkStatus);
                txtPhaseStatus.BackColor = WorkStatus.GetStatusColor(phase.WorkStatus);
            }
            else
            {
                txtPhaseStatus.Text = "รอเริ่มดำเนินการ";
                txtPhaseStatus.BackColor = Color.LightGray;
            }
        }


        private void LoadWorkStatuses()
        {
            var allowedStatuses = new List<string>
            {
                WorkStatus.InProgress,
                WorkStatus.Completed
            };

            var items = allowedStatuses
                .Select(status => new
                {
                    Display = WorkStatus.GetDisplayName(status), // แสดงชื่อภาษาไทย
                    Value = status                                // ใช้ค่าจริงเก็บในฐานข้อมูล
                }).ToList();

            cmbPhaseStatus.DataSource = items;
            cmbPhaseStatus.DisplayMember = "Display";
            cmbPhaseStatus.ValueMember = "Value";
            cmbPhaseStatus.SelectedIndex = 0;
        }

        //ข้อมูลโปรเจค
        private void LoadProjectData()
        {
            InitializeDataGridViewProject(); // ✅ ตรวจสอบตารางก่อนโหลดข้อมูล

            ProjectDAL dal = new ProjectDAL();
            List<Project> projects = dal.GetAllProjects();

            dtgvProjectData.Rows.Clear();
            foreach (var project in projects)
            {
                dtgvProjectData.Rows.Add(
                    project.ProjectID,
                    project.ProjectName,
                    project.ProjectStart.ToString("dd/MM/yyyy"),
                    project.ProjectEnd.ToString("dd/MM/yyyy"),
                    project.ProjectBudget.ToString("N2"),
                    project.CurrentPhaseNumber,  // ✅ จำนวนเฟส
                    project.CustomerName,  // ✅ ชื่อลูกค้า
                    project.EmployeeName   // ✅ ชื่อผู้ดูแลโครงการ
                );
            }
        }
        private void LoadProjectDetailData(int projectId)
        {
            // ดึงข้อมูล Project ทั้งหมด
            ProjectDAL dal = new ProjectDAL();
            Project project = dal.GetProjectDetailsById(projectId);

            if (project != null)
            {
                txtProjectID.Text = project.ProjectID.ToString();
                txtProjectName.Text = project.ProjectName;
                txtProjectDetail.Text = project.ProjectDetail;

                // โหลดหมายเลขเฟสทั้งหมดใส่ ComboBox
                LoadPhasesToComboBox(projectId);
            }
        }
        private void LoadPhasesToComboBox(int projectId)
        {
            PhaseDAL phaseDAL = new PhaseDAL();
            int phaseCount = phaseDAL.GetPhaseCountByProjectID(projectId);

            cmbSelectPhase.Items.Clear();

            for (int i = 1; i <= phaseCount; i++)
            {
                cmbSelectPhase.Items.Add("เฟสที่  " + i.ToString());
            }

            // ตั้งค่า Default เลือกอันแรก
            if (cmbSelectPhase.Items.Count > 0)
                cmbSelectPhase.SelectedIndex = 0;
        }

        private void CustomizeDataGridViewProject()
        {

            dtgvProjectData.BorderStyle = BorderStyle.None;
            dtgvProjectData.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dtgvProjectData.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvProjectData.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgvProjectData.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvProjectData.BackgroundColor = Color.White;

            dtgvProjectData.EnableHeadersVisualStyles = false;
            dtgvProjectData.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvProjectData.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgvProjectData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvProjectData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dtgvProjectData.ColumnHeadersHeight = 30;

            dtgvProjectData.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtgvProjectData.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvProjectData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvProjectData.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);

            dtgvProjectData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvProjectData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvProjectData.RowTemplate.Height = 30;

            dtgvProjectData.GridColor = Color.LightGray;
            dtgvProjectData.RowHeadersVisible = false;

            dtgvProjectData.ReadOnly = true;
            dtgvProjectData.AllowUserToAddRows = false;
            dtgvProjectData.AllowUserToResizeRows = false;
        }
        private void InitializeDataGridViewProject()
        {
            // ✅ ป้องกันการเพิ่มคอลัมน์ซ้ำ
            if (dtgvProjectData.Columns.Count == 0)
            {
                dtgvProjectData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dtgvProjectData.MultiSelect = false; // ✅ เลือกได้ทีละแถวเท่านั้น

                dtgvProjectData.AllowUserToAddRows = false;
                // ✅ เพิ่มคอลัมน์
                dtgvProjectData.Columns.Add("ProjectID", "รหัสโครงการ");
                dtgvProjectData.Columns.Add("ProjectName", "ชื่อโครงการ");
                dtgvProjectData.Columns.Add("ProjectStart", "วันที่เริ่มโครงการ");
                dtgvProjectData.Columns.Add("ProjectEnd", "วันที่สิ้นสุดโครงการ");
                dtgvProjectData.Columns.Add("ProjectBudget", "งบประมาณ (บาท)");
                dtgvProjectData.Columns.Add("CurrentPhaseNumber", "จำนวนเฟสงาน");
                dtgvProjectData.Columns.Add("CustomerName", "ชื่อลูกค้า");
                dtgvProjectData.Columns.Add("EmployeeName", "ชื่อผู้ดูแลโครงการ");

                // ✅ ปรับแต่งคอลัมน์
                dtgvProjectData.Columns["ProjectID"].Width = 80;
                dtgvProjectData.Columns["ProjectID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProjectData.Columns["ProjectID"].ReadOnly = true;

                dtgvProjectData.Columns["ProjectName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dtgvProjectData.Columns["ProjectName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dtgvProjectData.Columns["ProjectName"].ReadOnly = true;

                dtgvProjectData.Columns["ProjectStart"].Width = 120;
                dtgvProjectData.Columns["ProjectStart"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProjectData.Columns["ProjectStart"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dtgvProjectData.Columns["ProjectStart"].ReadOnly = true;

                dtgvProjectData.Columns["ProjectEnd"].Width = 120;
                dtgvProjectData.Columns["ProjectEnd"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProjectData.Columns["ProjectEnd"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dtgvProjectData.Columns["ProjectEnd"].ReadOnly = true;

                dtgvProjectData.Columns["ProjectBudget"].Width = 150;
                dtgvProjectData.Columns["ProjectBudget"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dtgvProjectData.Columns["ProjectBudget"].DefaultCellStyle.Format = "N2"; // แสดงเป็น 1,200.00
                dtgvProjectData.Columns["ProjectBudget"].ReadOnly = true;

                dtgvProjectData.Columns["CurrentPhaseNumber"].Width = 120;
                dtgvProjectData.Columns["CurrentPhaseNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvProjectData.Columns["CurrentPhaseNumber"].ReadOnly = true;

                dtgvProjectData.Columns["CustomerName"].Width = 150;
                dtgvProjectData.Columns["CustomerName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dtgvProjectData.Columns["CustomerName"].ReadOnly = true;

                dtgvProjectData.Columns["EmployeeName"].Width = 150;
                dtgvProjectData.Columns["EmployeeName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dtgvProjectData.Columns["EmployeeName"].ReadOnly = true;

                // ✅ ใช้ฟังก์ชันตกแต่ง
                CustomizeDataGridViewProject();
            }
        }
        private void dtgvProjectData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int projectId = Convert.ToInt32(dtgvProjectData.Rows[e.RowIndex].Cells["ProjectID"].Value);
                LoadProjectDetailData(projectId);
                LoadPhaseStatus(projectId, 1); // เริ่มต้นด้วยเฟส 1 เสมอ

            }
        }


        private void cmbAmountPictureUpload_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAmountPictureUpload.SelectedItem != null)
            {
                int imageCount = int.Parse(cmbAmountPictureUpload.SelectedItem.ToString());
                GenerateImageUploadControls(imageCount);
            }
        }

        private void GenerateImageUploadControls(int count)
        {
            pnlUploadImages.Controls.Clear(); // เคลียร์ช่องอัปโหลดเก่าออก

            int panelWidth = 662;
            int panelHeight = 500; // สูงขึ้นเพื่อรองรับ TextBox ใหญ่ขึ้น
            int marginX = 20;
            int marginY = 20;
            int columns = 2;

            Font defaultFont = new Font("Segoe UI", 16); // ✅ ฟอนต์หลักที่ใช้ทั้งระบบ

            for (int i = 0; i < count; i++)
            {
                int x = (i % columns) * (panelWidth + marginX);
                int y = (i / columns) * (panelHeight + marginY);

                Panel imagePanel = new Panel
                {
                    Size = new Size(panelWidth, panelHeight),
                    Location = new Point(x, y),
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label lblTitle = new Label
                {
                    Text = $"รูปที่ {i + 1} *",
                    ForeColor = Color.Red,
                    Font = new Font("Segoe UI", 16, FontStyle.Bold),
                    Location = new Point(10, 5),
                    AutoSize = true
                };

                PictureBox pictureBox = new PictureBox
                {
                    Size = new Size(642, 261),
                    Location = new Point(10, 40),
                    BorderStyle = BorderStyle.FixedSingle,
                    SizeMode = PictureBoxSizeMode.Zoom
                };
                pictureBox.Click += (s, e) => UploadImage(pictureBox);

                Button btnUpload = new Button
                {
                    Text = "อัพโหลดรูปภาพ",
                    Size = new Size(200, 45), // ✅ ปรับขนาดปุ่มให้เหมาะกับฟอนต์ 16
                    Location = new Point(10, 310),
                    Font = defaultFont // ✅ ฟอนต์เดียวกัน
                };
                btnUpload.Click += (s, e) => UploadImage(pictureBox);

                TextBox txtDescription = CreatePlaceholderTextBox($"คำอธิบายรูปที่ {i + 1}");
                txtDescription.Size = new Size(642, 100);   // ✅ รองรับข้อความยาว
                txtDescription.Multiline = true;            // ✅ หลายบรรทัด
                txtDescription.Location = new Point(10, 360);
                txtDescription.ScrollBars = ScrollBars.Vertical;
                txtDescription.Font = defaultFont;          // ✅ ฟอนต์หลัก
                txtDescription.ForeColor = Color.Gray;

                // ป้องกัน scroll กระโดดเมื่อคลิก
                txtDescription.Enter += (s, e) =>
                {
                    pnlUploadImages.ScrollControlIntoView(txtDescription);
                };

                // เพิ่มทุก control ลง Panel
                imagePanel.Controls.Add(lblTitle);
                imagePanel.Controls.Add(pictureBox);
                imagePanel.Controls.Add(btnUpload);
                imagePanel.Controls.Add(txtDescription);

                pnlUploadImages.Controls.Add(imagePanel);
            }
        }




        private void UploadImage(PictureBox pictureBox)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                    pictureBox.Tag = openFileDialog.FileName; // เก็บ Path รูปไว้ใน Tag
                }
            }
        }

        // ✅ ฟังก์ชันเลียนแบบ PlaceholderText
        private TextBox CreatePlaceholderTextBox(string placeholder)
        {
            TextBox txt = new TextBox();
            txt.ForeColor = Color.Gray;
            txt.Text = placeholder;

            txt.GotFocus += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };

            txt.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };

            return txt;
        }

        //button action
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateProjectData())
                return;

            int projectId = int.Parse(txtProjectID.Text);
            int selectedPhaseNo = cmbSelectPhase.SelectedIndex + 1;
            string selectedStatus = cmbPhaseStatus.SelectedValue?.ToString();
            string detail = txtDetailWorkFlow.Text.Trim();
            string remark = txtWorkRemark.Text.Trim();

            // ✅ สร้างอ็อบเจกต์สำหรับตาราง phase_working
            PhaseWorking phase = new PhaseWorking
            {
                ProjectID = projectId,             // ✅ แก้จาก ProId → ProjectID
                PhaseNo = selectedPhaseNo,
                WorkStatus = selectedStatus,
                WorkDetail = detail,
                WorkDate = dtpWorkDate.Value,
                EndDate = selectedStatus == WorkStatus.Completed ? dtpWorkDate.Value : (DateTime?)null,
                UpdateDate = DateTime.Now,
                Remark = remark,
                // ✅ กรณีเลือก Supplier
                SupplierID = !string.IsNullOrWhiteSpace(selectedSupplierID) ? selectedSupplierID : null
            };

            // ✅ บันทึกลงตาราง phase_working
            PhaseWorkDAL phaseDal = new PhaseWorkDAL();
            bool result = phaseDal.InsertPhaseWorking(phase);

            if (!result)
            {
                MessageBox.Show("ไม่สามารถบันทึกข้อมูลงานได้", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ วนลูปเพื่อบันทึกรูปใน panel
            foreach (Panel imagePanel in pnlUploadImages.Controls.OfType<Panel>())
            {
                PictureBox pictureBox = imagePanel.Controls.OfType<PictureBox>().FirstOrDefault();
                TextBox txtDescription = imagePanel.Controls.OfType<TextBox>().FirstOrDefault();

                if (pictureBox?.Image != null)
                {
                    byte[] imageData = ImageToByteArray(pictureBox.Image);
                    string description = txtDescription.Text.Trim();

                    WorkingPictureDAL picDAL = new WorkingPictureDAL();
                    picDAL.InsertPicture(new WorkingPicture
                    {
                        PhaseNo = selectedPhaseNo,
                        PictureData = imageData,           
                        PictureDetail = description        
                    });
                }
            }

            MessageBox.Show("บันทึกข้อมูลเรียบร้อยแล้ว", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        private void btnSearchSupplier_Click(object sender, EventArgs e)
        {
            OpenSearchForm("Supplier", txtSupplierName, new TextBox(), txtJuristicNumber);
        }
        private string selectedSupplierID = "";
        private void OpenSearchForm(string searchType, TextBox nameTextBox, TextBox lastNameTextBox, TextBox idCardOrRoleTextBox, TextBox phoneTextBox = null, TextBox emailTextBox = null)
        {
            using (SearchForm searchForm = new SearchForm(searchType))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    // ✅ เก็บค่า ID ของลูกค้าหรือพนักงาน (แต่ไม่แสดงใน UI)
                    
                    if (searchType == "Supplier")
                    {
                        selectedSupplierID = searchForm.SelectedID;
                    }

                    // ✅ แสดงเฉพาะข้อมูลที่ต้องการใน TextBox
                    nameTextBox.Text = searchForm.SelectedName;
                    lastNameTextBox.Text = searchForm.SelectedLastName;
                    idCardOrRoleTextBox.Text = searchForm.SelectedIDCardOrRole;

                    if (phoneTextBox != null) phoneTextBox.Text = searchForm.SelectedPhone;
                    if (emailTextBox != null) emailTextBox.Text = searchForm.SelectedEmail;
                }
            }
        }

        // valid check 
        private bool ValidateProjectData()
        {
            if (string.IsNullOrWhiteSpace(txtDetailWorkFlow.Text))
            {
                MessageBox.Show("กรุณากรอกชื่อโครงการ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                starDetailWorkFlow.Visible = true;
                return false;
            }
            else
            {
                starDetailWorkFlow.Visible = false;
                return true;
            }
        }

        private void cmbSelectPhase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSelectPhase.SelectedItem != null && int.TryParse(cmbSelectPhase.SelectedItem.ToString(), out int selectedPhaseNo))
            {
                int projectId = Convert.ToInt32(txtProjectID.Text); // หรือเก็บไว้เป็น field ก็ได้
                LoadPhaseStatus(projectId, selectedPhaseNo);
            }
        }

        //private void 

    }
}
