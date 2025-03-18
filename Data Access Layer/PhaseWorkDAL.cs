using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Data_Access_Layer
{
    public class PhaseWorkDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;



    }

}
