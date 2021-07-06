using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class Cliente : Entity
    {
        public int CODCLI { get; set; }
        public string CLIENTE { get; set; }
        public string ESTENT { get; set; }
        public string CGCENT { get; set; }
        public string IEENT { get; set; }
        public string BLOQUEIO { get; set; }
        public string BLOQUEIOSEFAZ { get; set; }
        public string BLOQUEIOSEFAZPED { get; set; }
        public string BLOQUEIODEFINITIVO { get; set; }
        public string BLOQUEIOINATIVIDADE { get; set; }
    }
}
