using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class NotaFiscalSaida : Entity
    {
        public long NUMNOTA { get; set; }
        public string VLTOTAL { get; set; }
        public long CODCLI { get; set; }
        public long NUMTRANSVENDA { get; set; }
        public string CHAVENFE { get; set; }
        public string CGCENT { get; set; }
        public string IEENT { get; set; }
        public string SERIE { get; set; }
        public string CGCFILIAL { get; set; }
        public long? CODFORNECFRETE { get; set; }
        public DateTime? DTSAIDA { get; set; }
        public DateTime? DTCANCEL { get; set; }
        public long? SITUACAONFE { get; set; }
        public string TIPOVENDA { get; set; }
        public long CONDVENDA { get; set; }
        public long CODFISCAL { get; set; }
    }
}
