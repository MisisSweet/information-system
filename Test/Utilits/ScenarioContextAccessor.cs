using System.Runtime.CompilerServices;
using TechTalk.SpecFlow;
using Test.Utilits;

namespace Test.Utilits
{
    public sealed class ScenarioContextAccessor
    {
        private const string LastHttpResponseKey = nameof(LastHttpResponse);

        private readonly ScenarioContext context;

        private ScenarioContext Context => this.context;

        public JsonHttpResponseMessage LastHttpResponse
        {
            get => (JsonHttpResponseMessage)this.context[LastHttpResponseKey];
            set => this.context[LastHttpResponseKey] = value;
        }
        private T Get<T>([CallerMemberName] string key = "") => (T)this.Context[key];

        private void Set<T>(T value, [CallerMemberName] string key = "") => this.Context[key] = value;
    }
}
