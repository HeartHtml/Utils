using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using NDesk.Options;

namespace EventLogCreator
{
    class Program
    {
        public static void Main(string[] args)
        {
            string eventLogToCreate = string.Empty;

            string eventSourceToCreate = string.Empty;

            bool showHelp = false;

            OptionSet p = new OptionSet
            {
                {"EventLog=", "The name of the log to create.", v => eventLogToCreate = v},
                {"EventSource=", "The name of the source to create in the specified log.", v => eventSourceToCreate = v},
                {"h|help", "Show this message and exit", v => showHelp = v != null },
            };

            List<string> extra;

            try
            {
                extra = p.Parse(args);
            }
            catch (Exception ex)
            {
                Console.Write("EventLogCreator: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try `EventLogCreator --help' for more information.");
                return;
            }

            if (showHelp || extra.Count > 0 || args.Length == 0)
            {
                ShowHelp(p);
                return;
            }

            try
            {
                if (!EventLog.SourceExists(eventSourceToCreate))
                {
                    EventLog.CreateEventSource(eventSourceToCreate, eventLogToCreate);
                    Console.WriteLine("Source: {0} created successfully in Log: {1}", eventSourceToCreate, eventLogToCreate);
                }
                else
                {
                    Console.WriteLine("Source: {0} already exists", eventSourceToCreate);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: EventLogCreator [OPTIONS]+");
            Console.WriteLine("Create an event log with a specified source.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
