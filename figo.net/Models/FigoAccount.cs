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

        /// <summary>
        /// Internal figo Connect bank ID
        /// </summary>
        [JsonProperty("bank_id")]
        public string BankId { get; set; }

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
        public bool isAutoSync { get; set; }

        /// <summary>
        /// Account number
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Bank code
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        /// <summary>
        /// Bank name
        /// </summary>
        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        /// <summary>
        /// Three-character currency code
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// IBAN
        /// </summary>
        [JsonProperty("iban")]
        public string IBAN { get; set; }

        /// <summary>
        /// BIC
        /// </summary>
        [JsonProperty("bic")]
        public string BIC { get; set; }

        /// <summary>
        /// Account type: Giro account, Savings account, Credit card, Loan account, PayPal or Unknown
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Account icon URL
        /// </summary>
        [JsonProperty("icon")]
        public string IconUrl { get; set; }

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
