using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace figo {
    /// <summary>
    /// Object representing a payment the user made via figo
    /// </summary>
    [JsonObject]
    public class FigoPayment {
        /// <summary>
        /// Internal figo Connect payment ID
        /// </summary>
        [JsonProperty("payment_id")]
        public String PaymentID { get; set; }

        /// <summary>
        /// Internal figo Connect account ID
        /// </summary>
        [JsonProperty("account_id")]
        public String AccountID { get; set; }

        /// <summary>
        /// Payment type
        /// </summary>
        [JsonProperty("type")]
        public String Type { get; set; }

        /// <summary>
        /// Name of creditor or debitor
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Account number of creditor or debitor
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Bank code of creditor or debitor
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        /// <summary>
        /// Bank name of creditor or debitor
        /// </summary>
        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        /// <summary>
        /// Icon of creditor or debitor bank
        /// </summary>
        [JsonProperty("bank_icon")]
        public string BankIcon { get; set; }

        /// <summary>
        /// Order amount
        /// </summary>
        [JsonProperty("amount")]
        public float Amount { get; set; }

        /// <summary>
        /// Three-character currency code
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Purpose text
        /// </summary>
        [JsonProperty("purpose")]
        public string Purpose { get; set; }

        /// <summary>
        /// DTA text key
        /// </summary>
        [JsonProperty("text_key")]
        public int TextKey { get; set; }

        /// <summary>
        /// DTA text key extension
        /// </summary>
        [JsonProperty("text_key_extension")]
        public int TextkeyExtension { get; set; }

        /// <summary>
        /// Timestamp of submission to the bank server
        /// </summary>
        [JsonProperty("submission_timestamp")]
        public DateTime SubmissionTimestamp { get; set; }

        /// <summary>
        /// Internal creation timestamp on the figo Connect server
        /// </summary>
        [JsonProperty("creation_timestamp")]
        public DateTime CreationTimestamp { get; set; }

        /// <summary>
        /// Internal modification timestamp on the figo Connect server
        /// </summary>
        [JsonProperty("modification_timestamp")]
        public DateTime ModificationTimestmap { get; set; }

        /// <summary>
        /// Helper type to match actual response from figo API
        /// </summary>
        internal class PaymentsResponse {
            /// <summary>
            /// List of payments asked for
            /// </summary>
            [JsonProperty("payments")]
            public List<FigoPayment> Payments { get; set; }
        }
    }
}
