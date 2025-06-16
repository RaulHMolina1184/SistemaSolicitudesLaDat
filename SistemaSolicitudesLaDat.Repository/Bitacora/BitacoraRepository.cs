using Dapper;
using Newtonsoft.Json;
using SistemaSolicitudesLaDat.Repository.Infrastructure;
using System.Data;

namespace SistemaSolicitudesLaDat.Repository.Bitacora
{
    
    
    public class BitacoraRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public BitacoraRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> RegistrarAccionAsync(string idUsuario, string descripcion, object accionesJson, string? idSolicitud = null)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("pI_usuario_ejecutor", string.IsNullOrEmpty(idUsuario) ? DBNull.Value : idUsuario, DbType.String); // Para validar si el ID de usuario es nulo o vacío (operador ternario)
                parameters.Add("pI_descripcion_accion", descripcion, DbType.String);
                parameters.Add("pI_listado_acciones", JsonConvert.SerializeObject(accionesJson), DbType.String);
                parameters.Add("pI_id_solicitud",string.IsNullOrWhiteSpace(idSolicitud) ? DBNull.Value : idSolicitud,DbType.String);
                parameters.Add("pS_resultado", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "insertar_bitacora",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int resultado = parameters.Get<int>("pS_resultado");
                return resultado == 1; 
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RegistrarErrorAsync(string idUsuario, string errorDetalle, string? idSolicitud = null)
        {
            try
            {
                var descripcion = "Error técnico";
                var detalle = new { error = errorDetalle };

                return await RegistrarAccionAsync(idUsuario, descripcion, JsonConvert.SerializeObject(detalle), idSolicitud);
            }
            catch (Exception ex)
            {
                // Loguear el error si es necesario
                Console.WriteLine($"Error al registrar en bitácora: {ex.Message}");
                return false;
            }
        }
    }
}
