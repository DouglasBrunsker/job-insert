namespace Brunsker.Integracao.Domain.Models
{
    public class Fornecedor : Entity
    {
        public string CODFORNEC { get; set; }
        public string FORNECEDOR { get; set; }
        public string CNPJ { get; set; }
        public string DEPARTAMENTO { get; set; }
        public string BLOQUEIOSEFAZFORNEC { get; set; }
        public string ESTADO { get; set; }
        public string REVENDA { get; set; }
        public int PRAZOENTREGA { get; set; }
        public string BLOQUEIO { get; set; }
        public long CODFORNECPRINC { get; set; }
        public string CODCONTAB { get; set; }
    }
}
