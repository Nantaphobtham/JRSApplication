using JRSApplication.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using System.Linq;

namespace JRSApplication.Data_Access_Layer
{
    public class PhaseWorkDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        public class PhaseWorkingFullRow
        {
            public int ProjectID { get; set; }
            public string ProjectNumber { get; set; }
            public string ProjectName { get; set; }
            public string EmployeeName { get; set; }
            public string CustomerName { get; set; }

            public int PhaseNo { get; set; }
            public string PhaseDetail { get; set; }

            public string WorkID { get; set; }
            public string WorkStatus { get; set; }
            public DateTime WorkDate { get; set; }
            public DateTime? WorkEndDate { get; set; }
            public string WorkDetail { get; set; }
            public string WorkRemark { get; set; }

            public int PicNo { get; set; }
            public string PicName { get; set; }
            public string PicDescription { get; set; }
            public byte[] PicData { get; set; }
        }

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
        public void InsertPhaseWorkingAndPictures(PhaseWorking work, List<WorkingPicture> pictures)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Generate WorkID
                        work.WorkID = GenerateWorkIdByYear(conn);

                        // 1. Insert phase_working
                        string sqlWork = @"INSERT INTO phase_working
                    (work_id, phase_id, work_detail, work_status, work_date, work_end_date, work_update_date, work_remark, supplier_assignment_id)
                    VALUES (@WorkId, @PhaseId, @WorkDetail, @WorkStatus, @WorkDate, @WorkEndDate, @WorkUpdateDate, @WorkRemark, @SupplierAssignmentId)";
                        using (var cmd = new MySqlCommand(sqlWork, conn, tran))
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

                        // 2. Insert working_picture (ถ้ามี)
                        if (pictures != null && pictures.Count > 0)
                        {
                            string sqlPic = @"INSERT INTO working_picture
                        (work_id, pic_no, pic_name, description, picture_data, created_at)
                        VALUES (@WorkId, @PicNo, @PicName, @Description, @PictureData, @CreatedAt)";

                            foreach (var pic in pictures)
                            {
                                pic.WorkID = work.WorkID; // set work_id ให้รูป
                                using (var cmd = new MySqlCommand(sqlPic, conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@WorkId", pic.WorkID);
                                    cmd.Parameters.AddWithValue("@PicNo", pic.PicNo);
                                    cmd.Parameters.AddWithValue("@PicName", pic.PicName ?? ""); // ไม่เข้าใจ Null 
                                    cmd.Parameters.AddWithValue("@Description", pic.Description ?? "");
                                    cmd.Parameters.AddWithValue("@PictureData", pic.PictureData);
                                    cmd.Parameters.AddWithValue("@CreatedAt", pic.CreatedAt == default(DateTime) ? DateTime.Now : pic.CreatedAt);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // สมมุติ work.PhaseID กับ work.WorkStatus พร้อมใช้งาน
                        string updatePhaseSql = @"UPDATE project_phase
                          SET phase_status = @PhaseStatus
                          WHERE phase_id = @PhaseID";

                        using (var cmd = new MySqlCommand(updatePhaseSql, conn, tran))
                        {
                            // จะใช้ work.WorkStatus ตรงๆ หรือจะใส่เงื่อนไขเพิ่มก็ได้ เช่น Completed/InProgress
                            cmd.Parameters.AddWithValue("@PhaseStatus",
                                work.WorkStatus == "Waiting" ? "InProgress" : work.WorkStatus);
                            cmd.Parameters.AddWithValue("@PhaseID", work.PhaseID);
                            cmd.ExecuteNonQuery();
                        }


                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw; // ส่ง error กลับไป
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------
        //สำหรับการ approve phase_working

        public List<PhaseWorkingFullRow> GetFullPhaseWorkingByPhaseId(int phaseId)
        {
            List<PhaseWorkingFullRow> results = new List<PhaseWorkingFullRow>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"
            SELECT 
                pj.pro_id, pj.pro_number, pj.pro_name,
                emp.emp_name, emp.emp_lname,
                cus.cus_name, cus.cus_lname,
                ph.phase_no, ph.phase_detail,
                pw.work_id, pw.work_status, pw.work_date, pw.work_end_date,
                pw.work_detail, pw.work_remark,
                wp.pic_no, wp.pic_name, wp.description, wp.picture_data
            FROM phase_working pw
            JOIN project_phase ph ON pw.phase_id = ph.phase_id
            JOIN project pj ON ph.pro_id = pj.pro_id
            JOIN employee emp ON pj.emp_id = emp.emp_id
            JOIN customer cus ON pj.cus_id = cus.cus_id
            LEFT JOIN working_picture wp ON pw.work_id = wp.work_id
            WHERE ph.phase_id = @phaseId
            ORDER BY pw.work_date, wp.pic_no";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@phaseId", phaseId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new PhaseWorkingFullRow
                            {
                                ProjectID = reader.GetInt32("pro_id"),
                                ProjectNumber = reader["pro_number"].ToString(),
                                ProjectName = reader["pro_name"].ToString(),
                                EmployeeName = reader["emp_name"].ToString() + " " + reader["emp_lname"].ToString(),
                                CustomerName = reader["cus_name"].ToString() + " " + reader["cus_lname"].ToString(),
                                PhaseNo = Convert.ToInt32(reader["phase_no"]),
                                PhaseDetail = reader["phase_detail"].ToString(),

                                WorkID = reader["work_id"].ToString(),
                                WorkStatus = reader["work_status"].ToString(),
                                WorkDate = reader.GetDateTime("work_date"),
                                WorkEndDate = reader["work_end_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["work_end_date"]),
                                WorkDetail = reader["work_detail"].ToString(),
                                WorkRemark = reader["work_remark"].ToString(),

                                PicNo = reader["pic_no"] == DBNull.Value ? 0 : Convert.ToInt32(reader["pic_no"]),
                                PicName = reader["pic_name"]?.ToString(),
                                PicDescription = reader["description"]?.ToString(),
                                PicData = reader["picture_data"] == DBNull.Value ? null : (byte[])reader["picture_data"]
                            };

                            results.Add(row);
                        }
                    }
                }
            }

            return results;
        }

        public void UpdateWorkStatus(string workId, string status, string remark)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"
            UPDATE phase_working 
            SET 
                work_status = @status,
                work_remark = @remark,
                work_update_date = CURRENT_DATE()
            WHERE work_id = @workId";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@remark", string.IsNullOrWhiteSpace(remark) ? DBNull.Value : (object)remark);
                    cmd.Parameters.AddWithValue("@workId", workId);

                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
