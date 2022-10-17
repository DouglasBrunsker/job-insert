using Brunsker.Integracao.Domain.Models;
using Brunsker.Integracao.OracleAdapter;
using Dapper;
using Dapper.Oracle;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Brunsker.Integracao.WorkService
{

    internal class Program
    {
        private static DbConnectionDbRepositoryAdapter _config;
        

        static void Main(string[] args)
        {

            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .Build();

            string conexao = config["ConnectionString"];
            ConfigRabbit rabbitConfig = config.GetRequiredSection("RabbitMQ").Get<ConfigRabbit>();

            ConsumirRabbitMQ rabbitMQ = new ConsumirRabbitMQ(rabbitConfig, conexao);

            Console.ReadLine();
        }
        

    }
}
