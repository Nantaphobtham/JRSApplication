namespace JRSApplication
{
    partial class ProjectManagerForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Body = new System.Windows.Forms.Panel();
            this.PicLogo = new System.Windows.Forms.PictureBox();
            this.Siderbar = new System.Windows.Forms.Panel();
            this.menuContainer = new System.Windows.Forms.Panel();
            this.btnChooseSubcontractors = new System.Windows.Forms.Button();
            this.btnAllocateEmployee = new System.Windows.Forms.Button();
            this.btnPurchaseOrder = new System.Windows.Forms.Button();
            this.Header = new System.Windows.Forms.Panel();
            this.txtsubFunctionname = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.txtFunctionname = new System.Windows.Forms.Label();
            this.txtPosition = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.Label();
            this.Profile = new System.Windows.Forms.PictureBox();
            this.menuTransition = new System.Windows.Forms.Timer(this.components);
            this.btnProjectInformation = new System.Windows.Forms.Button();
            this.btnPaymentsInfomation = new System.Windows.Forms.Button();
            this.btnRequestsforApproval = new System.Windows.Forms.Button();
            this.btnProjectPhaseUpdate = new System.Windows.Forms.Button();
            this.btnHeadmenu = new System.Windows.Forms.Button();
            this.Body.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicLogo)).BeginInit();
            this.Siderbar.SuspendLayout();
            this.menuContainer.SuspendLayout();
            this.Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Profile)).BeginInit();
            this.SuspendLayout();
            // 
            // Body
            // 
            this.Body.Controls.Add(this.PicLogo);
            this.Body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Body.Location = new System.Drawing.Point(336, 126);
            this.Body.Name = "Body";
            this.Body.Size = new System.Drawing.Size(1584, 954);
            this.Body.TabIndex = 3;
            // 
            // PicLogo
            // 
            this.PicLogo.Image = global::JRSApplication.Properties.Resources.logo;
            this.PicLogo.Location = new System.Drawing.Point(110, 32);
            this.PicLogo.Name = "PicLogo";
            this.PicLogo.Size = new System.Drawing.Size(1365, 891);
            this.PicLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicLogo.TabIndex = 0;
            this.PicLogo.TabStop = false;
            // 
            // Siderbar
            // 
            this.Siderbar.BackgroundImage = global::JRSApplication.Properties.Resources.sidebar;
            this.Siderbar.Controls.Add(this.btnProjectInformation);
            this.Siderbar.Controls.Add(this.menuContainer);
            this.Siderbar.Controls.Add(this.btnChooseSubcontractors);
            this.Siderbar.Controls.Add(this.btnAllocateEmployee);
            this.Siderbar.Controls.Add(this.btnPurchaseOrder);
            this.Siderbar.Controls.Add(this.btnPaymentsInfomation);
            this.Siderbar.Dock = System.Windows.Forms.DockStyle.Left;
            this.Siderbar.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Siderbar.Location = new System.Drawing.Point(0, 126);
            this.Siderbar.Name = "Siderbar";
            this.Siderbar.Size = new System.Drawing.Size(336, 954);
            this.Siderbar.TabIndex = 2;
            // 
            // menuContainer
            // 
            this.menuContainer.BackColor = System.Drawing.Color.Transparent;
            this.menuContainer.Controls.Add(this.btnRequestsforApproval);
            this.menuContainer.Controls.Add(this.btnProjectPhaseUpdate);
            this.menuContainer.Controls.Add(this.btnHeadmenu);
            this.menuContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuContainer.Location = new System.Drawing.Point(0, 320);
            this.menuContainer.Name = "menuContainer";
            this.menuContainer.Size = new System.Drawing.Size(336, 80);
            this.menuContainer.TabIndex = 4;
            // 
            // btnChooseSubcontractors
            // 
            this.btnChooseSubcontractors.BackColor = System.Drawing.Color.Transparent;
            this.btnChooseSubcontractors.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnChooseSubcontractors.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnChooseSubcontractors.FlatAppearance.BorderSize = 0;
            this.btnChooseSubcontractors.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnChooseSubcontractors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseSubcontractors.Location = new System.Drawing.Point(0, 240);
            this.btnChooseSubcontractors.Name = "btnChooseSubcontractors";
            this.btnChooseSubcontractors.Size = new System.Drawing.Size(336, 80);
            this.btnChooseSubcontractors.TabIndex = 3;
            this.btnChooseSubcontractors.Text = "กำหนดผู้รับเหมา";
            this.btnChooseSubcontractors.UseVisualStyleBackColor = false;
            this.btnChooseSubcontractors.Click += new System.EventHandler(this.btnChooseSubcontractors_Click);
            // 
            // btnAllocateEmployee
            // 
            this.btnAllocateEmployee.BackColor = System.Drawing.Color.Transparent;
            this.btnAllocateEmployee.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAllocateEmployee.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnAllocateEmployee.FlatAppearance.BorderSize = 0;
            this.btnAllocateEmployee.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnAllocateEmployee.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAllocateEmployee.Location = new System.Drawing.Point(0, 160);
            this.btnAllocateEmployee.Name = "btnAllocateEmployee";
            this.btnAllocateEmployee.Size = new System.Drawing.Size(336, 80);
            this.btnAllocateEmployee.TabIndex = 2;
            this.btnAllocateEmployee.Text = "จัดสรรบุคลากร";
            this.btnAllocateEmployee.UseVisualStyleBackColor = false;
            this.btnAllocateEmployee.Click += new System.EventHandler(this.btnAllocateEmployee_Click);
            // 
            // btnPurchaseOrder
            // 
            this.btnPurchaseOrder.BackColor = System.Drawing.Color.Transparent;
            this.btnPurchaseOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPurchaseOrder.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnPurchaseOrder.FlatAppearance.BorderSize = 0;
            this.btnPurchaseOrder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnPurchaseOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPurchaseOrder.Location = new System.Drawing.Point(0, 80);
            this.btnPurchaseOrder.Name = "btnPurchaseOrder";
            this.btnPurchaseOrder.Size = new System.Drawing.Size(336, 80);
            this.btnPurchaseOrder.TabIndex = 4;
            this.btnPurchaseOrder.Text = "อนุมัติใบสั่งซื้อ";
            this.btnPurchaseOrder.UseVisualStyleBackColor = false;
            this.btnPurchaseOrder.Click += new System.EventHandler(this.btnPurchaseOrder_Click);
            // 
            // Header
            // 
            this.Header.BackgroundImage = global::JRSApplication.Properties.Resources.header;
            this.Header.Controls.Add(this.txtsubFunctionname);
            this.Header.Controls.Add(this.btnClose);
            this.Header.Controls.Add(this.btnMinimize);
            this.Header.Controls.Add(this.txtFunctionname);
            this.Header.Controls.Add(this.txtPosition);
            this.Header.Controls.Add(this.txtName);
            this.Header.Controls.Add(this.Profile);
            this.Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.Header.Location = new System.Drawing.Point(0, 0);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(1920, 126);
            this.Header.TabIndex = 1;
            // 
            // txtsubFunctionname
            // 
            this.txtsubFunctionname.AutoSize = true;
            this.txtsubFunctionname.BackColor = System.Drawing.Color.Transparent;
            this.txtsubFunctionname.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsubFunctionname.Location = new System.Drawing.Point(960, 67);
            this.txtsubFunctionname.Name = "txtsubFunctionname";
            this.txtsubFunctionname.Size = new System.Drawing.Size(0, 37);
            this.txtsubFunctionname.TabIndex = 6;
            this.txtsubFunctionname.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.Red;
            this.btnClose.Location = new System.Drawing.Point(1842, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(1761, 3);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(75, 23);
            this.btnMinimize.TabIndex = 5;
            this.btnMinimize.Text = "_";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // txtFunctionname
            // 
            this.txtFunctionname.AutoSize = true;
            this.txtFunctionname.BackColor = System.Drawing.Color.Transparent;
            this.txtFunctionname.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFunctionname.Location = new System.Drawing.Point(884, 23);
            this.txtFunctionname.Name = "txtFunctionname";
            this.txtFunctionname.Size = new System.Drawing.Size(152, 37);
            this.txtFunctionname.TabIndex = 4;
            this.txtFunctionname.Text = "ยินดีต้อนรับ";
            this.txtFunctionname.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPosition
            // 
            this.txtPosition.AutoSize = true;
            this.txtPosition.BackColor = System.Drawing.Color.Transparent;
            this.txtPosition.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPosition.Location = new System.Drawing.Point(137, 73);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(86, 30);
            this.txtPosition.TabIndex = 3;
            this.txtPosition.Text = "Position";
            // 
            // txtName
            // 
            this.txtName.AutoSize = true;
            this.txtName.BackColor = System.Drawing.Color.Transparent;
            this.txtName.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(136, 23);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(191, 32);
            this.txtName.TabIndex = 2;
            this.txtName.Text = "Name Lastname";
            // 
            // Profile
            // 
            this.Profile.Location = new System.Drawing.Point(30, 23);
            this.Profile.Name = "Profile";
            this.Profile.Size = new System.Drawing.Size(80, 80);
            this.Profile.TabIndex = 1;
            this.Profile.TabStop = false;
            // 
            // menuTransition
            // 
            this.menuTransition.Interval = 10;
            this.menuTransition.Tick += new System.EventHandler(this.menuTransition_Tick);
            // 
            // btnProjectInformation
            // 
            this.btnProjectInformation.BackColor = System.Drawing.Color.Transparent;
            this.btnProjectInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProjectInformation.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnProjectInformation.FlatAppearance.BorderSize = 0;
            this.btnProjectInformation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnProjectInformation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProjectInformation.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProjectInformation.Location = new System.Drawing.Point(0, 400);
            this.btnProjectInformation.Name = "btnProjectInformation";
            this.btnProjectInformation.Size = new System.Drawing.Size(336, 80);
            this.btnProjectInformation.TabIndex = 0;
            this.btnProjectInformation.Text = "ตรวจสอบข้อมูลโครงการ";
            this.btnProjectInformation.UseVisualStyleBackColor = false;
            this.btnProjectInformation.Click += new System.EventHandler(this.btnProjectInformation_Click);
            // 
            // btnPaymentsInfomation
            // 
            this.btnPaymentsInfomation.BackColor = System.Drawing.Color.Transparent;
            this.btnPaymentsInfomation.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPaymentsInfomation.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnPaymentsInfomation.FlatAppearance.BorderSize = 0;
            this.btnPaymentsInfomation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnPaymentsInfomation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPaymentsInfomation.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPaymentsInfomation.Location = new System.Drawing.Point(0, 0);
            this.btnPaymentsInfomation.Name = "btnPaymentsInfomation";
            this.btnPaymentsInfomation.Size = new System.Drawing.Size(336, 80);
            this.btnPaymentsInfomation.TabIndex = 1;
            this.btnPaymentsInfomation.Text = "ตรวจสอบการชำระเงินโครงการ";
            this.btnPaymentsInfomation.UseVisualStyleBackColor = false;
            this.btnPaymentsInfomation.Click += new System.EventHandler(this.btnPaymentsInfomation_Click);
            // 
            // btnRequestsforApproval
            // 
            this.btnRequestsforApproval.BackColor = System.Drawing.Color.Transparent;
            this.btnRequestsforApproval.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRequestsforApproval.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRequestsforApproval.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnRequestsforApproval.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRequestsforApproval.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRequestsforApproval.Location = new System.Drawing.Point(0, 162);
            this.btnRequestsforApproval.Name = "btnRequestsforApproval";
            this.btnRequestsforApproval.Size = new System.Drawing.Size(336, 80);
            this.btnRequestsforApproval.TabIndex = 4;
            this.btnRequestsforApproval.Text = "รายการคำขออนุมัติผลการดำเนินงาน";
            this.btnRequestsforApproval.UseVisualStyleBackColor = false;
            this.btnRequestsforApproval.Click += new System.EventHandler(this.btnRequestsforApproval_Click);
            // 
            // btnProjectPhaseUpdate
            // 
            this.btnProjectPhaseUpdate.BackColor = System.Drawing.Color.Transparent;
            this.btnProjectPhaseUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProjectPhaseUpdate.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnProjectPhaseUpdate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnProjectPhaseUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProjectPhaseUpdate.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProjectPhaseUpdate.Location = new System.Drawing.Point(0, 80);
            this.btnProjectPhaseUpdate.Name = "btnProjectPhaseUpdate";
            this.btnProjectPhaseUpdate.Size = new System.Drawing.Size(336, 82);
            this.btnProjectPhaseUpdate.TabIndex = 5;
            this.btnProjectPhaseUpdate.Text = "ปรับปรุงข้อมูลเฟส";
            this.btnProjectPhaseUpdate.UseVisualStyleBackColor = false;
            this.btnProjectPhaseUpdate.Click += new System.EventHandler(this.btnProjectPhaseUpdate_Click);
            // 
            // btnHeadmenu
            // 
            this.btnHeadmenu.BackColor = System.Drawing.Color.Transparent;
            this.btnHeadmenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHeadmenu.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnHeadmenu.FlatAppearance.BorderSize = 0;
            this.btnHeadmenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnHeadmenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHeadmenu.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHeadmenu.Location = new System.Drawing.Point(0, 0);
            this.btnHeadmenu.Name = "btnHeadmenu";
            this.btnHeadmenu.Size = new System.Drawing.Size(336, 80);
            this.btnHeadmenu.TabIndex = 2;
            this.btnHeadmenu.Text = "ปรับปรุงข้อมูลโครงการ";
            this.btnHeadmenu.UseVisualStyleBackColor = false;
            this.btnHeadmenu.Click += new System.EventHandler(this.btnHeadmenu_Click);
            // 
            // ProjectManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.Body);
            this.Controls.Add(this.Siderbar);
            this.Controls.Add(this.Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProjectManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProjectManagerDashboard";
            this.Load += new System.EventHandler(this.ProjectManagerForm_Load);
            this.Body.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicLogo)).EndInit();
            this.Siderbar.ResumeLayout(false);
            this.menuContainer.ResumeLayout(false);
            this.Header.ResumeLayout(false);
            this.Header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Profile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Header;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Label txtFunctionname;
        private System.Windows.Forms.Label txtPosition;
        private System.Windows.Forms.Label txtName;
        private System.Windows.Forms.PictureBox Profile;
        private System.Windows.Forms.Panel Siderbar;
        private System.Windows.Forms.Button btnChooseSubcontractors;
        private System.Windows.Forms.Button btnAllocateEmployee;
        private System.Windows.Forms.Panel Body;
        private System.Windows.Forms.PictureBox PicLogo;
        private System.Windows.Forms.Button btnPurchaseOrder;
        private System.Windows.Forms.Timer menuTransition;
        private System.Windows.Forms.Panel menuContainer;
        private System.Windows.Forms.Label txtsubFunctionname;
        private System.Windows.Forms.Button btnProjectInformation;
        private System.Windows.Forms.Button btnPaymentsInfomation;
        private System.Windows.Forms.Button btnRequestsforApproval;
        private System.Windows.Forms.Button btnProjectPhaseUpdate;
        private System.Windows.Forms.Button btnHeadmenu;
    }
}