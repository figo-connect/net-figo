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

        public FigoException(ErrorResponse response) : this(response.Error.Code.ToString(), response.Error.Description) {
        }

        public string ErrorCode {
            get;
            private set;
        }

        [JsonObject]
        public class ErrorData
        {
            [JsonProperty("code")]
            public Int32 Code { get; set; }

            [JsonProperty("data")]
            public Object Data { get; set; }

            [JsonProperty("description")]
            public String Description { get; set; }

            [JsonProperty("group")]
            public String Group { get; set; }

            [JsonProperty("message")]
            public String Message { get; set; }

            [JsonProperty("name")]
            public String Name { get; set; }
        }

        [JsonObject]
        public class ErrorResponse {
            [JsonProperty("error")]
            public ErrorData Error { get; set; }

            [JsonProperty("status")]
            public Int32 Status { get; set; }
        }
    }
}
