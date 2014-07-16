using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace figo.models {
    /// <summary>
    /// Object representing a Bank
    /// </summary>
    [JsonObject]
    public class FigoBank {
        /// <summary>
        /// Internal figo Connect bank ID
        /// </summary>
        [JsonProperty("bank_id")]
        public string BankId { get; set; }
        public bool ShouldSerializeBankId() { return false; }

        /// <summary>
        /// SEPA direct debit creditor ID
        /// </summary>
        [JsonProperty("sepa_creditor_id")]
        public string SepaCreditorId { get; set; }

        /// <summary>
        /// This flag indicates whether the user has chosen to save the PIN on the figo Connect server
        /// </summary>
        [JsonProperty("save_pin")]
        public Boolean isPinSaved { get; set; }
    }
}
