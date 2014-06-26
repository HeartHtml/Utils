using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ADODB;
using JS.Entities.ExtensionMethods;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using MSDASC;
using MultiScriptLib;
using Resources = MultiScript.Properties.Resources;
using StoredProcedure = MultiScriptLib.StoredProcedure;

namespace MultiScript.Forms
{
    public partial class MainForm : Form
    {
        #region Preferences Properties
        private static Preferences FormPreferences
        {
            get
            {
                return new Preferences
                    {
                        RemoveSuccessfulLogs = RemoveSuccessfulLogs,
                        DefaultFolderLocation = DefaultFolderLocation,
                        DefaultEnvironment = DefaultEnvironment,
                        RegisterContextMenu = RegisterContextMenu,
                        AutoAddFolders = AutoAddFolders
                    };
            }
        }

        private static bool RemoveSuccessfulLogs
        {
            get { return Properties.Settings.Default.RemoveSuccessfulLogs; }
            set
            {
                Properties.Settings.Default.RemoveSuccessfulLogs = value;
                Properties.Settings.Default.Save();
            }
        }

        private static string DefaultFolderLocation
        {
            get { return Properties.Settings.Default.DefaultFolderLocation; }
            set
            {
                Properties.Settings.Default.DefaultFolderLocation = value;
                Properties.Settings.Default.Save();
            }
        }

        private static EnumEnvironments DefaultEnvironment
        {
            get { return Properties.Settings.Default.DefaultEnvironment; }
            set
            {
                Properties.Settings.Default.DefaultEnvironment = value;
                Properties.Settings.Default.Save();
            }
        }

        private static bool RegisterContextMenu
        {
            get { return Properties.Settings.Default.RegisterContextMenu; }
            set
            {
                Properties.Settings.Default.RegisterContextMenu = value;
                Properties.Settings.Default.Save();
            }
        }

