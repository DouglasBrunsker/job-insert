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

namespace Brunsker.Integracao.WorkService
{
    internal class Program
    {
        private static DbConnectionDbRepositoryAdapter _config;
        private static string conexao = "User Id=TESTERABBIT;Password=TESTERABBIT#2022;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=152.67.39.41)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=BRUNSKER)))";
        static void Main(string[] args)
        {

            //byte[] mensagens = null;
            try
            {
                EventingBasicConsumer consumer = null;

                //TODO mandar para o appsettings
                string[] filas = new string[] { "Testes", "Testes2", "Testes3", "Testes4", "Testes5" };
                var factory = new ConnectionFactory() { HostName = "168.138.250.55",
                                                        UserName = "brunsker",
                                                        Password = "brunsker$2020"
                                                        };


                var _connection = factory.CreateConnection();

                var channel = _connection.CreateModel();
                consumer = new EventingBasicConsumer(channel);

                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var fila = ea.RoutingKey.ToString();

                    var msg = Encoding.UTF8.GetString(body);
                    
                    Triagem(msg, fila);
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
                // _logger.LogError(e.InnerException.Message);
                Console.WriteLine(e.Message);
                throw;
            }

            Console.ReadLine();
        }


        public static async Task Triagem(string msg, string fila)
        {
            string package = "";
            switch (fila)
            {
                case ("Testes"):
                    package = "pkg_webserv_insert_bsnotas.PROC_INS_PCNFENT";
                    NotaFiscalEntradaAsync(msg, package);
                    break;

                case ("Testes2"):
                    package = "pkg_webserv_insert_bsnotas.PROC_INS_PCNFSAID";
                    NotaFiscalSaidaAsync(msg, package);
                    break;

                case ("Testes3"):
                    package = "pkg_webserv_insert_bsnotas.PROC_INS_PCPRODUT";
                    ProdutoAsync(msg, package);
                    break;

                //case ("Testes4"):
                //    package = "pkg_webserv_insert_bsnotas.PROC_INS_PCNFENT";
                //    NotaFiscalEntradaAsync(msg, package);
                //    break;

                //case ("Testes5"):
                //    package = "pkg_webserv_insert_bsnotas.PROC_INS_PCNFENT";
                //    NotaFiscalEntradaAsync(msg, package);
                //    break;

                default:
                    break;
            }

        }

        public static async Task NotaFiscalEntradaAsync(string notasFiscaisEntradaJson, string package)
        {


            using OracleConnection conn = new OracleConnection(conexao);

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


                await conn.ExecuteAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                //notasFiscaisEntradaJson.Where(x => x.DeliveryTag == notasFiscaisEntradaJson.DeliveryTag).ToList().ForEach(n => n.Executado = true);

            }
            catch (Exception e)
            {
                //  _logger.LogError(e.Message);
                Console.WriteLine(e.Message);
                // notasFiscaisEntradaJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
            }


        }

        public static async Task NotaFiscalSaidaAsync(string notasFiscaisSaidaJson, string package)
        {
            using OracleConnection conn = new OracleConnection(conexao);

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


                await conn.ExecuteAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                //notasFiscaisSaidaJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                Console.WriteLine(e.Message);
                // notasFiscaisSaidaJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
            }

        }

        public static async Task ProdutoAsync(string produtosJson, string package)
        {

            using OracleConnection conn = new OracleConnection(conexao);

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

                await conn.ExecuteAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                //produtosJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);
            }
            catch (Exception e)
            {
                // _logger.LogError(e.Message);
                Console.WriteLine(e.Message);
                // produtosJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
            }
        }

    }
}
