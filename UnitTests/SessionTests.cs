using figo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests {
    [TestClass]
    public class SessionTests {
        private FigoSession sut = null;

        [TestInitialize]
        public void SetUp() {
            sut = new FigoSession("ASHWLIkouP2O6_bgA2wWReRhletgWKHYjLqDaqb0LFfamim9RjexTo22ujRIP_cjLiRiSyQXyt2kM1eXU2XLFZQ0Hro15HikJQT_eNeT_9XQ");
        }

        [TestMethod]
        public void testGetAccount() {
            Task<FigoAccount> task_a = sut.getAccount("A1.2");
            FigoAccount a = task_a.Result;
            Assert.AreEqual("A1.2", a.AccountId);

            Task<FigoAccountBalance> task_b = sut.getAccountBalance(a);
            task_b.Wait();
            FigoAccountBalance b = task_b.Result;
            Assert.IsNotNull(b.Balance);
            Assert.IsNotNull(b.BalanceDate);

            Task<List<FigoTransaction>> task_c = sut.getTransactions(a);
            task_c.Wait();
            List<FigoTransaction> ts = task_c.Result;
            Assert.IsTrue(ts.Count > 0);
        }

        [TestMethod]
        public void testMissingHandling() {
            var task_a = sut.getAccount("A1.5");
            task_a.Wait();
            Assert.IsNull(task_a.Result);
	    }

	    [TestMethod]
	    public void testGetTransactions() {
            Task<List<FigoTransaction>> task_a = sut.getTransactions();
            task_a.Wait();
            List<FigoTransaction> transactions = task_a.Result;
            Assert.IsTrue(transactions.Count > 0);
	    }
    }
}
