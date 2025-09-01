using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication.Sitesupervisor
{
    public partial class WorkResponse : UserControl
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        
        public WorkResponse()
        {
            InitializeComponent();
            SetupGrid();
            LoadWorkResponse();
        }
        private void SetupGrid()
        {
            dtgvWorkResponse.AutoGenerateColumns = false;
            dtgvWorkResponse.AllowUserToAddRows = false;
            dtgvWorkResponse.ReadOnly = true;
            dtgvWorkResponse.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvWorkResponse.MultiSelect = false;
            dtgvWorkResponse.RowHeadersVisible = false;
            dtgvWorkResponse.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dtgvWorkResponse.Columns.Clear();

            // ลำดับ
            var colIndex = new DataGridViewTextBoxColumn
            {
                Name = "colIndex",
                HeaderText = "ลำดับ",
                ReadOnly = true
            };
            dtgvWorkResponse.Columns.Add(colIndex);

            // 🏷️ รหัสรายการ (phase_no + order_number)
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colOrderNo",
                HeaderText = "รหัสรายการ",
                DataPropertyName = "display_key"
            });

            // 🧭 รหัสโครงการ
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProId",
                HeaderText = "รหัสโครงการ",
                DataPropertyName = "pro_id"
            });

            // 🧩 เฟสที่ (phase_no)
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPhase",
                HeaderText = "เฟสที่",
                DataPropertyName = "phase_no"
            });

            // 📝 รายละเอียด (order_detail)
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDetail",
                HeaderText = "รายละเอียด",
                DataPropertyName = "order_detail"
            });

            // 📅 วันที่ (order_date)
            var colDate = new DataGridViewTextBoxColumn
            {
                Name = "colOrderDate",
                HeaderText = "วันที่",
                DataPropertyName = "order_date",
                DefaultCellStyle = { Format = "yyyy-MM-dd" }
            };
            dtgvWorkResponse.Columns.Add(colDate);

            // 🚦 สถานะ (order_status)
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "สถานะ",
                DataPropertyName = "order_status"
            });

            // วาดเลขลำดับอัตโนมัติ
            dtgvWorkResponse.RowPostPaint += (s, e) =>
            {
                dtgvWorkResponse.Rows[e.RowIndex].Cells["colIndex"].Value = (e.RowIndex + 1).ToString();
            };
        }
        private void LoadWorkResponse()
        {
            string sql = @"
                SELECT 
                    po.order_id,
                    po.order_number,
                    po.order_detail,
                    po.order_date,
                    po.order_status,
                    pp.phase_id,
                    pp.phase_no,
                    pp.phase_detail,
                    pp.pro_id
                FROM 
                    purchaseorder po
                INNER JOIN 
                    project_phase pp ON po.pro_id = pp.pro_id
                -- หากต้องการ Filter เฉพาะ เพิ่ม WHERE ได้
            ";

            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, con))
            using (var da = new MySqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                // สร้างคอลัมน์ "display_key" สำหรับโชว์รหัสรายการ (Phase + Order)
                dt.Columns.Add("display_key", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row["display_key"] = $"Phase: {row["phase_no"]} | Order: {row["order_number"]}";
                }

                dtgvWorkResponse.DataSource = dt;
            }
        }

        
    }
}
