using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using figo.models;

namespace figo
{
    public class FigoSession {
        /// <summary>
        /// figo API endpoint to use. This should only be changed when using a custom figo deployment.
        /// </summary>
        private string _apiEndoint = "https://api.figo.me";
        public string ApiEndpoint { 
            get { return _apiEndoint; } 
            set { _apiEndoint = value; }
        }

        /// <summary>
        /// The users access token used for authentication
        /// </summary>
        public string AccessToken { get; set; }

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
            req.Headers["Authorization"] = "Bearer " + AccessToken;
            if(req is HttpWebRequest) {
                ((HttpWebRequest)req).ContinueTimeout = Timeout;
                ((HttpWebRequest)req).Accept = "application/json";
            }

            if (body != null) {
                req.ContentType = "application/json";

                using(var request_stream = await req.GetRequestStreamAsync()) {
                    byte[] bytes = Encoding.UTF8.GetBytes(body);
                    request_stream.Write(bytes, 0, bytes.Length);
                }
            }

            string result = null;
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

        #region User
        /// <summary>
        /// Get the current figo Account
        /// </summary>
        /// <returns>User object for the current figo Account</returns>
        public async Task<FigoUser> GetUser() {
            return await DoRequest<FigoUser>("/rest/user");
        }

        /// <summary>
        /// Modify the current figo Account
        /// </summary>
        /// <param name="user">modified user object to be saved</param>
        /// <returns>User object for the updated figo Account</returns>
        public async Task<FigoUser> UpdateUser(FigoUser user) {
            return await DoRequest<FigoUser>("/rest/user", "PUT", user);
        }

        /// <summary>
        /// Delete current figo Account
        /// </summary>
        public async Task<bool> RemoveUser() {
            await DoRequest("/rest/user", "DELETE");
            return true;
        }
        #endregion

        #region Accounts
        /// <summary>
        /// All accounts the user has granted your App access to
        /// </summary>
        /// <returns>List of accounts</returns>
        public async Task<IList<FigoAccount>> GetAccounts() {
            var response = await this.DoRequest<FigoAccount.FigoAccountsResponse>("/rest/accounts");
            return response == null ? null : response.Accounts;
	    }

        /// <summary>
        /// Returns the account with the specified ID
        /// </summary>
        /// <param name="accountId">figo ID for the account to be retrieved</param>
        /// <returns>Account or null</returns>
        public async Task<FigoAccount> GetAccount(String accountId) {
            return await this.DoRequest<FigoAccount>("/rest/accounts/" + accountId);
	    }

        /// <summary>
        /// Modify an account
        /// </summary>
        /// <param name="account">the modified account to be saved</param>
        /// <returns>Account object for the updated account returned by server</returns>
        public async Task<FigoAccount> UpdateAccount(FigoAccount account) {
            return await this.DoRequest<FigoAccount>("/rest/accounts/" + account.AccountId, "PUT", account);
        }

        /// <summary>
        /// Remove an account
        /// </summary>
        /// <param name="account">account to be removed</param>
        public async Task<bool> RemoveAccount(FigoAccount account) {
            return await RemoveAccount(account.AccountId);
        }

        /// <summary>
        /// Remove an account
        /// </summary>
        /// <param name="account">ID of the account to be removed</param>
        public async Task<bool> RemoveAccount(String accountId) {
            await this.DoRequest("/rest/accounts/" + accountId, "DELETE");
            return true;
        }

        /// <summary>
        /// Returns the balance details of the account with the specified ID
        /// </summary>
        /// <param name="accountId">figo ID for the account whose balance is to be retrieved</param>
        /// <returns>Account balance or null</returns>
        public async Task<FigoAccountBalance> GetAccountBalance(String accountId) {
            return await this.DoRequest<FigoAccountBalance>("/rest/accounts/" + accountId + "/balance");
	    }

        /// <summary>
        /// Returns the balance details of the specified account
        /// </summary>
        /// <param name="account">Account whose balance is to be retrieved</param>
        /// <returns>Account balance or null</returns>
	    public async Task<FigoAccountBalance> GetAccountBalance(FigoAccount account){
		    return await GetAccountBalance(account.AccountId);
	    }

