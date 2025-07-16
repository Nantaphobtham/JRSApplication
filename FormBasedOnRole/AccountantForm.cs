using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JRSApplication.Accountant;

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
        private void LoadUserControl(UserControl uc)
        {
            Body.Controls.Clear();     // ล้างเนื้อหาเก่าออก
            uc.Dock = DockStyle.Fill;  // ขยายเต็ม panel
            Body.Controls.Add(uc);     // เพิ่ม UserControl
            uc.BringToFront();        // ดันไปข้างหน้า
        }


        private void btnInvoice_Click(object sender, EventArgs e)
        {
            LoadUserControl(new Invoice());
        }

        private void btnConfirmInvoice_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ConfirmInvoice());
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            LoadUserControl(new Receipt());
        }



    }
}
