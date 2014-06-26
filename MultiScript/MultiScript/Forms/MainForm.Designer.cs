namespace MultiScript.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnConnection = new System.Windows.Forms.Button();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtScriptFolder = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnRunScripts = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serversToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearConnectionStringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.btnAddFolder = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnViewFolders = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnAddAllSubFolders = new System.Windows.Forms.Button();
            this.btnRemoveLogFiles = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.serverMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.serverMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnection
            // 
            this.btnConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnection.Location = new System.Drawing.Point(701, 38);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(75, 23);
            this.btnConnection.TabIndex = 3;
            this.btnConnection.Text = "&Connection";
            this.toolTip1.SetToolTip(this.btnConnection, "Select Connection String");
            this.btnConnection.UseVisualStyleBackColor = true;
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(12, 40);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.ReadOnly = true;
            this.txtConnectionString.Size = new System.Drawing.Size(667, 20);
            this.txtConnectionString.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtConnectionString, "Selected Connection String");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Connection String:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Script Folder:";
            // 
            // txtScriptFolder
            // 
            this.txtScriptFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtScriptFolder.Location = new System.Drawing.Point(12, 79);
            this.txtScriptFolder.Name = "txtScriptFolder";
            this.txtScriptFolder.Size = new System.Drawing.Size(667, 20);
            this.txtScriptFolder.TabIndex = 5;
            this.txtScriptFolder.Text = "\\\\file02\\Development\\SQLMIGRATION";
            this.toolTip1.SetToolTip(this.txtScriptFolder, "Selected Directory");
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(701, 77);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 6;
            this.btnBrowse.Text = "&Browse";
            this.toolTip1.SetToolTip(this.btnBrowse, "Select Directory");
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.SelectedPath = "\\\\ppi-file01\\Development\\SQLMIGRATION\\PRODUCTION";
            // 
            // btnRunScripts
            // 
            this.btnRunScripts.Location = new System.Drawing.Point(494, 128);
            this.btnRunScripts.Name = "btnRunScripts";
            this.btnRunScripts.Size = new System.Drawing.Size(75, 23);
            this.btnRunScripts.TabIndex = 7;
            this.btnRunScripts.Text = "&Run Scripts";
            this.toolTip1.SetToolTip(this.btnRunScripts, "Run Selected Folders");
            this.btnRunScripts.UseVisualStyleBackColor = true;
            this.btnRunScripts.Click += new System.EventHandler(this.btnRunScripts_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "txt";
            this.saveFileDialog1.FileName = "Results.txt";
            this.saveFileDialog1.Filter = "Text Files|*.txt|All Files|*.*";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.serversToolStripMenuItem,
            this.clearConnectionStringToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(795, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(137, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.preferencesToolStripMenuItem.Text = "&Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // serversToolStripMenuItem
            // 
            this.serversToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewToolStripMenuItem,
            this.toolStripSeparator1});
            this.serversToolStripMenuItem.Name = "serversToolStripMenuItem";
            this.serversToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.serversToolStripMenuItem.Text = "Servers";
            // 
            // addNewToolStripMenuItem
            // 
            this.addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            this.addNewToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addNewToolStripMenuItem.Text = "Add New";
            this.addNewToolStripMenuItem.Click += new System.EventHandler(this.addNewToolStripMenuItem_Click);
            // 
            // clearConnectionStringToolStripMenuItem
            // 
            this.clearConnectionStringToolStripMenuItem.Name = "clearConnectionStringToolStripMenuItem";
            this.clearConnectionStringToolStripMenuItem.Size = new System.Drawing.Size(148, 20);
            this.clearConnectionStringToolStripMenuItem.Text = "Clear Connection String";
            this.clearConnectionStringToolStripMenuItem.Click += new System.EventHandler(this.clearConnectionStringToolStripMenuItem_Click);
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Location = new System.Drawing.Point(213, 128);
            this.btnAddFolder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(141, 23);
            this.btnAddFolder.TabIndex = 10;
            this.btnAddFolder.Text = "&Add Folder To Collection";
            this.toolTip1.SetToolTip(this.btnAddFolder, "Adds Selected Directory And Connection String Into Folders To Run Collection");
            this.btnAddFolder.UseVisualStyleBackColor = true;
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnViewFolders
            // 
            this.btnViewFolders.Location = new System.Drawing.Point(370, 128);
            this.btnViewFolders.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnViewFolders.Name = "btnViewFolders";
            this.btnViewFolders.Size = new System.Drawing.Size(107, 23);
            this.btnViewFolders.TabIndex = 11;
            this.btnViewFolders.Text = "&View Current Folders";
            this.toolTip1.SetToolTip(this.btnViewFolders, "View/Remove Current Folders Selected To Run");
            this.btnViewFolders.UseVisualStyleBackColor = true;
            this.btnViewFolders.Click += new System.EventHandler(this.btnViewFolders_Click);
            // 
            // btnAddAllSubFolders
            // 
            this.btnAddAllSubFolders.Location = new System.Drawing.Point(95, 128);
            this.btnAddAllSubFolders.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddAllSubFolders.Name = "btnAddAllSubFolders";
            this.btnAddAllSubFolders.Size = new System.Drawing.Size(101, 23);
            this.btnAddAllSubFolders.TabIndex = 12;
            this.btnAddAllSubFolders.Text = "Add All &SubFolders";
            this.toolTip1.SetToolTip(this.btnAddAllSubFolders, "Adds all the sub folders of the current selected folder");
            this.btnAddAllSubFolders.UseVisualStyleBackColor = true;
            this.btnAddAllSubFolders.Click += new System.EventHandler(this.btnAddAllSubFolders_Click);
            // 
            // btnRemoveLogFiles
            // 
            this.btnRemoveLogFiles.Location = new System.Drawing.Point(585, 128);
            this.btnRemoveLogFiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRemoveLogFiles.Name = "btnRemoveLogFiles";
            this.btnRemoveLogFiles.Size = new System.Drawing.Size(107, 23);
            this.btnRemoveLogFiles.TabIndex = 14;
            this.btnRemoveLogFiles.Text = "Remove &Log Files";
            this.toolTip1.SetToolTip(this.btnRemoveLogFiles, "Remove Log Text Files In Selected Directory");
            this.btnRemoveLogFiles.UseVisualStyleBackColor = true;
            this.btnRemoveLogFiles.Click += new System.EventHandler(this.btnRemoveLogFiles_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // serverMenuStrip
            // 
            this.serverMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.serverMenuStrip.Name = "serverMenuStrip";
            this.serverMenuStrip.Size = new System.Drawing.Size(153, 70);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 173);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnRemoveLogFiles);
            this.Controls.Add(this.btnAddAllSubFolders);
            this.Controls.Add(this.btnViewFolders);
            this.Controls.Add(this.btnAddFolder);
            this.Controls.Add(this.btnRunScripts);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtScriptFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.btnConnection);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(803, 200);
            this.MinimumSize = new System.Drawing.Size(803, 200);
            this.Name = "MainForm";
            this.Text = "SQL Multi Script";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.serverMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtScriptFolder;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnRunScripts;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.Button btnAddFolder;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnViewFolders;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnAddAllSubFolders;
        private System.Windows.Forms.Button btnRemoveLogFiles;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serversToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearConnectionStringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip serverMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

