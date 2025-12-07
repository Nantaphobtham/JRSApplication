namespace JRSApplication
{
    partial class AccountantForm
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
            this.btnMinimize = new System.Windows.Forms.Button();
            this.txtSubFunctionname = new System.Windows.Forms.Label();
            this.txtFunctionname = new System.Windows.Forms.Label();
            this.txtPosition = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.Label();
            this.Profile = new System.Windows.Forms.PictureBox();
            this.Siderbar = new System.Windows.Forms.Panel();
            this.panelReceivePaymentSub = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Body = new System.Windows.Forms.Panel();
            this.PicLogo = new System.Windows.Forms.PictureBox();
            this.Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Profile)).BeginInit();
            this.Siderbar.SuspendLayout();
            this.panelReceivePaymentSub.SuspendLayout();
            this.Body.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // Header
            // 
            this.Header.BackgroundImage = global::JRSApplication.Properties.Resources.header;
            this.Header.Controls.Add(this.btnClose);
            this.Header.Controls.Add(this.btnMinimize);
            this.Header.Controls.Add(this.txtSubFunctionname);
            this.Header.Controls.Add(this.txtFunctionname);
            this.Header.Controls.Add(this.txtPosition);
            this.Header.Controls.Add(this.txtName);
            this.Header.Controls.Add(this.Profile);
            this.Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.Header.Location = new System.Drawing.Point(0, 0);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(1455, 126);
            this.Header.TabIndex = 1;
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
            // txtSubFunctionname
            // 
            this.txtSubFunctionname.AutoSize = true;
            this.txtSubFunctionname.BackColor = System.Drawing.Color.Transparent;
            this.txtSubFunctionname.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubFunctionname.Location = new System.Drawing.Point(1128, 75);
            this.txtSubFunctionname.Name = "txtSubFunctionname";
            this.txtSubFunctionname.Size = new System.Drawing.Size(0, 37);
            this.txtSubFunctionname.TabIndex = 4;
            this.txtSubFunctionname.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.txtName.Click += new System.EventHandler(this.Profile_Click);
            // 
            // Profile
            // 
            this.Profile.BackColor = System.Drawing.Color.White;
            this.Profile.Location = new System.Drawing.Point(37, 23);
            this.Profile.Name = "Profile";
            this.Profile.Size = new System.Drawing.Size(80, 80);
            this.Profile.TabIndex = 1;
            this.Profile.TabStop = false;
            this.Profile.Click += new System.EventHandler(this.Profile_Click);
            // 
            // Siderbar
            // 
            this.Siderbar.BackgroundImage = global::JRSApplication.Properties.Resources.sidebar;
            this.Siderbar.Controls.Add(this.panelReceivePaymentSub);
            this.Siderbar.Dock = System.Windows.Forms.DockStyle.Left;
            this.Siderbar.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Siderbar.Location = new System.Drawing.Point(0, 126);
            this.Siderbar.Name = "Siderbar";
            this.Siderbar.Size = new System.Drawing.Size(336, 768);
            this.Siderbar.TabIndex = 2;
            // 
            // panelReceivePaymentSub
            // 
            this.panelReceivePaymentSub.BackColor = System.Drawing.Color.Transparent;
            this.panelReceivePaymentSub.Controls.Add(this.button3);
            this.panelReceivePaymentSub.Controls.Add(this.button2);
            this.panelReceivePaymentSub.Controls.Add(this.button1);
            this.panelReceivePaymentSub.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelReceivePaymentSub.Location = new System.Drawing.Point(0, 0);
            this.panelReceivePaymentSub.Name = "panelReceivePaymentSub";
            this.panelReceivePaymentSub.Size = new System.Drawing.Size(336, 159);
            this.panelReceivePaymentSub.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button3.Dock = System.Windows.Forms.DockStyle.Top;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(0, 104);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(336, 55);
            this.button3.TabIndex = 6;
            this.button3.Text = "พิมพ์ใบเสร็จรับเงิน";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.btnPrintReceipt_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(0, 52);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(336, 52);
            this.button2.TabIndex = 5;
            this.button2.Text = "ยืนยันการรับชำระเงิน";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.btnConfirmInvoice_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(336, 52);
            this.button1.TabIndex = 4;
            this.button1.Text = "เรียกชำระเงิน";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnInvoice_Click);
            // 
            // Body
            // 
            this.Body.Controls.Add(this.PicLogo);
            this.Body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Body.Location = new System.Drawing.Point(336, 126);
            this.Body.Name = "Body";
            this.Body.Size = new System.Drawing.Size(1119, 768);
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
            // AccountantForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1455, 894);
            this.Controls.Add(this.Body);
            this.Controls.Add(this.Siderbar);
            this.Controls.Add(this.Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AccountantForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AccountantDashboard";
            this.Header.ResumeLayout(false);
            this.Header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Profile)).EndInit();
            this.Siderbar.ResumeLayout(false);
            this.panelReceivePaymentSub.ResumeLayout(false);
            this.Body.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicLogo)).EndInit();
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
        private System.Windows.Forms.Panel Body;
        private System.Windows.Forms.PictureBox PicLogo;
        private System.Windows.Forms.Label txtSubFunctionname;
        private System.Windows.Forms.Panel panelReceivePaymentSub;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}