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

        [TestMethod]
        public void testGetSync() {
            Task<string> task_a = sut.getSyncTaskToken("test", "http://localhost:3000/callback");
            task_a.Wait();
            Assert.IsNotNull(task_a.Result);
        }

        [TestMethod]
        public void testErrorHandling() {
            try {
                Task<string> task_a = sut.getSyncTaskToken("", "http://localhost:3003/");
                task_a.Wait();
                Assert.Fail("No exception encountered, bad");
            } catch(Exception exc) {
                Assert.IsInstanceOfType(exc.InnerException, typeof(FigoException));
            }
        }

        [TestMethod]
        public void testCreateUpdateDeleteNotification() {
            FigoNotification notification = new FigoNotification { ObserveKey = "/rest/transactions", NotifyURI = "http://figo.me/test", State = "qwe" };
            Task<FigoNotification> task_add = sut.addNotification(notification);
            task_add.Wait();
            FigoNotification addedNotification = task_add.Result;
            Assert.IsNotNull(addedNotification.NotificationId);
            Assert.AreEqual("/rest/transactions", addedNotification.ObserveKey);
            Assert.AreEqual("http://figo.me/test", addedNotification.NotifyURI);
            Assert.AreEqual("qwe", addedNotification.State);

            addedNotification.State = "asd";
            Task<FigoNotification> task_update = sut.updateNotification(addedNotification);
            task_update.Wait();

            Task<FigoNotification> task_get = sut.getNotification(addedNotification.NotificationId);
            task_get.Wait();
            FigoNotification updatedNotification = task_get.Result;
		    Assert.AreEqual(addedNotification.NotificationId, updatedNotification.NotificationId);
		    Assert.AreEqual("/rest/transactions", updatedNotification.ObserveKey);
		    Assert.AreEqual("http://figo.me/test", updatedNotification.NotifyURI);
		    Assert.AreEqual("asd", updatedNotification.State);

		    Task<bool> task_delete = sut.removeNotification(updatedNotification);
            task_delete.Wait();

            Task<FigoNotification> task_test = sut.getNotification(addedNotification.NotificationId);
            task_test.Wait();
            Assert.IsNull(task_test.Result);
	    }
    }
}
