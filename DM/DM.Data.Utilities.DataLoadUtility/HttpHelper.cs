using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility
{
    public class HttpHelper<T> : IDisposable
    {
        private const string ApiRoot = "http://dnd5eapi.co";

        private readonly HttpClient _client;

        public HttpHelper()
        {
            _client = new HttpClient();

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<T> ReadContent(string route)
        {
            using var httpResponse = await _client.GetAsync($"{ApiRoot}{route}");

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
    }
}