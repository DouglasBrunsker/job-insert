using Brunsker.Integracao.Domain.Adapters;
using Brunsker.Integracao.Domain.Models;
using Brunsker.Integracao.Domain.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brunsker.Integracao.Application
{
    public class ExecutarService : IExecutarService
    {
        private readonly ILogger<ExecutarService> _logger;
        private readonly IRabbitMqAdapter _rabbitMqAdapter;
        private readonly IIntegracaoApi _refit;
        private readonly IOracleRepositoryAdapter _rep;
        public ExecutarService(ILogger<ExecutarService> logger, IRabbitMqAdapter rabbitMqAdapter,
                               IIntegracaoApi refit, IOracleRepositoryAdapter rep)
        {
            _logger = logger;

            _rabbitMqAdapter = rabbitMqAdapter;

            _refit = refit;

            _rep = rep;
        }
        public async Task ExecutarProcessoAsync()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            _logger.LogInformation("Inicio da execucao de processamento, :" + " " + DateTime.Now);
         
            var msg = _rabbitMqAdapter.RecieveMessageRabbitAsync();

           var t = Encoding.UTF8.GetString(msg);

            _logger.LogInformation("Fim da execucao de processamento, :" + " " + DateTime.Now + "TempoExecucao:" + " " + sw.Elapsed.TotalMinutes + "Minutos");

            sw.Stop();
        }
        private byte[] RecieveMessageRabbitAsync()
        {
            byte[] mensagens = null;
            try
            {
                EventingBasicConsumer consumer = null;
                string[] filas = new string[] { "Testes", "Testes2", "Testes3", "Testes4", "Testes5" };
                var factory = new ConnectionFactory() { HostName = "168.138.250.55", UserName = "brunsker", Password = "brunsker$2020" };

                var _connection = factory.CreateConnection();

                var channel = _connection.CreateModel();
                consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    mensagens = ea.Body.ToArray();

                };

                channel.BasicConsume(queue: filas[0],
                                        autoAck: true,
                                        consumer: consumer);
                channel.BasicConsume(queue: filas[1],
                                        autoAck: true,
                                        consumer: consumer);
                channel.BasicConsume(queue: filas[2],
                                       autoAck: true,
                                       consumer: consumer);
                channel.BasicConsume(queue: filas[3],
                                       autoAck: true,
                                       consumer: consumer);
                channel.BasicConsume(queue: filas[4],
                                       autoAck: true,
                                       consumer: consumer);



            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.Message);


            }

            return mensagens;
        }


    }
}


