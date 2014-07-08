using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace figo
{
    [JsonObject]
    internal class AuthenticationCodeTokenRequest {
        [JsonProperty("grant_type")]
        public string GrantType { get { return "authorization_code"; } }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }
    }

    [JsonObject]
    internal class RefreshTokenTokenRequest {
        [JsonProperty("grant_type")]
        public string GrantType { get { return "refresh_token"; } }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }

    [JsonObject]
    internal class PasswordTokenRequest {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("device_udid")]
        public string DeviceUdid { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
