using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class Departamento : Entity
    {
        public long CODEPTO { get; set; }
        public string DESCRICAO { get; set; }
        public string CODFORNEC { get; set; }
        public string FORNECEDOR { get; set; }
    }
}
