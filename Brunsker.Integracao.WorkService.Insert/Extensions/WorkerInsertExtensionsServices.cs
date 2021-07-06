using System;
using Brunsker.Integracao.Domain.Models;
using Brunsker.Integracao.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Brunsker.Integracao.WorkService.Insert.Extensions
{
    public static class WorkerInsertExtensionsServices
    {
        public static IServiceCollection AddWorkerExtensionServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<WorkServiceInsert>();

            services.AddApplicationService();

            services.AddRabbitAdapterConfiguration();

            // services.AddRefitClient<IIntegracaoApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri("http://168.138.250.55:4400"));

            services.AddRefitClient<IIntegracaoApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5000"));

            services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfiguration"));

            services.AddOracleAdapterRespository(configuration);

            return services;
        }
    }
}