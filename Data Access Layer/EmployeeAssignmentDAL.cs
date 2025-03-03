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
    public class EmployeeAssignmentDAL
    {
        private string connectionString;

        public EmployeeAssignmentDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        public bool InsertAssignment(EmployeeAssignment assignment)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = "INSERT INTO employee_assignment (emp_id, pro_id, assign_by, assign_role, assign_date) " +
                             "VALUES (@EmpID, @ProjectID, @AssignBy, @AssignRole, @AssignDate)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmpID", assignment.EmployeeID);
                    cmd.Parameters.AddWithValue("@ProjectID", assignment.ProjectID);
                    cmd.Parameters.AddWithValue("@AssignBy", assignment.AssignBy);
                    cmd.Parameters.AddWithValue("@AssignRole", assignment.AssignRole);
                    cmd.Parameters.AddWithValue("@AssignDate", assignment.AssignDate);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
    }

}
