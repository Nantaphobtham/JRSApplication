using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public class Invoice
    {
        public int InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public int EmployeeID { get; set; }
        public int CustomerID { get; set; }
        public int ProjectID { get; set; }
        public int PhaseNo { get; set; }
    }
}
