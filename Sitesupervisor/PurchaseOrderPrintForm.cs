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

        public int OrderId => _orderId;

        public string ConnectionString => connectionString;

        public PurchaseOrderPrintForm(int orderId)
        {
            InitializeComponent();
            _orderId = orderId;
        }

        private void PurchaseOrderPrintForm_Load(object sender, EventArgs e)
        {
            try
            {
                // ✅ ระบุ path ของไฟล์รายงาน RDLC ให้ชัดเจน
                string reportPath = Path.Combine(Application.StartupPath, "Sitesupervisor", "POreport.rdlc");
                if (!File.Exists(reportPath))
                {
                    MessageBox.Show("ไม่พบไฟล์รายงาน: " + reportPath);
                    return;
                }

                reportViewer1.LocalReport.ReportPath = reportPath; // กำหนดไฟล์ที่ใช้

                // ✅ โหลดข้อมูล
                var dtHeader = GetPurchaseOrderHeader(OrderId);
                var dtDetail = GetPurchaseOrderDetail(OrderId);
                // ✅ เพิ่มคอลัมน์ใหม่สำหรับชื่อเต็ม (ชื่อ+นามสกุล)
                if (!dtHeader.Columns.Contains("OrderByFullName"))
                    dtHeader.Columns.Add("OrderByFullName", typeof(string));
                if (!dtHeader.Columns.Contains("ApprovedByFullName"))
                    dtHeader.Columns.Add("ApprovedByFullName", typeof(string));
                // ✅ ใส่ค่าด้วยฟังก์ชันที่คุณมี
                foreach (DataRow row in dtHeader.Rows)
                {
                    string orderByEmpId = row["emp_id"]?.ToString();
                    string approvedByEmpId = row["approved_by_emp_id"]?.ToString();

                    row["OrderByFullName"] = GetEmployeeFullName(orderByEmpId);
                    row["ApprovedByFullName"] = GetEmployeeFullName(approvedByEmpId);
                }

                Console.WriteLine($"Header Rows: {dtHeader.Rows.Count}");
                Console.WriteLine($"Detail Rows: {dtDetail.Rows.Count}");

                // ✅ ล้าง DataSource เก่า
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("POHeaderDataSet", dtHeader));
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("PODetailDataSet", dtDetail));

                reportViewer1.RefreshReport();
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
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
            p.pro_number       AS ProjectNumber,
            po.emp_id,               -- ต้องดึงมา
            po.approved_by_emp_id   -- ต้องดึงมา
        FROM purchaseorder po
        INNER JOIN project_phase pp ON po.pro_id = pp.pro_id
        INNER JOIN project p ON pp.pro_id = p.pro_id
        WHERE po.order_id = @orderId
        LIMIT 1;
    ";

            var dt = new DataTable();
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, con))
            using (var da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@orderId", orderId);
                da.Fill(dt);
            }

            // ✅ เพิ่มคอลัมน์ใหม่
            dt.Columns.Add("OrderByFullName", typeof(string));
            dt.Columns.Add("ApprovedByFullName", typeof(string));

            // ✅ เติมค่าชื่อเต็มลงไป
            foreach (DataRow row in dt.Rows)
            {
                var empId = row["emp_id"]?.ToString();
                var approvedId = row["approved_by_emp_id"]?.ToString();

                row["OrderByFullName"] = GetEmployeeFullName(empId);
                row["ApprovedByFullName"] = GetEmployeeFullName(approvedId);
            }

            return dt;
        }


        //งงว่าต้องส่งไปที่ไหน
        private string GetEmployeeFullName(string empId)
        {
            string sql = "SELECT emp_name, emp_lname FROM employee WHERE emp_id = @empId";
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@empId", empId);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return $"{reader["emp_name"]} {reader["emp_lname"]}";
                }
            }
            return empId; // fallback
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
            using (var con = new MySqlConnection(ConnectionString))
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
                    $"PurchaseOrder_{OrderId}.pdf"
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
