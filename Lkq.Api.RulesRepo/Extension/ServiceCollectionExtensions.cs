using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Lkq.Api.RulesRepo.Model;
using Lkq.Core.RulesRepo.Interfaces;
using Lkq.Core.RulesRepo.Implementations;
using Lkq.Data.RulesRepo.Interfaces;
using Lkq.Data.RulesRepo.Specifications;

namespace Lkq.Api.RulesRepo.Extension
{
    /// <summary>
    /// ServiceCollectionExtensions
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IRulesRepoApiResponse, RulesRepoApiResponse>();
            services.AddScoped<IRules, Rules>();
            services.AddScoped<IRulesRepository, RulesRepository>();
            services.AddScoped<IDataSourceService, DataSourceService>();
        }
    }
}
