using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;


namespace JRSApplication
{
    public partial class SearchboxControl : UserControl
    {
        // ยิงอีเวนต์ให้ฟอร์มแม่
        public event EventHandler<SearchEventArgs> SearchTriggered;

        // ค่าดีฟอลต์ (เปลี่ยนได้ตามหน้า)
        public string DefaultRole { get; set; } = "Admin";
        public string DefaultFunction { get; set; } = "จัดการบัญชีผู้ใช้";

        // เก็บตัวเลือกค้นหาตาม Role/Function
        private Dictionary<string, Dictionary<string, List<string>>> searchOptionsByRoleFunction;

        // properties เผื่อฟอร์มแม่อยากอ่านค่า
        public string SelectedSearchBy => cmbSearchBy.SelectedItem?.ToString() ?? "";
        public string Keyword
        {
            get => (txtSearchKeyword.Text ?? "").Trim();
            set => txtSearchKeyword.Text = value ?? "";
        }

        public SearchboxControl()
        {
            InitializeComponent();
            InitializeSearchOptions();

            // ใส่ค่า default ให้เลย ตอนคอนโทรลโหลดครั้งแรก
            this.Load += (s, e) => ApplyDefaultIfEmpty();
            // กรณีบางฟอร์ม Load เร็วไป ให้ยิงอีกทีตอน handle ถูกสร้าง
            // (กันกรณีคอนโทรลอยู่ใน Tab/Panel ที่สร้างช้ากว่า)
            this.HandleCreated += (s, e) => ApplyDefaultIfEmpty();

            // พิมพ์แล้วค้นหาอัตโนมัติ
            txtSearchKeyword.TextChanged += (s, e) => TriggerSearch();
            // กด Enter เพื่อยิงค้นหา, Esc เพื่อล้าง
            txtSearchKeyword.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    TriggerSearch();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    txtSearchKeyword.Clear();
                    TriggerSearch();
                }
            };

            btnSearch.Click += (s, e) => TriggerSearch();
        }

        // ใส่ชุดตัวเลือก
        private void InitializeSearchOptions()
        {
            searchOptionsByRoleFunction = new Dictionary<string, Dictionary<string, List<string>>>
            {
                {
                    "Admin", new Dictionary<string, List<string>>
                    {
                        { "จัดการบัญชีผู้ใช้", new List<string> { "ชื่อ", "นามสกุล", "ชื่อผู้ใช้", "เบอร์โทร", "อีเมล", "ตำแหน่ง", "รหัสพนักงาน", "ที่อยู่" } },
                        { "ทะเบียนลูกค้า", new List<string> { "ชื่อลูกค้า", "เลขประจำตัว", "อีเมล" } },
                        { "ทะเบียนซัพพลายเออร์", new List<string> { "ชื่อบริษัทซัพพลายเออร์", "รหัสซัพพลายเออร์" } },
                        { "จัดการข้อมูลโครงการ", new List<string> { "รหัสโครงการ", "ชื่อโครงการ", "เลขที่สัญญา" } }
                    }
                },
                {
                    "Projectmanager", new Dictionary<string, List<string>>
                    {
                        { "ตรวจสอบข้อมูลโครงการ", new List<string> { "รหัสโครงการ", "ชื่อโครงการ", "เลขที่สัญญา" } },
                        { "อนุมัติคำขอ", new List<string> { "ชื่อคำขอ", "ผู้ส่งคำขอ", "สถานะ" } }
                    }
                },
                {
                    "Sitesupervisor", new Dictionary<string, List<string>>
                    {
                        { "ตรวจสอบข้อมูลโครงการ", new List<string> { "รหัสโครงการ", "ชื่อโครงการ", "เลขที่สัญญา" } }
                    }
                },
                {
                    "Accountant", new Dictionary<string, List<string>>
                    {
                        { "จัดการการเงิน", new List<string> { "รหัสใบแจ้งหนี้", "ยอดชำระ", "สถานะ" } }
                    }
                }
            };
        }

        // ฟอร์มแม่เรียกเมธอดนี้เพื่อเซ็ต Role/Function
        public void SetRoleAndFunction(string role, string function = null)
        {
            if (!searchOptionsByRoleFunction.TryGetValue(role, out var functions))
            {
                MessageBox.Show($"ไม่มีตัวเลือกสำหรับ Role: {role}", "แจ้งเตือน",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(function) && functions.ContainsKey(function))
            {
                UpdateSearchOptions(functions[function]);
            }
            else if (functions.Count > 0)
            {
                // เอา function แรกเป็นค่าเริ่มต้น
                string defaultFunction = new List<string>(functions.Keys)[0];
                UpdateSearchOptions(functions[defaultFunction]);
            }
            else
            {
                cmbSearchBy.Items.Clear();
                cmbSearchBy.SelectedIndex = -1;
            }
        }

        // อัปเดตรายการในคอมโบบ็อกซ์ (คัดทิ้งสายว่าง)
        private void UpdateSearchOptions(List<string> options)
        {
            cmbSearchBy.Items.Clear();

            var clean = new List<string>();
            if (options != null)
            {
                foreach (var s in options)
                {
                    var t = (s ?? "").Trim();
                    if (!string.IsNullOrWhiteSpace(t)) clean.Add(t);
                }
            }

            if (clean.Count > 0)
            {
                cmbSearchBy.Items.AddRange(clean.ToArray());
                cmbSearchBy.SelectedIndex = 0;
            }
            else
            {
                cmbSearchBy.SelectedIndex = -1;
            }
        }

        // บังคับอัดค่า default ถ้ายังว่าง
        private void ApplyDefaultIfEmpty()
        {
            if (cmbSearchBy.Items.Count > 0) return;

            if (searchOptionsByRoleFunction != null &&
                searchOptionsByRoleFunction.TryGetValue(DefaultRole, out var funcs))
            {
                if (!string.IsNullOrWhiteSpace(DefaultFunction) && funcs.ContainsKey(DefaultFunction))
                {
                    UpdateSearchOptions(funcs[DefaultFunction]);
                }
                else if (funcs.Count > 0)
                {
                    // เอาค่าลิสต์แรกของ dictionary values อย่างถูกชนิด
                    var firstOptionList = funcs.Values.First(); // ต้องมี using System.Linq;
                    UpdateSearchOptions(firstOptionList);
                }
            }
            else
            {
                // fallback กันว่าง
                UpdateSearchOptions(new List<string> { "ชื่อ", "นามสกุล", "ชื่อผู้ใช้", "เบอร์โทร", "อีเมล" });
            }
        }


        // ยิงค้นหาออกไป (คีย์เวิร์ดว่างก็ยิง เพื่อให้ฟอร์มแม่ล้างฟิลเตอร์ได้)
        private void TriggerSearch()
        {
            string selectedOption = cmbSearchBy.SelectedItem?.ToString() ?? "";
            string keyword = (txtSearchKeyword.Text ?? "").Trim();
            SearchTriggered?.Invoke(this, new SearchEventArgs(selectedOption, keyword));
        }
    }

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
