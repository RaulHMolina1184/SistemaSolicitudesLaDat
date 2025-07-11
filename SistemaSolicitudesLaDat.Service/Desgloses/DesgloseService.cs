using SistemaSolicitudesLaDat.Entities.Desgloses;
using SistemaSolicitudesLaDat.Entities.Representantes;
using SistemaSolicitudesLaDat.Repository.Desgloses;
using SistemaSolicitudesLaDat.Repository.Representantes;
using SistemaSolicitudesLaDat.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Service.Desgloses
{
    public class DesgloseService : IDesgloseService
    {
        private readonly DesgloseRepository _desgloseRepository;
        private readonly IBitacoraService _bitacoraService;

        public DesgloseService(DesgloseRepository desgloseRepository, IBitacoraService bitacoraService)
        {
            _desgloseRepository = desgloseRepository;
            _bitacoraService = bitacoraService;
        }

        public async Task<IEnumerable<Desglose>> GetAllAsync()
        {
            return await _desgloseRepository.GetAllAsync();
        }

        public async Task<(IEnumerable<Desglose> Desgloses, int Total)> ObtenerDesglosesPaginadosAsync(string idSolicitud, int paginaActual, int pageSize)
        {
            return await _desgloseRepository.ObtenerDesglosesPaginadosAsync(idSolicitud, paginaActual, pageSize);
        }

        public async Task<Desglose?> GetByIdAsync(string id)
        {
            return await _desgloseRepository.GetByIdAsync(id);
        }
        public async Task<int> ObtenerSiguienteNumeroAsync()
        {
            return await _desgloseRepository.ObtenerSiguienteNumeroAsync();
        }

        public async Task<int> InsertAsync(Desglose desglose, string usuarioEjecutor)
        {
            try
            {
                int siguienteNumero = await _desgloseRepository.ObtenerSiguienteNumeroAsync();
                desglose.id_desglose = $"DESG{siguienteNumero:D3}";

                var resultado = await _desgloseRepository.InsertAsync(desglose);

                if (resultado == 1)
                {
                    var registrado = await _bitacoraService.RegistrarAccionAsync(
                        usuarioEjecutor,
                        "Creación de desglose",
                        new
                        {
                            desglose.id_desglose,
                            desglose.id_solicitud,
                            desglose.anio,
                            desglose.horas
                        },
                        desglose.id_solicitud
                    );

                    if (!registrado)
                    {
                        Console.WriteLine($"Bitácora no registrada para creación de desglose {desglose.id_desglose}.");
                    }
                }

                return resultado;
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync(usuarioEjecutor, ex.ToString());
                throw;
            }
        }

        public async Task<int> UpdateAsync(Desglose desglose, string usuarioEjecutor)
        {
            try
            {
                var resultado = await _desgloseRepository.UpdateAsync(desglose);

                if (resultado == 1)
                {
                    var registrado = await _bitacoraService.RegistrarAccionAsync(
                        usuarioEjecutor,
                        "Actualización de desglose",
                        new
                        {
                            desglose.id_desglose,
                            desglose.id_solicitud,
                            desglose.anio,
                            desglose.horas
                        },
                        desglose.id_solicitud
                    );

                    if (!registrado)
                    {
                        Console.WriteLine($" Bitácora no registrada para actualización de desglose {desglose.id_desglose}.");
                    }
                }

                return resultado;
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync(usuarioEjecutor, ex.ToString());
                throw;
            }
        }

        public async Task<bool> EliminarAsync(string idDesglose, string usuarioEjecutor)
        {
            try
            {
                var eliminado = await _desgloseRepository.EliminarAsync(idDesglose);

                if (eliminado)
                {
                    var registrado = await _bitacoraService.RegistrarAccionAsync(
                        usuarioEjecutor,
                        "Eliminación de desglose",
                        new { id_desglose = idDesglose }
                    );

                    if (!registrado)
                    {
                        Console.WriteLine($"Bitácora no registrada para eliminación de desglose {idDesglose}.");
                    }
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