        /// <summary>
        /// Modify balance or account limits
        /// </summary>
        /// <param name="accountId">ID of the account to be modified</param>
        /// <param name="accountBalance">modified AccountBalance object to be saved</param>
        /// <returns>AccountBalance object for the updated account as returned by the server</returns>
        public async Task<FigoAccountBalance> UpdateAccountBalance(String accountId, FigoAccountBalance accountBalance) {
            return await this.DoRequest<FigoAccountBalance>("/rest/accounts/" + accountId + "/balance", "PUT", accountBalance);
        }

        /// <summary>
        /// Modify balance or account limits
        /// </summary>
        /// <param name="account">the account to be modified</param>
        /// <param name="accountBalance">modified AccountBalance object to be saved</param>
        /// <returns>AccountBalance object for the updated account as returned by the server</returns>
        public async Task<FigoAccountBalance> UpdateAccountBalance(FigoAccount account, FigoAccountBalance accountBalance) {
            return await UpdateAccountBalance(account.AccountId, accountBalance);
        }
        #endregion

        #region Banks
        /// <summary>
        /// Get bank
        /// </summary>
        /// <param name="bankId">ID of the bank to be retrieved</param>
        /// <returns>Bank object representing the bank to be retrieved</returns>
        public async Task<FigoBank> GetBank(String bankId) {
            return await this.DoRequest<FigoBank>("/rest/banks/" + bankId);
        }

        /// <summary>
        /// Modify a bank
        /// </summary>
        /// <param name="bank">modified bank object to be saved</param>
        /// <returns>Bank object for the updated bank</returns>
        public async Task<FigoBank> UpdateBank(FigoBank bank) {
            return await this.DoRequest<FigoBank>("/rest/banks/" + bank.BankId, "PUT", bank);
        }

        /// <summary>
        /// Remove the stored PIN for a bank (if there was one)
        /// </summary>
        /// <param name="bank">bank whose pin should be removed</param>
        public async Task<bool> RemoveBankPin(FigoBank bank) {
            return await RemoveBankPin(bank.BankId);
        }

        /// <summary>
        /// Remove the stored PIN for a bank (if there was one)
        /// </summary>
        /// <param name="bankId">ID of the bank whose pin should be removed</param>
        public async Task<bool> RemoveBankPin(String bankId) {
            await this.DoRequest("/rest/banks/" + bankId + "/remove_pin", "POST");
            return true;
        }
        #endregion

        #region Transactions
        /// <summary>
        /// All transactions of a specific account
        /// </summary>
        /// <param name="account">Account whose balance is to be retrieved</param>
        /// <param name="since">this parameter can either be a transaction ID or a date</param>
        /// <param name="count">limit the number of returned transactions</param>
        /// <param name="offset">which offset into the result set should be used to determin the first transaction to return (useful in combination with count)</param>
        /// <param name="include_pending">this flag indicates whether pending transactions should be included in the response; pending transactions are always included as a complete set, regardless of the `since` parameter</param>
        /// <returns>List of transactions</returns>
        public async Task<List<FigoTransaction>> GetTransactions(FigoAccount account, String since = null, int count = 1000, int offset = 0, bool include_pending = false) {
            return await GetTransactions(account.AccountId, since, count, offset, include_pending);
        }

        /// <summary>
        /// All transactions of a specific account
        /// </summary>
        /// <param name="accountId">ID of the account whose transactions are to be retrieved</param>
        /// <param name="since">this parameter can either be a transaction ID or a date</param>
        /// <param name="count">limit the number of returned transactions</param>
        /// <param name="offset">which offset into the result set should be used to determin the first transaction to return (useful in combination with count)</param>
        /// <param name="include_pending">this flag indicates whether pending transactions should be included in the response; pending transactions are always included as a complete set, regardless of the `since` parameter</param>
        /// <returns>List of transactions</returns>
	    public async Task<List<FigoTransaction>> GetTransactions(String accountId = null, String since = null, int count = 1000, int offset = 0, bool include_pending = false) {
            StringBuilder sb = new StringBuilder();
            if (accountId == null)
                sb.Append("/rest/transactions?count=");
            else
                sb.Append("/rest/accounts/" + accountId + "/transactions?count=");
            sb.Append(count);
            sb.Append("&offset=");
            sb.Append(offset);
            sb.Append("&include_pending=");
            sb.Append(include_pending ? "1" : "0");
            if(since != null) {
                sb.Append("&since=");
                sb.Append(WebUtility.UrlEncode(since));
            }

            FigoTransaction.TransactionsResponse response = await this.DoRequest<FigoTransaction.TransactionsResponse>(sb.ToString());
            return response == null ? null : response.Transactions;
	    }

