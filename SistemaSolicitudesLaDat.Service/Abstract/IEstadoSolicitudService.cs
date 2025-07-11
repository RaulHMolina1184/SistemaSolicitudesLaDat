using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaSolicitudesLaDat.Entities.Solicitudes;

namespace SistemaSolicitudesLaDat.Service.Abstract
{
    public interface IEstadoSolicitudService
    {
        Task<IEnumerable<EstadoSolicitud>> ObtenerTodosAsync();
        Task<EstadoSolicitud?> ObtenerPorIdAsync(string id);
        Task<EstadoSolicitud?> ObtenerPorNombreAsync(string nombreEstado);
        Task<string?> ObtenerIdPorNombreAsync(string nombreEstado);
        Task<(IEnumerable<EstadoSolicitud> Estados, int Total)> ObtenerEstadosPaginadosAsync(int pagina, int tamanoPagina);
        Task CrearAsync(EstadoSolicitud estado, string usuarioEjecutor);
        Task<bool> ActualizarAsync(EstadoSolicitud estado, string usuarioEjecutor);
        Task<bool> EliminarAsync(EstadoSolicitud estado, string usuarioEjecutor);
    }
}
