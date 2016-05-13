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
        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        [Required]
        [RegularExpression(EmailRegex, ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        public string Subject { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        public bool IsEmailSent { get; set; }

        public bool HasError { get; set; }
    }
}