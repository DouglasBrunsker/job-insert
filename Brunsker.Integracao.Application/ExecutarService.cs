using Brunsker.Integracao.Domain.Adapters;
using Brunsker.Integracao.Domain.Models;
using Brunsker.Integracao.Domain.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Brunsker.Integracao.Application
{
    public class ExecutarService : IExecutarService
    {
        private readonly ILogger _logger;
        private readonly IRabbitMqAdapter _rabbitMqAdapter;
        private readonly IIntegracaoApi _refit;
        private readonly IOracleRepositoryAdapter _rep;
        public ExecutarService(ILoggerFactory loggerFactory, IRabbitMqAdapter rabbitMqAdapter, IIntegracaoApi refit, IOracleRepositoryAdapter rep)
        {
            _logger = loggerFactory?.CreateLogger<ExecutarService>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            _rabbitMqAdapter = rabbitMqAdapter ?? throw new ArgumentNullException(nameof(rabbitMqAdapter));

            _refit = refit;

            _rep = rep;
        }
        public async Task ExecutarProcessoAsync()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            _logger.LogInformation("Inicio da execucao de processamento, :" + " " + DateTime.Now);

            await ProcessamentoPCESTCOM();
            
            await ProcessamentoNotasFiscaisSaida();


            await ProcessamentoPCCONTAS();

            await ProcessamentoConsultaClienteSefaz();

            await ProcessamentoNotasFiscaisPreEntrada();

            await ProcessamentoFornecedores();

            await ProcessamentoPreEntrada();
            await ProcessamentoPreLancamento();
            await ProcessamentoPCPARCELASC();
            await ProcessamentoPCCONSUM();
            await ProcessamentoPCCODFABRICA();
            await ProcessamentoProdutos();
            await ProcessamentoPedidos();
            await ProcessamentoPCTABPR();
            await ProcessamentoPCEST();
            await ProcessamentoPCCFO();
            await ProcessamentoPCNEGFORNEC();
            await ProcessamentoPCPRODFILIAL();
            await ProcessamentoPCTABTRIBENT();
            await ProcessamentoPCTRIBENTRADA();
            await ProcessamentoFiliais();
            await ProcessamentoXml();
            await ProcessamentoLancamentos();

            await ProcessamentoClientes();
            await ProcessamentoDepartamentos();
            await ProcessamentoItens();
            await ProcessamentoMovimentacoes();
            await ProcessamentoNotasFiscaisEntrada();
            await ProcessamentoPrest();

            _logger.LogInformation("Fim da execucao de processamento, :" + " " + DateTime.Now + "TempoExecucao:" + " " + sw.Elapsed.TotalMinutes + "Minutos");

            sw.Stop();
        }
        private async Task ProcessamentoPCESTCOM()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_PCESTCOM);
        }
        private async Task ProcessamentoPCCONTAS()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_PCCONTAS);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizacaoPCCONTAS);
        }
        private async Task ProcessamentoPCCONSUM()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_PCCONSUM);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizacaoPCCONSUM);
        }
        private async Task ProcessamentoPCPARCELASC()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_PCPARCELASC);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizacaoPCPARCELASC);
        }
        private async Task ProcessamentoPCCODFABRICA()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_PCCODFABRICA);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizacaoPCCODFABRICA);
        }
        private async Task ProcessamentoPCEST()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_PCEST);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizacaoPCEST);
        }
        private async Task ProcessamentoPCCFO()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_PCCFO);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizacaoPCCFO);
        }
        private async Task ProcessamentoPCNEGFORNEC()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_PCNEGFORNEC);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizacaoPCNEGFORNEC);
        }
        private async Task ProcessamentoPCPRODFILIAL()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_PCPRODFILIAL);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizacaoPCPRODFILIAL);
        }
        private async Task ProcessamentoPCTABPR()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_PCTABPR);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizacaoPCTABPR);
        }
        private async Task ProcessamentoPCTABTRIBENT()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_PCTABTRIBENT);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizacaoPCTABTRIBENT);
        }
        private async Task ProcessamentoPCTRIBENTRADA()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_PCTRIBENTRADA);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizacaoPCTRIBENTRADA);
        }
        private async Task ProcessamentoConsultaClienteSefaz()
        {
            try
            {
                var consultas = await _rep.SelectConsultaCliente();

                if (consultas.Any())
                {
                    foreach (var consulta in consultas)
                    {
                        await _refit.EnviarStatusCliente(new DtoParametro { dados = JsonConvert.SerializeObject(consulta) });

                        await _rep.ConfirmaEnvioDadosApi(consulta, "pkg_clientes_nfe.PROC_UPD_CLIENTE_ALETRADOS");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        private async Task ProcessamentoPreEntrada()
        {
            try
            {
                var preEntradas = await _rep.SelectPreEntrada();

                if (preEntradas.Any())
                {
                    foreach (var preEntrada in preEntradas)
                    {
                        await _refit.EnviarPreEntradaAsync(new DtoParametro { dados = JsonConvert.SerializeObject(preEntrada) });

                        await _rep.ConfirmaEnvioDadosApi(preEntrada.SEQ_CLIENTE, preEntrada.ROWID_TB, "pkg_pre_entrada.PROC_OK_PCMOVPREENT");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        private async Task ProcessamentoPreLancamento()
        {
            try
            {
                var preLancamentos = await _rep.SelectLancamento();

                if (preLancamentos.Any())
                {
                    foreach (var preLancamento in preLancamentos)
                    {
                        await _refit.EnviarPreLancamentoAsync(new DtoParametro { dados = JsonConvert.SerializeObject(preLancamento) });

                        await _rep.ConfirmaEnvioDadosApi(preLancamento.SEQ_CLIENTE, preLancamento.ROWID_TB, "pkg_pre_entrada.PROC_OK_PCLANCPREENT");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        private async Task ProcessamentoXml()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_Xml);
        }
        private async Task ProcessamentoClientes()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_Clientes);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizarClientes);
        }
        private async Task ProcessamentoDepartamentos()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_Departamento);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizarDepartamento);
        }
        private async Task ProcessamentoFiliais()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_Filial);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizarFilial);
        }
        private async Task ProcessamentoFornecedores()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_Fornecedor);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizarFornecedor);
        }
        private async Task ProcessamentoItens()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_Item);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizarItem);
        }
        private async Task ProcessamentoLancamentos()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_Lancamento);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizarLancamento);
        }
        private async Task ProcessamentoMovimentacoes()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_Movimentacao);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizarMovimentacao);
        }
        private async Task ProcessamentoNotasFiscaisEntrada()
        {

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_NotaFiscalEntrada);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizarNotaFiscalEntrada);
        }
        private async Task ProcessamentoNotasFiscaisPreEntrada()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_NotaFiscalPreEntrada);

        }
        private async Task ProcessamentoNotasFiscaisSaida()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_NotaFiscalSaida);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizarNotaFiscalSaida);
        }
        private async Task ProcessamentoPedidos()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_Pedido);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizarPedido);
        }
        private async Task ProcessamentoPrest()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_Prest);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizarPrest);
        }
        private async Task ProcessamentoProdutos()
        {
            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_Produto);

            await _rabbitMqAdapter.ReceiverMessageAsync(Contexto.Integracao_AtualizarProduto);
        }
    }
}
