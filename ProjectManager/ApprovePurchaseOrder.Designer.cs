namespace JRSApplication
{
    partial class ApprovePurchaseOrder
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtgvProject = new System.Windows.Forms.DataGridView();
            this.txtNormalData = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.searchboxControl1 = new JRSApplication.SearchboxControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvProject)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtgvProject);
            this.panel1.Controls.Add(this.searchboxControl1);
            this.panel1.Controls.Add(this.txtNormalData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1533, 527);
            this.panel1.TabIndex = 0;
            // 
            // dtgvProject
            // 
            this.dtgvProject.AllowUserToAddRows = false;
            this.dtgvProject.AllowUserToDeleteRows = false;
            this.dtgvProject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvProject.Location = new System.Drawing.Point(28, 104);
            this.dtgvProject.Margin = new System.Windows.Forms.Padding(2);
            this.dtgvProject.Name = "dtgvProject";
            this.dtgvProject.ReadOnly = true;
            this.dtgvProject.RowHeadersWidth = 51;
            this.dtgvProject.RowTemplate.Height = 24;
            this.dtgvProject.Size = new System.Drawing.Size(1502, 406);
            this.dtgvProject.TabIndex = 5;
            // 
            // txtNormalData
            // 
            this.txtNormalData.AutoSize = true;
            this.txtNormalData.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNormalData.Location = new System.Drawing.Point(64, 39);
            this.txtNormalData.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtNormalData.Name = "txtNormalData";
            this.txtNormalData.Size = new System.Drawing.Size(159, 30);
            this.txtNormalData.TabIndex = 3;
            this.txtNormalData.Text = "รายการใบสั่งซื้อ";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 585);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1533, 680);
            this.panel2.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(1328, 599);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(202, 63);
            this.button2.TabIndex = 8;
            this.button2.Text = "ไม่อนุมัติใบสั่งซื้อ";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(242)))), ((int)(((byte)(74)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(1070, 599);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(202, 63);
            this.button1.TabIndex = 7;
            this.button1.Text = "อนุมัติใบสั่งซื้อ";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(28, 75);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1502, 501);
            this.dataGridView1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(64, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(328, 30);
            this.label1.TabIndex = 4;
            this.label1.Text = "รายการวัสดุก่อสร้างและวัสดุไฟฟ้า";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 527);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1533, 58);
            this.panel3.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(64, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 30);
            this.label2.TabIndex = 5;
            this.label2.Text = "ตรวจสอบใบสั่งซื้อ";
            // 
            // searchboxControl1
            // 
            this.searchboxControl1.BackColor = System.Drawing.Color.White;
            this.searchboxControl1.Location = new System.Drawing.Point(880, 39);
            this.searchboxControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchboxControl1.Name = "searchboxControl1";
            this.searchboxControl1.Size = new System.Drawing.Size(650, 50);
            this.searchboxControl1.TabIndex = 4;
            // 
            // ApprovePurchaseOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ApprovePurchaseOrder";
            this.Size = new System.Drawing.Size(1533, 954);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvProject)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label txtNormalData;
        private SearchboxControl searchboxControl1;
        private System.Windows.Forms.DataGridView dtgvProject;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}
