using Brunsker.Integracao.Domain.Adapters;
using Brunsker.Integracao.RabbitMQ;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitAdapterCollectionExtensions
    {
        public static IServiceCollection AddRabbitAdapterConfiguration(this IServiceCollection services)
        {
            services.AddTransient<IRabbitMqAdapter, ReceiverTrigger>();

            return services;
        }
    }
}
