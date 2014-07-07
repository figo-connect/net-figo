figo.net [![Build Status](https://travis-ci.org/figo-connect/net-figo.png)](https://travis-ci.org/figo-connect/net-figo)
===========

.net bindings for the figo connect API: http://figo.io

Simply add figo.net from NuGet.

And just as easy to use:
```csharp
using figo;

class FigoExample {
	static void Main(string[] args)  {
        FigoSession session = new FigoSession("ASHWLIkouP2O6_bgA2wWReRhletgWKHYjLqDaqb0LFfamim9RjexTo22ujRIP_cjLiRiSyQXyt2kM1eXU2XLFZQ0Hro15HikJQT_eNeT_9XQ");

		// print out a list of accounts including its balance
        var task_accounts = session.getAccounts();
        task_accounts.Wait();
        foreach(FigoAccount account in task_accounts.Result) {
            Console.WriteLine(account.Name);

            var task_balance = session.getAccountBalance(account);
            task_balance.Wait();
            Console.WriteLine(task_balance.Result.Balance);
		}

		// print out the list of all transactions on a specific account
        var task_transactions = session.getTransactions("A1.2");
        task_transactions.Wait();
		foreach(FigoTransaction transaction in task_transactions.Result) {
			Console.WriteLine(transaction.PurposeText);
		}
	}
}
```

A more detailed documentation of the figo connect API can be found at http://docs.figo.io.