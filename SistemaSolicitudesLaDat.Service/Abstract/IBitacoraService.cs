using SistemaSolicitudesLaDat.Entities.Usuarios;

namespace SistemaSolicitudesLaDat.Service.Abstract
{
    public interface IBitacoraService
    {
        Task<bool> RegistrarAccionAsync(string idUsuario, string descripcion, object accionesJson, string? idSolicitud = null);
        Task<bool> RegistrarErrorAsync(string idUsuario, string errorDetalle, string? idSolicitud = null);

    }
}
