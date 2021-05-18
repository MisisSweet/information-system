using BoDi;
using information_system;
using information_system.Data;
using information_system.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;
using Microsoft.EntityFrameworkCore;
using Moq;
using Test.Mocks;
using MassTransit;
using System;

namespace Test.Setup
{
    [Binding]
    class SetupHook
    {
        private static WebApplicationFactory<Startup> _webApplicationFactory;
        private readonly IObjectContainer objectContainer;

        public SetupHook(IObjectContainer objectContainer)
        {
            this.objectContainer=objectContainer;
        }

        [BeforeTestRun]
        public static void SetupTestRun()
        {
            _webApplicationFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(b =>
                {
                    b.UseEnvironment("UnitTests");
                    b.ConfigureTestServices(ConfigureTestServices);
                });
        }
        [BeforeScenario(Order = 1)]
        public void InjectMocks()
        {
            var httpContextAccessorMock = new HttpContextAccessorMock();
            this.objectContainer.RegisterInstanceAs(httpContextAccessorMock.Object);
        }
        [BeforeScenario(Order = 2)]
        public void SetupInMemoryDatabase()
        {
            var options = new DbContextOptionsBuilder<SystemContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=information_system;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            this.objectContainer.RegisterInstanceAs(options);
        }
        [BeforeScenario(Order = 3)]
        public void SeedInMemoryDatabase()
        {
            var databaseContext = this.objectContainer.Resolve<SystemContext>();
            databaseContext.SaveChanges();
        }
        public static void ConfigureTestServices(IServiceCollection services)
        {
            services.AddScoped(p =>
                  ScenarioContext.Current.ScenarioContainer.Resolve<DbContextOptions<SystemContext>>());

            services.AddSingleton(p => new Mock<IBusControl>().Object);
        }
        private static void MockServiceScoped<TService, TMock>(IServiceCollection services)
            where TService : class
            where TMock : Mock<TService>
        {
            services.AddScoped(provider => ScenarioContext.Current.ScenarioContainer.Resolve<TMock>().Object);
        }
    }
}
