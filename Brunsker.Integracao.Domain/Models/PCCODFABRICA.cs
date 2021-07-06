namespace Brunsker.Integracao.Domain.Models
{
    public class PCCODFABRICA : Entity
    {
        public long CODPROD { get; set; }
        public long CODFORNEC { get; set; }
        public string CODFAB { get; set; }
        public string FATOR { get; set; }
    }
}