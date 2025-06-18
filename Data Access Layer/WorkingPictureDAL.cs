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
    public class WorkingPictureDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public bool InsertPicture(WorkingPicture pic)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO working_picture 
                               (work_id, pic_no, pic_name, description, picture_data, created_at)
                               VALUES 
                               (@WorkId, @PicNo, @PicName, @Description, @PictureData, @CreatedAt)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@WorkId", pic.WorkID);
                cmd.Parameters.AddWithValue("@PicNo", pic.PicNo);
                cmd.Parameters.AddWithValue("@PicName", pic.PicName);
                cmd.Parameters.AddWithValue("@Description", pic.Description);
                cmd.Parameters.AddWithValue("@PictureData", pic.PictureData);
                cmd.Parameters.AddWithValue("@CreatedAt", pic.CreatedAt);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
     }




        //ไม่ถูกใช้งานค่อยลบ
        //public bool InsertPicture(WorkingPicture pic)
        //{
        //    using (MySqlConnection conn = new MySqlConnection(connectionString))
        //    {
        //        string sql = @"INSERT INTO working_picture (phase_id, pic_data, pic_detail)
        //                   VALUES (@PhaseID, @PictureData, @PictureDetail)";

        //        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@PhaseID", pic.PhaseID);
        //            cmd.Parameters.AddWithValue("@PictureData", pic.PictureData);
        //            cmd.Parameters.AddWithValue("@PictureDetail", pic.PictureDetail);

        //            conn.Open();
        //            int result = cmd.ExecuteNonQuery();
        //            return result > 0;
        //        }
        //    }
        //}
    }


