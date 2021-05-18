using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Test.Utilits;

namespace Test.Utilits
{
    public class JsonHttpClient : IDisposable
    {
        private readonly System.Net.Http.HttpClient internalClient;
        private readonly JsonSerializerSettings serializerSettings;
        private readonly JsonMediaTypeFormatter formatter;

        public JsonHttpClient(System.Net.Http.HttpClient internalClient, JsonSerializerSettings serializerSettings)
        {
            this.internalClient = internalClient;
            this.serializerSettings = serializerSettings;
            this.formatter = new JsonMediaTypeFormatter { SerializerSettings = serializerSettings };
        }

        protected JsonHttpClient(JsonHttpClient jsonHttpClient)
            : this(jsonHttpClient.internalClient, jsonHttpClient.serializerSettings)
        {
        }

        public Task<JsonHttpResponseMessage> PostAsync<T>(string requestUri, T value)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new ObjectContent<T>(value, this.formatter),
            };

            return this.SendAsync(httpRequestMessage);
        }

        public Task<JsonHttpResponseMessage> PutAsync<T>(string requestUri, T value)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri)
            {
                Content = new ObjectContent<T>(value, this.formatter),
            };

            return this.SendAsync(httpRequestMessage);
        }

        public Task<JsonHttpResponseMessage> GetAsync(string requestUri)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            return this.SendAsync(httpRequestMessage);
        }

        public Task<JsonHttpResponseMessage> DeleteAsync(string requestUri)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);

            return this.SendAsync(httpRequestMessage);
        }

        private JsonHttpResponseMessage CreateJsonHttpResponseMessage(HttpResponseMessage response)
        {
            return new JsonHttpResponseMessage(response, this.serializerSettings);
        }

        protected virtual async Task<JsonHttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            var response = await this.internalClient.SendAsync(httpRequestMessage);

            return this.CreateJsonHttpResponseMessage(response);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.internalClient.Dispose();
            }
        }
    }
}
