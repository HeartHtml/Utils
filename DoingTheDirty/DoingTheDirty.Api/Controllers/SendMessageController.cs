using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace DoingTheDirty.Api.Controllers
{
    public class SendMessageController : ApiController
    {
        [System.Web.Http.HttpPost]
        public ActionResult Send([FromUri]string token)
        {
            string configToken = ConfigurationManager.AppSettings["ApiKey"];

            if (!token.Equals(configToken))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DetermineNightMessage nightMessage = new DetermineNightMessage();

            nightMessage.Send();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
