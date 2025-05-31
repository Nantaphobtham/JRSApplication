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
    public partial class FormPDFPreview : Form
    {
        private string tempFilePath;

        public FormPDFPreview(byte[] pdfBytes)
        {
            InitializeComponent();

            // ตั้งค่ารูปร่างของ Form
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new System.Drawing.Size(800, 600);  // ✅ ปรับขนาดได้ตามต้องการ
            this.TopMost = true;                            // ✅ ลอยอยู่ด้านบน
            this.ShowInTaskbar = false;                     // ✅ ไม่โชว์ใน taskbar

            // สร้าง WebBrowser เพื่อแสดง PDF
            WebBrowser browser = new WebBrowser
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(browser);

            // 🧊 สร้างไฟล์ชั่วคราวจาก byte[]
            tempFilePath = Path.Combine(Path.GetTempPath(), $"preview_{Guid.NewGuid()}.pdf");
            File.WriteAllBytes(tempFilePath, pdfBytes);

            // แสดง PDF
            browser.Navigate(tempFilePath);
        }

        // 🔁 ลบไฟล์ชั่วคราวเมื่อปิดฟอร์ม
        //protected override void OnFormClosed(FormClosedEventArgs e)
        //{
        //    base.OnFormClosed(e);
        //    try
        //    {
        //        if (File.Exists(tempFilePath))
        //            File.Delete(tempFilePath);
        //    }
        //    catch { }
        //}
    }
}
