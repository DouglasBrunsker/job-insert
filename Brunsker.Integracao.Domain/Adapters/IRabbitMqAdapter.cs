using Brunsker.Integracao.Domain.Models;
using System.Threading.Tasks;

namespace Brunsker.Integracao.Domain.Adapters
{
    public interface IRabbitMqAdapter
    {

        byte[] RecieveMessageRabbitAsync();
    }
}
