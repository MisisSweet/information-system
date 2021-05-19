using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Test.Utilits;

namespace Test.Bindings
{
    [Binding]
    public sealed class AlgorithmBindings
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
        [Given("I have a book whith following parametrs:")]
        //public async Task GivenBook(Table table)
        //{
        //    foreach (var row in table.Rows)
        //    {
        //        await this.algorithmApi.GetBook();
        //    }
        //}
        public async Task GivenBook(Table table)
        {
            foreach (var row in table.Rows)
            {
                await this.algorithmApi.Create(
                    row["Name"],
                    row["Author"],
                    row["Description"],
                    row["Articl"],
                    row["BookPicture"],
                    int.Parse(row["Year"]),
                    int.Parse(row["Count"])
                    );

            }
        }
    }
}
