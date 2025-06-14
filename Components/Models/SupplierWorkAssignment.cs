using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components.Models
{
    public class SupplierWorkAssignment
    {
        public int AssignmentId { get; set; }       // Primary Key, Auto Increment
        public string SupId { get; set; }           // VARCHAR(10), Not null
        public DateTime StartDate { get; set; }  // start_date
        public DateTime? DueDate { get; set; }      // due_date
        public string AssignDescription { get; set; } // assign_description
        public string AssignRemark { get; set; } // assign_remark
        public int PhaseId { get; set; } // phase_id (FK)
    }
}
