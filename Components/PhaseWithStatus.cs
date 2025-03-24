using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public class PhaseWithStatus
    {
        public int PhaseNumber { get; set; }
        public string PhaseDetail { get; set; }
        public decimal PhaseBudget { get; set; }
        public decimal PhasePercent { get; set; }
        public string PhaseStatus { get; set; } // << สถานะ
    }

}
