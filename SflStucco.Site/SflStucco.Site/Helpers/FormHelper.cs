using System;
using System.Web;

namespace SflStucco.Site.Helpers
{
    public static class FormHelper
    {
        /// <summary>
        /// Gets the base url of application
        /// </summary>
        /// <returns></returns>
        public static string GetBaseUrl()
        {
            HttpRequest request = HttpContext.Current.Request;

            var urlBase = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, (new System.Web.Mvc.UrlHelper(request.RequestContext)).Content("~"));

            return urlBase;
        }

        public static string Combine(string baseUrl, string part)
        {
            Uri baseUri = new Uri(baseUrl);

            Uri newUri = new Uri(baseUri, part);

            return newUri.AbsolutePath;
        }
    }
}