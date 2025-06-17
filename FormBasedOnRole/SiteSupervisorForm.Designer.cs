﻿namespace JRSApplication
{
    partial class SiteSupervisorForm
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
            this.Header = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMaximize = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.txtFunctionname = new System.Windows.Forms.Label();
            this.txtPosition = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.Label();
            this.Profile = new System.Windows.Forms.PictureBox();
            this.Siderbar = new System.Windows.Forms.Panel();
            this.btnPurchaseOrder = new System.Windows.Forms.Button();
            this.btnRegisSupplier = new System.Windows.Forms.Button();
            this.btnRegisCustomer = new System.Windows.Forms.Button();
            this.btnCheckProjectInformation = new System.Windows.Forms.Button();
            this.Body = new System.Windows.Forms.Panel();
            this.PicLogo = new System.Windows.Forms.PictureBox();
            this.txtsubFunctionname = new System.Windows.Forms.Label();
            this.Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Profile)).BeginInit();
            this.Siderbar.SuspendLayout();
            this.Body.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // Header
            // 
            this.Header.BackgroundImage = global::JRSApplication.Properties.Resources.header;
            this.Header.Controls.Add(this.btnClose);
            this.Header.Controls.Add(this.btnMaximize);
            this.Header.Controls.Add(this.btnMinimize);
            this.Header.Controls.Add(this.txtsubFunctionname);
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
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.Red;
            this.btnClose.Location = new System.Drawing.Point(1826, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnMaximize
            // 
            this.btnMaximize.BackColor = System.Drawing.Color.Transparent;
            this.btnMaximize.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.Location = new System.Drawing.Point(1743, 3);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(75, 23);
            this.btnMaximize.TabIndex = 5;
            this.btnMaximize.Text = "☐";
            this.btnMaximize.UseVisualStyleBackColor = false;
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(1660, 3);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(75, 23);
            this.btnMinimize.TabIndex = 5;
            this.btnMinimize.Text = "_";
            this.btnMinimize.UseVisualStyleBackColor = false;
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
            // Siderbar
            // 
            this.Siderbar.BackgroundImage = global::JRSApplication.Properties.Resources.sidebar;
            this.Siderbar.Controls.Add(this.btnPurchaseOrder);
            this.Siderbar.Controls.Add(this.btnRegisSupplier);
            this.Siderbar.Controls.Add(this.btnRegisCustomer);
            this.Siderbar.Controls.Add(this.btnCheckProjectInformation);
            this.Siderbar.Dock = System.Windows.Forms.DockStyle.Left;
            this.Siderbar.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Siderbar.Location = new System.Drawing.Point(0, 126);
            this.Siderbar.Name = "Siderbar";
            this.Siderbar.Size = new System.Drawing.Size(336, 954);
            this.Siderbar.TabIndex = 2;
            // 
            // btnPurchaseOrder
            // 
            this.btnPurchaseOrder.BackColor = System.Drawing.Color.Transparent;
            this.btnPurchaseOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPurchaseOrder.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnPurchaseOrder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnPurchaseOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPurchaseOrder.Location = new System.Drawing.Point(0, 240);
            this.btnPurchaseOrder.Name = "btnPurchaseOrder";
            this.btnPurchaseOrder.Size = new System.Drawing.Size(336, 80);
            this.btnPurchaseOrder.TabIndex = 3;
            this.btnPurchaseOrder.Text = "ออกใบสั่งซื้อ";
            this.btnPurchaseOrder.UseVisualStyleBackColor = false;
            this.btnPurchaseOrder.Click += new System.EventHandler(this.btnPurchaseOrder_Click);
            // 
            // btnRegisSupplier
            // 
            this.btnRegisSupplier.BackColor = System.Drawing.Color.Transparent;
            this.btnRegisSupplier.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRegisSupplier.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRegisSupplier.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnRegisSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegisSupplier.Location = new System.Drawing.Point(0, 160);
            this.btnRegisSupplier.Name = "btnRegisSupplier";
            this.btnRegisSupplier.Size = new System.Drawing.Size(336, 80);
            this.btnRegisSupplier.TabIndex = 2;
            this.btnRegisSupplier.Text = "รอออออ";
            this.btnRegisSupplier.UseVisualStyleBackColor = false;
            // 
            // btnRegisCustomer
            // 
            this.btnRegisCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnRegisCustomer.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRegisCustomer.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRegisCustomer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnRegisCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegisCustomer.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegisCustomer.Location = new System.Drawing.Point(0, 80);
            this.btnRegisCustomer.Name = "btnRegisCustomer";
            this.btnRegisCustomer.Size = new System.Drawing.Size(336, 80);
            this.btnRegisCustomer.TabIndex = 1;
            this.btnRegisCustomer.Text = "รออออ";
            this.btnRegisCustomer.UseVisualStyleBackColor = false;
            // 
            // btnCheckProjectInformation
            // 
            this.btnCheckProjectInformation.BackColor = System.Drawing.Color.Transparent;
            this.btnCheckProjectInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCheckProjectInformation.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnCheckProjectInformation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnCheckProjectInformation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckProjectInformation.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckProjectInformation.Location = new System.Drawing.Point(0, 0);
            this.btnCheckProjectInformation.Name = "btnCheckProjectInformation";
            this.btnCheckProjectInformation.Size = new System.Drawing.Size(336, 80);
            this.btnCheckProjectInformation.TabIndex = 0;
            this.btnCheckProjectInformation.Text = "ตรวจสอบข้อมูลโครงการ";
            this.btnCheckProjectInformation.UseVisualStyleBackColor = false;
            this.btnCheckProjectInformation.Click += new System.EventHandler(this.btnCheckProjectInformation_Click);
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
            // txtsubFunctionname
            // 
            this.txtsubFunctionname.AutoSize = true;
            this.txtsubFunctionname.BackColor = System.Drawing.Color.Transparent;
            this.txtsubFunctionname.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsubFunctionname.Location = new System.Drawing.Point(884, 67);
            this.txtsubFunctionname.Name = "txtsubFunctionname";
            this.txtsubFunctionname.Size = new System.Drawing.Size(0, 37);
            this.txtsubFunctionname.TabIndex = 4;
            this.txtsubFunctionname.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SiteSupervisorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.Body);
            this.Controls.Add(this.Siderbar);
            this.Controls.Add(this.Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SiteSupervisorForm";
            this.Text = "SiteSupervisorDashboard";
            this.Header.ResumeLayout(false);
            this.Header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Profile)).EndInit();
            this.Siderbar.ResumeLayout(false);
            this.Body.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Header;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMaximize;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Label txtFunctionname;
        private System.Windows.Forms.Label txtPosition;
        private System.Windows.Forms.Label txtName;
        private System.Windows.Forms.PictureBox Profile;
        private System.Windows.Forms.Panel Siderbar;
        private System.Windows.Forms.Button btnPurchaseOrder;
        private System.Windows.Forms.Button btnRegisSupplier;
        private System.Windows.Forms.Button btnRegisCustomer;
        private System.Windows.Forms.Button btnCheckProjectInformation;
        private System.Windows.Forms.Panel Body;
        private System.Windows.Forms.PictureBox PicLogo;
        private System.Windows.Forms.Label txtsubFunctionname;
    }
}