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
        private List<WorkingPicture> pictures = new List<WorkingPicture>();
        private List<PhaseWithStatus> phaseList = new List<PhaseWithStatus>();

        public UpdateProjectPhase()
        {
            InitializeComponent();
            InitGrid();
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

        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            using (var searchForm = new SearchForm("Project"))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    // รับค่าจาก SearchForm กลับมา
                    txtProjectID.Text = searchForm.SelectedID;
                    txtProjectName.Text = searchForm.SelectedName;

                    // โหลด Phase ทั้งหมดของโครงการที่เลือก
                    LoadPhaseForProject(searchForm.SelectedID);
                }
            }
        }

        private void LoadPhaseForProject(string projectId)
        {
            PhaseDAL phaseDAL = new PhaseDAL();
            phaseList = phaseDAL.GetPhasesWithStatus(Convert.ToInt32(projectId));

            var comboSource = new List<object>();
            comboSource.Add(new { phase_id = 0, phase_no = "-- เลือกเฟส --" });
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
            if (cmbSelectPhase.SelectedIndex <= 0)
            {
                txtPhaseStatus.Text = "";
                txtPhaseStatus.BackColor = Color.Yellow; // สี default หรือลบออกก็ได้
                return;
            }

            // ดึง phase id ที่เลือก
            var selectedValue = cmbSelectPhase.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(selectedValue))
            {
                txtPhaseStatus.Text = "";
                txtPhaseStatus.BackColor = Color.Yellow;
                return;
            }

            int phaseId;
            if (!int.TryParse(selectedValue, out phaseId)) return;

            // หา phase ที่เลือก
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




    }
}
