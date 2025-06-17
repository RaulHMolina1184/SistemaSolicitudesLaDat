using SistemaSolicitudesLaDat.Entities.Representantes;
using SistemaSolicitudesLaDat.Repository.Representantes;
using SistemaSolicitudesLaDat.Service.Abstract;


namespace SistemaSolicitudesLaDat.Service.Representantes
{
    public class RepresentanteService : IRepresentanteService
    {
        private readonly IRepresentanteRepository _repo;

        public RepresentanteService(IRepresentanteRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Representante>> ObtenerTodosAsync() => _repo.ObtenerTodosAsync();

        public Task<Representante?> ObtenerPorIdAsync(string id) => _repo.ObtenerPorIdAsync(id);

        //public Task<bool> CrearAsync(Representante r) => _repo.CrearAsync(r);

        public async Task<bool> CrearAsync(Representante representante)
        {
            // Verificar si ya existe por correo
            var existentes = await _repo.ObtenerTodosAsync();
            if (existentes.Any(r => r.Email.Trim().ToLower() == representante.Email.Trim().ToLower()))
            {
                return false; // Ya existe
            }

            return await _repo.CrearAsync(representante);
        }



        public Task<bool> ActualizarAsync(Representante r) => _repo.ActualizarAsync(r);

        public async Task<(bool Eliminado, string Mensaje)> EliminarAsync(string id)
        {
            if (await _repo.TieneSolicitudesAsync(id))
                return (false, "No se puede eliminar un registro con datos relacionados.");

            var eliminado = await _repo.EliminarAsync(id);
            return (eliminado, eliminado ? "Registro eliminado correctamente." : "No se pudo eliminar.");
        }

        public async Task<(bool Editado, string Mensaje)> EditarAsync(Representante modelo)
        {
            var actualizado = await _repo.ActualizarAsync(modelo);
            return (actualizado, actualizado ? "Representante actualizado correctamente." : "No se pudo actualizar.");
        }

        public async Task<(IEnumerable<Representante> Lista, int Total)> ObtenerPaginadoAsync(int pagina, int tamano)
        {
            var lista = await _repo.ObtenerPaginadoAsync(pagina, tamano);
            var total = await _repo.ContarTotalAsync();
            return (lista, total);
        }

    }
}
