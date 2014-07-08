using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace figo {
    [JsonObject]
    public class TokenResponse {
        /// <summary>
        /// The access token
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// A refresh token that may be used to request new access tokens. Refresh tokens remais valid until the user revokes access to your application. This response parameter is only present if the permission offline has been requested in the authorization code request.
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// The remaining live time of the access token in seconds
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// A space delimited set of requested permissions
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Which kind of authenitcation scheme this token can be used with. Should always be Bearer.
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}
