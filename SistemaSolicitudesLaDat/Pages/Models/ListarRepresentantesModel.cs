namespace SistemaSolicitudesLaDat.Pages.Models;

public class RepresentanteListadoModel
{
    public string IdRepresentante { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;

    public int PaginaActual { get; set; }
    public int TotalPaginas { get; set; }

}
