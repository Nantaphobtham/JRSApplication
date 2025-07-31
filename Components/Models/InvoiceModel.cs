using System;

namespace JRSApplication.Components
{
    public class InvoiceModel
    {
        public string InvNo { get; set; }
        public DateTime InvDate { get; set; }
        public DateTime InvDueDate { get; set; }
        public string CusId { get; set; }
        public string CusName { get; set; }
        public string ProId { get; set; }
        public string ProNumber { get; set; }
        public string ProName { get; set; }
        public string PhaseId { get; set; }
        public string PhaseBudget { get; set; }
        public string PhaseDetail { get; set; }
        public string InvId { get; set; }
        public string Detail { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
