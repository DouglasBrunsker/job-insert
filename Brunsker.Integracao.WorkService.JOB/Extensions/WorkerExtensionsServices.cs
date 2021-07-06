using Brunsker.Integracao.Domain.Models;
using Brunsker.Integracao.Domain.Services;
using Brunsker.Integracao.WorkService.Insert;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

namespace Brunsker.Integracao.WorkService.JOB.Extensions
{
    public static class WorkerExtensionsServices
    {
        public static IServiceCollection AddWorkerExtensionServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<WorkServiceInsert>();

            services.AddApplicationService();

            services.AddRabbitAdapterConfiguration();

            services.AddRefitClient<IIntegracaoApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri("http://168.138.250.55:4400"));

            services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfiguration"));

            services.AddOracleAdapterRespository(configuration);

            return services;
        }
    }
}