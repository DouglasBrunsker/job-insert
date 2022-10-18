using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ConsoleTesteJobInsert
{
    public class Program
    {
        static void Main(string[] args)
        {
            PublishMessageRabbit();
        }

        public static void PublishMessageRabbit()
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "144.22.203.207", UserName = "admin", Password = "brunsker" };
                var routingKey = "Producao";

                var _connection = factory.CreateConnection();
                var channel = _connection.CreateModel();


                string[] text = File.ReadAllLines(@"C:\Users\brunsker\Documents\teste.txt");
                string nfe = text[0];
                string nfs = text[1];
                string prod = text[2];

                byte[] body;

                var properties = channel.CreateBasicProperties();
                
                for (int i = 0; i < 50000; i++)
                {
                    properties.Type = "Integracao_NotaFiscalEntrada";
                    body = Encoding.UTF8.GetBytes(nfe.Replace("teste2", $"teste{i}"));
                    channel.BasicPublish(exchange: "",
                                         routingKey: routingKey,
                                         basicProperties: properties,
                                         body: body);

                    properties.Type = "Integracao_NotaFiscalSaida";
                    body = Encoding.UTF8.GetBytes(nfs.Replace("4987996", $"{i}"));
                    channel.BasicPublish(exchange: "",
                                         routingKey: routingKey,
                                         basicProperties: properties,
                                         body: body);

                    properties.Type = "Integracao_Produto";
                    body = Encoding.UTF8.GetBytes(prod.Replace("96934", $"{i}"));
                    channel.BasicPublish(exchange: "",
                                         routingKey: routingKey,
                                         basicProperties: properties,
                                         body: body);

                    properties.Type = "Integracao_Consulta_Cliente";
                    body = Encoding.UTF8.GetBytes(prod.Replace("1286", $"{i}"));
                    channel.BasicPublish(exchange: "",
                                         routingKey: routingKey,
                                         basicProperties: properties,
                                         body: body);

                    Thread.Sleep(1);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
