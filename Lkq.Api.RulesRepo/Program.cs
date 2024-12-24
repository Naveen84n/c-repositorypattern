using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Lkq.Api.RulesRepo
{
    /// <summary>
    /// Main program routine
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Program
    {

        /// <summary>
        /// Configuration read from file, and injecting changes based on environment, default to PROD
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "prod"}.json", optional: true)
            .Build();

        /// <summary>
        /// Main Entry Point
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Main(string[] args)
        {
            // start the SeriLog up front
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                // create the host services
                var host = CreateHostBuilder(args).Build();
                Log.Information("Starting host...");
                // Inject anything before the server starts [OPTIONAL]
                // TODO: Remove any unneeded startup code here
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    //DataGenerator.Initialize(services);
                }
                // start the host
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Create a host and build in a serilogger
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}
