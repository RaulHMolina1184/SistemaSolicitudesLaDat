using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SistemaSolicitudesLaDat.Entities.Tareas;
using System.Data;

namespace SistemaSolicitudesLaDat.Repository.Reportes
{
    public class ReporteRepository
    {
        private readonly string _connectionString;

        public ReporteRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection Conexion() => new MySqlConnection(_connectionString);

        public async Task<TareaReporteResultado> ObtenerTareasPorUsuarioYMes(string idUsuario, int mes, int anio)
        {
            using var db = Conexion();

            using var multi = await db.QueryMultipleAsync("reporte_tareas_por_usuario",
                new { pI_id_usuario = idUsuario, pI_mes = mes, pI_anio = anio },
                commandType: CommandType.StoredProcedure);

            var tareas = (await multi.ReadAsync<TareaReporte>()).ToList();
            var total = await multi.ReadFirstOrDefaultAsync<decimal>();

            return new TareaReporteResultado
            {
                Tareas = tareas,
                TotalHoras = total
            };
        }

        public async Task<TareaReporteResultado> ObtenerTareasPorSolicitudYMes(string idSolicitud, int mes, int anio)
        {
            using var db = Conexion();

            var resultado = new TareaReporteResultado();

            using var multi = await db.QueryMultipleAsync(
                "reporte_tareas_por_solicitud",
                new { pI_id_solicitud = idSolicitud, pI_mes = mes, pI_anio = anio },
                commandType: CommandType.StoredProcedure
            );

            resultado.Tareas = (await multi.ReadAsync<TareaReporte>()).ToList();
            resultado.TotalHoras = await multi.ReadFirstOrDefaultAsync<decimal>();

            return resultado;
        }

    }
}

