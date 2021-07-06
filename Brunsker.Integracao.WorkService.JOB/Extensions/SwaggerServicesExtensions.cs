using System;
using Microsoft.Extensions.DependencyInjection;

namespace Brunsker.Integracao.WorkService.JOB.Extensions
{
    public static class SwaggerServicesExtensions
    {
        public static IServiceCollection AddSwaggerServiceExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "",
                        Version = "v1",
                        Description = "",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "� 2020 BrunSker Tecnologia",
                            Url = new Uri("https://www.brunsker.com.br/")
                        },
                    }
                    );
            });

            return services;
        }
    }
}