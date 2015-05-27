using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace figo
{
    [JsonObject]
    public class FigoSecurity
    {


        
        [JsonProperty("account_id")]
        public string AccountId { get; set; }
        public bool ShouldSerializeAccountId() { return false; }


      
        [JsonProperty("amount")]
        public float Amount { get; set; }
        public bool ShouldSerializeAmount() { return false; }


        
        [JsonProperty("amount_original_currency")]
        public string AmountOriginalCurrency { get; set; }
        public bool ShouldSerializeAmountOriginalCurrency() { return false; }





      
        [JsonProperty("creation_timestamp")]
        public DateTime CreationTimestamp { get; set; }
        public bool ShouldSerializeCreationTimestamp() { return false; }




     
        [JsonProperty("currency")]
        public string Currency { get; set; }
        public bool ShouldSerializeCurrency() { return false; }





      
        [JsonProperty("isin")]
        public string Isin { get; set; }
        public bool ShouldSerializeIsin() { return false; }


      
        [JsonProperty("name")]
        public string Name { get; set; }
        public bool ShouldSerializeName() { return false; }



       
        [JsonProperty("price")]
        public float Price { get; set; }
        public bool ShouldSerializePrice() { return false; }

       
        [JsonProperty("price_currency")]
        public string PriceCurrency { get; set; }
        public bool ShouldSerializePriceCurrency() { return false; }



      
        [JsonProperty("purchase_price")]
        public float PurchasePrice { get; set; }
        public bool ShouldSerializePurchasePrice() { return false; }

      
        [JsonProperty("purchase_price_currency")]
        public string PurchasePriceCurrency { get; set; }
        public bool ShouldSerializePurchasePriceCurrency() { return false; }


      
        [JsonProperty("quantity")]
        public float Quantity { get; set; }
        public bool ShouldSerializeQuantity() { return false; }



        [JsonProperty("security_id")]
        public string SecurityId { get; set; }
        public bool ShouldSerializeSecurityId() { return false; }



        [JsonProperty("wkn")]
        public string Wkn { get; set; }
        public bool ShouldSerializeWkn() { return false; }


        [JsonProperty("exchange_rate")]
        public double ExchangeRate { get; set; }
        public bool ShouldSerializeExchangeRate() { return false; }


        [JsonProperty("market")]
        public string Market { get; set; }
        public bool ShouldSerializeMarket() { return false; }


        [JsonProperty("modification_timestamp")]
        public DateTime ModificationTimestamp { get; set; }
        public bool ShouldSerializeModificationTimestamp() { return false; }



        [JsonProperty("trade_timestamp")]
        public DateTime TradeTimestamp { get; set; }
        public bool ShouldSerializeTradeTimestamp() { return false; }


        [JsonProperty("visited")]
        public bool Visited { get; set; }
        public bool ShouldSerializeVisited() { return false; }
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