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

        public List<PhaseWorking> GetAllPhaseWorking()
        {
            var list = new List<PhaseWorking>();
            string sql = "SELECT pw.*, pp.pro_id AS project_id, pp.phase_no AS phase_no " +
                            "FROM phase_working pw " +
                            "LEFT JOIN project_phase pp ON pw.phase_id = pp.phase_id " +
                            "ORDER BY pw.work_id";
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pw = new PhaseWorking
                        {
                            WorkID = reader["work_id"]?.ToString(),
                            PhaseID = reader.GetInt32(reader.GetOrdinal("phase_id")),
                            // เพิ่มบรรทัดนี้สำหรับ ProjectID
                            ProjectID = reader.IsDBNull(reader.GetOrdinal("project_id"))
                                ? 0 : reader.GetInt32(reader.GetOrdinal("project_id")),
                            PhaseNo = reader.GetInt32(reader.GetOrdinal("phase_no")),
                            WorkDetail = reader["work_detail"]?.ToString(),
                            WorkStatus = reader["work_status"]?.ToString(),

                            WorkDate = reader.IsDBNull(reader.GetOrdinal("work_date"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("work_date")),

                            EndDate = reader.IsDBNull(reader.GetOrdinal("work_end_date"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("work_end_date")),

                            UpdateDate = reader.IsDBNull(reader.GetOrdinal("work_update_date"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("work_update_date")),

                            Remark = reader["work_remark"]?.ToString(),
                            SupplierAssignmentId = reader["supplier_assignment_id"]?.ToString(),
                        };
                        list.Add(pw);
                    }
                }
            }
            return list;
        }

        //ฟังก์ชันอ่าน work_status ล่าสุด จาก phase_working
        public string GetWorkStatusByPhaseId(int phaseId)
        {
            string status = null;
            string sql = "SELECT work_status FROM phase_working WHERE phase_id = @PhaseId ORDER BY work_id DESC LIMIT 1";
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@PhaseId", phaseId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    status = result.ToString();
            }
            return status;
        }
        // ดึง Phase + Status (พร้อม SupplierWorkAssignment) งง
        public List<PhaseWithStatus> GetPhasesWithStatusAndAssignments(int projectId)
        {
            var list = new List<PhaseWithStatus>();
            string sql = @"
                        SELECT 
                            pp.phase_id,
                            pp.phase_no,
                            pp.phase_detail,
                            pw.work_status
                        FROM project_phase pp
                        LEFT JOIN (
                            SELECT phase_id, work_status
                            FROM phase_working
                            WHERE (phase_id, work_date) IN (
                                SELECT phase_id, MAX(work_date)
                                FROM phase_working
                                GROUP BY phase_id
                            )
                        ) pw ON pw.phase_id = pp.phase_id
                        WHERE pp.pro_id = @ProjectId
                        ORDER BY pp.phase_no;
                    ";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ProjectId", projectId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new PhaseWithStatus
                        {
                            PhaseId = reader.GetInt32("phase_id"),
                            PhaseNumber = reader.GetInt32("phase_no"),
                            PhaseDetail = reader["phase_detail"]?.ToString(),
                            PhaseStatus = reader["work_status"]?.ToString() ?? "" // ถ้า NULL ให้เป็น string ว่าง
                        };
                        list.Add(p);
                    }
                }
            }
            return list;
        }
        // --- 1. ดึง phase + work status --- ไม่ใช้ค่อยลบ 
        public DataTable GetPhasesWithStatus(int projectId)
        {
            DataTable dt = new DataTable();
            string sql = @"
            SELECT 
                pp.phase_id,
                pp.phase_no,
                pp.phase_detail,
                pw.work_status
            FROM project_phase pp
            LEFT JOIN phase_working pw
                ON pw.phase_id = pp.phase_id
                AND pw.work_date = (
                    SELECT MAX(w2.work_date)
                    FROM phase_working w2
                    WHERE w2.phase_id = pp.phase_id
                )
            WHERE pp.pro_id = @ProjectID
            ORDER BY pp.phase_no
        ";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ProjectID", projectId);
                conn.Open();
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        // --- 2. ดึง supplier_work_assignment สำหรับ phase นี้ ---
        public DataTable GetAssignmentsByPhase(int phaseId)
        {
            DataTable dt = new DataTable();
            string query = @"
            SELECT
                supplier_assignment_id AS 'เลขที่งาน',
                assign_description AS 'รายละเอียดงาน',
                start_date AS 'วันเริ่มต้น',
                due_date AS 'วันครบกำหนด',
                assign_status AS 'สถานะงาน'
            FROM supplier_work_assignment
            WHERE phase_id = @PhaseId
            ORDER BY start_date;
        ";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PhaseId", phaseId);
                conn.Open();
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }
        //--------------------------------------------------------------------------------
        // insert and update data in table   supplier_work_assignment  +  phase_working 
        // --- อัปเดตสถานะ supplier_work_assignment
        public void UpdateSupplierWorkAssignmentStatus(string assignmentId, string status)
        {
            string sql = @"UPDATE supplier_work_assignment 
                       SET assign_status = @Status, update_at = @Now
                       WHERE supplier_assignment_id = @Id";
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Now", DateTime.Now);
                cmd.Parameters.AddWithValue("@Id", assignmentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public string GenerateWorkIdByYear(MySqlConnection conn)
        {
            string prefix = $"HW{DateTime.Now:yy}"; // หรือใช้ yyyy ได้
            int maxLength = 20;
            string workId = "";
            int runNo = 0;
            string postfix = "";

            string sql = $@"
                            SELECT work_id 
                            FROM phase_working
                            WHERE work_id LIKE '{prefix}%'
                            ORDER BY work_id DESC
                            LIMIT 1
                        ";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    string lastId = result.ToString();
                    string remain = lastId.Substring(prefix.Length);
                    string alphaPart = new string(remain.TakeWhile(char.IsLetter).ToArray());
                    string numberPart = new string(remain.SkipWhile(char.IsLetter).ToArray());

                    postfix = alphaPart;
                    int.TryParse(numberPart, out runNo);
                }
            }

            runNo++;

            if (runNo > 9999)
            {
                runNo = 1;
                postfix = NextAlpha(postfix);
            }

            workId = $"{prefix}{postfix}{runNo.ToString("D4")}";
            if (workId.Length > maxLength)
                throw new Exception("WorkID length exceed 20 characters!");

            return workId;
        }

        // ฟังก์ชัน NextAlpha เดิม (เหมือนที่โพสต์ก่อนหน้า)
        private string NextAlpha(string alpha)
        {
            if (string.IsNullOrEmpty(alpha)) return "A";
            var arr = alpha.ToCharArray();
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                if (arr[i] < 'Z')
                {
                    arr[i]++;
                    return new string(arr, 0, i + 1).PadRight(arr.Length, 'A');
                }
                else
                {
                    arr[i] = 'A';
                    if (i == 0)
                        return "A" + new string(arr);
                }
            }
            return "A";
        }

        // --- Insert ลง phase_working
        public void InsertPhaseWorking(PhaseWorking work)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                // Generate WorkID 
                work.WorkID = GenerateWorkIdByYear(conn);

                string sql = @"INSERT INTO phase_working
            (work_id, phase_id, work_detail, work_status, work_date, work_end_date, work_update_date, work_remark, supplier_assignment_id)
            VALUES
            (@WorkId, @PhaseId, @WorkDetail, @WorkStatus, @WorkDate, @WorkEndDate, @WorkUpdateDate, @WorkRemark, @SupplierAssignmentId)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkId", work.WorkID);
                    cmd.Parameters.AddWithValue("@PhaseId", work.PhaseID);
                    cmd.Parameters.AddWithValue("@WorkDetail", work.WorkDetail);
                    cmd.Parameters.AddWithValue("@WorkStatus", work.WorkStatus);
                    cmd.Parameters.AddWithValue("@WorkDate", work.WorkDate);
                    cmd.Parameters.AddWithValue("@WorkEndDate", work.EndDate.HasValue ? (object)work.EndDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@WorkUpdateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@WorkRemark", string.IsNullOrWhiteSpace(work.Remark) ? DBNull.Value : (object)work.Remark);
                    cmd.Parameters.AddWithValue("@SupplierAssignmentId", work.SupplierAssignmentId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

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
