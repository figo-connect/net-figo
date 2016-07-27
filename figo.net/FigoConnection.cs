using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace figo {
    public class FigoConnection {
        /// <summary>
        /// figo API endpoint to use. This should only be changed when using a custom figo deployment.
        /// </summary>
        private string _apiEndoint = "https://api.figo.me";
        public string ApiEndpoint {
            get { return _apiEndoint; }
            set { _apiEndoint = value; }
        }

        /// <summary>
        /// the OAuth Client ID as provided by your figo developer contact
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// the OAuth Client Secret as provided by your figo developer contact
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// the URI the users gets redirected to after the login is finished or if he presses cancels
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// The timeout for a API request
        /// </summary>
        private int _timeout = 5000;
        public int Timeout {
            get { return _timeout; }
            set { _timeout = value; }
        }

        #region Request Handling
        protected async Task<T> DoRequest<T>(string endpoint, string method = "GET", object body = null) {
            string request_body = null;
            if(body != null)
                request_body = JsonConvert.SerializeObject(body, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            string response_body = await DoRequest(endpoint, method, request_body);
            if(response_body == null)
                return default(T);
            else
                return JsonConvert.DeserializeObject<T>(response_body);
        }

        protected async Task<String> DoRequest(string endpoint, string method = "GET", string body = null) {
            WebRequest req = (WebRequest)WebRequest.Create(ApiEndpoint + endpoint);
            req.Method = method;
            req.Headers["Authorization"] = "Basic " + (this.ClientId + ":" + this.ClientSecret).ToBase64();
            if(req is HttpWebRequest) {
                ((HttpWebRequest)req).ContinueTimeout = Timeout;
                ((HttpWebRequest)req).Accept = "application/json";
            }

            if(body != null) {
                req.ContentType = "application/json";

                using(var request_stream = await req.GetRequestStreamAsync()) {
                    byte[] bytes = Encoding.UTF8.GetBytes(body);
                    request_stream.Write(bytes, 0, bytes.Length);
                }
            }

            string result = null;
            try {
                using(WebResponse resp = await req.GetResponseAsync()) {
                    result = resp.GetResponseAsString();
                }
            } catch(WebException wexc) {
                if(wexc.Response != null) {
                    string json_error = wexc.Response.GetResponseAsString();

                    HttpStatusCode status_code = HttpStatusCode.BadRequest;
                    HttpWebResponse resp = wexc.Response as HttpWebResponse;
                    if(resp != null)
                        status_code = resp.StatusCode;

                    if((int)status_code >= 200 && (int)status_code < 300) {
                        return json_error;
                    } else if((int)status_code == 400) {
                        FigoException.ErrorResponse error_response = JsonConvert.DeserializeObject<FigoException.ErrorResponse>(json_error);
                        throw new FigoException(error_response.Error.Code.ToString(), error_response.Error.Description);
                    } else if((int)status_code == 401) {
                        throw new FigoException("access_denied", "Access Denied");
                    } else if((int)status_code == 404) {
                        return null;
                    } else {
                        throw new FigoException("internal_server_error", "We are very sorry, but something went wrong");
                    }
                }
                throw;
            }
            return result;
        }
        #endregion

        /// <summary>
        /// The URL a user should open in his/her web browser to start the login process.
        /// 
        /// When the process is completed, the user is redirected to the URL provided to the constructor and passes on an authentication code. This code can be converted into an access token for data access.
        /// </summary>
        /// <param name="scope">Scope of data access to ask the user for, e.g. `accounts=ro`</param>
        /// <param name="state">String passed on through the complete login process and to the redirect target at the end. It should be used to validated the authenticity of the call to the redirect URL</param>
        /// <returns>the URL of the first page of the login process</returns>
        public String GetLoginUrl(string scope, string state) {
	        StringBuilder sb = new StringBuilder();
            sb.Append(ApiEndpoint);
	        sb.Append("/auth/code?response_type=code&client_id=");
	        sb.Append(WebUtility.UrlEncode(this.ClientId));
	        sb.Append("&redirect_uri=");
	        sb.Append(WebUtility.UrlEncode(this.RedirectUri));
	        sb.Append("&scope=");
	        sb.Append(WebUtility.UrlEncode(scope));
	        sb.Append("&state=");
	        sb.Append(WebUtility.UrlEncode(state));
	        return sb.ToString();
        }
     
        /// <summary>
        /// Convert the authentication code received as result of the login process into an access token usable for data access.
        /// </summary>
        /// <param name="authenticationCode">the code received as part of the call to the redirect URL at the end of the logon process</param>
        /// <returns>the actual access and refresh tokens. 
        /// You can pass it into `FigoConnection.open_session` to get a FigoSession and access the users data. 
        /// If the scope contained the `offline` flag, also a refresh token is generated. It can be used to generate new access tokens, when the first one has expired.</returns>
        public async Task<TokenResponse> ConvertAuthenticationCode(string authenticationCode) {
             if (!authenticationCode.StartsWith("O")) {
        	    throw new FigoException("invalid_code", "Invalid authentication code");
            }

            return await DoRequest<TokenResponse>("/auth/token", "POST", new AuthenticationCodeTokenRequest { Code=authenticationCode, RedirectUri=RedirectUri});
        }
        
        /// <summary>
        /// Convert a refresh token (granted for offline access and returned by `convert_authentication_code`) into an access token usabel for data acccess.
        /// </summary>
        /// <param name="refreshToken">refresh token returned by convertAuthenticationCode</param>
        /// <param name="scope">Subscope of the refresh token used for the access token</param>
        /// <returns>the new access token</returns>
        public async Task<TokenResponse> ConvertRefreshToken(string refreshToken, string scope = null) {
             if (!refreshToken.StartsWith("R")) {
        	    throw new FigoException("invalid_code", "Invalid authentication code");
            }

            return await DoRequest<TokenResponse>("/auth/token", "POST", new RefreshTokenTokenRequest { RefreshToken=refreshToken, Scope=scope});
        }

        /// <summary>
        /// Convert a refresh token (granted for offline access and returned by `convert_authentication_code`) into an access token usabel for data acccess.
        /// </summary>
        /// <param name="tokenResponse">return value of convertAuthenticationCode with RefreshToken set</param>
        /// <param name="scope">Subscope of the refresh token used for the access token</param>
        /// <returns>the new access token</returns>
        public async Task<TokenResponse> ConvertRefreshToken(TokenResponse tokenResponse, string scope = null) {
             if (String.IsNullOrEmpty(tokenResponse.RefreshToken)) {
        	    throw new FigoException("invalid_code", "Invalid authentication code");
            }

            return await ConvertRefreshToken(tokenResponse.RefreshToken, scope);
        }

        /// <summary>
        /// Revoke a granted access or refresh token and thereby invalidate it.
        /// 
        /// Note: this action has immediate effect, i.e. you will not be able use that token anymore after this call.
        /// </summary>
        /// <param name="token">access or refresh token to be revoked</param>
        /// <returns>allways returns true</returns>
        public async Task<bool> RevokeToken(string token) {
			await DoRequest("/auth/revoke?token=" + WebUtility.UrlEncode(token));
            return true;
        }

        /// <summary>
        /// Revoke a granted access or refresh token and thereby invalidate it.
        /// 
        /// Note: this action has immediate effect, i.e. you will not be able use that token anymore after this call.
        /// </summary>
        /// <param name="tokenResponse">return value from convertRefreshToken or convertAuthenticationCode. If a refresh token is set, that is revoked otherwise the access token</param>
        /// <returns>allways returns true</returns>
        public async Task<bool> RevokeToken(TokenResponse tokenResponse) {
            if(!String.IsNullOrEmpty(tokenResponse.RefreshToken))
                return await RevokeToken(tokenResponse.RefreshToken);
            else
                return await RevokeToken(tokenResponse.AccessToken);
        }
    }
}
