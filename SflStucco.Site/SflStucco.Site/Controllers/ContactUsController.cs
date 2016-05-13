using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SflStucco.Site.Models;

namespace SflStucco.Site.Controllers
{
    public class ContactUsController : ApplicationController
    {
        // GET: ContactUs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendEmail(ContactUsModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Email) ||
                    string.IsNullOrWhiteSpace(model.Name) ||
                    string.IsNullOrWhiteSpace(model.Message))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                return PartialView("~/Views/ContactUs/Partials/_ContactUsFormBody.cshtml", model);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}