using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using UtilsLib.ExtensionMethods;

namespace MultiScriptLib
{
    /// <summary>
    /// Enumeration of Different Deployment Environments
    /// </summary>
    public enum EnumEnvironments
    {
        None = -1,
        Development = 1,
        Stage = 2,
        PreProduction = 3,
        Production = 4,
        Uat = 5
    }

    public enum EnumDataBase
    {
        None = 0,
        Accounts = 1,
        Sms = 2,
        Sync = 3,
        MDM = 4
    }

    [Serializable]
    public class RegisteredConnectionString {
        
        public Guid? ConnectionStringId { get; set; }

        public string ConnectionString { get; set; }

        public string DisplayName { get; set; }

        public override string ToString()
        {
            return string.Format("Display Name - {0}, Connection String: {1}", DisplayName, ConnectionString);
        }
    }

    [Serializable]
    public class RegisteredServer
    {
        public Guid? ServerId { get; set; }

        public string ServerName { get; set; }

        public List<RegisteredConnectionString> ConnectionStrings { get; set; }

        public RegisteredServer()
        {
            ConnectionStrings = new List<RegisteredConnectionString>();
        }
    }

    /// <summary>
    /// Reports on Progress of MultiScript
    /// </summary>
    public class ProgressReporter
    {
        public int TotalFolders { get; set; }

        public int CurrentFolder { get; set; }

        public int TotalScriptsInFolder { get; set; }

        public int CurrentScriptInFolder { get; set; }

        public void ClearFolder()
        {
            TotalScriptsInFolder = 0;
            CurrentScriptInFolder = 0;
        }

        public void ClearProgress()
        {
            TotalFolders = 0;
            CurrentFolder = 0;
            TotalScriptsInFolder = 0;
            CurrentScriptInFolder = 0;
        }

        public int FolderPercentage
        {
            get { return TotalFolders == 0 ? 0 : CurrentFolder * 100 / TotalFolders; }
        }

        public string ProgressString
        {
            get
            {
                return string.Format("Folder Progress: {0}/{1} - {2}% - Current Scripts Progress: {3}/{4} - {5}%",
                                     CurrentFolder,
                                     TotalFolders,
                                     FolderPercentage,
                                     CurrentScriptInFolder,
                                     TotalScriptsInFolder,
                                     FolderScriptsPercentage);
            }
        }

        public int FolderScriptsPercentage
        {
            get { return TotalScriptsInFolder == 0 ? 0 : CurrentScriptInFolder * 100 / TotalScriptsInFolder; }
        }

        public void IncrementFolderCount()
        {
            CurrentFolder++;
        }

        public void IncrementFolderScriptCount()
        {
            CurrentScriptInFolder++;
        }
    }

    /// <summary>
    /// Specific Folder/ConnectionString
    /// </summary>
    public class FolderToRun
    {
        public int SequenceNumber { get; set; }
        public string ConnectionString { get; set; }
        public string ParentFolder { get; set; }
        public string SelectedFolder { get; set; }
        public string FolderName { get; set; }
        public string Results { get; set; }

        public List<StoredProcedure> ErrorProcedures { get; set; }
        public List<StoredProcedure> DupedProcedures { get; set; }

        public FolderToRun()
        {
            ErrorProcedures = new List<StoredProcedure>();
            DupedProcedures = new List<StoredProcedure>();
        }

        public string ServerName
        {
            get
            {
                string server = string.Empty;

                if (!string.IsNullOrWhiteSpace(ConnectionString))
                {
                    string[] connectionParts = ConnectionString.Split(';');
                    if (connectionParts.Length > 1)
                    {
                        foreach (string[] keyValue in connectionParts.Select(connectionPart => connectionPart.Split('='))
                                                                     .Where(keyValue => connectionParts.Length > 1)
                                                                     .Where(keyValue => keyValue[0].ToLower().Contains("data source")))
                        {
                            server = keyValue[1];
                        }
                    }
                }

                return server;
            }
        }

