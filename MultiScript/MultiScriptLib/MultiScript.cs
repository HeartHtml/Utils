using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MultiScriptLib
{
    public class MultiScript
    {
        /// <summary>
        /// Returns true if value contains any value in valueToCheck
        /// </summary>
        /// <param name="value" />
        /// <param name="valuesToCheck" />
        /// <param name="comparison" />
        /// <returns />
        public static bool ContainsAny(string value,
                                       IEnumerable<string> valuesToCheck,
                                       StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            bool contains = false;

            if (!string.IsNullOrWhiteSpace(value))
            {
                foreach (string valueToCheck in valuesToCheck)
                {
                    switch (comparison)
                    {
                        case StringComparison.CurrentCultureIgnoreCase:
                        case StringComparison.InvariantCultureIgnoreCase:
                        case StringComparison.OrdinalIgnoreCase:
                            contains = value.ToUpper().Trim().Contains(valueToCheck.ToUpper().Trim());
                            break;

                        default:
                            contains = value.Trim().Contains(valueToCheck.Trim());
                            break;
                    }

                    if (contains)
                    {
                        break;
                    }
                }
            }

            return contains;
        }

        /// <summary>
        /// Removes all strings from string
        /// </summary>
        /// <param name="value" />
        /// <param name="delims" />
        /// <returns />
        public static string RemoveStrings(string value, params string[] delims)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = delims.Aggregate(value, (current, delim) => current.Replace(delim, string.Empty));
            }

            return value;
        }

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
            if (databaseName.Equals(Resources.AccountsDb))
            {
                return EnumDataBase.Accounts;
            }

            if (databaseName.Equals(Resources.SyncDb))
            {
                return EnumDataBase.Sync;
            }

            if (databaseName.Equals(Resources.SmsDb))
            {
                return EnumDataBase.Sms;
            }

            if (databaseName.Equals(Resources.MdmDb))
            {
                return EnumDataBase.MDM;
            }

            return EnumDataBase.None;
        }

        public static EnumEnvironments ParseServerEnvironmentFromString(string serverName)
        {
            if (serverName.Equals(Resources.DevServer))
            {
                return EnumEnvironments.Development;
            }

            if (serverName.Equals(Resources.StageServer))
            {
                return EnumEnvironments.Stage;
            }

            if (serverName.Equals(Resources.PreprodServer))
            {
                return EnumEnvironments.PreProduction;
            }

            if (serverName.Equals(Resources.UatServer))
            {
                return EnumEnvironments.Uat;
            }

            if (serverName.Equals(Resources.ProductionServer))
            {
                return EnumEnvironments.Production;
            }

            return EnumEnvironments.None;
        }
    }
}
