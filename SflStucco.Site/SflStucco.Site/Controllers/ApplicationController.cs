using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SflStucco.Site.Controllers
{
    public class ApplicationController : Controller
    {
        // GET: Application
        public string SMTP_DESTINATION_EMAIL
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTP_DESTINATION_EMAIL"];
            }
        }
    }
}