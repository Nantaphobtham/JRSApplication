namespace JRSApplication.Components.Service
{
    partial class ChangePassword1
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
            this.components = new System.ComponentModel.Container();
            this.pnlRole = new System.Windows.Forms.Panel();
            this.txtPosition = new System.Windows.Forms.TextBox();
            this.starRole = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            this.pnlconfirmPassword = new System.Windows.Forms.Panel();
            this.chkShowNewPassword = new System.Windows.Forms.CheckBox();
            this.starConfirmPassword = new System.Windows.Forms.Label();
            this.lblconfirmPassword = new System.Windows.Forms.Label();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.pnlUsername = new System.Windows.Forms.Panel();
            this.starUsername = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.pnlPassword = new System.Windows.Forms.Panel();
            this.chkShowOldPassword = new System.Windows.Forms.CheckBox();
            this.starPassword = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveChanges = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtOldPassword = new System.Windows.Forms.TextBox();
            this.pnlRole.SuspendLayout();
            this.pnlconfirmPassword.SuspendLayout();
            this.pnlUsername.SuspendLayout();
            this.pnlPassword.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlRole
            // 
            this.pnlRole.Controls.Add(this.txtPosition);
            this.pnlRole.Controls.Add(this.starRole);
            this.pnlRole.Controls.Add(this.lblRole);
            this.pnlRole.Location = new System.Drawing.Point(568, 175);
            this.pnlRole.Margin = new System.Windows.Forms.Padding(2);
            this.pnlRole.Name = "pnlRole";
            this.pnlRole.Size = new System.Drawing.Size(311, 74);
            this.pnlRole.TabIndex = 21;
            // 
            // txtPosition
            // 
            this.txtPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPosition.Enabled = false;
            this.txtPosition.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPosition.Location = new System.Drawing.Point(2, 36);
            this.txtPosition.Margin = new System.Windows.Forms.Padding(2);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.ReadOnly = true;
            this.txtPosition.Size = new System.Drawing.Size(308, 36);
            this.txtPosition.TabIndex = 3;
            // 
            // starRole
            // 
            this.starRole.AutoSize = true;
            this.starRole.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starRole.ForeColor = System.Drawing.Color.Red;
            this.starRole.Location = new System.Drawing.Point(76, 3);
            this.starRole.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.starRole.Name = "starRole";
            this.starRole.Size = new System.Drawing.Size(22, 30);
            this.starRole.TabIndex = 2;
            this.starRole.Text = "*";
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRole.Location = new System.Drawing.Point(2, 3);
            this.lblRole.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(82, 30);
            this.lblRole.TabIndex = 2;
            this.lblRole.Text = "ตำแหน่ง";
            // 
            // pnlconfirmPassword
            // 
            this.pnlconfirmPassword.Controls.Add(this.chkShowNewPassword);
            this.pnlconfirmPassword.Controls.Add(this.starConfirmPassword);
            this.pnlconfirmPassword.Controls.Add(this.lblconfirmPassword);
            this.pnlconfirmPassword.Controls.Add(this.txtNewPassword);
            this.pnlconfirmPassword.Location = new System.Drawing.Point(568, 256);
            this.pnlconfirmPassword.Margin = new System.Windows.Forms.Padding(2);
            this.pnlconfirmPassword.Name = "pnlconfirmPassword";
            this.pnlconfirmPassword.Size = new System.Drawing.Size(311, 98);
            this.pnlconfirmPassword.TabIndex = 20;
            // 
            // chkShowNewPassword
            // 
            this.chkShowNewPassword.AutoSize = true;
            this.chkShowNewPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowNewPassword.Location = new System.Drawing.Point(219, 76);
            this.chkShowNewPassword.Margin = new System.Windows.Forms.Padding(2);
            this.chkShowNewPassword.Name = "chkShowNewPassword";
            this.chkShowNewPassword.Size = new System.Drawing.Size(87, 19);
            this.chkShowNewPassword.TabIndex = 3;
            this.chkShowNewPassword.Text = "แสดงรหัสผ่าน";
            this.chkShowNewPassword.UseVisualStyleBackColor = true;
            this.chkShowNewPassword.CheckedChanged += new System.EventHandler(this.chkShowNewPassword_CheckedChanged);
            // 
            // starConfirmPassword
            // 
            this.starConfirmPassword.AutoSize = true;
            this.starConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starConfirmPassword.ForeColor = System.Drawing.Color.Red;
            this.starConfirmPassword.Location = new System.Drawing.Point(109, 3);
            this.starConfirmPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.starConfirmPassword.Name = "starConfirmPassword";
            this.starConfirmPassword.Size = new System.Drawing.Size(22, 30);
            this.starConfirmPassword.TabIndex = 2;
            this.starConfirmPassword.Text = "*";
            // 
            // lblconfirmPassword
            // 
            this.lblconfirmPassword.AutoSize = true;
            this.lblconfirmPassword.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblconfirmPassword.Location = new System.Drawing.Point(2, 3);
            this.lblconfirmPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblconfirmPassword.Name = "lblconfirmPassword";
            this.lblconfirmPassword.Size = new System.Drawing.Size(114, 30);
            this.lblconfirmPassword.TabIndex = 2;
            this.lblconfirmPassword.Text = "รหัสผ่านใหม่";
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewPassword.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewPassword.Location = new System.Drawing.Point(0, 37);
            this.txtNewPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Size = new System.Drawing.Size(308, 36);
            this.txtNewPassword.TabIndex = 2;
            // 
            // pnlUsername
            // 
            this.pnlUsername.Controls.Add(this.starUsername);
            this.pnlUsername.Controls.Add(this.lblUsername);
            this.pnlUsername.Controls.Add(this.txtUsername);
            this.pnlUsername.Location = new System.Drawing.Point(244, 175);
            this.pnlUsername.Margin = new System.Windows.Forms.Padding(2);
            this.pnlUsername.Name = "pnlUsername";
            this.pnlUsername.Size = new System.Drawing.Size(311, 74);
            this.pnlUsername.TabIndex = 18;
            // 
            // starUsername
            // 
            this.starUsername.AutoSize = true;
            this.starUsername.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starUsername.ForeColor = System.Drawing.Color.Red;
            this.starUsername.Location = new System.Drawing.Point(70, 3);
            this.starUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.starUsername.Name = "starUsername";
            this.starUsername.Size = new System.Drawing.Size(22, 30);
            this.starUsername.TabIndex = 2;
            this.starUsername.Text = "*";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.Location = new System.Drawing.Point(2, 3);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(73, 30);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "ชื่อผู้ใช้";
            // 
            // txtUsername
            // 
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsername.Enabled = false;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(0, 37);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(2);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.ReadOnly = true;
            this.txtUsername.Size = new System.Drawing.Size(308, 36);
            this.txtUsername.TabIndex = 2;
            // 
            // pnlPassword
            // 
            this.pnlPassword.Controls.Add(this.txtOldPassword);
            this.pnlPassword.Controls.Add(this.chkShowOldPassword);
            this.pnlPassword.Controls.Add(this.starPassword);
            this.pnlPassword.Controls.Add(this.lblPassword);
            this.pnlPassword.Location = new System.Drawing.Point(244, 256);
            this.pnlPassword.Margin = new System.Windows.Forms.Padding(2);
            this.pnlPassword.Name = "pnlPassword";
            this.pnlPassword.Size = new System.Drawing.Size(311, 98);
            this.pnlPassword.TabIndex = 19;
            // 
            // chkShowOldPassword
            // 
            this.chkShowOldPassword.AutoSize = true;
            this.chkShowOldPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowOldPassword.Location = new System.Drawing.Point(217, 76);
            this.chkShowOldPassword.Margin = new System.Windows.Forms.Padding(2);
            this.chkShowOldPassword.Name = "chkShowOldPassword";
            this.chkShowOldPassword.Size = new System.Drawing.Size(87, 19);
            this.chkShowOldPassword.TabIndex = 3;
            this.chkShowOldPassword.Text = "แสดงรหัสผ่าน";
            this.chkShowOldPassword.UseVisualStyleBackColor = true;
            this.chkShowOldPassword.CheckedChanged += new System.EventHandler(this.chkShowOldPassword_CheckedChanged);
            // 
            // starPassword
            // 
            this.starPassword.AutoSize = true;
            this.starPassword.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starPassword.ForeColor = System.Drawing.Color.Red;
            this.starPassword.Location = new System.Drawing.Point(105, 5);
            this.starPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.starPassword.Name = "starPassword";
            this.starPassword.Size = new System.Drawing.Size(22, 30);
            this.starPassword.TabIndex = 2;
            this.starPassword.Text = "*";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(2, 3);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(112, 30);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "รหัสผ่านเก่า";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel1.Controls.Add(this.btnSaveChanges);
            this.panel1.Location = new System.Drawing.Point(211, 131);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(699, 300);
            this.panel1.TabIndex = 22;
            // 
            // btnSaveChanges
            // 
            this.btnSaveChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(251)))), ((int)(((byte)(77)))));
            this.btnSaveChanges.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSaveChanges.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(255)))), ((int)(((byte)(78)))));
            this.btnSaveChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveChanges.Font = new System.Drawing.Font("Segoe UI", 12.75F);
            this.btnSaveChanges.Location = new System.Drawing.Point(493, 239);
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new System.Drawing.Size(175, 58);
            this.btnSaveChanges.TabIndex = 1;
            this.btnSaveChanges.Text = "ยืนยันเปลี่ยนรหัสผ่าน";
            this.btnSaveChanges.UseVisualStyleBackColor = false;
            this.btnSaveChanges.Click += new System.EventHandler(this.btnSaveChanges_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // txtOldPassword
            // 
            this.txtOldPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOldPassword.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOldPassword.Location = new System.Drawing.Point(0, 37);
            this.txtOldPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.Size = new System.Drawing.Size(308, 36);
            this.txtOldPassword.TabIndex = 4;
            // 
            // ChangePassword1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlRole);
            this.Controls.Add(this.pnlconfirmPassword);
            this.Controls.Add(this.pnlUsername);
            this.Controls.Add(this.pnlPassword);
            this.Controls.Add(this.panel1);
            this.Name = "ChangePassword1";
            this.Size = new System.Drawing.Size(1120, 563);
            this.Load += new System.EventHandler(this.uc_ChangePassword_Load);
            this.pnlRole.ResumeLayout(false);
            this.pnlRole.PerformLayout();
            this.pnlconfirmPassword.ResumeLayout(false);
            this.pnlconfirmPassword.PerformLayout();
            this.pnlUsername.ResumeLayout(false);
            this.pnlUsername.PerformLayout();
            this.pnlPassword.ResumeLayout(false);
            this.pnlPassword.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlRole;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.Label starRole;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Panel pnlconfirmPassword;
        private System.Windows.Forms.CheckBox chkShowNewPassword;
        private System.Windows.Forms.Label starConfirmPassword;
        private System.Windows.Forms.Label lblconfirmPassword;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Panel pnlUsername;
        private System.Windows.Forms.Label starUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Panel pnlPassword;
        private System.Windows.Forms.CheckBox chkShowOldPassword;
        private System.Windows.Forms.Label starPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSaveChanges;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox txtOldPassword;
    }
}
