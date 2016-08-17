using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace figo {
    [JsonObject]
    public class FigoUser {
        /// <summary>
        /// Internal figo Connect user ID
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        public bool ShouldSerializeUserId() { return false; }

        /// <summary>
        /// First and last name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
        public bool ShouldSerializeEmail() { return false; }

        /// <summary>
        /// Two-letter code of preferred language
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Postal address for bills, etc.
        /// </summary>
        [JsonProperty("address")]
        public Dictionary<String, String> Address { get; set; }

        /// <summary>
        /// This flag indicates whether the email address has been verified
        /// </summary>
        [JsonProperty("verified_email")]
        public bool IsEmailVerified { get; set; }
        public bool ShouldSerializeIsEmailVerified() { return false; }

        /// <summary>
        /// This flag indicates whether the user has agreed to be contacted by email
        /// </summary>
        [JsonProperty("send_newsletter")]
        public bool ShouldSendNewsletter { get; set; }

        /// <summary>
        /// This flag indicates whether the figo Account plan is free or premium
        /// </summary>
        [Obsolete("Deprecated.", true)]
        public bool IsPremium { get; set; }
        public bool ShouldSerializeIsPremium() { return false; }

        /// <summary>
        /// Timestamp of premium figo Account expiry
        /// </summary>
        [Obsolete("Deprecated.", true)]
        public DateTime PremiumExpiresOn { get; set; }
        public bool ShouldSerializePremiumExpiresOn() { return false; }

        /// <summary>
        /// Provider for premium subscription or Null of no subscription is active
        /// </summary>
        [Obsolete("Deprecated.", true)]
        public String PremiumSubscription { get; set; }
        public bool ShouldSerializePremiumSubscription() { return false; }

        /// <summary>
        /// Timestamp of figo Account registration
        /// </summary>
        [JsonProperty("join_date")]
        public DateTime JoinDate { get; set; }
        public bool ShouldSerializeJoinDate() { return false; }
    }
}
