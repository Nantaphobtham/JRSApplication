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

        public decimal GetTotalPhaseBudget(int projectId)
        {
            decimal totalBudget = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "SELECT SUM(phase_budget) FROM project_phase WHERE pro_id = @ProjectID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        totalBudget = Convert.ToDecimal(result);
                    }
                }
            }
            return totalBudget;
        }


    }
}
