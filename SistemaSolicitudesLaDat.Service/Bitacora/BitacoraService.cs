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
    }
}
