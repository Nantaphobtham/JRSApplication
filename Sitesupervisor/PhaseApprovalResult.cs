using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication.Sitesupervisor
{
    public partial class PhaseApprovalResult : UserControl
    {
        private readonly string _empId;
        private readonly string _role;
        public PhaseApprovalResult()
        {
            InitializeComponent();
        }

        public PhaseApprovalResult(string empId, string role) : this()
        {
            _empId = empId;
            _role = role;

            // ถ้าต้องใช้ค่า ก็เซ็ต/โหลดข้อมูลที่นี่
            // e.g., LoadDataFor(empId, role);
        }
    }
}