        private static bool AutoAddFolders
        {
            get { return Properties.Settings.Default.AutoAddFolders; }
            set
            {
                Properties.Settings.Default.AutoAddFolders = value;
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        private string CurrentSaveAsFileName { get; set; }

        public MainForm()
        {
            InitializeComponent();
            SetFolder(DefaultFolderLocation);
            CheckFoldersReadyToRun();
            StartPosition = FormStartPosition.CenterScreen;
            SetContextMenu();
        }

        #region Helper Methods
        public void SetFolder(string folderLocation)
        {
            txtScriptFolder.Text = folderLocation.TrimSafely();

            if (AutoAddFolders)
            {
                AddSubFolders();
            }
        }

        private void ShowConnectionDialog()
        {
            object connection = new Connection();
            DataLinksClass linkClass = new DataLinksClass();

            if (linkClass.PromptEdit(ref connection))
            {
                Connection connString = (Connection)connection;
                DbConnectionStringBuilder builder
                    = new DbConnectionStringBuilder
                        {
                            ConnectionString = connString.ConnectionString.TrimSafely()
                        };
                builder.Remove("Provider");
                txtConnectionString.Text = builder.ConnectionString;
            }
        }

        public void RunMultiScriptMultiFolder()
        {
            FoldersToRun.ClearOutAllErrorsOrWarnings();

            StoredProcedures.ClearProcedures();

            ProgressReporter progressReporter
                = new ProgressReporter
                    {
                        TotalFolders = FoldersToRun.SelectedFolders.Count,
                        CurrentFolder = 0,
                        TotalScriptsInFolder = 0,
                        CurrentScriptInFolder = 0
                    };

            foreach (FolderToRun folderToRun in FoldersToRun.SelectedFolders)
            {
                progressReporter.ClearFolder();

                progressReporter.IncrementFolderCount();

                backgroundWorker1.ReportProgress(0, progressReporter);

                RunMultiScript(folderToRun, progressReporter);

                SaveFile(folderToRun);
            }

            if (FoldersToRun.NewArticlesForBatch.SafeAny())
            {
                FileInfo info = new FileInfo(txtScriptFolder.Text);

                CurrentSaveAsFileName = Path.Combine(txtScriptFolder.Text.TrimSafely(),
                    string.Format(Resources.NewArticlesLogFileName,
                        info.Name,
                        DateTime.Now.ToString(Resources.DateTimeDefaultFormat)));

                SaveFile(FoldersToRun.GetNewArticlesString());

                CurrentSaveAsFileName = string.Empty;
            }
        }

        public void RunMultiScript(FolderToRun folderToRun, ProgressReporter progressReporter)
        {
            List<string> procedureNames = new List<string>();

            List<string> fileEntries = Directory.GetFiles(folderToRun.SelectedFolder, Resources.SQLFileExtension).ToList();

            fileEntries.Sort(string.CompareOrdinal);

            folderToRun.ClearProcedureLists();

            progressReporter.TotalScriptsInFolder = fileEntries.Count();

            folderToRun.Results += string.Format(Resources.CurrentConnectionString,
                                                 folderToRun.ConnectionString,
                                                 Environment.NewLine);

            foreach (string fileName in fileEntries)
            {
                progressReporter.IncrementFolderScriptCount();

                backgroundWorker1.ReportProgress(0, progressReporter);

                try
                {
                    string currentDatabase = string.Empty;

                    FileInfo currentFile = new FileInfo(fileName);
                    string currentScriptContents = currentFile.OpenText().ReadToEnd();

                    Server server = new Server(new ServerConnection(new SqlConnection(folderToRun.ConnectionString)));

                    folderToRun.Results += string.Format(Resources.ExecutingScript, currentFile, Environment.NewLine);

                    string[] newLines = currentScriptContents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string newLine in newLines.Where(n => !n.IsNullOrWhiteSpace()))
                    {
                        if (newLine.StartsWith("USE", StringComparison.CurrentCultureIgnoreCase))
                        {
                            currentDatabase = newLine.Substring(newLine.IndexOf("USE", StringComparison.CurrentCultureIgnoreCase));

                            if (!currentDatabase.IsNullOrWhiteSpace())
                            {
                                currentDatabase = currentDatabase.RemoveStrings("USE", "[", "]").TrimSafely();
                            }
                        }

                        string[] articleTypes =
                        {
                            "PROCEDURE",
                            "PROC",
                            "VIEW",
                            "TABLE",
                            "INDEX",
                            "TRIGGER",
                            "CONSTRAINT"
                        };

                        string[] sqlObjectCommands
                            =
                        {
                            "CREATE",
                            "ALTER",
                            "PROCEDURE",
                            "PROC",
                            "VIEW",
                            "TABLE",
                            "INDEX",
                            "TRIGGER",
                        };

                        string[] procedureStrings
                            =
                        {
                            "CREATE PROCEDURE",
                            "ALTER PROCEDURE",
                            "CREATE PROC",
                            "ALTER PROC",
                        };

                        bool isNewItem = newLine.Contains("CREATE");

                        string articleType = string.Empty;

                        string itemName = string.Empty;

                        if (isNewItem)
                        {
                            StringBuilder articleTypeBuilder = new StringBuilder();

                            foreach (string type in articleTypes)
                            {
                                if (newLine.Contains(type))
                                {
                                    articleTypeBuilder.Append(type + "/");
                                }
                            }

                            articleType = articleTypeBuilder.ToString();

                            if (!articleType.IsNullOrWhiteSpace())
                            {
                                articleType = articleType.RemoveLastInstanceOfWord("/");
                            }
                        }

                        string[] words = newLine.Split(' ');

                        foreach (string word in words.Where(w => !w.ContainsAny(sqlObjectCommands)))
                        {
                            itemName = word.RemoveStrings("[", "]");

                            if (itemName.Contains("("))
                            {
                                if (itemName.IndexOf("(", StringComparison.Ordinal) - 1 >= 0)
                                {
                                    itemName =
                                        itemName.SafeSubstring(itemName.IndexOf("(", StringComparison.Ordinal) - 1);
                                }
                            }

                            itemName = itemName.TrimSafely();
                            break;
                        }

                        if (newLine.ContainsAny(procedureStrings))
                        {
                            bool procedureIsDuplicated
                                = StoredProcedures.AddProcedure(folderToRun.ServerName,
                                                                currentDatabase.IsNullOrWhiteSpace()
                                                                    ? folderToRun.DataBaseName
                                                                    : currentDatabase,
                                                                itemName);

                            if (procedureIsDuplicated)
                            {
                                string warningText = string.Format(Resources.StoredProcChangedMultipleTimes, itemName);

                                folderToRun.Results += string.Format("{0}{1}", warningText, Environment.NewLine);

                                folderToRun.DupedProcedures.Add(new StoredProcedure(folderToRun.ServerName,
                                                                                    folderToRun.DataBaseName,
                                                                                    itemName));
                            }
                        }

                        if (isNewItem)
                        {
                            NewArticle article = new NewArticle { ArticleName = itemName, ArticleType = articleType };

                            folderToRun.NewArticleNames.Add(article);
                        }
                    }

                    procedureNames.Sort(string.CompareOrdinal);

                    server.ConnectionContext.InfoMessage
                        += (sender, args)
                           => folderToRun.Results += string.Format("{0}{1}{2}", '\t', args.Message, Environment.NewLine);

                    server.ConnectionContext.ExecuteNonQuery(currentScriptContents);

                    folderToRun.Results += Environment.NewLine;
                }
                catch (Exception ex)
                {
                    folderToRun.Results += string.Format(Resources.ErrorOccurredExecutingScript, fileName, Environment.NewLine);
                    folderToRun.Results += Environment.NewLine;
                    folderToRun.Results += string.Format(Resources.ExceptionMessage, Environment.NewLine);
                    folderToRun.Results += string.Format("{0}{1}", ex, Environment.NewLine);
                    folderToRun.Results += Environment.NewLine;

                    folderToRun.ErrorProcedures.Add(new StoredProcedure(folderToRun.ServerName,
                                                                        folderToRun.DataBaseName,
                                                                        fileName));
                }
            }
        }

        private void SaveFile(string fileContents)
        {
            if (CurrentSaveAsFileName.IsNullOrWhiteSpace() && saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                CurrentSaveAsFileName = saveFileDialog1.FileName;
            }

            if (!CurrentSaveAsFileName.IsNullOrWhiteSpace())
            {
                using (StreamWriter writer = new StreamWriter(File.Open(CurrentSaveAsFileName, FileMode.Create)))
                {
                    writer.Write(fileContents);
                    writer.Flush();
                }
            }

            CurrentSaveAsFileName = string.Empty;
        }

        private void SaveFile(FolderToRun folderToRun)
        {
            CurrentSaveAsFileName = Path.Combine(folderToRun.ParentFolder,
                                                 string.Format(Resources.ResultsLogFileName,
                                                               folderToRun.FolderName,
                                                               DateTime.Now.ToString(Resources.DateTimeDefaultFormat)));

            SaveFile(folderToRun.Results.TrimSafely());

            if (folderToRun.ErrorProcedures.SafeAny())
            {
                FoldersToRun.ThereWasAnErrorOrWarning();

                CurrentSaveAsFileName = Path.Combine(folderToRun.ParentFolder,
                                                     string.Format(Resources.FailuresLogFileName,
                                                                   folderToRun.FolderName,
                                                                   DateTime.Now.ToString(Resources.DateTimeDefaultFormat)));

                SaveFile(folderToRun.GetErrorProceduresString());
            }

            if (folderToRun.DupedProcedures.SafeAny())
            {
                FoldersToRun.ThereWasAnErrorOrWarning();

                CurrentSaveAsFileName = Path.Combine(folderToRun.ParentFolder,
                                                     string.Format(Resources.WarningsLogFileName,
                                                                   folderToRun.FolderName,
                                                                   DateTime.Now.ToString(Resources.DateTimeDefaultFormat)));

                SaveFile(folderToRun.GetDuplicatedProceduresString());
            }

            CurrentSaveAsFileName = string.Empty;
        }

        private void RemoveLogFiles(string scriptFolder)
        {
            DirectoryInfo mainDirectory = new DirectoryInfo(scriptFolder);
            List<FileInfo> files = mainDirectory.EnumerateFiles(Resources.TextExtension, SearchOption.AllDirectories).ToList();

            StringBuilder builder = new StringBuilder();

            foreach (FileInfo file in files)
            {
                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {
                    builder.AppendFormat(Resources.ErrorDeletingFile, file.FullName, Environment.NewLine);
                    builder.Append(ex);
                }
            }

            if (!builder.IsNullOrWhiteSpace())
            {
                ShowMessage(builder.ToString(),
                            Resources.ErrorDeletingFiles,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
            }
        }

        private DialogResult ShowMessage(string message,
                                         string title,
                                         MessageBoxButtons buttons = MessageBoxButtons.OK,
                                         MessageBoxIcon icon = MessageBoxIcon.Information,
                                         MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button2)
        {
            return MessageBox.Show(this, message, title, buttons, icon, defaultButton);
        }

        public void CheckFoldersReadyToRun()
        {
            bool readyToRun = FoldersToRun.ReadyToRun;
            btnViewFolders.Enabled = readyToRun;
            btnRunScripts.Enabled = readyToRun;
        }

        private void DisableForm()
        {
            btnRunScripts.Enabled = false;
            toolTip1.SetToolTip(btnRunScripts, Resources.RunningMultiScriptOnSelectedFolders);

            btnAddAllSubFolders.Enabled = false;
            btnAddFolder.Enabled = false;
            btnViewFolders.Enabled = false;
            btnRemoveLogFiles.Enabled = false;
            btnConnection.Enabled = false;
            btnBrowse.Enabled = false;
        }

        private void EnableForm()
        {
            btnRunScripts.Enabled = true;
            toolTip1.SetToolTip(btnRunScripts, Resources.RunMultiScriptOnSelectedFolders);

            btnAddAllSubFolders.Enabled = true;
            btnAddFolder.Enabled = true;
            btnViewFolders.Enabled = true;
            btnRemoveLogFiles.Enabled = true;
            btnConnection.Enabled = true;
            btnBrowse.Enabled = true;

            Text = Resources.MultiScriptTitle;
        }

        private static void SetContextMenu()
        {
            //using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Default))
            //{
            //    if (RegisterContextMenu)
            //    {
            //        using (RegistryKey multiScriptKey = key.CreateSubKey(Resources.ContextMenuCommand, RegistryKeyPermissionCheck.ReadWriteSubTree))
            //        {
            //            if (multiScriptKey != null)
            //            {
            //                multiScriptKey.SetValue(string.Empty,
            //                                        string.Format("{0} {1}",
            //                                                      Application.ExecutablePath,
            //                                                      Resources.FirstArgument));
            //            }
            //        }
            //    }
            //    else
            //    {
            //        key.DeleteSubKeyTree(Resources.ContextMenuName, false);
            //    }
            //}
        }

        private void AddSubFolders()
        {
            FoldersToRun.ClearFolders();

            errorProvider1.Clear();
            bool hasErrors = false;

            List<DirectoryInfo> subDirectories = new List<DirectoryInfo>();

            if (txtScriptFolder.Text.IsNullOrWhiteSpace())
            {
                errorProvider1.SetError(txtScriptFolder, Resources.FolderLocationRequired);
                hasErrors = true;
            }
            else
            {
                if (Directory.Exists(txtScriptFolder.Text.TrimSafely()))
                {
                    DirectoryInfo directory = new DirectoryInfo(txtScriptFolder.Text.TrimSafely());
                    subDirectories = directory.EnumerateDirectories().ToList();
                    if (!subDirectories.SafeAny())
                    {
                        errorProvider1.SetError(txtScriptFolder, Resources.NoSubFoldersFound);
                        hasErrors = true;
                    }
                }
                else
                {
                    errorProvider1.SetError(txtScriptFolder, Resources.FolderLocationMustBeValid);
                    hasErrors = true;
                }
            }

            if (!hasErrors)
            {
                StringBuilder builder = new StringBuilder();
                StringBuilder wrongFormatFolders = new StringBuilder();

                foreach (DirectoryInfo subDirectory in subDirectories.OrderBy(s => s.Name))
                {
                    string folderPath = subDirectory.FullName.TrimSafely();

                    string connectionString = txtConnectionString.Text;

                    if (!connectionString.IsNullOrWhiteSpace())
                    {
                        string errorMessage;
                        FoldersToRun.AddFolder(connectionString,
                                               txtScriptFolder.Text.TrimSafely(),
                                               folderPath,
                                               out errorMessage);

                        if (!errorMessage.IsNullOrWhiteSpace())
                        {
                            builder.AppendLine(errorMessage);
                        }
                    }

                }

                if (!wrongFormatFolders.IsNullOrWhiteSpace())
                {
                    builder.AppendLine(string.Format("The following folders were not named in the correct format:{0}", Environment.NewLine));
                    builder.AppendLine(wrongFormatFolders.ToString());
                }

                CheckFoldersReadyToRun();

                if (!builder.IsNullOrWhiteSpace())
                {
                    ShowMessage(string.Format(Resources.ErrorSavingFolderToRunMessage,
                                              string.Format("{0}{1}",
                                                            Environment.NewLine,
                                                            builder)),
                                Resources.ErrorSavingFolderToRunTitle,
                                defaultButton: MessageBoxDefaultButton.Button1);
                }
            }
        }
        #endregion

        #region Events
        #region Button Events
        private void btnConnection_Click(object sender, EventArgs e)
        {
            ShowConnectionDialog();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (!txtScriptFolder.Text.IsNullOrWhiteSpace())
            {
                string path = txtScriptFolder.Text.TrimSafely();
                if (Directory.Exists(path))
                {
                    folderBrowserDialog1.SelectedPath = txtScriptFolder.Text.TrimSafely();
                }
            }

            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtScriptFolder.Text = folderBrowserDialog1.SelectedPath.TrimSafely();
            }
        }

        private void btnAddAllSubFolders_Click(object sender, EventArgs e)
        {
            AddSubFolders();
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool hasErrors = false;

            if (txtConnectionString.Text.IsNullOrWhiteSpace())
            {
                errorProvider1.SetError(txtConnectionString, Resources.ConnectionStringRequired);
                hasErrors = true;
            }

            if (txtScriptFolder.Text.IsNullOrWhiteSpace())
            {
                errorProvider1.SetError(txtScriptFolder, Resources.FolderLocationRequired);
                hasErrors = true;
            }
            else
            {
                if (!Directory.Exists(txtScriptFolder.Text.TrimSafely()))
                {
                    errorProvider1.SetError(txtScriptFolder, Resources.FolderLocationMustBeValid);
                    hasErrors = true;
                }
            }

            if (!hasErrors)
            {
                string errorMessage;
                bool addedToFolders = FoldersToRun.AddFolder(txtConnectionString.Text.TrimSafely(),
                                                             txtScriptFolder.Text.TrimSafely(),
                                                             txtScriptFolder.Text.TrimSafely(),
                                                             out errorMessage);

                if (addedToFolders)
                {
                    CheckFoldersReadyToRun();
                }
                else
                {
                    ShowMessage(string.Format(Resources.ErrorSavingFolderToRunMessage,
                                              string.Format("{0}{1}",
                                                            Environment.NewLine,
                                                            errorMessage)),
                                Resources.ErrorSavingFolderToRunTitle,
                                defaultButton: MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void btnViewFolders_Click(object sender, EventArgs e)
        {
            using (FoldersToRunForm form = new FoldersToRunForm())
            {
                form.Closed += FoldersToRunFormOn_Closed;
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
        }

        private void btnRunScripts_Click(object sender, EventArgs e)
        {
            bool runScripts = ShowMessage(Resources.RunScriptsWarning,
                                          Resources.RunningScripts,
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Exclamation) == DialogResult.Yes;

            if (runScripts)
            {
                DisableForm();
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void btnRemoveLogFiles_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool hasErrors = false;

            if (txtScriptFolder.Text.IsNullOrWhiteSpace())
            {
                errorProvider1.SetError(txtScriptFolder, Resources.FolderLocationRequired);
                hasErrors = true;
            }
            else
            {
                if (!Directory.Exists(txtScriptFolder.Text.TrimSafely()))
                {
                    errorProvider1.SetError(txtScriptFolder, Resources.FolderLocationMustBeValid);
                    hasErrors = true;
                }
            }

            if (!hasErrors)
            {
                bool removeLogFiles = ShowMessage(Resources.RemoveLogFilesWarning,
                                                  Resources.RemovingLogFiles,
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Exclamation) == DialogResult.Yes;

                if (removeLogFiles)
                {
                    RemoveLogFiles(txtScriptFolder.Text.TrimSafely());
                }
            }
        }
        #endregion

        #region Background Worker Events
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            RunMultiScriptMultiFolder();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressReporter progressReporter = e.UserState as ProgressReporter;
            if (progressReporter != null)
            {
                Text = string.Format("{0} - {1}", Resources.MultiScriptTitle, progressReporter.ProgressString);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool logsWereRemoved = false;

            //If no errors/warnings, then remove logs (Either automatically, if set, or by prompting user)
            if (FoldersToRun.NoErrorsOrWarningsEncountered && !txtScriptFolder.Text.IsNullOrWhiteSpace())
            {
                bool removeLogFiles = RemoveSuccessfulLogs;
                if (!removeLogFiles)
                {
                    removeLogFiles = ShowMessage(Resources.RemoveLogFilesWarning,
                                                 Resources.RemovingLogFiles,
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Exclamation) == DialogResult.Yes;
                }

                if (removeLogFiles)
                {
                    logsWereRemoved = true;
                    RemoveLogFiles(txtScriptFolder.Text.TrimSafely());
                }
            }

            string title = FoldersToRun.NoErrorsOrWarningsEncountered
                               ? Resources.Success
                               : Resources.Error;

            string message = FoldersToRun.NoErrorsOrWarningsEncountered
                                 ? logsWereRemoved
                                       ? Resources.NoErrorsLogRemoved
                                       : Resources.LogsSavedWithNoErrors
                                 : Resources.LogsSavedWithErrorsOrWarnings;

            ShowMessage(message, title, defaultButton: MessageBoxDefaultButton.Button1);

            FoldersToRun.ClearFolders();

            EnableForm();

            CheckFoldersReadyToRun();
        }
        #endregion

        #region Toolbar Events
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentSaveAsFileName = string.Empty;
            txtConnectionString.Clear();
            FoldersToRun.ClearFolders();
            SetFolder(DefaultFolderLocation);
            EnableForm();
            CheckFoldersReadyToRun();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PreferencesForm form = new PreferencesForm())
            {
                form.LoadFormFromPreferences(FormPreferences);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    //Save preferences here
                    Preferences prefs = form.MultiScriptPreferences;
                    RemoveSuccessfulLogs = prefs.RemoveSuccessfulLogs;
                    DefaultFolderLocation = prefs.DefaultFolderLocation;
                    DefaultEnvironment = prefs.DefaultEnvironment;
                    RegisterContextMenu = prefs.RegisterContextMenu;
                    AutoAddFolders = prefs.AutoAddFolders;
                    SetFolder(DefaultFolderLocation);
                    SetContextMenu();
                }
            }
        }

        #endregion

        #region Misc Events

        private void FoldersToRunFormOn_Closed(object sender, EventArgs eventArgs)
        {
            CheckFoldersReadyToRun();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            }
            catch
            {
                ShowMessage(Resources.ErrorPerformingDragDrop, Resources.ErrorDragDropTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    IEnumerable<string> folder = e.Data.GetData(DataFormats.FileDrop) as IEnumerable<string>;
                    if (folder.SafeAny())
                    {
                        string folderName = folder.FirstOrDefault();
                        if (!folderName.IsNullOrWhiteSpace())
                        {
                            SetFolder(folderName);
                        }
                    }
                }
            }
            catch
            {
                ShowMessage(Resources.ErrorPerformingDragDrop, Resources.ErrorDragDropTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void dEVToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void clearConnectionStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = string.Empty;
        }

        private void devAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Development,
                                                                                      EnumDataBase.Accounts);
        }

        private void syncToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Development,
                EnumDataBase.Sync);
        }

        private void smsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Development,
                EnumDataBase.Sms);
        }

