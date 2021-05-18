using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Test.Utilits
{
    public sealed class JsonHttpResponseMessage
    {
        private readonly HttpResponseMessage response;
        private readonly JsonSerializerSettings serializerSettings;

        public bool IsSuccessStatusCode => this.response.IsSuccessStatusCode;

        public HttpStatusCode StatusCode => this.response.StatusCode;

        public JsonHttpResponseMessage(HttpResponseMessage response, JsonSerializerSettings serializerSettings)
        {
            this.response = response;
            this.serializerSettings = serializerSettings;
        }

        public async Task<T> ReadAsJsonAsync<T>()
        {
            var json = await this.response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json, this.serializerSettings);
        }

        public void Dispose()
        {
            this.response.Dispose();
        }
    }
}
