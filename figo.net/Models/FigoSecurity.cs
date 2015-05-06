using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace figo
{
    [JsonObject]
    public class FigoSecurity
    {


        /// <summary>
        /// Internal figo Connect account ID
        /// </summary>
        [JsonProperty("account_id")]
        public string AccountId { get; set; }
        public bool ShouldSerializeAccountId() { return false; }


        /// <summary>
        /// Transaction amount
        /// </summary>
        [JsonProperty("amount")]
        public float Amount { get; set; }
        public bool ShouldSerializeAmount() { return false; }


        /// <summary>
        /// Transaction amount
        /// </summary>
        [JsonProperty("amount_original_currency")]
        public string AmountOriginalCurrency { get; set; }
        public bool ShouldSerializeAmountOriginalCurrency() { return false; }





        /// <summary>
        /// Booking date
        /// </summary>
        [JsonProperty("creation_timestamp")]
        public DateTime CreationTimestamp { get; set; }
        public bool ShouldSerializeCreationTimestamp() { return false; }




        /// <summary>
        /// Three-character currency code
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }
        public bool ShouldSerializeCurrency() { return false; }





        /// <summary>
        /// Booking text
        /// </summary>
        [JsonProperty("isin")]
        public string Isin { get; set; }
        public bool ShouldSerializeIsin() { return false; }


        /// <summary>
        /// Booking text
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        public bool ShouldSerializeName() { return false; }



        /// <summary>
        /// Transaction amount
        /// </summary>
        [JsonProperty("price")]
        public float Price { get; set; }
        public bool ShouldSerializePrice() { return false; }

        /// <summary>
        /// Transaction amount
        /// </summary>
        [JsonProperty("price_currency")]
        public string PriceCurrency { get; set; }
        public bool ShouldSerializePriceCurrency() { return false; }



        /// <summary>
        /// Transaction amount
        /// </summary>
        [JsonProperty("purchase_price")]
        public float PurchasePrice { get; set; }
        public bool ShouldSerializePurchasePrice() { return false; }

        /// <summary>
        /// Transaction amount
        /// </summary>
        [JsonProperty("purchase_price_currency")]
        public string PurchasePriceCurrency { get; set; }
        public bool ShouldSerializePurchasePriceCurrency() { return false; }


        /// <summary>
        /// Transaction amount
        /// </summary>
        [JsonProperty("quantity")]
        public float Quantity { get; set; }
        public bool ShouldSerializeQuantity() { return false; }


        /// <summary>
        /// Transaction amount
        /// </summary>
        [JsonProperty("security_id")]
        public string SecurityId { get; set; }
        public bool ShouldSerializeSecurityId() { return false; }


        /// <summary>
        /// Transaction amount
        /// </summary>
        [JsonProperty("wkn")]
        public string Wkn { get; set; }
        public bool ShouldSerializeWkn() { return false; }


        /// <summary>
        /// Helper type to represent the actual answer from the figo API
        /// </summary>
        [JsonObject]
        internal class SecurityResponse
        {
            /// <summary>
            /// List of transactions asked for
            /// </summary>
            [JsonProperty("securities")]
            public List<FigoSecurity> Securities { get; set; }

            /// <summary>
            /// Synchronization status between figo and bank servers
            /// </summary>
            [JsonProperty("status")]
            public FigoSynchronizationStatus Status { get; set; }
        }


    }
}