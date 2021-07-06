using System.Data;

namespace Brunsker.Integracao.OracleAdapter
{
    public class DbConnectionDbRepositoryAdapter
    {
        public IDbConnection Connection { get; set; }
        public IDbConnection ConnectionLocal { get; set; }
        public DbConnectionDbRepositoryAdapter(IDbConnection connection, IDbConnection connectionLocal)
        {
            Connection = connection;
            ConnectionLocal = connectionLocal;
        }
    }
}
