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
            this.searchboxControl1 = new JRSApplication.SearchboxControl();
            this.dtgvRequestApproval = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvRequestApproval)).BeginInit();
            this.SuspendLayout();
            // 
            // searchboxControl1
            // 
            this.searchboxControl1.BackColor = System.Drawing.Color.White;
            this.searchboxControl1.Location = new System.Drawing.Point(898, 9);
            this.searchboxControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchboxControl1.Name = "searchboxControl1";
            this.searchboxControl1.Size = new System.Drawing.Size(650, 50);
            this.searchboxControl1.TabIndex = 15;
            // 
            // dtgvRequestApproval
            // 
            this.dtgvRequestApproval.AllowUserToAddRows = false;
            this.dtgvRequestApproval.AllowUserToDeleteRows = false;
            this.dtgvRequestApproval.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvRequestApproval.Location = new System.Drawing.Point(36, 76);
            this.dtgvRequestApproval.Margin = new System.Windows.Forms.Padding(2);
            this.dtgvRequestApproval.Name = "dtgvRequestApproval";
            this.dtgvRequestApproval.ReadOnly = true;
            this.dtgvRequestApproval.RowHeadersWidth = 51;
            this.dtgvRequestApproval.RowTemplate.Height = 24;
            this.dtgvRequestApproval.Size = new System.Drawing.Size(1512, 851);
            this.dtgvRequestApproval.TabIndex = 14;
            this.dtgvRequestApproval.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgvRequestApproval_CellDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 30);
            this.label1.TabIndex = 13;
            this.label1.Text = "ค้นหาโครงการ";
            // 
            // ProjectPhaseListRequestsforApproval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchboxControl1);
            this.Controls.Add(this.dtgvRequestApproval);
            this.Name = "ProjectPhaseListRequestsforApproval";
            this.Size = new System.Drawing.Size(1584, 954);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvRequestApproval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private SearchboxControl searchboxControl1;
        private System.Windows.Forms.DataGridView dtgvRequestApproval;
        private System.Windows.Forms.Label label1;
    }
}
