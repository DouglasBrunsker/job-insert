using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class Lancamento : Entity
    {
        public long RECNUM { get; set; }
        public DateTime? DTLANC { get; set; }
        public string CGC { get; set; }
        public string HISTORICO { get; set; }
        public string HISTORICO2 { get; set; }
        public long CODFORNEC { get; set; }
        public long NUMNOTA { get; set; }
        public string DUPLIC { get; set; }
        public decimal VALOR { get; set; }
        public DateTime? DTVENC { get; set; }
        public decimal VPAGO { get; set; }
        public DateTime? DTPAGTO { get; set; }
        public long NUMTRANSENT { get; set; }
    }
}
