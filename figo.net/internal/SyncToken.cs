using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace figo {
    [JsonObject]
    class SyncTokenRequest {
        /// <summary>
        /// Any kind of string that will be forwarded in the callback response message. It serves two purposes: The value is used to maintain state between this request and the callback, e.g. it might contain a session ID from your application. The value should also contain a random component, which your application checks to mitigate cross-site request forgery.
        /// </summary>
        [JsonProperty("state")]
        public String State { get; set; }

        /// <summary>
        /// At the end of the synchronization process a response will be sent to this callback URL
        /// </summary>
        [JsonProperty("redirect_uri")]
        public String RedirectURI { get; set; }
    }

    [JsonObject]
    class TaskTokenResponse {
        /// <summary>
        /// Task token
        /// </summary>
        [JsonProperty("task_token")]
        public String TaskToken { get; set; }
    }
}
