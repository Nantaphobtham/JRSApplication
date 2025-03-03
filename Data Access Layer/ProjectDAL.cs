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

        // ✅ 1️⃣ ดึงข้อมูลโครงการทั้งหมด
        public List<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM project";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            projects.Add(new Project
                            {
                                ProjectID = reader.GetInt32("pro_id"),
                                ProjectName = reader.GetString("pro_name"),
                                ProjectDetail = reader.IsDBNull(reader.GetOrdinal("pro_detail")) ? "" : reader.GetString("pro_detail"),
                                ProjectAddress = reader.IsDBNull(reader.GetOrdinal("pro_address")) ? "" : reader.GetString("pro_address"),
                                ProjectBudget = reader.IsDBNull(reader.GetOrdinal("pro_budget")) ? 0 : reader.GetDecimal("pro_budget"),
                                ProjectStart = reader.GetDateTime("pro_start"),
                                ProjectEnd = reader.GetDateTime("pro_end"),
                                CurrentPhaseNumber = reader.IsDBNull(reader.GetOrdinal("pro_currentphasenumber")) ? 0 : reader.GetInt32("pro_currentphasenumber"),
                                Remark = reader.IsDBNull(reader.GetOrdinal("pro_remark")) ? "" : reader.GetString("pro_remark"),
                                ProjectNumber = reader.GetString("pro_number"),
                                ConstructionBlueprint = reader.IsDBNull(reader.GetOrdinal("pro_con_blueprint")) ? "" : reader.GetString("pro_con_blueprint"),
                                DemolitionModel = reader.IsDBNull(reader.GetOrdinal("pro_demolition_model")) ? "" : reader.GetString("pro_demolition_model"),
                                EmployeeID = reader.GetInt32("emp_id"),
                                CustomerID = reader.GetInt32("cus_id")
                            });
                        }
                    }
                }
            }
            return projects;
        }

        public bool InsertProject(Project project)
        {
            bool isSuccess = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "INSERT INTO project (pro_name, pro_detail, pro_address, pro_budget, pro_start, pro_end, " +
                             "pro_currentphasenumber, pro_remark, pro_number, pro_con_blueprint, pro_demolition_model, emp_id, cus_id) " +
                             "VALUES (@ProjectName, @ProjectDetail, @ProjectAddress, @ProjectBudget, @ProjectStart, @ProjectEnd, " +
                             "@CurrentPhaseNumber, @Remark, @ProjectNumber, @ConstructionBlueprint, @DemolitionModel, @EmployeeID, @CustomerID)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
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

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    isSuccess = rows > 0;
                }
            }
            return isSuccess;
        }

        public bool DeleteProject(int projectID)
        {
            bool isSuccess = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "DELETE FROM project WHERE pro_id = @ProjectID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectID);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    isSuccess = rows > 0;
                }
            }
            return isSuccess;
        }


    }
}
