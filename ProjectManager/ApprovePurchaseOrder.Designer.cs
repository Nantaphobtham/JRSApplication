namespace JRSApplication.ProjectManager
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
            this.label1 = new System.Windows.Forms.Label();
            this.dtgvListofPO = new System.Windows.Forms.DataGridView();
            this.searchboxControl1 = new JRSApplication.SearchboxControl();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvListofPO)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "รายการใบสั่งซื้อ";
            // 
            // dtgvListofPO
            // 
            this.dtgvListofPO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvListofPO.Location = new System.Drawing.Point(29, 80);
            this.dtgvListofPO.Name = "dtgvListofPO";
            this.dtgvListofPO.Size = new System.Drawing.Size(1528, 794);
            this.dtgvListofPO.TabIndex = 2;
            this.dtgvListofPO.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgvListofPO_CellDoubleClick);
            this.dtgvListofPO.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dtgvListofPO_CellFormatting);
            // 
            // searchboxControl1
            // 
            this.searchboxControl1.BackColor = System.Drawing.Color.White;
            this.searchboxControl1.Location = new System.Drawing.Point(907, 15);
            this.searchboxControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchboxControl1.Name = "searchboxControl1";
            this.searchboxControl1.Size = new System.Drawing.Size(650, 50);
            this.searchboxControl1.TabIndex = 1;
            // 
            // ApprovePurchaseOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.dtgvListofPO);
            this.Controls.Add(this.searchboxControl1);
            this.Controls.Add(this.label1);
            this.Name = "ApprovePurchaseOrder";
            this.Size = new System.Drawing.Size(1584, 954);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvListofPO)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private SearchboxControl searchboxControl1;
        private System.Windows.Forms.DataGridView dtgvListofPO;
    }
}
