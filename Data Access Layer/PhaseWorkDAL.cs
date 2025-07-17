using JRSApplication.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace JRSApplication.Data_Access_Layer
{
    public class PhaseWorkDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        // 📌 ดึงประวัติการดำเนินงานตาม Phase ID
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
                ORDER BY work_date DESC";

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

        // 📌 Insert ข้อมูลการดำเนินงาน (Working Phase)
        public bool Insert(PhaseWorking phaseWork)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"
                    INSERT INTO phase_working 
                        (phase_id, pro_id, work_detail, work_status, work_date, update_date, remark, emp_id, start_date, end_date) 
                    VALUES 
                        (@PhaseID, @ProjectID, @WorkDetail, @WorkStatus, @WorkDate, @UpdateDate, @Remark, @EmpID, @StartDate, @EndDate)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PhaseID", phaseWork.PhaseID);
                    cmd.Parameters.AddWithValue("@ProjectID", phaseWork.ProjectID);
                    cmd.Parameters.AddWithValue("@WorkDetail", phaseWork.WorkDetail ?? "");
                    cmd.Parameters.AddWithValue("@WorkStatus", phaseWork.WorkStatus ?? "InProgress");
                    cmd.Parameters.AddWithValue("@WorkDate", phaseWork.WorkDate);
                    cmd.Parameters.AddWithValue("@UpdateDate", phaseWork.UpdateDate);
                    cmd.Parameters.AddWithValue("@Remark", phaseWork.Remark ?? "");
                    cmd.Parameters.AddWithValue("@EmpID", phaseWork.EmployeeID);
                    cmd.Parameters.AddWithValue("@StartDate", phaseWork.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", phaseWork.EndDate);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
    }
}
