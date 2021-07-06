using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class Pedido : Entity
    {
        public long NUMPED { get; set; }
        public DateTime? DATAEMISSAO { get; set; }
        public double VLTOTAL { get; set; }
        public long CODFORNEC { get; set; }
        public string FORNECEDOR { get; set; }
        public string CODFILIAL { get; set; }
        public string CGC { get; set; }
        public DateTime? DTFATUR { get; set; }
        public long CODPARCELA { get; set; }
    }
}
