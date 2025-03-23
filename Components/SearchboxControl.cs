using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class SearchboxControl : UserControl
    {
        // Event สำหรับส่งค่าการค้นหาออกไปยังฟอร์มที่เรียก
        public event EventHandler<SearchEventArgs> SearchTriggered;

        // Dictionary เก็บตัวเลือกการค้นหา
        private Dictionary<string, Dictionary<string, List<string>>> searchOptionsByRoleFunction;

        public SearchboxControl()
        {
            InitializeComponent();
            InitializeSearchOptions();
        }

        // ฟังก์ชันกำหนดตัวเลือกการค้นหา
        private void InitializeSearchOptions()
        {
            searchOptionsByRoleFunction = new Dictionary<string, Dictionary<string, List<string>>>
            {
                {
                    "Admin", new Dictionary<string, List<string>>
                    {
                        { "จัดการบัญชีผู้ใช้", new List<string> { "ชื่อ", "เบอร์โทร", "ตำแหน่ง", "รหัสพนักงาน" } },
                        { "ทะเบียนลูกค้า", new List<string> { "ชื่อลูกค้า", "เลขประจำตัว", "อีเมล" } },
                        { "ทะเบียนซัพพลายเออร์", new List<string> {"ชื่อบริซัทซัพพลายเออร์","รหัสซัพพลายเออร์","" } },
                        { "จัดการข้อมูลโครงการ", new List<string> {"รหัสโครงการ","ชื่อโครงการ","เลขที่สัญญา"} }
                        //สามารถเพิ่มอื่นๆได้
                    }
                },
                {
                    "Projectmanager", new Dictionary<string, List<string>>
                    {
                        { "ตรวจสอบข้อมูลโครงการ", new List<string> {"รหัสโครงการ","ชื่อโครงการ","เลขที่สัญญา"} },
                        { "อนุมัติคำขอ", new List<string> { "ชื่อคำขอ", "ผู้ส่งคำขอ", "สถานะ" } }
                        //สามารถเพิ่มอื่นๆได้
                    }
                },
                {
                    "Sitesupervisor", new Dictionary<string, List<string>>
                    {
                        {"ตรวจสอบข้อมูลโครงการ", new List<string> {"รหัสโครงการ","ชื่อโครงการ","เลขที่สัญญา"} }
                        //สามารถเพิ่มอื่นๆได้
                    }
                },
                {
                    "Accountant", new Dictionary<string, List<string>>
                    {
                        { "จัดการการเงิน", new List<string> { "รหัสใบแจ้งหนี้", "ยอดชำระ", "สถานะ" } }
                        //สามารถเพิ่มอื่นๆได้
                    }
                }
            };
        }

        // ฟังก์ชันสำหรับตั้งค่า Role และ Function (ใช้จากฟอร์มอื่น)
        public void SetRoleAndFunction(string role, string function = null)
        {
            if (searchOptionsByRoleFunction.ContainsKey(role))
            {
                var functions = searchOptionsByRoleFunction[role];
                if (!string.IsNullOrEmpty(function) && functions.ContainsKey(function))
                {
                    UpdateSearchOptions(functions[function]);
                }
                else if (functions.Count > 0)
                {
                    string defaultFunction = new List<string>(functions.Keys)[0];
                    UpdateSearchOptions(functions[defaultFunction]);
                }
                else
                {
                    MessageBox.Show($"Role '{role}' ไม่มีตัวเลือกการค้นหา", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show($"ไม่มีตัวเลือกสำหรับ Role: {role}", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        // Event กดปุ่มค้นหา
        private void btnSearch_Click(object sender, EventArgs e)
        {
            TriggerSearch();
        }

        // Event ค้นหาอัตโนมัติเมื่อพิมพ์ข้อความ
        private void txtSearchKeyword_TextChanged(object sender, EventArgs e)
        {
            TriggerSearch();
        }

        private void TriggerSearch()
        {
            string selectedOption = cmbSearchBy.SelectedItem?.ToString();
            string keyword = txtSearchKeyword.Text.Trim();

            if (string.IsNullOrWhiteSpace(selectedOption) || string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show("กรุณาเลือกตัวเลือกและกรอกคำค้นหา", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Trigger Event ส่งค่าการค้นหาไปยังฟอร์มที่เรียกใช้
            SearchTriggered?.Invoke(this, new SearchEventArgs(selectedOption, keyword));
        }
    }

    // คลาส EventArgs สำหรับส่งค่าการค้นหาออกไป
    public class SearchEventArgs : EventArgs
    {
        public string SearchBy { get; }
        public string Keyword { get; }

        public SearchEventArgs(string searchBy, string keyword)
        {
            SearchBy = searchBy;
            Keyword = keyword;
        }
    }
}
