using Brunsker.Integracao.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brunsker.Integracao.Domain.Adapters
{
    public interface IOracleRepositoryAdapter
    {
        /*NOVO*/
        Task<IEnumerable<PreEntrada>> SelectPreEntrada();
        Task<IEnumerable<PreLancamento>> SelectLancamento();
        Task ConfirmaEnvioDadosApi(int seqCliente, string rowId, string sql);
        Task<List<Message>> PCESTAsync(List<Message> pcests, string package);
        Task<List<Message>> PCPRODFILIALAsync(List<Message> pcprodfiliais, string package);
        Task<List<Message>> PCTABPRAsync(List<Message> pctabprs, string package);
        Task<List<Message>> PCNEGFORNECAsync(List<Message> pcnegfornecs, string package);
        Task<List<Message>> PCTABTRIBENTAsync(List<Message> pctabtribents, string package);
        Task<List<Message>> PCTRIBENTRADAAsync(List<Message> pctribentradas, string package);
        Task<List<Message>> PCCFOAsync(List<Message> pccfos, string package);
        Task<List<Message>> PCCODFABRICA(List<Message> mensagens, string package);
        Task<List<Message>> PCPARCELASC(List<Message> mensagens, string package);
        Task<List<Message>> PCCONSUM(List<Message> mensagens, string package);
        Task<List<Message>> PREENTRADA_WINTHOR(List<Message> mensagens, string package);
        Task<IEnumerable<ConsultaCliente>> SelectConsultaCliente();
        Task ConfirmaEnvioDadosApi(ConsultaCliente consulta, string sql);

        /*END NOVO*/



        /// <summary>
        /// Realiza insert no banco brunsker dados retornados do cliente.
        /// Este adaptador consiste realizar envio ao
        /// serviço de integracao brunsker
        /// </summary>
        /// <param name="clientes">lista de  cliente</param>
        /// <param name="package">pkg que define insert ou update </param>
        Task<List<Message>> ClientesAsync(List<Message> clientes, string package);



        /// <summary>
        /// Realiza insert no banco brunsker dados retornados do cliente.
        /// Este adaptador consiste realizar envio ao
        /// serviço de integracao brunsker
        /// </summary>
        /// <param name = "departamentos" > lista de departamento</param>
        /// <param name="package">pkg que define insert ou update </param>
        Task<List<Message>> DepartamentoAsync(List<Message> departamentos, string package);


        /// <summary>
        /// Realiza insert no banco brunsker dados retornados do cliente.
        /// Este adaptador consiste realizar envio ao
        /// serviço de integracao brunsker
        /// </summary>
        /// <param name="filiais">lista de  filiais</param>
        /// <param name="package">pkg que define insert ou update </param>
        Task<List<Message>> FilialAsync(List<Message> filiais, string package);

        /// <summary>
        /// Realiza insert no banco brunsker dados retornados do cliente.
        /// Este adaptador consiste realizar envio ao
        /// serviço de integracao brunsker
        /// </summary>
        /// <param name="fornecedores">lista de fornecedor</param>
        /// <param name="package">pkg que define insert ou update </param>
        Task<List<Message>> FornecedorAsync(List<Message> fornecedores, string package);



        /// <summary>
        /// Realiza insert no banco brunsker dados retornados do cliente.
        /// Este adaptador consiste realizar envio ao
        /// serviço de integracao brunsker
        /// </summary>
        /// <param name="items">lista de itens do cliente</param>
        /// <param name="package">pkg que define insert ou update </param>
        Task<List<Message>> ItemAsync(List<Message> items, string package);



        /// <summary>
        /// Realiza insert no banco brunsker dados retornados do cliente.
        /// Este adaptador consiste realizar envio ao
        /// serviço de integracao brunsker
        /// </summary>
        /// <param name="lancamentos">lista de cliente</param>
        /// <param name="package">pkg que define insert ou update </param>
        Task<List<Message>> LancamentoAsync(List<Message> lancamentos, string package);



        /// <summary>
        /// Realiza insert no banco brunsker dados retornados do cliente.
        /// Este adaptador consiste realizar envio ao
        /// serviço de integracao brunsker
        /// </summary>
        /// <param name="movimentacoes">lista de movimentacoes do cliente</param>
        /// <param name="package">pkg que define insert ou update </param>
        Task<List<Message>> MovimentacaoAsync(List<Message> movimentacoes, string package);



        /// <summary>
        /// Realiza insert no banco brunsker dados retornados do cliente.
        /// Este adaptador consiste realizar envio ao
        /// serviço de integracao brunsker
        /// </summary>
        /// <param name="notasFiscaisEntrada">lista de NFE</param>
        /// <param name="package">lista de  cliente</param>
        Task<List<Message>> NotaFiscalEntradaAsync(List<Message> notasFiscaisEntrada, string package);

        /// <summary>
        /// Realiza insert no banco brunsker dados retornados do cliente.
        /// Este adaptador consiste realizar envio ao
        /// serviço de integracao brunsker
        /// </summary>
        /// <param name="notasFiscaisPreEntrada">Codigo do cliente</param>
        /// <param name="package">pkg que define insert ou update </param>
        Task<List<Message>> NotaFiscalPreEntradaAsync(List<Message> notasFiscaisPreEntrada, string package);


        /// <summary>
        /// Realiza insert no banco brunsker dados retornados do cliente.
        /// Este adaptador consiste realizar envio ao
        /// serviço de integracao brunsker
        /// </summary>
        /// <param name="notasFiscaisSaida">lista notas fiscais de saida</param>
        /// <param name="package">pkg que define insert ou update </param>
        Task<List<Message>> NotaFiscalSaidaAsync(List<Message> notasFiscaisSaida, string package);



        /// <summary>
        /// Realiza insert no banco brunsker dados retornados do cliente.
        /// Este adaptador consiste realizar envio ao
        /// serviço de integracao brunsker
        /// </summary>
        /// <param name="pedidos">lista de pedidos</param>
        /// <param name="package">pkg que define insert ou update </param>
        Task<List<Message>> PedidoAsync(List<Message> pedidos, string package);



        /// <summary>
        /// Realiza insert no banco brunsker dados retornados do cliente.
        /// Este adaptador consiste realizar envio ao
        /// serviço de integracao brunsker
        /// </summary>
        /// <param name="prest">Codigo do cliente</param>
        /// <param name="package">pkg que define insert ou update </param>
        Task<List<Message>> PrestAsync(List<Message> prest, string package);



        /// <summary>
        /// Realiza insert no banco brunsker dados retornados do cliente.
        /// Este adaptador consiste realizar envio ao
        /// serviço de integracao brunsker
        /// </summary>
        /// <param name="produtos">Codigo do cliente</param>
        /// <param name="package">pkg que define insert ou update </param>
        Task<List<Message>> ProdutoAsync(List<Message> produtos, string package);


        Task<List<Message>> XmlAsync(List<Message> xml, string package);

    }
}
