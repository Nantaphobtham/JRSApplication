namespace JRSApplication.Accountant
{
    partial class ConfirmInvoice
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.searchboxControl1 = new JRSApplication.SearchboxControl();
            this.txtDueDate = new System.Windows.Forms.DateTimePicker();
            this.label16 = new System.Windows.Forms.Label();
            this.dtpInvoiceDate = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.txtCustomerAddress = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtCustomerIDCard = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtProjectID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtInvoiceNumber = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.Button();
            this.dtpPaymentDate = new System.Windows.Forms.DateTimePicker();
            this.comboPaymentMethod = new System.Windows.Forms.ComboBox();
            this.dgvInvoiceDetails = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvInvoices = new System.Windows.Forms.DataGridView();
            this.label20 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoiceDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoices)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.searchboxControl1);
            this.panel3.Controls.Add(this.txtDueDate);
            this.panel3.Controls.Add(this.label16);
            this.panel3.Controls.Add(this.dtpInvoiceDate);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.txtCustomerAddress);
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.txtCustomerIDCard);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.txtCustomerName);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.textBox7);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.txtProjectID);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.txtProjectName);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.txtInvoiceNumber);
            this.panel3.Controls.Add(this.button5);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label15);
            this.panel3.Controls.Add(this.txtFilePath);
            this.panel3.Controls.Add(this.dtpPaymentDate);
            this.panel3.Controls.Add(this.comboPaymentMethod);
            this.panel3.Controls.Add(this.dgvInvoiceDetails);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.dgvInvoices);
            this.panel3.Controls.Add(this.label20);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 78);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1556, 876);
            this.panel3.TabIndex = 28;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // searchboxControl1
            // 
            this.searchboxControl1.BackColor = System.Drawing.Color.White;
            this.searchboxControl1.Location = new System.Drawing.Point(897, 15);
            this.searchboxControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchboxControl1.Name = "searchboxControl1";
            this.searchboxControl1.Size = new System.Drawing.Size(650, 50);
            this.searchboxControl1.TabIndex = 68;
            // 
            // txtDueDate
            // 
            this.txtDueDate.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDueDate.Location = new System.Drawing.Point(628, 445);
            this.txtDueDate.Name = "txtDueDate";
            this.txtDueDate.Size = new System.Drawing.Size(223, 35);
            this.txtDueDate.TabIndex = 66;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(372, 411);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(164, 31);
            this.label16.TabIndex = 65;
            this.label16.Text = "วันที่เรียกชำระเงิน";
            // 
            // dtpInvoiceDate
            // 
            this.dtpInvoiceDate.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpInvoiceDate.Location = new System.Drawing.Point(378, 445);
            this.dtpInvoiceDate.Name = "dtpInvoiceDate";
            this.dtpInvoiceDate.Size = new System.Drawing.Size(223, 35);
            this.dtpInvoiceDate.TabIndex = 64;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(23, 749);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(100, 31);
            this.label14.TabIndex = 63;
            this.label14.Text = "ที่อยู่ลูกค้า";
            // 
            // txtCustomerAddress
            // 
            this.txtCustomerAddress.Enabled = false;
            this.txtCustomerAddress.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerAddress.Location = new System.Drawing.Point(29, 783);
            this.txtCustomerAddress.Multiline = true;
            this.txtCustomerAddress.Name = "txtCustomerAddress";
            this.txtCustomerAddress.Size = new System.Drawing.Size(581, 78);
            this.txtCustomerAddress.TabIndex = 62;
            this.txtCustomerAddress.TextChanged += new System.EventHandler(this.txtCustomerAddress_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(384, 671);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(165, 31);
            this.label13.TabIndex = 61;
            this.label13.Text = "เลขบัตรประชาชน";
            // 
            // txtCustomerIDCard
            // 
            this.txtCustomerIDCard.Enabled = false;
            this.txtCustomerIDCard.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerIDCard.Location = new System.Drawing.Point(390, 705);
            this.txtCustomerIDCard.Name = "txtCustomerIDCard";
            this.txtCustomerIDCard.Size = new System.Drawing.Size(358, 35);
            this.txtCustomerIDCard.TabIndex = 60;
            this.txtCustomerIDCard.TextChanged += new System.EventHandler(this.txtCustomerIDCard_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(23, 671);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 31);
            this.label12.TabIndex = 59;
            this.label12.Text = "ชื่อลูกค้า";
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Enabled = false;
            this.txtCustomerName.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerName.Location = new System.Drawing.Point(29, 705);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(338, 35);
            this.txtCustomerName.TabIndex = 58;
            this.txtCustomerName.TextChanged += new System.EventHandler(this.txtCustomerName_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(23, 581);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 31);
            this.label11.TabIndex = 57;
            this.label11.Text = "ระบุงวดงาน";
            // 
            // textBox7
            // 
            this.textBox7.Enabled = false;
            this.textBox7.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox7.Location = new System.Drawing.Point(29, 615);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(146, 35);
            this.textBox7.TabIndex = 56;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(478, 499);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(120, 31);
            this.label10.TabIndex = 55;
            this.label10.Text = "เลขที่สัญญา";
            // 
            // txtProjectID
            // 
            this.txtProjectID.Enabled = false;
            this.txtProjectID.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectID.Location = new System.Drawing.Point(484, 533);
            this.txtProjectID.Name = "txtProjectID";
            this.txtProjectID.Size = new System.Drawing.Size(343, 35);
            this.txtProjectID.TabIndex = 54;
            this.txtProjectID.TextChanged += new System.EventHandler(this.txtProjectID_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(23, 499);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 31);
            this.label8.TabIndex = 53;
            this.label8.Text = "ขื่อโครงการ";
            // 
            // txtProjectName
            // 
            this.txtProjectName.Enabled = false;
            this.txtProjectName.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectName.Location = new System.Drawing.Point(29, 533);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(433, 35);
            this.txtProjectName.TabIndex = 52;
            this.txtProjectName.TextChanged += new System.EventHandler(this.txtProjectName_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(622, 411);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(187, 31);
            this.label6.TabIndex = 51;
            this.label6.Text = "กำหนดชำระเงิน(วัน)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(23, 411);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(142, 31);
            this.label9.TabIndex = 49;
            this.label9.Text = "เลขที่ใบแจ้งหนี้";
            // 
            // txtInvoiceNumber
            // 
            this.txtInvoiceNumber.Enabled = false;
            this.txtInvoiceNumber.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvoiceNumber.Location = new System.Drawing.Point(29, 445);
            this.txtInvoiceNumber.Name = "txtInvoiceNumber";
            this.txtInvoiceNumber.Size = new System.Drawing.Size(321, 35);
            this.txtInvoiceNumber.TabIndex = 48;
            this.txtInvoiceNumber.TextChanged += new System.EventHandler(this.txtInvoiceNumber_TextChanged);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Lime;
            this.button5.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(1186, 801);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(199, 60);
            this.button5.TabIndex = 47;
            this.button5.Text = "ยืนยันการชำระเงิน";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1356, 698);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 31);
            this.label5.TabIndex = 41;
            this.label5.Text = "วันที่ชำระเงิน";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1167, 698);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(192, 31);
            this.label4.TabIndex = 40;
            this.label4.Text = "หลักฐานการชำระเงิน";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(974, 700);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(107, 31);
            this.label15.TabIndex = 39;
            this.label15.Text = "วิธีชำระเงิน";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilePath.Location = new System.Drawing.Point(1173, 732);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(183, 40);
            this.txtFilePath.TabIndex = 35;
            this.txtFilePath.Text = "เลือกไฟล์";
            this.txtFilePath.UseVisualStyleBackColor = true;
            this.txtFilePath.Click += new System.EventHandler(this.btnChooseFile_Click);
            // 
            // dtpPaymentDate
            // 
            this.dtpPaymentDate.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpPaymentDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPaymentDate.Location = new System.Drawing.Point(1362, 733);
            this.dtpPaymentDate.Name = "dtpPaymentDate";
            this.dtpPaymentDate.Size = new System.Drawing.Size(183, 35);
            this.dtpPaymentDate.TabIndex = 33;
            // 
            // comboPaymentMethod
            // 
            this.comboPaymentMethod.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboPaymentMethod.FormattingEnabled = true;
            this.comboPaymentMethod.Location = new System.Drawing.Point(980, 734);
            this.comboPaymentMethod.Name = "comboPaymentMethod";
            this.comboPaymentMethod.Size = new System.Drawing.Size(183, 38);
            this.comboPaymentMethod.TabIndex = 32;
            // 
            // dgvInvoiceDetails
            // 
            this.dgvInvoiceDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInvoiceDetails.Location = new System.Drawing.Point(974, 393);
            this.dgvInvoiceDetails.Name = "dgvInvoiceDetails";
            this.dgvInvoiceDetails.Size = new System.Drawing.Size(573, 291);
            this.dgvInvoiceDetails.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1154, 360);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(210, 30);
            this.label3.TabIndex = 17;
            this.label3.Text = "ตารางการเรียกรับเงิน";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 360);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(259, 30);
            this.label1.TabIndex = 16;
            this.label1.Text = "ปรับปรุงข้อมูลการชำระเงิน";
            // 
            // dgvInvoices
            // 
            this.dgvInvoices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInvoices.Location = new System.Drawing.Point(20, 81);
            this.dgvInvoices.Name = "dgvInvoices";
            this.dgvInvoices.Size = new System.Drawing.Size(1527, 263);
            this.dgvInvoices.TabIndex = 15;
            this.dgvInvoices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInvoices_CellContentClick);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(15, 24);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(310, 30);
            this.label20.TabIndex = 13;
            this.label20.Text = "รายการรับชำระเงินตามโครงการ";
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(226)))), ((int)(((byte)(249)))));
            this.button1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(20, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(195, 51);
            this.button1.TabIndex = 67;
            this.button1.Text = "ค้นหาโครงการ";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnSearchProject_Click);
            // 
            // ConfirmInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.button1);
            this.Name = "ConfirmInvoice";
            this.Size = new System.Drawing.Size(1556, 954);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoiceDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoices)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgvInvoices;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.DataGridView dgvInvoiceDetails;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboPaymentMethod;
        private System.Windows.Forms.Button txtFilePath;
        private System.Windows.Forms.DateTimePicker dtpPaymentDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DateTimePicker dtpInvoiceDate;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtCustomerAddress;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtCustomerIDCard;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtCustomerName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtProjectID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtInvoiceNumber;
        private System.Windows.Forms.DateTimePicker txtDueDate;
        private System.Windows.Forms.Button button1;
        private SearchboxControl searchboxControl1;
    }
}
