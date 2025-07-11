using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaSolicitudesLaDat.Entities.Solicitudes;

namespace SistemaSolicitudesLaDat.Service.Abstract
{
    public interface ISolicitudService
    {
        Task<Solicitud?> ObtenerPorIdAsync(string id);
        Task CrearAsync(Solicitud solicitud, string usuarioEjecutor);
        Task ActualizarAsync(Solicitud solicitud, string usuarioEjecutor);
        Task<bool> EliminarAsync(Solicitud solicitud, string usuarioEjecutor);
        Task<(IEnumerable<Solicitud> Solicitudes, int Total)> ObtenerSolicitudesPaginadasAsync(int PaginaActual, int pageSize);
        Task<int> MarcarSolicitudesVencidasAsync(string idEstadoVencida);

    }
}
