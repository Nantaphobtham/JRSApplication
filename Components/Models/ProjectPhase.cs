using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public class ProjectPhase
    {
        public int PhaseID { get; set; }
        public int PhaseNumber { get; set; }  // phase_no
        public string PhaseDetail { get; set; }  // phase_detail
        public decimal PhaseBudget { get; set; }  // phase_budget
        public string PhaseStatus { get; set; }  // phase_status
        public decimal PhasePercent { get; set; }  // ✅ เปอร์เซ็นต์งานของแต่ละเฟส
    }

}
