using System;
using System.Net;

namespace NuevoSoftware.ApplicationMonitoring.ClientBusiness
{
    /// <summary>
    /// 
    /// </summary>
    public static class StatusHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static bool GetStatus(string URL)
        {
            HttpStatusCode httpStatusCode = default(HttpStatusCode);
            bool result;
            try
            {
                WebRequest request = HttpWebRequest.Create(URL);
                request.Method = "HEAD";
                using (HttpWebResponse webResponse = request.GetResponse() as HttpWebResponse)
                {
                    if (webResponse != null)
                    {
                        httpStatusCode = webResponse.StatusCode;
                        webResponse.Close();
                    }
                }
                int statusCode = (int)httpStatusCode;
                result = 200 <= statusCode && statusCode < 300;
            }
            catch 
            {
                result = false;
            }
            return result;
        }
    }
}