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
        public PhaseWorking GetPhaseWorking(int projectId, int phaseNo)
        {
            PhaseWorking phase = null;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM phase_working 
                       WHERE pro_id = @ProjectID AND phase_no = @PhaseNo";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                    cmd.Parameters.AddWithValue("@PhaseNo", phaseNo);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            phase = new PhaseWorking
                            {
                                PhaseID = reader.GetInt32("phase_line_no"),
                                PhaseNo = reader.GetInt32("phase_no"),
                                WorkStatus = reader.GetString("work_status"),
                                // ✅ เพิ่มเติม field อื่นได้ตามต้องการ
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
                            PhaseID = reader.GetInt32("phase_line_no"),
                            PhaseNo = reader.GetInt32("phase_no"),
                            WorkStatus = reader.GetString("work_status"),
                            WorkDate = reader.GetDateTime("work_date"),
                            EndDate = reader.IsDBNull(reader.GetOrdinal("work_end_date")) ? DateTime.MinValue : reader.GetDateTime("work_end_date"),
                            UpdateDate = reader.IsDBNull(reader.GetOrdinal("work_update_date")) ? DateTime.MinValue : reader.GetDateTime("work_update_date"),
                            WorkDetail = reader.GetString("work_detail"),
                            Remark = reader.GetString("work_remark"),
                            ProjectID = projectId
                        });
                    }
                }
            }

            return list;
        }


    }

}
