using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace epicture.Network
{
    public class Networker
    {
        

        public static async Task<string> PostRequest(String url, Dictionary<String, String> header = null, Dictionary < String, String> content = null)
        {
            var client = new HttpClient();
            var encodedContent = content == null ? new FormUrlEncodedContent(new Dictionary<String, String>()) : new FormUrlEncodedContent(content);
            var response = await client.PostAsync(url, encodedContent);

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<HttpResponseMessage> GetRequest(String url, Dictionary<String, String> content = null)
        {
            var client = new HttpClient();
            url = url[url.Length - 1] == '?' ? url : url + "?";
            if (content != null)
                url = content.Aggregate(url, (current, val) => current + val.Key + "=" + val.Value + "&");

            return await client.GetAsync(url);
        }
    }
}
