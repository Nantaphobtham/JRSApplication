using JRSApplication.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Data_Access_Layer
{
    public class PhaseWorkDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public PhaseWorking GetPhaseWorking(int projectId, int phaseId)
        {
            PhaseWorking phase = null;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM phase_working 
                           WHERE pro_id = @ProjectID AND phase_id = @PhaseID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                    cmd.Parameters.AddWithValue("@PhaseID", phaseId);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            phase = new PhaseWorking
                            {
                                WorkID = reader.GetInt32("work_id"),
                                PhaseID = reader.GetInt32("phase_id"),
                                WorkStatus = reader.GetString("work_status"),
                                WorkDate = reader.GetDateTime("work_date"),
                                EndDate = reader.IsDBNull(reader.GetOrdinal("work_end_date")) ? DateTime.MinValue : reader.GetDateTime("work_end_date"),
                                UpdateDate = reader.IsDBNull(reader.GetOrdinal("work_update_date")) ? DateTime.MinValue : reader.GetDateTime("work_update_date"),
                                WorkDetail = reader.GetString("work_detail"),
                                Remark = reader.GetString("work_remark"),
                                ProjectID = projectId,
                                SupplierID = reader.IsDBNull(reader.GetOrdinal("sup_id")) ? null : reader.GetString("sup_id")
                            };
                        }
                    }
                }
            }

            return phase;
        }

        public List<PhaseWorking> GetPhaseWorkingsByProjectID(int projectId)
        {
            List<PhaseWorking> list = new List<PhaseWorking>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM phase_working WHERE pro_id = @ProjectID";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProjectID", projectId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PhaseWorking
                        {
                            WorkID = reader.GetInt32("work_id"),
                            PhaseID = reader.GetInt32("phase_id"),
                            WorkStatus = reader.GetString("work_status"),
                            WorkDate = reader.GetDateTime("work_date"),
                            EndDate = reader.IsDBNull(reader.GetOrdinal("work_end_date")) ? DateTime.MinValue : reader.GetDateTime("work_end_date"),
                            UpdateDate = reader.IsDBNull(reader.GetOrdinal("work_update_date")) ? DateTime.MinValue : reader.GetDateTime("work_update_date"),
                            WorkDetail = reader.GetString("work_detail"),
                            Remark = reader.GetString("work_remark"),
                            ProjectID = projectId,
                            SupplierID = reader.IsDBNull(reader.GetOrdinal("sup_id")) ? null : reader.GetString("sup_id")
                        });
                    }
                }
            }

            return list;
        }
        //มีปัญหา insert data : error  Insert phase_id ที่ไม่มีอยู่ในตาราง project_phase ผิด Foreign Key constraint ที่ผูกไว้
        public bool InsertPhaseWorking(PhaseWorking phase)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO phase_working
                      (phase_id, work_detail, work_status, work_date, work_end_date, work_update_date, work_remark, pro_id, sup_id)
                      VALUES
                      (@PhaseID, @WorkDetail, @WorkStatus, @WorkDate, @EndDate, @UpdateDate, @Remark, @ProjectID, @SupplierID)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PhaseID", phase.PhaseID);
                    cmd.Parameters.AddWithValue("@WorkDetail", phase.WorkDetail);
                    cmd.Parameters.AddWithValue("@WorkStatus", phase.WorkStatus);
                    cmd.Parameters.AddWithValue("@WorkDate", phase.WorkDate);
                    cmd.Parameters.AddWithValue("@EndDate", phase.EndDate);
                    cmd.Parameters.AddWithValue("@UpdateDate", phase.UpdateDate);
                    cmd.Parameters.AddWithValue("@Remark", phase.Remark);
                    cmd.Parameters.AddWithValue("@ProjectID", phase.ProjectID);
                    cmd.Parameters.AddWithValue("@SupplierID", string.IsNullOrEmpty(phase.SupplierID) ? DBNull.Value : (object)phase.SupplierID);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public PhaseWorking GetPhaseWorkingByPhaseID(int projectId, int phaseId)
        {
            PhaseWorking phase = null;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM phase_working 
                       WHERE pro_id = @ProjectID AND phase_id = @PhaseID
                       ORDER BY work_date DESC
                       LIMIT 1"; // ✅ ดึงเฉพาะล่าสุด

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                    cmd.Parameters.AddWithValue("@PhaseID", phaseId);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            phase = new PhaseWorking
                            {
                                WorkStatus = reader.GetString("work_status"),
                                // ✅ เพิ่ม field อื่นตามต้องการ
                            };
                        }
                    }
                }
            }

            return phase;
        }

    }


}
