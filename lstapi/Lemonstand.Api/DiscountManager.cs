using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Lemonstand.Domain.Base;
using Lemonstand.Domain.Entities;
using Newtonsoft.Json;

namespace Lemonstand.Api
{
    public class DiscountManager
    {
        protected void CreateCertificate()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
        }

        public Discount GetDiscount(int discountId)
        {
            CreateCertificate();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = LemonstandEnvironment.GetAuthenticationHeaderValue();

                var data = client.GetAsync(LemonstandEnvironment.GetUrl(string.Format("discount/{0}", discountId))).Result;

                string json = data.Content.ReadAsStringAsync().Result;

                LemonstandResponseObject discount = JsonConvert.DeserializeObject<LemonstandResponseObject>(json);

                return discount.DataObject as Discount;
            }
        }

        public void UpdateDiscount(Discount discount)
        {
            CreateCertificate();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = LemonstandEnvironment.GetAuthenticationHeaderValue();

                HttpContent content = new StringContent(JsonConvert.SerializeObject(discount), Encoding.UTF8, "application/json");

                string url = LemonstandEnvironment.GetUrl(string.Format("discount/{0}", discount.Id));

                Uri patchUri = new Uri(url);

                var data = client.PatchSync(patchUri, content);

                if (!data.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException(data.ReasonPhrase);
                }
            }
        }
    }
} 
