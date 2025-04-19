namespace JRSApplication
{
    partial class UpdateProjectPhase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateProjectPhase));
            this.pnlProjectdata = new System.Windows.Forms.Panel();
            this.dtgvProjectData = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtPhaseStatus = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtProjectDetail = new System.Windows.Forms.TextBox();
            this.StarSeclectPhase = new System.Windows.Forms.Label();
            this.cmbSelectPhase = new System.Windows.Forms.ComboBox();
            this.txtProjectID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.lblSeclectPhase = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlWorkflow = new System.Windows.Forms.Panel();
            this.dtgvWorkHistory = new System.Windows.Forms.DataGridView();
            this.lblWorkHistory = new System.Windows.Forms.Label();
            this.pnlStep = new System.Windows.Forms.Panel();
            this.pnlUploadpic = new System.Windows.Forms.Panel();
            this.pnlUploadImages = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.starAmountPictureUpload = new System.Windows.Forms.Label();
            this.lblAmountPictureUpload = new System.Windows.Forms.Label();
            this.cmbAmountPictureUpload = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.cmbPhaseStatus = new System.Windows.Forms.ComboBox();
            this.dtpWorkDate = new System.Windows.Forms.DateTimePicker();
            this.txtJuristicNumber = new System.Windows.Forms.TextBox();
            this.txtWorkRemark = new System.Windows.Forms.TextBox();
            this.txtDetailWorkFlow = new System.Windows.Forms.TextBox();
            this.txtSupplierName = new System.Windows.Forms.TextBox();
            this.starPhaseStatus = new System.Windows.Forms.Label();
            this.starDateDone = new System.Windows.Forms.Label();
            this.lblPhaseStatus = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblIssues = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.btnSearchSupplier = new System.Windows.Forms.Button();
            this.starDetailWorkFlow = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblWorkFlow = new System.Windows.Forms.Label();
            this.searchboxControl2 = new JRSApplication.SearchboxControl();
            this.searchboxControl1 = new JRSApplication.SearchboxControl();
            this.pnlProjectdata.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvProjectData)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.pnlWorkflow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvWorkHistory)).BeginInit();
            this.pnlUploadpic.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlProjectdata
            // 
            this.pnlProjectdata.Controls.Add(this.dtgvProjectData);
            this.pnlProjectdata.Controls.Add(this.label1);
            this.pnlProjectdata.Controls.Add(this.searchboxControl1);
            this.pnlProjectdata.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProjectdata.Location = new System.Drawing.Point(0, 0);
            this.pnlProjectdata.Name = "pnlProjectdata";
            this.pnlProjectdata.Size = new System.Drawing.Size(1411, 360);
            this.pnlProjectdata.TabIndex = 0;
            // 
            // dtgvProjectData
            // 
            this.dtgvProjectData.AllowUserToAddRows = false;
            this.dtgvProjectData.AllowUserToDeleteRows = false;
            this.dtgvProjectData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvProjectData.Location = new System.Drawing.Point(27, 89);
            this.dtgvProjectData.Margin = new System.Windows.Forms.Padding(2);
            this.dtgvProjectData.Name = "dtgvProjectData";
            this.dtgvProjectData.ReadOnly = true;
            this.dtgvProjectData.RowHeadersWidth = 51;
            this.dtgvProjectData.RowTemplate.Height = 24;
            this.dtgvProjectData.Size = new System.Drawing.Size(1512, 253);
            this.dtgvProjectData.TabIndex = 12;
            this.dtgvProjectData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgvProjectData_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(38, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 30);
            this.label1.TabIndex = 10;
            this.label1.Text = "ค้นหาโครงการ";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lblStatus);
            this.pnlMain.Controls.Add(this.txtPhaseStatus);
            this.pnlMain.Controls.Add(this.label6);
            this.pnlMain.Controls.Add(this.txtProjectDetail);
            this.pnlMain.Controls.Add(this.StarSeclectPhase);
            this.pnlMain.Controls.Add(this.cmbSelectPhase);
            this.pnlMain.Controls.Add(this.txtProjectID);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.txtProjectName);
            this.pnlMain.Controls.Add(this.lblSeclectPhase);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.pnlWorkflow);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlMain.Location = new System.Drawing.Point(0, 360);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1411, 1731);
            this.pnlMain.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(1328, 8);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(115, 25);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Text = "สถานะเฟสงาน";
            // 
            // txtPhaseStatus
            // 
            this.txtPhaseStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(0)))));
            this.txtPhaseStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhaseStatus.Enabled = false;
            this.txtPhaseStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhaseStatus.Location = new System.Drawing.Point(1235, 43);
            this.txtPhaseStatus.Name = "txtPhaseStatus";
            this.txtPhaseStatus.ReadOnly = true;
            this.txtPhaseStatus.Size = new System.Drawing.Size(289, 35);
            this.txtPhaseStatus.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(441, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 30);
            this.label6.TabIndex = 9;
            this.label6.Text = "รายละเอียด";
            // 
            // txtProjectDetail
            // 
            this.txtProjectDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectDetail.Location = new System.Drawing.Point(550, 84);
            this.txtProjectDetail.Multiline = true;
            this.txtProjectDetail.Name = "txtProjectDetail";
            this.txtProjectDetail.ReadOnly = true;
            this.txtProjectDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProjectDetail.Size = new System.Drawing.Size(975, 52);
            this.txtProjectDetail.TabIndex = 8;
            // 
            // StarSeclectPhase
            // 
            this.StarSeclectPhase.AutoSize = true;
            this.StarSeclectPhase.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StarSeclectPhase.ForeColor = System.Drawing.Color.Red;
            this.StarSeclectPhase.Location = new System.Drawing.Point(688, 8);
            this.StarSeclectPhase.Name = "StarSeclectPhase";
            this.StarSeclectPhase.Size = new System.Drawing.Size(20, 25);
            this.StarSeclectPhase.TabIndex = 7;
            this.StarSeclectPhase.Text = "*";
            this.StarSeclectPhase.Visible = false;
            // 
            // cmbSelectPhase
            // 
            this.cmbSelectPhase.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSelectPhase.FormattingEnabled = true;
            this.cmbSelectPhase.Location = new System.Drawing.Point(446, 41);
            this.cmbSelectPhase.Name = "cmbSelectPhase";
            this.cmbSelectPhase.Size = new System.Drawing.Size(262, 38);
            this.cmbSelectPhase.TabIndex = 6;
            this.cmbSelectPhase.SelectedIndexChanged += new System.EventHandler(this.cmbSelectPhase_SelectedIndexChanged);
            // 
            // txtProjectID
            // 
            this.txtProjectID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectID.Enabled = false;
            this.txtProjectID.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectID.Location = new System.Drawing.Point(132, 90);
            this.txtProjectID.Name = "txtProjectID";
            this.txtProjectID.ReadOnly = true;
            this.txtProjectID.Size = new System.Drawing.Size(289, 35);
            this.txtProjectID.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(21, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 30);
            this.label3.TabIndex = 4;
            this.label3.Text = "รหัสโครงการ";
            // 
            // txtProjectName
            // 
            this.txtProjectName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectName.Enabled = false;
            this.txtProjectName.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectName.ForeColor = System.Drawing.Color.Black;
            this.txtProjectName.Location = new System.Drawing.Point(132, 36);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.ReadOnly = true;
            this.txtProjectName.Size = new System.Drawing.Size(289, 35);
            this.txtProjectName.TabIndex = 3;
            // 
            // lblSeclectPhase
            // 
            this.lblSeclectPhase.AutoSize = true;
            this.lblSeclectPhase.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeclectPhase.Location = new System.Drawing.Point(441, 8);
            this.lblSeclectPhase.Name = "lblSeclectPhase";
            this.lblSeclectPhase.Size = new System.Drawing.Size(241, 25);
            this.lblSeclectPhase.TabIndex = 1;
            this.lblSeclectPhase.Text = "เลือกเฟสที่ต้องการปรับปรุงข้อมูล";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "ชื่อโครงการ";
            // 
            // pnlWorkflow
            // 
            this.pnlWorkflow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
            this.pnlWorkflow.Controls.Add(this.dtgvWorkHistory);
            this.pnlWorkflow.Controls.Add(this.searchboxControl2);
            this.pnlWorkflow.Controls.Add(this.lblWorkHistory);
            this.pnlWorkflow.Controls.Add(this.pnlStep);
            this.pnlWorkflow.Controls.Add(this.pnlUploadpic);
            this.pnlWorkflow.Controls.Add(this.btnSave);
            this.pnlWorkflow.Controls.Add(this.btnEdit);
            this.pnlWorkflow.Controls.Add(this.cmbPhaseStatus);
            this.pnlWorkflow.Controls.Add(this.dtpWorkDate);
            this.pnlWorkflow.Controls.Add(this.txtJuristicNumber);
            this.pnlWorkflow.Controls.Add(this.txtWorkRemark);
            this.pnlWorkflow.Controls.Add(this.txtDetailWorkFlow);
            this.pnlWorkflow.Controls.Add(this.txtSupplierName);
            this.pnlWorkflow.Controls.Add(this.starPhaseStatus);
            this.pnlWorkflow.Controls.Add(this.starDateDone);
            this.pnlWorkflow.Controls.Add(this.lblPhaseStatus);
            this.pnlWorkflow.Controls.Add(this.label12);
            this.pnlWorkflow.Controls.Add(this.label11);
            this.pnlWorkflow.Controls.Add(this.lblIssues);
            this.pnlWorkflow.Controls.Add(this.label16);
            this.pnlWorkflow.Controls.Add(this.btnSearchSupplier);
            this.pnlWorkflow.Controls.Add(this.starDetailWorkFlow);
            this.pnlWorkflow.Controls.Add(this.label10);
            this.pnlWorkflow.Controls.Add(this.label9);
            this.pnlWorkflow.Controls.Add(this.lblWorkFlow);
            this.pnlWorkflow.Location = new System.Drawing.Point(27, 142);
            this.pnlWorkflow.Name = "pnlWorkflow";
            this.pnlWorkflow.Size = new System.Drawing.Size(1512, 1573);
            this.pnlWorkflow.TabIndex = 0;
            // 
            // dtgvWorkHistory
            // 
            this.dtgvWorkHistory.AllowUserToAddRows = false;
            this.dtgvWorkHistory.AllowUserToDeleteRows = false;
            this.dtgvWorkHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvWorkHistory.Location = new System.Drawing.Point(53, 1237);
            this.dtgvWorkHistory.Margin = new System.Windows.Forms.Padding(2);
            this.dtgvWorkHistory.Name = "dtgvWorkHistory";
            this.dtgvWorkHistory.ReadOnly = true;
            this.dtgvWorkHistory.RowHeadersWidth = 51;
            this.dtgvWorkHistory.RowTemplate.Height = 24;
            this.dtgvWorkHistory.Size = new System.Drawing.Size(1406, 310);
            this.dtgvWorkHistory.TabIndex = 12;
            // 
            // lblWorkHistory
            // 
            this.lblWorkHistory.AutoSize = true;
            this.lblWorkHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkHistory.Location = new System.Drawing.Point(61, 1187);
            this.lblWorkHistory.Name = "lblWorkHistory";
            this.lblWorkHistory.Size = new System.Drawing.Size(200, 30);
            this.lblWorkHistory.TabIndex = 10;
            this.lblWorkHistory.Text = "ประวัติการดำเนินงาน";
            // 
            // pnlStep
            // 
            this.pnlStep.BackColor = System.Drawing.Color.Black;
            this.pnlStep.Location = new System.Drawing.Point(53, 1153);
            this.pnlStep.Name = "pnlStep";
            this.pnlStep.Size = new System.Drawing.Size(1406, 10);
            this.pnlStep.TabIndex = 9;
            // 
            // pnlUploadpic
            // 
            this.pnlUploadpic.BackColor = System.Drawing.Color.White;
            this.pnlUploadpic.Controls.Add(this.pnlUploadImages);
            this.pnlUploadpic.Controls.Add(this.panel4);
            this.pnlUploadpic.Location = new System.Drawing.Point(53, 317);
            this.pnlUploadpic.Name = "pnlUploadpic";
            this.pnlUploadpic.Size = new System.Drawing.Size(1406, 830);
            this.pnlUploadpic.TabIndex = 8;
            // 
            // pnlUploadImages
            // 
            this.pnlUploadImages.AutoScroll = true;
            this.pnlUploadImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUploadImages.Location = new System.Drawing.Point(0, 100);
            this.pnlUploadImages.Name = "pnlUploadImages";
            this.pnlUploadImages.Size = new System.Drawing.Size(1406, 730);
            this.pnlUploadImages.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel4.BackgroundImage")));
            this.panel4.Controls.Add(this.starAmountPictureUpload);
            this.panel4.Controls.Add(this.lblAmountPictureUpload);
            this.panel4.Controls.Add(this.cmbAmountPictureUpload);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1406, 100);
            this.panel4.TabIndex = 0;
            // 
            // starAmountPictureUpload
            // 
            this.starAmountPictureUpload.AutoSize = true;
            this.starAmountPictureUpload.BackColor = System.Drawing.Color.Transparent;
            this.starAmountPictureUpload.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starAmountPictureUpload.ForeColor = System.Drawing.Color.Red;
            this.starAmountPictureUpload.Location = new System.Drawing.Point(168, 15);
            this.starAmountPictureUpload.Name = "starAmountPictureUpload";
            this.starAmountPictureUpload.Size = new System.Drawing.Size(22, 30);
            this.starAmountPictureUpload.TabIndex = 1;
            this.starAmountPictureUpload.Text = "*";
            this.starAmountPictureUpload.Visible = false;
            // 
            // lblAmountPictureUpload
            // 
            this.lblAmountPictureUpload.AutoSize = true;
            this.lblAmountPictureUpload.BackColor = System.Drawing.Color.Transparent;
            this.lblAmountPictureUpload.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountPictureUpload.Location = new System.Drawing.Point(43, 15);
            this.lblAmountPictureUpload.Name = "lblAmountPictureUpload";
            this.lblAmountPictureUpload.Size = new System.Drawing.Size(117, 30);
            this.lblAmountPictureUpload.TabIndex = 1;
            this.lblAmountPictureUpload.Text = "ระบุจำนวนรูป";
            // 
            // cmbAmountPictureUpload
            // 
            this.cmbAmountPictureUpload.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAmountPictureUpload.FormattingEnabled = true;
            this.cmbAmountPictureUpload.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cmbAmountPictureUpload.Location = new System.Drawing.Point(39, 48);
            this.cmbAmountPictureUpload.Name = "cmbAmountPictureUpload";
            this.cmbAmountPictureUpload.Size = new System.Drawing.Size(147, 38);
            this.cmbAmountPictureUpload.TabIndex = 0;
            this.cmbAmountPictureUpload.SelectedIndexChanged += new System.EventHandler(this.cmbAmountPictureUpload_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(255)))), ((int)(((byte)(78)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(1293, 232);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(180, 58);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "บันทึกข้อมูล";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.White;
            this.btnEdit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(1098, 232);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(180, 58);
            this.btnEdit.TabIndex = 7;
            this.btnEdit.Text = "แก้ไขข้อมูล";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // cmbPhaseStatus
            // 
            this.cmbPhaseStatus.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPhaseStatus.FormattingEnabled = true;
            this.cmbPhaseStatus.Location = new System.Drawing.Point(1273, 113);
            this.cmbPhaseStatus.Name = "cmbPhaseStatus";
            this.cmbPhaseStatus.Size = new System.Drawing.Size(200, 38);
            this.cmbPhaseStatus.TabIndex = 6;
            // 
            // dtpWorkDate
            // 
            this.dtpWorkDate.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpWorkDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpWorkDate.Location = new System.Drawing.Point(1008, 114);
            this.dtpWorkDate.Name = "dtpWorkDate";
            this.dtpWorkDate.Size = new System.Drawing.Size(229, 35);
            this.dtpWorkDate.TabIndex = 5;
            // 
            // txtJuristicNumber
            // 
            this.txtJuristicNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJuristicNumber.Enabled = false;
            this.txtJuristicNumber.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJuristicNumber.Location = new System.Drawing.Point(615, 114);
            this.txtJuristicNumber.Name = "txtJuristicNumber";
            this.txtJuristicNumber.ReadOnly = true;
            this.txtJuristicNumber.Size = new System.Drawing.Size(355, 35);
            this.txtJuristicNumber.TabIndex = 4;
            // 
            // txtWorkRemark
            // 
            this.txtWorkRemark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWorkRemark.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWorkRemark.Location = new System.Drawing.Point(615, 198);
            this.txtWorkRemark.Multiline = true;
            this.txtWorkRemark.Name = "txtWorkRemark";
            this.txtWorkRemark.Size = new System.Drawing.Size(468, 92);
            this.txtWorkRemark.TabIndex = 4;
            // 
            // txtDetailWorkFlow
            // 
            this.txtDetailWorkFlow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetailWorkFlow.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetailWorkFlow.Location = new System.Drawing.Point(47, 198);
            this.txtDetailWorkFlow.Multiline = true;
            this.txtDetailWorkFlow.Name = "txtDetailWorkFlow";
            this.txtDetailWorkFlow.Size = new System.Drawing.Size(554, 92);
            this.txtDetailWorkFlow.TabIndex = 4;
            // 
            // txtSupplierName
            // 
            this.txtSupplierName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSupplierName.Enabled = false;
            this.txtSupplierName.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSupplierName.Location = new System.Drawing.Point(246, 114);
            this.txtSupplierName.Name = "txtSupplierName";
            this.txtSupplierName.ReadOnly = true;
            this.txtSupplierName.Size = new System.Drawing.Size(355, 35);
            this.txtSupplierName.TabIndex = 4;
            // 
            // starPhaseStatus
            // 
            this.starPhaseStatus.AutoSize = true;
            this.starPhaseStatus.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starPhaseStatus.ForeColor = System.Drawing.Color.Red;
            this.starPhaseStatus.Location = new System.Drawing.Point(1375, 81);
            this.starPhaseStatus.Name = "starPhaseStatus";
            this.starPhaseStatus.Size = new System.Drawing.Size(22, 30);
            this.starPhaseStatus.TabIndex = 2;
            this.starPhaseStatus.Text = "*";
            this.starPhaseStatus.Visible = false;
            // 
            // starDateDone
            // 
            this.starDateDone.AutoSize = true;
            this.starDateDone.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starDateDone.ForeColor = System.Drawing.Color.Red;
            this.starDateDone.Location = new System.Drawing.Point(1061, 81);
            this.starDateDone.Name = "starDateDone";
            this.starDateDone.Size = new System.Drawing.Size(22, 30);
            this.starDateDone.TabIndex = 2;
            this.starDateDone.Text = "*";
            this.starDateDone.Visible = false;
            // 
            // lblPhaseStatus
            // 
            this.lblPhaseStatus.AutoSize = true;
            this.lblPhaseStatus.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhaseStatus.Location = new System.Drawing.Point(1273, 81);
            this.lblPhaseStatus.Name = "lblPhaseStatus";
            this.lblPhaseStatus.Size = new System.Drawing.Size(96, 30);
            this.lblPhaseStatus.TabIndex = 2;
            this.lblPhaseStatus.Text = "สถานะเฟส";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(1008, 81);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 30);
            this.label12.TabIndex = 2;
            this.label12.Text = "วันที่";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(615, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(174, 30);
            this.label11.TabIndex = 2;
            this.label11.Text = "เลขทะเบียนนิติบุคคล";
            // 
            // lblIssues
            // 
            this.lblIssues.AutoSize = true;
            this.lblIssues.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIssues.Location = new System.Drawing.Point(615, 165);
            this.lblIssues.Name = "lblIssues";
            this.lblIssues.Size = new System.Drawing.Size(177, 30);
            this.lblIssues.TabIndex = 2;
            this.lblIssues.Text = "ปัญหาการดำเนินงาน";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(47, 165);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(214, 30);
            this.label16.TabIndex = 2;
            this.label16.Text = "รายละเอียดการดำเนินงาน";
            // 
            // btnSearchSupplier
            // 
            this.btnSearchSupplier.BackColor = System.Drawing.Color.White;
            this.btnSearchSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnSearchSupplier.Location = new System.Drawing.Point(47, 109);
            this.btnSearchSupplier.Name = "btnSearchSupplier";
            this.btnSearchSupplier.Size = new System.Drawing.Size(184, 46);
            this.btnSearchSupplier.TabIndex = 3;
            this.btnSearchSupplier.Text = "ค้นหาซัพพลายเออร์";
            this.btnSearchSupplier.UseVisualStyleBackColor = false;
            this.btnSearchSupplier.Click += new System.EventHandler(this.btnSearchSupplier_Click);
            // 
            // starDetailWorkFlow
            // 
            this.starDetailWorkFlow.AutoSize = true;
            this.starDetailWorkFlow.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starDetailWorkFlow.ForeColor = System.Drawing.Color.Red;
            this.starDetailWorkFlow.Location = new System.Drawing.Point(267, 165);
            this.starDetailWorkFlow.Name = "starDetailWorkFlow";
            this.starDetailWorkFlow.Size = new System.Drawing.Size(22, 30);
            this.starDetailWorkFlow.TabIndex = 2;
            this.starDetailWorkFlow.Text = "*";
            this.starDetailWorkFlow.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(246, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 30);
            this.label10.TabIndex = 2;
            this.label10.Text = "ชื่อบริษัท";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(64, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(149, 30);
            this.label9.TabIndex = 2;
            this.label9.Text = "ระบุบซัพลายเออร์";
            // 
            // lblWorkFlow
            // 
            this.lblWorkFlow.AutoSize = true;
            this.lblWorkFlow.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkFlow.Location = new System.Drawing.Point(42, 27);
            this.lblWorkFlow.Name = "lblWorkFlow";
            this.lblWorkFlow.Size = new System.Drawing.Size(197, 30);
            this.lblWorkFlow.TabIndex = 2;
            this.lblWorkFlow.Text = "ข้อมูลการดำเนินการ";
            // 
            // searchboxControl2
            // 
            this.searchboxControl2.BackColor = System.Drawing.Color.White;
            this.searchboxControl2.Location = new System.Drawing.Point(809, 1177);
            this.searchboxControl2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchboxControl2.Name = "searchboxControl2";
            this.searchboxControl2.Size = new System.Drawing.Size(650, 50);
            this.searchboxControl2.TabIndex = 11;
            // 
            // searchboxControl1
            // 
            this.searchboxControl1.BackColor = System.Drawing.Color.White;
            this.searchboxControl1.Location = new System.Drawing.Point(874, 22);
            this.searchboxControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchboxControl1.Name = "searchboxControl1";
            this.searchboxControl1.Size = new System.Drawing.Size(650, 50);
            this.searchboxControl1.TabIndex = 0;
            // 
            // UpdateProjectPhase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlProjectdata);
            this.Name = "UpdateProjectPhase";
            this.Size = new System.Drawing.Size(1411, 954);
            this.pnlProjectdata.ResumeLayout(false);
            this.pnlProjectdata.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvProjectData)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlWorkflow.ResumeLayout(false);
            this.pnlWorkflow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvWorkHistory)).EndInit();
            this.pnlUploadpic.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlProjectdata;
        private System.Windows.Forms.Panel pnlMain;
        private SearchboxControl searchboxControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dtgvProjectData;
        private System.Windows.Forms.ComboBox cmbSelectPhase;
        private System.Windows.Forms.TextBox txtProjectID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.Label lblSeclectPhase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlWorkflow;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtPhaseStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtProjectDetail;
        private System.Windows.Forms.Label StarSeclectPhase;
        private System.Windows.Forms.Button btnSearchSupplier;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblWorkFlow;
        private System.Windows.Forms.DateTimePicker dtpWorkDate;
        private System.Windows.Forms.TextBox txtJuristicNumber;
        private System.Windows.Forms.TextBox txtSupplierName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbPhaseStatus;
        private System.Windows.Forms.TextBox txtDetailWorkFlow;
        private System.Windows.Forms.Label starPhaseStatus;
        private System.Windows.Forms.Label starDateDone;
        private System.Windows.Forms.Label lblPhaseStatus;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label starDetailWorkFlow;
        private System.Windows.Forms.Panel pnlUploadpic;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.TextBox txtWorkRemark;
        private System.Windows.Forms.Label lblIssues;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblAmountPictureUpload;
        private System.Windows.Forms.ComboBox cmbAmountPictureUpload;
        private System.Windows.Forms.Panel pnlUploadImages;
        private System.Windows.Forms.Label starAmountPictureUpload;
        private SearchboxControl searchboxControl2;
        private System.Windows.Forms.Label lblWorkHistory;
        private System.Windows.Forms.Panel pnlStep;
        private System.Windows.Forms.DataGridView dtgvWorkHistory;
    }
}
