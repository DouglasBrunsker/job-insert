namespace Brunsker.Integracao.Domain.Models
{
    public class Message
    {
        public ulong DeliveryTag { get; set; }
        public string Content { get; set; }
        public bool Executado { get; set; }
    }
}
