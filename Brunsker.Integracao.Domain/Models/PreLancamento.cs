using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class PreLancamento : Entity
    {
        public string CNPJ { get; set; }
        public DateTime DTVENC { get; set; }
        public double VALOR { get; set; }
        public long CODFORNEC { get; set; }
        public string HISTORICO { get; set; }
        public long NUMNOTA { get; set; }
        public int CODFILIAL { get; set; }
        public int INDICE { get; set; }
        public double VPAGO { get; set; }
        public string TIPOPARCEIRO { get; set; }
        public int DUPLIC { get; set; }
        public DateTime DTEMISSAO { get; set; }
    }
}