        public string DataBaseName
        {
            get
            {
                string dataBase = string.Empty;

                if (!ConnectionString.IsNullOrWhiteSpace())
                {
                    string[] connectionParts = ConnectionString.Split(';');
                    if (connectionParts.Length > 1)
                    {
                        foreach (string[] keyValue in connectionParts.Select(connectionPart => connectionPart.Split('='))
                                                                     .Where(keyValue => connectionParts.Length > 1)
                                                                     .Where(keyValue => keyValue[0].Contains("initial catalog",
                                                                                                             StringComparison.InvariantCultureIgnoreCase)))
                        {
                            dataBase = keyValue[1];
                        }
                    }
                }

                return dataBase;
            }
        }

        public override string ToString()
        {
            return string.Format("Sequence: {0}; Server: {1}; Database: {2}; SelectedFolder: {3};",
                                 SequenceNumber,
                                 ServerName,
                                 DataBaseName,
                                 SelectedFolder);
        }

        public string GetErrorProceduresString()
        {
            StringBuilder builder = new StringBuilder();

            if (ErrorProcedures.SafeAny())
            {
                builder.AppendLine("The following procedures encountered errors:");

                foreach (StoredProcedure errorProc in ErrorProcedures)
                {
                    builder.AppendLine(string.Format("-> {0}", errorProc));
                }
            }

            return builder.ToString();
        }

        

        public string GetDuplicatedProceduresString()
        {
            StringBuilder builder = new StringBuilder();

            if (DupedProcedures.SafeAny())
            {
                builder.AppendLine("The following procedures were run multiple times:");

                foreach (StoredProcedure dupedProc in DupedProcedures)
                {
                    builder.AppendLine(string.Format("-> {0}", dupedProc));
                }
            }

            return builder.ToString();
        }

        public void ClearProcedureLists()
        {
            ErrorProcedures = new List<StoredProcedure>();
            DupedProcedures = new List<StoredProcedure>();
        }
    }

    /// <summary>
    /// Static Class which contains all the folders to run in current session
    /// </summary>
    public static class FoldersToRun
    {
        private static bool AnyErrorsOrWarnings { get; set; }

        private static List<FolderToRun> SelectedFoldersToRun { get; set; }

        public static int NextSequenceNumber { get { return SelectedFoldersToRun.SafeAny() ? SelectedFoldersToRun.Max(f => f.SequenceNumber) + 1 : 1; } }

        public static bool ReadyToRun
        {
            get
            {
                return SelectedFoldersToRun.SafeAny();
            }
        }

        public static bool NoErrorsOrWarningsEncountered
        {
            get { return !AnyErrorsOrWarnings; }
        }

        public static void InitializeFolders()
        {
            SelectedFoldersToRun = new List<FolderToRun>();

            AnyErrorsOrWarnings = false;
        }

        public static void ClearFolders()
        {
            InitializeFolders();
        }

        public static List<FolderToRun> SelectedFolders
        {
            get { return SelectedFoldersToRun; }
        }

        public static bool AddFolder(string connectionString, string parentFolderLocation, string folderLocation, out string errorMessage)
        {
            if (FolderToRunIsValid(connectionString, folderLocation, out errorMessage))
            {
                DirectoryInfo selectedFolder = new DirectoryInfo(folderLocation);

                FolderToRun folderToRun
                    = new FolderToRun
                        {
                            SequenceNumber = NextSequenceNumber,
                            ConnectionString = connectionString,
                            SelectedFolder = folderLocation,
                            ParentFolder = parentFolderLocation,
                            FolderName = selectedFolder.Name
                        };

                if (SelectedFoldersToRun == null)
                {
                    InitializeFolders();
                }

                if (SelectedFoldersToRun != null)
                {
                    SelectedFoldersToRun.Add(folderToRun);
                    FixSequenceNumbers();
                }

                return true;
            }

            return false;
        }

        public static void RemoveFolder(FolderToRun folderToRemove)
        {
            RemoveFolder(folderToRemove.ConnectionString, folderToRemove.SelectedFolder);
        }

        public static void RemoveFolder(string connectionString, string folderLocation)
        {
            if (SelectedFoldersToRun.SafeAny(f => f.ConnectionString.Equals(connectionString, StringComparison.InvariantCultureIgnoreCase) &&
                                                  f.SelectedFolder.Equals(folderLocation, StringComparison.InvariantCultureIgnoreCase)))
            {
                SelectedFoldersToRun.RemoveAll(f => f.ConnectionString.Equals(connectionString, StringComparison.InvariantCultureIgnoreCase) &&
                                                    f.SelectedFolder.Equals(folderLocation, StringComparison.InvariantCultureIgnoreCase));
            }

            FixSequenceNumbers();
        }

