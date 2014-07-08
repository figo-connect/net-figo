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

        /// <summary>
        /// Two-letter code of preferred language
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Timestamp of figo Account registration
        /// </summary>
        [JsonProperty("join_date")]
        public DateTime JoinDate { get; set; }
    }
}
