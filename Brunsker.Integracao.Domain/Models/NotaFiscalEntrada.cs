using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class NotaFiscalEntrada : Entity
    {
        public int NUMNOTA { get; set; }
        public string SERIE { get; set; }
        public string ESPECIE { get; set; }
        public DateTime? DTEMISSAO { get; set; }
        public int CODFORNEC { get; set; }
        public string CGC { get; set; }
        public string CGCFILIAL { get; set; }
        public string FORNECEDOR { get; set; }
        public string CHAVENFE { get; set; }
        public string CODDEVOL { get; set; }
        public string NUMTRANSENT { get; set; }
        public DateTime? DTCANCEL { get; set; }
        public string TIPODESCARGA { get; set; }
        public DateTime? DTENT { get; set; }
    }
}
