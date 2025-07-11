using Dapper;
using SistemaSolicitudesLaDat.Entities.Desgloses;
using SistemaSolicitudesLaDat.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Repository.Desgloses
{
    public class ImpuestoRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public ImpuestoRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<ImpuestoValorAgregado>> ObtenerTodosAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            return await connection.QueryAsync<ImpuestoValorAgregado>(
                "ListarIVAS",  
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
