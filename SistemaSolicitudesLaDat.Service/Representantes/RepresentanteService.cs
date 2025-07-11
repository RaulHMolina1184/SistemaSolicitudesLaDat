using SistemaSolicitudesLaDat.Entities.Representantes;
using SistemaSolicitudesLaDat.Repository.Representantes;
using SistemaSolicitudesLaDat.Service.Abstract;

namespace SistemaSolicitudesLaDat.Service.Representantes
{
    public class RepresentanteService : IRepresentanteService
    {
        private readonly RepresentantesRepository _representantesRepository;
        private readonly IBitacoraService _bitacoraService;

        public RepresentanteService(RepresentantesRepository representantesRepository, IBitacoraService bitacoraService)
        {
            _representantesRepository = representantesRepository;
            _bitacoraService = bitacoraService; 
        }

        public async Task<(IEnumerable<Representante> Representantes, int Total)> ObtenerRepresentantesPaginadosAsync(int paginaActual, int pageSize)
        {
            return await _representantesRepository.ObtenerRepresentantesPaginadosAsync(paginaActual, pageSize);
        }

        public async Task<Representante?> ObtenerPorIdAsync(string id)
        {
            return await _representantesRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Representante>> ObtenerTodosAsync()
        {
            var representantes = await _representantesRepository.ObtenerTodosAsync();

            return representantes;  
        }

        public async Task CrearAsync(Representante representante, string usuarioEjecutor)
        {
            try
            {
                int siguienteNumero = await _representantesRepository.ObtenerSiguienteNumeroAsync();
                representante.id_representante = $"REP{siguienteNumero:D3}";

                await _representantesRepository.InsertAsync(representante);

                var registroExitoso = await _bitacoraService.RegistrarAccionAsync(
                    usuarioEjecutor,
                    "Creación de representante",
                    new
                    {
                        representante.id_representante,
                        representante.nombre,
                        representante.email,
                    },
                    representante.id_representante
                );

                if (!registroExitoso)
                    Console.WriteLine("No se pudo registrar la acción en bitácora.");
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync(usuarioEjecutor, ex.ToString());
                throw;
            }
        }

        public async Task ActualizarAsync(Representante representante, string usuarioEjecutor)
        {
            await _representantesRepository.UpdateAsync(representante);

            var exito = await _bitacoraService.RegistrarAccionAsync(
                usuarioEjecutor,
                "Actualización de representante",
                new
                {
                    representante.id_representante,
                    representante.nombre,
                    representante.email,
                });

            if (!exito)
                Console.WriteLine("⚠️ No se pudo registrar bitácora al actualizar representante.");
        }

        public async Task<bool> EliminarAsync(Representante representante, string usuarioEjecutor)
        {
            var eliminado = await _representantesRepository.DeleteAsync(representante);

            if (eliminado)
            {
                var exito = await _bitacoraService.RegistrarAccionAsync(
                    usuarioEjecutor,
                    "Eliminación de representante",
                    new
                    {
                        representante.id_representante,
                        representante.nombre
                    });

                if (!exito)
                    Console.WriteLine(" No se pudo registrar bitácora al eliminar representante.");
            }

            return eliminado;
        }
    }
}
