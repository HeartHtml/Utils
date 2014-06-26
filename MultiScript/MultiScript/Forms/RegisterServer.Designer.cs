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
            this.components = new System.ComponentModel.Container();
            this.lblServerName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.lblConnectionStringName = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.ltConnectionStrings = new System.Windows.Forms.ListBox();
            this.connectionStringsMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.btnSaveConnectionString = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ConnectionStringErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.ServerErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.connectionStringsMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionStringErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerErrorProvider)).BeginInit();
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            this.tableLayoutPanel1.Controls.Add(this.txtServerName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblServerName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblConnectionStringName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtDisplayName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDatabase, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ltConnectionStrings, 1, 3);
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
            this.txtServerName.Size = new System.Drawing.Size(351, 20);
            this.txtServerName.TabIndex = 1;
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
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(79, 28);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(351, 20);
            this.txtDisplayName.TabIndex = 2;
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(3, 92);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(64, 39);
            this.lblDatabase.TabIndex = 3;
            this.lblDatabase.Text = "Saved Connection Strings:";
            // 
            // ltConnectionStrings
            // 
            this.ltConnectionStrings.ContextMenuStrip = this.connectionStringsMenuStrip;
            this.ltConnectionStrings.FormattingEnabled = true;
            this.ltConnectionStrings.Location = new System.Drawing.Point(79, 95);
            this.ltConnectionStrings.Name = "ltConnectionStrings";
            this.ltConnectionStrings.Size = new System.Drawing.Size(351, 69);
            this.ltConnectionStrings.TabIndex = 0;
            this.ltConnectionStrings.SelectedIndexChanged += new System.EventHandler(this.ltConnectionStrings_SelectedIndexChanged);
            // 
            // connectionStringsMenuStrip
            // 
            this.connectionStringsMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.connectionStringsMenuStrip.Name = "connectionStringsMenuStrip";
            this.connectionStringsMenuStrip.Size = new System.Drawing.Size(153, 48);
            this.connectionStringsMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.connectionStringsMenuStrip_Opening);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
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
            this.txtConnectionString.Size = new System.Drawing.Size(351, 20);
            this.txtConnectionString.TabIndex = 3;
            // 
            // btnSaveConnectionString
            // 
            this.btnSaveConnectionString.Location = new System.Drawing.Point(436, 28);
            this.btnSaveConnectionString.Name = "btnSaveConnectionString";
            this.btnSaveConnectionString.Size = new System.Drawing.Size(143, 23);
            this.btnSaveConnectionString.TabIndex = 4;
            this.btnSaveConnectionString.Text = "Save Conn String";
            this.btnSaveConnectionString.UseVisualStyleBackColor = true;
            this.btnSaveConnectionString.Click += new System.EventHandler(this.btnSaveConnectionString_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(369, 196);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(455, 196);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ConnectionStringErrorProvider
            // 
            this.ConnectionStringErrorProvider.ContainerControl = this;
            // 
            // ServerErrorProvider
            // 
            this.ServerErrorProvider.ContainerControl = this;
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
            this.connectionStringsMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionStringErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.ListBox ltConnectionStrings;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.Label lblConnectionStringName;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Button btnSaveConnectionString;
        private System.Windows.Forms.ErrorProvider ConnectionStringErrorProvider;
        private System.Windows.Forms.ErrorProvider ServerErrorProvider;
        private System.Windows.Forms.ContextMenuStrip connectionStringsMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;

    }
}