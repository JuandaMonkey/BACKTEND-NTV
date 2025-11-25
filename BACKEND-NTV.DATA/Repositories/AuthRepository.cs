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

        #region Ejemplo Jalando HAAAY MAMAAAAA
        public async Task<IEnumerable<UsuarioModel>> getUsuarios()
        {
            var usuarios = new List<UsuarioModel>();

            try
            {
                await using var database = DbConnection();
                await database.OpenAsync();

                string sqlQuery = @"select * from public.usuario;";

                var resutl = (await database.QueryAsync<UsuarioModel>(
                    sqlQuery)).ToList();

                return resutl;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener usuarios." + ex);
            }
            return usuarios;
        }
        #endregion
    }
}
