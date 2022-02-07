using System.Threading.Tasks;
using Brunsker.Integracao.Domain.Models;
using Refit;

namespace Brunsker.Integracao.Domain.Services
{
    public interface IIntegracaoApi
    {
        [Post("/api/Integracao/v1/InserirPreEntrada")]
        Task EnviarPreEntradaAsync(DtoParametro dados);


        [Post("/api/Integracao/v1/InserirPreLancamento")]
        Task EnviarPreLancamentoAsync(DtoParametro dados);

        [Post("/api/Integracao/v1/EnviarStatusCliente")]
        Task EnviarStatusCliente(DtoParametro dados);

        [Post("/api/Integracao/v1/EnviarBuscaXML")]
        Task EnviarBuscaXMLAsync(DtoParametro dados);
    }
}