        private static void FixSequenceNumbers()
        {
            int sequence = 1;
            foreach (FolderToRun folder in SelectedFoldersToRun.OrderBy(f => f.SequenceNumber))
            {
                folder.SequenceNumber = sequence;
                sequence++;
            }
        }

        /// <summary>
        /// Validate Folder Location and Connection String
        /// </summary>
        /// <param name="connectionString" />
        /// <param name="folderLocation" />
        /// <param name="errorMessage" />
        /// <returns />
        private static bool FolderToRunIsValid(string connectionString, string folderLocation, out string errorMessage)
        {
            bool folderToRunIsValid = true;
            errorMessage = string.Empty;

            try
            {
                if (connectionString.IsNullOrWhiteSpace() || folderLocation.IsNullOrWhiteSpace())
                {
                    folderToRunIsValid = false;
                    errorMessage += "ConnectionString or FolderLocation were missing.{0}";
                }
                else
                {
                    if (Directory.Exists(folderLocation))
                    {
                        if (SelectedFoldersToRun.SafeAny(f => f.ConnectionString.Equals(connectionString) && f.SelectedFolder.Equals(folderLocation)))
                        {
                            folderToRunIsValid = false;
                            errorMessage += "Selected Folder has already been added to list.{0}";
                        }
                    }
                    else
                    {
                        folderToRunIsValid = false;
                        errorMessage += "FolderLocation did not exist.{0}";
                    }
                }
            }
            catch (Exception)
            {
                folderToRunIsValid = false;
                errorMessage += "Unexpected error occured.{0}";
            }

            if (!errorMessage.IsNullOrWhiteSpace())
            {
                errorMessage = string.Format("Folder: {0}{3} ConnectionString: {1}{3} Errors: {2}{3}",
                                             folderLocation,
                                             connectionString,
                                             errorMessage,
                                             Environment.NewLine);
            }

            errorMessage = string.Format(errorMessage.TrimSafely(), Environment.NewLine);

            return folderToRunIsValid;
        }

        public static void ThereWasAnErrorOrWarning()
        {
            AnyErrorsOrWarnings = true;
        }

        public static void ClearOutAllErrorsOrWarnings()
        {
            AnyErrorsOrWarnings = false;
        }

        public new static string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (FolderToRun folder in SelectedFoldersToRun)
            {
                builder.AppendLine(folder.ToString());
                builder.AppendHtmlLine(StringExtensions.BreakTag);
            }

