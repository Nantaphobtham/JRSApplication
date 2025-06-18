namespace JRSApplication.Components.Service
{
    partial class POForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.dtgvMaterial = new System.Windows.Forms.DataGridView();
            this.btnRejected = new System.Windows.Forms.Button();
            this.btnApproved = new System.Windows.Forms.Button();
            this.lblSummary = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvMaterial)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(248, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "รายการวัสดุก่อสร้างและวัสดุไฟฟ้า";
            // 
            // dtgvMaterial
            // 
            this.dtgvMaterial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvMaterial.Location = new System.Drawing.Point(12, 42);
            this.dtgvMaterial.Name = "dtgvMaterial";
            this.dtgvMaterial.Size = new System.Drawing.Size(776, 513);
            this.dtgvMaterial.TabIndex = 1;
            // 
            // btnRejected
            // 
            this.btnRejected.AutoSize = true;
            this.btnRejected.BackColor = System.Drawing.Color.Red;
            this.btnRejected.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRejected.Location = new System.Drawing.Point(434, 644);
            this.btnRejected.Name = "btnRejected";
            this.btnRejected.Size = new System.Drawing.Size(174, 55);
            this.btnRejected.TabIndex = 2;
            this.btnRejected.Text = "ไม่อนุมัติใบสั่งซื้อ";
            this.btnRejected.UseVisualStyleBackColor = false;
            this.btnRejected.Click += new System.EventHandler(this.btnRejected_Click);
            // 
            // btnApproved
            // 
            this.btnApproved.AutoSize = true;
            this.btnApproved.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(255)))), ((int)(((byte)(78)))));
            this.btnApproved.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApproved.Location = new System.Drawing.Point(614, 644);
            this.btnApproved.Name = "btnApproved";
            this.btnApproved.Size = new System.Drawing.Size(174, 55);
            this.btnApproved.TabIndex = 2;
            this.btnApproved.Text = "อนุมัติใบสั่งซื้อ";
            this.btnApproved.UseVisualStyleBackColor = false;
            this.btnApproved.Click += new System.EventHandler(this.btnApproved_Click);
            // 
            // lblSummary
            // 
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSummary.Location = new System.Drawing.Point(25, 576);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(141, 30);
            this.lblSummary.TabIndex = 3;
            this.lblSummary.Text = "คำนวนราคารวม";
            // 
            // POForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 711);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.btnApproved);
            this.Controls.Add(this.btnRejected);
            this.Controls.Add(this.dtgvMaterial);
            this.Controls.Add(this.label1);
            this.Name = "POForm";
            this.Text = "ตรวจสอบรายการวัสดุที่ต้องกาารสั่งซื้อ";
            ((System.ComponentModel.ISupportInitialize)(this.dtgvMaterial)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dtgvMaterial;
        private System.Windows.Forms.Button btnRejected;
        private System.Windows.Forms.Button btnApproved;
        private System.Windows.Forms.Label lblSummary;
    }
}