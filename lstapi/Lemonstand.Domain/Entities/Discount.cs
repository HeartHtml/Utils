using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lemonstand.Domain.Base;
using Lemonstand.Domain.Interfaces;
using Newtonsoft.Json;

namespace Lemonstand.Domain.Entities
{
    public class Discount : ILemonstandDataObject
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string SubtotalDiscount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("is_active")]
        public int IsActive { get; set; }

        [JsonProperty("coupon_codes")]
        public string CouponCode { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("end_date")]
        public string EndDate { get; set; }

        [JsonProperty("max_uses_per_coupon")]
        public int? MaxUsesPerCoupon { get; set; }

        [JsonProperty("max_uses_per_customer")]
        public int? MaxUsesPerCustomer { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }
}
