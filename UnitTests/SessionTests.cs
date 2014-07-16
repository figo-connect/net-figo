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
            Task<FigoAccount> task_a = sut.GetAccount("A1.2");
            FigoAccount a = task_a.Result;
            Assert.AreEqual("A1.2", a.AccountId);
            Assert.IsNotNull(a.Balance.Balance);
            Assert.IsNotNull(a.Balance.BalanceDate);

            Task<List<FigoTransaction>> task_b = sut.GetTransactions(a);
            task_b.Wait();
            List<FigoTransaction> ts = task_b.Result;
            Assert.IsTrue(ts.Count > 0);

            Task<List<FigoPayment>> task_c = sut.GetPayments(a);
            task_c.Wait();
            List<FigoPayment> ps = task_c.Result;
            Assert.IsTrue(ps.Count >= 0);
        }

        [TestMethod]
        public void testGetTransactions() {
            Task<List<FigoTransaction>> task_a = sut.GetTransactions();
            task_a.Wait();
            List<FigoTransaction> transactions = task_a.Result;
            Assert.IsTrue(transactions.Count > 0);
        }

        [TestMethod]
        public void testGetNotifications() {
            Task<List<FigoNotification>> task_a = sut.GetNotifications();
            task_a.Wait();
            List<FigoNotification> notifications = task_a.Result;
            Assert.IsTrue(notifications.Count > 0);
        }

        [TestMethod]
        public void testGetPayments() {
            Task<List<FigoPayment>> task_a = sut.GetPayments();
            task_a.Wait();
            List<FigoPayment> payments = task_a.Result;
            Assert.IsTrue(payments.Count > 0);
        }

        [TestMethod]
        public void testMissingHandling() {
            var task_a = sut.GetAccount("A1.5");
            task_a.Wait();
            Assert.IsNull(task_a.Result);
	    }

        [TestMethod]
        public void testErrorHandling() {
            try {
                Task<string> task_a = sut.GetSyncTaskToken("", "http://localhost:3003/");
                task_a.Wait();
                Assert.Fail("No exception encountered, bad");
            } catch(Exception exc) {
                Assert.IsInstanceOfType(exc.InnerException, typeof(FigoException));
            }
        }

        [TestMethod]
        public void testGetSync() {
            Task<string> task_a = sut.GetSyncTaskToken("test", "http://localhost:3000/callback");
            task_a.Wait();
            Assert.IsNotNull(task_a.Result);
        }

        [TestMethod]
        public void testUser() {
            Task<FigoUser> task_a = sut.GetUser();
            task_a.Wait();
            Assert.AreEqual("demo@figo.me", task_a.Result.Email);
        }

        [TestMethod]
        public void testCreateUpdateDeleteNotification() {
            FigoNotification notification = new FigoNotification { ObserveKey = "/rest/transactions", NotifyURI = "http://figo.me/test", State = "qwe" };
            Task<FigoNotification> task_add = sut.AddNotification(notification);
            task_add.Wait();
            FigoNotification addedNotification = task_add.Result;
            Assert.IsNotNull(addedNotification);
            Assert.IsNotNull(addedNotification.NotificationId);
            Assert.AreEqual("/rest/transactions", addedNotification.ObserveKey);
            Assert.AreEqual("http://figo.me/test", addedNotification.NotifyURI);
            Assert.AreEqual("qwe", addedNotification.State);

            addedNotification.State = "asd";
            Task<FigoNotification> task_update = sut.UpdateNotification(addedNotification);
            task_update.Wait();

            Task<FigoNotification> task_get = sut.GetNotification(addedNotification.NotificationId);
            task_get.Wait();
            FigoNotification updatedNotification = task_get.Result;
            Assert.IsNotNull(updatedNotification);
		    Assert.AreEqual(addedNotification.NotificationId, updatedNotification.NotificationId);
		    Assert.AreEqual("/rest/transactions", updatedNotification.ObserveKey);
		    Assert.AreEqual("http://figo.me/test", updatedNotification.NotifyURI);
		    Assert.AreEqual("asd", updatedNotification.State);

		    Task<bool> task_delete = sut.RemoveNotification(updatedNotification);
            task_delete.Wait();

            Task<FigoNotification> task_test = sut.GetNotification(addedNotification.NotificationId);
            task_test.Wait();
            Assert.IsNull(task_test.Result);
	    }

        [TestMethod]
        public void testCreateUpdateDeletePayment() {
            FigoPayment payment = new FigoPayment { Type = "Transfer", AccountNumber = "4711951501", BankCode = "90090042", Name = "figo", Purpose = "Thanks for all the fish.", Amount = 0.89F };
            Task<FigoPayment> task_add = sut.AddPayment("A1.1", payment);
            task_add.Wait();
            FigoPayment addedPayment = task_add.Result;
            Assert.IsNotNull(addedPayment);
            Assert.IsNotNull(addedPayment.PaymentId);
            Assert.AreEqual("A1.1", addedPayment.AccountId);
            Assert.AreEqual("Demobank", addedPayment.BankName);
            Assert.AreEqual(0.89F, addedPayment.Amount);

            addedPayment.Amount = 2.39F;
            Task<FigoPayment> task_update = sut.UpdatePayment(addedPayment);
            task_update.Wait();

            Task<FigoPayment> task_get = sut.GetPayment(addedPayment.AccountId, addedPayment.PaymentId);
            task_get.Wait();
            FigoPayment updatedPayment = task_get.Result;
            Assert.IsNotNull(updatedPayment);
            Assert.AreEqual(addedPayment.PaymentId, updatedPayment.PaymentId);
            Assert.AreEqual("A1.1", updatedPayment.AccountId);
            Assert.AreEqual("Demobank", updatedPayment.BankName);
            Assert.AreEqual(2.39F, updatedPayment.Amount);

            Task<bool> task_delete = sut.RemovePayment(updatedPayment);
            task_delete.Wait();

            Task<FigoPayment> task_test = sut.GetPayment(addedPayment.AccountId, addedPayment.PaymentId);
            task_test.Wait();
            Assert.IsNull(task_test.Result);
        }
    }
}
