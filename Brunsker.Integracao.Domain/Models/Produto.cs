using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class Produto : Entity
    {
        public int CODPROD { get; set; }
        public string DESCRICAO { get; set; }
        public string EMBALAGEM { get; set; }
        public string UNIDADE { get; set; }
        public int CODFORNEC { get; set; }
        public double PERICM { get; set; }
        public double PERCST { get; set; }
        public string CODAUXILIAR { get; set; }
        public string CODAUXILIAR2 { get; set; }
        public string CODFAB { get; set; }
        public string NBM { get; set; }
        public string CODFILIAL { get; set; }
        public string OBS { get; set; }
        public string OBS2 { get; set; }
        public string CODNCMEX { get; set; }
        public string PRECIFICESTRANGEIRA { get; set; }
        public string TIPOMERC { get; set; }
        public double PERCIPI { get; set; }
        public double PERCALIQINT { get; set; }
        public double PERCALIQEXT { get; set; }
        public double PERCIVA { get; set; }
        public DateTime? DTEXCLUSAO { get; set; }
        public long CODFISCAL { get; set; }
    }
}

