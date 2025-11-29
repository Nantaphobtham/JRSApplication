using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Drawing;


namespace JRSApplication
{
    public partial class AllocatePersonnel : UserControl
    {
        private bool isEditing = true; // เปิดให้เพิ่มได้เลย
        private bool isAssigned = false; // ต้องกดปุ่ม "กำหนดบุคลากร"
        private int? currentEditId = null;


        public AllocatePersonnel()
        {
            InitializeComponent();
            LoadSupervisionData();
            CustomizeDataGridViewStyle(dtgvCustomer);

            // ยกเลิกก่อนค่อยผูกใหม่
            this.btnSave.Click -= btnSave_Click;
            this.btnEdit.Click -= btnEdit_Click;

            this.btnSearchProject.Click += btnSearchProject_Click; // ปุ่มค้นหาโครงการ
            this.btnSearchEmployee.Click += btnSearchEmployee_Click; // ปุ่มค้นหาพนักงาน
            this.btnSave.Click += btnSave_Click; // ปุ่มบันทึก
            this.btnEdit.Click += btnEdit_Click; // ปุ่มแก้ไข
            this.button1.Click += button1_Click; // ปุ่มกำหนดบุคลากร

            isEditing = true;           // ✅ อนุญาตให้พิมพ์
            currentEditId = null;       // ✅ ไม่มีรายการที่เลือก
            SetFormEditable(true);      // ✅ เปิดให้แก้ทุกช่องตอนเริ่ม

            // ✅ ผูก SearchboxControl ให้ใช้ role Projectmanager / ตรวจสอบข้อมูลโครงการ
            try
            {
                searchboxControl1.DefaultRole = "Projectmanager";
                searchboxControl1.DefaultFunction = "ตรวจสอบข้อมูลโครงการ";
                searchboxControl1.SetRoleAndFunction("Projectmanager", "จัดสรรบุคลากร");

                searchboxControl1.SearchTriggered += SearchboxAllocate_SearchTriggered;
            }
            catch
            {
                // กัน error ตอนเปิดใน Designer
            }
        }

        // ================= Searchbox → filter dtgvCustomer ================

        private void SearchboxAllocate_SearchTriggered(object sender, SearchEventArgs e)
        {
            ApplyAllocateGridFilter(e.SearchBy, e.Keyword);
        }

        private void ApplyAllocateGridFilter(string searchBy, string keyword)
        {
            if (!(dtgvCustomer.DataSource is DataTable table))
                return;

            string q = (keyword ?? "").Trim();

            // ไม่กรอกอะไรเลย = แสดงทุกแถว
            if (string.IsNullOrEmpty(q))
            {
                table.DefaultView.RowFilter = string.Empty;
                return;
            }

            q = EscapeLikeValue(q);
            string filter = "";

            switch (searchBy)
            {
                case "รหัสงาน":
                    // id ของตาราง project_employee_supervision
                    filter = $"CONVERT(id, 'System.String') LIKE '%{q}%'";
                    break;

                case "รหัสโครงการ":
                    filter = $"CONVERT(project_id, 'System.String') LIKE '%{q}%'";
                    break;

                case "ชื่อโครงการ":
                    filter = $"CONVERT(project_name, 'System.String') LIKE '%{q}%'";
                    break;

                case "เลขที่สัญญา":
                    filter = $"CONVERT(contract_number, 'System.String') LIKE '%{q}%'";
                    break;

                case "ชื่อลูกค้า":
                    filter = $"CONVERT(customer_name, 'System.String') LIKE '%{q}%'";
                    break;

                case "อีเมลลูกค้า":
                    filter = $"CONVERT(customer_email, 'System.String') LIKE '%{q}%'";
                    break;

                case "รหัสพนักงาน":
                    filter = $"CONVERT(emp_id, 'System.String') LIKE '%{q}%'";
                    break;

                case "ชื่อพนักงาน":
                    filter =
                        $"CONVERT(emp_first_name, 'System.String') LIKE '%{q}%'" +
                        $" OR CONVERT(emp_last_name, 'System.String') LIKE '%{q}%'";
                    break;

                case "ตำแหน่ง":
                    filter = $"CONVERT(emp_position, 'System.String') LIKE '%{q}%'";
                    break;

                default:
                    // ค้นทุกคอลัมน์หลัก
                    filter =
                        $"CONVERT(id, 'System.String') LIKE '%{q}%'" +
                        $" OR CONVERT(project_id, 'System.String') LIKE '%{q}%'" +
                        $" OR CONVERT(project_name, 'System.String') LIKE '%{q}%'" +
                        $" OR CONVERT(contract_number, 'System.String') LIKE '%{q}%'" +
                        $" OR CONVERT(customer_name, 'System.String') LIKE '%{q}%'" +
                        $" OR CONVERT(customer_email, 'System.String') LIKE '%{q}%'" +
                        $" OR CONVERT(emp_id, 'System.String') LIKE '%{q}%'" +
                        $" OR CONVERT(emp_first_name, 'System.String') LIKE '%{q}%'" +
                        $" OR CONVERT(emp_last_name, 'System.String') LIKE '%{q}%'" +
                        $" OR CONVERT(emp_position, 'System.String') LIKE '%{q}%'";
                    break;
            }

            table.DefaultView.RowFilter = filter;
        }



