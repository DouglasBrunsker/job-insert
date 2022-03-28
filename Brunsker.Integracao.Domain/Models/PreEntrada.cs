using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class PreEntrada : Entity
    {
        public string CODBARRAS { get; set; }
        public string CODFAB { get; set; }
        public string CODFORNEC { get; set; }
        public double QT { get; set; }
        public double QTCOM { get; set; }
        public double PUNIT { get; set; }
        public string TIPOMOVIMENTACAO { get; set; }
        public string PRECOULTIMAENTRADA { get; set; }
        public int PRAZOENTREGA { get; set; }
        public double BASEICMS { get; set; }
        public double PERCST { get; set; }
        public double BCST { get; set; }
        public double VLBASEIPI { get; set; }
        public double VLIPI { get; set; }
        public double PERCIPI { get; set; }
        public long NUMNOTA { get; set; }
        public int CODFILIAL { get; set; }
        public double PICMS { get; set; }
        public long NUMPEDPREENT { get; set; }
        public double ICMS { get; set; }
        public double PICMSST { get; set; }
        public double BCPIS { get; set; }
        public double VPIS { get; set; }
        public double VCOFINS { get; set; }
        public double CSTIPI { get; set; }
        public int CODENQIPI { get; set; }
        public double VBCFCP { get; set; }
        public double VBCFCPST { get; set; }
        public double VFCP { get; set; }
        public double VFCPST { get; set; }
        public int SERIE { get; set; }
        public DateTime DTEMISSAO { get; set; }
        public double VLTOTALFRETECIF { get; set; }
        public double VLTOTALDESCONTO { get; set; }
        public double VLTOTDESCONTO { get; set; }
        public double VLTOTALNF { get; set; }
        public double VLTOTIPI { get; set; }
        public string OBS { get; set; }
        public DateTime DTSAIDA { get; set; }
        public double VLTOTFRETE { get; set; }
        public double VLTOTALST { get; set; }
        public double BASEICST { get; set; }
        public long NSU { get; set; }
        public double VLTOTALICMS { get; set; }
        public double VLTOTALBASEICMS { get; set; }
        public double VLBASEICST { get; set; }
        public double VLTOTALPROD { get; set; }
        public string CHAVENFE { get; set; }
        public string UF { get; set; }
        public int FINALIDADENFE { get; set; }
        public string CGCCLIENTE { get; set; }
        public string PLACAVEICULO { get; set; }
        public string UFVEICULO { get; set; }
        public decimal PERCOFINS { get; set; }
        public decimal PERPIS { get; set; }
        public decimal ST { get; set; }
        public string CONVERTERUNIDADE { get; set; }
        public decimal CODFISCAL { get; set; }
        public string UCOM { get; set; }
        public string UTRIB { get; set; }
        public string TIPOFRETE { get; set; }
        public long NUMSEQ { get; set; }
        public decimal VLOUTRASDESP { get; set; }
    }
}