        /// <summary>
        /// All securities of a specific account
        /// </summary>
        /// <param name="account"></param>
        /// <param name="since"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <param name="include_pending"></param>
        /// <returns></returns>
        public async Task<List<FigoSecurity>> GetSecurities(FigoAccount account, String since = null, int count = 1000, int offset = 0, bool include_pending = false)
        {
            return await GetSecurities(account.AccountId, since, count, offset, include_pending);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="since"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <param name="include_pending"></param>
        /// <returns></returns>
        private async Task<List<FigoSecurity>> GetSecurities(String accountId = null, String since = null, int count = 1000, int offset = 0, bool include_pending = false)
        {
            StringBuilder sb = new StringBuilder();
            if (accountId == null)
                sb.Append("/rest/securities?count=");
            else
                sb.Append("/rest/accounts/" + accountId + "/securities?count=");
            sb.Append(count);
            sb.Append("&offset=");
            sb.Append(offset);
            sb.Append("&include_pending=");
            sb.Append(include_pending ? "1" : "0");
            if (since != null)
            {
                sb.Append("&since=");
                sb.Append(WebUtility.UrlEncode(since));
            }

            FigoSecurity.SecurityResponse response = await DoRequest<FigoSecurity.SecurityResponse>(sb.ToString());
            return response == null ? null : response.Securities;
        }


        /// <summary>
        /// Retrieve a specific transaction by ID
        /// </summary>
        /// <param name="transactionId">transactionId the figo ID of the specific transaction</param>
        /// <returns>Transaction or null</returns>
	    public async Task<FigoTransaction> GetTransaction(String transactionId) {
            return await this.DoRequest<FigoTransaction>("/rest/transactions/" + transactionId);
	    }
        #endregion

        #region Account Synchronization
        /// <summary>
        /// Create a new synchronization task.
        /// </summary>
        /// <param name="state">Any kind of string that will be forwarded in the callback response message. It serves two purposes: The value is used to maintain state between this request and the callback, e.g. it might contain a session ID from your application. The value should also contain a random component, which your application checks to mitigate cross-site request forgery</param>
        /// <param name="redirect_url">At the end of the synchronization process a response will be sent to this callback URL</param>
        /// <returns>the URL to be opened by the user</returns>
        public async Task<string> GetSyncTaskToken(String state, String redirect_url) {
		    TaskTokenResponse response = await this.DoRequest<TaskTokenResponse>("/rest/sync", "POST", new SyncTokenRequest { State=state, RedirectURI=redirect_url});
            return ApiEndpoint + "/task/start?id=" + response.TaskToken;
	    }
        #endregion

        #region Notifications
        /// <summary>
        /// All notifications registered by this client for the user
        /// </summary>
        /// <returns>List of notifications</returns>
	    public async Task<List<FigoNotification>> GetNotifications() {
		    FigoNotification.NotificationsResponse response = await this.DoRequest<FigoNotification.NotificationsResponse>("/rest/notifications");
            return response == null ? null : response.Notifications;
	    }

        /// <summary>
        /// Retrieve a specific notification by ID
        /// </summary>
        /// <param name="notificationId">figo ID for the notification to be retrieved</param>
        /// <returns>Notification or Null</returns>
	    public async Task<FigoNotification> GetNotification(String notificationId) {
		    return await this.DoRequest<FigoNotification>("/rest/notifications/" + notificationId);
	    }

        /// <summary>
        /// Register a new notification on the server for the user
        /// </summary>
        /// <param name="notification">Notification which should be registered</param>
        /// <returns>created notification including its figo ID</returns>
	    public async Task<FigoNotification> AddNotification(FigoNotification notification) {
		    return await this.DoRequest<FigoNotification>("/rest/notifications", "POST", notification);
	    }

        /// <summary>
        /// Update a stored notification
        /// </summary>
        /// <param name="notification">Notification with updated values</param>
        /// <returns>Updated notification</returns>
	    public async Task<FigoNotification> UpdateNotification(FigoNotification notification) {
		    return await this.DoRequest<FigoNotification>("/rest/notifications/" + notification.NotificationId, "PUT", notification);
	    }

        /// <summary>
        /// Remove a stored notification from the server
        /// </summary>
        /// <param name="notification">Notification to be removed</param>
	    public async Task<bool> RemoveNotification(FigoNotification notification) {
		    await this.DoRequest("/rest/notifications/" + notification.NotificationId, "DELETE");
            return true;
	    }
        #endregion

        #region Payments
        /// <summary>
        /// All payments on all accounts of the user
        /// </summary>
        /// <returns>List of payments</returns>
        public async Task<List<FigoPayment>> GetPayments() {
            FigoPayment.PaymentsResponse response = await this.DoRequest<FigoPayment.PaymentsResponse>("/rest/payments");
            return response == null ? null : response.Payments;
        }

        /// <summary>
        /// All payments on the specified account of the user
        /// </summary>
        /// <param name="accountId">ID of the account for which to retrieve the payments</param>
        /// <returns>List of payments</returns>
        public async Task<List<FigoPayment>> GetPayments(string accountId) {
            FigoPayment.PaymentsResponse response = await this.DoRequest<FigoPayment.PaymentsResponse>("/rest/accounts/" + accountId + "/payments");
            return response == null ? null : response.Payments;
        }

        /// <summary>
        /// All payments on the specified account of the user
        /// </summary>
        /// <param name="account">account for which to retrieve the payments</param>
        /// <returns>List of payments</returns>
        public async Task<List<FigoPayment>> GetPayments(FigoAccount account) {
            return await GetPayments(account.AccountId);
        }

        /// <summary>
        /// Retrieve a specific payment by ID
        /// </summary>
        /// <param name="accountId">figo ID for the account on which the payment to be retrieved was created</param>
        /// <param name="paymentId">figo ID for the payment to be retrieved</param>
        /// <returns>Payment or Null</returns>
        public async Task<FigoPayment> GetPayment(string accountId, string paymentId) {
            return await this.DoRequest<FigoPayment>("/rest/accounts/" + accountId + "/payments/" + paymentId);
        }

        /// <summary>
        /// Create a new payment
        /// </summary>
        /// <param name="accountId">ID of the account to be used for the new payment</param>
        /// <param name="payment">Payment which should be created</param>
        /// <returns>created payment including its figo ID</returns>
        public async Task<FigoPayment> AddPayment(string accountId, FigoPayment payment) {
            return await this.DoRequest<FigoPayment>("/rest/accounts/" + accountId + "/payments", "POST", payment);
        }

        /// <summary>
        /// Update a stored payment. This is only possible if it has not been submitted yet.
        /// </summary>
        /// <param name="payment">Payment with updated values</param>
        /// <returns>Updated payment</returns>
        public async Task<FigoPayment> UpdatePayment(FigoPayment payment) {
            return await this.DoRequest<FigoPayment>("/rest/accounts/" + payment.AccountId + "/payments/" + payment.PaymentId, "PUT", payment);
        }

        /// <summary>
        /// Submit a stored payment to the bank. This is only possible if it has not been submitted yet.
        /// </summary>
        /// <param name="payment">Payment to be submitted</param>
        /// <returns>Updated payment</returns>
        public async Task<string> SubmitPayment(FigoPayment payment) {
            var response = await this.DoRequest<TaskTokenResponse>("/rest/accounts/" + payment.AccountId + "/payments/" + payment.PaymentId + "/submit", "POST");
            return ApiEndpoint + "/task/start?id=" + response.TaskToken;
        }

        /// <summary>
        /// Remove a stored payment from the server. This is only possible if it has not been submitted yet.
        /// </summary>
        /// <param name="payment">Payment to be removed</param>
        public async Task<bool> RemovePayment(FigoPayment payment) {
            await this.DoRequest("/rest/accounts/" + payment.AccountId + "/payments/" + payment.PaymentId, "DELETE");
            return true;
        }
        #endregion
    }
}
