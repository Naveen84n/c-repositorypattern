using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using FluentValidation.AspNetCore;
using Lkq.Web;
using Lkq.Api.RulesRepo.Extension;
using Lkq.Core.RulesRepo.MappingProfiles;
using Lkq.Data.RulesRepo.DbContexts;
using Lkq.Api.RulesRepo.Filters;
using System.Diagnostics.CodeAnalysis;

namespace Lkq.Api.RulesRepo
{
    /// <summary>
    /// Main startup class
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        #region CTOR
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="appEnv"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment appEnv)
        {
            Configuration = configuration;
            CurrentEnvironment = appEnv;
        }
        #endregion

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Environment
        /// </summary>
        private IWebHostEnvironment CurrentEnvironment{ get; } 

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Filters
            services.AddMvc(options => {
                options.Filters.Add<ApiExceptionFilter>();
            });
            //Add Dependencies
            services.AddServices();

            //Auto Mapper
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddDbContext<AutoCareDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("AutoCareDBConnectionString")));
            services.AddDbContext<PartsDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("PartsDBConnectionString")));

            // Scoped services are created and destroyed for each "scope" or caller instance.  Each unique request gets their own instance.
            // TODO: add your own custom business functionality
            //services.AddScoped<WeatherAnalysisEngine>();

            // TODO: adjust the LKQ Dev Ops to meet your applications needs
            services.AddLkqDevOpsPipes(o =>
                {
                    o.AddHttpContextAccessor = true; // allows access of the HTTP Context from outside the API project
                    o.AddNewtonsoftJson = false; // if you are having problems with serialization, you might need legacy newtonsoft
                    o.SwaggerExamples = typeof(Startup); // where to get swagger examples from, whatever DLL you have examples in, use that type
                    o.InjectConfigurationSingleton = false; // this will put the configuration in the dependency injection in a singleton,
                                                            // Singleton will remain 1 instance for the life of the application, across all requests
                    o.AddIdentityServerAuthentication = true; // add identity authentication to API (and swagger)
                    o.AddApiVersioning = true; // enables the ability to auto inject versions into you controllers and URL path handling (v1, v2)
                    o.UseCors = true; // this requires APIOrigin in your appsettings.json file, this allows you to drive and control cross site scripting
                    
                    o.SwaggerXmlFiles = new[] { System.IO.Path.Combine(AppContext.BaseDirectory, // injects documentation into your swagger site
                        System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".xml"), // this DLL
                        System.IO.Path.Combine(AppContext.BaseDirectory,
                            System.Reflection.Assembly.GetExecutingAssembly().GetName().Name?.Replace(".Api.",".Models.") + ".xml") // Models DLL
                    };
                    o.PerformSwaggerGen = true; // perform swagger gen - Required to generate swagger documents
                    o.AddControllersWithViewsAndDefaultJson = true; // add the controllers for this API
                },
                Configuration);

            //Fluent validation
            services.AddControllers()
            .AddFluentValidation(s =>
            {
                s.RegisterValidatorsFromAssemblyContaining<Startup>();
                s.ValidatorOptions.CascadeMode = FluentValidation.CascadeMode.Stop;
                //s.RunDefaultMvcValidationAfterFluentValidationExecutes = true;
            });

            //Suppress validation
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //Newtonsoft Json settings
            services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                options.UseCamelCasing(true);
            });

            // TODO: Modify the policies or remove if not used (single scope)
            // Optionally you can create security policies based on scopes.  Scopes are defined at an API resources level.  When you
            // build your API, you define different levels of API access.  Multiple "clients" can be created that are "allowed" access
            // to one or more scopes using a "secret".  Some clients may have all scopes, some may have only 1.  This helps drive security
            // to your resource.  This is NOT user based security, but rather client based security.
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Readers", policy =>
                    policy.RequireClaim("scope", "rulesreporeadwrite"));
                options.AddPolicy("Writers", policy =>
                    policy.RequireClaim("scope", "rulesreporeadwrite"));
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // middleware is first in last out, UseDeveloperExceptionPage is first in, last out
            // the UseLkqDevOpsPipes does not let you further control this first in last out, so you either come before it all, or after
            // if you need more granular control, you'll have to manually invoke each step

            // TODO: Modify the Pipeline to meet your application needs
            app.UseLkqDevOpsPipes(o =>
                {
                    o.EnableProxyHeaders = true; // use the proxy headers injected from DevOps to help navigate URL paths
                    o.RequestMessageIdHeader = ""; // header value you want as a unique ID for each request, null will generate a new ID for each request
                                                   // null will also attempt to get the header from appsettings.json > MessageIdHeader or Settings.MessageIdHeader
                                                   // if no header value is provided in the supplied header field, still a generated ID is made
                    o.UseBodyLogging = true; // log the body of the incoming and outgoing message
                    o.UseAuthentication = true; // use authentication on endpoints that have authorization from the authentication middleware
                    o.UseStaticFiles = true; // use static web files, ability to supply custom scripts and additional files in wwwroot
                    o.UseSwaggerAsHomePage = true; // default home page is swagger
                    o.UseCors = true; // use cors (requires APIOrigin in your appsettings json)
                    o.GenerateUseSwagger = true; // generates the swagger document
                    o.GenerateSwaggerUI = true; // generates swagger UI files
                    o.UseHttpsRedirect = true; // redirect traffic that is HTTP to HTTPs
                    o.UseRouting = true; // use routing
                    o.UseAuthorization = true; // use authorization
                    o.GenerateEndPoints = true; // generate endpoints at the end of middleware
                    
                    /* Optional set custom routes in the Generate End Points
                    o.Routes = x =>
                    {
                        x.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Home}/{action=Index}/{id?}");
                    };
                    */

                    o.HealthPageOptions = t =>
                    {
                        t.ExcludeFromSerilog = true; // health page calls do not show up on your log feed
                        t.UseBuiltInSQL = false; // optionally your health page can sniff out your SQL connections and test them on health page hits
                    };

                    
                },
                Configuration);



            /*   // MANUAL PROCESS TO MIMIC ABOVE - NOTE that if webtools are updated, this workflow below may need manual updating
            app.UseForwardedHeaders(new ForwardedHeadersOptions() {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | 
                                   Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            }); // this step addresses some issues with routing and DevOps
            app.UseMiddleware<RequestTransactionMiddleware>();  // This step adds a transaction number to the logs, doesn't use headers though like webtools does
            app.UseStaticFiles();
            app.UseSerilogHealthCheckExcluder(); // this step removes healthcheck page from serilog
            if (!string.IsNullOrEmpty(Configuration.GetValue<string>("Swagger:Prefix")))
            {
                app.UsePathBase(Configuration.GetValue<string>("Swagger:Prefix")); // this step fixes the path prefix of swagger in DevOps land
            }
            app.UseLkqSerilogBodyLoggingSkipRTM(); // this step adds body logging to the output logs
            app.UseSwagger(o =>
            {
                if (!string.IsNullOrEmpty(Configuration.GetValue<string>("Swagger:Prefix")))
                {
                    o.PreSerializeProxy(Configuration); // this step fixes the prefix in the swagger docs
                }
            });
            app.UseSwaggerUI(o =>
            {
                o.AddSwaggerEndpoints(Configuration);  // this step generates endpoints based on the appsettings.json
                if (string.IsNullOrEmpty(Configuration.GetValue<string>("ID4:clientId"))) return;
                o.OAuthClientId(Configuration.GetValue<string>("ID4:clientId"));   // the next two settings update swagger to use identity security
                o.OAuthAppName("Swagger Api Calls");
            });
            app.AddHealthPage(o =>   // this adds the health page and basic options to your API
                {
                    o.IncludeInSwagger = true;
                    o.ExcludeFromSerilog = true;
                    o.UseBuiltInSQL = false;
                }
            );
            app.MakeSwaggerHome();
            app.UseApiVersioning();
            app.UseRouting();
            app.UseCors("AllowedScriptOrigins");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(x =>
            {
                x.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            */
        }
    }
}
