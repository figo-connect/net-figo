namespace WebDemo {
    using figo;
    using Nancy;
    using System;

    public class IndexModule : NancyModule {
        private readonly FigoConnection _figoConnection = new FigoConnection { ClientId = "CaESKmC8MAhNpDe5rvmWnSkRE_7pkkVIIgMwclgzGcQY", ClientSecret = "STdzfv0GXtEj_bwYn7AgCVszN1kKq5BdgEIKOM_fzybQ" };

        public IndexModule() {
            Get["/{id?}", true] = async (parameters, ct) => {
                FigoSession figoSession = null;
                models.IndexModel model = null;

                // checkout whether we are logged in already
                if(Request.Session["figo_token"] == null) {
                    return Response.AsRedirect(_figoConnection.GetLoginUrl(null, "qweqwe"));
                } else {
                    figoSession = new FigoSession(Request.Session["figo_token"].ToString());
                }
                
                if(parameters.ContainsKey("id"))
                    model = new models.IndexModel {
                        User = await figoSession.GetUser(),

                        Accounts = await figoSession.GetAccounts(),
                        CurrentAccount = await figoSession.GetAccount(parameters.id),
                        CurrentAccountBalance = await figoSession.GetAccountBalance(parameters.id),
                        
                        Transactions = await figoSession.GetTransactions(parameters.id),
                    };
                else
                    model = new models.IndexModel {
                        User = await figoSession.GetUser(),

                        Accounts = await figoSession.GetAccounts(),
                        CurrentAccount = null,
                        CurrentAccountBalance = null,

                        Transactions = await figoSession.GetTransactions(),
                    };
                return View["index", model];
            };

            Get["/logout", true] = async (parameters, ct) => {
                if (Request.Session["figo_token"] != null) {
                    await _figoConnection.RevokeToken(Request.Session["figo_token"].ToString());
                }

                Request.Session.DeleteAll();
                return Response.AsRedirect("/");
            };

            Get["/callback", true] = async (parameters, ct) => {
                if (Request.Query["code"] != null) {
                    TokenResponse tokenResponse = await _figoConnection.ConvertAuthenticationCode(Request.Query["code"]);
                    Request.Session["figo_token"] = tokenResponse.AccessToken;
                }
                return Response.AsRedirect("/");
            };
        }
    }
}