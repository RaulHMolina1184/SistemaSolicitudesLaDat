using SistemaSolicitudesLaDat.Repository.Bitacora;
using SistemaSolicitudesLaDat.Service.Abstract;

namespace SistemaSolicitudesLaDat.Service.Bitacora
{
    public class BitacoraService : IBitacoraService
    {
        private readonly BitacoraRepository _bitacoraRepository;

        public BitacoraService(BitacoraRepository bitacoraRepository)
        {
            _bitacoraRepository = bitacoraRepository;
        }

        public async Task<bool> RegistrarAccionAsync(string idUsuario, string descripcion, object accionesJson, string? idSolicitud = null)
        {
            return await _bitacoraRepository.RegistrarAccionAsync(idUsuario, descripcion, accionesJson, idSolicitud);
        }

        public async Task<bool> RegistrarErrorAsync(string idUsuario, string errorDetalle, string? idSolicitud = null)
        {
            return await _bitacoraRepository.RegistrarErrorAsync(idUsuario, errorDetalle, idSolicitud);
        }

        // 👇 Métodos agregados para facilitar el uso desde los servicios de negocio

        public async Task<bool> RegistrarCreacion(string entidad, string idUsuario, object nuevo, string? idSolicitud = null)
        {
            string descripcion = $"Creación de {entidad}";
            return await RegistrarAccionAsync(idUsuario, descripcion, nuevo, idSolicitud);
        }

        public async Task<bool> RegistrarActualizacion(string entidad, string idUsuario, object anterior, object nuevo, string? idSolicitud = null)
        {
            string descripcion = $"Actualización de {entidad}";
            var cambio = new { anterior, nuevo };
            return await RegistrarAccionAsync(idUsuario, descripcion, cambio, idSolicitud);
        }

        public async Task<bool> RegistrarEliminacion(string entidad, string idUsuario, object eliminado, string? idSolicitud = null)
        {
            string descripcion = $"Eliminación de {entidad}";
            return await RegistrarAccionAsync(idUsuario, descripcion, eliminado, idSolicitud);
        }
    }
}
