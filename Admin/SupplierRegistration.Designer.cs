namespace JRSApplication
{
    partial class SupplierRegistration
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
            this.pnlIdCompany = new System.Windows.Forms.Panel();
            this.starIdCompany = new System.Windows.Forms.Label();
            this.lblIdCompany = new System.Windows.Forms.Label();
            this.txtJuristic = new System.Windows.Forms.TextBox();
            this.pnlName = new System.Windows.Forms.Panel();
            this.starCompanyName = new System.Windows.Forms.Label();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblToppic1 = new System.Windows.Forms.Label();
            this.starAddress = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.pnlAddress = new System.Windows.Forms.Panel();
            this.lblToppic2 = new System.Windows.Forms.Label();
            this.dtgvSupplier = new System.Windows.Forms.DataGridView();
            this.pnlSuppilerdata = new System.Windows.Forms.Panel();
            this.searchboxSuppiler = new JRSApplication.SearchboxControl();
            this.pnlStep = new System.Windows.Forms.Panel();
            this.pnlActionMenu = new System.Windows.Forms.Panel();
            this.btDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
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
            this.pnlIdCompany.SuspendLayout();
            this.pnlName.SuspendLayout();
            this.pnlAddress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvSupplier)).BeginInit();
            this.pnlSuppilerdata.SuspendLayout();
            this.pnlActionMenu.SuspendLayout();
            this.pnlInfomation.SuspendLayout();
            this.subInfo.SuspendLayout();
            this.pnlEmail.SuspendLayout();
            this.pnlPhone.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlIdCompany
            // 
            this.pnlIdCompany.Controls.Add(this.starIdCompany);
            this.pnlIdCompany.Controls.Add(this.lblIdCompany);
            this.pnlIdCompany.Controls.Add(this.txtJuristic);
            this.pnlIdCompany.Location = new System.Drawing.Point(420, 54);
            this.pnlIdCompany.Margin = new System.Windows.Forms.Padding(2);
            this.pnlIdCompany.Name = "pnlIdCompany";
            this.pnlIdCompany.Size = new System.Drawing.Size(331, 74);
            this.pnlIdCompany.TabIndex = 4;
            // 
            // starIdCompany
            // 
            this.starIdCompany.AutoSize = true;
            this.starIdCompany.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starIdCompany.ForeColor = System.Drawing.Color.Red;
            this.starIdCompany.Location = new System.Drawing.Point(192, 3);
            this.starIdCompany.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.starIdCompany.Name = "starIdCompany";
            this.starIdCompany.Size = new System.Drawing.Size(22, 30);
            this.starIdCompany.TabIndex = 2;
            this.starIdCompany.Text = "*";
            this.starIdCompany.Visible = false;
            // 
            // lblIdCompany
            // 
            this.lblIdCompany.AutoSize = true;
            this.lblIdCompany.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIdCompany.Location = new System.Drawing.Point(2, 3);
            this.lblIdCompany.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIdCompany.Name = "lblIdCompany";
            this.lblIdCompany.Size = new System.Drawing.Size(186, 30);
            this.lblIdCompany.TabIndex = 2;
            this.lblIdCompany.Text = "เลขทะเบียนนิติบุคคล";
            // 
            // txtJuristic
            // 
            this.txtJuristic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJuristic.Enabled = false;
            this.txtJuristic.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJuristic.Location = new System.Drawing.Point(0, 37);
            this.txtJuristic.Margin = new System.Windows.Forms.Padding(2);
            this.txtJuristic.Name = "txtJuristic";
            this.txtJuristic.ReadOnly = true;
            this.txtJuristic.Size = new System.Drawing.Size(329, 36);
            this.txtJuristic.TabIndex = 2;
            this.txtJuristic.TextChanged += new System.EventHandler(this.txtJuristic_TextChanged);
            // 
            // pnlName
            // 
            this.pnlName.Controls.Add(this.starCompanyName);
            this.pnlName.Controls.Add(this.lblCompanyName);
            this.pnlName.Controls.Add(this.txtName);
            this.pnlName.Location = new System.Drawing.Point(70, 54);
            this.pnlName.Margin = new System.Windows.Forms.Padding(2);
            this.pnlName.Name = "pnlName";
            this.pnlName.Size = new System.Drawing.Size(331, 74);
            this.pnlName.TabIndex = 7;
            // 
            // starCompanyName
            // 
            this.starCompanyName.AutoSize = true;
            this.starCompanyName.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starCompanyName.ForeColor = System.Drawing.Color.Red;
            this.starCompanyName.Location = new System.Drawing.Point(83, 3);
            this.starCompanyName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.starCompanyName.Name = "starCompanyName";
            this.starCompanyName.Size = new System.Drawing.Size(22, 30);
            this.starCompanyName.TabIndex = 2;
            this.starCompanyName.Text = "*";
            this.starCompanyName.Visible = false;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyName.Location = new System.Drawing.Point(2, 3);
            this.lblCompanyName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(86, 30);
            this.lblCompanyName.TabIndex = 2;
            this.lblCompanyName.Text = "ชื่อบริษัท";
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
            this.lblToppic1.Location = new System.Drawing.Point(70, 5);
            this.lblToppic1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblToppic1.Name = "lblToppic1";
            this.lblToppic1.Size = new System.Drawing.Size(318, 37);
            this.lblToppic1.TabIndex = 1;
            this.lblToppic1.Text = "ข้อมูลบริษัทซัพพลายเออร์";
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
            // pnlAddress
            // 
            this.pnlAddress.Controls.Add(this.starAddress);
            this.pnlAddress.Controls.Add(this.lblAddress);
            this.pnlAddress.Controls.Add(this.txtAddress);
            this.pnlAddress.Location = new System.Drawing.Point(70, 144);
            this.pnlAddress.Margin = new System.Windows.Forms.Padding(2);
            this.pnlAddress.Name = "pnlAddress";
            this.pnlAddress.Size = new System.Drawing.Size(443, 74);
            this.pnlAddress.TabIndex = 6;
            // 
            // lblToppic2
            // 
            this.lblToppic2.AutoSize = true;
            this.lblToppic2.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToppic2.Location = new System.Drawing.Point(23, 22);
            this.lblToppic2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblToppic2.Name = "lblToppic2";
            this.lblToppic2.Size = new System.Drawing.Size(309, 37);
            this.lblToppic2.TabIndex = 2;
            this.lblToppic2.Text = "บัญชีบริษัทซัพพลายเออร์";
            // 
            // dtgvSupplier
            // 
            this.dtgvSupplier.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvSupplier.Location = new System.Drawing.Point(30, 86);
            this.dtgvSupplier.Name = "dtgvSupplier";
            this.dtgvSupplier.Size = new System.Drawing.Size(1525, 522);
            this.dtgvSupplier.TabIndex = 4;
            this.dtgvSupplier.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgvSupplier_CellClick);
            // 
            // pnlSuppilerdata
            // 
            this.pnlSuppilerdata.Controls.Add(this.dtgvSupplier);
            this.pnlSuppilerdata.Controls.Add(this.searchboxSuppiler);
            this.pnlSuppilerdata.Controls.Add(this.lblToppic2);
            this.pnlSuppilerdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSuppilerdata.Location = new System.Drawing.Point(0, 343);
            this.pnlSuppilerdata.Margin = new System.Windows.Forms.Padding(2);
            this.pnlSuppilerdata.Name = "pnlSuppilerdata";
            this.pnlSuppilerdata.Size = new System.Drawing.Size(1584, 611);
            this.pnlSuppilerdata.TabIndex = 7;
            // 
            // searchboxSuppiler
            // 
            this.searchboxSuppiler.BackColor = System.Drawing.Color.White;
            this.searchboxSuppiler.Location = new System.Drawing.Point(905, 22);
            this.searchboxSuppiler.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchboxSuppiler.Name = "searchboxSuppiler";
            this.searchboxSuppiler.Size = new System.Drawing.Size(650, 50);
            this.searchboxSuppiler.TabIndex = 3;
            // 
            // pnlStep
            // 
            this.pnlStep.BackColor = System.Drawing.Color.Black;
            this.pnlStep.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStep.Location = new System.Drawing.Point(0, 333);
            this.pnlStep.Margin = new System.Windows.Forms.Padding(2);
            this.pnlStep.Name = "pnlStep";
            this.pnlStep.Size = new System.Drawing.Size(1584, 10);
            this.pnlStep.TabIndex = 6;
            // 
            // pnlActionMenu
            // 
            this.pnlActionMenu.Controls.Add(this.btDelete);
            this.pnlActionMenu.Controls.Add(this.btnEdit);
            this.pnlActionMenu.Controls.Add(this.btnAdd);
            this.pnlActionMenu.Controls.Add(this.btnSave);
            this.pnlActionMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActionMenu.Location = new System.Drawing.Point(0, 239);
            this.pnlActionMenu.Margin = new System.Windows.Forms.Padding(2);
            this.pnlActionMenu.Name = "pnlActionMenu";
            this.pnlActionMenu.Size = new System.Drawing.Size(1584, 94);
            this.pnlActionMenu.TabIndex = 5;
            // 
            // btDelete
            // 
            this.btDelete.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDelete.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btDelete.Location = new System.Drawing.Point(1380, 17);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(175, 58);
            this.btDelete.TabIndex = 3;
            this.btDelete.Text = "ลบข้อมูล";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnEdit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkOrange;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(1163, 17);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(175, 58);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "แก้ไขข้อมูล";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(255)))), ((int)(((byte)(78)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(946, 17);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(175, 58);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "เพิ่มข้อมูล";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(255)))), ((int)(((byte)(78)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(729, 17);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(175, 58);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "บันทึกข้อมูล";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlInfomation
            // 
            this.pnlInfomation.Controls.Add(this.subInfo);
            this.pnlInfomation.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfomation.Location = new System.Drawing.Point(0, 0);
            this.pnlInfomation.Margin = new System.Windows.Forms.Padding(2);
            this.pnlInfomation.Name = "pnlInfomation";
            this.pnlInfomation.Size = new System.Drawing.Size(1584, 239);
            this.pnlInfomation.TabIndex = 4;
            // 
            // subInfo
            // 
            this.subInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
            this.subInfo.Controls.Add(this.pnlEmail);
            this.subInfo.Controls.Add(this.pnlPhone);
            this.subInfo.Controls.Add(this.pnlIdCompany);
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
            this.pnlEmail.Location = new System.Drawing.Point(1116, 54);
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
            this.pnlPhone.Location = new System.Drawing.Point(769, 54);
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
            // SupplierRegistration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlSuppilerdata);
            this.Controls.Add(this.pnlStep);
            this.Controls.Add(this.pnlActionMenu);
            this.Controls.Add(this.pnlInfomation);
            this.Name = "SupplierRegistration";
            this.Size = new System.Drawing.Size(1584, 954);
            this.pnlIdCompany.ResumeLayout(false);
            this.pnlIdCompany.PerformLayout();
            this.pnlName.ResumeLayout(false);
            this.pnlName.PerformLayout();
            this.pnlAddress.ResumeLayout(false);
            this.pnlAddress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvSupplier)).EndInit();
            this.pnlSuppilerdata.ResumeLayout(false);
            this.pnlSuppilerdata.PerformLayout();
            this.pnlActionMenu.ResumeLayout(false);
            this.pnlInfomation.ResumeLayout(false);
            this.subInfo.ResumeLayout(false);
            this.subInfo.PerformLayout();
            this.pnlEmail.ResumeLayout(false);
            this.pnlEmail.PerformLayout();
            this.pnlPhone.ResumeLayout(false);
            this.pnlPhone.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlIdCompany;
        private System.Windows.Forms.Label starIdCompany;
        private System.Windows.Forms.Label lblIdCompany;
        private System.Windows.Forms.TextBox txtJuristic;
        private System.Windows.Forms.Panel pnlName;
        private System.Windows.Forms.Label starCompanyName;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.TextBox txtName;
        private SearchboxControl searchboxSuppiler;
        private System.Windows.Forms.Label lblToppic1;
        private System.Windows.Forms.Label starAddress;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Panel pnlAddress;
        private System.Windows.Forms.Label lblToppic2;
        private System.Windows.Forms.DataGridView dtgvSupplier;
        private System.Windows.Forms.Panel pnlSuppilerdata;
        private System.Windows.Forms.Panel pnlStep;
        private System.Windows.Forms.Panel pnlActionMenu;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlInfomation;
        private System.Windows.Forms.Panel subInfo;
        private System.Windows.Forms.Panel pnlEmail;
        private System.Windows.Forms.Label starEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Panel pnlPhone;
        private System.Windows.Forms.Label starPhone;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
    }
}
