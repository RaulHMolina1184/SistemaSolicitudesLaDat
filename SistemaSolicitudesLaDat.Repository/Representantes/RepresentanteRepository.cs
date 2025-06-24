using Dapper;
using SistemaSolicitudesLaDat.Entities.Representantes;
using SistemaSolicitudesLaDat.Repository.Infrastructure;
using System.Data;

namespace SistemaSolicitudesLaDat.Repository.Representantes
{
    public class RepresentanteRepository : IRepresentanteRepository
    {
        private readonly IDbConnectionFactory _db;

        public RepresentanteRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Representante>> ObtenerTodosAsync()
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<Representante>("SELECT id_representante AS IdRepresentante, nombre, email FROM representante");
        }

        public async Task<Representante?> ObtenerPorIdAsync(string id)
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Representante>(
                "SELECT * FROM representante WHERE id_representante = @id",
                new { id });
        }

        //public async Task<bool> CrearAsync(Representante r)
        //{
        //    using var conn = _db.CreateConnection();
        //    var query = @"INSERT INTO representante (id_representante, nombre, email)
        //                  VALUES (@IdRepresentante, @Nombre, @Email)";
        //    return await conn.ExecuteAsync(query, r) > 0;
        //}

        public async Task<bool> CrearAsync(Representante r)
        {
            using var conn = _db.CreateConnection();
            var query = @"INSERT INTO representante (id_representante, nombre, email)
                  VALUES (@IdRepresentante, @Nombre, @Email)";
            return await conn.ExecuteAsync(query, r) > 0;
        }


        public async Task<bool> ActualizarAsync(Representante r)
        {
            using var conn = _db.CreateConnection();
            var query = @"UPDATE representante SET nombre = @Nombre, email = @Email
                          WHERE id_representante = @IdRepresentante";
            return await conn.ExecuteAsync(query, r) > 0;
        }

        public async Task<bool> EliminarAsync(string id)
        {
            using var conn = _db.CreateConnection();
            return await conn.ExecuteAsync(
                "DELETE FROM representante WHERE id_representante = @id", new { id }) > 0;
        }

        public async Task<bool> TieneSolicitudesAsync(string id)
        {
            using var conn = _db.CreateConnection();
            var count = await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM solicitud WHERE id_representante = @id", new { id });
            return count > 0;
        }

        public async Task<IEnumerable<Representante>> ObtenerPaginadoAsync(int pagina, int tamanoPagina)
        {
            using var conn = _db.CreateConnection();
            var offset = (pagina - 1) * tamanoPagina;

            var query = @"SELECT id_representante AS IdRepresentante, nombre, email
                  FROM representante
                  ORDER BY nombre
                  LIMIT @Tamano OFFSET @Offset";

            return await conn.QueryAsync<Representante>(query, new { Tamano = tamanoPagina, Offset = offset });
        }

        public async Task<int> ContarTotalAsync()
        {
            using var conn = _db.CreateConnection();
            return await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM representante");
        }
    }
}
