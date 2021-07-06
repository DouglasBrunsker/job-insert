namespace Brunsker.Integracao.Domain.Models
{
    public class Movimentacao : Entity
    {
        public long NUMPED { get; set; }
        public long NUMNOTA { get; set; }
        public long CODPROC { get; set; }
        public decimal QT { get; set; }
        public long CODFORNEC { get; set; }
        public long CODFILIAL { get; set; }
        public string CHAVENFE { get; set; }
        public long CODAUXILIAR { get; set; }
    }
}
