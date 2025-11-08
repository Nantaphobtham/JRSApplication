using JRSApplication.Components.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Data_Access_Layer
{
    public class SupplierAssignmentFileDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public void Insert(SupplierAssignmentFile file)
        {
            string query = @"
                INSERT INTO supplier_assignment_file 
                (supplier_assignment_id, file_name, file_type, file_data, uploaded_at, uploaded_by)
                VALUES (@AssignmentId, @FileName, @FileType, @FileData, @UploadedAt, @UploadedBy);
            ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@AssignmentId", file.AssignmentId);
                cmd.Parameters.AddWithValue("@FileName", file.FileName);
                cmd.Parameters.AddWithValue("@FileType", file.FileType);
                cmd.Parameters.AddWithValue("@FileData", file.FileData);
                cmd.Parameters.AddWithValue("@UploadedAt", file.UploadedAt);
                cmd.Parameters.AddWithValue("@UploadedBy", file.UploadedBy);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // เพิ่มฟังก์ชันดึงไฟล์, ลบไฟล์ ฯลฯ ได้ตามต้องการ

        public void DeleteByAssignmentId(string assignmentId)
        {
            string query = "DELETE FROM supplier_assignment_file WHERE supplier_assignment_id = @AssignmentId";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@AssignmentId", assignmentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }




    }
}
