namespace SQLEntityClassGenerator.Forms
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
            this.chkDisableCustomProperties = new System.Windows.Forms.CheckBox();
            this.btnDefaultSaveLocation = new System.Windows.Forms.Button();
            this.txtDefaultSaveLocation = new System.Windows.Forms.TextBox();
            this.chkDisableMessageBoxes = new System.Windows.Forms.CheckBox();
            this.txtLibraryLocation = new System.Windows.Forms.TextBox();
            this.chkAutoIncludeFiles = new System.Windows.Forms.CheckBox();
            this.btnLibraryLocation = new System.Windows.Forms.Button();
            this.chkModifyProjectFile = new System.Windows.Forms.CheckBox();
            this.chkDisableToStringPrompt = new System.Windows.Forms.CheckBox();
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
            this.tableLayoutPanel1.Controls.Add(this.chkDisableCustomProperties, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnDefaultSaveLocation, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtDefaultSaveLocation, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkDisableMessageBoxes, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtLibraryLocation, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkAutoIncludeFiles, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnLibraryLocation, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkModifyProjectFile, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkDisableToStringPrompt, 1, 3);
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
            // chkDisableCustomProperties
            // 
            this.chkDisableCustomProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDisableCustomProperties.AutoSize = true;
            this.chkDisableCustomProperties.Location = new System.Drawing.Point(3, 214);
            this.chkDisableCustomProperties.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkDisableCustomProperties.Name = "chkDisableCustomProperties";
            this.chkDisableCustomProperties.Size = new System.Drawing.Size(231, 59);
            this.chkDisableCustomProperties.TabIndex = 8;
            this.chkDisableCustomProperties.Text = "Disable Custom Properties Prompt";
            this.chkDisableCustomProperties.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkDisableCustomProperties.UseVisualStyleBackColor = true;
            // 
            // btnDefaultSaveLocation
            // 
            this.btnDefaultSaveLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDefaultSaveLocation.Location = new System.Drawing.Point(240, 8);
            this.btnDefaultSaveLocation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDefaultSaveLocation.Name = "btnDefaultSaveLocation";
            this.btnDefaultSaveLocation.Size = new System.Drawing.Size(237, 33);
            this.btnDefaultSaveLocation.TabIndex = 1;
            this.btnDefaultSaveLocation.Text = "Change Default Save Location";
            this.btnDefaultSaveLocation.UseVisualStyleBackColor = true;
            this.btnDefaultSaveLocation.Click += new System.EventHandler(this.btnDefaultSaveLocation_Click);
            // 
            // txtDefaultSaveLocation
            // 
            this.txtDefaultSaveLocation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtDefaultSaveLocation.Location = new System.Drawing.Point(3, 14);
            this.txtDefaultSaveLocation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDefaultSaveLocation.Name = "txtDefaultSaveLocation";
            this.txtDefaultSaveLocation.Size = new System.Drawing.Size(212, 21);
            this.txtDefaultSaveLocation.TabIndex = 2;
            this.txtDefaultSaveLocation.Validating += new System.ComponentModel.CancelEventHandler(this.txtDefaultSaveLocation_Validating);
            // 
            // chkDisableMessageBoxes
            // 
            this.chkDisableMessageBoxes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDisableMessageBoxes.AutoSize = true;
            this.chkDisableMessageBoxes.Location = new System.Drawing.Point(3, 105);
            this.chkDisableMessageBoxes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkDisableMessageBoxes.Name = "chkDisableMessageBoxes";
            this.chkDisableMessageBoxes.Size = new System.Drawing.Size(231, 47);
            this.chkDisableMessageBoxes.TabIndex = 0;
            this.chkDisableMessageBoxes.Text = "Disable Message Boxes";
            this.chkDisableMessageBoxes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkDisableMessageBoxes.UseVisualStyleBackColor = true;
            // 
            // txtLibraryLocation
            // 
            this.txtLibraryLocation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLibraryLocation.Location = new System.Drawing.Point(3, 64);
            this.txtLibraryLocation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLibraryLocation.Name = "txtLibraryLocation";
            this.txtLibraryLocation.Size = new System.Drawing.Size(212, 21);
            this.txtLibraryLocation.TabIndex = 3;
            this.txtLibraryLocation.Validating += new System.ComponentModel.CancelEventHandler(this.txtLibraryLocation_Validating);
            // 
            // chkAutoIncludeFiles
            // 
            this.chkAutoIncludeFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAutoIncludeFiles.AutoSize = true;
            this.chkAutoIncludeFiles.Location = new System.Drawing.Point(240, 105);
            this.chkAutoIncludeFiles.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkAutoIncludeFiles.Name = "chkAutoIncludeFiles";
            this.chkAutoIncludeFiles.Size = new System.Drawing.Size(237, 47);
            this.chkAutoIncludeFiles.TabIndex = 4;
            this.chkAutoIncludeFiles.Text = "Auto Include Files In Project";
            this.chkAutoIncludeFiles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkAutoIncludeFiles.UseVisualStyleBackColor = true;
            this.chkAutoIncludeFiles.Validating += new System.ComponentModel.CancelEventHandler(this.chkAutoIncludeFiles_Validating);
            // 
            // btnLibraryLocation
            // 
            this.btnLibraryLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLibraryLocation.Location = new System.Drawing.Point(240, 61);
            this.btnLibraryLocation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLibraryLocation.Name = "btnLibraryLocation";
            this.btnLibraryLocation.Size = new System.Drawing.Size(237, 28);
            this.btnLibraryLocation.TabIndex = 5;
            this.btnLibraryLocation.Text = "Change Library Location";
            this.btnLibraryLocation.UseVisualStyleBackColor = true;
            this.btnLibraryLocation.Click += new System.EventHandler(this.btnLibraryLocation_Click);
            // 
            // chkModifyProjectFile
            // 
            this.chkModifyProjectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkModifyProjectFile.AutoSize = true;
            this.chkModifyProjectFile.Location = new System.Drawing.Point(3, 160);
            this.chkModifyProjectFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkModifyProjectFile.Name = "chkModifyProjectFile";
            this.chkModifyProjectFile.Size = new System.Drawing.Size(231, 46);
            this.chkModifyProjectFile.TabIndex = 6;
            this.chkModifyProjectFile.Text = "Modify Project File";
            this.chkModifyProjectFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkModifyProjectFile.UseVisualStyleBackColor = true;
            // 
            // chkDisableToStringPrompt
            // 
            this.chkDisableToStringPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDisableToStringPrompt.AutoSize = true;
            this.chkDisableToStringPrompt.Location = new System.Drawing.Point(240, 160);
            this.chkDisableToStringPrompt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkDisableToStringPrompt.Name = "chkDisableToStringPrompt";
            this.chkDisableToStringPrompt.Size = new System.Drawing.Size(237, 46);
            this.chkDisableToStringPrompt.TabIndex = 7;
            this.chkDisableToStringPrompt.Text = "Disable ToString Prompt";
            this.chkDisableToStringPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkDisableToStringPrompt.UseVisualStyleBackColor = true;
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
        private System.Windows.Forms.CheckBox chkDisableMessageBoxes;
        private System.Windows.Forms.Button btnDefaultSaveLocation;
        private System.Windows.Forms.TextBox txtDefaultSaveLocation;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TextBox txtLibraryLocation;
        private System.Windows.Forms.CheckBox chkAutoIncludeFiles;
        private System.Windows.Forms.Button btnLibraryLocation;
        private System.Windows.Forms.CheckBox chkModifyProjectFile;
        private System.Windows.Forms.CheckBox chkDisableToStringPrompt;
        private System.Windows.Forms.CheckBox chkDisableCustomProperties;

    }
}
