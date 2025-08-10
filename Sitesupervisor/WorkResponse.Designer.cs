namespace JRSApplication.Sitesupervisor
{
    partial class WorkResponse
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
            this.dtgvWorkResponse = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvWorkResponse)).BeginInit();
            this.SuspendLayout();
            // 
            // searchboxControl1
            // 
            this.searchboxControl1.BackColor = System.Drawing.Color.White;
            this.searchboxControl1.Location = new System.Drawing.Point(918, 12);
            this.searchboxControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchboxControl1.Name = "searchboxControl1";
            this.searchboxControl1.Size = new System.Drawing.Size(650, 50);
            this.searchboxControl1.TabIndex = 0;
            // 
            // dtgvWorkResponse
            // 
            this.dtgvWorkResponse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvWorkResponse.Location = new System.Drawing.Point(17, 79);
            this.dtgvWorkResponse.Name = "dtgvWorkResponse";
            this.dtgvWorkResponse.Size = new System.Drawing.Size(1551, 841);
            this.dtgvWorkResponse.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "รายการผลการอนุมัติ";
            // 
            // WorkResponse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtgvWorkResponse);
            this.Controls.Add(this.searchboxControl1);
            this.Name = "WorkResponse";
            this.Size = new System.Drawing.Size(1584, 954);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvWorkResponse)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SearchboxControl searchboxControl1;
        private System.Windows.Forms.DataGridView dtgvWorkResponse;
        private System.Windows.Forms.Label label1;
    }
}
