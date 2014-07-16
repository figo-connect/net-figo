using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace figo {
    /// <summary>
    /// Object representing the balance of a certain bank account of the user
    /// </summary>
    [JsonObject]
    public class FigoAccountBalance {
        /// <summary>
        /// Account balance or null if the balance is not yet known
        /// </summary>
        [JsonProperty("balance")]
        public float Balance { get; set; }
        public bool ShouldSerializeBalance() { return false; }

        /// <summary>
        /// Bank server timestamp of balance or null if the balance is not yet known.
        /// </summary>
        [JsonProperty("balance_date")]
        public DateTime BalanceDate { get; set; }
        public bool ShouldSerializeBalanceDate() { return false; }

        /// <summary>
        /// Credit line
        /// </summary>
        [JsonProperty("credit_line")]
        public float CreditLine { get; set; }

        /// <summary>
        /// User-defined spending limit
        /// </summary>
        [JsonProperty("monthly_spending_limit")]
        public float MonthlySpendingLimit { get; set; }
    }
}
