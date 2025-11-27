using JRSApplication.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace JRSApplication.Data_Access_Layer
{
    public class ProjectDAL
    {
        private string connectionString =
            ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        // ------------------------------------------------------------
        // ดึงรายการโครงการทั้งหมด (ใช้ในตารางหน้า ManageProject)
        // ------------------------------------------------------------
        public List<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"
                    SELECT 
                        p.pro_id, 
                        p.pro_name, 
                        p.pro_number, 
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
                            var project = new Project
                            {
                                ProjectID = reader.GetInt32("pro_id"),
                                ProjectName = reader.GetString("pro_name"),

                                // เลขที่สัญญา
                                ProjectNumber = reader["pro_number"] == DBNull.Value
                                    ? string.Empty
                                    : reader["pro_number"].ToString(),

                                ProjectDetail = reader.IsDBNull(reader.GetOrdinal("pro_detail"))
                                    ? string.Empty
                                    : reader.GetString("pro_detail"),

                                ProjectAddress = reader.IsDBNull(reader.GetOrdinal("pro_address"))
                                    ? string.Empty
                                    : reader.GetString("pro_address"),

                                ProjectBudget = reader.IsDBNull(reader.GetOrdinal("pro_budget"))
                                    ? 0
                                    : reader.GetDecimal("pro_budget"),

                                ProjectStart = reader.GetDateTime("pro_start"),
                                ProjectEnd = reader.GetDateTime("pro_end"),

                                CurrentPhaseNumber = reader.IsDBNull(reader.GetOrdinal("pro_currentphasenumber"))
                                    ? 0
                                    : reader.GetInt32("pro_currentphasenumber"),

                                CustomerName = reader.IsDBNull(reader.GetOrdinal("CustomerFullName"))
                                    ? "ไม่ระบุ"
                                    : reader.GetString("CustomerFullName"),

                                EmployeeName = reader.IsDBNull(reader.GetOrdinal("EmployeeFullName"))
                                    ? "ไม่ระบุ"
                                    : reader.GetString("EmployeeFullName")
                            };

                            projects.Add(project);
                        }
                    }
                }
            }

            return projects;
        }

        // ------------------------------------------------------------
        // ดึงรายละเอียดโครงการ + Phase + File ตาม ProjectID
        // ------------------------------------------------------------
        public Project GetProjectDetailsById(int projectId)
        {
            Project project = null;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"
                    SELECT 
                        p.pro_id              AS ProjectID,
                        p.pro_name            AS ProjectName,
                        p.pro_start           AS ProjectStart,
                        p.pro_end             AS ProjectEnd,
                        p.pro_number          AS ContractNumber,
                        p.pro_budget          AS ProjectBudget,
                        p.pro_detail          AS ProjectDetail,
                        p.pro_address         AS ProjectAddress,
                        p.pro_currentphasenumber AS CurrentPhaseNumber,
                        p.pro_remark          AS Remark,

                        CONCAT(c.cus_name, ' ', c.cus_lname) AS CustomerFullName,
                        CONCAT(e.emp_name, ' ', e.emp_lname) AS ProjectManagerFullName,

                        pf.con_blueprint      AS ConstructionBlueprint,
                        pf.demolition_model   AS DemolitionModel
                    FROM project p
                    LEFT JOIN customer c   ON p.cus_id = c.cus_id
                    LEFT JOIN employee e   ON p.emp_id = e.emp_id
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
                                ProjectID = reader.GetInt32(reader.GetOrdinal("ProjectID")),
                                ProjectName = reader.GetString(reader.GetOrdinal("ProjectName")),
                                ProjectStart = reader.GetDateTime(reader.GetOrdinal("ProjectStart")),
                                ProjectEnd = reader.GetDateTime(reader.GetOrdinal("ProjectEnd")),

                                // เลขที่สัญญา
                                ProjectNumber = reader.IsDBNull(reader.GetOrdinal("ContractNumber"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("ContractNumber")),

                                ProjectDetail = reader.IsDBNull(reader.GetOrdinal("ProjectDetail"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("ProjectDetail")),

                                ProjectAddress = reader.IsDBNull(reader.GetOrdinal("ProjectAddress"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("ProjectAddress")),

                                ProjectBudget = reader.GetDecimal(reader.GetOrdinal("ProjectBudget")),

                                Remark = reader.IsDBNull(reader.GetOrdinal("Remark"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("Remark")),

                                CurrentPhaseNumber = reader.IsDBNull(reader.GetOrdinal("CurrentPhaseNumber"))
                                    ? 0
                                    : reader.GetInt32(reader.GetOrdinal("CurrentPhaseNumber")),

                                CustomerName = reader.IsDBNull(reader.GetOrdinal("CustomerFullName"))
                                    ? "ไม่มีข้อมูล"
                                    : reader.GetString(reader.GetOrdinal("CustomerFullName")),

                                EmployeeName = reader.IsDBNull(reader.GetOrdinal("ProjectManagerFullName"))
                                    ? "ไม่มีข้อมูล"
                                    : reader.GetString(reader.GetOrdinal("ProjectManagerFullName")),

                                ProjectFile = new ProjectFile
                                {
                                    ConstructionBlueprint =
                                        reader["ConstructionBlueprint"] != DBNull.Value
                                            ? (byte[])reader["ConstructionBlueprint"]
                                            : null,
                                    DemolitionModel =
                                        reader["DemolitionModel"] != DBNull.Value
                                            ? (byte[])reader["DemolitionModel"]
                                            : null
                                }
                            };
                        }
                    }
                }

                // โหลด Phase
                if (project != null)
                {
                    string phaseSql = @"
                        SELECT phase_no, phase_detail, phase_budget, phase_percent, phase_status
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
                                    PhasePercent = phaseReader.GetDecimal("phase_percent"),
                                    PhaseStatus = phaseReader["phase_status"] == DBNull.Value
                                        ? null
                                        : phaseReader["phase_status"].ToString()
                                });
                            }

                            project.Phases = phases;
                        }
                    }
                }
            }

            return project;
        }

        // ------------------------------------------------------------
        // ออก pro_id ใหม่
        // ------------------------------------------------------------
        public int GenerateProjectID()
        {
            int baseID = int.Parse(DateTime.Now.ToString("yyMM") + "000");
            int newID = baseID + 1;

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
                        newID = Convert.ToInt32(result) + 1;
                    }
                }
            }

            return newID;
        }

        // ------------------------------------------------------------
        // Insert Project + Phases + Files
        // ------------------------------------------------------------
        public bool InsertProjectWithPhases(Project project, List<ProjectPhase> phases,
                                            byte[] constructionBlueprint, byte[] demolitionModel)
        {
            bool isSuccess = false;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlProject = @"
                            INSERT INTO project (
                                pro_id, pro_name, pro_detail, pro_address, pro_budget,
                                pro_start, pro_end, pro_currentphasenumber,
                                pro_remark, pro_number, emp_id, cus_id
                            )
                            VALUES (
                                @ProjectID, @ProjectName, @ProjectDetail, @ProjectAddress, @ProjectBudget,
                                @ProjectStart, @ProjectEnd, @CurrentPhaseNumber,
                                @Remark, @ProjectNumber, @EmployeeID, @CustomerID
                            );";

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

                        foreach (var phase in phases)
                        {
                            string sqlPhase = @"
                                INSERT INTO project_phase (
                                    phase_no, phase_detail, phase_budget, phase_percent,
                                    pro_id, phase_status
                                )
                                VALUES (
                                    @PhaseNo, @PhaseDetail, @PhaseBudget, @PhasePercent,
                                    @ProjectID, @PhaseStatus
                                );";

                            using (MySqlCommand cmdPhase = new MySqlCommand(sqlPhase, conn, transaction))
                            {
                                cmdPhase.Parameters.AddWithValue("@PhaseNo", phase.PhaseNumber);
                                cmdPhase.Parameters.AddWithValue("@PhaseDetail", phase.PhaseDetail);
                                cmdPhase.Parameters.AddWithValue("@PhaseBudget", phase.PhaseBudget);
                                cmdPhase.Parameters.AddWithValue("@PhasePercent", phase.PhasePercent);
                                cmdPhase.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                                cmdPhase.Parameters.AddWithValue("@PhaseStatus", phase.PhaseStatus ?? "waiting");

                                cmdPhase.ExecuteNonQuery();
                            }
                        }

                        if (constructionBlueprint != null)
                        {
                            string sqlFile = @"
                                INSERT INTO project_files (pro_id, con_blueprint, demolition_model)
                                VALUES (@ProjectID, @ConstructionBlueprint, @DemolitionModel);";

                            using (MySqlCommand cmdFile = new MySqlCommand(sqlFile, conn, transaction))
                            {
                                cmdFile.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                                cmdFile.Parameters.AddWithValue("@ConstructionBlueprint", constructionBlueprint);
                                cmdFile.Parameters.AddWithValue("@DemolitionModel",
                                    demolitionModel != null ? (object)demolitionModel : DBNull.Value);

                                cmdFile.ExecuteNonQuery();
                            }
                        }

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

        // ------------------------------------------------------------
        // Update Project + Phases + Files
        // ------------------------------------------------------------
        public bool UpdateProjectWithPhases(Project project, List<ProjectPhase> updatedPhases,
                                            byte[] blueprintBytes, byte[] demolitionBytes)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
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
                            WHERE pro_id = @ProjectID;";

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

                        List<ProjectPhase> existingPhases = new List<ProjectPhase>();
                        string selectPhaseSql = @"
                            SELECT phase_no, phase_detail, phase_budget, phase_percent, phase_status
                            FROM project_phase
                            WHERE pro_id = @ProjectID;";

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
                                        PhasePercent = Convert.ToDecimal(reader["phase_percent"]),
                                        PhaseStatus = reader["phase_status"] == DBNull.Value
                                            ? null
                                            : reader["phase_status"].ToString()
                                    });
                                }
                            }
                        }

                        foreach (var phase in updatedPhases)
                        {
                            var match = existingPhases.FirstOrDefault(p => p.PhaseNumber == phase.PhaseNumber);

                            if (match != null)
                            {
                                string updatePhaseSql = @"
                                    UPDATE project_phase SET
                                        phase_detail = @PhaseDetail,
                                        phase_budget = @PhaseBudget,
                                        phase_percent = @PhasePercent,
                                        phase_status = @PhaseStatus
                                    WHERE pro_id = @ProjectID AND phase_no = @PhaseNo;";

                                using (MySqlCommand cmdUpdate = new MySqlCommand(updatePhaseSql, conn, tran))
                                {
                                    cmdUpdate.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                                    cmdUpdate.Parameters.AddWithValue("@PhaseNo", phase.PhaseNumber);
                                    cmdUpdate.Parameters.AddWithValue("@PhaseDetail", phase.PhaseDetail);
                                    cmdUpdate.Parameters.AddWithValue("@PhaseBudget", phase.PhaseBudget);
                                    cmdUpdate.Parameters.AddWithValue("@PhasePercent", phase.PhasePercent);
                                    cmdUpdate.Parameters.AddWithValue("@PhaseStatus", phase.PhaseStatus ?? match.PhaseStatus ?? "waiting");
                                    cmdUpdate.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                string insertSql = @"
                                    INSERT INTO project_phase (
                                        pro_id, phase_no, phase_detail, phase_budget, phase_percent, phase_status
                                    )
                                    VALUES (
                                        @ProjectID, @PhaseNo, @PhaseDetail, @PhaseBudget, @PhasePercent, @PhaseStatus
                                    );";

                                using (MySqlCommand cmdInsert = new MySqlCommand(insertSql, conn, tran))
                                {
                                    cmdInsert.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                                    cmdInsert.Parameters.AddWithValue("@PhaseNo", phase.PhaseNumber);
                                    cmdInsert.Parameters.AddWithValue("@PhaseDetail", phase.PhaseDetail);
                                    cmdInsert.Parameters.AddWithValue("@PhaseBudget", phase.PhaseBudget);
                                    cmdInsert.Parameters.AddWithValue("@PhasePercent", phase.PhasePercent);
                                    cmdInsert.Parameters.AddWithValue("@PhaseStatus", phase.PhaseStatus ?? "waiting");
                                    cmdInsert.ExecuteNonQuery();
                                }
                            }
                        }

                        var toDelete = existingPhases
                            .Where(old => !updatedPhases.Any(up => up.PhaseNumber == old.PhaseNumber))
                            .ToList();

                        foreach (var del in toDelete)
                        {
                            string deleteSql = "DELETE FROM project_phase WHERE pro_id = @ProjectID AND phase_no = @PhaseNo;";
                            using (MySqlCommand cmdDelete = new MySqlCommand(deleteSql, conn, tran))
                            {
                                cmdDelete.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                                cmdDelete.Parameters.AddWithValue("@PhaseNo", del.PhaseNumber);
                                cmdDelete.ExecuteNonQuery();
                            }
                        }

                        string upsertFileSql = @"
                            INSERT INTO project_files (pro_id, con_blueprint, demolition_model)
                            VALUES (@ProjectID, @Blueprint, @Demolition)
                            ON DUPLICATE KEY UPDATE
                                con_blueprint = @Blueprint,
                                demolition_model = @Demolition;";

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

        // ------------------------------------------------------------
        // Delete Project + Phase + PhaseWorking
        // ------------------------------------------------------------
        public bool DeleteProjectWithPhases(int projectId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string deleteWorkSql = @"
                        DELETE pw 
                        FROM phase_working pw
                        JOIN project_phase pp ON pw.phase_id = pp.phase_id
                        WHERE pp.pro_id = @ProjectID;";

                    using (MySqlCommand cmd = new MySqlCommand(deleteWorkSql, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ProjectID", projectId);
                        cmd.ExecuteNonQuery();
                    }

                    string deletePhaseSql = "DELETE FROM project_phase WHERE pro_id = @ProjectID;";
                    using (MySqlCommand cmd = new MySqlCommand(deletePhaseSql, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ProjectID", projectId);
                        cmd.ExecuteNonQuery();
                    }

                    string deleteProjectSql = "DELETE FROM project WHERE pro_id = @ProjectID;";
                    using (MySqlCommand cmd = new MySqlCommand(deleteProjectSql, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ProjectID", projectId);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
