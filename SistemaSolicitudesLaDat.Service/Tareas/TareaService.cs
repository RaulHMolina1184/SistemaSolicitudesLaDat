using SistemaSolicitudesLaDat.Entities.Tareas;
using SistemaSolicitudesLaDat.Repository.Tareas;
using SistemaSolicitudesLaDat.Service.Bitacora;

namespace SistemaSolicitudesLaDat.Service.Tareas
{
    public class TareaService
    {
        private readonly TareaRepository _repo;
        private readonly BitacoraService _bitacora;

        public TareaService(TareaRepository repo, BitacoraService bitacora)
        {
            _repo = repo;
            _bitacora = bitacora;
        }

        public async Task<IEnumerable<Tarea>> ListarPorSolicitud(string idSolicitud)
        {
            return await _repo.ObtenerPorSolicitud(idSolicitud);
        }

        public async Task<Tarea?> ObtenerPorId(string id)
        {
            return await _repo.ObtenerPorId(id);
        }

        public async Task<bool> Crear(Tarea t, string usuarioActual)
        {
            var creado = await _repo.Insertar(t);
            if (creado)
                await _bitacora.RegistrarCreacion("tarea", usuarioActual, t);
            return creado;
        }

        public async Task<bool> Editar(Tarea nuevo, Tarea anterior, string usuarioActual)
        {
            var actualizado = await _repo.Actualizar(nuevo);
            if (actualizado)
                await _bitacora.RegistrarActualizacion("tarea", usuarioActual, anterior, nuevo);
            return actualizado;
        }

        public async Task<bool> Eliminar(string id, Tarea t, string usuarioActual)
        {
            var eliminado = await _repo.Eliminar(id);
            if (eliminado)
                await _bitacora.RegistrarEliminacion("tarea", usuarioActual, t);
            return eliminado;
        }
    }
}
