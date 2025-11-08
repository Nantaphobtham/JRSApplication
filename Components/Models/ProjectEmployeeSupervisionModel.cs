using System;

namespace JRSApplication.Components.Models
{
    public class ProjectEmployeeSupervisionModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ContractNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string EmpId { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string EmpPhone { get; set; }
        public string EmpPosition { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
