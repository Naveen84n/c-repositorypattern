using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Lkq.Api.RulesRepo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace TestRulesRepo.Server
{
    [ExcludeFromCodeCoverage]
    internal class TestServers
    {
        /// <summary>
        /// Creates a default test server using Lkq.Api.RulesRepo.Startup
        /// </summary>
        /// <returns></returns>
       

        public static TestServer CreateServer()
        {
            return CreateServer(Builder());
        }

        /// <summary>
        /// Builds a custom test server based on your WebHostBuilder
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static TestServer CreateServer(IWebHostBuilder builder)
        {
            return new TestServer(builder);
        }

        /// <summary>
        /// Builds default IWebHostBuilder
        /// </summary>
        /// <returns></returns>
        public static IWebHostBuilder Builder()
        {
            static string CalculateRelativeContentRootPath() =>
                Path.Combine(AssemblyDirectory,
                    @"..\..\..\..\Lkq.Api.RulesRepo");

            var w = new WebHostBuilder()
                .UseContentRoot(CalculateRelativeContentRootPath())
                .UseEnvironment("TEST")
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(CalculateRelativeContentRootPath())
                    .AddJsonFile("appsettings.json")
                    .Build()
                )
                .UseStartup<StartupWrapper>().UseSerilog();
            return w;
        }

        /// <summary>
        /// Gets assembly directory
        /// </summary>
        private static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().Location;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }

    [ExcludeFromCodeCoverage]
    public class StartupWrapper
    {
        

        public StartupWrapper(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostEnv = env;
            Startup = new Startup(configuration, env);
        }

        /// <summary>
        /// Configuration file
        /// </summary>
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment HostEnv { get; }
        private Startup Startup { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            Startup.ConfigureServices(services);
            //services.AddDbContextInMemory<WeatherModelContext>("weatherModel"); // custom data source - using in memory fake data
            //services.AddScoped<IWeatherRepository,MockWeatherDb>(); // Easily support dependency injection of data
            var asm = typeof(Startup).Assembly;
            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(asm));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Startup.Configure(app, env);
        }
    }
}
