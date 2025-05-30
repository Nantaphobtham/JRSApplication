using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public class InvoiceDetail
    {
        public int InvoiceNo { get; set; }
        public int LineNo { get; set; }
        public string Detail { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
