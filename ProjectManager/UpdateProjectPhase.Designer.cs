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
            this.pnlProjectDetail = new System.Windows.Forms.Panel();
            this.btnSearchProject = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtPhaseStatus = new System.Windows.Forms.TextBox();
            this.cmbSelectPhase = new System.Windows.Forms.ComboBox();
            this.txtProjectID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.lblSeclectPhase = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblAmountPictureUpload = new System.Windows.Forms.Label();
            this.pnlWorkHistory = new System.Windows.Forms.Panel();
            this.dtgvPhaseWorkingHistory = new System.Windows.Forms.DataGridView();
            this.fpnlDatainfomation = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlMailDescription = new System.Windows.Forms.Panel();
            this.dtgvDetailSubcontractorWork = new System.Windows.Forms.DataGridView();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.txtWorkingDescription = new System.Windows.Forms.TextBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblRemark = new System.Windows.Forms.Label();
            this.lblWorkingDescription = new System.Windows.Forms.Label();
            this.lblSeclectStatus = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpkDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlPictureDescription = new System.Windows.Forms.Panel();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.btnEditPicture = new System.Windows.Forms.Button();
            this.btnAddPicture = new System.Windows.Forms.Button();
            this.txtPictureDescription = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnInsertPicture = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dtgvPicturelist = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.pnlProjectDetail.SuspendLayout();
            this.pnlWorkHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvPhaseWorkingHistory)).BeginInit();
            this.fpnlDatainfomation.SuspendLayout();
            this.pnlMailDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvDetailSubcontractorWork)).BeginInit();
            this.pnlPictureDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvPicturelist)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlProjectDetail
            // 
            this.pnlProjectDetail.Controls.Add(this.btnSearchProject);
            this.pnlProjectDetail.Controls.Add(this.lblStatus);
            this.pnlProjectDetail.Controls.Add(this.txtPhaseStatus);
            this.pnlProjectDetail.Controls.Add(this.cmbSelectPhase);
            this.pnlProjectDetail.Controls.Add(this.txtProjectID);
            this.pnlProjectDetail.Controls.Add(this.label3);
            this.pnlProjectDetail.Controls.Add(this.txtProjectName);
            this.pnlProjectDetail.Controls.Add(this.lblSeclectPhase);
            this.pnlProjectDetail.Controls.Add(this.label2);
            this.pnlProjectDetail.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProjectDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlProjectDetail.Name = "pnlProjectDetail";
            this.pnlProjectDetail.Size = new System.Drawing.Size(1572, 84);
            this.pnlProjectDetail.TabIndex = 1;
            // 
            // btnSearchProject
            // 
            this.btnSearchProject.AutoSize = true;
            this.btnSearchProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(226)))), ((int)(((byte)(249)))));
            this.btnSearchProject.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchProject.Location = new System.Drawing.Point(19, 21);
            this.btnSearchProject.Name = "btnSearchProject";
            this.btnSearchProject.Size = new System.Drawing.Size(138, 40);
            this.btnSearchProject.TabIndex = 12;
            this.btnSearchProject.Text = "ค้นหาโครงการ";
            this.btnSearchProject.UseVisualStyleBackColor = false;
            this.btnSearchProject.Click += new System.EventHandler(this.btnSearchProject_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(1195, 24);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(115, 25);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Text = "สถานะเฟสงาน";
            // 
            // txtPhaseStatus
            // 
            this.txtPhaseStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(0)))));
            this.txtPhaseStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhaseStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhaseStatus.Location = new System.Drawing.Point(1316, 21);
            this.txtPhaseStatus.Name = "txtPhaseStatus";
            this.txtPhaseStatus.ReadOnly = true;
            this.txtPhaseStatus.Size = new System.Drawing.Size(241, 35);
            this.txtPhaseStatus.TabIndex = 10;
            // 
            // cmbSelectPhase
            // 
            this.cmbSelectPhase.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSelectPhase.FormattingEnabled = true;
            this.cmbSelectPhase.Location = new System.Drawing.Point(1042, 22);
            this.cmbSelectPhase.Name = "cmbSelectPhase";
            this.cmbSelectPhase.Size = new System.Drawing.Size(147, 38);
            this.cmbSelectPhase.TabIndex = 6;
            this.cmbSelectPhase.SelectedIndexChanged += new System.EventHandler(this.cmbSelectPhase_SelectedIndexChanged);
            // 
            // txtProjectID
            // 
            this.txtProjectID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectID.Enabled = false;
            this.txtProjectID.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectID.Location = new System.Drawing.Point(610, 22);
            this.txtProjectID.Name = "txtProjectID";
            this.txtProjectID.ReadOnly = true;
            this.txtProjectID.Size = new System.Drawing.Size(289, 35);
            this.txtProjectID.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(493, 24);
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
            this.txtProjectName.Location = new System.Drawing.Point(280, 22);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.ReadOnly = true;
            this.txtProjectName.Size = new System.Drawing.Size(205, 35);
            this.txtProjectName.TabIndex = 3;
            // 
            // lblSeclectPhase
            // 
            this.lblSeclectPhase.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeclectPhase.Location = new System.Drawing.Point(915, 21);
            this.lblSeclectPhase.Name = "lblSeclectPhase";
            this.lblSeclectPhase.Size = new System.Drawing.Size(121, 53);
            this.lblSeclectPhase.TabIndex = 1;
            this.lblSeclectPhase.Text = "เลือกเฟสที่ต้องการปรับปรุงข้อมูล";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(169, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "ชื่อโครงการ";
            // 
            // lblAmountPictureUpload
            // 
            this.lblAmountPictureUpload.AutoSize = true;
            this.lblAmountPictureUpload.BackColor = System.Drawing.Color.Transparent;
            this.lblAmountPictureUpload.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountPictureUpload.Location = new System.Drawing.Point(14, 3);
            this.lblAmountPictureUpload.Name = "lblAmountPictureUpload";
            this.lblAmountPictureUpload.Size = new System.Drawing.Size(200, 30);
            this.lblAmountPictureUpload.TabIndex = 1;
            this.lblAmountPictureUpload.Text = "ประวัติการดำเนินงาน";
            // 
            // pnlWorkHistory
            // 
            this.pnlWorkHistory.Controls.Add(this.dtgvPhaseWorkingHistory);
            this.pnlWorkHistory.Controls.Add(this.lblAmountPictureUpload);
            this.pnlWorkHistory.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlWorkHistory.Location = new System.Drawing.Point(0, 84);
            this.pnlWorkHistory.Name = "pnlWorkHistory";
            this.pnlWorkHistory.Size = new System.Drawing.Size(1572, 173);
            this.pnlWorkHistory.TabIndex = 2;
            // 
            // dtgvPhaseWorkingHistory
            // 
            this.dtgvPhaseWorkingHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvPhaseWorkingHistory.Location = new System.Drawing.Point(19, 36);
            this.dtgvPhaseWorkingHistory.Name = "dtgvPhaseWorkingHistory";
            this.dtgvPhaseWorkingHistory.ReadOnly = true;
            this.dtgvPhaseWorkingHistory.Size = new System.Drawing.Size(1538, 128);
            this.dtgvPhaseWorkingHistory.TabIndex = 2;
            // 
            // fpnlDatainfomation
            // 
            this.fpnlDatainfomation.BackColor = System.Drawing.Color.White;
            this.fpnlDatainfomation.Controls.Add(this.panel3);
            this.fpnlDatainfomation.Controls.Add(this.pnlMailDescription);
            this.fpnlDatainfomation.Controls.Add(this.pnlPictureDescription);
            this.fpnlDatainfomation.Location = new System.Drawing.Point(17, 263);
            this.fpnlDatainfomation.Name = "fpnlDatainfomation";
            this.fpnlDatainfomation.Size = new System.Drawing.Size(1538, 688);
            this.fpnlDatainfomation.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(35, 241);
            this.panel3.TabIndex = 1;
            // 
            // pnlMailDescription
            // 
            this.pnlMailDescription.BackColor = System.Drawing.Color.White;
            this.pnlMailDescription.Controls.Add(this.dtgvDetailSubcontractorWork);
            this.pnlMailDescription.Controls.Add(this.btnEdit);
            this.pnlMailDescription.Controls.Add(this.btnSave);
            this.pnlMailDescription.Controls.Add(this.txtRemark);
            this.pnlMailDescription.Controls.Add(this.txtWorkingDescription);
            this.pnlMailDescription.Controls.Add(this.comboBox1);
            this.pnlMailDescription.Controls.Add(this.cmbStatus);
            this.pnlMailDescription.Controls.Add(this.lblRemark);
            this.pnlMailDescription.Controls.Add(this.lblWorkingDescription);
            this.pnlMailDescription.Controls.Add(this.label6);
            this.pnlMailDescription.Controls.Add(this.lblSeclectStatus);
            this.pnlMailDescription.Controls.Add(this.lblDate);
            this.pnlMailDescription.Controls.Add(this.dtpkDate);
            this.pnlMailDescription.Controls.Add(this.label5);
            this.pnlMailDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMailDescription.Location = new System.Drawing.Point(44, 3);
            this.pnlMailDescription.Name = "pnlMailDescription";
            this.pnlMailDescription.Size = new System.Drawing.Size(1460, 241);
            this.pnlMailDescription.TabIndex = 0;
            // 
            // dtgvDetailSubcontractorWork
            // 
            this.dtgvDetailSubcontractorWork.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvDetailSubcontractorWork.Location = new System.Drawing.Point(3, 34);
            this.dtgvDetailSubcontractorWork.Name = "dtgvDetailSubcontractorWork";
            this.dtgvDetailSubcontractorWork.Size = new System.Drawing.Size(759, 194);
            this.dtgvDetailSubcontractorWork.TabIndex = 11;
            // 
            // btnEdit
            // 
            this.btnEdit.AutoSize = true;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(1186, 193);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(124, 40);
            this.btnEdit.TabIndex = 10;
            this.btnEdit.Text = "แก้ไขข้อมูล";
            this.btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(251)))), ((int)(((byte)(77)))));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(1316, 192);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(124, 40);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "บันทึกข้อมูล";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemark.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemark.Location = new System.Drawing.Point(771, 193);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(387, 35);
            this.txtRemark.TabIndex = 7;
            // 
            // txtWorkingDescription
            // 
            this.txtWorkingDescription.BackColor = System.Drawing.Color.White;
            this.txtWorkingDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWorkingDescription.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWorkingDescription.Location = new System.Drawing.Point(768, 122);
            this.txtWorkingDescription.Name = "txtWorkingDescription";
            this.txtWorkingDescription.Size = new System.Drawing.Size(672, 35);
            this.txtWorkingDescription.TabIndex = 7;
            // 
            // cmbStatus
            // 
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(1225, 48);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(175, 38);
            this.cmbStatus.TabIndex = 6;
            // 
            // lblRemark
            // 
            this.lblRemark.AutoSize = true;
            this.lblRemark.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemark.Location = new System.Drawing.Point(766, 160);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(177, 30);
            this.lblRemark.TabIndex = 5;
            this.lblRemark.Text = "ปัญหาการดำเนินงาน";
            // 
            // lblWorkingDescription
            // 
            this.lblWorkingDescription.AutoSize = true;
            this.lblWorkingDescription.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkingDescription.Location = new System.Drawing.Point(763, 89);
            this.lblWorkingDescription.Name = "lblWorkingDescription";
            this.lblWorkingDescription.Size = new System.Drawing.Size(221, 30);
            this.lblWorkingDescription.TabIndex = 5;
            this.lblWorkingDescription.Text = "รายะละเอียดการดำเนินงาน";
            // 
            // lblSeclectStatus
            // 
            this.lblSeclectStatus.AutoSize = true;
            this.lblSeclectStatus.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeclectStatus.Location = new System.Drawing.Point(1220, 18);
            this.lblSeclectStatus.Name = "lblSeclectStatus";
            this.lblSeclectStatus.Size = new System.Drawing.Size(126, 30);
            this.lblSeclectStatus.TabIndex = 5;
            this.lblSeclectStatus.Text = "สถานะเฟสงาน";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(768, 18);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(47, 30);
            this.lblDate.TabIndex = 5;
            this.lblDate.Text = "วันที่";
            // 
            // dtpkDate
            // 
            this.dtpkDate.CalendarFont = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpkDate.Cursor = System.Windows.Forms.Cursors.Default;
            this.dtpkDate.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpkDate.Location = new System.Drawing.Point(768, 51);
            this.dtpkDate.Name = "dtpkDate";
            this.dtpkDate.Size = new System.Drawing.Size(213, 35);
            this.dtpkDate.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(244, 30);
            this.label5.TabIndex = 3;
            this.label5.Text = "รายละเอียดการดำเนินงาน";
            // 
            // pnlPictureDescription
            // 
            this.pnlPictureDescription.Controls.Add(this.dtgvPicturelist);
            this.pnlPictureDescription.Controls.Add(this.pictureBoxPreview);
            this.pnlPictureDescription.Controls.Add(this.btnEditPicture);
            this.pnlPictureDescription.Controls.Add(this.btnAddPicture);
            this.pnlPictureDescription.Controls.Add(this.txtPictureDescription);
            this.pnlPictureDescription.Controls.Add(this.label4);
            this.pnlPictureDescription.Controls.Add(this.btnInsertPicture);
            this.pnlPictureDescription.Controls.Add(this.label1);
            this.pnlPictureDescription.Location = new System.Drawing.Point(3, 250);
            this.pnlPictureDescription.Name = "pnlPictureDescription";
            this.pnlPictureDescription.Size = new System.Drawing.Size(1501, 424);
            this.pnlPictureDescription.TabIndex = 2;
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.BackColor = System.Drawing.Color.Gray;
            this.pictureBoxPreview.Location = new System.Drawing.Point(829, 38);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(558, 203);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPreview.TabIndex = 6;
            this.pictureBoxPreview.TabStop = false;
            // 
            // btnEditPicture
            // 
            this.btnEditPicture.AutoSize = true;
            this.btnEditPicture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            this.btnEditPicture.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditPicture.Location = new System.Drawing.Point(1135, 374);
            this.btnEditPicture.Name = "btnEditPicture";
            this.btnEditPicture.Size = new System.Drawing.Size(124, 40);
            this.btnEditPicture.TabIndex = 5;
            this.btnEditPicture.Text = "แก้ไขรูปภาพ";
            this.btnEditPicture.UseVisualStyleBackColor = false;
            // 
            // btnAddPicture
            // 
            this.btnAddPicture.AutoSize = true;
            this.btnAddPicture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(251)))), ((int)(((byte)(77)))));
            this.btnAddPicture.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddPicture.Location = new System.Drawing.Point(1274, 374);
            this.btnAddPicture.Name = "btnAddPicture";
            this.btnAddPicture.Size = new System.Drawing.Size(113, 40);
            this.btnAddPicture.TabIndex = 5;
            this.btnAddPicture.Text = "เพิ่มรูปภาพ";
            this.btnAddPicture.UseVisualStyleBackColor = false;
            this.btnAddPicture.Click += new System.EventHandler(this.btnAddPicture_Click);
            // 
            // txtPictureDescription
            // 
            this.txtPictureDescription.BackColor = System.Drawing.Color.White;
            this.txtPictureDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPictureDescription.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPictureDescription.Location = new System.Drawing.Point(829, 293);
            this.txtPictureDescription.Multiline = true;
            this.txtPictureDescription.Name = "txtPictureDescription";
            this.txtPictureDescription.Size = new System.Drawing.Size(558, 75);
            this.txtPictureDescription.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1042, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 30);
            this.label4.TabIndex = 3;
            this.label4.Text = "ข้อมูลรูปภาพ";
            // 
            // btnInsertPicture
            // 
            this.btnInsertPicture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(226)))), ((int)(((byte)(249)))));
            this.btnInsertPicture.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInsertPicture.Location = new System.Drawing.Point(829, 247);
            this.btnInsertPicture.Name = "btnInsertPicture";
            this.btnInsertPicture.Size = new System.Drawing.Size(558, 40);
            this.btnInsertPicture.TabIndex = 0;
            this.btnInsertPicture.Text = "เลือกรูปภาพ";
            this.btnInsertPicture.UseVisualStyleBackColor = false;
            this.btnInsertPicture.Click += new System.EventHandler(this.btnInsertPicture_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(318, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "รายการรูปภาพ";
            // 
            // dtgvPicturelist
            // 
            this.dtgvPicturelist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvPicturelist.Location = new System.Drawing.Point(44, 38);
            this.dtgvPicturelist.Name = "dtgvPicturelist";
            this.dtgvPicturelist.Size = new System.Drawing.Size(759, 376);
            this.dtgvPicturelist.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1007, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(180, 25);
            this.label6.TabIndex = 5;
            this.label6.Text = "สถานะงานผู้รับเหมาช่วง";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(1012, 48);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(175, 38);
            this.comboBox1.TabIndex = 6;
            // 
            // UpdateProjectPhase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.fpnlDatainfomation);
            this.Controls.Add(this.pnlWorkHistory);
            this.Controls.Add(this.pnlProjectDetail);
            this.Name = "UpdateProjectPhase";
            this.Size = new System.Drawing.Size(1572, 954);
            this.pnlProjectDetail.ResumeLayout(false);
            this.pnlProjectDetail.PerformLayout();
            this.pnlWorkHistory.ResumeLayout(false);
            this.pnlWorkHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvPhaseWorkingHistory)).EndInit();
            this.fpnlDatainfomation.ResumeLayout(false);
            this.pnlMailDescription.ResumeLayout(false);
            this.pnlMailDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvDetailSubcontractorWork)).EndInit();
            this.pnlPictureDescription.ResumeLayout(false);
            this.pnlPictureDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvPicturelist)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlProjectDetail;
        private System.Windows.Forms.ComboBox cmbSelectPhase;
        private System.Windows.Forms.TextBox txtProjectID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.Label lblSeclectPhase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtPhaseStatus;
        private System.Windows.Forms.Label lblAmountPictureUpload;
        private System.Windows.Forms.Button btnSearchProject;
        private System.Windows.Forms.Panel pnlWorkHistory;
        private System.Windows.Forms.DataGridView dtgvPhaseWorkingHistory;
        private System.Windows.Forms.FlowLayoutPanel fpnlDatainfomation;
        private System.Windows.Forms.Panel pnlMailDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pnlPictureDescription;
        private System.Windows.Forms.Button btnInsertPicture;
        private System.Windows.Forms.TextBox txtPictureDescription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpkDate;
        private System.Windows.Forms.TextBox txtWorkingDescription;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblSeclectStatus;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label lblRemark;
        private System.Windows.Forms.Label lblWorkingDescription;
        private System.Windows.Forms.Button btnAddPicture;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEditPicture;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.DataGridView dtgvDetailSubcontractorWork;
        private System.Windows.Forms.DataGridView dtgvPicturelist;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label6;
    }
}
