using System;

namespace Brunsker.Integracao.Domain.Models
{
    public class ArquivoXml
    {

        public TipoArquivoXml TipoXml { get; set; }
        public string Conteudo { get; set; }
        public string NOME_ARQUIVO { get; set; }
        public int SEQ_CLIENTE { get; set; }
        public string STRING_BANCO { get; set; }
        public long NUM_LOTE_NFE { get; set; }
        public string CHAVENFE { get; set; }
        public int ORIGEM { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataLeitura { get; set; }
    }
}
