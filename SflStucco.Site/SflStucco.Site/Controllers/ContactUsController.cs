using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SflStucco.Site.Helpers;
using SflStucco.Site.Models;

namespace SflStucco.Site.Controllers
{
    public class ContactUsController : ApplicationController
    {
        // GET: ContactUs
        public ActionResult Index()
        {
            ContactUsModel model = new ContactUsModel();

            return View(model);
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

                StringBuilder builder = new StringBuilder();

                builder.AppendHtmlLine("Hi,");

                builder.AppendHtmlLine(string.Empty);

                builder.AppendHtmlLine(string.Format("Name: {0}", model.Name));

                if (!model.Address.IsNullOrWhiteSpace())
                {
                    builder.AppendHtmlLine(string.Format("Address: {0}", model.Address));
                }

                if (!model.Subject.IsNullOrWhiteSpace())
                {
                    builder.AppendHtmlLine(string.Format("Subject: {0}", model.Subject));
                }

                builder.AppendHtmlLine(string.Format("Email: {0}", model.Email)); 

                builder.AppendHtmlLine(string.Empty);

                builder.AppendHtmlLine(string.Format("Message: {0}", model.Message));

                string body = builder.ToString();

                EmailHelper.SendEmail("Web Contact Request", 
                                       body,
                                       model.Email, 
                                       true,
                                       SMTP_DESTINATION_EMAIL);

                model.IsEmailSent = true;

                return Json(model);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}