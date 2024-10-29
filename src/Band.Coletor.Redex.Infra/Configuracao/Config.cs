using System.Configuration;

namespace Band.Coletor.Redex.Infra.Configuracao
{
    public static class Config
    {
        public static string StringConexao()
          => ConfigurationManager.ConnectionStrings["StringConexaoSqlServer"].ConnectionString;
    }
}