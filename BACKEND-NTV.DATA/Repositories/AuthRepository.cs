using BACKEND_NTV.DATA.Interfaces;
using BACKEND_NTV.MODEL;
using Npgsql;
using Dapper;

namespace BACKEND_NTV.DATA.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        #region PostgreSQL
        PostgreSQLConnection? _connection;

        public AuthRepository(PostgreSQLConnection connection)
        {
            _connection = connection;
        }

        protected NpgsqlConnection DbConnection()
        {
            if (_connection == null)
                throw new InvalidOperationException("La conexión no ha sido inicializada.");

            return _connection.GetConnection();
        }
        #endregion
    }
}
