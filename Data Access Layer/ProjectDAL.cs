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

        public List<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"
                        SELECT 
                            p.pro_id, 
                            p.pro_name, 
                            p.pro_detail, 
                            p.pro_address, 
                            p.pro_budget, 
                            p.pro_start, 
                            p.pro_end, 
                            p.pro_currentphasenumber, 
                            CONCAT(c.cus_name, ' ', c.cus_lname) AS CustomerFullName,
                            CONCAT(e.emp_name, ' ', e.emp_lname) AS EmployeeFullName
                        FROM project p
                        LEFT JOIN customer c ON p.cus_id = c.cus_id
                        LEFT JOIN employee e ON p.emp_id = e.emp_id";

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

                                //  แสดงชื่อ-นามสกุลรวม
                                CustomerName = reader.IsDBNull(reader.GetOrdinal("CustomerFullName")) ? "ไม่ระบุ" : reader.GetString("CustomerFullName"),
                                EmployeeName = reader.IsDBNull(reader.GetOrdinal("EmployeeFullName")) ? "ไม่ระบุ" : reader.GetString("EmployeeFullName")
                            });
                        }
                    }
                }
            }

            return projects;
        }

        public Project GetProjectDetailsById(int projectId)
        {
            Project project = null;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // ✅ โหลด Project + Customer + Employee + Files
                string sql = @"
                            SELECT 
                                p.pro_id AS ProjectID,
                                p.pro_name AS ProjectName,
                                p.pro_start AS ProjectStart,
                                p.pro_end AS ProjectEnd,
                                p.pro_number AS ContractNumber,
                                p.pro_budget AS ProjectBudget,
                                p.pro_detail AS ProjectDetail,
                                p.pro_address AS ProjectAddress,
                                p.pro_currentphasenumber AS CurrentPhaseNumber,
                                p.pro_remark AS Remark,

                                CONCAT(c.cus_name, ' ', c.cus_lname) AS CustomerFullName,
                                CONCAT(e.emp_name, ' ', e.emp_lname) AS ProjectManagerFullName,

                                pf.con_blueprint AS ConstructionBlueprint,
                                pf.demolition_model AS DemolitionModel

                            FROM project p
                            LEFT JOIN customer c ON p.cus_id = c.cus_id
                            LEFT JOIN employee e ON p.emp_id = e.emp_id
                            LEFT JOIN project_files pf ON p.pro_id = pf.pro_id
                            WHERE p.pro_id = @ProjectID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            project = new Project
                            {
                                ProjectID = reader.GetInt32("ProjectID"),
                                ProjectName = reader.GetString("ProjectName"),
                                ProjectStart = reader.GetDateTime("ProjectStart"),
                                ProjectEnd = reader.GetDateTime("ProjectEnd"),
                                ProjectNumber = reader.GetString("ContractNumber"),
                                ProjectDetail = reader.GetString("ProjectDetail"),
                                ProjectAddress = reader.IsDBNull(reader.GetOrdinal("ProjectAddress")) ? "" : reader.GetString("ProjectAddress"),
                                ProjectBudget = reader.GetDecimal("ProjectBudget"),
                                Remark = reader.IsDBNull(reader.GetOrdinal("Remark")) ? "" : reader.GetString("Remark"),
                                CurrentPhaseNumber = reader.IsDBNull(reader.GetOrdinal("CurrentPhaseNumber")) ? 0 : reader.GetInt32("CurrentPhaseNumber"),

                                CustomerName = reader.IsDBNull(reader.GetOrdinal("CustomerFullName")) ? "ไม่มีข้อมูล" : reader.GetString("CustomerFullName"),
                                EmployeeName = reader.IsDBNull(reader.GetOrdinal("ProjectManagerFullName")) ? "ไม่มีข้อมูล" : reader.GetString("ProjectManagerFullName"),

                                ProjectFile = new ProjectFile
                                {
                                    ConstructionBlueprint = reader["ConstructionBlueprint"] != DBNull.Value
                                        ? (byte[])reader["ConstructionBlueprint"]
                                        : null,
                                    DemolitionModel = reader["DemolitionModel"] != DBNull.Value
                                        ? (byte[])reader["DemolitionModel"]
                                        : null
                                }
                            };
                        }
                    }
                }

                // ✅ โหลดเฟสทั้งหมดที่เกี่ยวกับ ProjectID
                if (project != null)
                {
                    string phaseSql = @"
                                    SELECT phase_no, phase_detail, phase_budget, phase_percent
                                    FROM project_phase
                                    WHERE pro_id = @ProjectID
                                    ORDER BY phase_no";

                    using (MySqlCommand phaseCmd = new MySqlCommand(phaseSql, conn))
                    {
                        phaseCmd.Parameters.AddWithValue("@ProjectID", projectId);
                        using (MySqlDataReader phaseReader = phaseCmd.ExecuteReader())
                        {
                            List<ProjectPhase> phases = new List<ProjectPhase>();
                            while (phaseReader.Read())
                            {
                                phases.Add(new ProjectPhase
                                {
                                    PhaseNumber = phaseReader.GetInt32("phase_no"),
                                    PhaseDetail = phaseReader.GetString("phase_detail"),
                                    PhaseBudget = phaseReader.GetDecimal("phase_budget"),
                                    PhasePercent = phaseReader.GetDecimal("phase_percent")
                                });
                            }

                            project.Phases = phases;
                        }
                    }
                }
            }

            return project;
        }


        public int GenerateProjectID()
        {
            int newID = int.Parse(DateTime.Now.ToString("yyMM") + "000"); // เริ่มที่ 2503000 เช่น ถ้าเป็นปี 2025 เดือน 03 = 2503000
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "SELECT MAX(pro_id) FROM project WHERE pro_id LIKE @Prefix";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Prefix", DateTime.Now.ToString("yyMM") + "%"); // หา MAX ของเดือนนี้
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        newID = Convert.ToInt32(result) + 1; // ถ้ามีอยู่แล้ว ให้บวกเพิ่มจาก MAX
                    }
                }
            }
            return newID;
        }

        public bool InsertProjectWithPhases(Project project, List<ProjectPhase> phases, byte[] constructionBlueprint, byte[] demolitionModel)
        {
            bool isSuccess = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        //  1️ Insert into project
                        string sqlProject = @"
                                INSERT INTO project (pro_id, pro_name, pro_detail, pro_address, pro_budget, pro_start, pro_end,
                                                     pro_currentphasenumber, pro_remark, pro_number, emp_id, cus_id)
                                VALUES (@ProjectID, @ProjectName, @ProjectDetail, @ProjectAddress, @ProjectBudget, @ProjectStart, @ProjectEnd,
                                        @CurrentPhaseNumber, @Remark, @ProjectNumber, @EmployeeID, @CustomerID)";

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
                            cmd.Parameters.AddWithValue("@EmployeeID", project.EmployeeID);
                            cmd.Parameters.AddWithValue("@CustomerID", project.CustomerID);

                            cmd.ExecuteNonQuery();
                        }

                        //  2️ Insert into project_phase
                        foreach (var phase in phases)
                        {
                            string sqlPhase = @"
                                    INSERT INTO project_phase (phase_no, phase_detail, phase_budget, phase_percent, pro_id)
                                    VALUES (@PhaseNo, @PhaseDetail, @PhaseBudget, @PhasePercent, @ProjectID)";

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

                        //  3️ Insert into project_files (เฉพาะเมื่อ ConstructionBlueprint มีข้อมูล)
                        if (constructionBlueprint != null)
                        {
                            string sqlFile = @"
                                    INSERT INTO project_files (pro_id, con_blueprint, demolition_model)
                                    VALUES (@ProjectID, @ConstructionBlueprint, @DemolitionModel)";

                            using (MySqlCommand cmdFile = new MySqlCommand(sqlFile, conn, transaction))
                            {
                                cmdFile.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                                cmdFile.Parameters.AddWithValue("@ConstructionBlueprint", constructionBlueprint);
                                cmdFile.Parameters.AddWithValue("@DemolitionModel",
                                    demolitionModel != null ? (object)demolitionModel : DBNull.Value);

                                cmdFile.ExecuteNonQuery();
                            }
                        }

                        //  4️ Commit
                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error saving project, phases, or files: " + ex.Message);
                    }
                }
            }
            return isSuccess;
        }

        public bool UpdateProjectWithPhases(Project project, List<ProjectPhase> updatedPhases, byte[] blueprintBytes, byte[] demolitionBytes)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // ✅ 1) UPDATE Project
                        string updateProjectSql = @"
                                                UPDATE project SET
                                                    pro_name = @ProjectName,
                                                    pro_detail = @ProjectDetail,
                                                    pro_address = @ProjectAddress,
                                                    pro_budget = @ProjectBudget,
                                                    pro_start = @ProjectStart,
                                                    pro_end = @ProjectEnd,
                                                    pro_currentphasenumber = @CurrentPhaseNumber,
                                                    pro_remark = @Remark,
                                                    pro_number = @ProjectNumber,
                                                    emp_id = @EmployeeID,
                                                    cus_id = @CustomerID
                                                WHERE pro_id = @ProjectID";

                        using (MySqlCommand cmd = new MySqlCommand(updateProjectSql, conn, tran))
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
                            cmd.Parameters.AddWithValue("@EmployeeID", project.EmployeeID);
                            cmd.Parameters.AddWithValue("@CustomerID", project.CustomerID);
                            cmd.ExecuteNonQuery();
                        }

                        // ✅ 2) ดึง Phase ปัจจุบันจาก DB
                        List<ProjectPhase> existingPhases = new List<ProjectPhase>();
                        string selectPhaseSql = "SELECT phase_no, phase_detail, phase_budget, phase_percent FROM project_phase WHERE pro_id = @ProjectID";

                        using (MySqlCommand cmdSelect = new MySqlCommand(selectPhaseSql, conn, tran))
                        {
                            cmdSelect.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                            using (MySqlDataReader reader = cmdSelect.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    existingPhases.Add(new ProjectPhase
                                    {
                                        PhaseNumber = Convert.ToInt32(reader["phase_no"]),
                                        PhaseDetail = reader["phase_detail"].ToString(),
                                        PhaseBudget = Convert.ToDecimal(reader["phase_budget"]),
                                        PhasePercent = Convert.ToDecimal(reader["phase_percent"])
                                    });
                                }
                            }
                        }

                        // ✅ 3) Loop UPDATE หรือ INSERT
                        foreach (var phase in updatedPhases)
                        {
                            var match = existingPhases.FirstOrDefault(p => p.PhaseNumber == phase.PhaseNumber);

                            if (match != null)
                            {
                                // ✅ UPDATE
                                string updatePhaseSql = @"
                                                UPDATE project_phase
                                                SET phase_detail = @PhaseDetail,
                                                    phase_budget = @PhaseBudget,
                                                    phase_percent = @PhasePercent
                                                WHERE pro_id = @ProjectID AND phase_no = @PhaseNo";

                                using (MySqlCommand cmdUpdate = new MySqlCommand(updatePhaseSql, conn, tran))
                                {
                                    cmdUpdate.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                                    cmdUpdate.Parameters.AddWithValue("@PhaseNo", phase.PhaseNumber);
                                    cmdUpdate.Parameters.AddWithValue("@PhaseDetail", phase.PhaseDetail);
                                    cmdUpdate.Parameters.AddWithValue("@PhaseBudget", phase.PhaseBudget);
                                    cmdUpdate.Parameters.AddWithValue("@PhasePercent", phase.PhasePercent);
                                    cmdUpdate.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // ✅ INSERT
                                string insertSql = @"
                            INSERT INTO project_phase (pro_id, phase_no, phase_detail, phase_budget, phase_percent)
                            VALUES (@ProjectID, @PhaseNo, @PhaseDetail, @PhaseBudget, @PhasePercent)";

                                using (MySqlCommand cmdInsert = new MySqlCommand(insertSql, conn, tran))
                                {
                                    cmdInsert.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                                    cmdInsert.Parameters.AddWithValue("@PhaseNo", phase.PhaseNumber);
                                    cmdInsert.Parameters.AddWithValue("@PhaseDetail", phase.PhaseDetail);
                                    cmdInsert.Parameters.AddWithValue("@PhaseBudget", phase.PhaseBudget);
                                    cmdInsert.Parameters.AddWithValue("@PhasePercent", phase.PhasePercent);
                                    cmdInsert.ExecuteNonQuery();
                                }
                            }
                        }

                        // ✅ 4) DELETE Phase ที่ไม่อยู่ในรายการใหม่
                        var toDelete = existingPhases
                            .Where(old => !updatedPhases.Any(up => up.PhaseNumber == old.PhaseNumber))
                            .ToList();

                        foreach (var del in toDelete)
                        {
                            string deleteSql = "DELETE FROM project_phase WHERE pro_id = @ProjectID AND phase_no = @PhaseNo";
                            using (MySqlCommand cmdDelete = new MySqlCommand(deleteSql, conn, tran))
                            {
                                cmdDelete.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                                cmdDelete.Parameters.AddWithValue("@PhaseNo", del.PhaseNumber);
                                cmdDelete.ExecuteNonQuery();
                            }
                        }

                        // ✅ 5) UPDATE/INSERT ไฟล์
                        string upsertFileSql = @"
                                        INSERT INTO project_files (pro_id, con_blueprint, demolition_model)
                                        VALUES (@ProjectID, @Blueprint, @Demolition)
                                        ON DUPLICATE KEY UPDATE
                                            con_blueprint = @Blueprint,
                                            demolition_model = @Demolition";

                        using (MySqlCommand cmdFile = new MySqlCommand(upsertFileSql, conn, tran))
                        {
                            cmdFile.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                            cmdFile.Parameters.AddWithValue("@Blueprint", blueprintBytes ?? (object)DBNull.Value);
                            cmdFile.Parameters.AddWithValue("@Demolition", demolitionBytes ?? (object)DBNull.Value);
                            cmdFile.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception("Error updating project and phases: " + ex.Message);
                    }
                }
            }
        }

        public bool DeleteProjectWithPhases(int projectId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Delete phase_working
                    string deleteWorkSql = @"
                DELETE pw 
                FROM phase_working pw
                JOIN project_phase pp ON pw.phase_id = pp.phase_id
                WHERE pp.pro_id = @ProjectID";

                    using (MySqlCommand cmd = new MySqlCommand(deleteWorkSql, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ProjectID", projectId);
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Delete project_phase
                    string deletePhaseSql = "DELETE FROM project_phase WHERE pro_id = @ProjectID";
                    using (MySqlCommand cmd = new MySqlCommand(deletePhaseSql, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ProjectID", projectId);
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Delete project
                    string deleteProjectSql = "DELETE FROM project WHERE pro_id = @ProjectID";
                    using (MySqlCommand cmd = new MySqlCommand(deleteProjectSql, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ProjectID", projectId);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }


    }
}
