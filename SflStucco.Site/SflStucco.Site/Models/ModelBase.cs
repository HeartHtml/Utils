using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SflStucco.Site.Models
{
    public class ModelBase
    {
        public bool IsSelected { get; set; }

        protected const string EmailRegex = @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$";

        protected const string PhoneRegex = @"^([0-9]( |-)?)?(\(?[0-9]{3}\)?|[0-9]{3})( |-)?([0-9]{3}( |-)?[0-9]{4}|[a-zA-Z0-9]{7})$";

        protected const string ZipCodeRegex = @"^\d{5}-?(\d{4})?$";
    }
}