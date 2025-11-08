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
    public class MaterialDetailDAL
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public List<MaterialDetail> LoadMaterialData(int orderId)
        {
            List<MaterialDetail> materials = new List<MaterialDetail>();
            string query = @"SELECT mat_line_no AS MatNo,
                            mat_detail AS MatDetail,
                            mat_quantity AS MatQuantity,
                            mat_unit AS MatUnit,
                            mat_price AS MatPrice,
                            mat_amount AS MatAmount
                     FROM jrsconstruction.material_detail
                     WHERE order_id = @orderId
                     ORDER BY mat_line_no";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@orderId", orderId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        materials.Add(new MaterialDetail
                        {
                            MatNo = reader.GetInt32("MatNo"),
                            MatDetail = reader.GetString("MatDetail"),
                            MatQuantity = reader.GetDecimal("MatQuantity"),
                            MatUnit = reader.GetString("MatUnit"),
                            MatPrice = reader.GetDecimal("MatPrice"),
                            MatAmount = reader.GetDecimal("MatAmount")
                        });
                    }
                }
            }

            return materials;
        }

    }
}
