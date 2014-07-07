using System;
using System.IO;
using System.Net;
using System.Text;

namespace figo {
    public static class ExtensionMethods {
        public static string GetResponseAsString(this WebResponse response) {
            using(StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8)) {
                return sr.ReadToEnd();
            }
        }
    }
}
