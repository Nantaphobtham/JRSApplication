namespace JRSApplication
{
    partial class SearchboxControl
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
            this.lblSearchBy = new System.Windows.Forms.Label();
            this.lblSearchterm = new System.Windows.Forms.Label();
            this.cmbSearchBy = new System.Windows.Forms.ComboBox();
            this.txtSearchKeyword = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSearchBy
            // 
            this.lblSearchBy.AutoSize = true;
            this.lblSearchBy.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchBy.Location = new System.Drawing.Point(5, 10);
            this.lblSearchBy.Name = "lblSearchBy";
            this.lblSearchBy.Size = new System.Drawing.Size(123, 38);
            this.lblSearchBy.TabIndex = 0;
            this.lblSearchBy.Text = "ค้นหาจาก";
            // 
            // lblSearchterm
            // 
            this.lblSearchterm.AutoSize = true;
            this.lblSearchterm.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblSearchterm.Location = new System.Drawing.Point(318, 13);
            this.lblSearchterm.Name = "lblSearchterm";
            this.lblSearchterm.Size = new System.Drawing.Size(104, 32);
            this.lblSearchterm.TabIndex = 1;
            this.lblSearchterm.Text = "คำค้นหา";
            // 
            // cmbSearchBy
            // 
            this.cmbSearchBy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbSearchBy.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearchBy.FormattingEnabled = true;
            this.cmbSearchBy.Location = new System.Drawing.Point(134, 7);
            this.cmbSearchBy.Name = "cmbSearchBy";
            this.cmbSearchBy.Size = new System.Drawing.Size(178, 45);
            this.cmbSearchBy.TabIndex = 2;
            // 
            // txtSearchKeyword
            // 
            this.txtSearchKeyword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchKeyword.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchKeyword.Location = new System.Drawing.Point(428, 8);
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new System.Drawing.Size(357, 43);
            this.txtSearchKeyword.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Image = global::JRSApplication.Properties.Resources.search;
            this.btnSearch.Location = new System.Drawing.Point(793, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(61, 43);
            this.btnSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnSearch.TabIndex = 4;
            this.btnSearch.TabStop = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // SearchboxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearchKeyword);
            this.Controls.Add(this.cmbSearchBy);
            this.Controls.Add(this.lblSearchterm);
            this.Controls.Add(this.lblSearchBy);
            this.Name = "SearchboxControl";
            this.Size = new System.Drawing.Size(866, 61);
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSearchBy;
        private System.Windows.Forms.Label lblSearchterm;
        private System.Windows.Forms.ComboBox cmbSearchBy;
        private System.Windows.Forms.TextBox txtSearchKeyword;
        private System.Windows.Forms.PictureBox btnSearch;
    }
}
