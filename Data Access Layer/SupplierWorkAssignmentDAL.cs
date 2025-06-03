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
                            swa.supplier_assignment_id AS AssignmentID,
                            swa.sup_id AS SupplierID,
                            swa.start_date AS StartDate,
                            swa.due_date AS DueDate,
                            swa.assign_description AS AssignDescription,
                            swa.assign_remark AS AssignRemark,
                            pp.phase_no AS PhaseNo
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


    }
}
