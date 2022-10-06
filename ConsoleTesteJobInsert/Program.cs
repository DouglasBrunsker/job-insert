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
        static async Task Main(string[] args)
        {

            await ReceiverMessageRabbitAsync();
        }


        public static async Task ReceiverMessageRabbitAsync()
        {
            try
            {
                EventingBasicConsumer consumer = null;
                string[] filas = new string[2] { "Testes", "Testes2" };
                var factory = new ConnectionFactory() { HostName = "168.138.250.55", UserName = "brunsker", Password = "brunsker$2020" };

                var _connection = factory.CreateConnection();
                var channel = _connection.CreateModel();


                string[] text = File.ReadAllLines(@"C:\Users\brunsker\Documents\teste.txt");
                string nfe = text[0];
                string nfs = text[1];
                string prod = text[2];

                byte[] body;

                for (int i = 0; i < 50000; i++)
                {
                    body = Encoding.UTF8.GetBytes(nfe.Replace("teste2", $"teste{i}"));
                    channel.BasicPublish(exchange: "",
                                         routingKey: "Testes",
                                         basicProperties: null,
                                         body: body);

                    body = Encoding.UTF8.GetBytes(nfs.Replace("4987996", $"{i}"));
                    channel.BasicPublish(exchange: "",
                                         routingKey: "Testes2",
                                         basicProperties: null,
                                         body: body);

                    body = Encoding.UTF8.GetBytes(prod.Replace("96934", $"{i}"));
                    channel.BasicPublish(exchange: "",
                                         routingKey: "Testes3",
                                         basicProperties: null,
                                         body: body);

                    //body = Encoding.UTF8.GetBytes(nfe.Replace("teste2", $"teste2{i}"));
                    //channel.BasicPublish(exchange: "",
                    //                     routingKey: "Testes4",
                    //                     basicProperties: null,
                    //                     body: body);

                    //body = Encoding.UTF8.GetBytes(nfe.Replace("teste2", $"teste2{i}"));
                    //channel.BasicPublish(exchange: "",
                    //                     routingKey: "Testes5",
                    //                     basicProperties: null,
                    //                     body: body);
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
