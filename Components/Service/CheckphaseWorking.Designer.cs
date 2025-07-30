namespace JRSApplication.Components.Service
{
    partial class CheckphaseWorking
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
            this.phaseDetail = new System.Windows.Forms.Panel();
            this.Work = new System.Windows.Forms.Panel();
            this.pnlAction = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.btnReject = new System.Windows.Forms.Button();
            this.btnApproved = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtProjectID = new System.Windows.Forms.TextBox();
            this.txtProjectname = new System.Windows.Forms.TextBox();
            this.txtProjectNumber = new System.Windows.Forms.TextBox();
            this.txtEmployee = new System.Windows.Forms.TextBox();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPhaseNo = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPhaseDetail = new System.Windows.Forms.TextBox();
            this.Header.SuspendLayout();
            this.phaseDetail.SuspendLayout();
            this.pnlAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // Header
            // 
            this.Header.Controls.Add(this.txtProjectNumber);
            this.Header.Controls.Add(this.txtCustomer);
            this.Header.Controls.Add(this.txtEmployee);
            this.Header.Controls.Add(this.txtProjectname);
            this.Header.Controls.Add(this.txtProjectID);
            this.Header.Controls.Add(this.label6);
            this.Header.Controls.Add(this.label5);
            this.Header.Controls.Add(this.label4);
            this.Header.Controls.Add(this.label3);
            this.Header.Controls.Add(this.label2);
            this.Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.Header.Location = new System.Drawing.Point(0, 0);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(787, 133);
            this.Header.TabIndex = 0;
            // 
            // phaseDetail
            // 
            this.phaseDetail.Controls.Add(this.txtPhaseDetail);
            this.phaseDetail.Controls.Add(this.txtPhaseNo);
            this.phaseDetail.Controls.Add(this.label8);
            this.phaseDetail.Controls.Add(this.label7);
            this.phaseDetail.Dock = System.Windows.Forms.DockStyle.Top;
            this.phaseDetail.Location = new System.Drawing.Point(0, 133);
            this.phaseDetail.Name = "phaseDetail";
            this.phaseDetail.Size = new System.Drawing.Size(787, 150);
            this.phaseDetail.TabIndex = 1;
            // 
            // Work
            // 
            this.Work.AutoScroll = true;
            this.Work.Dock = System.Windows.Forms.DockStyle.Top;
            this.Work.Location = new System.Drawing.Point(0, 283);
            this.Work.Name = "Work";
            this.Work.Size = new System.Drawing.Size(787, 800);
            this.Work.TabIndex = 2;
            // 
            // pnlAction
            // 
            this.pnlAction.Controls.Add(this.label1);
            this.pnlAction.Controls.Add(this.txtRemark);
            this.pnlAction.Controls.Add(this.btnReject);
            this.pnlAction.Controls.Add(this.btnApproved);
            this.pnlAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAction.Location = new System.Drawing.Point(0, 1083);
            this.pnlAction.Name = "pnlAction";
            this.pnlAction.Size = new System.Drawing.Size(787, 100);
            this.pnlAction.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "หมายเหตุ";
            // 
            // txtRemark
            // 
            this.txtRemark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemark.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemark.Location = new System.Drawing.Point(12, 50);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(377, 35);
            this.txtRemark.TabIndex = 1;
            // 
            // btnReject
            // 
            this.btnReject.AutoSize = true;
            this.btnReject.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReject.Location = new System.Drawing.Point(395, 48);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(197, 40);
            this.btnReject.TabIndex = 0;
            this.btnReject.Text = "ไม่อนุมัติผลการทำงาน";
            this.btnReject.UseVisualStyleBackColor = true;
            // 
            // btnApproved
            // 
            this.btnApproved.AutoSize = true;
            this.btnApproved.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApproved.Location = new System.Drawing.Point(598, 48);
            this.btnApproved.Name = "btnApproved";
            this.btnApproved.Size = new System.Drawing.Size(177, 40);
            this.btnApproved.TabIndex = 0;
            this.btnApproved.Text = "อนุมัติผลการทำงาน";
            this.btnApproved.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 30);
            this.label2.TabIndex = 0;
            this.label2.Text = "รหัสโครงการ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(20, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 30);
            this.label3.TabIndex = 0;
            this.label3.Text = "ชื่อโครงการ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 30);
            this.label4.TabIndex = 0;
            this.label4.Text = "เลขที่สัญญา";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(407, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 30);
            this.label5.TabIndex = 0;
            this.label5.Text = "ผู้ดูแลโครงการ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(474, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 30);
            this.label6.TabIndex = 0;
            this.label6.Text = "ลูกค้า";
            // 
            // txtProjectID
            // 
            this.txtProjectID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectID.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectID.Location = new System.Drawing.Point(129, 9);
            this.txtProjectID.Name = "txtProjectID";
            this.txtProjectID.ReadOnly = true;
            this.txtProjectID.Size = new System.Drawing.Size(237, 35);
            this.txtProjectID.TabIndex = 1;
            // 
            // txtProjectname
            // 
            this.txtProjectname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectname.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectname.Location = new System.Drawing.Point(129, 50);
            this.txtProjectname.Name = "txtProjectname";
            this.txtProjectname.ReadOnly = true;
            this.txtProjectname.Size = new System.Drawing.Size(237, 35);
            this.txtProjectname.TabIndex = 1;
            // 
            // txtProjectNumber
            // 
            this.txtProjectNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectNumber.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectNumber.Location = new System.Drawing.Point(129, 91);
            this.txtProjectNumber.Name = "txtProjectNumber";
            this.txtProjectNumber.ReadOnly = true;
            this.txtProjectNumber.Size = new System.Drawing.Size(237, 35);
            this.txtProjectNumber.TabIndex = 1;
            // 
            // txtEmployee
            // 
            this.txtEmployee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmployee.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmployee.Location = new System.Drawing.Point(538, 9);
            this.txtEmployee.Name = "txtEmployee";
            this.txtEmployee.ReadOnly = true;
            this.txtEmployee.Size = new System.Drawing.Size(237, 35);
            this.txtEmployee.TabIndex = 1;
            // 
            // txtCustomer
            // 
            this.txtCustomer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomer.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomer.Location = new System.Drawing.Point(538, 50);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.ReadOnly = true;
            this.txtCustomer.Size = new System.Drawing.Size(237, 35);
            this.txtCustomer.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(79, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 30);
            this.label7.TabIndex = 0;
            this.label7.Text = "เฟสที่";
            // 
            // txtPhaseNo
            // 
            this.txtPhaseNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhaseNo.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhaseNo.Location = new System.Drawing.Point(142, 19);
            this.txtPhaseNo.Name = "txtPhaseNo";
            this.txtPhaseNo.ReadOnly = true;
            this.txtPhaseNo.Size = new System.Drawing.Size(127, 35);
            this.txtPhaseNo.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(7, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(129, 59);
            this.label8.TabIndex = 0;
            this.label8.Text = "รายละเอียด\r\nการดำเนินงาน\r\n";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPhaseDetail
            // 
            this.txtPhaseDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhaseDetail.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhaseDetail.Location = new System.Drawing.Point(142, 69);
            this.txtPhaseDetail.Multiline = true;
            this.txtPhaseDetail.Name = "txtPhaseDetail";
            this.txtPhaseDetail.ReadOnly = true;
            this.txtPhaseDetail.Size = new System.Drawing.Size(633, 65);
            this.txtPhaseDetail.TabIndex = 1;
            // 
            // CheckphaseWorking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(804, 1061);
            this.Controls.Add(this.pnlAction);
            this.Controls.Add(this.Work);
            this.Controls.Add(this.phaseDetail);
            this.Controls.Add(this.Header);
            this.Name = "CheckphaseWorking";
            this.Text = "ตรวจสอบเฟสการทำงาน";
            this.Header.ResumeLayout(false);
            this.Header.PerformLayout();
            this.phaseDetail.ResumeLayout(false);
            this.phaseDetail.PerformLayout();
            this.pnlAction.ResumeLayout(false);
            this.pnlAction.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Header;
        private System.Windows.Forms.Panel phaseDetail;
        private System.Windows.Forms.Panel Work;
        private System.Windows.Forms.Panel pnlAction;
        private System.Windows.Forms.Button btnApproved;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProjectNumber;
        private System.Windows.Forms.TextBox txtEmployee;
        private System.Windows.Forms.TextBox txtProjectname;
        private System.Windows.Forms.TextBox txtProjectID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.TextBox txtPhaseNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPhaseDetail;
    }
}