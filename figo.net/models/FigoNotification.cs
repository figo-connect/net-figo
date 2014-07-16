using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace figo {
    /// <summary>
    /// Object representing a configured notification, e.g a webhook or email hook
    /// </summary>
    [JsonObject]
    public class FigoNotification {
        /// <summary>
        /// Internal figo Connect notification ID from the notification registration response
        /// </summary>
        [JsonProperty("notification_id")]
        public String NotificationId { get; set; }
        public bool ShouldSerializeNotificationId() { return false; }

        /// <summary>
        /// Notification key: see http://docs.figo.io/#notification-keys
        /// </summary>
        [JsonProperty("observe_key")]
        public String ObserveKey { get; set; }

        /// <summary>
        /// Notification messages will be sent to this URL
        /// </summary>
        [JsonProperty("notify_uri")]
        public String NotifyURI { get; set; }

        /// <summary>
        /// State similar to sync and login process. It will passed as POST payload for webhooks
        /// </summary>
        [JsonProperty("state")]
        public String State { get; set; }

        /// <summary>
        /// Helper type to match actual response from figo API
        /// </summary>
        internal class NotificationsResponse {
            /// <summary>
            /// List of notifications asked for
            /// </summary>
            [JsonProperty("notifications")]
            public List<FigoNotification> Notifications { get; set; }
        }
    }
}
