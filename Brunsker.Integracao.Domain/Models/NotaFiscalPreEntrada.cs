using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class NotaFiscalPreEntrada : Entity
    {
        public int NUMNOTA { get; set; }
        public string ESPECIE { get; set; }
        public DateTime? DTEMISSAO { get; set; }
        public int CODFORNEC { get; set; }
        public string CGC { get; set; }
        public string FORNRCEDOR { get; set; }
        public string CHAVENFE { get; set; }
        public int CODDEVOL { get; set; }
        public int NUMTRANSENT { get; set; }
    }
}
