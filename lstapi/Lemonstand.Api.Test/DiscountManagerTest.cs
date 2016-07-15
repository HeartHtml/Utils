using System;
using Lemonstand.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lemonstand.Api.Test
{
    [TestClass]
    public class DiscountManagerTest
    {
        private readonly DiscountManager _discountManager = new DiscountManager();

        [TestMethod]
        public void TestGet()
        {
            Discount discount = _discountManager.GetDiscount(1);

            Assert.IsNotNull(discount);
        }

        [TestMethod]
        public void TestUpdate()
        {
            Discount discount = _discountManager.GetDiscount(1);

            Assert.IsNotNull(discount);

            discount.MaxUsesPerCoupon += 5;

            _discountManager.UpdateDiscount(discount);

        }
    }
}
