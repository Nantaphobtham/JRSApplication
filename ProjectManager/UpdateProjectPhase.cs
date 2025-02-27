using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

            int panelWidth = 662;  // ปรับขนาด Panel ตามที่ออกแบบ (ใหญ่ขึ้นเล็กน้อยจาก 642)
            int panelHeight = 400; // ปรับให้สูงขึ้นรองรับ PictureBox และปุ่ม
            int marginX = 20;      // ระยะห่างแนวนอน
            int marginY = 20;      // ระยะห่างแนวตั้ง
            int columns = 2;       // จัดเรียงเป็น 2 คอลัมน์

            for (int i = 0; i < count; i++)
            {
                // คำนวณตำแหน่ง X, Y ตามลำดับ
                int x = (i % columns) * (panelWidth + marginX);
                int y = (i / columns) * (panelHeight + marginY);

                Panel imagePanel = new Panel();
                imagePanel.Size = new Size(panelWidth, panelHeight);
                imagePanel.Location = new Point(x, y);
                imagePanel.BorderStyle = BorderStyle.FixedSingle;

                Label lblTitle = new Label();
                lblTitle.Text = $"รูปที่ {i + 1} *";
                lblTitle.ForeColor = Color.Red;
                lblTitle.Font = new Font("Tahoma", 10, FontStyle.Bold);
                lblTitle.Location = new Point(10, 5);

                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(642, 261); // ปรับขนาดให้ใหญ่ขึ้นตามที่คุณต้องการ
                pictureBox.Location = new Point(10, 30); // ขยับลงเล็กน้อย
                pictureBox.BorderStyle = BorderStyle.FixedSingle;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Click += (s, e) => UploadImage(pictureBox);

                Button btnUpload = new Button();
                btnUpload.Text = "อัพโหลดรูปภาพ";
                btnUpload.Size = new Size(120, 30);
                btnUpload.Location = new Point(10, 300); // ขยับลงหลังจาก PictureBox
                btnUpload.Click += (s, e) => UploadImage(pictureBox);

                TextBox txtDescription = CreatePlaceholderTextBox($"คำอธิบายรูปที่ {i + 1}");
                txtDescription.Size = new Size(642, 25); // ให้เท่ากับความกว้างของ PictureBox
                txtDescription.Location = new Point(10, 340);

                // เพิ่ม Controls ลง Panel
                imagePanel.Controls.Add(lblTitle);
                imagePanel.Controls.Add(pictureBox);
                imagePanel.Controls.Add(btnUpload);
                imagePanel.Controls.Add(txtDescription);

                // เพิ่ม Panel ลงใน pnlUploadImages
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

        // ✅ ฟังก์ชันเลียนแบบ PlaceholderText สำหรับ .NET Framework 4.x
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
    }
}
