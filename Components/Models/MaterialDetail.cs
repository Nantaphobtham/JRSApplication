using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components.Models
{
    public class MaterialDetail
    {
        public int OrderId { get; set; }            // order_id
        public int MatLineNo { get; set; }          // mat_line_no
        public int MatNo { get; set; }           // mat_no
        public string MatDetail { get; set; }       // mat_detail
        public decimal MatAmount { get; set; }      // mat_amount
        public decimal MatPrice { get; set; }       // mat_price
        public decimal MatQuantity { get; set; }    // mat_quantity
        public string MatUnit { get; set; }        // mat_unit

        // Navigation property (optional, if using EF)
        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
