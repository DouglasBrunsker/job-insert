using System;
using System.Collections.Generic;
using System.Text;

namespace Brunsker.Integracao.WorkService
{
    internal class ClientConfiguration
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int ClientId { get; set; }
        public string ServiceName { get; set; }
        public string PathNfeEntrada { get; set; }
        public string PathNfeSaida { get; set; }
        public string PathNfeCte { get; set; }
        public string PathNfeMdfe { get; set; }
        public string PathNfeNfce { get; set; }
        public string PathPadrao { get; set; }
        public string DataInicioEnvioXml { get; set; }
        public string DataFimEnvioXml { get; set; }
        public bool EnvioXmlAtivo { get; set; }
        public string ConnectionString { get; set; }
        public string ConnectionStringTest { get; set; }
    }
}
