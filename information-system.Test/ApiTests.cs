using BoDi;
using information_system.Controllers;
using information_system.Data;
using information_system.Models.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace information_system.Test
{
    public class ApiTests
    {
        private readonly HttpClient _client;

        public ApiTests()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Fact]
        public async Task GetUser()
        {
            //var role = new IdentityRole();
            var _userManager = new Mock<UserManager<User>>(MockBehavior.Loose);
            //var _roleManager = new Mock<RoleManager<IdentityRole>>();
            //var _systemContext = new Mock<SystemContext>();
            //var _appEnvironment = new Mock<IWebHostEnvironment>();
            //var controller = new Mock<UserRolesController>();

            //var result = new UserRolesController(_userManager.Object, _roleManager.Object, _systemContext.Object, _appEnvironment.Object);

            //await result.ReturnUser();


        }
    }
}
