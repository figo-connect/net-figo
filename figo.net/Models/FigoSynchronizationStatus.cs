using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace figo {
    /// <summary>
    /// Represents the status of the synchonisation between figo and the bank servers
    /// </summary>
    [JsonObject]
    public class FigoSynchronizationStatus {
        /// <summary>
        /// Internal figo status code
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }
        public bool ShouldSerializeCode() { return false; }

        /// <summary>
        /// Human-readable error message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
        public bool ShouldSerializeMessage() { return false; }

        /// <summary>
        /// Timestamp of last synchronization
        /// </summary>
        [JsonProperty("sync_timestamp")]
        public DateTime SyncTimestamp { get; set; }
        public bool ShouldSerializeSyncTimestamp() { return false; }

        /// <summary>
        /// Timestamp of last successful synchronization
        /// </summary>
        [JsonProperty("success_timestamp")]
        public DateTime SuccessTimestamp { get; set; }
        public bool ShouldSerializeSuccessTimestamp() { return false; }
    }
}
