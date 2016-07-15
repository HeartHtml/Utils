using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Lemonstand.Api
{
    public static class LemonstandEnvironment
    {
        public static string BaseEndpointUrl
        {
            get
            {
                string url = ConfigurationManager.AppSettings["EndpointUrl"];

                return url;
            }
        }

        public static string AccessToken
        {
            get
            {
                string token = ConfigurationManager.AppSettings["AccessToken"];

                return token;
            }
        }

        public static AuthenticationHeaderValue GetAuthenticationHeaderValue()
        {
            return new AuthenticationHeaderValue("Bearer", AccessToken);
        }


        public static string GetUrl(string entity)
        {
            return string.Format("{0}/{1}", BaseEndpointUrl, entity);
        }
    }
}
