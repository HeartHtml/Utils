using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SflStucco.Site.Models;

namespace SflStucco.Site.Controllers
{
    public class HomeController : ApplicationController
    {
        public ActionResult Index()
        {
            ContactUsModel model = new ContactUsModel();

            return View("~/Views/Home/Index.cshtml", model);
        }
    }
}