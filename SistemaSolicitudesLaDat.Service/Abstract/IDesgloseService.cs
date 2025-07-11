using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaSolicitudesLaDat.Entities.Desgloses;

namespace SistemaSolicitudesLaDat.Service.Abstract
{
    public interface IDesgloseService
    {
        Task<int> InsertAsync(Desglose desglose, string usuarioEjecutor);
        Task<int> UpdateAsync(Desglose desglose, string usuarioEjecutor);
        Task<Desglose?> GetByIdAsync(string id);
        Task<IEnumerable<Desglose>> GetAllAsync();
        Task<bool> EliminarAsync(string idDesglose, string usuarioEjecutor);
        Task<(IEnumerable<Desglose> Desgloses, int Total)> ObtenerDesglosesPaginadosAsync(string idSolicitud, int paginaActual, int pageSize);

    }
}
