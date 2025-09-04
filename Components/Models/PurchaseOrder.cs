using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components.Models
{
    public class PurchaseOrder
    {
        public int OrderId { get; set; }           // order_id
        public string OrderNumber { get; set; }     // order_number
        public string OrderDetail { get; set; }     // order_detail
        public DateTime OrderDate { get; set; }     // order_date
        public string OrderStatus { get; set; }     // order_status (ENUM in DB)
        public DateTime OrderDueDate { get; set; }  // order_duedate
        public string OrderRemark { get; set; }     // order_remark
        public string EmpId { get; set; }           // emp_id (VARCHAR(20))
        public string ApprovedByEmpId { get; set; } // approved_by_emp_id (VARCHAR(20))
        public DateTime? ApprovedDate { get; set; } // approved_date (nullable)
        public int ProId { get; set; }              // pro_id (int, foreign key to product table)

        // 👇 เพิ่ม property สำหรับเก็บชื่อ (JOIN มาจาก employee)
        public string EmpName { get; set; }          // ชื่อผู้สร้างใบสั่งซื้อ
        public string ApprovedByName { get; set; }   // ชื่อผู้อนุมัติ

        // Navigation property (optional, if using EF)
        public List<MaterialDetail> MaterialDetails { get; set; } = new List<MaterialDetail>();
        public PurchaseOrder() { }
    }
}
