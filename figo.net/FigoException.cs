using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace figo {
    public class FigoException : Exception {
        public FigoException(String error_code, String error_message) : base(error_message) {
            this.ErrorCode = error_code;
        }

        public FigoException(String error_code, String error_message, Exception exc) : base(error_message, exc) {
            this.ErrorCode = error_code;
        }

        public FigoException(ErrorResponse response) : this(response.Error, response.ErrorDescription) {
        }

        public string ErrorCode {
            get;
            private set;
        }

        [JsonObject]
        public class ErrorResponse {
            [JsonProperty("error")]
            public String Error { get; set; }

            [JsonProperty("error_description")]
            public String ErrorDescription { get; set; }
        }
    }
}
