using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Test.Bindings;
using Test.Utilits;

namespace Test.Bindings
{
    internal sealed class AlgorithmApi
    {
        private readonly ScenarioSpecificJsonHttpClient httpClient;

        public AlgorithmApi(ScenarioSpecificJsonHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
       
    }
}
