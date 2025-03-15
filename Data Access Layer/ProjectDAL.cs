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
    public class ProjectDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public int GenerateProjectID()
        {
            int newID = int.Parse(DateTime.Now.ToString("yyMM") + "000"); // เริ่มที่ 2503000
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "SELECT MAX(pro_id) FROM project WHERE pro_id LIKE @Prefix";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Prefix", DateTime.Now.ToString("yyMM") + "%");
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        newID = Convert.ToInt32(result) + 1; // รันเลขถัดไป
                    }
                }
            }
            return newID;
        }

        public bool InsertProjectWithPhases(Project project, List<ProjectPhase> phases)
        {
            bool isSuccess = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // ✅ 1️⃣ บันทึก `Project`
                        string sqlProject = "INSERT INTO project (pro_id, pro_name, pro_detail, pro_address, pro_budget, pro_start, pro_end, " +
                                            "pro_currentphasenumber, pro_remark, pro_number, pro_con_blueprint, pro_demolition_model, emp_id, cus_id) " +
                                            "VALUES (@ProjectID, @ProjectName, @ProjectDetail, @ProjectAddress, @ProjectBudget, @ProjectStart, @ProjectEnd, " +
                                            "@CurrentPhaseNumber, @Remark, @ProjectNumber, @ConstructionBlueprint, @DemolitionModel, @EmployeeID, @CustomerID)";

                        using (MySqlCommand cmd = new MySqlCommand(sqlProject, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                            cmd.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                            cmd.Parameters.AddWithValue("@ProjectDetail", project.ProjectDetail);
                            cmd.Parameters.AddWithValue("@ProjectAddress", project.ProjectAddress);
                            cmd.Parameters.AddWithValue("@ProjectBudget", project.ProjectBudget);
                            cmd.Parameters.AddWithValue("@ProjectStart", project.ProjectStart);
                            cmd.Parameters.AddWithValue("@ProjectEnd", project.ProjectEnd);
                            cmd.Parameters.AddWithValue("@CurrentPhaseNumber", project.CurrentPhaseNumber);
                            cmd.Parameters.AddWithValue("@Remark", project.Remark);
                            cmd.Parameters.AddWithValue("@ProjectNumber", project.ProjectNumber);
                            cmd.Parameters.AddWithValue("@ConstructionBlueprint", project.ConstructionBlueprint);
                            cmd.Parameters.AddWithValue("@DemolitionModel", project.DemolitionModel);
                            cmd.Parameters.AddWithValue("@EmployeeID", project.EmployeeID);
                            cmd.Parameters.AddWithValue("@CustomerID", project.CustomerID);

                            cmd.ExecuteNonQuery();
                        }

                        // ✅ 2️⃣ บันทึก `ProjectPhase`
                        foreach (var phase in phases)
                        {
                            string sqlPhase = "INSERT INTO project_phase (phase_no, phase_detail, phase_budget, phase_percent, pro_id) " +
                                              "VALUES (@PhaseNo, @PhaseDetail, @PhaseBudget, @PhasePercent, @ProjectID)";

                            using (MySqlCommand cmdPhase = new MySqlCommand(sqlPhase, conn, transaction))
                            {
                                cmdPhase.Parameters.AddWithValue("@PhaseNo", phase.PhaseNumber);
                                cmdPhase.Parameters.AddWithValue("@PhaseDetail", phase.PhaseDetail);
                                cmdPhase.Parameters.AddWithValue("@PhaseBudget", phase.PhaseBudget);
                                cmdPhase.Parameters.AddWithValue("@PhasePercent", phase.PhasePercent);
                                cmdPhase.Parameters.AddWithValue("@ProjectID", project.ProjectID);

                                cmdPhase.ExecuteNonQuery();
                            }
                        }

                        // ✅ 3️⃣ Commit Transaction
                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error saving project and phases: " + ex.Message);
                    }
                }
            }
            return isSuccess;
        }


    }
}
