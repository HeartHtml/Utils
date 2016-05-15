using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SflStucco.Site.Helpers
{
    public class EmailHelper
    {
        /// <summary>
        /// Default SMTP Server
        /// </summary>
        public static string DefaultSMTPServer { get { return ConfigurationManager.AppSettings["SMTP_SERVER"]; } }

        /// <summary>
        /// Default SMTP Port
        /// </summary>
        public static int DefaultSMTPPort { get { return Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_SERVER_PORT"]); } }

        /// <summary>
        /// Default Email Sender
        /// </summary>
        public static string DefaultSender { get { return "donotreply@sflstucco.com"; } }

        /// <summary>
        /// Default Username for smtp server
        /// </summary>
        public static string DefaultUsername { get { return ConfigurationManager.AppSettings["SMTP_SERVER_USER"]; } }

        /// <summary>
        /// Default password for smtp server
        /// </summary>
        public static string DefaultPassword { get { return ConfigurationManager.AppSettings["SMTP_SERVER_PASS"]; } }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="body">The body content of the email</param>
        /// <param name="sender"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="toAddress">A collection of recipient address for the email</param>
        /// <returns>True if success, false otherwise</returns>
        public static void SendEmail(string subject,
                                            string body,
                                            string sender,
                                            bool isBodyHtml,
                                            params string[] toAddress)
        {
            SendEmail(DefaultSMTPServer,
                      DefaultSMTPPort,
                      DefaultUsername,
                      DefaultPassword,
                      sender,
                      subject,
                      body,
                      isBodyHtml,
                      toAddress);
        }

        /// <summary>
        /// Sends an email with using a default configuration.
        /// </summary>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="body">The body content of the email</param>
        /// <param name="sender"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="toAddress">A collection of recipient address for the email</param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="smtpServer"></param>
        /// <param name="smtpPort"></param>
        /// <returns>True if success, false otherwise</returns>
        public static void SendDefaultEmail(string subject
            , string body
            , string smtpServer = ""
            , int smtpPort = 0
            , string userName = ""
            , string password = ""
            , string sender = ""
            ,bool isBodyHtml = false 
            ,params string[] toAddress)
        {
            smtpServer = string.IsNullOrEmpty(smtpServer) ? DefaultSMTPServer : smtpServer;
            smtpPort = smtpPort == 0 ? DefaultSMTPPort : smtpPort;
            userName = string.IsNullOrEmpty(userName) ? DefaultUsername : userName;
            password = string.IsNullOrEmpty(password) ? DefaultPassword : password;
            sender = string.IsNullOrEmpty(sender) ? DefaultSender : sender;

            SendEmail(smtpServer,
                      smtpPort,
                      userName,
                      password,
                      sender,
                      subject,
                      body,
                      false,
                      toAddress);
        }

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="smtpServer">The smtp server to used to send the email</param>
        /// <param name="port">The port used to connect to the smtp server</param>
        /// <param name="smtpPassword">The password used to authenticate to the server</param>
        /// <param name="fromAddress">The sender of the email</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="body">The body content of the email</param>
        /// <param name="isBodyHtml">Flag indicating if the body of the email is html</param>
        /// <param name="toAddress">A collection of recipient address for the email</param>
        /// <param name="smtpUsername">The username used to authenticate to the server</param>
        /// <returns>True if success, false otherwise</returns>
        public static void SendEmail(string smtpServer,
                                     int port,
                                     string smtpUsername,
                                     string smtpPassword,
                                     string fromAddress,
                                     string subject,
                                     string body,
                                     bool isBodyHtml,
                                     params string[] toAddress)
        {
            var msg = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Body = body,
                Subject = subject,
                IsBodyHtml = isBodyHtml,
            };

            if (toAddress != null)
            {
                foreach (string recipient in toAddress)
                {
                    msg.To.Add(new MailAddress(recipient));
                }
            }

            SmtpClient client = new SmtpClient();

            string environment = ConfigurationManager.AppSettings["Environment"];

            if (environment.Equals("DEV"))
            {
                client = new SmtpClient(smtpServer, port)
                {
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    EnableSsl = true
                };
            }
            else
            {
                client.Host = smtpServer;   //-- Donot change.
                client.Port = port;
                client.EnableSsl = false;//--- Donot change
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            }

            client.Send(msg);
        }
    }
}