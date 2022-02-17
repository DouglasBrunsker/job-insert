using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class PCDOCELETRONICO : Entity
    {
        public long NUMTRANSACAO { get; set; }
        public string XMLNFE { get; set; }
        public string XMLNFCE { get; set; }
        public DateTime? DTDC { get; set; }
        public string MOVIMENTO { get; set; }
    }
}