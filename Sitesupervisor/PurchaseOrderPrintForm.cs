using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;

namespace JRSApplication.Sitesupervisor
{
    public partial class PurchaseOrderPrintForm : Form
    {
        private readonly int _orderId;
        private readonly string connectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public PurchaseOrderPrintForm(int orderId)
        {
            InitializeComponent();
            _orderId = orderId;
        }

        private void PurchaseOrderPrintForm_Load(object sender, EventArgs e)
        {
            try
            {
                string reportPath = Path.Combine(Application.StartupPath, "Sitesupervisor", "POreport.rdlc");
                reportViewer1.LocalReport.ReportPath = reportPath;

                // ✅ โหลดข้อมูลหัวใบสั่งซื้อ
                var dtHeader = GetPurchaseOrderHeader(_orderId);
                // ✅ โหลดรายละเอียดวัสดุ
                var dtDetail = GetPurchaseOrderDetail(_orderId);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dataPO", dtHeader));
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataMat", dtDetail));


                // ✅ เตรียมพารามิเตอร์
                string poNumber = "", projectNumber = "", orderDetail = "";
                string orderIssuer = "คุณ A";     // ตัวอย่าง สามารถดึงจากระบบ Login
                string orderApprover = "คุณ B";   // ตัวอย่าง สามารถดึงจากฐานข้อมูลจริง
                DateTime? orderDate = null;
                DateTime? approvedDate = null;

                if (dtHeader.Rows.Count > 0)
                {
                    DataRow r = dtHeader.Rows[0];

                    if (r.Table.Columns.Contains("OrderNumber"))
                        poNumber = r["OrderNumber"]?.ToString();

                    if (r.Table.Columns.Contains("ProjectNumber"))
                        projectNumber = r["ProjectNumber"]?.ToString();

                    if (r.Table.Columns.Contains("OrderDetail"))
                        orderDetail = r["OrderDetail"]?.ToString();

                    if (r.Table.Columns.Contains("OrderDate") && r["OrderDate"] != DBNull.Value)
                        orderDate = Convert.ToDateTime(r["OrderDate"]);

                    if (r.Table.Columns.Contains("ApproveDate") && r["ApproveDate"] != DBNull.Value)
                        approvedDate = Convert.ToDateTime(r["ApproveDate"]);
                }

                // ✅ ส่งพารามิเตอร์ไปยัง RDLC
                var parameters = new ReportParameter[]
                {
                    new ReportParameter("pPONumber", poNumber ?? ""),
                    new ReportParameter("pProjectNumber", projectNumber ?? ""),
                    new ReportParameter("pOrderIssuer", orderIssuer ?? ""),
                    new ReportParameter("pOrderApprover", orderApprover ?? ""),
                    new ReportParameter("pOrderDate", orderDate?.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd")),
                    new ReportParameter("pApprovedDate", approvedDate?.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd")),
                    new ReportParameter("pOrderDetail", orderDetail ?? "")
                };

                reportViewer1.LocalReport.SetParameters(parameters);
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"เกิดข้อผิดพลาดในการโหลดรายงาน:\n{ex.Message}\n\nรายละเอียดเพิ่มเติม:\n{ex.InnerException?.Message}",
                    "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // 🧩 ดึงข้อมูลส่วนหัวใบสั่งซื้อ
        private DataTable GetPurchaseOrderHeader(int orderId)
        {
            string sql = @"
                    SELECT 
                        po.order_id        AS OrderId,
                        po.order_number    AS OrderNumber,
                        po.order_detail    AS OrderDetail,
                        po.order_date      AS OrderDate,
                        po.order_duedate   AS DueDate,
                        po.approved_date   AS ApproveDate,
                        po.order_status    AS OrderStatus,
                        po.order_remark    AS OrderRemark,
                        pp.phase_no        AS PhaseNo,
                        p.pro_id           AS ProjectId,
                        p.pro_number       AS ProjectNumber
                    FROM purchaseorder po
                    INNER JOIN project_phase pp ON po.pro_id = pp.pro_id
                    INNER JOIN project p ON pp.pro_id = p.pro_id
                    WHERE po.order_id = @orderId";

            var dt = new DataTable();
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, con))
            using (var da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@orderId", orderId);
                da.Fill(dt);
            }
            return dt;
        }


        // 🧩 ดึงข้อมูลรายละเอียดวัสดุ
        private DataTable GetPurchaseOrderDetail(int orderId)
        {
            string sql = @"
                    SELECT 
                        m.mat_line_no AS MatLineNo,
                        m.mat_no AS MatNo,
                        m.mat_detail AS MatDetail,
                        m.mat_quantity AS MatQuantity,
                        m.mat_unit AS MatUnit,
                        m.mat_price AS MatPrice,
                        m.mat_amount AS MatAmount,
                        m.order_id AS OrderId
                    FROM material_detail m
                    WHERE m.order_id = @orderId
                    ORDER BY m.mat_line_no ASC;";

            var dt = new DataTable();
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, con))
            using (var da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@orderId", orderId);
                da.Fill(dt);
            }
            return dt;
        }




        // 🧾 ปุ่ม Export PDF
        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string savePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    $"PurchaseOrder_{_orderId}.pdf"
                );

                byte[] bytes = reportViewer1.LocalReport.Render("PDF");
                File.WriteAllBytes(savePath, bytes);

                MessageBox.Show($"บันทึกไฟล์สำเร็จที่:\n{savePath}",
                    "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการบันทึก PDF:\n" + ex.Message,
                    "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
