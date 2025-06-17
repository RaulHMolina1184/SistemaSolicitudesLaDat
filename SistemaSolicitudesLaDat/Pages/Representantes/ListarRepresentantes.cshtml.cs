using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using SistemaSolicitudesLaDat.Pages.Models;
using SistemaSolicitudesLaDat.Service.Abstract;
using SistemaSolicitudesLaDat.Repository.Representantes;

namespace SistemaSolicitudesLaDat.Pages.Representantes
{
    [Authorize]
    public class ListarRepresentantesModel : PageModel
    {
        private readonly IRepresentanteService _representanteService;
        private readonly IBitacoraService _bitacoraService;

        public List<RepresentanteListadoModel> Representantes { get; set; } = new();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }

        public ListarRepresentantesModel(
            IRepresentanteService representanteService,
            IBitacoraService bitacoraService)
        {
            _representanteService = representanteService;
            _bitacoraService = bitacoraService;
        }

        public async Task OnGetAsync(int pagina = 1)
        {
            const int TAMANO_PAGINA = 10;
            var (lista, total) = await _representanteService.ObtenerPaginadoAsync(pagina, TAMANO_PAGINA);

            Representantes = lista.Select(r => new RepresentanteListadoModel
            {
                IdRepresentante = r.IdRepresentante,
                Nombre = r.Nombre,
                Email = r.Email
            }).ToList();

            PaginaActual = pagina;
            TotalPaginas = (int)Math.Ceiling((double)total / TAMANO_PAGINA);

            var idUsuarioEjecutor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                await _bitacoraService.RegistrarAccionAsync(
                    idUsuarioEjecutor,
                    "Consulta de lista de representantes (paginada)",
                    new { TotalRepresentantes = total, Pagina = PaginaActual }
                );
            }
        }

        public async Task<IActionResult> OnPostEliminarAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Mensaje"] = "Error al eliminar el representante.";
                return RedirectToPage();
            }

            var idUsuarioEjecutor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "No se pudo obtener el ID del usuario autenticado.";
                return RedirectToPage();
            }

            var resultado = await _representanteService.EliminarAsync(id);
            TempData["Mensaje"] = resultado.Eliminado
                ? "Representante eliminado correctamente."
                : resultado.Mensaje;

            return RedirectToPage();
        }
    }
}
