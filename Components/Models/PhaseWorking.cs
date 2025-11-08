using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public class PhaseWorking
    {
        public string WorkID { get; set; }
        public int ProjectID { get; set; } 
        public int PhaseID { get; set; }
        public int PhaseNo { get; set; }
        public string WorkDetail { get; set; }
        public string WorkStatus { get; set; }
        public DateTime? WorkDate { get; set; }
        public DateTime? EndDate { get; set; } 
        public DateTime? UpdateDate { get; set; }
        public string Remark { get; set; }
        public string SupplierAssignmentId { get; set; }

        
    }
}
