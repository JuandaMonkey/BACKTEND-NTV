using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace BACKEND_NTV.DATA
{
    public class PostgreSQLConnection
    {
        private readonly PostgreSQLConfiguration _config;

        public PostgreSQLConnection(PostgreSQLConfiguration config)
        {
            _config = config;
        }

        public NpgsqlConnection GetConnection()
        {
            return _config.GetConnection();
        }
    }

    // Esta clase maneja la configuración de PostgreSQL
    public class PostgreSQLConfiguration
    {
        private readonly string? _connectionString;

        public PostgreSQLConfiguration(IConfiguration configuration)
        {
            // Primero intenta appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            // Si es null, intenta la variable de entorno
            if (string.IsNullOrEmpty(_connectionString))
                _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            if (string.IsNullOrEmpty(_connectionString))
                throw new InvalidOperationException("La cadena de conexión no está configurada.");
        }

        public NpgsqlConnection GetConnection()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new InvalidOperationException("La cadena de conexión no está configurada.");

            return new NpgsqlConnection(_connectionString);
        }
    }
}
