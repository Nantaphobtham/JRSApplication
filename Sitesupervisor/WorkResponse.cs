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

            // 🧮 ลำดับ (คอลัมน์ไม่ผูกข้อมูล)
            var colIndex = new DataGridViewTextBoxColumn
            {
                Name = "colIndex",
                HeaderText = "ลำดับ",
                ReadOnly = true
            };
            dtgvWorkResponse.Columns.Add(colIndex);

            // 🏷️ รหัสรายการ
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colOrderNo",
                HeaderText = "รหัสรายการ",
                DataPropertyName = "order_no" // ควรเป็น order_id และ phase_id
            });

            // 🧭 รหัสโครงการ
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProId",
                HeaderText = "รหัสโครงการ",
                DataPropertyName = "pro_id"
            });

            // 🧩 เฟสที่
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPhase",
                HeaderText = "เฟสที่",
                DataPropertyName = "phase_name"
            });

            // 📝 รายละเอียด
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDetail",
                HeaderText = "รายละเอียด",
                DataPropertyName = "detail"
            });

            // 📅 วันที่
            var colDate = new DataGridViewTextBoxColumn
            {
                Name = "colOrderDate",
                HeaderText = "วันที่",
                DataPropertyName = "order_date",
                DefaultCellStyle = { Format = "yyyy-MM-dd" }
            };
            dtgvWorkResponse.Columns.Add(colDate);

            // 🚦 สถานะ
            dtgvWorkResponse.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "สถานะ",
                DataPropertyName = "status"
            });

            // วาดเลขลำดับอัตโนมัติ
            dtgvWorkResponse.RowPostPaint += (s, e) =>
            {
                dtgvWorkResponse.Rows[e.RowIndex].Cells["colIndex"].Value = (e.RowIndex + 1).ToString();
            };
        }
    }
}
