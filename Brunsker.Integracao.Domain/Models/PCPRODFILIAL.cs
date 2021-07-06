namespace Brunsker.Integracao.Domain.Models
{
    public class PCPRODFILIAL : Entity
    {
        public string CODFILIAL { get; set; }
        public int CODPROD { get; set; }
        public string PROIBIDAVENDA { get; set; }
        public string ORIGMERCTRIB { get; set; }
        public string FORALINHA { get; set; }
    }
}