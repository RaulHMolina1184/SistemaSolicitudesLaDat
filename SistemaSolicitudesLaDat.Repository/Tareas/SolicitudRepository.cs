using Dapper;
using SistemaSolicitudesLaDat.Entities.Solicitudes;
using SistemaSolicitudesLaDat.Repository.Infrastructure;
using System.Data;

namespace SistemaSolicitudesLaDat.Repository.Solicitudes
{
    public class SolicitudRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public SolicitudRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Solicitud>> ObtenerTodasAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.QueryAsync<Solicitud>(
                "mostrar_todas_solicitudes",
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
