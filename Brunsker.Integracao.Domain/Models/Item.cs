using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class Item : Entity
    {
        public string NUMPED { get; set; }
        public long CODPROD { get; set; }
        public string MOEDA { get; set; }
        public DateTime? DTULTCOMPRA { get; set; }
        public DateTime? DTULTENT { get; set; }
        public decimal PERCICMRED { get; set; }
        public decimal PERICM { get; set; }
        public decimal QTENTREGUE { get; set; }
        public decimal QTPEDIDA { get; set; }
        public decimal PLIQUIDO { get; set; }
        public decimal PCOMPRA { get; set; }
        public int NUMSEQ { get; set; }
        public double MULTIPLOCOMPRAS { get; set; }
        public double PERIPI { get; set; }
        public double PERCIVA { get; set; }
        public double PERCALIQINT { get; set; }
        public double PERCALIQEXT { get; set; }
        public double PERCALIQEXTGUIA { get; set; }
        public double PERCREDICMS { get; set; }
        public double PERPIS { get; set; }
        public double PERCOFINS { get; set; }
    }
}
