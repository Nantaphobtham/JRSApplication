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
                string sql = @"INSERT INTO working_picture (phase_no, pic_data, pic_detail)
                           VALUES (@PhaseNo, @PictureData, @PictureDetail)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PhaseNo", pic.PhaseNo);
                    cmd.Parameters.AddWithValue("@PictureData", pic.PictureData);
                    cmd.Parameters.AddWithValue("@PictureDetail", pic.PictureDetail);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }
    }
}
