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
            this.dEVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.smsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sTAGEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.smsToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.pREPRODToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountsToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.smsToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.productionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountsToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.smsToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
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
            this.uATToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mDMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mDMToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mDMToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mDMToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.accountsToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.smsToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.mDMToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnection
            // 
            this.btnConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnection.Location = new System.Drawing.Point(818, 47);
            this.btnConnection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(87, 28);
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
            this.txtConnectionString.Location = new System.Drawing.Point(14, 49);
            this.txtConnectionString.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.ReadOnly = true;
            this.txtConnectionString.Size = new System.Drawing.Size(777, 21);
            this.txtConnectionString.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtConnectionString, "Selected Connection String");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Connection String:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Script Folder:";
            // 
            // txtScriptFolder
            // 
            this.txtScriptFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtScriptFolder.Location = new System.Drawing.Point(14, 97);
            this.txtScriptFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtScriptFolder.Name = "txtScriptFolder";
            this.txtScriptFolder.Size = new System.Drawing.Size(777, 21);
            this.txtScriptFolder.TabIndex = 5;
            this.txtScriptFolder.Text = "\\\\file02\\Development\\SQLMIGRATION";
            this.toolTip1.SetToolTip(this.txtScriptFolder, "Selected Directory");
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(818, 95);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(87, 28);
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
            this.btnRunScripts.Location = new System.Drawing.Point(576, 157);
            this.btnRunScripts.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRunScripts.Name = "btnRunScripts";
            this.btnRunScripts.Size = new System.Drawing.Size(87, 28);
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
            this.menuStrip1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.serversToolStripMenuItem,
            this.clearConnectionStringToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(919, 25);
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
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(40, 21);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(146, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(91, 21);
            this.preferencesToolStripMenuItem.Text = "&Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // serversToolStripMenuItem
            // 
            this.serversToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dEVToolStripMenuItem,
            this.sTAGEToolStripMenuItem,
            this.pREPRODToolStripMenuItem,
            this.productionToolStripMenuItem,
            this.uATToolStripMenuItem});
            this.serversToolStripMenuItem.Name = "serversToolStripMenuItem";
            this.serversToolStripMenuItem.Size = new System.Drawing.Size(64, 21);
            this.serversToolStripMenuItem.Text = "Servers";
            // 
            // dEVToolStripMenuItem
            // 
            this.dEVToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountsToolStripMenuItem,
            this.syncToolStripMenuItem1,
            this.smsToolStripMenuItem1,
            this.mDMToolStripMenuItem});
            this.dEVToolStripMenuItem.Name = "dEVToolStripMenuItem";
            this.dEVToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dEVToolStripMenuItem.Text = "Dev";
            this.dEVToolStripMenuItem.Click += new System.EventHandler(this.dEVToolStripMenuItem_Click);
            // 
            // accountsToolStripMenuItem
            // 
            this.accountsToolStripMenuItem.Name = "accountsToolStripMenuItem";
            this.accountsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.accountsToolStripMenuItem.Text = "Accounts";
            this.accountsToolStripMenuItem.Click += new System.EventHandler(this.devAccountsToolStripMenuItem_Click);
            // 
            // syncToolStripMenuItem1
            // 
            this.syncToolStripMenuItem1.Name = "syncToolStripMenuItem1";
            this.syncToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.syncToolStripMenuItem1.Text = "Sync";
            this.syncToolStripMenuItem1.Click += new System.EventHandler(this.syncToolStripMenuItem1_Click);
            // 
            // smsToolStripMenuItem1
            // 
            this.smsToolStripMenuItem1.Name = "smsToolStripMenuItem1";
            this.smsToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.smsToolStripMenuItem1.Text = "Sms";
            this.smsToolStripMenuItem1.Click += new System.EventHandler(this.smsToolStripMenuItem1_Click);
            // 
            // sTAGEToolStripMenuItem
            // 
            this.sTAGEToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountsToolStripMenuItem1,
            this.syncToolStripMenuItem2,
            this.smsToolStripMenuItem2,
            this.mDMToolStripMenuItem1});
            this.sTAGEToolStripMenuItem.Name = "sTAGEToolStripMenuItem";
            this.sTAGEToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sTAGEToolStripMenuItem.Text = "Stage";
            // 
            // accountsToolStripMenuItem1
            // 
            this.accountsToolStripMenuItem1.Name = "accountsToolStripMenuItem1";
            this.accountsToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.accountsToolStripMenuItem1.Text = "Accounts";
            this.accountsToolStripMenuItem1.Click += new System.EventHandler(this.stageAccountsToolStripMenuItem1_Click);
            // 
            // syncToolStripMenuItem2
            // 
            this.syncToolStripMenuItem2.Name = "syncToolStripMenuItem2";
            this.syncToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.syncToolStripMenuItem2.Text = "Sync";
            this.syncToolStripMenuItem2.Click += new System.EventHandler(this.syncToolStripMenuItem2_Click);
            // 
            // smsToolStripMenuItem2
            // 
            this.smsToolStripMenuItem2.Name = "smsToolStripMenuItem2";
            this.smsToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.smsToolStripMenuItem2.Text = "Sms";
            this.smsToolStripMenuItem2.Click += new System.EventHandler(this.smsToolStripMenuItem2_Click);
            // 
            // pREPRODToolStripMenuItem
            // 
            this.pREPRODToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountsToolStripMenuItem2,
            this.syncToolStripMenuItem3,
            this.smsToolStripMenuItem3,
            this.mDMToolStripMenuItem2});
            this.pREPRODToolStripMenuItem.Name = "pREPRODToolStripMenuItem";
            this.pREPRODToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pREPRODToolStripMenuItem.Text = "PreProd";
            // 
            // accountsToolStripMenuItem2
            // 
            this.accountsToolStripMenuItem2.Name = "accountsToolStripMenuItem2";
            this.accountsToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.accountsToolStripMenuItem2.Text = "Accounts";
            this.accountsToolStripMenuItem2.Click += new System.EventHandler(this.preProdAccountsToolStripMenuItem2Click);
            // 
            // syncToolStripMenuItem3
            // 
            this.syncToolStripMenuItem3.Name = "syncToolStripMenuItem3";
            this.syncToolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
            this.syncToolStripMenuItem3.Text = "Sync";
            this.syncToolStripMenuItem3.Click += new System.EventHandler(this.syncToolStripMenuItem3_Click);
            // 
            // smsToolStripMenuItem3
            // 
            this.smsToolStripMenuItem3.Name = "smsToolStripMenuItem3";
            this.smsToolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
            this.smsToolStripMenuItem3.Text = "Sms";
            this.smsToolStripMenuItem3.Click += new System.EventHandler(this.smsToolStripMenuItem3_Click);
            // 
            // productionToolStripMenuItem
            // 
            this.productionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountsToolStripMenuItem4,
            this.syncToolStripMenuItem4,
            this.smsToolStripMenuItem4,
            this.mDMToolStripMenuItem3});
            this.productionToolStripMenuItem.Name = "productionToolStripMenuItem";
            this.productionToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.productionToolStripMenuItem.Text = "Production";
            // 
            // accountsToolStripMenuItem4
            // 
            this.accountsToolStripMenuItem4.Name = "accountsToolStripMenuItem4";
            this.accountsToolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
            this.accountsToolStripMenuItem4.Text = "Accounts";
            this.accountsToolStripMenuItem4.Click += new System.EventHandler(this.accountsToolStripMenuItem4_Click);
            // 
            // syncToolStripMenuItem4
            // 
            this.syncToolStripMenuItem4.Name = "syncToolStripMenuItem4";
            this.syncToolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
            this.syncToolStripMenuItem4.Text = "Sync";
            this.syncToolStripMenuItem4.Click += new System.EventHandler(this.syncToolStripMenuItem4_Click);
            // 
            // smsToolStripMenuItem4
            // 
            this.smsToolStripMenuItem4.Name = "smsToolStripMenuItem4";
            this.smsToolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
            this.smsToolStripMenuItem4.Text = "Sms";
            this.smsToolStripMenuItem4.Click += new System.EventHandler(this.smsToolStripMenuItem4_Click);
            // 
            // clearConnectionStringToolStripMenuItem
            // 
            this.clearConnectionStringToolStripMenuItem.Name = "clearConnectionStringToolStripMenuItem";
            this.clearConnectionStringToolStripMenuItem.Size = new System.Drawing.Size(162, 21);
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
            this.btnAddFolder.Location = new System.Drawing.Point(248, 157);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(165, 28);
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
            this.btnViewFolders.Location = new System.Drawing.Point(432, 157);
            this.btnViewFolders.Name = "btnViewFolders";
            this.btnViewFolders.Size = new System.Drawing.Size(125, 28);
            this.btnViewFolders.TabIndex = 11;
            this.btnViewFolders.Text = "&View Current Folders";
            this.toolTip1.SetToolTip(this.btnViewFolders, "View/Remove Current Folders Selected To Run");
            this.btnViewFolders.UseVisualStyleBackColor = true;
            this.btnViewFolders.Click += new System.EventHandler(this.btnViewFolders_Click);
            // 
            // btnAddAllSubFolders
            // 
            this.btnAddAllSubFolders.Location = new System.Drawing.Point(111, 157);
            this.btnAddAllSubFolders.Name = "btnAddAllSubFolders";
            this.btnAddAllSubFolders.Size = new System.Drawing.Size(118, 28);
            this.btnAddAllSubFolders.TabIndex = 12;
            this.btnAddAllSubFolders.Text = "Add All &SubFolders";
            this.toolTip1.SetToolTip(this.btnAddAllSubFolders, "Adds all the sub folders of the current selected folder");
            this.btnAddAllSubFolders.UseVisualStyleBackColor = true;
            this.btnAddAllSubFolders.Click += new System.EventHandler(this.btnAddAllSubFolders_Click);
            // 
            // btnRemoveLogFiles
            // 
            this.btnRemoveLogFiles.Location = new System.Drawing.Point(682, 157);
            this.btnRemoveLogFiles.Name = "btnRemoveLogFiles";
            this.btnRemoveLogFiles.Size = new System.Drawing.Size(125, 28);
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
            // uATToolStripMenuItem
            // 
            this.uATToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountsToolStripMenuItem5,
            this.syncToolStripMenuItem5,
            this.smsToolStripMenuItem5,
            this.mDMToolStripMenuItem4});
            this.uATToolStripMenuItem.Name = "uATToolStripMenuItem";
            this.uATToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.uATToolStripMenuItem.Text = "UAT";
            // 
            // mDMToolStripMenuItem
            // 
            this.mDMToolStripMenuItem.Name = "mDMToolStripMenuItem";
            this.mDMToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.mDMToolStripMenuItem.Text = "MDM";
            this.mDMToolStripMenuItem.Click += new System.EventHandler(this.mDMToolStripMenuItem_Click);
            // 
            // mDMToolStripMenuItem1
            // 
            this.mDMToolStripMenuItem1.Name = "mDMToolStripMenuItem1";
            this.mDMToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.mDMToolStripMenuItem1.Text = "MDM";
            this.mDMToolStripMenuItem1.Click += new System.EventHandler(this.mDMToolStripMenuItem1_Click);
            // 
            // mDMToolStripMenuItem2
            // 
            this.mDMToolStripMenuItem2.Name = "mDMToolStripMenuItem2";
            this.mDMToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.mDMToolStripMenuItem2.Text = "MDM";
            this.mDMToolStripMenuItem2.Click += new System.EventHandler(this.mDMToolStripMenuItem2_Click);
            // 
            // mDMToolStripMenuItem3
            // 
            this.mDMToolStripMenuItem3.Name = "mDMToolStripMenuItem3";
            this.mDMToolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
            this.mDMToolStripMenuItem3.Text = "MDM";
            this.mDMToolStripMenuItem3.Click += new System.EventHandler(this.mDMToolStripMenuItem3_Click);
            // 
            // accountsToolStripMenuItem5
            // 
            this.accountsToolStripMenuItem5.Name = "accountsToolStripMenuItem5";
            this.accountsToolStripMenuItem5.Size = new System.Drawing.Size(152, 22);
            this.accountsToolStripMenuItem5.Text = "Accounts";
            this.accountsToolStripMenuItem5.Click += new System.EventHandler(this.accountsToolStripMenuItem5_Click);
            // 
            // syncToolStripMenuItem5
            // 
            this.syncToolStripMenuItem5.Name = "syncToolStripMenuItem5";
            this.syncToolStripMenuItem5.Size = new System.Drawing.Size(152, 22);
            this.syncToolStripMenuItem5.Text = "Sync";
            this.syncToolStripMenuItem5.Click += new System.EventHandler(this.syncToolStripMenuItem5_Click);
            // 
            // smsToolStripMenuItem5
            // 
            this.smsToolStripMenuItem5.Name = "smsToolStripMenuItem5";
            this.smsToolStripMenuItem5.Size = new System.Drawing.Size(152, 22);
            this.smsToolStripMenuItem5.Text = "Sms";
            this.smsToolStripMenuItem5.Click += new System.EventHandler(this.smsToolStripMenuItem5_Click);
            // 
            // mDMToolStripMenuItem4
            // 
            this.mDMToolStripMenuItem4.Name = "mDMToolStripMenuItem4";
            this.mDMToolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
            this.mDMToolStripMenuItem4.Text = "MDM";
            this.mDMToolStripMenuItem4.Click += new System.EventHandler(this.mDMToolStripMenuItem4_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 201);
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
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(935, 240);
            this.MinimumSize = new System.Drawing.Size(935, 240);
            this.Name = "MainForm";
            this.Text = "SQL Multi Script";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
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
        private System.Windows.Forms.ToolStripMenuItem dEVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sTAGEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pREPRODToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem accountsToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem syncToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem smsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem syncToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem smsToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem syncToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem smsToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem clearConnectionStringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountsToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem smsToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem syncToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem mDMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mDMToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mDMToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mDMToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem uATToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountsToolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem syncToolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem smsToolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem mDMToolStripMenuItem4;
    }
}

