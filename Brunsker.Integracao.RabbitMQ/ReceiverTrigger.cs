using Brunsker.Integracao.Domain.Adapters;
using Brunsker.Integracao.Domain.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EasyNetQ;
using RabbitMQ.Client.Events;

namespace Brunsker.Integracao.RabbitMQ
{

    public class ReceiverTrigger : IRabbitMqAdapter
    {
        private BasicGetResult result { get; set; }
        private readonly IOracleRepositoryAdapter _oracleRepositoryAdapter;
        private RabbitMqConfiguration _rabbitConnection { get; }
        private readonly ILogger<ReceiverTrigger> _logger;

        public ReceiverTrigger(IOracleRepositoryAdapter oracleRepositoryAdapter, IOptions<RabbitMqConfiguration> rabbitConnection, ILogger<ReceiverTrigger> logger)
        {
            _rabbitConnection = rabbitConnection.Value;

            _oracleRepositoryAdapter = oracleRepositoryAdapter;

            _logger = logger;
        }

        public byte[] RecieveMessageRabbitAsync()
        {
            byte[] mensagens = null;
            try
            {
                EventingBasicConsumer consumer = null;
                string[] filas = new string[] { "Testes", "Testes2" , "Testes3", "Testes4" , "Testes5" };
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



            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.Message);


            }

            return mensagens;
        }


        //                    case Contexto.Integracao_Clientes:
        //                        retornoExecucao = await _oracleRepositoryAdapter.ClientesAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCCLIENT");
        //                        break;

        //                    case Contexto.Integracao_AtualizarClientes:
        //                        retornoExecucao = await _oracleRepositoryAdapter.ClientesAsync(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCCLIENT");
        //                        break;

        //                    case Contexto.Integracao_Departamento:
        //                        retornoExecucao = await _oracleRepositoryAdapter.DepartamentoAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCDEPTO");
        //                        break;

        //                    case Contexto.Integracao_AtualizarDepartamento:
        //                        retornoExecucao = await _oracleRepositoryAdapter.DepartamentoAsync(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCDEPTO");
        //                        break;

        //                    case Contexto.Integracao_Filial:
        //                        retornoExecucao = await _oracleRepositoryAdapter.FilialAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCFILIAL");
        //                        break;

        //                    case Contexto.Integracao_AtualizarFilial:
        //                        retornoExecucao = await _oracleRepositoryAdapter.FilialAsync(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCFILIAL");
        //                        break;

        //                    case Contexto.Integracao_Fornecedor:
        //                        retornoExecucao = await _oracleRepositoryAdapter.FornecedorAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCFORNEC");
        //                        break;

        //                    case Contexto.Integracao_AtualizarFornecedor:
        //                        retornoExecucao = await _oracleRepositoryAdapter.FornecedorAsync(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCFORNEC");
        //                        break;

        //                    case Contexto.Integracao_Item:
        //                        retornoExecucao = await _oracleRepositoryAdapter.ItemAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCITEM");
        //                        break;

        //                    case Contexto.Integracao_AtualizarItem:
        //                        retornoExecucao = await _oracleRepositoryAdapter.ItemAsync(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCITEM");
        //                        break;

        //                    case Contexto.Integracao_DelItem:
        //                        retornoExecucao = await _oracleRepositoryAdapter.DelItemAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_DEL_PCITEM");
        //                        break;

        //                    case Contexto.Integracao_Lancamento:
        //                        retornoExecucao = await _oracleRepositoryAdapter.LancamentoAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCLANC");
        //                        break;

        //                    case Contexto.Integracao_AtualizarLancamento:
        //                        retornoExecucao = await _oracleRepositoryAdapter.LancamentoAsync(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCLANC");
        //                        break;

        //                    case Contexto.Integracao_Movimentacao:
        //                        retornoExecucao = await _oracleRepositoryAdapter.MovimentacaoAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCMOV");
        //                        break;

        //                    case Contexto.Integracao_NotaFiscalEntrada:
        //                        retornoExecucao = await _oracleRepositoryAdapter.NotaFiscalEntradaAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCNFENT");
        //                        break;

        //                    case Contexto.Integracao_AtualizarNotaFiscalEntrada:
        //                        retornoExecucao = await _oracleRepositoryAdapter.NotaFiscalEntradaAsync(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCNFENT");
        //                        break;

        //                    case Contexto.Integracao_NotaFiscalPreEntrada:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PREENTRADA_WINTHOR(mensagens, "pkg_webserv_insert_bsnotas.proc_ins_preentrada_winthor");
        //                        break;
        //                    case Contexto.Integracao_AtualizarNotaFiscalPreEntrada:
        //                        // retornoExecucao = await _oracleRepositoryAdapter.NotaFiscalPreEntradaAsync(listaMensagens, "");
        //                        break;

