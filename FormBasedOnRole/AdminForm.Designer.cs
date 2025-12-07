using System;

namespace JRSApplication
{
    partial class AdminForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminForm));
            this.Body = new System.Windows.Forms.Panel();
            this.PicLogo = new System.Windows.Forms.PictureBox();
            this.Siderbar = new System.Windows.Forms.Panel();
            this.btnManageProject = new System.Windows.Forms.Button();
            this.btnRegisSupplier = new System.Windows.Forms.Button();
            this.btnRegisCustomer = new System.Windows.Forms.Button();
            this.btnManageUser = new System.Windows.Forms.Button();
            this.Header = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.txtFunctionname = new System.Windows.Forms.Label();
            this.txtPosition = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.Label();
            this.Profile = new System.Windows.Forms.PictureBox();
            this.Body.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicLogo)).BeginInit();
            this.Siderbar.SuspendLayout();
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
            this.Body.TabIndex = 2;
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
            this.PicLogo.Click += new System.EventHandler(this.Profile_Click);
            // 
            // Siderbar
            // 
            this.Siderbar.BackgroundImage = global::JRSApplication.Properties.Resources.sidebar;
            this.Siderbar.Controls.Add(this.btnManageProject);
            this.Siderbar.Controls.Add(this.btnRegisSupplier);
            this.Siderbar.Controls.Add(this.btnRegisCustomer);
            this.Siderbar.Controls.Add(this.btnManageUser);
            this.Siderbar.Dock = System.Windows.Forms.DockStyle.Left;
            this.Siderbar.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Siderbar.Location = new System.Drawing.Point(0, 126);
            this.Siderbar.Name = "Siderbar";
            this.Siderbar.Size = new System.Drawing.Size(336, 954);
            this.Siderbar.TabIndex = 1;
            // 
            // btnManageProject
            // 
            this.btnManageProject.BackColor = System.Drawing.Color.Transparent;
            this.btnManageProject.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnManageProject.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnManageProject.FlatAppearance.BorderSize = 0;
            this.btnManageProject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnManageProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageProject.Location = new System.Drawing.Point(0, 240);
            this.btnManageProject.Name = "btnManageProject";
            this.btnManageProject.Size = new System.Drawing.Size(336, 80);
            this.btnManageProject.TabIndex = 3;
            this.btnManageProject.Text = "จัดการข้อมูลโครงการ";
            this.btnManageProject.UseVisualStyleBackColor = false;
            this.btnManageProject.Click += new System.EventHandler(this.btnManageProject_Click);
            // 
            // btnRegisSupplier
            // 
            this.btnRegisSupplier.BackColor = System.Drawing.Color.Transparent;
            this.btnRegisSupplier.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRegisSupplier.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRegisSupplier.FlatAppearance.BorderSize = 0;
            this.btnRegisSupplier.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnRegisSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegisSupplier.Location = new System.Drawing.Point(0, 160);
            this.btnRegisSupplier.Name = "btnRegisSupplier";
            this.btnRegisSupplier.Size = new System.Drawing.Size(336, 80);
            this.btnRegisSupplier.TabIndex = 2;
            this.btnRegisSupplier.Text = "ทะเบียนซัพพลายเออร์";
            this.btnRegisSupplier.UseVisualStyleBackColor = false;
            this.btnRegisSupplier.Click += new System.EventHandler(this.btnRegisSupplier_Click);
            // 
            // btnRegisCustomer
            // 
            this.btnRegisCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnRegisCustomer.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRegisCustomer.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRegisCustomer.FlatAppearance.BorderSize = 0;
            this.btnRegisCustomer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnRegisCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegisCustomer.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegisCustomer.Location = new System.Drawing.Point(0, 80);
            this.btnRegisCustomer.Name = "btnRegisCustomer";
            this.btnRegisCustomer.Size = new System.Drawing.Size(336, 80);
            this.btnRegisCustomer.TabIndex = 1;
            this.btnRegisCustomer.Text = "ทะเบียนลูกค้า";
            this.btnRegisCustomer.UseVisualStyleBackColor = false;
            this.btnRegisCustomer.Click += new System.EventHandler(this.btnRegisCustomer_Click);
            // 
            // btnManageUser
            // 
            this.btnManageUser.BackColor = System.Drawing.Color.Transparent;
            this.btnManageUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnManageUser.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnManageUser.FlatAppearance.BorderSize = 0;
            this.btnManageUser.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnManageUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageUser.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageUser.Location = new System.Drawing.Point(0, 0);
            this.btnManageUser.Name = "btnManageUser";
            this.btnManageUser.Size = new System.Drawing.Size(336, 80);
            this.btnManageUser.TabIndex = 0;
            this.btnManageUser.Text = "จัดการบัญชีผู้ใช้";
            this.btnManageUser.UseVisualStyleBackColor = false;
            this.btnManageUser.Click += new System.EventHandler(this.btnManageUser_Click);
            // 
            // Header
            // 
            this.Header.BackgroundImage = global::JRSApplication.Properties.Resources.header;
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
            this.Header.TabIndex = 0;
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
            this.txtFunctionname.Location = new System.Drawing.Point(884, 45);
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
            this.Profile.Click += new System.EventHandler(this.Profile_Click);
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.Body);
            this.Controls.Add(this.Siderbar);
            this.Controls.Add(this.Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AdminForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AdminForm";
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this.Body.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicLogo)).EndInit();
            this.Siderbar.ResumeLayout(false);
            this.Header.ResumeLayout(false);
            this.Header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Profile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Header;
        private System.Windows.Forms.Panel Siderbar;
        private System.Windows.Forms.Panel Body;
        private System.Windows.Forms.Label txtPosition;
        private System.Windows.Forms.Label txtName;
        private System.Windows.Forms.PictureBox Profile;
        private System.Windows.Forms.Label txtFunctionname;
        private System.Windows.Forms.Button btnManageUser;
        private System.Windows.Forms.Button btnRegisCustomer;
        private System.Windows.Forms.Button btnManageProject;
        private System.Windows.Forms.Button btnRegisSupplier;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.PictureBox PicLogo;
    }
}