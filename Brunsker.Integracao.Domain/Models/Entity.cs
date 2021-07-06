using System;

namespace Brunsker.Integracao.Domain.Models
{
    public abstract class Entity
    {
        public int SEQ_CLIENTE { get; set; }
        public string ROWID_TB { get; set; }
        public string STRING_BANCO { get; set; }
        public DateTime? DT_INSERT { get; set; }
    }
}