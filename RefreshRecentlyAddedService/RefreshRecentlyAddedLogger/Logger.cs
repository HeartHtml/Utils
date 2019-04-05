using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefreshRecentlyAddedLogger
{
    public class Logger
    {
        public void LogException(Exception e)
        {
            if (!EventLog.SourceExists("RefreshRecentlyAdded"))
            {
                EventLog.CreateEventSource("RefreshRecentlyAdded", "FatalError");
            }
            EventLog.WriteEntry("RefreshRecentlyAdded", e.Message, EventLogEntryType.Error);
        }
    }
}
