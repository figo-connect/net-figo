using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace figo {
    /// <summary>
    /// Object representing one bank transaction on a certain bank account of the user
    /// </summary>
    [JsonObject]
    public class FigoTransaction {
        /// <summary>
        /// Internal figo Connect transaction ID
        /// </summary>
        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        /// <summary>
        /// Internal figo Connect account ID
        /// </summary>
        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        /// <summary>
        /// Name of originator or recipient
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Account number of originator or recipient
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Bank code of originator or recipient
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        /// <summary>
        /// Bank name of originator or recipient
        /// </summary>
        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        /// <summary>
        /// Transaction amount
        /// </summary>
        [JsonProperty("amount")]
        public float Amount { get; set; }

        /// <summary>
        /// Three-character currency code
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Booking date
        /// </summary>
        [JsonProperty("booking_date")]
        public DateTime BookingDate { get; set; }

        /// <summary>
        /// Value date
        /// </summary>
        [JsonProperty("value_date")]
        public DateTime ValueDate { get; set; }

        /// <summary>
        /// Purpose text
        /// </summary>
        [JsonProperty("purpose")]
        public string PurposeText { get; set; }

        /// <summary>
        /// Transaction type: Transfer, Standing order, Direct debit, Salary or rent, Electronic cash, GeldKarte, ATM, Charges or interest or Unknown
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Booking text
        /// </summary>
        [JsonProperty("booking_text")]
        public string BookingText { get; set; }

        /// <summary>
        /// This flag indicates whether the transaction is booked or pending
        /// </summary>
        [JsonProperty("booked")]
        public bool isBooked { get; set; }

        /// <summary>
        /// Helper type to represent the actual answer from the figo API
        /// </summary>
        [JsonObject]
        internal class TransactionsResponse {
            /// <summary>
            /// List of transactions asked for
            /// </summary>
            [JsonProperty("transactions")]
            public List<FigoTransaction> Transactions { get; set; }

            /// <summary>
            /// Synchronization status between figo and bank servers
            /// </summary>
            [JsonProperty("status")]
            public FigoSynchronizationStatus Status { get; set; }
        }
    }
}
