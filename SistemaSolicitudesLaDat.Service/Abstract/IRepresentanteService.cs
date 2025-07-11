using SistemaSolicitudesLaDat.Entities.Representantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Service.Abstract
{
    public interface IRepresentanteService
    {
        Task<(IEnumerable<Representante> Representantes, int Total)> ObtenerRepresentantesPaginadosAsync(int paginaActual, int pageSize);
        Task<IEnumerable<Representante>> ObtenerTodosAsync();
        Task<Representante?> ObtenerPorIdAsync(string id);
        Task CrearAsync(Representante representante, string usuarioEjecutor);
        Task ActualizarAsync(Representante representante, string usuarioEjecutor);
        Task<bool> EliminarAsync(Representante representante, string usuarioEjecutor);
    }
}
