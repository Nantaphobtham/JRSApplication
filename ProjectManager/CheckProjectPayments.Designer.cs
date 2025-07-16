namespace JRSApplication
{
    partial class CheckProjectPayments
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckProjectPayments));
            this.panel1 = new System.Windows.Forms.Panel();
            this.searchboxControl1 = new JRSApplication.SearchboxControl();
            this.dtgvInvoice = new System.Windows.Forms.DataGridView();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.txtProjectManager = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtCustomername = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtProjectname = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtProjectID = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtHeaderSearch = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtIDCard = new System.Windows.Forms.TextBox();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtPaymentMethod = new System.Windows.Forms.TextBox();
            this.txtPaymentDate = new System.Windows.Forms.TextBox();
            this.txtEmpName = new System.Windows.Forms.TextBox();
            this.txtInvoiceNo = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtPhase = new System.Windows.Forms.TextBox();
            this.txtProjectName2 = new System.Windows.Forms.TextBox();
            this.txtContractNumber = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.pdfProofofPayment = new AxAcroPDFLib.AxAcroPDF();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvInvoice)).BeginInit();
            this.panel8.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pdfProofofPayment)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.searchboxControl1);
            this.panel1.Controls.Add(this.dtgvInvoice);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.txtHeaderSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1482, 776);
            this.panel1.TabIndex = 0;
            // 
            // searchboxControl1
            // 
            this.searchboxControl1.BackColor = System.Drawing.Color.White;
            this.searchboxControl1.Location = new System.Drawing.Point(788, 180);
            this.searchboxControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchboxControl1.Name = "searchboxControl1";
            this.searchboxControl1.Size = new System.Drawing.Size(650, 50);
            this.searchboxControl1.TabIndex = 41;
            // 
            // dtgvInvoice
            // 
            this.dtgvInvoice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvInvoice.Location = new System.Drawing.Point(28, 235);
            this.dtgvInvoice.Name = "dtgvInvoice";
            this.dtgvInvoice.Size = new System.Drawing.Size(1410, 327);
            this.dtgvInvoice.TabIndex = 40;
            // 
            // button4
            // 
            this.button4.AutoSize = true;
            this.button4.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(211, 39);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(159, 40);
            this.button4.TabIndex = 38;
            this.button4.Text = "ค้นหาโครงการ";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.btnSearchProject_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 170);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 30);
            this.label1.TabIndex = 4;
            this.label1.Text = "ข้อมูลการชำระเงิน";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.txtProjectManager);
            this.panel8.Controls.Add(this.label19);
            this.panel8.Controls.Add(this.txtCustomername);
            this.panel8.Controls.Add(this.label17);
            this.panel8.Controls.Add(this.txtProjectname);
            this.panel8.Controls.Add(this.label16);
            this.panel8.Controls.Add(this.txtProjectID);
            this.panel8.Controls.Add(this.label18);
            this.panel8.Location = new System.Drawing.Point(19, 90);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1512, 61);
            this.panel8.TabIndex = 8;
            // 
            // txtProjectManager
            // 
            this.txtProjectManager.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtProjectManager.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectManager.Enabled = false;
            this.txtProjectManager.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectManager.Location = new System.Drawing.Point(1205, 19);
            this.txtProjectManager.Name = "txtProjectManager";
            this.txtProjectManager.ReadOnly = true;
            this.txtProjectManager.Size = new System.Drawing.Size(245, 35);
            this.txtProjectManager.TabIndex = 17;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(1072, 19);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(125, 30);
            this.label19.TabIndex = 16;
            this.label19.Text = "ผู้ดูแลโครงการ";
            // 
            // txtCustomername
            // 
            this.txtCustomername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtCustomername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomername.Enabled = false;
            this.txtCustomername.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomername.Location = new System.Drawing.Point(821, 16);
            this.txtCustomername.Name = "txtCustomername";
            this.txtCustomername.ReadOnly = true;
            this.txtCustomername.Size = new System.Drawing.Size(245, 35);
            this.txtCustomername.TabIndex = 15;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(757, 16);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(58, 30);
            this.label17.TabIndex = 14;
            this.label17.Text = "ลูกค้า";
            // 
            // txtProjectname
            // 
            this.txtProjectname.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtProjectname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectname.Enabled = false;
            this.txtProjectname.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectname.Location = new System.Drawing.Point(506, 14);
            this.txtProjectname.Name = "txtProjectname";
            this.txtProjectname.ReadOnly = true;
            this.txtProjectname.Size = new System.Drawing.Size(245, 35);
            this.txtProjectname.TabIndex = 13;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(397, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(103, 30);
            this.label16.TabIndex = 12;
            this.label16.Text = "ชื่อโครงการ";
            // 
            // txtProjectID
            // 
            this.txtProjectID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtProjectID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectID.Enabled = false;
            this.txtProjectID.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectID.Location = new System.Drawing.Point(140, 14);
            this.txtProjectID.Name = "txtProjectID";
            this.txtProjectID.ReadOnly = true;
            this.txtProjectID.Size = new System.Drawing.Size(245, 35);
            this.txtProjectID.TabIndex = 11;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(23, 14);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(111, 30);
            this.label18.TabIndex = 8;
            this.label18.Text = "รหัสโครงการ";
            // 
            // txtHeaderSearch
            // 
            this.txtHeaderSearch.AutoSize = true;
            this.txtHeaderSearch.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHeaderSearch.Location = new System.Drawing.Point(39, 39);
            this.txtHeaderSearch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtHeaderSearch.Name = "txtHeaderSearch";
            this.txtHeaderSearch.Size = new System.Drawing.Size(151, 30);
            this.txtHeaderSearch.TabIndex = 2;
            this.txtHeaderSearch.Text = "ค้นหาโครงการ";
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(6, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(181, 40);
            this.button1.TabIndex = 39;
            this.button1.Text = "ค้นหาข้อมูลชำระเงิน";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnSearchPayment_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Control;
            this.panel4.Controls.Add(this.txtAddress);
            this.panel4.Controls.Add(this.txtIDCard);
            this.panel4.Controls.Add(this.button1);
            this.panel4.Controls.Add(this.txtCustomer);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Location = new System.Drawing.Point(3, 37);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(500, 295);
            this.panel4.TabIndex = 6;
            // 
            // txtAddress
            // 
            this.txtAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.Location = new System.Drawing.Point(192, 167);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(305, 100);
            this.txtAddress.TabIndex = 12;
            // 
            // txtIDCard
            // 
            this.txtIDCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtIDCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIDCard.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIDCard.Location = new System.Drawing.Point(192, 111);
            this.txtIDCard.Name = "txtIDCard";
            this.txtIDCard.Size = new System.Drawing.Size(305, 35);
            this.txtIDCard.TabIndex = 11;
            // 
            // txtCustomer
            // 
            this.txtCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtCustomer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomer.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomer.Location = new System.Drawing.Point(192, 55);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(305, 35);
            this.txtCustomer.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(20, 167);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 30);
            this.label7.TabIndex = 9;
            this.label7.Text = "ที่อยู่";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(20, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(152, 30);
            this.label6.TabIndex = 8;
            this.label6.Text = "เลขบัตรประชาชน";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 30);
            this.label2.TabIndex = 7;
            this.label2.Text = "ลูกค้า";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(187, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 30);
            this.label3.TabIndex = 6;
            this.label3.Text = "ข้อมูลลูกค้า";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.txtPaymentMethod);
            this.panel6.Controls.Add(this.txtPaymentDate);
            this.panel6.Controls.Add(this.txtEmpName);
            this.panel6.Controls.Add(this.txtInvoiceNo);
            this.panel6.Controls.Add(this.label14);
            this.panel6.Controls.Add(this.label13);
            this.panel6.Controls.Add(this.label12);
            this.panel6.Controls.Add(this.label11);
            this.panel6.Controls.Add(this.label5);
            this.panel6.Location = new System.Drawing.Point(1015, 37);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(500, 295);
            this.panel6.TabIndex = 8;
            // 
            // txtPaymentMethod
            // 
            this.txtPaymentMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtPaymentMethod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaymentMethod.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPaymentMethod.Location = new System.Drawing.Point(185, 220);
            this.txtPaymentMethod.Name = "txtPaymentMethod";
            this.txtPaymentMethod.Size = new System.Drawing.Size(305, 35);
            this.txtPaymentMethod.TabIndex = 16;
            // 
            // txtPaymentDate
            // 
            this.txtPaymentDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtPaymentDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaymentDate.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPaymentDate.Location = new System.Drawing.Point(185, 165);
            this.txtPaymentDate.Name = "txtPaymentDate";
            this.txtPaymentDate.Size = new System.Drawing.Size(305, 35);
            this.txtPaymentDate.TabIndex = 15;
            // 
            // txtEmpName
            // 
            this.txtEmpName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtEmpName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmpName.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpName.Location = new System.Drawing.Point(185, 110);
            this.txtEmpName.Name = "txtEmpName";
            this.txtEmpName.Size = new System.Drawing.Size(305, 35);
            this.txtEmpName.TabIndex = 14;
            // 
            // txtInvoiceNo
            // 
            this.txtInvoiceNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtInvoiceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInvoiceNo.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvoiceNo.Location = new System.Drawing.Point(185, 55);
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.Size = new System.Drawing.Size(305, 35);
            this.txtInvoiceNo.TabIndex = 13;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(30, 223);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(99, 30);
            this.label14.TabIndex = 12;
            this.label14.Text = "วิธีชำระเงิน";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(30, 167);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(94, 30);
            this.label13.TabIndex = 11;
            this.label13.Text = "วันที่รับเงิน";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(30, 111);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(141, 30);
            this.label12.TabIndex = 10;
            this.label12.Text = "พนักงานผู้รับเงิน";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(30, 55);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(128, 30);
            this.label11.TabIndex = 9;
            this.label11.Text = "เลขที่ใบแจ้งหนี้";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(156, 9);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(188, 30);
            this.label5.TabIndex = 6;
            this.label5.Text = "ข้อมูลการชำระเงิน";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtPhase);
            this.panel5.Controls.Add(this.txtProjectName2);
            this.panel5.Controls.Add(this.txtContractNumber);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Location = new System.Drawing.Point(509, 37);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(500, 295);
            this.panel5.TabIndex = 7;
            // 
            // txtPhase
            // 
            this.txtPhase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtPhase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhase.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhase.Location = new System.Drawing.Point(180, 165);
            this.txtPhase.Name = "txtPhase";
            this.txtPhase.Size = new System.Drawing.Size(305, 35);
            this.txtPhase.TabIndex = 13;
            // 
            // txtProjectName2
            // 
            this.txtProjectName2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtProjectName2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectName2.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectName2.Location = new System.Drawing.Point(180, 109);
            this.txtProjectName2.Name = "txtProjectName2";
            this.txtProjectName2.Size = new System.Drawing.Size(305, 35);
            this.txtProjectName2.TabIndex = 12;
            // 
            // txtContractNumber
            // 
            this.txtContractNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtContractNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContractNumber.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContractNumber.Location = new System.Drawing.Point(180, 55);
            this.txtContractNumber.Name = "txtContractNumber";
            this.txtContractNumber.Size = new System.Drawing.Size(305, 35);
            this.txtContractNumber.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(25, 164);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 30);
            this.label10.TabIndex = 10;
            this.label10.Text = "เฟสงาน";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(25, 109);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 30);
            this.label9.TabIndex = 9;
            this.label9.Text = "ชื่อโครงการ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(25, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 30);
            this.label8.TabIndex = 8;
            this.label8.Text = "เลขที่สัญญา";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(175, 9);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 30);
            this.label4.TabIndex = 6;
            this.label4.Text = "ข้อมูลโครงการ";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 776);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1482, 276);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label15);
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 1052);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1482, 929);
            this.panel3.TabIndex = 2;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(38, 310);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(212, 30);
            this.label15.TabIndex = 10;
            this.label15.Text = "หลักฐานการชำระเงิน";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
            this.panel7.Controls.Add(this.pdfProofofPayment);
            this.panel7.Location = new System.Drawing.Point(19, 349);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1512, 808);
            this.panel7.TabIndex = 9;
            // 
            // pdfProofofPayment
            // 
            this.pdfProofofPayment.Enabled = true;
            this.pdfProofofPayment.Location = new System.Drawing.Point(93, 28);
            this.pdfProofofPayment.Margin = new System.Windows.Forms.Padding(2);
            this.pdfProofofPayment.Name = "pdfProofofPayment";
            this.pdfProofofPayment.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("pdfProofofPayment.OcxState")));
            this.pdfProofofPayment.Size = new System.Drawing.Size(1326, 763);
            this.pdfProofofPayment.TabIndex = 7;
            // 
            // CheckProjectPayments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "CheckProjectPayments";
            this.Size = new System.Drawing.Size(1482, 954);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvInvoice)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pdfProofofPayment)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label txtHeaderSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPaymentMethod;
        private System.Windows.Forms.TextBox txtPaymentDate;
        private System.Windows.Forms.TextBox txtEmpName;
        private System.Windows.Forms.TextBox txtInvoiceNo;
        private System.Windows.Forms.TextBox txtPhase;
        private System.Windows.Forms.TextBox txtProjectName2;
        private System.Windows.Forms.TextBox txtContractNumber;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtIDCard;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label15;
        private AxAcroPDFLib.AxAcroPDF pdfProofofPayment;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.TextBox txtProjectID;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txtProjectManager;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtCustomername;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtProjectname;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dtgvInvoice;
        private SearchboxControl searchboxControl1;
    }
}