        private static string EscapeLikeValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            // escape ตัวอักษรพิเศษของ RowFilter: [, ], %, *
            return value
                .Replace("[", "[[]")
                .Replace("]", "[]]")
                .Replace("%", "[%]")
                .Replace("*", "[*]")
                .Replace("'", "''");
        }

        // ================= logic เดิมทั้งหมด =================

        private void SetFormEditable(bool editable)
        {
            bool isEditExisting = isEditing && currentEditId.HasValue;

            // Project Fields
            textBox3.Enabled = false; // project_id - ห้ามแก้
            textBox1.Enabled = false; // project_name - ห้ามแก้
            textBox4.Enabled = false; // contract_number - ห้ามแก้
            textBox5.Enabled = false; // customer_name - ห้ามแก้

            textBox2.Enabled = (editable && isEditExisting); // เบอร์โทรลูกค้า - แก้ได้
            textBox6.Enabled = (editable && isEditExisting); // email - แก้ได้

            // Employee Fields
            textBox10.Enabled = (editable && isEditExisting); // emp_first_name 
            textBox11.Enabled = (editable && isEditExisting); // emp_last_name 
            textBox12.Enabled = (editable && isEditExisting); // เบอร์พนักงาน - แก้ได้
            textBox13.Enabled = (editable && isEditExisting); // ตำแหน่ง 

            // ตั้งค่าสีให้เหมาะสมกับ Enabled
            SetTextboxStyle(textBox3);
            SetTextboxStyle(textBox1);
            SetTextboxStyle(textBox4);
            SetTextboxStyle(textBox5);
            SetTextboxStyle(textBox2);
            SetTextboxStyle(textBox6);
            SetTextboxStyle(textBox10);
            SetTextboxStyle(textBox11);
            SetTextboxStyle(textBox12);
            SetTextboxStyle(textBox13);

            // ปุ่มค้นหา
            btnSearchProject.Enabled = editable && !isEditExisting;
            btnSearchEmployee.Enabled = editable;  // ✅ กดเลือกพนักงานได้เสมอ ถ้าอยู่ในโหมดแก้ไข

        }

        private void SetTextboxStyle(TextBox textbox)
        {
            if (!textbox.Enabled)
            {
                textbox.BackColor = Color.LightGray;
                textbox.ForeColor = Color.Gray;
            }
            else
            {
                textbox.BackColor = Color.White;
                textbox.ForeColor = Color.Black;
            }
        }

        private void SetTextboxColor(TextBox textbox)
        {
            if (textbox.ReadOnly)
            {
                textbox.BackColor = Color.LightGray;
                textbox.ForeColor = Color.Gray;
            }
            else
            {
                textbox.BackColor = Color.White;
                textbox.ForeColor = Color.Black;
            }
        }

