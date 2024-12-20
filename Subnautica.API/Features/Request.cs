namespace Subnautica.API.Features
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    public class Request
    {
        /**
         *
         * İçeriği döner veya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetContent(string remoteUrl, HttpMethod type = null, Dictionary<string, string> payload = null)
        {
            if (type == null)
            {
                type = HttpMethod.Get;
            }

            HttpContent content = null;
            if(payload != null)
            {
                content = new FormUrlEncodedContent(payload);
            }

            var response = string.Empty;
            using (var httpClient = new HttpClient())
            {
                if (remoteUrl.Contains("?"))
                {
                    remoteUrl = string.Format("{0}&t={1}", remoteUrl, Tools.GetRandomInt(1000000000, 2000000000));
                }
                else
                {
                    remoteUrl = string.Format("{0}?t={1}", remoteUrl, Tools.GetRandomInt(1000000000, 2000000000));
                }

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method     = type,
                    RequestUri = new Uri(remoteUrl),
                    Content    = content
                };

                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x86arm) AppleWebKit/527.30 (KHTML, like Gecko) Chrome/100.0.0.0 Safair/535.29");

                var result = httpClient.SendAsync(request).Result;
                response   = result.Content.ReadAsStringAsync().Result;
            }

            return response.Trim();
        }
    }
}