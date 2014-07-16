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
        public bool ShouldSerializeTransactionId() { return false; }

        /// <summary>
        /// Internal figo Connect account ID
        /// </summary>
        [JsonProperty("account_id")]
        public string AccountId { get; set; }
        public bool ShouldSerializeAccountId() { return false; }

        /// <summary>
        /// Name of originator or recipient
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        public bool ShouldSerializeName() { return false; }

        /// <summary>
        /// Account number of originator or recipient
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }
        public bool ShouldSerializeAccountNumber() { return false; }

        /// <summary>
        /// Bank code of originator or recipient
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }
        public bool ShouldSerializeBankCode() { return false; }

        /// <summary>
        /// Bank name of originator or recipient
        /// </summary>
        [JsonProperty("bank_name")]
        public string BankName { get; set; }
        public bool ShouldSerializeBankName() { return false; }

        /// <summary>
        /// Transaction amount
        /// </summary>
        [JsonProperty("amount")]
        public float Amount { get; set; }
        public bool ShouldSerializeAmount() { return false; }

        /// <summary>
        /// Three-character currency code
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }
        public bool ShouldSerializeCurrency() { return false; }

        /// <summary>
        /// Booking date
        /// </summary>
        [JsonProperty("booking_date")]
        public DateTime BookingDate { get; set; }
        public bool ShouldSerializeBookingDate() { return false; }

        /// <summary>
        /// Value date
        /// </summary>
        [JsonProperty("value_date")]
        public DateTime ValueDate { get; set; }
        public bool ShouldSerializeValueDate() { return false; }

        /// <summary>
        /// Purpose text
        /// </summary>
        [JsonProperty("purpose")]
        public string Purpose { get; set; }
        public bool ShouldSerializePurpose() { return false; }

        /// <summary>
        /// Transaction type: Transfer, Standing order, Direct debit, Salary or rent, Electronic cash, GeldKarte, ATM, Charges or interest or Unknown
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        public bool ShouldSerializeType() { return false; }

        /// <summary>
        /// Booking text
        /// </summary>
        [JsonProperty("booking_text")]
        public string BookingText { get; set; }
        public bool ShouldSerializeBookingText() { return false; }

        /// <summary>
        /// This flag indicates whether the transaction is booked or pending
        /// </summary>
        [JsonProperty("booked")]
        public bool IsBooked { get; set; }
        public bool ShouldSerializeIsBooked() { return false; }

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
