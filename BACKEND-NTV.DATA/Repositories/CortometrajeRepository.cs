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
using NpgsqlTypes;

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
        public async Task<CortometrajesResultDTO> GetCortometrajes(CortometrajeFilter filters)
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

                return new CortometrajesResultDTO
                {
                    Exito = true,
                    Mensaje = "Cortometrajes obtenidos correctamente.",
                    Data = data,
                    Total = total
                };
            }
            catch (Exception ex)
            {
                return new CortometrajesResultDTO
                {
                    Exito = false,
                    Mensaje = "Error al obtener cortometrajes: " + ex.Message,
                    Data = new List<CortometrajeDTO>(),
                    Total = 0
                };
            }
        }
        #endregion

        #region GetCortometrajeById
        public async Task<CortometrajeResultDTO> GetCortometrajeById(int idCortometraje)
        {
            try
            {
                await using var database = DbConnection();
                await database.OpenAsync();

                var sqlQuery = @"select * from f_get_cortometraje_by_id(@IdCortometraje);";

                var sqlParams = new
                {
                    IdCortometraje = idCortometraje
                };

                var data = (await database.QueryAsync<CortometrajeDTO>(
                    sqlQuery,
                    sqlParams)).FirstOrDefault();

                if (data == null)
                {
                    return new CortometrajeResultDTO
                    {
                        Exito = false,
                        Mensaje = "No se encontró el cortometraje solicitado.",
                        Data = null
                    };
                }

                return new CortometrajeResultDTO
                {
                    Exito = true,
                    Mensaje = "Cortometraje obtenido correctamente.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new CortometrajeResultDTO
                {
                    Exito = false,
                    Mensaje = "Error al obtener cortometraje: " + ex.Message,
                    Data = null
                };
            }
        }
        #endregion

        #region PostCortometraje
        public async Task<CortometrajeResultDTO> PostCortometraje(CortometrajeCreateDTO cortometraje)
        {
            try
            {
                await using var database = DbConnection();
                await database.OpenAsync();

                var sqlQuery = @"
                select f_post_cortometraje(
                    @Titulo,
                    @Autor,
                    @Descripcion,
                    @UrlVideo,
                    @UrlPortada,
                    @DuracionMinutos,
                    @DuracionSegundos,
                    @AnioLanzamiento,
                    @Estado,
                    @Categorias);";

                var sqlParams = new DynamicParameters();

                sqlParams.Add("@Titulo", cortometraje.Titulo);
                sqlParams.Add("@Autor", cortometraje.Autor);
                sqlParams.Add("@Descripcion", cortometraje.Descripcion);
                sqlParams.Add("@UrlVideo", cortometraje.UrlVideo);
                sqlParams.Add("@UrlPortada", cortometraje.UrlPortada);
                sqlParams.Add("@DuracionMinutos", cortometraje.DuracionMinutos);
                sqlParams.Add("@DuracionSegundos", cortometraje.DuracionSegundos);
                sqlParams.Add("@AnioLanzamiento", cortometraje.AnioLanzamiento);
                sqlParams.Add("@Estado", cortometraje.Estado);
                sqlParams.Add("@Categorias", cortometraje.Categorias.ToArray());


                var newId = (await database.QueryAsync<int>(
                    sqlQuery,
                    sqlParams)).First();

                var result = await GetCortometrajeById(newId);

                result.Mensaje = "Cortometraje agregado correctamente.";

                return result;
            }
            catch (Exception ex)
            {
                return new CortometrajeResultDTO
                {
                    Exito = false,
                    Mensaje = "Error al agregar cortometraje: " + ex.Message,
                    Data = null
                };
            }
        }
        #endregion

        #region PutCortometraje
        public async Task<CortometrajeResultDTO> PutCortometraje(
            int idCortometraje,
            CortometrajeUpdateDTO cortometrajeUpdate,
            CortometrajeCategoriasUpdateDTO categoriaUpdate)
        {
            try
            {
                await using var database = DbConnection();
                await database.OpenAsync();

                var sqlQuery = @"
                select f_put_cortometraje(
                    @IdCortometraje,
                    @Titulo,
                    @Autor,
                    @Descripcion,
                    @UrlVideo,
                    @UrlPortada,
                    @DuracionMinutos,
                    @DuracionSegundos,
                    @AnioLanzamiento,
                    @Estado);";

                var sqlParams = new DynamicParameters();

                sqlParams.Add("@IdCortometraje", idCortometraje);
                sqlParams.Add("@Titulo", cortometrajeUpdate.Titulo);
                sqlParams.Add("@Autor", cortometrajeUpdate.Autor);
                sqlParams.Add("@Descripcion", cortometrajeUpdate.Descripcion);
                sqlParams.Add("@UrlVideo", cortometrajeUpdate.UrlVideo);
                sqlParams.Add("@UrlPortada", cortometrajeUpdate.UrlPortada);
                sqlParams.Add("@DuracionMinutos", cortometrajeUpdate.DuracionMinutos);
                sqlParams.Add("@DuracionSegundos", cortometrajeUpdate.DuracionSegundos);
                sqlParams.Add("@AnioLanzamiento", cortometrajeUpdate.AnioLanzamiento);
                sqlParams.Add("@Estado", cortometrajeUpdate.Estado);

                if (categoriaUpdate?.Categorias != null)
                {
                    var sqlQuery2 = @"
                    select f_put_categorias_cortometraje(
                        @IdCortometraje,
                        @Categorias);";

                    var sqlParams2 = new DynamicParameters();

                    sqlParams2.Add("@IdCortometraje", idCortometraje);
                    sqlParams2.Add("@Categorias", categoriaUpdate.Categorias.ToArray());

                    await database.QueryAsync(
                    sqlQuery2,
                    sqlParams2);
                }

                await database.QueryAsync<int>(sqlQuery, sqlParams);

                var result = await GetCortometrajeById(idCortometraje);

                result.Mensaje = "Cortometraje actualizado correctamente.";

                return result;
            }
            catch (Exception ex)
            {
                return new CortometrajeResultDTO
                {
                    Exito = false,
                    Mensaje = "Error al actualizar cortometraje: " + ex.Message,
                    Data = null
                };
            }
        }
        #endregion

        #region DeleteCortometraje
        public async Task<CortometrajeResultDTO> DeleteCortometraje(int idCortometraje)
        {
            try
            {
                await using var database = DbConnection();
                await database.OpenAsync();

                var sqlQuery = @"select f_delete_cortometraje(@IdCortometraje);";

                var sqlParams = new
                {
                    IdCortometraje = idCortometraje
                };

                await database.QueryAsync(
                    sqlQuery,
                    sqlParams);

                return new CortometrajeResultDTO
                {
                    Exito = true,
                    Mensaje = "Cortometraje eliminado correctamente.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new CortometrajeResultDTO
                {
                    Exito = false,
                    Mensaje = "Error al eliminar cortometraje: " + ex.Message,
                    Data = null
                };
            }
        }
        #endregion
    }
}
