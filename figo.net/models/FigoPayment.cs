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
        public String PaymentId { get; set; }
        public bool ShouldSerializePaymentId() { return false; }

        /// <summary>
        /// Internal figo Connect account ID
        /// </summary>
        [JsonProperty("account_id")]
        public String AccountId { get; set; }
        public bool ShouldSerializeAccountId() { return false; }

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
        public bool ShouldSerializeBankName() { return false; }

        /// <summary>
        /// Icon of creditor or debitor bank
        /// </summary>
        [JsonProperty("bank_icon")]
        public string BankIcon { get; set; }
        public bool ShouldSerializeBankIcon() { return false; }

        /// <summary>
        /// Icon of the creditor or debtor bank in other resolutions
        /// </summary>
        [JsonProperty("bank_additional_icons")]
        public Dictionary<String, String> BankAdditionalIcons { get; set; }
        public bool ShouldSerializeBankAdditionalIcons() { return false; }

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
        /// Timestamp of submission to the bank server
        /// </summary>
        [JsonProperty("submission_timestamp")]
        public DateTime? SubmissionTimestamp { get; set; }
        public bool ShouldSerializeSubmissionTimestamp() { return false; }

        /// <summary>
        /// Internal creation timestamp on the figo Connect server
        /// </summary>
        [JsonProperty("creation_timestamp")]
        public DateTime CreationTimestamp { get; set; }
        public bool ShouldSerializeCreationTimestamp() { return false; }

        /// <summary>
        /// Internal modification timestamp on the figo Connect server
        /// </summary>
        [JsonProperty("modification_timestamp")]
        public DateTime ModificationTimestmap { get; set; }
        public bool ShouldSerializeNotificationTimestamp() { return false; }

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
