using System;
using System.Threading;
using JS.Entities.ExtensionMethods;

namespace MultiScriptLib
{
    public class MultiScript
    {
        public static void RunMultiScriptCommandLine(CommandLineArguments commandLineArguments)
        {
            commandLineArguments.RunScriptsAsync();
        }

        public static string GetConnectionString(EnumEnvironments environment, EnumDataBase dataBase, bool useUsernameAndPasswordConnectionString = false)
        {
            string server;
            string db;

            string user = Resources.SqlUser;

            string password = Resources.SqlPassword;

            switch (environment)
            {
                case EnumEnvironments.Development:
                    {
                        server = Resources.DevServer;
                        break;
                    }
                case EnumEnvironments.Stage:
                    {
                        server = Resources.StageServer;
                        break;
                    }
                case EnumEnvironments.PreProduction:
                    {
                        server = Resources.PreprodServer;
                        break;
                    }
                case EnumEnvironments.Production:
                    {
                        server = Resources.ProductionServer;
                        break;
                    }
                case EnumEnvironments.Uat:
                    {
                        server = Resources.UatServer;
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("Not implement connection string");
                    }
            }

            switch (dataBase)
            {
                case EnumDataBase.Accounts:
                    {
                        db = Resources.AccountsDb;
                        break;
                    }
                case EnumDataBase.Sms:
                    {
                        db = Resources.SmsDb;
                        break;
                    }
                case EnumDataBase.Sync:
                    {
                        db = Resources.SyncDb;
                        break;
                    }
                case EnumDataBase.MDM:
                    {
                        db = Resources.MdmDb;
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("Not implement connection string");
                    }
            }

            return string.Format(useUsernameAndPasswordConnectionString ? Resources.UsernameAndPasswordConnectionString : Resources.IntegratedSecurityConnectionString, server, db, user, password);
        }

        public static EnumDataBase ParseDatabaseFromString(string databaseName)
        {
            if (databaseName.SafeEquals(Resources.AccountsDb))
            {
                return EnumDataBase.Accounts;
            }

            if (databaseName.SafeEquals(Resources.SyncDb))
            {
                return EnumDataBase.Sync;
            }

            if (databaseName.SafeEquals(Resources.SmsDb))
            {
                return EnumDataBase.Sms;
            }

            if (databaseName.SafeEquals(Resources.MdmDb))
            {
                return EnumDataBase.MDM;
            }

            return EnumDataBase.None;
        }

        public static EnumEnvironments ParseServerEnvironmentFromString(string serverName)
        {
            if (serverName.SafeEquals(Resources.DevServer))
            {
                return EnumEnvironments.Development;
            }

            if (serverName.SafeEquals(Resources.StageServer))
            {
                return EnumEnvironments.Stage;
            }

            if (serverName.SafeEquals(Resources.PreprodServer))
            {
                return EnumEnvironments.PreProduction;
            }

            if (serverName.SafeEquals(Resources.UatServer))
            {
                return EnumEnvironments.Uat;
            }

            if (serverName.SafeEquals(Resources.ProductionServer))
            {
                return EnumEnvironments.Production;
            }

            return EnumEnvironments.None;
        }
    }
}
