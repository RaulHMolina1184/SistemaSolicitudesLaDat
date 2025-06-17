using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SistemaSolicitudesLaDat.Entities.Solicitudes;
using System.Data;

namespace SistemaSolicitudesLaDat.Service.Solicitudes
{
    public class SolicitudService
    {
        private readonly string _connectionString;

        public SolicitudService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection Conexion() => new MySqlConnection(_connectionString);

        public async Task<List<SolicitudResumen>> ObtenerResumenes()
        {
            using var db = Conexion();
            var sql = "SELECT id_solicitud AS IdSolicitud, titulo_solicitud AS Titulo FROM solicitud ORDER BY titulo_solicitud ASC";
            var resultado = await db.QueryAsync<SolicitudResumen>(sql);
            return resultado.ToList();
        }
    }
}