        private void stageAccountsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Stage,
                                                                                      EnumDataBase.Accounts);
        }

        private void syncToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Stage,
               EnumDataBase.Sync);
        }

        private void smsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Stage,
               EnumDataBase.Sms);
        }

        private void preProdAccountsToolStripMenuItem2Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.PreProduction,
                                                                                      EnumDataBase.Accounts);
        }

        private void syncToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.PreProduction,
                                                                                      EnumDataBase.Sync);
        }

        private void smsToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.PreProduction,
                                                                                      EnumDataBase.Sms);
        }

        private void accountsToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Production,
                EnumDataBase.Accounts, true);
        }

        private void smsToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Production,
                EnumDataBase.Sms, true);
        }

        private void syncToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Production,
               EnumDataBase.Sync, true);
        }

        private void mDMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Development,
                EnumDataBase.MDM);
        }

        private void mDMToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Stage,
                EnumDataBase.MDM);
        }

        private void mDMToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.PreProduction,
                EnumDataBase.MDM);
        }

        private void mDMToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Production,
               EnumDataBase.MDM, true);
        }

        private void accountsToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Uat,
               EnumDataBase.Accounts);
        }

        private void syncToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Uat,
               EnumDataBase.Sync);
        }

        private void smsToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Uat,
              EnumDataBase.Sms);
        }

        private void mDMToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            txtConnectionString.Text = MultiScriptLib.MultiScript.GetConnectionString(EnumEnvironments.Uat,
             EnumDataBase.MDM);
        }
    }
}