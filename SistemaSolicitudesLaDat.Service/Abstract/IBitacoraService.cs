using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Service.Abstract
{
    public interface IBitacoraService
    {
        Task<bool> RegistrarAccionAsync(string idUsuario, string descripcion, object accionesJson, string? idSolicitud = null);
        Task<bool> RegistrarErrorAsync(string idUsuario, string errorDetalle, string? idSolicitud = null);
    }
}
