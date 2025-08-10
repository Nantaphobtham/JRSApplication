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
            this.btnSave = new System.Windows.Forms.Button();
            this.lblSummary = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPONumber = new System.Windows.Forms.TextBox();
            this.txtProjectNumber = new System.Windows.Forms.TextBox();
            this.txtEmpName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEmpApprove = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDetail = new System.Windows.Forms.TextBox();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtApproveDate = new System.Windows.Forms.TextBox();
            this.chkApproved = new System.Windows.Forms.CheckBox();
            this.chkRejected = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(357, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "ใบสั่งซื้อ";
            // 
            // dtgvMaterial
            // 
            this.dtgvMaterial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvMaterial.Location = new System.Drawing.Point(12, 255);
            this.dtgvMaterial.Name = "dtgvMaterial";
            this.dtgvMaterial.Size = new System.Drawing.Size(776, 484);
            this.dtgvMaterial.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(255)))), ((int)(((byte)(78)))));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(614, 845);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(174, 55);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "บันทึก";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblSummary
            // 
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSummary.Location = new System.Drawing.Point(537, 752);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(141, 30);
            this.lblSummary.TabIndex = 3;
            this.lblSummary.Text = "คำนวนราคารวม";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(435, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 30);
            this.label2.TabIndex = 4;
            this.label2.Text = "เลขที่ใบสั่งซื้อ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(431, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 30);
            this.label3.TabIndex = 4;
            this.label3.Text = "เลขที่โครงการ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(435, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 30);
            this.label4.TabIndex = 4;
            this.label4.Text = "ผู้ออกใบสั่งซื้อ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtPONumber
            // 
            this.txtPONumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPONumber.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPONumber.Location = new System.Drawing.Point(562, 50);
            this.txtPONumber.Name = "txtPONumber";
            this.txtPONumber.ReadOnly = true;
            this.txtPONumber.Size = new System.Drawing.Size(226, 35);
            this.txtPONumber.TabIndex = 5;
            // 
            // txtProjectNumber
            // 
            this.txtProjectNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectNumber.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectNumber.Location = new System.Drawing.Point(562, 91);
            this.txtProjectNumber.Name = "txtProjectNumber";
            this.txtProjectNumber.ReadOnly = true;
            this.txtProjectNumber.Size = new System.Drawing.Size(226, 35);
            this.txtProjectNumber.TabIndex = 5;
            // 
            // txtEmpName
            // 
            this.txtEmpName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmpName.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpName.Location = new System.Drawing.Point(562, 132);
            this.txtEmpName.Name = "txtEmpName";
            this.txtEmpName.ReadOnly = true;
            this.txtEmpName.Size = new System.Drawing.Size(226, 35);
            this.txtEmpName.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(419, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 30);
            this.label5.TabIndex = 4;
            this.label5.Text = "ผู้อนุมัติใบสั่งซื้อ";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtEmpApprove
            // 
            this.txtEmpApprove.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmpApprove.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpApprove.Location = new System.Drawing.Point(562, 173);
            this.txtEmpApprove.Name = "txtEmpApprove";
            this.txtEmpApprove.ReadOnly = true;
            this.txtEmpApprove.Size = new System.Drawing.Size(226, 35);
            this.txtEmpApprove.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 216);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(161, 30);
            this.label6.TabIndex = 4;
            this.label6.Text = "รายละเอียดใบสั่งซื้อ";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtDetail
            // 
            this.txtDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetail.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetail.Location = new System.Drawing.Point(179, 214);
            this.txtDetail.Name = "txtDetail";
            this.txtDetail.ReadOnly = true;
            this.txtDetail.Size = new System.Drawing.Size(609, 35);
            this.txtDetail.TabIndex = 5;
            // 
            // txtRemark
            // 
            this.txtRemark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemark.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemark.Location = new System.Drawing.Point(17, 775);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(373, 146);
            this.txtRemark.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 742);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 30);
            this.label7.TabIndex = 4;
            this.label7.Text = "หมายเหตุ";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::JRSApplication.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 52);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(217, 74);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(25, 134);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 30);
            this.label8.TabIndex = 4;
            this.label8.Text = "วันที่ออก";
            // 
            // txtDate
            // 
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDate.Location = new System.Drawing.Point(112, 132);
            this.txtDate.Name = "txtDate";
            this.txtDate.ReadOnly = true;
            this.txtDate.Size = new System.Drawing.Size(226, 35);
            this.txtDate.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(12, 175);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 30);
            this.label9.TabIndex = 4;
            this.label9.Text = "วันที่อนุมัติ";
            // 
            // txtApproveDate
            // 
            this.txtApproveDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtApproveDate.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApproveDate.Location = new System.Drawing.Point(112, 173);
            this.txtApproveDate.Name = "txtApproveDate";
            this.txtApproveDate.ReadOnly = true;
            this.txtApproveDate.Size = new System.Drawing.Size(226, 35);
            this.txtApproveDate.TabIndex = 5;
            // 
            // chkApproved
            // 
            this.chkApproved.AutoSize = true;
            this.chkApproved.Checked = true;
            this.chkApproved.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkApproved.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkApproved.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkApproved.Location = new System.Drawing.Point(413, 836);
            this.chkApproved.Name = "chkApproved";
            this.chkApproved.Size = new System.Drawing.Size(165, 35);
            this.chkApproved.TabIndex = 7;
            this.chkApproved.Text = "อนุมัติใบสั่งซื้อ";
            this.chkApproved.UseVisualStyleBackColor = true;
            // 
            // chkRejected
            // 
            this.chkRejected.AutoSize = true;
            this.chkRejected.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkRejected.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRejected.Location = new System.Drawing.Point(413, 876);
            this.chkRejected.Name = "chkRejected";
            this.chkRejected.Size = new System.Drawing.Size(189, 35);
            this.chkRejected.TabIndex = 7;
            this.chkRejected.Text = "ไม่อนุมัติใบสั่งซื้อ";
            this.chkRejected.UseVisualStyleBackColor = true;
            // 
            // POForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 933);
            this.Controls.Add(this.chkRejected);
            this.Controls.Add(this.chkApproved);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.txtDetail);
            this.Controls.Add(this.txtEmpApprove);
            this.Controls.Add(this.txtEmpName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtProjectNumber);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtApproveDate);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.txtPONumber);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dtgvMaterial);
            this.Controls.Add(this.label1);
            this.Name = "POForm";
            this.Text = "ตรวจสอบรายการวัสดุที่ต้องกาารสั่งซื้อ";
            ((System.ComponentModel.ISupportInitialize)(this.dtgvMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dtgvMaterial;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPONumber;
        private System.Windows.Forms.TextBox txtProjectNumber;
        private System.Windows.Forms.TextBox txtEmpName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEmpApprove;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDetail;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtApproveDate;
        private System.Windows.Forms.CheckBox chkApproved;
        private System.Windows.Forms.CheckBox chkRejected;
    }
}