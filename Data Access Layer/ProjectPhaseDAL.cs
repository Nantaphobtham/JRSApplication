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
    public class ProjectPhaseDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        // ✅ 1️⃣ ดึงข้อมูลเฟสของโครงการตาม `pro_id`
        public List<ProjectPhase> GetPhasesByProject(int projectID)
        {
            List<ProjectPhase> phases = new List<ProjectPhase>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM project_phase WHERE pro_id = @ProjectID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            phases.Add(new ProjectPhase
                            {
                                PhaseNumber = reader.GetInt32("phase_no"),
                                PhaseDetail = reader.GetString("phase_detail"),
                                PhaseBudget = reader.IsDBNull(reader.GetOrdinal("phase_budget")) ? 0 : reader.GetDecimal("phase_budget"),
                                PhaseRemark = reader.IsDBNull(reader.GetOrdinal("phase_remark")) ? "" : reader.GetString("phase_remark"),
                                ProjectID = reader.GetInt32("pro_id"),
                                SupplierID = reader.IsDBNull(reader.GetOrdinal("sup_id")) ? "" : reader.GetString("sup_id")
                            });
                        }
                    }
                }
            }
            return phases;
        }
    }
}
