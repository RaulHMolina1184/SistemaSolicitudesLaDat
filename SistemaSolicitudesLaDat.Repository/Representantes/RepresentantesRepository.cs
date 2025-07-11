using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaSolicitudesLaDat.Entities.Representantes;
using SistemaSolicitudesLaDat.Repository.Infrastructure;

namespace SistemaSolicitudesLaDat.Repository.Representantes
{
    public class RepresentantesRepository
    {
        
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public RepresentantesRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Representante?> GetByIdAsync(string id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Representante>(
                "ListarPorIdRepresentante",
                new { p_IdRepresentante = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Representante>> ObtenerTodosAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.QueryAsync<Representante>(
                "ListarRepresentantes",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<(IEnumerable<Representante> Representantes, int Total)> ObtenerRepresentantesPaginadosAsync(int paginaActual, int pageSize)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("p_PaginaActual", paginaActual);
            parameters.Add("p_pageSize", pageSize);

            using var multi = await connection.QueryMultipleAsync("Listar10Representantes", parameters, commandType: CommandType.StoredProcedure);

            var representantes = await multi.ReadAsync<Representante>();
            var total = await multi.ReadFirstAsync<int>();

            return (representantes, total);
        }

        public async Task<int> ObtenerSiguienteNumeroAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var siguienteNumero = await connection.QueryFirstOrDefaultAsync<int>(
                "ObtenerSiguienteNumeroRepresentante",
                commandType: CommandType.StoredProcedure);

            return siguienteNumero == 0 ? 1 : siguienteNumero;
        }

        public async Task<int> InsertAsync(Representante representante)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("p_id_representante", representante.id_representante);
            parameters.Add("p_nombre", representante.nombre);
            parameters.Add("p_email", representante.email);

            return await connection.ExecuteAsync(
                "CrearRepresentante",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(Representante representante)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("p_id_representante", representante.id_representante);
            parameters.Add("p_nombre", representante.nombre);
            parameters.Add("p_email", representante.email);

            return await connection.ExecuteAsync(
                "ActualizarRepresentante",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        public async Task<bool> DeleteAsync(Representante representante)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_id_representante", representante.id_representante?.Trim(), DbType.String, ParameterDirection.Input);
            parameters.Add("p_result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var connection = _dbConnectionFactory.CreateConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            await connection.ExecuteAsync(
                "EliminarRepresentante",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int resultado = parameters.Get<int>("p_result");
            return resultado == 1;
        }
    }
}
