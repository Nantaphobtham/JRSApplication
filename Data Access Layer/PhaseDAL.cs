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
    public class PhaseDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public List<ProjectPhase> GetAllPhasesByPrjectID(int projectId)
        {
            List<ProjectPhase> phases = new List<ProjectPhase>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"
                         SELECT 
                              phase_no, 
                              phase_detail, 
                              phase_budget, 
                              phase_percent
                              FROM project_phase
                         WHERE pro_id = @ProjectID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            phases.Add(new ProjectPhase
                            {
                                PhaseNumber = reader.GetInt32("phase_no"),
                                PhaseDetail = reader.GetString("phase_detail"),
                                PhaseBudget = reader.GetDecimal("phase_budget"),
                                PhasePercent = reader.GetDecimal("phase_percent")
                            });
                        }
                    }
                }
            }

            return phases;
        }
        public List<PhaseWithStatus> GetPhasesWithStatus(int projectId)
        {
            List<PhaseWithStatus> phases = new List<PhaseWithStatus>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"
                    SELECT 
                        pp.phase_id,
                        pp.phase_no,
                        pp.phase_detail,
                        pp.phase_budget,
                        pp.phase_percent,
                        pw.work_status
                    FROM project_phase pp
                    LEFT JOIN phase_working pw 
                        ON pw.phase_id = pp.phase_id
                        AND pw.work_date = (
                            SELECT MAX(w2.work_date)
                            FROM phase_working w2
                            WHERE w2.phase_id = pw.phase_id
                        )
                    WHERE pp.pro_id = @ProjectID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            phases.Add(new PhaseWithStatus
                            {
                                PhaseNumber = reader.GetInt32("phase_no"),
                                PhaseDetail = reader.GetString("phase_detail"),
                                PhaseBudget = reader.GetDecimal("phase_budget"),
                                PhasePercent = reader.GetDecimal("phase_percent"),
                                PhaseStatus = reader.IsDBNull(reader.GetOrdinal("work_status"))
                                    ? WorkStatus.NotStarted
                                    : reader.GetString("work_status")
                            });
                        }
                    }
                }
            }

            return phases;
        }

        //ไม่ถูกใช้งานค่อยลลบ 
        public int GetPhaseCountByProjectID(int projectId)
        {
            int count = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) FROM project_phase WHERE pro_id = @ProjectID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                    conn.Open();
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return count;
        }

        public List<ProjectPhase> GetPhasesByProjectID(int projectId)
        {
            List<ProjectPhase> phases = new List<ProjectPhase>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT phase_id, phase_no, phase_detail FROM project_phase WHERE pro_id = @ProjectID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            phases.Add(new ProjectPhase
                            {
                                PhaseID = reader.GetInt32("phase_id"),
                                PhaseNumber = reader.GetInt32("phase_no"),
                                PhaseDetail = reader.GetString("phase_detail")
                            });
                        }
                    }
                }
            }

            return phases;
        }
        public (decimal budget, string detail) GetPhaseBudgetAndDetail(string phaseId)
        {
            decimal budget = 0;
            string detail = "";

            string query = "SELECT phase_budget, phase_detail FROM project_phase WHERE phase_id = @phaseId";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@phaseId", phaseId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        budget = reader.GetDecimal("phase_budget");
                        detail = reader.GetString("phase_detail");
                    }
                }
            }

            return (budget, detail);
        }
        public class PhaseData
        {
            public decimal Budget { get; set; }
            public string Detail { get; set; }
        }

        public PhaseData GetPhaseDataById(string phaseId)
        {
            string query = "SELECT phase_budget, phase_detail FROM project_phase WHERE phase_id = @phaseId";
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@phaseId", phaseId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new PhaseData
                        {
                            Budget = reader.GetDecimal("phase_budget"),
                            Detail = reader.GetString("phase_detail")
                        };
                    }
                }
            }
            return null;
        }


    }
}
