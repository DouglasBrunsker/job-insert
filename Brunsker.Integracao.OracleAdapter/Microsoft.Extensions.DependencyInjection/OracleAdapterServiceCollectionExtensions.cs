using Brunsker.Integracao.Domain.Adapters;
using Brunsker.Integracao.OracleAdapter;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OracleAdapterServiceCollectionExtensions
    {

        public static IServiceCollection AddOracleAdapterRespository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(d =>
            {
                return new DbConnectionDbRepositoryAdapter(new OracleConnection(configuration.GetConnectionString("OracleConnection")),
                                                           new OracleConnection(configuration.GetConnectionString("OracleConnectionLocal")));
            });

            services.AddScoped<IOracleRepositoryAdapter, OracleRepositoryAdapter>();

            return services;
        }
    }
}
