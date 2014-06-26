using System.Configuration;

namespace MultiScriptLib
{
    public class Resources
    {

        public static string AccountsDb { get { return @"Accounts"; } }

        public static string SyncDb { get { return @"Sync"; } }

        public static string SmsDb { get { return @"Sms"; } }

        public static string MdmDb { get { return @"MDM"; }}

        public static string DevServer { get { return @"DEVDB01"; } }

        public static string StageServer { get { return @"QADB01"; } }

        public static string PreprodServer { get { return @"VALDB01"; } }

        public static string UatServer { get { return @"m3db01\m3uat"; }}

        public static string ProductionServer { get { return Properties.Settings.Default.ProductionServer; } }

        public static string SqlUser { get { return Properties.Settings.Default.SqlUser; } }

        public static string SqlPassword { get { return Properties.Settings.Default.SqlPassword; } }

        public static string IntegratedSecurityConnectionString { get { return @"Data Source={0};Integrated Security=True;initial catalog={1}"; } }

        public static string UsernameAndPasswordConnectionString { get { return @"Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}"; } }

        public static string SQLFileExtension { get { return @"*.sql"; } }

        public static string ExecutingScript { get { return @"Executing Script: {0}{1}"; } }

        public static string StoredProcChangedMultipleTimes { get { return @"WARNING: The Stored Procedure {0} Has Been Changed Multiple Times."; } }

        public static string FailuresLogFileName { get { return @"1-Failures-{0}-{1}.txt"; } }

        public static string WarningsLogFileName { get { return @"2-Warnings-{0}-{1}.txt"; } }

        public static string ResultsLogFileName { get { return @"3-ResultsLog-{0}-{1}.txt"; } }

        public static string DateTimeDefaultFormat { get { return @"yyyyMMdd_HHmmss"; } }

        public static string CurrentConnectionString { get { return "Current Connection String: {0}{1}"; } }

        public static string NewArticlesLogFileName { get { return "4-NewArticles-{0}-{1}.txt"; }}

    }
}
