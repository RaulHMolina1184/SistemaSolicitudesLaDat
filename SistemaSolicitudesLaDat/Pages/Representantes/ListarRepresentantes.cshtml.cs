using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Representantes;
using SistemaSolicitudesLaDat.Service.Abstract;
using System.Security.Claims;

namespace SistemaSolicitudesLaDat.Pages.Representantes
{
    [Authorize]
    public class ListarRepresentanteModel : PageModel
    {
        private readonly IRepresentanteService _representanteService;
        private readonly IBitacoraService _bitacoraService;

        public List<Representante> Representantes { get; set; } = new();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        private const int TamanoPagina = 10;

        public ListarRepresentanteModel(IRepresentanteService representanteService, IBitacoraService bitacoraService)
        {
            _representanteService = representanteService;
            _bitacoraService = bitacoraService;
        }

        public async Task<IActionResult> OnGetAsync(int pagina = 1)
        {
            var resultado = await _representanteService.ObtenerRepresentantesPaginadosAsync(pagina, TamanoPagina);

            TotalPaginas = (int)Math.Ceiling(resultado.Total / (double)TamanoPagina);
            if (TotalPaginas < 1) TotalPaginas = 1;

            PaginaActual = pagina;
            Representantes = resultado.Representantes.ToList();

            var idUsuarioEjecutor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                await _bitacoraService.RegistrarAccionAsync(
                    idUsuarioEjecutor,
                    "Consulta paginada de representantes",
                    new { Pagina = pagina, RepresentantesMostrados = Representantes.Count }
                );
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(string id_representante)
        {
            var idUsuarioEjecutor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "Error al identificar al usuario.";
                return RedirectToPage();
            }

            var representante = new Representante { id_representante = id_representante };

            var eliminado = await _representanteService.EliminarAsync(representante, idUsuarioEjecutor);

            // Mensaje de éxito o error
            TempData["Mensaje"] = eliminado
                ? "Representante eliminado correctamente."
                : "No se pudo eliminar el representante. Puede estar relacionado con otras tablas.";

            return RedirectToPage();
        }
    }
}
