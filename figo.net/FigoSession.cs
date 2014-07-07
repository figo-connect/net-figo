using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            //req.Credentials = access_token_credential;
            //req.PreAuthenticate = true;
            //req.Timeout = TimeoutSeconds * 1000;

            if (method == "POST" || method == "PUT")
                req.ContentType = "application/json";

            return req;
        }

        protected async Task<T> DoRequest<T>(string endpoint, string method = "GET", string body = null) {
            string json = await DoRequest(endpoint, method, body);
            if(json == null)
                return default(T);
            else
                return JsonConvert.DeserializeObject<T>(json);
        }

        protected async Task<String> DoRequest(string endpoint, string method = "GET", string body = null) {
            string result = null;
            WebRequest req = SetupRequest(method, endpoint);

            /*if (body != null) {
                byte[] bytes = encoding.GetBytes(body.ToString());
                req.ContentLength = bytes.Length;
                using (Stream st = req.GetRequestStream()) {
                    st.Write(bytes, 0, bytes.Length);
                }
            }*/

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
    }
}
