using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public class EmployeeAssignment
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeLastName { get; set; }
        public string AssignRole { get; set; }
        public string AssignBy { get; set; }
        public DateTime AssignDate { get; set; }
        public int ProjectID { get; set; }
    }
}
