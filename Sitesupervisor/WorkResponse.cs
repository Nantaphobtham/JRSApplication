using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication.Sitesupervisor
{
    public partial class WorkResponse : UserControl
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        
        public WorkResponse()
        {
            InitializeComponent();
        }

    }
}
