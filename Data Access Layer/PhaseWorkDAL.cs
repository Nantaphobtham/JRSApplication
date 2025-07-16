using JRSApplication.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication.Data_Access_Layer
{
    public class PhaseWorkDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public DataTable GetWorkingHistoryByPhase(int phaseId)
        {
            DataTable dt = new DataTable();

            string query = @"
                        SELECT 
                            work_id                 AS 'ลำดับ',
                            work_detail             AS 'รายละเอียดการดำเนินงาน',
                            work_status             AS 'สถานะ',
                            work_date               AS 'วันที่บันทึก',
                            work_end_date           AS 'วันสิ้นสุดจริง',
                            work_update_date        AS 'วันที่อัปเดต',
                            work_remark             AS 'หมายเหตุ',
                            supplier_assignment_id  AS 'รหัสงาน Subcontractor'
                        FROM phase_working
                        WHERE phase_id = @PhaseId
                        ORDER BY work_date DESC
                    ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PhaseId", phaseId);
                conn.Open();
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }


    }
}








    