            return builder.ToString().RemoveLastInstanceOfWord(Environment.NewLine);
        }
    }

    /// <summary>
    /// Command Line Arguments
    /// </summary>
    public class CommandLineArguments
    {
        public CommandLineArguments()
        {
            MainFolderToRun = string.Empty;
            FoldersToRun.ClearFolders();
            FoldersToRun.ClearOutAllErrorsOrWarnings();
            FoldersToRun.InitializeFolders();
            StoredProcedures.ClearProcedures();
        }

        public void RunScriptsAsync()
        {
            if (IsValid)
            {
                Thread th = new Thread(WorkerProcess);

                th.Start();
            }
        }

        private void WorkerProcess()
        {
            foreach (FolderToRun folderToRun in FoldersToRun.SelectedFolders)
            {
                List<string> procedureNames = new List<string>();

                List<string> fileEntries = Directory.GetFiles(folderToRun.SelectedFolder, Resources.SQLFileExtension).ToList();

                fileEntries.Sort(string.CompareOrdinal);

                folderToRun.ClearProcedureLists();

                folderToRun.Results += string.Format(Resources.CurrentConnectionString,
                                                 folderToRun.ConnectionString,
                                                 Environment.NewLine);

                foreach (string fileName in fileEntries)
                {
                    try
                    {
                        string currentDatabase = string.Empty;

                        FileInfo currentFile = new FileInfo(fileName);
                        string currentScriptContents = currentFile.OpenText().ReadToEnd();

                        Server server = new Server(new ServerConnection(new SqlConnection(folderToRun.ConnectionString)));

                        folderToRun.Results += string.Format(Resources.ExecutingScript, currentFile, Environment.NewLine);

                        Console.WriteLine(Resources.ExecutingScript, currentFile, Environment.NewLine);

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

                            string articleType = string.Empty;

                            string itemName = string.Empty;

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
                        }

                        procedureNames.Sort(string.CompareOrdinal);

                        string message = string.Empty;

                        server.ConnectionContext.InfoMessage
                            += (sender, args)
                               => folderToRun.Results += (message = string.Format("{0}{1}{2}", '\t', args.Message, Environment.NewLine));

                        Console.WriteLine(message);

                        server.ConnectionContext.ExecuteNonQuery(currentScriptContents);

                        folderToRun.Results += Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = string.Format("Error Occurred Executing Script: {0}{1}", fileName, Environment.NewLine);
                        errorMessage += Environment.NewLine;
                        errorMessage += string.Format("Exception Message:{0}", Environment.NewLine);
                        errorMessage += string.Format("{0}{1}", ex, Environment.NewLine);
                        errorMessage += Environment.NewLine;

                        Console.WriteLine(errorMessage);

                        folderToRun.Results += errorMessage;
                        folderToRun.ErrorProcedures.Add(new StoredProcedure(folderToRun.ServerName,
                                                                            folderToRun.DataBaseName,
                                                                            fileName));

                        FoldersToRun.ThereWasAnErrorOrWarning();
                    }

                }

                SaveFile(folderToRun);
            }

            if (FoldersToRun.NoErrorsOrWarningsEncountered)
            {
                Console.WriteLine("FIN");
            }
            else
            {
                Console.WriteLine("MultiScript encountered at least one error/warning. Logs have been saved in the main folder. Please consult them for more details.");
                Environment.ExitCode = 1;
            }
        }

        private void SaveFile(string fileContents)
        {
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

        public bool BuildFoldersToRun(out string errorMessage)
        {
            FoldersToRun.ClearFolders();

            FoldersToRun.ClearOutAllErrorsOrWarnings();

            FoldersToRun.InitializeFolders();

            StoredProcedures.ClearProcedures();

            StringBuilder builder = new StringBuilder();

            errorMessage = string.Empty;

            if (AddAllSubfolders)
            {
                DirectoryInfo directory = new DirectoryInfo(MainFolderToRun);

                List<DirectoryInfo> subDirectories = directory.EnumerateDirectories().ToList();

                foreach (DirectoryInfo subDirectory in subDirectories.OrderBy(s => s.Name))
                {
                    string folderPath = subDirectory.FullName.TrimSafely();

                    string connectionString = ConnectionString;

                    if (!connectionString.IsNullOrWhiteSpace())
                    {
                        FoldersToRun.AddFolder(connectionString,
                                               MainFolderToRun,
                                               folderPath,
                                               out errorMessage);

                        if (!errorMessage.IsNullOrWhiteSpace())
                        {
                            builder.AppendLine(errorMessage);
                        }
                    }

                }

            }
            else
            {
                FoldersToRun.AddFolder(ConnectionString, MainFolderToRun, MainFolderToRun, out errorMessage);
            }

            return errorMessage.IsNullOrWhiteSpace();
        }

        private string CurrentSaveAsFileName { get; set; }

        public EnumDataBase Database
        {
            get
            {
                EnumDataBase dataBase = EnumDataBase.None;

                if (!DatabaseName.IsNullOrWhiteSpace())
                {
                    dataBase = MultiScript.ParseDatabaseFromString(DatabaseName);
                }

                return dataBase;
            }
        }

        public EnumEnvironments EnvironmentContext
        {
            get
            {
                EnumEnvironments environmentContext = EnumEnvironments.None;

                if (!ServerName.IsNullOrWhiteSpace())
                {
                    environmentContext = MultiScript.ParseServerEnvironmentFromString(ServerName);
                }

                return environmentContext;
            }
        }

        public bool OverrideConnectionString { get; set; }

        public string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/sets which folder to run MultiScript on
        /// </summary>
        public string MainFolderToRun { get; set; }

        /// <summary>
        /// Gets/sets which server to run MultiScript on
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets/sets which database to run MultiScript on
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets/sets/ a flag indicating if all sub folders will be added from the current folder
        /// </summary>
        public bool AddAllSubfolders { get; set; }

        /// <summary>
        /// Gets whether or not all arguments are valid;
        /// </summary>
        public bool IsValid
        {
            get
            {
                string errorMessage = CommandLineInvalidArgumentDetails;

                return errorMessage.IsNullOrWhiteSpace();
            }
        }

        public string CommandLineInvalidArgumentDetails
        {
            get
            {
                string details = string.Empty;

                if (MainFolderToRun.IsNullOrWhiteSpace())
                {
                    details += string.Format("Script folder is required.{0}", Environment.NewLine);
                }

                if (!Directory.Exists(MainFolderToRun))
                {
                    details += string.Format("Script folder must be a valid directory.{0}", Environment.NewLine);
                }

                if (!OverrideConnectionString && ServerName.IsNullOrWhiteSpace())
                {
                    details += string.Format("Server name must be valid.{0}", Environment.NewLine);
                }

                if (!OverrideConnectionString && !ServerName.IsNullOrWhiteSpace() &&
                    EnvironmentContext == EnumEnvironments.None)
                {
                    details +=
                        string.Format(
                            "Environment could not be determined. Please specify connection string explicity.{0}",
                            Environment.NewLine);
                }

                if (!OverrideConnectionString && !DatabaseName.IsNullOrWhiteSpace() && Database == EnumDataBase.None)
                {
                    details +=
                        string.Format(
                            "Database could not be determined. Please specify connection string explicity.{0}",
                            Environment.NewLine);
                }

                return details;
            }
        }
    }

    public class NewArticle
    {
        public string ArticleName { get; set; }

        public string ArticleType { get; set; }
    }

    /// <summary>
    /// StoredProcedures Ran Through MultiScript
    /// </summary>
    public class StoredProcedure
    {
        public StoredProcedure(string serverName, string dataBaseName, string procedureName)
        {
            Server = serverName;
            Database = dataBaseName;
            ProcedureName = procedureName;
        }

        public string Server { get; set; }
        public string Database { get; set; }
        public string ProcedureName { get; set; }

        public override string ToString()
        {
            return string.Format("Server: {0}; Database: {1}; ProcedureName: {2};", Server, Database, ProcedureName);
        }
    }

    /// <summary>
    /// StoredProcedures Ran Through MultiScript
    /// </summary>
    public static class StoredProcedures
    {
        private static List<StoredProcedure> CurrentStoredProcedures { get; set; }

        public static void InitializeProcedures()
        {
            CurrentStoredProcedures = new List<StoredProcedure>();
        }

        public static void ClearProcedures()
        {
            InitializeProcedures();
        }

        public static List<StoredProcedure> CurrentProcedures
        {
            get { return CurrentStoredProcedures; }
        }

        public static bool Exists(string server, string database, string procedureName)
        {
            return CurrentStoredProcedures.SafeAny(c => c.Server.SafeEquals(server) &&
                                                        c.Database.SafeEquals(database) &&
                                                        c.ProcedureName.SafeEquals(procedureName));
        }

        public static bool AddProcedure(string server, string database, string procedureName)
        {
            server = server.TrimSafely();
            database = database.TrimSafely();
            procedureName = procedureName.TrimSafely();

            bool procedureIsDuplicated = false;

            if (Exists(server, database, procedureName))
            {
                procedureIsDuplicated = true;
            }
            else
            {
                CurrentStoredProcedures.Add(new StoredProcedure(server, database, procedureName));
            }

            return procedureIsDuplicated;
        }
    }

    [Serializable]
    public class Preferences
    {
        public bool RemoveSuccessfulLogs { get; set; }

        public string DefaultFolderLocation { get; set; }

        public EnumEnvironments DefaultEnvironment { get; set; }

        public bool RegisterContextMenu { get; set; }

        public bool AutoAddFolders { get; set; }

        public Preferences()
        {
            RemoveSuccessfulLogs = true;
            DefaultFolderLocation = string.Empty;
            DefaultEnvironment = EnumEnvironments.None;
            RegisterContextMenu = true;
            AutoAddFolders = false;
        }
    }
}
