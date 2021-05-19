using System.Threading.Tasks;
using information_system.Models;
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
        public async Task<JsonHttpResponseMessage> Create(string name, string author, string description, string articl, string bookpictures, int year, int count)
        {
            var request = new CreateBookViewModel
            {
                BookName=name,
                Author=author,
                Articl=articl,
                Description=description,
                BookPicture=bookpictures,
                Year=year,
                Count=count
            };
            return await this.httpClient.PostAsync("https://localhost:44376/UserRoles/CreateB", request);
        }
        public async Task<JsonHttpResponseMessage> GetBook()
        {
            return await this.httpClient.GetAsync("https://localhost:44376/UserRoles/ReturnBook");
        }

    }
}
