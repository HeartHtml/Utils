using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using Lemonstand.Api;
using Lemonstand.Domain.Entities;
using UtilsLib.ExtensionMethods;

namespace lst.Controllers
{
    public class DiscountController : ApiController
    {
        private readonly DiscountManager _discountManager = new DiscountManager();

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public ActionResult IncrementNewbieDiscount([FromUri] string secret)
        {
            try
            {
                string apiSecret = ConfigurationManager.AppSettings["ApiSecret"];

                if (apiSecret.SafeEquals(secret))
                {
                    int newbieDiscountId = Convert.ToInt32(ConfigurationManager.AppSettings["NewbieDiscountId"]);

                    Discount discount = _discountManager.GetDiscount(newbieDiscountId);

                    if (discount == null)
                    {
                        throw new NoNullAllowedException("Discount was null");
                    }

                    int couponIncrement = Convert.ToInt32(ConfigurationManager.AppSettings["CouponIncrement"]);

                    discount.MaxUsesPerCoupon += couponIncrement;

                    _discountManager.UpdateDiscount(discount);

                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }

                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
