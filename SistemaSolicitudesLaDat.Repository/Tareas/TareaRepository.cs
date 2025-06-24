using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SistemaSolicitudesLaDat.Entities.Tareas;
using System.Data;

namespace SistemaSolicitudesLaDat.Repository.Tareas
{
    public class TareaRepository
    {
        private readonly string _connectionString;

        public TareaRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection Conexion() => new MySqlConnection(_connectionString);

        public async Task<IEnumerable<Tarea>> ObtenerPorSolicitud(string idSolicitud)
        {
            var query = "CALL obtener_tareas_por_solicitud(@idSolicitud)";
            using var db = Conexion();
            return await db.QueryAsync<Tarea>(query, new { idSolicitud });
        }

        public async Task<Tarea?> ObtenerPorId(string id)
        {
            var sql = @"
                SELECT 
                    id_tarea AS IdTarea,
                    descripcion AS Descripcion,
                    fecha AS Fecha,
                    horas_invertidas AS HorasInvertidas,
                    id_usuario AS IdUsuario,
                    id_solicitud AS IdSolicitud
                FROM tarea
                WHERE id_tarea = @id";
            using var db = Conexion();
            return await db.QueryFirstOrDefaultAsync<Tarea>(sql, new { id });
        }


        public async Task<bool> Insertar(Tarea t)
        { 
            using var db = Conexion();
            var p = new DynamicParameters();
            p.Add("pI_descripcion", t.Descripcion);
            p.Add("pI_fecha", t.Fecha);
            p.Add("pI_horas", t.HorasInvertidas);
            p.Add("pI_id_usuario", t.IdUsuario);
            p.Add("pI_id_solicitud", t.IdSolicitud);
            p.Add("pS_resultado", dbType: DbType.Int32, direction: ParameterDirection.Output);



            try
            {
                await db.ExecuteAsync("insertar_tarea", p, commandType: CommandType.StoredProcedure);
                var resultado = p.Get<int>("pS_resultado");
                Console.WriteLine($"Resultado del procedimiento: {resultado}");
                return resultado == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar tarea: {ex.Message}");
                throw;
            }
        }


        public async Task<bool> Actualizar(Tarea t)
        {
            var sql = @"
                UPDATE tarea SET
                    descripcion = @Descripcion,
                    fecha = @Fecha,
                    horas_invertidas = @HorasInvertidas,
                    id_solicitud = @IdSolicitud
                WHERE id_tarea = @IdTarea";

            using var db = Conexion();
            return await db.ExecuteAsync(sql, t) > 0;
        }

        public async Task<bool> Eliminar(string id)
        {
            var sql = "DELETE FROM tarea WHERE id_tarea = @id";
            using var db = Conexion();
            return await db.ExecuteAsync(sql, new { id }) > 0;
        }
    }
}
