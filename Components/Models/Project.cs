using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public class Project
    {
        public int ProjectID { get; set; }  // pro_id (Primary Key)
        public string ProjectName { get; set; }  // pro_name
        
        public string ProjectDetail { get; set; }  // pro_detail
        public string ProjectAddress { get; set; }  // pro_address
        public decimal ProjectBudget { get; set; }  // pro_budget
        public DateTime ProjectStart { get; set; }  // pro_start
        public DateTime ProjectEnd { get; set; }  // pro_end
        public int CurrentPhaseNumber { get; set; }  // pro_currentphasenumber
        public string Remark { get; set; }  // pro_remark
        public string ProjectNumber { get; set; }  // pro_number
        public string EmployeeID { get; set; }  // emp_id (FK)
        public int CustomerID { get; set; }  // cus_id (FK)

        public string CustomerName { get; set; } // ✅ ชื่อลูกค้า
        public string EmployeeName { get; set; } // ✅ ชื่อผู้ดูแลโครงการ

        // รองรับ ProjectFile
        public ProjectFile ProjectFile { get; set; }
        // ✅ เพิ่มส่วนนี้
        public List<ProjectPhase> Phases { get; set; }
    }

}
