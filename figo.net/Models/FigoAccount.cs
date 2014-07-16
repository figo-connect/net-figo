using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace figo {
    /// <summary>
    /// Object representing one bank account of the user, independent of the exact account type
    /// </summary>
    [JsonObject]
    public class FigoAccount {
        /// <summary>
        /// Internal figo Connect account ID
        /// </summary>
        [JsonProperty("account_id")]
        public string AccountId { get; set; }
        public bool ShouldSerializeAccountId() { return false; }

        /// <summary>
        /// Internal figo Connect bank ID
        /// </summary>
        [JsonProperty("bank_id")]
        public string BankId { get; set; }
        public bool ShouldSerializeBankId() { return false; }

        /// <summary>
        /// Account name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Account owner
        /// </summary>
        [JsonProperty("owner")]
        public string Owner { get; set; }

        /// <summary>
        /// This flag indicates whether the account will be automatically synchronized
        /// </summary>
        [JsonProperty("auto_sync")]
        public bool IsAutoSync { get; set; }

        /// <summary>
        /// Account number
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }
        public bool ShouldSerializeAccountNumber() { return false; }

        /// <summary>
        /// Bank code
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }
        public bool ShouldSerializeBankCode() { return false; }

        /// <summary>
        /// Bank name
        /// </summary>
        [JsonProperty("bank_name")]
        public string BankName { get; set; }
        public bool ShouldSerializeBankName() { return false; }

        /// <summary>
        /// Three-character currency code
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }
        public bool ShouldSerializeCurrency() { return false; }

        /// <summary>
        /// IBAN
        /// </summary>
        [JsonProperty("iban")]
        public string IBAN { get; set; }
        public bool ShouldSerializeIBAN() { return false; }

        /// <summary>
        /// BIC
        /// </summary>
        [JsonProperty("bic")]
        public string BIC { get; set; }
        public bool ShouldSerializeBIC() { return false; }

        /// <summary>
        /// Account type: Giro account, Savings account, Credit card, Loan account, PayPal or Unknown
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        public bool ShouldSerializeType() { return false; }

        /// <summary>
        /// Account icon URL
        /// </summary>
        [JsonProperty("icon")]
        public string IconUrl { get; set; }
        public bool ShouldSerializeIconUrl() { return false; }

        /// <summary>
        /// Account icon in other resolutions
        /// </summary>
        [JsonProperty("additional_icons")]
        public Dictionary<String, String> AdditionalIcons { get; set; }
        public bool ShouldSerializeAdditionalIcons() { return false; }

        /// <summary>
        /// Balance details
        /// </summary>
        [JsonProperty("balance")]
        public FigoAccountBalance Balance { get; set; }

        /// <summary>
        /// Synchronization details
        /// </summary>
        [JsonProperty("status")]
        public FigoSynchronizationStatus Status { get; set; }

        /// <summary>
        /// Helper type to match actual response from figo API
        /// </summary>
        [JsonObject]
        internal class FigoAccountsResponse {
            /// <summary>
            /// List of accounts asked for
            /// </summary>
            [JsonProperty("accounts")]
            public List<FigoAccount> Accounts { get; set; }
        }
    }
}
