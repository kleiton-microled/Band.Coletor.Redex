using System;

namespace Band.Coletor.Redex.Infra.Repositorios.Sql
{
    /// <summary>
    /// Representa erros que ocorrem na camada de acesso a dados.
    /// </summary>
    public class DataAccessException : ApplicationException
    {
        public DataAccessException() { }

        public DataAccessException(string message)
            : base(message) { }

        public DataAccessException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
