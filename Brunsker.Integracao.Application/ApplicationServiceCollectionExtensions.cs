using Brunsker.Integracao.Domain.Services;
using Brunsker.Integracao.Application;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IExecutarService, ExecutarService>();

            return services;
        }
    }
}
