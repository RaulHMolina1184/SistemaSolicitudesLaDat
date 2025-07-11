using Dapper;
using SistemaSolicitudesLaDat.Entities.Desgloses;
using SistemaSolicitudesLaDat.Repository.Infrastructure;
using System.Data;

namespace SistemaSolicitudesLaDat.Repository.Desgloses
{
    public class DesgloseRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DesgloseRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Desglose>> GetAllAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var result = await connection.QueryAsync<Desglose, ImpuestoValorAgregado, Desglose>(
                "ListarDesgloses",
                (desglose, iva) =>
                {
                    desglose.Impuesto = iva;
                    return desglose;
                },
                commandType: CommandType.StoredProcedure,
                splitOn: "monto_iva"
            );

            return result;
        }

        public async Task<(IEnumerable<Desglose> Desgloses, int Total)> ObtenerDesglosesPaginadosAsync(string idSolicitud, int paginaActual, int pageSize)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("p_id_solicitud", idSolicitud);
            parameters.Add("p_PaginaActual", paginaActual);
            parameters.Add("p_pageSize", pageSize);

            using var multi = await connection.QueryMultipleAsync("Listar10Desglose", parameters, commandType: CommandType.StoredProcedure);

            var desgloses = multi.Read<Desglose, ImpuestoValorAgregado, Desglose>(
                (desglose, iva) =>
                {
                    desglose.Impuesto = iva;
                    return desglose;
                },
                splitOn: "id_iva" 
            );

            var total = await multi.ReadFirstAsync<int>();

            return (desgloses, total);
        }

        public async Task<Desglose?> GetByIdAsync(string id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Desglose>(
                "ListarPorIdDesglose",
                new { p_IdDesglose = id },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<int> ObtenerSiguienteNumeroAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var siguienteNumero = await connection.QueryFirstOrDefaultAsync<int>(
                "ObtenerSiguienteNumeroDesglose",
                commandType: CommandType.StoredProcedure);

            return siguienteNumero == 0 ? 1 : siguienteNumero;
        }

        public async Task<int> InsertAsync(Desglose desglose)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("p_id_desglose", desglose.id_desglose);
            parameters.Add("p_mes", desglose.mes);
            parameters.Add("p_anio", desglose.anio);
            parameters.Add("p_horas", desglose.horas);
            parameters.Add("p_monto", desglose.monto);
            parameters.Add("p_total", desglose.total);
            parameters.Add("p_observaciones", desglose.observaciones);
            parameters.Add("p_porcentaje_cobro", desglose.porcentaje_cobro);
            parameters.Add("p_id_solicitud", desglose.id_solicitud);
            parameters.Add("p_id_iva", desglose.id_iva);

            return await connection.ExecuteAsync(
                "InsertarDesglose",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(Desglose desglose)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("p_id_desglose", desglose.id_desglose);
            parameters.Add("p_mes", desglose.mes);
            parameters.Add("p_anio", desglose.anio);
            parameters.Add("p_horas", desglose.horas);
            parameters.Add("p_monto", desglose.monto);
            parameters.Add("p_total", desglose.total);
            parameters.Add("p_observaciones", desglose.observaciones);
            parameters.Add("p_porcentaje_cobro", desglose.porcentaje_cobro);
            parameters.Add("p_id_iva", desglose.id_iva);

            return await connection.ExecuteAsync(
                "ActualizarDesglose",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> EliminarAsync(string idDesglose)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_id_desglose", idDesglose?.Trim(), DbType.String, ParameterDirection.Input);
            parameters.Add("p_result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var connection = _dbConnectionFactory.CreateConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            await connection.ExecuteAsync(
                "EliminarDesglose",
                parameters,
                commandType: CommandType.StoredProcedure);

            int resultado = parameters.Get<int>("p_result");
            return resultado == 1;
        }
    }
}
