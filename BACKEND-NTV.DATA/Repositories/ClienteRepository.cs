using BACKEND_NTV.DATA.Interfaces;
using BACKEND_NTV.DTOs.Usuario;
using BCrypt.Net;   // IMPORTANTE
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace BACKEND_NTV.DATA.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        #region PostgreSQL
        private readonly PostgreSQLConnection _connection;

        public ClienteRepository(PostgreSQLConnection connection)
        {
            _connection = connection;
        }

        protected NpgsqlConnection DbConnection()
        {
            return _connection.GetConnection();
        }
        #endregion

        public async Task<int> CrearClienteAsync(UsuarioCreateDTO dto)
        {
            using var connection = DbConnection();

            try
            {
                string hash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

                var parameters = new
                {
                    p_nombre = dto.Nombre,
                    p_correo = dto.Correo.ToLower().Trim(),
                    p_contrasena = hash
                };

                // ✅ EJECUTAR FUNCTION CON NOMBRE CORREGIDO
                var result = await connection.ExecuteScalarAsync<int>(
                    "SELECT fun_crear_cliente(@p_nombre, @p_correo, @p_contrasena);",
                    parameters
                );

                // ✅ MANEJAR CÓDIGOS DE ERROR
                if (result == -1)
                    throw new Exception("El correo ya está registrado");
                if (result == -2)
                    throw new Exception("Error en la base de datos al crear usuario");
                if (result <= 0)
                    throw new Exception($"Error desconocido: código {result}");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                throw;
            }
        }
    }
}


