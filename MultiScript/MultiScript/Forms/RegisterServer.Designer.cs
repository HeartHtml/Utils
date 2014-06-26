namespace MultiScript.Forms
{
    partial class RegisterServer
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
            this.lblServerName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.ltDatabases = new System.Windows.Forms.ListBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.lblConnectionStringName = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.btnSaveConnectionString = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(3, 0);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(41, 25);
            this.lblServerName.TabIndex = 0;
            this.lblServerName.Text = "Server Name:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.56757F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 82.43243F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 152F));
            this.tableLayoutPanel1.Controls.Add(this.txtServerName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblServerName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblConnectionStringName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDatabase, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ltDatabases, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblConnectionString, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtConnectionString, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveConnectionString, 2, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0885F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 69.91151F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(590, 174);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(79, 3);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(355, 20);
            this.txtServerName.TabIndex = 1;
            // 
            // ltDatabases
            // 
            this.ltDatabases.FormattingEnabled = true;
            this.ltDatabases.Location = new System.Drawing.Point(79, 95);
            this.ltDatabases.Name = "ltDatabases";
            this.ltDatabases.Size = new System.Drawing.Size(355, 69);
            this.ltDatabases.TabIndex = 2;
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(3, 92);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(61, 26);
            this.lblDatabase.TabIndex = 3;
            this.lblDatabase.Text = "Saved Databases:";
            // 
            // lblConnectionStringName
            // 
            this.lblConnectionStringName.AutoSize = true;
            this.lblConnectionStringName.Location = new System.Drawing.Point(3, 25);
            this.lblConnectionStringName.Name = "lblConnectionStringName";
            this.lblConnectionStringName.Size = new System.Drawing.Size(68, 26);
            this.lblConnectionStringName.TabIndex = 5;
            this.lblConnectionStringName.Text = "Connection String Name:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(369, 196);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(455, 196);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(79, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(355, 20);
            this.textBox1.TabIndex = 6;
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(3, 57);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(64, 26);
            this.lblConnectionString.TabIndex = 7;
            this.lblConnectionString.Text = "Connection String:";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(79, 60);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(355, 20);
            this.txtConnectionString.TabIndex = 8;
            // 
            // btnSaveConnectionString
            // 
            this.btnSaveConnectionString.Location = new System.Drawing.Point(440, 28);
            this.btnSaveConnectionString.Name = "btnSaveConnectionString";
            this.btnSaveConnectionString.Size = new System.Drawing.Size(111, 23);
            this.btnSaveConnectionString.TabIndex = 9;
            this.btnSaveConnectionString.Text = "Save Conn String";
            this.btnSaveConnectionString.UseVisualStyleBackColor = true;
            // 
            // RegisterServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 229);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisterServer";
            this.Text = "Register Server";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.ListBox ltDatabases;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.Label lblConnectionStringName;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Button btnSaveConnectionString;

    }
}