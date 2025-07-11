using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SistemaSolicitudesLaDat.Entities.Solicitudes;
using SistemaSolicitudesLaDat.Repository.Infrastructure;

namespace SistemaSolicitudesLaDat.Repository.Solicitudes
{
    public class EstadoSolicitudRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public EstadoSolicitudRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<EstadoSolicitud>> ObtenerTodosAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<EstadoSolicitud>(
                "ListarEstadoSolicitud",
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<(IEnumerable<EstadoSolicitud> Estados, int Total)> GetPagedAsync(int pagina, int tamanoPagina)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(
                "Listar10EstadosSolicitud",
                new { p_Pagina = pagina, p_TamanoPagina = tamanoPagina },
                commandType: CommandType.StoredProcedure);

            var estados = await multi.ReadAsync<EstadoSolicitud>();
            var totalObj = await multi.ReadFirstAsync();
            int total = (int)totalObj.TotalFilas;

            return (estados, total);
        }

        public async Task<EstadoSolicitud?> GetByIdAsync(string id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<EstadoSolicitud>(
                "ListarPorIdEstadoSolicitud",
                new { p_IdEstado = id },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<string?> ObtenerIdPorNombreAsync(string nombreEstado)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("p_nombre_estado", nombreEstado, DbType.String, ParameterDirection.Input);
            parameters.Add("p_id_estado", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("ObtenerIdEstadoPorNombre", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<string?>("p_id_estado");
        }


        public async Task<EstadoSolicitud?> GetByNombreAsync(string nombreEstado)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<EstadoSolicitud>(
                "ListarPorNombreEstadoSolicitud",
                new { p_NombreEstado = nombreEstado },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<int> CreateAsync(EstadoSolicitud estado)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteAsync(
                "InsertarEstadoSolicitud",
                new
                {
                    p_IdEstado = estado.id_estado,
                    p_Estado = estado.Estado
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(EstadoSolicitud estado)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteAsync(
                "ActualizarEstadoSolicitud",
                new
                {
                    p_IdEstado = estado.id_estado,
                    p_Estado = estado.Estado
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> TieneRelacionAsync(string id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(
                "ValidarRelacionEstadoSolicitud",
                new { p_IdEstado = id },
                commandType: CommandType.StoredProcedure
            );

            return result > 0;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            using var connection = _connectionFactory.CreateConnection();

            if (await TieneRelacionAsync(id))
                return false;

            await connection.ExecuteAsync(
                "EliminarEstadoSolicitud",
                new { p_IdEstado = id },
                commandType: CommandType.StoredProcedure
            );

            return true;
        }
    }
}
