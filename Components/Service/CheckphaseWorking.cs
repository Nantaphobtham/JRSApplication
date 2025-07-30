using JRSApplication.Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication.Components.Service
{
    public partial class CheckphaseWorking : Form
    {
        private string _workId;
        private int _phaseId;
        private int _projectId;
        public CheckphaseWorking(string workId, int phaseId, int projectId)
        {
            InitializeComponent();
            _workId = workId;
            _phaseId = phaseId;
            _projectId = projectId;
        }

    }
}
