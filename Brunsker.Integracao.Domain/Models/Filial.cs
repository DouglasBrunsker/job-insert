namespace Brunsker.Integracao.Domain.Models
{
    public class Filial : Entity
    {
        public int CODIGO { get; set; }
        public string CGC { get; set; }
        public string RAZAOSOCIAL { get; set; }
        public string ESTADO { get; set; }
        public string VSEQ_CLIENTE { get; set; }
        public string VSTRING_BANCO { get; set; }
        public long NUMREGIAOPADRAO { get; set; }
    }
}
