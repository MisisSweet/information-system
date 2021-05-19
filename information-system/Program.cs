using information_system.Data;
using information_system.Models.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<SystemContext>();
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await ContextSeed.SeedRolesAsync(userManager, roleManager);
                    await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);

                    Status status = context.Statuses.FirstOrDefault(s => s.StatusName.ToLower().Equals("в обработке"));
                    if (status == null)
                    {
                        context.Statuses.Add(new Status() { StatusName = "В обработке" });
                        context.SaveChanges();
                    }
                    status = context.Statuses.FirstOrDefault(s => s.StatusName.ToLower().Equals("отменена"));
                    if (status == null)
                    {
                        context.Statuses.Add(new Status() { StatusName = "Отменена" });
                        context.SaveChanges();
                    }
                    status = context.Statuses.FirstOrDefault(s => s.StatusName.ToLower().Equals("в пользовании"));
                    if (status == null)
                    {
                        context.Statuses.Add(new Status() { StatusName = "В пользовании" });
                        context.SaveChanges();
                    }
                    status = context.Statuses.FirstOrDefault(s => s.StatusName.ToLower().Equals("возвращено"));
                    if (status == null)
                    {
                        context.Statuses.Add(new Status() { StatusName = "возвращено" });
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
           .UseStartup<Startup>();
    }
}
