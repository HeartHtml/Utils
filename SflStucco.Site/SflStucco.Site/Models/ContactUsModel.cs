using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SflStucco.Site.Helpers;

namespace SflStucco.Site.Models
{
    public class ContactUsModel : ModelBase
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [RegularExpression(EmailRegex, ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }

        public bool IsEmailSent { get; set; }

        public bool HasError { get; set; }
    }
}