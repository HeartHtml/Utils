using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XmlPrettyPrint
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            string[] arguments = Environment.GetCommandLineArgs();

            if (arguments.Length > 0)
            {
                string formattedXml;

                try
                {
                    string xmlToPrettyPrint = arguments[1];

                    xmlToPrettyPrint = FixXmlDeclaration(xmlToPrettyPrint);

                    formattedXml = PrettyXml(xmlToPrettyPrint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Parsing xml failed: {0}", ex.Message);

                    formattedXml = string.Empty;
                }

                try
                {
                    SendTextToNotepad(formattedXml);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Sending to clipboard failed: {0}", ex.Message);
                }
            }
        }

        private static string FixXmlDeclaration(string xmlToPrettyPrint)
        {
            bool removeDeclaration = false;

            string firstLine;

            using (StringReader reader = new StringReader(xmlToPrettyPrint))
            {
                firstLine = reader.ReadLine();

                if (firstLine != null)
                {
                    if (firstLine.Contains("xml version"))
                    {
                        removeDeclaration = true;
                    }
                }
            }

            if (removeDeclaration)
            {
                xmlToPrettyPrint = xmlToPrettyPrint.Replace(firstLine, string.Empty);
            }

            return xmlToPrettyPrint;
        }

        public static string PrettyXml(string xml, bool omitXmlDeclaration = false)
        {
            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = omitXmlDeclaration,
                Indent = true,
                NewLineOnAttributes = true
            };

            Encoding encoding = Encoding.UTF8;

            settings.Encoding = encoding;

            StringWriter stringWriter = new StringWriterWithEncoding(encoding);

            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                element.Save(xmlWriter);
            }

            return stringWriter.ToString();
        }

        private const int WM_SETTEXT = 0x000c;

		[DllImport("User32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]

        private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle rect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void SendTextToNotepad(string text)
        {
            Process notepad = Process.Start("notepad.exe");

            notepad.WaitForInputIdle(100);

            IntPtr hWndNotepad = FindWindow("Notepad", null);

            IntPtr hWndEdit = FindWindowEx(hWndNotepad, IntPtr.Zero, "Edit", null);

            SendMessage(hWndEdit, WM_SETTEXT, 0, text);

            IntPtr hWnd = notepad.MainWindowHandle;

            if (hWnd != IntPtr.Zero)
            {
                SetForegroundWindow(hWnd);

                ShowWindow(hWnd, 3);

                Rectangle rect = new Rectangle();

                GetWindowRect(hWndNotepad, ref rect);

                Cursor.Position = new Point(rect.Left + 140, rect.Top + 240);

                //Call the imported function with the cursor's current position
                int x = Cursor.Position.X;

                int y = Cursor.Position.Y;

                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Convert.ToUInt32(x), Convert.ToUInt32(y), 0, 0);
            }
        }

    }
}
