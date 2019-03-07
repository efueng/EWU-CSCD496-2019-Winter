using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SecretSanta.Api.Models;
using SecretSanta.Domain.Models;
using Serilog;
using Serilog.Events;

namespace SecretSanta.Api
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        public static void Main(string[] args)
        {
            CurrentDirectoryHelpers.SetCurrentDirectory();

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("App Name", "SecretSanta.Api")
                .WriteTo.Console()
                .WriteTo.ApplicationInsights("ed1b38da-118f-4f5c-9e49-691e81093518", TelemetryConverter.Events)
                .WriteTo.SQLite(Configuration.GetConnectionString("DefaultConnection"))
                .CreateLogger();
            try
            {
                IWebHost host = CreateWebHostBuilder(args).Build();

                using (IServiceScope serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    using (ApplicationDbContext context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                    {
                        context.Database.Migrate();
                    }
                }

                host.Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
