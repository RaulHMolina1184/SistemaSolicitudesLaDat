using SistemaSolicitudesLaDat.Entities.Representantes;


namespace SistemaSolicitudesLaDat.Service.Abstract

{

    public interface IRepresentanteService
    {
        Task<IEnumerable<Representante>> ObtenerTodosAsync();
        Task<Representante?> ObtenerPorIdAsync(string id);
        Task<bool> CrearAsync(Representante r);
        Task<bool> ActualizarAsync(Representante r);
        Task<(bool Eliminado, string Mensaje)> EliminarAsync(string id);
        Task<(bool Editado, string Mensaje)> EditarAsync(Representante modelo);
        Task<(IEnumerable<Representante> Lista, int Total)> ObtenerPaginadoAsync(int pagina, int tamano);

    }


}
