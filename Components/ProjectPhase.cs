using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public class ProjectPhase
    {
        public int PhaseNumber { get; set; }  // phase_no
        public string PhaseDetail { get; set; }  // phase_detail
        public decimal PhaseBudget { get; set; }  // phase_budget
        public string PhaseRemark { get; set; }  // phase_remark
        public int ProjectID { get; set; }  // pro_id (FK)
        public string SupplierID { get; set; }  // sup_id (FK)

        public decimal PhasePercent { get; set; }  // ✅ เปอร์เซ็นต์งานของแต่ละเฟส
    }

}
