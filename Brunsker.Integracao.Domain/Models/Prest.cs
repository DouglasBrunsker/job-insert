using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class Prest : Entity
    {
        public int CODCLI { get; set; }
        public string PREST { get; set; }
        public string CLIENTE { get; set; }
        public string DUPLIC { get; set; }
        public decimal VALOR { get; set; }
        public DateTime? DTVENC { get; set; }
        public string CODCOB { get; set; }
        public decimal VPAGO { get; set; }
        public DateTime? DTPAG { get; set; }
        public DateTime? DTEMISSAO { get; set; }
        public string CODFILIAL { get; set; }
        public decimal VALORDESC { get; set; }
        public DateTime? DTBAIXA { get; set; }
        public DateTime? DTDESD { get; set; }
        public DateTime? DTFECHA { get; set; }
        public string NUMTRANSVENDA { get; set; }
        public string NUMPED { get; set; }
        public DateTime? DTINSERT { get; set; }
    }
}
