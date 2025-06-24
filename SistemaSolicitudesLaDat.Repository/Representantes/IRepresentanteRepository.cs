using SistemaSolicitudesLaDat.Entities.Representantes;

public interface IRepresentanteRepository
{
    Task<IEnumerable<Representante>> ObtenerTodosAsync();
    Task<Representante?> ObtenerPorIdAsync(string id);
    Task<bool> CrearAsync(Representante representante);
    Task<bool> ActualizarAsync(Representante representante);
    Task<bool> EliminarAsync(string id);
    Task<bool> TieneSolicitudesAsync(string id);

    Task<IEnumerable<Representante>> ObtenerPaginadoAsync(int pagina, int tamanoPagina);
    Task<int> ContarTotalAsync();

}
