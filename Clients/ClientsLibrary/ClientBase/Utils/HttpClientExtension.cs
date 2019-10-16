using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientsLibrary.ClientBase.Utils
{
    /// <summary>
    /// Небольшое расширение HttpClient, для единообразия в запросах клиентов
    /// </summary>
    public static class HttpClientExtension
    {
        public static Task<HttpResponseMessage> GetAsync(this HttpClient client, string requiestUri, string content) 
            => client.GetAsync(requiestUri + content);
        public static Task<HttpResponseMessage> DeleteAsync(this HttpClient client, string requiestUri, string content)
            => client.DeleteAsync(requiestUri + content);
    }
}
