namespace JRSApplication
{
    partial class ProjectPhaseListRequestsforApproval
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
            this.label1 = new System.Windows.Forms.Label();
            this.searchboxControl1 = new JRSApplication.SearchboxControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvProject)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.searchboxControl1);
            this.panel1.Controls.Add(this.dtgvProject);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1584, 348);
            this.panel1.TabIndex = 0;
            // 
            // dtgvProject
            // 
            this.dtgvProject.AllowUserToAddRows = false;
            this.dtgvProject.AllowUserToDeleteRows = false;
            this.dtgvProject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvProject.Location = new System.Drawing.Point(36, 82);
            this.dtgvProject.Margin = new System.Windows.Forms.Padding(2);
            this.dtgvProject.Name = "dtgvProject";
            this.dtgvProject.ReadOnly = true;
            this.dtgvProject.RowHeadersWidth = 51;
            this.dtgvProject.RowTemplate.Height = 24;
            this.dtgvProject.Size = new System.Drawing.Size(1512, 253);
            this.dtgvProject.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(56, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 30);
            this.label1.TabIndex = 13;
            this.label1.Text = "ค้นหาโครงการ";
            // 
            // searchboxControl1
            // 
            this.searchboxControl1.BackColor = System.Drawing.Color.White;
            this.searchboxControl1.Location = new System.Drawing.Point(898, 15);
            this.searchboxControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchboxControl1.Name = "searchboxControl1";
            this.searchboxControl1.Size = new System.Drawing.Size(650, 50);
            this.searchboxControl1.TabIndex = 15;
            // 
            // ProjectPhaseListRequestsforApproval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panel1);
            this.Name = "ProjectPhaseListRequestsforApproval";
            this.Size = new System.Drawing.Size(1584, 954);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvProject)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private SearchboxControl searchboxControl1;
        private System.Windows.Forms.DataGridView dtgvProject;
        private System.Windows.Forms.Label label1;
    }
}
