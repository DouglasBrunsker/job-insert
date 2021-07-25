using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class PCNFBASEENT : Entity
    {
        public string CODFILIALNF { get; set; }
        public long NUMTRANSENT { get; set; }
        public DateTime DTENTRADA { get; set; }
        public DateTime DTEMISSAO { get; set; }
        public string ESPECIE { get; set; }
        public string SERIE { get; set; }
        public long NUMNOTA { get; set; }
        public long CODFORNEC { get; set; }
        public string UF { get; set; }
        public decimal VLTOTAL { get; set; }
        public string CODCONT { get; set; }
        public long CODFISCAL { get; set; }
        public string OBS { get; set; }
    }
}