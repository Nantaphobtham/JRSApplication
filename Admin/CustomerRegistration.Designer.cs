namespace JRSApplication
{
    partial class CustomerRegistration
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlInfomation = new System.Windows.Forms.Panel();
            this.subInfo = new System.Windows.Forms.Panel();
            this.pnlEmail = new System.Windows.Forms.Panel();
            this.starEmail = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.pnlPhone = new System.Windows.Forms.Panel();
            this.starPhone = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.pnlIdcard = new System.Windows.Forms.Panel();
            this.starIdcard = new System.Windows.Forms.Label();
            this.lblIdcard = new System.Windows.Forms.Label();
            this.txtIdcard = new System.Windows.Forms.TextBox();
            this.pnlLastname = new System.Windows.Forms.Panel();
            this.starLastname = new System.Windows.Forms.Label();
            this.lblLastname = new System.Windows.Forms.Label();
            this.txtLastname = new System.Windows.Forms.TextBox();
            this.pnlAddress = new System.Windows.Forms.Panel();
            this.starAddress = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.pnlName = new System.Windows.Forms.Panel();
            this.starName = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblToppic1 = new System.Windows.Forms.Label();
            this.pnlActionMenu = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlStep = new System.Windows.Forms.Panel();
            this.pnlCustomerdata = new System.Windows.Forms.Panel();
            this.dtgvCustomer = new System.Windows.Forms.DataGridView();
            this.searchboxCustomer = new JRSApplication.SearchboxControl();
            this.lblToppic2 = new System.Windows.Forms.Label();
            this.pnlInfomation.SuspendLayout();
            this.subInfo.SuspendLayout();
            this.pnlEmail.SuspendLayout();
            this.pnlPhone.SuspendLayout();
            this.pnlIdcard.SuspendLayout();
            this.pnlLastname.SuspendLayout();
            this.pnlAddress.SuspendLayout();
            this.pnlName.SuspendLayout();
            this.pnlActionMenu.SuspendLayout();
            this.pnlCustomerdata.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvCustomer)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlInfomation
            // 
            this.pnlInfomation.Controls.Add(this.subInfo);
            this.pnlInfomation.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfomation.Location = new System.Drawing.Point(0, 0);
            this.pnlInfomation.Margin = new System.Windows.Forms.Padding(2);
            this.pnlInfomation.Name = "pnlInfomation";
            this.pnlInfomation.Size = new System.Drawing.Size(1584, 248);
            this.pnlInfomation.TabIndex = 0;
            // 
            // subInfo
            // 
            this.subInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
            this.subInfo.Controls.Add(this.pnlEmail);
            this.subInfo.Controls.Add(this.pnlPhone);
            this.subInfo.Controls.Add(this.pnlIdcard);
            this.subInfo.Controls.Add(this.pnlLastname);
            this.subInfo.Controls.Add(this.pnlAddress);
            this.subInfo.Controls.Add(this.pnlName);
            this.subInfo.Controls.Add(this.lblToppic1);
            this.subInfo.Location = new System.Drawing.Point(30, 9);
            this.subInfo.Name = "subInfo";
            this.subInfo.Size = new System.Drawing.Size(1525, 227);
            this.subInfo.TabIndex = 1;
            // 
            // pnlEmail
            // 
            this.pnlEmail.Controls.Add(this.starEmail);
            this.pnlEmail.Controls.Add(this.lblEmail);
            this.pnlEmail.Controls.Add(this.txtEmail);
            this.pnlEmail.Location = new System.Drawing.Point(23, 136);
            this.pnlEmail.Margin = new System.Windows.Forms.Padding(2);
            this.pnlEmail.Name = "pnlEmail";
            this.pnlEmail.Size = new System.Drawing.Size(331, 74);
            this.pnlEmail.TabIndex = 2;
            // 
            // starEmail
            // 
            this.starEmail.AutoSize = true;
            this.starEmail.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starEmail.ForeColor = System.Drawing.Color.Red;
            this.starEmail.Location = new System.Drawing.Point(50, 3);
            this.starEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.starEmail.Name = "starEmail";
            this.starEmail.Size = new System.Drawing.Size(22, 30);
            this.starEmail.TabIndex = 2;
            this.starEmail.Text = "*";
            this.starEmail.Visible = false;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(2, 3);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(55, 30);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "อีเมล";
            // 
            // txtEmail
            // 
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Enabled = false;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(0, 37);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(2);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.ReadOnly = true;
            this.txtEmail.Size = new System.Drawing.Size(329, 36);
            this.txtEmail.TabIndex = 2;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            // 
            // pnlPhone
            // 
            this.pnlPhone.Controls.Add(this.starPhone);
            this.pnlPhone.Controls.Add(this.lblPhone);
            this.pnlPhone.Controls.Add(this.txtPhone);
            this.pnlPhone.Location = new System.Drawing.Point(1107, 55);
            this.pnlPhone.Margin = new System.Windows.Forms.Padding(2);
            this.pnlPhone.Name = "pnlPhone";
            this.pnlPhone.Size = new System.Drawing.Size(331, 74);
            this.pnlPhone.TabIndex = 3;
            // 
            // starPhone
            // 
            this.starPhone.AutoSize = true;
            this.starPhone.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starPhone.ForeColor = System.Drawing.Color.Red;
            this.starPhone.Location = new System.Drawing.Point(124, 3);
            this.starPhone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.starPhone.Name = "starPhone";
            this.starPhone.Size = new System.Drawing.Size(22, 30);
            this.starPhone.TabIndex = 2;
            this.starPhone.Text = "*";
            this.starPhone.Visible = false;
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhone.Location = new System.Drawing.Point(2, 3);
            this.lblPhone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(125, 30);
            this.lblPhone.TabIndex = 2;
            this.lblPhone.Text = "เบอร์โทรศัพท์";
            // 
            // txtPhone
            // 
            this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhone.Enabled = false;
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhone.Location = new System.Drawing.Point(0, 37);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(2);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.ReadOnly = true;
            this.txtPhone.Size = new System.Drawing.Size(329, 36);
            this.txtPhone.TabIndex = 2;
            this.txtPhone.TextChanged += new System.EventHandler(this.txtPhone_TextChanged);
            // 
            // pnlIdcard
            // 
            this.pnlIdcard.Controls.Add(this.starIdcard);
            this.pnlIdcard.Controls.Add(this.lblIdcard);
            this.pnlIdcard.Controls.Add(this.txtIdcard);
            this.pnlIdcard.Location = new System.Drawing.Point(748, 55);
            this.pnlIdcard.Margin = new System.Windows.Forms.Padding(2);
            this.pnlIdcard.Name = "pnlIdcard";
            this.pnlIdcard.Size = new System.Drawing.Size(331, 74);
            this.pnlIdcard.TabIndex = 4;
            // 
            // starIdcard
            // 
            this.starIdcard.AutoSize = true;
            this.starIdcard.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starIdcard.ForeColor = System.Drawing.Color.Red;
            this.starIdcard.Location = new System.Drawing.Point(229, 3);
            this.starIdcard.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.starIdcard.Name = "starIdcard";
            this.starIdcard.Size = new System.Drawing.Size(22, 30);
            this.starIdcard.TabIndex = 2;
            this.starIdcard.Text = "*";
            this.starIdcard.Visible = false;
            // 
            // lblIdcard
            // 
            this.lblIdcard.AutoSize = true;
            this.lblIdcard.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIdcard.Location = new System.Drawing.Point(2, 3);
            this.lblIdcard.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIdcard.Name = "lblIdcard";
            this.lblIdcard.Size = new System.Drawing.Size(232, 30);
            this.lblIdcard.TabIndex = 2;
            this.lblIdcard.Text = "เลขบัตรประจำตัวประชาชน";
            // 
            // txtIdcard
            // 
            this.txtIdcard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIdcard.Enabled = false;
            this.txtIdcard.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIdcard.Location = new System.Drawing.Point(0, 37);
            this.txtIdcard.Margin = new System.Windows.Forms.Padding(2);
            this.txtIdcard.Name = "txtIdcard";
            this.txtIdcard.ReadOnly = true;
            this.txtIdcard.Size = new System.Drawing.Size(329, 36);
            this.txtIdcard.TabIndex = 2;
            this.txtIdcard.TextChanged += new System.EventHandler(this.txtIdcard_TextChanged);
            // 
            // pnlLastname
            // 
            this.pnlLastname.Controls.Add(this.starLastname);
            this.pnlLastname.Controls.Add(this.lblLastname);
            this.pnlLastname.Controls.Add(this.txtLastname);
            this.pnlLastname.Location = new System.Drawing.Point(387, 55);
            this.pnlLastname.Margin = new System.Windows.Forms.Padding(2);
            this.pnlLastname.Name = "pnlLastname";
            this.pnlLastname.Size = new System.Drawing.Size(331, 74);
            this.pnlLastname.TabIndex = 5;
            // 
            // starLastname
            // 
            this.starLastname.AutoSize = true;
            this.starLastname.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starLastname.ForeColor = System.Drawing.Color.Red;
            this.starLastname.Location = new System.Drawing.Point(80, 3);
            this.starLastname.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.starLastname.Name = "starLastname";
            this.starLastname.Size = new System.Drawing.Size(22, 30);
            this.starLastname.TabIndex = 2;
            this.starLastname.Text = "*";
            this.starLastname.Visible = false;
            // 
            // lblLastname
            // 
            this.lblLastname.AutoSize = true;
            this.lblLastname.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastname.Location = new System.Drawing.Point(2, 3);
            this.lblLastname.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLastname.Name = "lblLastname";
            this.lblLastname.Size = new System.Drawing.Size(85, 30);
            this.lblLastname.TabIndex = 2;
            this.lblLastname.Text = "นามสกุล";
            // 
            // txtLastname
            // 
            this.txtLastname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLastname.Enabled = false;
            this.txtLastname.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastname.Location = new System.Drawing.Point(0, 37);
            this.txtLastname.Margin = new System.Windows.Forms.Padding(2);
            this.txtLastname.Name = "txtLastname";
            this.txtLastname.ReadOnly = true;
            this.txtLastname.Size = new System.Drawing.Size(329, 36);
            this.txtLastname.TabIndex = 2;
            this.txtLastname.TextChanged += new System.EventHandler(this.txtLastname_TextChanged);
            // 
            // pnlAddress
            // 
            this.pnlAddress.Controls.Add(this.starAddress);
            this.pnlAddress.Controls.Add(this.lblAddress);
            this.pnlAddress.Controls.Add(this.txtAddress);
            this.pnlAddress.Location = new System.Drawing.Point(387, 136);
            this.pnlAddress.Margin = new System.Windows.Forms.Padding(2);
            this.pnlAddress.Name = "pnlAddress";
            this.pnlAddress.Size = new System.Drawing.Size(443, 74);
            this.pnlAddress.TabIndex = 6;
            // 
            // starAddress
            // 
            this.starAddress.AutoSize = true;
            this.starAddress.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starAddress.ForeColor = System.Drawing.Color.Red;
            this.starAddress.Location = new System.Drawing.Point(47, 3);
            this.starAddress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.starAddress.Name = "starAddress";
            this.starAddress.Size = new System.Drawing.Size(22, 30);
            this.starAddress.TabIndex = 2;
            this.starAddress.Text = "*";
            this.starAddress.Visible = false;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.Location = new System.Drawing.Point(2, 3);
            this.lblAddress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(50, 30);
            this.lblAddress.TabIndex = 2;
            this.lblAddress.Text = "ที่อยู่";
            // 
            // txtAddress
            // 
            this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress.Enabled = false;
            this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.Location = new System.Drawing.Point(0, 37);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(2);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.ReadOnly = true;
            this.txtAddress.Size = new System.Drawing.Size(441, 36);
            this.txtAddress.TabIndex = 2;
            this.txtAddress.TextChanged += new System.EventHandler(this.txtAddress_TextChanged);
            // 
            // pnlName
            // 
            this.pnlName.Controls.Add(this.starName);
            this.pnlName.Controls.Add(this.lblName);
            this.pnlName.Controls.Add(this.txtName);
            this.pnlName.Location = new System.Drawing.Point(23, 52);
            this.pnlName.Margin = new System.Windows.Forms.Padding(2);
            this.pnlName.Name = "pnlName";
            this.pnlName.Size = new System.Drawing.Size(331, 74);
            this.pnlName.TabIndex = 7;
            // 
            // starName
            // 
            this.starName.AutoSize = true;
            this.starName.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starName.ForeColor = System.Drawing.Color.Red;
            this.starName.Location = new System.Drawing.Point(32, 3);
            this.starName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.starName.Name = "starName";
            this.starName.Size = new System.Drawing.Size(22, 30);
            this.starName.TabIndex = 2;
            this.starName.Text = "*";
            this.starName.Visible = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(2, 3);
            this.lblName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 30);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "ชื่อ";
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Enabled = false;
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(0, 37);
            this.txtName.Margin = new System.Windows.Forms.Padding(2);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(329, 36);
            this.txtName.TabIndex = 2;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblToppic1
            // 
            this.lblToppic1.AutoSize = true;
            this.lblToppic1.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToppic1.Location = new System.Drawing.Point(16, 13);
            this.lblToppic1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblToppic1.Name = "lblToppic1";
            this.lblToppic1.Size = new System.Drawing.Size(118, 37);
            this.lblToppic1.TabIndex = 1;
            this.lblToppic1.Text = "ข้อมูลค้า";
            // 
            // pnlActionMenu
            // 
            this.pnlActionMenu.Controls.Add(this.btnDelete);
            this.pnlActionMenu.Controls.Add(this.btnEdit);
            this.pnlActionMenu.Controls.Add(this.btnAdd);
            this.pnlActionMenu.Controls.Add(this.btnSave);
            this.pnlActionMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActionMenu.Location = new System.Drawing.Point(0, 248);
            this.pnlActionMenu.Margin = new System.Windows.Forms.Padding(2);
            this.pnlActionMenu.Name = "pnlActionMenu";
            this.pnlActionMenu.Size = new System.Drawing.Size(1584, 94);
            this.pnlActionMenu.TabIndex = 1;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Red;
            this.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(1380, 17);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(175, 58);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "ลบข้อมูล";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnEdit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkOrange;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(1163, 17);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(175, 58);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "แก้ไขข้อมูล";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(251)))), ((int)(((byte)(77)))));
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(255)))), ((int)(((byte)(78)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(946, 17);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(175, 58);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "เพิ่มข้อมูล";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(251)))), ((int)(((byte)(77)))));
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(255)))), ((int)(((byte)(78)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(729, 17);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(175, 58);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "บันทึกข้อมูล";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlStep
            // 
            this.pnlStep.BackColor = System.Drawing.Color.Black;
            this.pnlStep.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStep.Location = new System.Drawing.Point(0, 342);
            this.pnlStep.Margin = new System.Windows.Forms.Padding(2);
            this.pnlStep.Name = "pnlStep";
            this.pnlStep.Size = new System.Drawing.Size(1584, 10);
            this.pnlStep.TabIndex = 2;
            // 
            // pnlCustomerdata
            // 
            this.pnlCustomerdata.Controls.Add(this.dtgvCustomer);
            this.pnlCustomerdata.Controls.Add(this.searchboxCustomer);
            this.pnlCustomerdata.Controls.Add(this.lblToppic2);
            this.pnlCustomerdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCustomerdata.Location = new System.Drawing.Point(0, 352);
            this.pnlCustomerdata.Margin = new System.Windows.Forms.Padding(2);
            this.pnlCustomerdata.Name = "pnlCustomerdata";
            this.pnlCustomerdata.Size = new System.Drawing.Size(1584, 602);
            this.pnlCustomerdata.TabIndex = 3;
            // 
            // dtgvCustomer
            // 
            this.dtgvCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvCustomer.Location = new System.Drawing.Point(30, 86);
            this.dtgvCustomer.Name = "dtgvCustomer";
            this.dtgvCustomer.Size = new System.Drawing.Size(1525, 500);
            this.dtgvCustomer.TabIndex = 4;
            this.dtgvCustomer.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgvCustomer_CellClick);
            // 
            // searchboxCustomer
            // 
            this.searchboxCustomer.BackColor = System.Drawing.Color.White;
            this.searchboxCustomer.Location = new System.Drawing.Point(905, 22);
            this.searchboxCustomer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchboxCustomer.Name = "searchboxCustomer";
            this.searchboxCustomer.Size = new System.Drawing.Size(650, 50);
            this.searchboxCustomer.TabIndex = 3;
            // 
            // lblToppic2
            // 
            this.lblToppic2.AutoSize = true;
            this.lblToppic2.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToppic2.Location = new System.Drawing.Point(23, 22);
            this.lblToppic2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblToppic2.Name = "lblToppic2";
            this.lblToppic2.Size = new System.Drawing.Size(171, 37);
            this.lblToppic2.TabIndex = 2;
            this.lblToppic2.Text = "ทะเบียนลูกค้า";
            // 
            // CustomerRegistration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.pnlCustomerdata);
            this.Controls.Add(this.pnlStep);
            this.Controls.Add(this.pnlActionMenu);
            this.Controls.Add(this.pnlInfomation);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CustomerRegistration";
            this.Size = new System.Drawing.Size(1584, 954);
            this.pnlInfomation.ResumeLayout(false);
            this.subInfo.ResumeLayout(false);
            this.subInfo.PerformLayout();
            this.pnlEmail.ResumeLayout(false);
            this.pnlEmail.PerformLayout();
            this.pnlPhone.ResumeLayout(false);
            this.pnlPhone.PerformLayout();
            this.pnlIdcard.ResumeLayout(false);
            this.pnlIdcard.PerformLayout();
            this.pnlLastname.ResumeLayout(false);
            this.pnlLastname.PerformLayout();
            this.pnlAddress.ResumeLayout(false);
            this.pnlAddress.PerformLayout();
            this.pnlName.ResumeLayout(false);
            this.pnlName.PerformLayout();
            this.pnlActionMenu.ResumeLayout(false);
            this.pnlCustomerdata.ResumeLayout(false);
            this.pnlCustomerdata.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvCustomer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlInfomation;
        private System.Windows.Forms.Panel pnlActionMenu;
        private System.Windows.Forms.Panel pnlStep;
        private System.Windows.Forms.Panel pnlCustomerdata;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel subInfo;
        private System.Windows.Forms.Panel pnlEmail;
        private System.Windows.Forms.Label starEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Panel pnlPhone;
        private System.Windows.Forms.Label starPhone;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Panel pnlIdcard;
        private System.Windows.Forms.Label starIdcard;
        private System.Windows.Forms.Label lblIdcard;
        private System.Windows.Forms.TextBox txtIdcard;
        private System.Windows.Forms.Panel pnlLastname;
        private System.Windows.Forms.Label starLastname;
        private System.Windows.Forms.Label lblLastname;
        private System.Windows.Forms.TextBox txtLastname;
        private System.Windows.Forms.Panel pnlAddress;
        private System.Windows.Forms.Label starAddress;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Panel pnlName;
        private System.Windows.Forms.Label starName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblToppic1;
        private System.Windows.Forms.Label lblToppic2;
        private SearchboxControl searchboxCustomer;
        private System.Windows.Forms.DataGridView dtgvCustomer;
    }
}