        //                    case Contexto.Integracao_NotaFiscalSaida:
        //                        retornoExecucao = await _oracleRepositoryAdapter.NotaFiscalSaidaAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCNFSAID");
        //                        break;

        //                    case Contexto.Integracao_AtualizarNotaFiscalSaida:
        //                        retornoExecucao = await _oracleRepositoryAdapter.NotaFiscalSaidaAsync(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCNFSAID");
        //                        break;

        //                    case Contexto.Integracao_Pedido:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PedidoAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCPEDIDO");
        //                        break;

        //                    case Contexto.Integracao_AtualizarPedido:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PedidoAsync(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCPEDIDO");
        //                        break;

        //                    case Contexto.Integracao_DelPedido:
        //                        retornoExecucao = await _oracleRepositoryAdapter.DelPedidoAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_DEL_PCPEDIDO");
        //                        break;

        //                    case Contexto.Integracao_Prest:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PrestAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCPREST");
        //                        break;

        //                    case Contexto.Integracao_AtualizarPrest:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PrestAsync(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCPREST");
        //                        break;

        //                    case Contexto.Integracao_Produto:
        //                        retornoExecucao = await _oracleRepositoryAdapter.ProdutoAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCPRODUT");
        //                        break;

        //                    case Contexto.Integracao_AtualizarProduto:
        //                        retornoExecucao = await _oracleRepositoryAdapter.ProdutoAsync(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCPRODUT");
        //                        break;

        //                    case Contexto.Integracao_Xml:
        //                        retornoExecucao = await _oracleRepositoryAdapter.XmlAsync(mensagens, "pkg_webserv_insert_bsnotas.INSERT_XML");
        //                        break;

        //                    case Contexto.Integracao_PCDOCELETRONICO:
        //                        retornoExecucao = await _oracleRepositoryAdapter.DocEletronicoAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCDOCELETRONICO");
        //                        break;
        //                    /*NOVO*/

        //                    case Contexto.Integracao_PCCFO:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCCFOAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCCFO");
        //                        break;

        //                    case Contexto.Integracao_PCEST:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCESTAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCEST");
        //                        break;

        //                    case Contexto.Integracao_PCNEGFORNEC:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCNEGFORNECAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCNEGFORNEC");
        //                        break;

        //                    case Contexto.Integracao_PCPRODFILIAL:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCPRODFILIALAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCPRODFILIAL");
        //                        break;

        //                    case Contexto.Integracao_PCTABPR:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCTABPRAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCTABPR");
        //                        break;

        //                    case Contexto.Integracao_AtualizacaoPCTABPR:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCTABPRAsync(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCTABPR");
        //                        break;

        //                    case Contexto.Integracao_PCTABTRIBENT:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCTABTRIBENTAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCTABTRIBENT");
        //                        break;

        //                    case Contexto.Integracao_PCTRIBENTRADA:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCTRIBENTRADAAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCTRIBENTRADA");
        //                        break;

        //                    case Contexto.Integracao_PCCODFABRICA:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCCODFABRICA(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCCODFABRICA");
        //                        break;

        //                    case Contexto.Integracao_AtualizacaoPCCODFABRICA:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCCODFABRICA(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCCODFABRICA");
        //                        break;


        //                    case Contexto.Integracao_PCPARCELASC:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCPARCELASC(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCPARCELASC");
        //                        break;

        //                    case Contexto.Integracao_AtualizacaoPCPARCELASC:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCPARCELASC(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCPARCELASC");
        //                        break;


        //                    case Contexto.Integracao_PCCONSUM:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCCONSUM(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCCONSUM");
        //                        break;

        //                    case Contexto.Integracao_AtualizacaoPCCONSUM:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCCONSUM(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCCONSUM");
        //                        break;

        //                    case Contexto.Integracao_PCCONTAS:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCCONTA(mensagens, "pkg_webserv_insert_bsnotas.PROC_INC_PCCONTA");
        //                        break;

        //                    case Contexto.Integracao_AtualizacaoPCCONTAS:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCCONTA(mensagens, "pkg_webserv_update_bsnotas.PROC_UPD_PCCONTA");
        //                        break;

        //                    case Contexto.Integracao_PCESTCOM:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCESTCOM(mensagens, "pkg_webserv_insert_bsnotas.PROC_INC_PCESTCOM");
        //                        break;

        //                    case Contexto.Integracao_PCNFBASEENT:
        //                        retornoExecucao = await _oracleRepositoryAdapter.PCNFBASEENTAsync(mensagens, "pkg_webserv_insert_bsnotas.PROC_INS_PCNFBASEENT");
        //                        break;




    }
}
