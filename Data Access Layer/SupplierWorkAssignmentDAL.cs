using JRSApplication.Components.Models;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace JRSApplication.Data_Access_Layer
{
    public class SupplierWorkAssignmentDAL
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        // สร้างรหัสงานรูปแบบ SWOYYMMNNN (ปี พ.ศ. 2 หลัก + เดือน 2 หลัก + รันนิ่ง 3 หลัก)
        public string GenerateWorkOrderId()
        {
            string prefix = "SWO";
            int thaiYear = DateTime.Now.Year + 543;
            string yearPart = thaiYear.ToString().Substring(2, 2); // YY
            string monthPart = DateTime.Now.Month.ToString("D2");  // MM

            int running = 1;

            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"
                    SELECT MAX(SUBSTRING(supplier_assignment_id, 8, 3))
                    FROM supplier_work_assignment 
                    WHERE SUBSTRING(supplier_assignment_id, 4, 2) = @YearPart 
                      AND SUBSTRING(supplier_assignment_id, 6, 2) = @MonthPart";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@YearPart", yearPart);
                    cmd.Parameters.AddWithValue("@MonthPart", monthPart);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                        running = Convert.ToInt32(result) + 1;
                }
            }

            string runningPart = running.ToString("D3"); // NNN
            return $"{prefix}{yearPart}{monthPart}{runningPart}";
        }

        // เพิ่มข้อมูลและคืนรหัสงานที่สร้างได้
        public string Insert(SupplierWorkAssignment model)
        {
            if (string.IsNullOrWhiteSpace(model.AssignmentId))
                model.AssignmentId = GenerateWorkOrderId();

            const string query = @"
                INSERT INTO supplier_work_assignment 
                (supplier_assignment_id, sup_id, start_date, due_date, 
                 assign_description, assign_remark, phase_id, assign_status, emp_id)
                VALUES 
                (@AssignmentId, @SupId, @StartDate, @DueDate, 
                 @AssignDescription, @AssignRemark, @PhaseId, @AssignStatus, @EmployeeID);";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@AssignmentId", model.AssignmentId);
                cmd.Parameters.AddWithValue("@SupId", model.SupId);
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                cmd.Parameters.AddWithValue("@DueDate", model.DueDate);
                cmd.Parameters.AddWithValue("@AssignDescription", model.AssignDescription);
                cmd.Parameters.AddWithValue("@AssignRemark", model.AssignRemark);
                cmd.Parameters.AddWithValue("@PhaseId", model.PhaseId);
                cmd.Parameters.AddWithValue("@AssignStatus", model.AssignStatus);
                cmd.Parameters.AddWithValue("@EmployeeID", model.EmployeeID);

                conn.Open();
                cmd.ExecuteNonQuery();
                return model.AssignmentId;
            }
        }

        // โหลดรายการทั้งหมด (พร้อม phase_no และไฟล์แนบ)
        public DataTable GetAllAssignmentsWithPhase()
        {
            DataTable dt = new DataTable();
            const string query = @"
                SELECT
                    swa.supplier_assignment_id AS 'รหัสงาน',
                    pp.pro_id AS 'รหัสโครงการ',
                    swa.sup_id AS 'รหัสผู้รับเหมา',
                    swa.start_date AS 'วันที่เริ่ม',
                    swa.due_date AS 'วันที่สิ้นสุด',
                    swa.assign_description AS 'รายละเอียดงาน',
                    swa.assign_remark AS 'หมายเหตุ',
                    pp.phase_no AS 'เฟสที่',
                    saf.file_name AS 'ไฟล์แนบ'
                FROM supplier_work_assignment swa
                LEFT JOIN project_phase pp ON swa.phase_id = pp.phase_id
                LEFT JOIN supplier_assignment_file saf 
                       ON swa.supplier_assignment_id = saf.supplier_assignment_id
                ORDER BY swa.supplier_assignment_id DESC;";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            using (var adapter = new MySqlDataAdapter(cmd))
            {
                conn.Open();
                adapter.Fill(dt);
            }
            return dt;
        }

        // ใช้ค้นงานตัวอย่างจาก phase_id (เผื่อกรอกซ้ำ)
        public SupplierWorkAssignment GetAssignmentByPhaseId(int phaseId)
        {
            SupplierWorkAssignment assignment = null;

            const string query = @"
                SELECT supplier_assignment_id, sup_id, start_date, due_date, 
                       assign_description, assign_remark, phase_id, assign_status, emp_id
                FROM supplier_work_assignment
                WHERE phase_id = @PhaseID
                ORDER BY start_date DESC
                LIMIT 1;";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PhaseID", phaseId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        assignment = new SupplierWorkAssignment
                        {
                            AssignmentId = reader.GetString("supplier_assignment_id"),
                            SupId = reader.GetString("sup_id"),
                            StartDate = reader.GetDateTime("start_date"),
                            DueDate = reader.GetDateTime("due_date"),
                            AssignDescription = reader.GetString("assign_description"),
                            AssignRemark = reader.GetString("assign_remark"),
                            PhaseId = reader.GetInt32("phase_id"),
                            AssignStatus = reader["assign_status"] as string,
                            EmployeeID = reader["emp_id"] as string
                        };
                    }
                }
            }
            return assignment;
        }

        // ดึงข้อมูล supplier + โครงการล่าสุดจากงาน
        public DataTable GetSupplierInfoFromAssignment(string supId)
        {
            DataTable dt = new DataTable();
            const string query = @"
                SELECT 
                    s.sup_id AS SupplierID,
                    s.sup_name AS Name,
                    s.sup_juristic AS Juristic,
                    s.sup_tel AS Phone,
                    s.sup_address AS Address,
                    s.sup_email AS Email,
                    pp.phase_id AS PhaseID,
                    pp.phase_no AS PhaseNo,
                    p.pro_id AS ProjectID,
                    p.pro_name AS ProjectName
                FROM supplier_work_assignment swa
                INNER JOIN supplier s ON swa.sup_id = s.sup_id
                INNER JOIN project_phase pp ON swa.phase_id = pp.phase_id
                INNER JOIN project p ON pp.pro_id = p.pro_id
                WHERE s.sup_id = @SupId
                ORDER BY swa.supplier_assignment_id DESC
                LIMIT 1;";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@SupId", supId);
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    conn.Open();
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        // แก้ไขข้อมูล (อัปเดตสถานะและพนักงานด้วย)
        public void Update(SupplierWorkAssignment model)
        {
            const string query = @"
                UPDATE supplier_work_assignment 
                SET 
                    sup_id = @SupId,
                    start_date = @StartDate,
                    due_date = @DueDate,
                    assign_description = @AssignDescription,
                    assign_remark = @AssignRemark,
                    phase_id = @PhaseId,
                    assign_status = @AssignStatus,
                    emp_id = @EmployeeID
                WHERE supplier_assignment_id = @AssignmentId;";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@SupId", model.SupId);
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                cmd.Parameters.AddWithValue("@DueDate", model.DueDate);
                cmd.Parameters.AddWithValue("@AssignDescription", model.AssignDescription);
                cmd.Parameters.AddWithValue("@AssignRemark", model.AssignRemark);
                cmd.Parameters.AddWithValue("@PhaseId", model.PhaseId);
                cmd.Parameters.AddWithValue("@AssignStatus", model.AssignStatus);
                cmd.Parameters.AddWithValue("@EmployeeID", model.EmployeeID);
                cmd.Parameters.AddWithValue("@AssignmentId", model.AssignmentId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ลบด้วยรหัสงาน (string)
        public void Delete(string assignmentId)
        {
            const string query = @"
                DELETE FROM supplier_work_assignment 
                WHERE supplier_assignment_id = @AssignmentId;";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@AssignmentId", assignmentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
