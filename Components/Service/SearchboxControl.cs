using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class SearchboxControl : UserControl
    {
        // ยิงอีเวนต์ให้ฟอร์มแม่
        public event EventHandler<SearchEventArgs> SearchTriggered;

        // ค่าดีฟอลต์ (ถ้าไม่เซ็ตจากข้างนอก)
        public string DefaultRole { get; set; } = "Admin";
        public string DefaultFunction { get; set; } = "จัดการบัญชีผู้ใช้";

        // เก็บตัวเลือกค้นหาตาม Role / Function
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

            // ใส่ค่า default ให้ตอนคอนโทรลโหลด/สร้าง handle ครั้งแรก
            this.Load += (s, e) => ApplyDefaultIfEmpty();
            this.HandleCreated += (s, e) => ApplyDefaultIfEmpty();

            // พิมพ์แล้วค้นหาอัตโนมัติ
            txtSearchKeyword.TextChanged += (s, e) => TriggerSearch();

            // Enter = ค้นหา, Esc = ล้าง
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

        // เซ็ตชุดตัวเลือกทั้งหมด
        private void InitializeSearchOptions()
        {
            searchOptionsByRoleFunction = new Dictionary<string, Dictionary<string, List<string>>>
            {
                {
                    "Admin", new Dictionary<string, List<string>>
                    {
                        {
                            "จัดการบัญชีผู้ใช้",
                            new List<string>
                            {
                                "ชื่อ", "นามสกุล", "ชื่อผู้ใช้",
                                "เบอร์โทร", "อีเมล", "ตำแหน่ง",
                                "รหัสพนักงาน", "ที่อยู่"
                            }
                        },
                        {
                            "ทะเบียนลูกค้า",
                            new List<string> { "ชื่อลูกค้า", "เลขประจำตัว", "อีเมล" }
                        },
                        {
                            "ทะเบียนซัพพลายเออร์",
                            new List<string> { "ชื่อบริษัทซัพพลายเออร์", "รหัสซัพพลายเออร์" }
                        },
                        {
                            "จัดการข้อมูลโครงการ",
                            new List<string> { "รหัสโครงการ", "ชื่อโครงการ", "เลขที่สัญญา" }
                        }
                    }
                },
                {
                    "Projectmanager", new Dictionary<string, List<string>>
                    {
                        {
                            "ตรวจสอบข้อมูลโครงการ",
                            new List<string>
                            { "รหัสโครงการ", "ชื่อโครงการ", "เลขที่สัญญา" }
                        },
                        {
                            "ตรวจสอบข้อมูลโครงการ2",
                            new List<string>
                            { "รหัสโครงการ", "ชื่อโครงการ"}
                        },
                        {
                            "อนุมัติคำขอ",
                            new List<string>
                            { "ชื่อคำขอ", "ผู้ส่งคำขอ", "สถานะ" }
                        },
                        {
                            "ตรวจสอบข้อมูลการเงินโครงการ",
                            new List<string>
                            { "รหัสงาน","รหัสโครงการ","ชื่อโครงการ","เลขที่สัญญา","ชื่อลูกค้า","รหัสพนักงาน","ชื่อพนักงาน"}
                        },
                        {
                            "อนุมัติใบสั่งซื้อ",
                            new List<string>
                            {"เลขที่ใบสั่งซื้อ","สถานะใบสั่งซื้อ","ผู้ออกใบสั่งซื้อ","ผู้อนุมัติ" }
                        },
                        {
                            "จัดสรรบุคลากร",
                            new List<string>
                            { "รหัสงาน", "รหัสโครงการ", "ชื่อโครงการ","เลขที่สัญญา","ชื่อลูกค้า","อีเมลลูกค้า","รหัสพนักงาน","ชื่อพนักงาน","ตำแหน่ง" }
                        },
                        {
                            "กำหนดผู้รับเหมา",
                            new List<string>
                            { "รหัสงาน", "รหัสโครงการ", "รหัสผู้รับเหมา"}
                        }

                        
                    }
                },
                {
                    "Sitesupervisor", new Dictionary<string, List<string>>
                    {
                        {
                            "ตรวจสอบข้อมูลโครงการ",
                            new List<string> { "รหัสโครงการ", "ชื่อโครงการ", "เลขที่สัญญา" }
                        }
                    }
                },
                {
                    // ใช้กับฝ่ายบัญชี – role = Accountant
                    "Accountant", new Dictionary<string, List<string>>
                    {
                        {
                            "จัดการการเงิน",
                            new List<string> { "รหัสใบแจ้งหนี้", "ยอดชำระ", "สถานะ" }
                        }
                    }
                },
                {
                    // เผื่อระบบล็อกอินส่ง role = "Account" แทน "Accountant"
                    "Account", new Dictionary<string, List<string>>
                    {
                        {
                            "จัดการการเงิน",
                            new List<string> { "รหัสใบแจ้งหนี้", "ยอดชำระ", "สถานะ" }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// ฟอร์มแม่เรียกเมธอดนี้เพื่อเซ็ต Role/Function
        /// เช่น searchbox.SetRoleAndFunction("Accountant", "จัดการการเงิน");
        /// </summary>
        public void SetRoleAndFunction(string role, string function = null)
        {
            if (string.IsNullOrWhiteSpace(role))
                role = DefaultRole;

            if (!searchOptionsByRoleFunction.TryGetValue(role, out var functions))
            {
                // ถ้า role ไม่เจอ ใช้ DefaultRole แทน
                if (!searchOptionsByRoleFunction.TryGetValue(DefaultRole, out functions))
                {
                    // ถ้า default ก็ยังไม่มี ให้ fallback แบบง่าย ๆ
                    UpdateSearchOptions(new List<string> { "ชื่อ", "นามสกุล", "อีเมล" });
                    return;
                }
            }

            if (!string.IsNullOrEmpty(function) && functions.ContainsKey(function))
            {
                UpdateSearchOptions(functions[function]);
            }
            else if (!string.IsNullOrWhiteSpace(DefaultFunction) && functions.ContainsKey(DefaultFunction))
            {
                // ถ้าระบุ function ไม่ตรง แต่มี DefaultFunction ให้ใช้ DefaultFunction
                UpdateSearchOptions(functions[DefaultFunction]);
            }
            else if (functions.Count > 0)
            {
                // เอา function แรกเป็นค่าเริ่มต้น
                var firstOptionList = functions.Values.First();
                UpdateSearchOptions(firstOptionList);
            }
            else
            {
                cmbSearchBy.Items.Clear();
                cmbSearchBy.SelectedIndex = -1;
            }
        }

        // เติมรายการคอมโบ (ล้างช่องว่าง/Null)
        private void UpdateSearchOptions(List<string> options)
        {
            cmbSearchBy.Items.Clear();

            var clean = new List<string>();
            if (options != null)
            {
                foreach (var s in options)
                {
                    var t = (s ?? "").Trim();
                    if (!string.IsNullOrWhiteSpace(t))
                        clean.Add(t);
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

        // ถ้ายังไม่ถูกเซ็ตอะไรเลย ให้ใช้ DefaultRole/DefaultFunction
        private void ApplyDefaultIfEmpty()
        {
            if (cmbSearchBy.Items.Count > 0)
                return;

            if (searchOptionsByRoleFunction != null &&
                searchOptionsByRoleFunction.TryGetValue(DefaultRole, out var funcs))
            {
                if (!string.IsNullOrWhiteSpace(DefaultFunction) &&
                    funcs.ContainsKey(DefaultFunction))
                {
                    UpdateSearchOptions(funcs[DefaultFunction]);
                }
                else if (funcs.Count > 0)
                {
                    var firstOptionList = funcs.Values.First();
                    UpdateSearchOptions(firstOptionList);
                }
            }
            else
            {
                // fallback กันว่าง
                UpdateSearchOptions(
                    new List<string> { "ชื่อ", "นามสกุล", "ชื่อผู้ใช้", "เบอร์โทร", "อีเมล" });
            }
        }

        // ยิงอีเวนต์ค้นหาออกไปให้ฟอร์มแม่
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
