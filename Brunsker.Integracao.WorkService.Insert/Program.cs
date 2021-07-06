using Brunsker.Integracao.WorkService.Insert.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Brunsker.Integracao.WorkService.Insert
{
    class Program
    {
        async static Task Main(string[] args)
        {

            IConfigurationBuilder configBuilderForMain = new ConfigurationBuilder();
            ConfigureConfiguration(configBuilderForMain);
            IConfiguration configForMain = configBuilderForMain.Build();


            var asService = !(Debugger.IsAttached || args.Contains("--console"));

            var builder = new HostBuilder()

            .ConfigureServices((hostContext, services) =>
            {
                services.AddWorkerExtensionServices(configForMain);
            });

            builder.UseEnvironment(asService ? Environments.Production : Environments.Development);

            if (asService)
            {
                await builder.RunAsServiceAsync();
            }
            else
            {
                await builder.RunConsoleAsync();
            }
        }

        public static void ConfigureConfiguration(IConfigurationBuilder config)
        {
            config.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        }
    }
}
