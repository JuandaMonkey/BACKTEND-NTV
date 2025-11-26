using BACKEND_NTV.DATA.Interfaces;
using Npgsql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACKEND_NTV.DTOs.Cortometraje;
using BACKEND_NTV.DATA.Filters;
using System.Reflection.Metadata;

namespace BACKEND_NTV.DATA.Repositories
{
    public class CortometrajeRepository : ICortometrajeRepository
    {
        #region PostgreSQL
        PostgreSQLConnection? _connection;

        public CortometrajeRepository(PostgreSQLConnection connection)
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

        #region GetCortometrajes
        public async Task<CortometrajeResultDTO> GetCortometrajes(CortometrajeFilter filters)
        {
            try
            {
                await using var database = DbConnection();
                await database.OpenAsync();

                string sqlQuery = @"select * from f_get_cortometrajes(
                        @Titulo, 
                        @DuracionMinutos, 
                        @DuracionSegundos,   
                        @Anio, 
                        @Estado, 
                        @Categoria,
                        @Skip,
                        @Take);";

                string sqlTotal = @"select count(*)
                                    from cortometraje c
                                    where (@Titulo is null or c.titulo ilike '%' || @Titulo || '%')
                                        and (@DuracionMinutos is null or c.duracionminutos >= @DuracionMinutos)
                                        and (@Anio is null or c.aniolanzamiento = @Anio)
                                        and (@Estado is null or c.estado = @Estado);";

                var sqlParams = new
                {
                    Titulo = filters.Titulo ?? null,
                    DuracionMinutos = filters.DuracionMinutos ?? null,
                    DuracionSegundos = filters.DuracionSegundos ?? null,
                    Anio = filters.Anio ?? null,
                    Estado = filters.Estado ?? null,
                    Categoria = filters.Categoria ?? null,
                    Skip = filters.Skip ?? 0,
                    Take = filters.Take ?? 10
                };

                var data = (await database.QueryAsync<CortometrajeDTO>(
                    sqlQuery,
                    sqlParams)).ToList();

                int total = await database.ExecuteScalarAsync<int>(
                    sqlTotal,
                    sqlParams);

                return new CortometrajeResultDTO
                {
                    Data = data,
                    Total = total
                };
            }
            catch (NpgsqlException)
            {
                Console.Error.WriteLine("Error de base de datos.");
                throw;
            }
            catch (TimeoutException)
            {
                Console.Error.WriteLine("Timeout al consultar la base de datos.");
                throw;
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Ha ocurrido un error inesperado.");
                throw;
            }
        }
        #endregion
    }
}