        private void CustomizeDataGridViewStyle(DataGridView dgv)
        {
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.BorderStyle = BorderStyle.None;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.BackgroundColor = Color.White;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 30;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 15);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Padding = new Padding(2, 3, 2, 3);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.RowTemplate.Height = 30;
            dgv.GridColor = Color.LightGray;
            dgv.RowHeadersVisible = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeRows = false;
        }

        private void LoadSupervisionData()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            MySqlConnection conn = new MySqlConnection(connStr);
            {
                string query = "SELECT * FROM project_employee_supervision ORDER BY created_at DESC";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable table = new DataTable();

                try
                {
                    adapter.Fill(table);
                    dtgvCustomer.DataSource = table;

                    dtgvCustomer.CellClick += DtgvCustomer_CellClick;


                    //ตั้งชื่อหัว
                    dtgvCustomer.Columns["id"].HeaderText = "รหัสงาน";
                    dtgvCustomer.Columns["project_id"].HeaderText = "รหัสโครงการ";
                    dtgvCustomer.Columns["project_name"].HeaderText = "ชื่อโครงการ";
                    dtgvCustomer.Columns["contract_number"].HeaderText = "เลขที่สัญญา";
                    dtgvCustomer.Columns["customer_name"].HeaderText = "ชื่อลูกค้า";
                    dtgvCustomer.Columns["customer_phone"].HeaderText = "เบอร์โทร";
                    dtgvCustomer.Columns["customer_email"].HeaderText = "อีเมล";
                    dtgvCustomer.Columns["emp_id"].HeaderText = "รหัสพนักงาน";
                    dtgvCustomer.Columns["emp_first_name"].HeaderText = "ชื่อพนักงาน";
                    dtgvCustomer.Columns["emp_last_name"].HeaderText = "นามสกุล";
                    dtgvCustomer.Columns["emp_phone"].HeaderText = "เบอร์พนักงาน";
                    dtgvCustomer.Columns["emp_position"].HeaderText = "ตำแหน่ง";
                    dtgvCustomer.Columns["created_at"].HeaderText = "วันที่บันทึก";

                    dtgvCustomer.ScrollBars = ScrollBars.Both; // เปิดเลื่อนแนวนอน
                    dtgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                    // กำหนดความกว้าง
                    dtgvCustomer.Columns["id"].Width = 60;
                    dtgvCustomer.Columns["project_id"].Width = 100;
                    dtgvCustomer.Columns["project_name"].Width = 150;
                    dtgvCustomer.Columns["contract_number"].Width = 120;
                    dtgvCustomer.Columns["customer_name"].Width = 150;
                    dtgvCustomer.Columns["customer_phone"].Width = 110;
                    dtgvCustomer.Columns["customer_email"].Width = 160;
                    dtgvCustomer.Columns["emp_id"].Width = 80;
                    dtgvCustomer.Columns["emp_first_name"].Width = 120;
                    dtgvCustomer.Columns["emp_last_name"].Width = 120;
                    dtgvCustomer.Columns["emp_phone"].Width = 110;
                    dtgvCustomer.Columns["emp_position"].Width = 100;
                    dtgvCustomer.Columns["created_at"].Width = 110;


                    CustomizeDataGridViewStyle(dtgvCustomer); // ✅ เรียกใช้หลังจากผูกข้อมูล

                    // ✅ เพิ่มลำดับแถว
                    dtgvCustomer.DataBindingComplete += (s, e) =>
                    {
                        for (int i = 0; i < dtgvCustomer.Rows.Count; i++)
                        {
                            dtgvCustomer.Rows[i].HeaderCell.Value = (i + 1).ToString();
                        }
                        dtgvCustomer.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show("โหลดข้อมูลไม่สำเร็จ: " + ex.Message);
                }
            }
        }

        private void DtgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dtgvCustomer.Rows[e.RowIndex].Cells["id"].Value != null)
            {
                DataGridViewRow row = dtgvCustomer.Rows[e.RowIndex];

                // ✅ Project Info
                textBox3.Text = row.Cells["project_id"].Value?.ToString();
                textBox1.Text = row.Cells["project_name"].Value?.ToString();
                textBox4.Text = row.Cells["contract_number"].Value?.ToString();
                textBox5.Text = row.Cells["customer_name"].Value?.ToString();
                textBox2.Text = row.Cells["customer_phone"].Value?.ToString();
                textBox6.Text = row.Cells["customer_email"].Value?.ToString();

                // ✅ Employee Info
                textBox10.Tag = row.Cells["emp_id"].Value?.ToString(); // แอบเก็บ emp_id ไว้
                textBox10.Text = row.Cells["emp_first_name"].Value?.ToString();
                textBox11.Text = row.Cells["emp_last_name"].Value?.ToString();
                textBox12.Text = row.Cells["emp_phone"].Value?.ToString();
                textBox13.Text = row.Cells["emp_position"].Value?.ToString();

                // ✅ ห้ามแก้ไข
                SetFormEditable(false);
                isEditing = false;

                currentEditId = Convert.ToInt32(row.Cells["id"].Value);

            }
        }



        private void btnSearchProject_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("กรุณากดปุ่มแก้ไขก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SearchForm searchForm = new SearchForm("Project");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = searchForm.SelectedID;          // ✅ project_id
                textBox1.Text = searchForm.SelectedName;        // ✅ project_name
                textBox4.Text = searchForm.SelectedContract;    // ✅ contract_number
                textBox5.Text = searchForm.SelectedLastName;    // ✅ customer_name
                textBox2.Text = searchForm.SelectedPhone;       // ✅ customer_phone
                textBox6.Text = searchForm.SelectedEmail;       // ✅ customer_email

            }

        }

        private void btnSearchEmployee_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("กรุณากดปุ่มแก้ไขก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SearchForm searchForm = new SearchForm("Employee");
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                textBox10.Text = searchForm.SelectedName;         // ชื่อ
                textBox11.Text = searchForm.SelectedLastName;    // นามสกุล
                textBox12.Text = searchForm.SelectedPhone;       // เบอร์โทร
                textBox13.Text = searchForm.SelectedIDCardOrRole; // ตำแหน่ง
                textBox10.Tag = searchForm.SelectedID;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox10.Tag?.ToString()))
            {
                MessageBox.Show("กรุณาเลือกโครงการและพนักงานให้ครบก่อนกดกำหนดบุคลากร", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            isAssigned = true;
            MessageBox.Show("กำหนดบุคลากรเรียบร้อย กรุณากดปุ่มบันทึกเพื่อบันทึกข้อมูล", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            isEditing = true;
            SetFormEditable(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("กรุณากดปุ่มแก้ไขก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!isAssigned && !currentEditId.HasValue)
            {
                MessageBox.Show("กรุณากดปุ่ม 'กำหนดบุคลากร' ก่อนบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox10.Tag?.ToString()))
            {
                MessageBox.Show("กรุณาเลือกโครงการและพนักงานก่อนบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string sql = currentEditId.HasValue ?
                    @"UPDATE project_employee_supervision SET
                        project_id = @project_id, project_name = @project_name, contract_number = @contract_number,
                        customer_name = @customer_name, customer_phone = @customer_phone, customer_email = @customer_email,
                        emp_id = @emp_id, emp_first_name = @emp_first_name, emp_last_name = @emp_last_name,
                        emp_phone = @emp_phone, emp_position = @emp_position WHERE id = @id" :
                    @"INSERT INTO project_employee_supervision (project_id, project_name, contract_number, customer_name, customer_phone, customer_email,
                        emp_id, emp_first_name, emp_last_name, emp_phone, emp_position, created_at)
                        VALUES (@project_id, @project_name, @contract_number, @customer_name, @customer_phone, @customer_email,
                        @emp_id, @emp_first_name, @emp_last_name, @emp_phone, @emp_position, NOW());";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@project_id", textBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@project_name", textBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@contract_number", textBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@customer_name", textBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@customer_phone", textBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@customer_email", textBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@emp_id", textBox10.Tag?.ToString() ?? "");
                cmd.Parameters.AddWithValue("@emp_first_name", textBox10.Text.Trim());
                cmd.Parameters.AddWithValue("@emp_last_name", textBox11.Text.Trim());
                cmd.Parameters.AddWithValue("@emp_phone", textBox12.Text.Trim());
                cmd.Parameters.AddWithValue("@emp_position", textBox13.Text.Trim());

                if (currentEditId.HasValue)
                    cmd.Parameters.AddWithValue("@id", currentEditId.Value);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(currentEditId.HasValue ? "แก้ไขข้อมูลสำเร็จ" : "บันทึกข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    isEditing = true;
                    isAssigned = false;
                    currentEditId = null;

                    SetFormEditable(true);
                    LoadSupervisionData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }
            }


        }
    }
}
