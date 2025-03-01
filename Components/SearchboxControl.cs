using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class SearchboxControl : UserControl
    {
        // Dictionary สำหรับเก็บตัวเลือกการค้นหาตาม Role และ Function
        private Dictionary<string, Dictionary<string, List<string>>> searchOptionsByRoleFunction;

        public SearchboxControl()
        {
            InitializeComponent();
            InitializeSearchOptions(); // เริ่มต้นตัวเลือกการค้นหา
        }

        // ฟังก์ชันกำหนดตัวเลือกการค้นหา
        private void InitializeSearchOptions()
        {
            searchOptionsByRoleFunction = new Dictionary<string, Dictionary<string, List<string>>>
            {
                {
                    "Admin", new Dictionary<string, List<string>>
                    {
                        // รอเพิ่ม cmbSearchBy 
                        { "จัดการบัญชีผู้ใช้", new List<string> { "ชื่อ", "เบอร์โทร", "ตำแหน่ง", "รหัสพนักงาน" } },
                        { "ทะเบียนลูกค้า", new List<string> { "ชื่อลูกค้า", "เลขประจำตัว", "อีเมล" } }
                    }
                },
                {
                    "Projectmanager", new Dictionary<string, List<string>>
                    {
                        { "ตรวจสอบข้อมูลโครงการ", new List<string> { "ชื่อโครงการ", "สถานะ", "วันที่เริ่ม" } },
                        { "อนุมัติคำขอ", new List<string> { "ชื่อคำขอ", "ผู้ส่งคำขอ", "สถานะ" } }
                    }
                },
                {
                    "Sitesupervisor", new Dictionary<string, List<string>>
                    {
                        // รอเพิ่ม cmbSearchBy 
                        {"ตรวจสอบข้อมูลโครงการ", new List<string> { "ชื่อโครงการ", "สถานะ", "วันที่เริ่ม" } }
                    }
                },
                {
                    "Accountant", new Dictionary<string, List<string>>
                    {
                        // รอเพิ่ม cmbSearchBy 
                        { "จัดการการเงิน", new List<string> { "รหัสใบแจ้งหนี้", "ยอดชำระ", "สถานะ" } }
                    }
                }
            };
        }

        // ฟังก์ชันสำหรับตั้งค่า Role และ Function จะถูกเรียกใช้ผ่าน form ต่างๆ
        public void SetRoleAndFunction(string role, string function = null)
        {
            if (searchOptionsByRoleFunction.ContainsKey(role))
            {
                // ตรวจสอบว่ามี Function อยู่ใน Role นี้หรือไม่
                var functions = searchOptionsByRoleFunction[role];
                if (!string.IsNullOrEmpty(function) && functions.ContainsKey(function))
                {
                    UpdateSearchOptions(functions[function]);
                }
                else if (functions.Count > 0)
                {
                    // หากไม่ระบุ Function ใช้ Function แรกเป็น Default
                    string defaultFunction = new List<string>(functions.Keys)[0];
                    UpdateSearchOptions(functions[defaultFunction]);
                }
                else
                {
                    MessageBox.Show($"Role '{role}' ไม่มีตัวเลือกการค้นหา", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"ไม่มีตัวเลือกสำหรับ Role: {role}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ฟังก์ชันอัปเดตตัวเลือกใน ComboBox
        private void UpdateSearchOptions(List<string> options)
        {
            cmbSearchBy.Items.Clear();
            cmbSearchBy.Items.AddRange(options.ToArray());
            if (options.Count > 0)
            {
                cmbSearchBy.SelectedIndex = 0; // เลือกตัวเลือกแรกเป็น Default
            }
        }

        // Event สำหรับปุ่มค้นหา
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string selectedOption = cmbSearchBy.SelectedItem?.ToString();
            string keyword = txtSearchKeyword.Text;

            if (string.IsNullOrWhiteSpace(selectedOption) || string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show("กรุณาเลือกตัวเลือกและกรอกคำค้นหา", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ตัวอย่าง: แสดงผลการค้นหาผ่าน MessageBox
            MessageBox.Show($"กำลังค้นหา '{selectedOption}' ด้วยคำค้น '{keyword}'");
        }
    }
}
