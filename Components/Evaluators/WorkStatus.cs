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
        public const string NotStarted = "NotStarted";        // ยังไม่เริ่ม
        public const string InProgress = "InProgress";        // กำลังดำเนินการ (ไซต์/PM)
        public const string Completed = "Completed";         // อนุมัติแล้ว (PM)
        public const string Rejected = "Rejected";          // ไม่อนุมัติ / ถูกปฏิเสธ
        public const string WaitingForInvoice = "WaitingForInvoice"; // รอเรียกเก็บเงิน (AC)
        public const string Paid = "Paid";              // ชำระเงินแล้ว (AC)

        // รวมทุกสถานะ (เผื่อใช้ bind กับ combobox)
        public static List<string> AllStatuses => new List<string>
        {
            NotStarted,
            InProgress,
            Completed,
            Rejected,
            WaitingForInvoice,
            Paid
        };

        // แปลงเป็นข้อความภาษาไทยสำหรับแสดงผล
        public static string GetDisplayName(string status)
        {
            switch (status)
            {
                case NotStarted:
                    return "ยังไม่เริ่ม";

                case InProgress:
                    return "กำลังดำเนินการ";

                case Completed:
                    // ใช้ข้อความนี้ตอนอนุมัติผลการทำงานแล้ว
                    return "อนุมัติแล้ว";

                case Rejected:
                    return "ไม่ผ่านการอนุมัติ";

                case WaitingForInvoice:
                    return "รอเรียกเก็บเงิน";

                case Paid:
                    return "ชำระเงินแล้ว";

                default:
                    return "รอการดำเนินการ";
            }
        }

        // สีสำหรับใช้แสดงใน Grid / Label
        public static Color GetStatusColor(string status)
        {
            switch (status)
            {
                case NotStarted:
                    return Color.LightGray;

                case InProgress:
                    return Color.Orange;

                case Completed:
                    return Color.MediumSeaGreen;

                case Rejected:
                    return Color.IndianRed;

                case WaitingForInvoice:
                    return Color.CornflowerBlue;

                case Paid:
                    return Color.MediumSeaGreen;

                default:
                    return Color.Yellow;
            }
        }
    }

    // ---------------- Purchase Order ----------------

    public static class PurchaseOrderStatus
    {
        public const string Submitted = "submitted";
        public const string Approved = "approved";
        public const string Rejected = "rejected";

        public static List<string> AllStatuses => new List<string>
        {
            Submitted,
            Approved,
            Rejected
        };

        public static string GetDisplayName(string status)
        {
            switch (status)
            {
                case Submitted:
                    return "ส่งใบสั่งซื้อ";

                case Approved:
                    return "อนุมัติ";

                case Rejected:
                    return "ปฏิเสธใบสั่งซื้อ";

                default:
                    return "ไม่ทราบสถานะ";
            }
        }

        public static Color GetStatusColor(string status)
        {
            switch (status)
            {
                case Submitted:
                    return Color.LightBlue;

                case Approved:
                    return Color.MediumSeaGreen;

                case Rejected:
                    return Color.IndianRed;

                default:
                    return Color.Gray;
            }
        }
    }
}
