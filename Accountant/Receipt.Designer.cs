namespace JRSApplication.Accountant
{
    partial class Receipt
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
            this.components = new System.ComponentModel.Container();
            this.Topic1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtgvInvoice = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCusName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCusIDCard = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCusAddress = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPhaseID = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtProName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtContractNo = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtEmpName = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtInvNo = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtReceiptNo = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.dtgvReceiptDetail = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.dtpPaidDate = new System.Windows.Forms.DateTimePicker();
            this.btn1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dtpReceiptDate = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvInvoice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvReceiptDetail)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // Topic1
            // 
            this.Topic1.AutoSize = true;
            this.Topic1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Topic1.Location = new System.Drawing.Point(118, 13);
            this.Topic1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Topic1.Name = "Topic1";
            this.Topic1.Size = new System.Drawing.Size(151, 30);
            this.Topic1.TabIndex = 28;
            this.Topic1.Text = "ค้นหาโครงการ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(118, 85);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 30);
            this.label1.TabIndex = 34;
            this.label1.Text = "ข้อมูลการรับชำระเงินโครงการ";
            // 
            // dtgvInvoice
            // 
            this.dtgvInvoice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvInvoice.Location = new System.Drawing.Point(115, 122);
            this.dtgvInvoice.Name = "dtgvInvoice";
            this.dtgvInvoice.Size = new System.Drawing.Size(1521, 191);
            this.dtgvInvoice.TabIndex = 36;
            this.dtgvInvoice.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgvInvoice_CellContentClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(132, 326);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 30);
            this.label3.TabIndex = 37;
            this.label3.Text = "ข้อมูลลูกค้า";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(691, 326);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 30);
            this.label4.TabIndex = 38;
            this.label4.Text = "ข้อมูลโครงการ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1194, 326);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(179, 30);
            this.label5.TabIndex = 39;
            this.label5.Text = "ข้อมูลการชำระเงิน";
            // 
            // txtCusName
            // 
            this.txtCusName.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCusName.Location = new System.Drawing.Point(253, 373);
            this.txtCusName.Name = "txtCusName";
            this.txtCusName.ReadOnly = true;
            this.txtCusName.Size = new System.Drawing.Size(240, 35);
            this.txtCusName.TabIndex = 49;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(88, 373);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 31);
            this.label9.TabIndex = 50;
            this.label9.Text = "ลูกค้า";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(88, 414);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 31);
            this.label6.TabIndex = 52;
            this.label6.Text = "เลขบัตรประชาชน";
            // 
            // txtCusIDCard
            // 
            this.txtCusIDCard.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCusIDCard.Location = new System.Drawing.Point(253, 414);
            this.txtCusIDCard.Name = "txtCusIDCard";
            this.txtCusIDCard.ReadOnly = true;
            this.txtCusIDCard.Size = new System.Drawing.Size(240, 35);
            this.txtCusIDCard.TabIndex = 51;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(88, 455);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 31);
            this.label8.TabIndex = 54;
            this.label8.Text = "ที่อยู่";
            // 
            // txtCusAddress
            // 
            this.txtCusAddress.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCusAddress.Location = new System.Drawing.Point(253, 455);
            this.txtCusAddress.Multiline = true;
            this.txtCusAddress.Name = "txtCusAddress";
            this.txtCusAddress.ReadOnly = true;
            this.txtCusAddress.Size = new System.Drawing.Size(240, 75);
            this.txtCusAddress.TabIndex = 53;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(570, 455);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 31);
            this.label10.TabIndex = 60;
            this.label10.Text = "งวดงาน";
            // 
            // txtPhaseID
            // 
            this.txtPhaseID.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhaseID.Location = new System.Drawing.Point(717, 455);
            this.txtPhaseID.Name = "txtPhaseID";
            this.txtPhaseID.ReadOnly = true;
            this.txtPhaseID.Size = new System.Drawing.Size(240, 35);
            this.txtPhaseID.TabIndex = 59;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(570, 414);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 31);
            this.label11.TabIndex = 58;
            this.label11.Text = "ชื่อโครงการ";
            // 
            // txtProName
            // 
            this.txtProName.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProName.Location = new System.Drawing.Point(717, 414);
            this.txtProName.Name = "txtProName";
            this.txtProName.ReadOnly = true;
            this.txtProName.Size = new System.Drawing.Size(240, 35);
            this.txtProName.TabIndex = 57;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(570, 373);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(120, 31);
            this.label12.TabIndex = 56;
            this.label12.Text = "เลขที่สัญญา";
            // 
            // txtContractNo
            // 
            this.txtContractNo.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContractNo.Location = new System.Drawing.Point(717, 373);
            this.txtContractNo.Name = "txtContractNo";
            this.txtContractNo.ReadOnly = true;
            this.txtContractNo.Size = new System.Drawing.Size(240, 35);
            this.txtContractNo.TabIndex = 55;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(1068, 451);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(123, 31);
            this.label13.TabIndex = 66;
            this.label13.Text = "วันที่ชำระเงิน";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(1068, 410);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(155, 31);
            this.label14.TabIndex = 64;
            this.label14.Text = "พนักงานผู้รับเงิน";
            // 
            // txtEmpName
            // 
            this.txtEmpName.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpName.Location = new System.Drawing.Point(1228, 410);
            this.txtEmpName.Name = "txtEmpName";
            this.txtEmpName.ReadOnly = true;
            this.txtEmpName.Size = new System.Drawing.Size(240, 35);
            this.txtEmpName.TabIndex = 63;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(1068, 369);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(142, 31);
            this.label15.TabIndex = 62;
            this.label15.Text = "เลขที่ใบแจ้งหนี้";
            // 
            // txtInvNo
            // 
            this.txtInvNo.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvNo.Location = new System.Drawing.Point(1228, 367);
            this.txtInvNo.Name = "txtInvNo";
            this.txtInvNo.ReadOnly = true;
            this.txtInvNo.Size = new System.Drawing.Size(240, 35);
            this.txtInvNo.TabIndex = 61;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(1068, 492);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(107, 31);
            this.label16.TabIndex = 68;
            this.label16.Text = "วิธีชำระเงิน";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(68, 561);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(183, 30);
            this.label17.TabIndex = 69;
            this.label17.Text = "ข้อมูลใบเสร็จรับเงิน";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(67, 606);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(174, 31);
            this.label18.TabIndex = 71;
            this.label18.Text = "เลขที่ใบเสร็จรับเงิน";
            // 
            // txtReceiptNo
            // 
            this.txtReceiptNo.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReceiptNo.Location = new System.Drawing.Point(73, 640);
            this.txtReceiptNo.Name = "txtReceiptNo";
            this.txtReceiptNo.Size = new System.Drawing.Size(240, 35);
            this.txtReceiptNo.TabIndex = 70;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(327, 606);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(52, 31);
            this.label19.TabIndex = 73;
            this.label19.Text = "วันที่";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(67, 690);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(96, 31);
            this.label20.TabIndex = 75;
            this.label20.Text = "หมายเหตุ";
            // 
            // txtReason
            // 
            this.txtReason.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReason.Location = new System.Drawing.Point(73, 724);
            this.txtReason.Multiline = true;
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(500, 93);
            this.txtReason.TabIndex = 74;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(920, 561);
            this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(229, 30);
            this.label21.TabIndex = 76;
            this.label21.Text = "รายละเอียดใบเสร็จรับเงิน";
            // 
            // dtgvReceiptDetail
            // 
            this.dtgvReceiptDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvReceiptDetail.Location = new System.Drawing.Point(737, 606);
            this.dtgvReceiptDetail.Name = "dtgvReceiptDetail";
            this.dtgvReceiptDetail.Size = new System.Drawing.Size(731, 259);
            this.dtgvReceiptDetail.TabIndex = 77;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.button6);
            this.panel4.Controls.Add(this.button5);
            this.panel4.Controls.Add(this.button3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 865);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1636, 103);
            this.panel4.TabIndex = 79;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.button6.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.Location = new System.Drawing.Point(815, 32);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(206, 68);
            this.button6.TabIndex = 47;
            this.button6.Text = "พิมพ์ใบเสร็จรับเงิน";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.btnPrintReceipt_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Lime;
            this.button5.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(1300, 32);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(161, 68);
            this.button5.TabIndex = 46;
            this.button5.Text = "บันทึกข้อมูล";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.button3.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(1109, 32);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(161, 68);
            this.button3.TabIndex = 44;
            this.button3.Text = "แก้ไขข้อมูล";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // dtpPaidDate
            // 
            this.dtpPaidDate.Enabled = false;
            this.dtpPaidDate.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpPaidDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPaidDate.Location = new System.Drawing.Point(1228, 451);
            this.dtpPaidDate.Name = "dtpPaidDate";
            this.dtpPaidDate.Size = new System.Drawing.Size(240, 35);
            this.dtpPaidDate.TabIndex = 81;
            // 
            // btn1
            // 
            this.btn1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold);
            this.btn1.Location = new System.Drawing.Point(1232, 55);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(181, 51);
            this.btn1.TabIndex = 83;
            this.btn1.Text = "ค้นหาโครงการ";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.btnSearchProject_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(1228, 495);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(240, 35);
            this.textBox1.TabIndex = 86;
            // 
            // dtpReceiptDate
            // 
            this.dtpReceiptDate.Enabled = false;
            this.dtpReceiptDate.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpReceiptDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpReceiptDate.Location = new System.Drawing.Point(333, 640);
            this.dtpReceiptDate.Name = "dtpReceiptDate";
            this.dtpReceiptDate.Size = new System.Drawing.Size(240, 35);
            this.dtpReceiptDate.TabIndex = 87;
            // 
            // Receipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.dtpReceiptDate);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.dtpPaidDate);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.dtgvReceiptDetail);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.txtReceiptNo);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtEmpName);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtInvNo);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtPhaseID);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtProName);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtContractNo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtCusAddress);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtCusIDCard);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtCusName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtgvInvoice);
            this.Controls.Add(this.Topic1);
            this.Name = "Receipt";
            this.Size = new System.Drawing.Size(1447, 645);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvInvoice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvReceiptDetail)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Topic1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dtgvInvoice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCusName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCusIDCard;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCusAddress;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtPhaseID;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtProName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtContractNo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtEmpName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtInvNo;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtReceiptNo;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.DataGridView dtgvReceiptDetail;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DateTimePicker dtpPaidDate;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DateTimePicker dtpReceiptDate;
    }
}
