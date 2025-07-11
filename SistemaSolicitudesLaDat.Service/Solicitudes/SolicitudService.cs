using SistemaSolicitudesLaDat.Entities.Representantes;
using SistemaSolicitudesLaDat.Entities.Solicitudes;
using SistemaSolicitudesLaDat.Repository.Representantes;
using SistemaSolicitudesLaDat.Repository.Solicitudes;
using SistemaSolicitudesLaDat.Service.Abstract;

namespace SistemaSolicitudesLaDat.Service.Solicitudes
{
    public class SolicitudService : ISolicitudService
    {
        private readonly SolicitudRepository _solicitudRepository;
        private readonly IBitacoraService _bitacoraService;
        private readonly IEstadoSolicitudService _estadoSolicitudService;

        public SolicitudService(
            SolicitudRepository solicitudRepository,
            IBitacoraService bitacoraService,
            IEstadoSolicitudService estadoSolicitudService)
        {
            _solicitudRepository = solicitudRepository;
            _bitacoraService = bitacoraService;
            _estadoSolicitudService = estadoSolicitudService;
        }

        public async Task<(IEnumerable<Solicitud> Solicitudes, int Total)> ObtenerSolicitudesPaginadasAsync(int PaginaActual, int pageSize)
        {
            return await _solicitudRepository.ObtenerSolicitudesPaginadasAsync(PaginaActual, pageSize);
        }

        public async Task<Solicitud?> ObtenerPorIdAsync(string id)
        {
            return await _solicitudRepository.GetByIdAsync(id);
        }
        public async Task<int> MarcarSolicitudesVencidasAsync(string idEstadoVencida)
        {
            return await _solicitudRepository.MarcarSolicitudesVencidasAsync(idEstadoVencida);
        }

        public async Task CrearAsync(Solicitud solicitud, string usuarioEjecutor)
        {
            try
            {
                int siguienteNumero = await _solicitudRepository.ObtenerSiguienteNumeroAsync();
                solicitud.id_solicitud = $"SOL{siguienteNumero:D3}";

                var resultado = await _solicitudRepository.InsertAsync(solicitud);

                if (resultado == 1)
                {
                    await _bitacoraService.RegistrarAccionAsync(
                        usuarioEjecutor,
                        "Creación de solicitud",
                        new
                        {
                            solicitud.id_solicitud,
                            solicitud.consecutivo_oficio,
                            solicitud.documento_respuesta,
                            solicitud.documento_inicio,
                            solicitud.titulo_solicitud,
                            solicitud.descripcion,
                            solicitud.id_representante,
                            solicitud.observaciones,
                            solicitud.estado_solicitud,
                            solicitud.fecha_ingreso,
                            solicitud.fecha_inicio,
                            solicitud.fecha_respuesta
                        },
                        solicitud.id_solicitud
                    );
                }
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync(usuarioEjecutor, ex.ToString());
                throw;
            }
        }


        public async Task ActualizarAsync(Solicitud solicitud, string usuarioEjecutor)
        {
            try
            {
                var actualizado = await _solicitudRepository.UpdateAsync(solicitud);

                if (actualizado == 1)
                {
                    var registro = await _bitacoraService.RegistrarAccionAsync(
                        usuarioEjecutor,
                        "Actualización de solicitud",
                        new
                        {
                            solicitud.id_solicitud,
                            solicitud.consecutivo_oficio,
                            solicitud.documento_respuesta,
                            solicitud.documento_inicio,
                            solicitud.titulo_solicitud,
                            solicitud.descripcion,
                            solicitud.id_representante,
                            solicitud.observaciones,
                            solicitud.estado_solicitud,
                            solicitud.fecha_ingreso,
                            solicitud.fecha_inicio,
                            solicitud.fecha_respuesta
                        },
                        solicitud.id_solicitud
                    );

                    if (!registro)
                    {
                        Console.WriteLine(" No se pudo registrar la bitácora de actualización.");
                    }
                }
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync(usuarioEjecutor, ex.ToString());
                throw;
            }
        }

        public async Task<bool> EliminarAsync(Solicitud solicitud, string usuarioEjecutor)
        {
            try
            {
                var eliminado = await _solicitudRepository.DeleteAsync(solicitud);

                if (eliminado)
                {
                    var registrado = await _bitacoraService.RegistrarAccionAsync(
                        usuarioEjecutor,
                        "Eliminación de solicitud",
                        new
                        {
                            solicitud.id_solicitud,
                            solicitud.titulo_solicitud,
                            solicitud.descripcion,
                            solicitud.estado_solicitud
                        },
                        solicitud.id_solicitud
                    );

                    if (!registrado)
                    {
                        Console.WriteLine($"Bitácora no registrada para la eliminación de la solicitud {solicitud.id_solicitud}");
                    }
                }
                else
                {
                    Console.WriteLine($" La solicitud {solicitud.id_solicitud} no fue eliminada (posiblemente no existe o tiene relaciones).");
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
