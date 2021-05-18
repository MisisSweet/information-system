using System;
using System.Collections.Generic;
using System.Text;
using Test.Utilits;

namespace Test.Bindings
{
    class AlgorithmBindings
    {
        private readonly ScenarioContextAccessor contextAccessor;
        private readonly AlgorithmApi algorithmApi;

        public AlgorithmBindings(
           ScenarioContextAccessor contextAccessor,
           ScenarioSpecificJsonHttpClient httpClient)
        {
            this.contextAccessor = contextAccessor;
            this.algorithmApi = new AlgorithmApi(httpClient);
        }
    }
}
