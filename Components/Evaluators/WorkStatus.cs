using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public static class WorkStatus
    {
        public const string NotStarted = "NotStarted";          // ใช้เป็น Default ในอนาคตถ้าต้องการ
        public const string InProgress = "InProgress";          // ฝ่าย PM / site suppervisor 
        public const string Completed = "Completed";            // ฝ่าย PM 
        public const string Rejected = "Rejected";            // ฝ่าย PM / site suppervisor (ถ้าไม่ผ่านการตรวจสอบ)
        public const string WaitingForInvoice = "WaitingForInvoice";  // ฝ่าย AC
        public const string Paid = "Paid";                      // ฝ่าย AC

        public static List<string> AllStatuses => new List<string>
        {
        InProgress, Completed, WaitingForInvoice, Paid
        };

        // ✅ ฟังก์ชันแปลงชื่อภาษาไทย
        public static string GetDisplayName(string status)
        {
            switch (status)
            {
                case NotStarted: return "ยังไม่เริ่ม"; //รอเปลี่ยนคำให้เหมาะสม
                case InProgress: return "กำลังดำเนินการ";
                case WaitingForInvoice: return "รอเรียกเก็บเงิน";
                case Rejected: return "ถูกปฏิเสธ"; // ใช้สำหรับกรณีที่ไม่ผ่านการตรวจสอบ
                //case Invoiced: return "ออกใบแจ้งหนี้แล้ว";
                case Paid: return "ชำระเงินแล้ว";
                case Completed: return "เสร็จสมบูรณ์";
                default: return "รอการดำเนินการ";
            }
        }

        // ✅ (เลือกใช้) ฟังก์ชันระบุสี (สำหรับ DataGridView หรือ Label)
        public static Color GetStatusColor(string status)
        {
            switch (status)
            {
                case NotStarted: return Color.LightGray;
                case InProgress: return Color.Orange;
                case WaitingForInvoice: return Color.Orange;
                case Rejected: return Color.IndianRed;
                //case Invoiced: return Color.CornflowerBlue;
                case Paid: return Color.MediumSeaGreen;
                case Completed: return Color.MediumSeaGreen;
                default: return Color.Yellow;
            }
        }
    }

    // class Purcash order
    public static class PurchaseOrderStatus
    {
        public const string Submitted = "submitted";
        public const string Approved = "approved";
        public const string Rejected = "rejected";

        public static List<string> AllStatuses => new List<string>
        {
            Submitted, Approved, Rejected
        };

        public static string GetDisplayName(string status)
        {
            switch (status)
            {
                case Submitted: return "ส่งใบสั่งซื้อแล้ว";
                case Approved: return "อนุมัติแล้ว";
                case Rejected: return "ถูกปฏิเสธ";
                default: return "ไม่ทราบสถานะ";
            }
        }

        public static Color GetStatusColor(string status)
        {
            switch (status)
            {
                case Submitted: return Color.LightBlue;
                case Approved: return Color.MediumSeaGreen;
                case Rejected: return Color.IndianRed;
                default: return Color.Gray;
            }
        }
    }
}
