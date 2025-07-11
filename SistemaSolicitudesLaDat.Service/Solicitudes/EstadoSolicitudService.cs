using SistemaSolicitudesLaDat.Entities.Representantes;
using SistemaSolicitudesLaDat.Entities.Solicitudes;
using SistemaSolicitudesLaDat.Repository.Representantes;
using SistemaSolicitudesLaDat.Repository.Solicitudes;
using SistemaSolicitudesLaDat.Service.Abstract;
using SistemaSolicitudesLaDat.Service.Bitacora;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Service.Solicitudes
{
    public class EstadoSolicitudService : IEstadoSolicitudService
    {
        private readonly EstadoSolicitudRepository _estadoSolicitudRepository;
        private readonly IBitacoraService _bitacoraService;

        public EstadoSolicitudService(EstadoSolicitudRepository estadoSolicitudRepository, IBitacoraService bitacoraService)
        {
            _estadoSolicitudRepository = estadoSolicitudRepository;
            _bitacoraService = bitacoraService;
        }

        public async Task<IEnumerable<EstadoSolicitud>> ObtenerTodosAsync()
        {
            var estadossolicitud = await _estadoSolicitudRepository.ObtenerTodosAsync();

            return estadossolicitud;
        }
        public async Task<(IEnumerable<EstadoSolicitud> Estados, int Total)> ObtenerEstadosPaginadosAsync(int pagina, int tamanoPagina)
        {
            return await _estadoSolicitudRepository.GetPagedAsync(pagina, tamanoPagina);
        }

        public async Task<EstadoSolicitud?> ObtenerPorIdAsync(string id)
        {
            return await _estadoSolicitudRepository.GetByIdAsync(id);
        }
        public async Task<string?> ObtenerIdPorNombreAsync(string nombreEstado)
        {
            return await _estadoSolicitudRepository.ObtenerIdPorNombreAsync(nombreEstado);
        }

        public async Task<EstadoSolicitud?> ObtenerPorNombreAsync(string nombre)
        {
            return await _estadoSolicitudRepository.GetByNombreAsync(nombre);
        }


        public async Task CrearAsync(EstadoSolicitud estado, string usuarioEjecutor)
        {
            try
            {
                int resultado = await _estadoSolicitudRepository.CreateAsync(estado);

                if (resultado == 1)
                {
                    var registrado = await _bitacoraService.RegistrarAccionAsync(
                        usuarioEjecutor,
                        "Creación de estado de solicitud",
                        new
                        {
                            estado.id_estado,
                            estado.Estado
                        },
                        estado.id_estado
                    );

                    if (!registrado)
                    {
                        Console.WriteLine("No se pudo registrar la acción en bitácora.");
                    }
                }
                else
                {
                    Console.WriteLine("No se pudo crear el estado de solicitud.");
                }
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync(usuarioEjecutor, ex.ToString());
                throw;
            }
        }

        public async Task<bool> ActualizarAsync(EstadoSolicitud estado, string usuarioEjecutor)
        {
            try
            {
                int actualizado = await _estadoSolicitudRepository.UpdateAsync(estado);

                if (actualizado == 1)
                {
                    var registrado = await _bitacoraService.RegistrarAccionAsync(
                        usuarioEjecutor,
                        "Actualización de estado de solicitud",
                        new
                        {
                            estado.id_estado,
                            estado.Estado
                        },
                        estado.id_estado
                    );

                    if (!registrado)
                    {
                        Console.WriteLine("No se pudo registrar la bitácora de actualización.");
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync(usuarioEjecutor, ex.ToString());
                throw;
            }
        }

        public async Task<bool> EliminarAsync(EstadoSolicitud estado, string usuarioEjecutor)
        {
            try
            {
                bool eliminado = await _estadoSolicitudRepository.DeleteAsync(estado.id_estado);

                if (eliminado)
                {
                    var registrado = await _bitacoraService.RegistrarAccionAsync(
                        usuarioEjecutor,
                        "Eliminación de estado de solicitud",
                        new
                        {
                            estado.id_estado,
                            estado.Estado
                        },
                        estado.id_estado
                    );

                    if (!registrado)
                    {
                        Console.WriteLine($"Bitácora no registrada para eliminación del estado {estado.id_estado}");
                    }
                }
                else
                {
                    Console.WriteLine($"El estado {estado.id_estado} no fue eliminado (puede tener relaciones).");
                }

                return eliminado;
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync(usuarioEjecutor, ex.ToString());
                throw;
            }
        }
    }
}
