using figo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDemo.models {
    public class IndexModel {
        public FigoUser User { get; set; }
        public FigoAccount CurrentAccount { get; set; }
        public FigoAccountBalance CurrentAccountBalance { get; set; }
        public IList<FigoAccount> Accounts { get; set; }
        public IList<FigoTransaction> Transactions { get; set; }
    }
}
