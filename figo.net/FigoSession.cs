using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace figo
{
    public class FigoSession {
        static readonly string API_ENDPOINT = "https://api.figo.me";

        private string access_token = null;

        /// <summary>
        /// Creates a FigoSession instance
        /// </summary>
        /// <param name="access_token">access_token the access token to bind this session to a user</param>
        public FigoSession(string access_token) {
            this.access_token = access_token;
        }

        #region Request Handling
        protected virtual WebRequest SetupRequest(string method, string url) {
            WebRequest req = (WebRequest)WebRequest.Create(API_ENDPOINT + url);
            req.Method = method;
            req.Headers["Authorization"] = "Bearer " + access_token;

            if (method == "POST" || method == "PUT")
                req.ContentType = "application/json";

            return req;
        }

        protected async Task<T> DoRequest<T>(string endpoint, string method = "GET", object body = null) {
            string request_body = null;
            if(body != null)
                request_body = JsonConvert.SerializeObject(body);

            string response_body = await DoRequest(endpoint, method, request_body);
            if(response_body == null)
                return default(T);
            else
                return JsonConvert.DeserializeObject<T>(response_body);
        }

        protected async Task<String> DoRequest(string endpoint, string method = "GET", string body = null) {
            string result = null;
            WebRequest req = SetupRequest(method, endpoint);

            if (body != null) {
                req.ContentType = "application/json";

                using(var request_stream = await req.GetRequestStreamAsync()) {
                    byte[] bytes = Encoding.UTF8.GetBytes(body);
                    request_stream.Write(bytes, 0, bytes.Length);
                }
            }

            try {
                using (WebResponse resp = await req.GetResponseAsync()) {
                    result = resp.GetResponseAsString();
                }
            } catch (WebException wexc) {
                if (wexc.Response != null) {
                    string json_error = wexc.Response.GetResponseAsString();

                    HttpStatusCode status_code = HttpStatusCode.BadRequest;
                    HttpWebResponse resp = wexc.Response as HttpWebResponse;
                    if (resp != null)
                        status_code = resp.StatusCode;

                    if ((int)status_code >= 200 && (int)status_code < 300) {
                        return json_error;
                    } else if((int)status_code == 400) {
                        FigoException.ErrorResponse error_response = JsonConvert.DeserializeObject<FigoException.ErrorResponse>(json_error);
			            throw new FigoException(error_response.Error, error_response.ErrorDescription);
                    } else if((int)status_code == 401) {
			            throw new FigoException("access_denied", "Access Denied");
                    } else if((int)status_code == 404) {
			            return null;
		            } else {
			            throw new FigoException("internal_server_error", "We are very sorry, but something went wrong");
		            }
                }
                throw;
            } catch(Exception e) {
                throw;
            }
            return result;
        }
        #endregion

        #region Accounts
        /// <summary>
        /// All accounts the user has granted your App access to
        /// </summary>
        /// <returns>List of accounts</returns>
        public async Task<IList<FigoAccount>> getAccounts() {
            var response = await this.DoRequest<FigoAccount.FigoAccountsResponse>("/rest/accounts");
            if(response == null)
                return null;
            else
                return response.Accounts;
	    }

        /// <summary>
        /// Returns the account with the specified ID
        /// </summary>
        /// <param name="accountId">figo ID for the account to be retrieved</param>
        /// <returns>Account or null</returns>
        public async Task<FigoAccount> getAccount(String accountId) {
            return await this.DoRequest<FigoAccount>("/rest/accounts/" + accountId);
	    }

        /// <summary>
        /// Returns the balance details of the account with the specified ID
        /// </summary>
        /// <param name="accountId">figo ID for the account whose balance is to be retrieved</param>
        /// <returns>Account balance or null</returns>
        public async Task<FigoAccountBalance> getAccountBalance(String accountId) {
            return await this.DoRequest<FigoAccountBalance>("/rest/accounts/" + accountId + "/balance");
	    }

        /// <summary>
        /// Returns the balance details of the specified account
        /// </summary>
        /// <param name="account">Account whose balance is to be retrieved</param>
        /// <returns>Account balance or null</returns>
	    public async Task<FigoAccountBalance> getAccountBalance(FigoAccount account){
		    return await getAccountBalance(account.AccountId);
	    }
        #endregion

        #region Transactions
        /// <summary>
        /// All transactions on all accounts of the user
        /// </summary>
        /// <returns>List of transactions</returns>
        public async Task<List<FigoTransaction>> getTransactions() {
            FigoTransaction.TransactionsResponse response = await this.DoRequest<FigoTransaction.TransactionsResponse>("/rest/transactions");
		    if (response == null)
    			return null;
	    	else
		    	return response.Transactions;
	    }

        /// <summary>
        /// All transactions of a specific account
        /// </summary>
        /// <param name="accountId">ID of the account whose transactions are to be retrieved</param>
        /// <returns>List of transactions</returns>
	    public async Task<List<FigoTransaction>> getTransactions(String accountId) {
            FigoTransaction.TransactionsResponse response = await this.DoRequest<FigoTransaction.TransactionsResponse>("/rest/accounts/" + accountId + "/transactions");
		    if (response == null)
			    return null;
		    else
			    return response.Transactions;
	    }

        /// <summary>
        /// All transactions of a specific account
        /// </summary>
        /// <param name="account">Account whose balance is to be retrieved</param>
        /// <returns>List of transactions</returns>
	    public async Task<List<FigoTransaction>> getTransactions(FigoAccount account) {
		    return await getTransactions(account.AccountId);
	    }

        /// <summary>
        /// Retrieve a specific transaction by ID
        /// </summary>
        /// <param name="transactionId">transactionId the figo ID of the specific transaction</param>
        /// <returns>Transaction or null</returns>
	    public async Task<FigoTransaction> getTransaction(String transactionId) {
            return await this.DoRequest<FigoTransaction>("/rest/transactions/" + transactionId);
	    }
        #endregion

        #region Account Synchronization
        /// <summary>
        /// Create a new synchronization task.
        /// </summary>
        /// <param name="state">Any kind of string that will be forwarded in the callback response message. It serves two purposes: The value is used to maintain state between this request and the callback, e.g. it might contain a session ID from your application. The value should also contain a random component, which your application checks to mitigate cross-site request forgery</param>
        /// <param name="redirect_url">At the end of the synchronization process a response will be sent to this callback URL</param>
        /// <returns>task token</returns>
        public async Task<string> getSyncTaskToken(String state, String redirect_url) {
		    SyncTokenResponse response = await this.DoRequest<SyncTokenResponse>("/rest/sync", "POST", new SyncTokenRequest { State=state, RedirectURI=redirect_url});
		    return response.TaskToken;
	    }
        #endregion

        #region Notifications
        /// <summary>
        /// All notifications registered by this client for the user
        /// </summary>
        /// <returns>List of Notification objects</returns>
	    public async Task<List<FigoNotification>> getNotifications() {
		    FigoNotification.NotificationsResponse response = await this.DoRequest<FigoNotification.NotificationsResponse>("/rest/notifications");
		    if (response == null)
			    return null;
		    else
			    return response.Notifications;
	    }

        /// <summary>
        /// Retrieve a specific notification by ID
        /// </summary>
        /// <param name="notificationId">figo ID for the notification to be retrieved</param>
        /// <returns>Notification or Null</returns>
	    public async Task<FigoNotification> getNotification(String notificationId) {
		    return await this.DoRequest<FigoNotification>("/rest/notifications/" + notificationId);
	    }

        /// <summary>
        /// Register a new notification on the server for the user
        /// </summary>
        /// <param name="notification">Notification which should be registered</param>
        /// <returns>created notification including its figo ID</returns>
	    public async Task<FigoNotification> addNotification(FigoNotification notification) {
		    return await this.DoRequest<FigoNotification>("/rest/notifications", "POST", notification);
	    }

        /// <summary>
        /// Update a stored notification
        /// </summary>
        /// <param name="notification">Notification with updated values</param>
        /// <returns>Updated notification</returns>
	    public async Task<FigoNotification> updateNotification(FigoNotification notification) {
		    return await this.DoRequest<FigoNotification>("/rest/notifications/" + notification.NotificationId, "PUT", notification);
	    }

        /// <summary>
        /// Remove a stored notification from the server
        /// </summary>
        /// <param name="notification">Notification to be removed</param>
	    public async Task<bool> removeNotification(FigoNotification notification) {
		    await this.DoRequest("/rest/notifications/" + notification.NotificationId, "DELETE");
            return true;
	    }
        #endregion
    }
}
