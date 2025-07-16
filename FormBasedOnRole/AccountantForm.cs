using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JRSApplication
{
    public partial class AccountantForm : Form
    {
        public AccountantForm()
        {
            InitializeComponent();
        }

        private void btnReceivePaymentMain_Click(object sender, EventArgs e)
        {
            // Toggle การซ่อน/แสดง
            panelReceivePaymentSub.Visible = !panelReceivePaymentSub.Visible;
        }




    }
}
