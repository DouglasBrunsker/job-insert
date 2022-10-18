using Brunsker.Integracao.Domain.Models;
using Dapper;
using Dapper.Oracle;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brunsker.Integracao.WorkService
{
    internal class ConsumirRabbitMQ
    {
        private static ConfigRabbit _rabbit;
        private static string _connection;
        private static readonly ILogger<ConsumirRabbitMQ> _logger;
        private static List<FilaMorta> list = null;

        //TODO apagar depois
        #region Testes. apagar dps
        //private const string WorkerExchange = "Producao.exchange";
        //private const string RetryExchange = "Fila Morta.exchange";
        //public const string WorkerQueue = "Producao";
        //private const string RetryQueue = "Fila Morta";
        #endregion

        public ConsumirRabbitMQ(ConfigRabbit rabbit, string connection)
        {
            _rabbit = rabbit;
            _connection = connection;
            ConsomeRabbit();
        }

        public static void ConsomeRabbit()
        {
            try
            {
                EventingBasicConsumer consumer = null;

                string queue = _rabbit.Queue;
                string[] types = _rabbit.Types.Split(";");
                var factory = new ConnectionFactory()
                {
                    HostName = _rabbit.HostName,
                    UserName = _rabbit.UserName,
                    Password = _rabbit.Password
                };

                var _connection = factory.CreateConnection();
                var channel = _connection.CreateModel();

                consumer = new EventingBasicConsumer(channel);

                //TODO apagar depois
                #region Teste apagar depois
                //channel.QueuePurge("Producao");
                //channel.QueuePurge("Fila Morta");
                //channel.QueueDelete("Producao");
                //channel.QueueDelete("Fila Morta");
                //channel.ExchangeDelete("Producao.exchange");
                //channel.ExchangeDelete("Fila Morta.exchange");
                //channel.ExchangeDeclare(WorkerExchange, "direct", durable: true);
                //channel.QueueDeclare
                //(
                //    WorkerQueue, true, false, false,
                //    new Dictionary<string, object>
                //    {
                //        {"x-dead-letter-exchange", RetryExchange},
                //        //{"x-dead-letter-routing-key", RetryQueue}
                //    }
                //);

                //channel.ExchangeDeclare(RetryExchange, "fanout", durable: true);
                //channel.QueueDeclare
                //(
                //    RetryQueue, true, false, false,
                //    new Dictionary<string, object> {
                //        { "x-dead-letter-exchange", WorkerExchange }
                //    }
                //);
                //channel.QueueBind(RetryQueue, RetryExchange, string.Empty, null);

                //consumer.Received += (model, ea) =>
                //{
                //    var body = ea.Body.ToArray();
                //    var message = Encoding.UTF8.GetString(body);
                //    Console.WriteLine(" [x] Received {0}", message);

                //    Thread.Sleep(1000);
                //    Console.WriteLine("Rejected message");

                //    channel.BasicReject(ea.DeliveryTag, false);
                //};

                //channel.BasicConsume(WorkerQueue, false, consumer);


                #endregion

                consumer.Received += async (model, ea) =>
                {
                    var type = ea.BasicProperties.Type;
                    var body = ea.Body.ToArray();

                    var msg = Encoding.UTF8.GetString(body);

                    bool successful = await Sorting(msg, type);

                    if (successful)
                    {
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        channel.BasicReject(ea.DeliveryTag, false);
                        list.Add(new FilaMorta
                        {
                            DateTime = DateTime.Now,
                            Item = msg,
                            Type = type
                        });
                    }
                };
                channel.BasicConsume(queue: queue,
                                        autoAck: false,
                                        consumer: consumer);

            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                ConsoleColor cor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = cor;
            }

            Console.ReadLine();
        }


        public static async Task<bool> Sorting(string msg, string type)
        {
            bool successful;
            string package;
            switch (type)
            {
                case ("Integracao_NotaFiscalEntrada"):
                    package = "pkg_webserv_insert_bsnotas.PROC_INS_PCNFENT";
                    successful = await NotaFiscalEntradaAsync(msg, package);
                    break;

                case ("Integracao_NotaFiscalSaida"):
                    package = "pkg_webserv_insert_bsnotas.PROC_INS_PCNFSAID";
                    successful = await NotaFiscalSaidaAsync(msg, package);
                    break;

                case ("Integracao_Produto"):
                    package = "pkg_webserv_insert_bsnotas.PROC_INS_PCPRODUT";
                    successful = await ProdutoAsync(msg, package);
                    break;

                case ("Integracao_Consulta_Cliente"):
                    package = "PKG_BS_CONSULTAS.CONSULTAR_CLIENTES";
                    successful = await ConsultaClienteAsync(msg, package);
                    break;

                default:
                    successful = false;
                    break;
            }
            return successful;
        }

        private static Task<bool> ConsultaClienteAsync(string msg, string package)
        {
            throw new NotImplementedException();
        }

        public static async Task<bool> NotaFiscalEntradaAsync(string notasFiscaisEntradaJson, string package)
        {


            using OracleConnection conn = new OracleConnection(_connection);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            try
            {
                var nf = JsonConvert.DeserializeObject<NotaFiscalEntrada>(notasFiscaisEntradaJson);

                OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                dynamicParameters.Add("pNUMNOTA", nf.NUMNOTA);
                dynamicParameters.Add("pSERIE", nf.SERIE);
                dynamicParameters.Add("pESPECIE", nf.ESPECIE);
                dynamicParameters.Add("pDTEMISSAO", nf.DTEMISSAO);
                dynamicParameters.Add("pCODFORNEC", nf.CODFORNEC);
                dynamicParameters.Add("pFORNECEDOR", nf.FORNECEDOR);
                dynamicParameters.Add("pCGC", nf.CGC);
                dynamicParameters.Add("pCGCFILIAL", nf.CGCFILIAL);
                dynamicParameters.Add("pSEQ_CLIENTE", nf.SEQ_CLIENTE);
                dynamicParameters.Add("pCHAVENFE", nf.CHAVENFE);
                dynamicParameters.Add("pCODDEVOL", nf.CODDEVOL);
                dynamicParameters.Add("pNUMTRANSENT", nf.NUMTRANSENT);
                dynamicParameters.Add("pTIPODESCARGA", nf.TIPODESCARGA);
                dynamicParameters.Add("pDTCANCEL", nf.DTCANCEL);
                dynamicParameters.Add("pROWID", nf.ROWID_TB);
                dynamicParameters.Add("pSTRING_BANCO", nf.STRING_BANCO);
                dynamicParameters.Add("pDTENT", nf.DTENT);

                await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                return true;
            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                ConsoleColor cor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = cor;

                var listaux = list.Find(l => l.Item.Equals(notasFiscaisEntradaJson));
                list.Remove(listaux);
                listaux.LogErro = e.Message + e.StackTrace;
                list.Add(listaux);

                return false;
            }
        }

        public static async Task<bool> NotaFiscalSaidaAsync(string notasFiscaisSaidaJson, string package)
        {
            using OracleConnection conn = new OracleConnection(_connection);

            if (conn.State == ConnectionState.Closed) conn.Open();


            try
            {
                var nfs = JsonConvert.DeserializeObject<NotaFiscalSaida>(notasFiscaisSaidaJson);

                OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                dynamicParameters.Add("pSEQ_CLIENTE", nfs.SEQ_CLIENTE);
                dynamicParameters.Add("pNUMNOTA", nfs.NUMNOTA);
                dynamicParameters.Add("pSERIE", nfs.SERIE);
                dynamicParameters.Add("pDTSAIDA", nfs.DTSAIDA);
                dynamicParameters.Add("pVLTOTAL", nfs.VLTOTAL);
                dynamicParameters.Add("pVLBONIFIC", nfs.VLBONIFIC);
                dynamicParameters.Add("pCODCLI", nfs.CODCLI);
                dynamicParameters.Add("pNUMTRANSVENDA", nfs.NUMTRANSVENDA);
                dynamicParameters.Add("pCHAVENFE", nfs.CHAVENFE);
                dynamicParameters.Add("pSITUACAONFE", nfs.SITUACAONFE);
                dynamicParameters.Add("pTIPOVENDA", nfs.TIPOVENDA);
                dynamicParameters.Add("pCONDVENDA", nfs.CONDVENDA);
                dynamicParameters.Add("pCODFISCAL", nfs.CODFISCAL);
                dynamicParameters.Add("pCGCENT", nfs.CGCENT);
                dynamicParameters.Add("pCGCFILIAL", nfs.CGCFILIAL);
                dynamicParameters.Add("pIEENT", nfs.IEENT);
                dynamicParameters.Add("pDTCANCEL", nfs.DTCANCEL);
                dynamicParameters.Add("pSTRING_BANCO", nfs.STRING_BANCO);

                await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                return true;

            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                ConsoleColor cor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = cor;

                var listaux = list.Find(l => l.Item.Equals(notasFiscaisSaidaJson));
                list.Remove(listaux);
                listaux.LogErro = e.Message + e.StackTrace;
                list.Add(listaux);

                return false;
            }

        }

        public static async Task<bool> ProdutoAsync(string produtosJson, string package)
        {

            using OracleConnection conn = new OracleConnection(_connection);

            if (conn.State == ConnectionState.Closed) conn.Open();

            try
            {
                var prod = JsonConvert.DeserializeObject<Produto>(produtosJson);

                OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                dynamicParameters.Add("pROWID", prod.ROWID_TB);
                dynamicParameters.Add("pSEQ_CLIENTE", prod.SEQ_CLIENTE);
                dynamicParameters.Add("pCODPROD", prod.CODPROD);
                dynamicParameters.Add("pDESCRICAO", prod.DESCRICAO);
                dynamicParameters.Add("pEMBALAGEM", prod.EMBALAGEM);
                dynamicParameters.Add("pUNIDADE", prod.UNIDADE);
                dynamicParameters.Add("pCODFORNEC", prod.CODFORNEC);
                dynamicParameters.Add("pPERICM", prod.PERICM);
                dynamicParameters.Add("pPERCST", prod.PERCST);
                dynamicParameters.Add("pCODAUXILIAR", prod.CODAUXILIAR);
                dynamicParameters.Add("pCODAUXILIAR2", prod.CODAUXILIAR2);
                dynamicParameters.Add("pCODFAB", prod.CODFAB);
                dynamicParameters.Add("pNBM", prod.NBM);
                dynamicParameters.Add("pDTEXCLUSAO", prod.DTEXCLUSAO);
                dynamicParameters.Add("pCODFILIAL", prod.CODFILIAL);
                dynamicParameters.Add("pOBS", prod.OBS);
                dynamicParameters.Add("pOBS2", prod.OBS2);
                dynamicParameters.Add("pCODNCMEX", prod.CODNCMEX);
                dynamicParameters.Add("pPRECIFICESTRANGEIRA", prod.PRECIFICESTRANGEIRA);
                dynamicParameters.Add("pTIPOMERC", prod.TIPOMERC);
                dynamicParameters.Add("pPERCIPI", prod.PERCIPI);
                dynamicParameters.Add("pPERCALIQINT", prod.PERCALIQINT);
                dynamicParameters.Add("pPERCALIQEXT", prod.PERCALIQEXT);
                dynamicParameters.Add("pPERCIVA", prod.PERCIVA);
                dynamicParameters.Add("pCODFISCAL", prod.CODFISCAL);
                dynamicParameters.Add("pSTRING_BANCO", prod.STRING_BANCO);
                dynamicParameters.Add("pQTUNIT", prod.QTUNIT);
                dynamicParameters.Add("pQTUNITCX", prod.QTUNITCX);

                if (package != "pkg_webserv_update_bsnotas.PROC_UPD_PCPRODUT")
                {
                    dynamicParameters.Add("pDTINSERT", prod.DT_INSERT);
                }

                await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                return true;

            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                ConsoleColor cor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = cor;

                var listaux = list.Find(l => l.Item.Equals(produtosJson));
                list.Remove(listaux);
                listaux.LogErro = e.Message + e.StackTrace;
                list.Add(listaux);

                return false;
            }
        }
    }
}
