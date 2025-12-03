using BACKEND_NTV.DATA;
using BACKEND_NTV.DATA.Interfaces;
using BACKEND_NTV.MODEL;
using Dapper;
using Npgsql;

namespace BACKEND_NTV.DATA.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly PostgreSQLConnection _connection;

        public AuthRepository(PostgreSQLConnection connection)
        {
            _connection = connection;
        }

        protected NpgsqlConnection DbConnection()
        {
            return _connection.GetConnection();
        }

        public async Task<UsuarioModel> ObtenerUsuarioPorCorreoAsync(string correo)
        {
            using var connection = DbConnection();

            // ✅ CONSULTA SEGURA CON PARÁMETROS
            const string query = @"
                SELECT 
                    idusuario as IdUsuario,
                    nombre as Nombre,
                    correo as Correo,
                    contrasena as Contrasena,
                    fk_idrol as FkIdRol
                FROM usuario
                WHERE correo = @Correo";

            var usuario = await connection.QueryFirstOrDefaultAsync<UsuarioModel>(
                query,
                new { Correo = correo.ToLower().Trim() } // ✅ PARÁMETRO SEGURO
            );

            return usuario;
        }

        public async Task<bool> VerificarContrasenaAsync(string contrasenaPlana, string contrasenaHash)
        {
            // ✅ VERIFICACIÓN SEGURA CON BCRYPT
            return await Task.Run(() =>
                BCrypt.Net.BCrypt.Verify(contrasenaPlana, contrasenaHash)
            );
        }

        public async Task<bool> RegistrarLogAsync(int usuarioId, string accion)
        {
            using var connection = DbConnection();

            const string query = @"
                INSERT INTO log_sesiones (usuario_id, accion, fecha)
                VALUES (@UsuarioId, @Accion, NOW())";

            try
            {
                var result = await connection.ExecuteAsync(query, new
                {
                    UsuarioId = usuarioId,
                    Accion = accion
                });
                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ExisteTokenEnBlacklistAsync(string token)
        {
            using var connection = DbConnection();

            const string query = @"
                SELECT COUNT(1) 
                FROM token_blacklist 
                WHERE token = @Token AND expiracion > NOW()";

            var count = await connection.ExecuteScalarAsync<int>(query, new { Token = token });
            return count > 0;
        }

        public async Task<bool> AgregarTokenABlacklistAsync(string token, DateTime expiracion)
        {
            using var connection = DbConnection();

            const string query = @"
                INSERT INTO token_blacklist (token, expiracion, fecha_agregado)
                VALUES (@Token, @Expiracion, NOW())";

            try
            {
                var result = await connection.ExecuteAsync(query, new
                {
                    Token = token,
                    Expiracion = expiracion
                });
                return result > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}