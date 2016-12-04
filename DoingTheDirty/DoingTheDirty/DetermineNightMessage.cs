using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DoingTheDirty
{
    public class DetermineNightMessage
    {

        private string Carrier   
        {
            get
            {
                return ConfigurationManager.AppSettings["Carrier"];
            }
        }

        private string PhoneNumber
        {
            get
            {
                return ConfigurationManager.AppSettings["PhoneNumber"];
            }
        }

        private string FromAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["FromAddress"];
            }
        }

        private string SmtpServer
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpServer"];
            }
        }

        private string SmtpPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpPassword"];
            }
        }

        private int SmtpPort
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
            }
        }

        private string RandomOrgEndpoint
        {
            get
            {
                return ConfigurationManager.AppSettings["RandomOrgEndpoint"];
            }
        }

        private int SexThreshold
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["SexThreshold"]);
            }
        }

        private int AnalThreshold
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["AnalThreshold"]);
            }
        }

        public void Send()
        {
            try
            {
                var httpClient = new HttpClient();

                var requestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri(RandomOrgEndpoint),
                    Method = new HttpMethod("POST"),
                };

                RandomNumberRequest request = new RandomNumberRequest
                {
                    Id = 12546,
                    Method = "generateIntegers",
                    JsonRpc = "2.0",
                    Params = new RandomNumberRequestParams
                    {
                        ApiKey = "00000000-0000-0000-0000-000000000000",
                        Base = 10,
                        Max = 1000,
                        Min = -1000,
                        N = 1,
                        Replacement = true
                    }
                };

                string requestContent = JsonConvert.SerializeObject(request);

                requestMessage.Content = new StringContent(requestContent);

                HttpResponseMessage responseMessage = Task.Run(() => httpClient.SendAsync(requestMessage)).Result;

                HttpContent content = responseMessage.Content;

                string randomOrgContent = Task.Run(() => content.ReadAsStringAsync()).Result;

                dynamic randomOrgResponse = JObject.Parse(randomOrgContent);

                int randomNumber = randomOrgResponse.result.random.data[0];

                string decision = "N";

                string anal = "N";

                if (randomNumber >= AnalThreshold)
                {
                    decision = "Y";
                    anal = "Y";
                }

                if (randomNumber >= SexThreshold)
                {
                    decision = "Y";
                    anal = "N";
                }

                string doTheDirtyMessage = string.Format("Number: {0} | Sex: {1} | Anal: {2}",
                    randomNumber, decision, anal);

                SmtpClient client = new SmtpClient(SmtpServer, SmtpPort)
                {
                    Credentials = new NetworkCredential(FromAddress, SmtpPassword),
                    EnableSsl = true
                };

                MailMessage message = new MailMessage
                {
                    Body = doTheDirtyMessage,
                    Subject = "Do the dirty",
                    IsBodyHtml = true,
                    From = new MailAddress(FromAddress),
                    To = { string.Format("{0}@{1}", PhoneNumber, Carrier)  }
                };

                client.Send(message);

            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
