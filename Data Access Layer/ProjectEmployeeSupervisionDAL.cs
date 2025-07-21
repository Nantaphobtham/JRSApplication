using JRSApplication.Components.Models;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace JRSApplication.Data_Access_Layer
{
    internal class ProjectEmployeeSupervisionDAL
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        // ✅ Method สำหรับ Insert
        public void InsertRecord(ProjectEmployeeSupervisionModel model)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"INSERT INTO project_employee_supervision
                (project_id, project_name, contract_number, customer_name, customer_phone, customer_email,
                 emp_id, emp_first_name, emp_last_name, emp_phone, emp_position, created_at)
                 VALUES (@project_id, @project_name, @contract_number, @customer_name, @customer_phone, @customer_email,
                         @emp_id, @emp_first_name, @emp_last_name, @emp_phone, @emp_position, @created_at);";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@project_id", model.ProjectId);
                cmd.Parameters.AddWithValue("@project_name", model.ProjectName);
                cmd.Parameters.AddWithValue("@contract_number", model.ContractNumber);
                cmd.Parameters.AddWithValue("@customer_name", model.CustomerName);
                cmd.Parameters.AddWithValue("@customer_phone", model.CustomerPhone);
                cmd.Parameters.AddWithValue("@customer_email", model.CustomerEmail);
                cmd.Parameters.AddWithValue("@emp_id", model.EmpId);
                cmd.Parameters.AddWithValue("@emp_first_name", model.EmpFirstName);
                cmd.Parameters.AddWithValue("@emp_last_name", model.EmpLastName);
                cmd.Parameters.AddWithValue("@emp_phone", model.EmpPhone);
                cmd.Parameters.AddWithValue("@emp_position", model.EmpPosition);
                cmd.Parameters.AddWithValue("@created_at", model.CreatedAt);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ✅ Method สำหรับ SELECT มาโชว์ใน DataGridView
        public DataTable GetAllRecords()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM project_employee_supervision ORDER BY created_at DESC;";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
