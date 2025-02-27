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
    public partial class SupplierRegistration : UserControl
    {
        public SupplierRegistration()
        {
            InitializeComponent();
        }









        private void btnSave_Click(object sender, EventArgs e)
        {
            //บันทึก
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //เพื่ม
            ReadOnlyControls();
            EnableControls();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //แก้ไข
        }
        private void btDelete_Click(object sender, EventArgs e)
        {
            //ลบ
        }

        private void ReadOnlyControls()
        {
            txtName.ReadOnly = false;
            txtIdCompany.ReadOnly = false;
            txtPhone.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtAddress.ReadOnly = false;
        }
        private void EnableControls()
        {
            txtName.Enabled = true;
            txtIdCompany.Enabled = true;
            txtPhone.Enabled = true;
            txtEmail.Enabled = true;
            txtAddress.Enabled = true;
        }
        private void ClearForm()
        {
            txtName.Clear();
            txtIdCompany.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
        }
    }
}
