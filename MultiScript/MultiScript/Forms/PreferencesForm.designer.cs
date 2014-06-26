﻿namespace MultiScript.Forms
{
    partial class PreferencesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesForm));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkRemoveSuccessfulLog = new System.Windows.Forms.CheckBox();
            this.btnDefaultFolderLocation = new System.Windows.Forms.Button();
            this.txtDefaultFolderLocation = new System.Windows.Forms.TextBox();
            this.chkRegisterContextMenu = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(7, 6);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(87, 27);
            this.btnOK.TabIndex = 24;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(105, 6);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 27);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Location = new System.Drawing.Point(293, 293);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(201, 41);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 237F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 243F));
            this.tableLayoutPanel1.Controls.Add(this.chkRemoveSuccessfulLog, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDefaultFolderLocation, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtDefaultFolderLocation, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkRegisterContextMenu, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(14, 15);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(479, 277);
            this.tableLayoutPanel1.TabIndex = 28;
            // 
            // chkRemoveSuccessfulLog
            // 
            this.chkRemoveSuccessfulLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRemoveSuccessfulLog.AutoSize = true;
            this.chkRemoveSuccessfulLog.Location = new System.Drawing.Point(3, 4);
            this.chkRemoveSuccessfulLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkRemoveSuccessfulLog.Name = "chkRemoveSuccessfulLog";
            this.chkRemoveSuccessfulLog.Size = new System.Drawing.Size(231, 41);
            this.chkRemoveSuccessfulLog.TabIndex = 4;
            this.chkRemoveSuccessfulLog.Text = "Remove Successful Logs";
            this.chkRemoveSuccessfulLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkRemoveSuccessfulLog.UseVisualStyleBackColor = true;
            // 
            // btnDefaultFolderLocation
            // 
            this.btnDefaultFolderLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDefaultFolderLocation.Location = new System.Drawing.Point(240, 63);
            this.btnDefaultFolderLocation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDefaultFolderLocation.Name = "btnDefaultFolderLocation";
            this.btnDefaultFolderLocation.Size = new System.Drawing.Size(237, 23);
            this.btnDefaultFolderLocation.TabIndex = 7;
            this.btnDefaultFolderLocation.Text = "Change Default Folder Location";
            this.btnDefaultFolderLocation.UseVisualStyleBackColor = true;
            this.btnDefaultFolderLocation.Click += new System.EventHandler(this.btnDefaultFolderLocation_Click);
            // 
            // txtDefaultFolderLocation
            // 
            this.txtDefaultFolderLocation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtDefaultFolderLocation.Location = new System.Drawing.Point(3, 64);
            this.txtDefaultFolderLocation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDefaultFolderLocation.Name = "txtDefaultFolderLocation";
            this.txtDefaultFolderLocation.Size = new System.Drawing.Size(231, 21);
            this.txtDefaultFolderLocation.TabIndex = 6;
            this.txtDefaultFolderLocation.Validating += new System.ComponentModel.CancelEventHandler(this.txtDefaultFolderLocation_Validating);
            // 
            // chkRegisterContextMenu
            // 
            this.chkRegisterContextMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRegisterContextMenu.AutoSize = true;
            this.chkRegisterContextMenu.Location = new System.Drawing.Point(3, 104);
            this.chkRegisterContextMenu.Name = "chkRegisterContextMenu";
            this.chkRegisterContextMenu.Size = new System.Drawing.Size(231, 49);
            this.chkRegisterContextMenu.TabIndex = 8;
            this.chkRegisterContextMenu.Text = "Register Context Menu";
            this.chkRegisterContextMenu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkRegisterContextMenu.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ClientSize = new System.Drawing.Size(507, 348);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreferencesForm";
            this.Padding = new System.Windows.Forms.Padding(10, 11, 10, 11);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.CheckBox chkRemoveSuccessfulLog;
        private System.Windows.Forms.TextBox txtDefaultFolderLocation;
        private System.Windows.Forms.Button btnDefaultFolderLocation;
        private System.Windows.Forms.CheckBox chkRegisterContextMenu;

    }
}
