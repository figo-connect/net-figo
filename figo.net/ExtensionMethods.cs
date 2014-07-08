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

        public static string ToBase64(this string data) {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }
    }
}
