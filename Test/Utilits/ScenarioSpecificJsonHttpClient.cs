using System.Net.Http;
using System.Threading.Tasks;
using Test.Utilits;

namespace Test.Utilits
{
    public sealed class ScenarioSpecificJsonHttpClient : JsonHttpClient
    {
        private readonly ScenarioContextAccessor contextAccessor;

        public ScenarioSpecificJsonHttpClient(
            ScenarioContextAccessor contextAccessor,
            JsonHttpClient jsonHttpClient)
            : base(jsonHttpClient)
        {
            this.contextAccessor = contextAccessor;
        }

        protected override async Task<JsonHttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            var response = await base.SendAsync(httpRequestMessage);
            this.contextAccessor.LastHttpResponse = response;

            return response;
        }
    }
}
