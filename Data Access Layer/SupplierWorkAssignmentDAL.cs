using JRSApplication.Components.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Data_Access_Layer
{
    public class SupplierWorkAssignmentDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public void Insert(SupplierWorkAssignment model)
        {
            string query = @"
                INSERT INTO supplier_work_assignment 
                (sup_id, start_date, due_date, assign_description, assign_remark, phase_id)
                VALUES (@SupId, @StartDate, @DueDate, @AssignDescription, @AssignRemark, @PhaseId);
            ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@SupId", model.SupId);
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                cmd.Parameters.AddWithValue("@DueDate", model.DueDate);
                cmd.Parameters.AddWithValue("@AssignDescription", model.AssignDescription);
                cmd.Parameters.AddWithValue("@AssignRemark", model.AssignRemark);
                cmd.Parameters.AddWithValue("@PhaseId", model.PhaseId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetAllAssignmentsWithPhase()
        {
            DataTable dt = new DataTable();

            string query = @"
                        SELECT
                            swa.supplier_assignment_id AS 'รหัสงาน',
                            swa.sup_id AS 'รหัสผู้รับเหมา',
                            swa.start_date AS 'วันที่เริ่ม',
                            swa.due_date AS 'วันที่สิ้นสุด',
                            swa.assign_description AS 'รายละเอียดงาน',
                            swa.assign_remark AS 'หมายเหตุ',
                            pp.phase_no AS 'เฟสที่'
                        FROM supplier_work_assignment swa
                        LEFT JOIN project_phase pp ON swa.phase_id = pp.phase_id
                        ORDER BY swa.supplier_assignment_id DESC;
                    ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
            {
                conn.Open();
                adapter.Fill(dt);
            }

            return dt;
        }


        public DataTable GetSupplierInfoFromAssignment(string supId)
        {
            DataTable dt = new DataTable();

            string query = @"
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
                        LIMIT 1;
                    ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@SupId", supId);

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    conn.Open();
                    adapter.Fill(dt);
                }
            }

            return dt;
        }






    }
}
