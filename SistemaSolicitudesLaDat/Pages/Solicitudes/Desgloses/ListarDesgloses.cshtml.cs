using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Desgloses;
using SistemaSolicitudesLaDat.Service.Abstract;
using System.Security.Claims;

namespace SistemaSolicitudesLaDat.Pages.Solicitudes.Desgloses
{
    [Authorize]
    public class ListarDesglosesModel : PageModel
    {
        private readonly IDesgloseService _desgloseService;
        private readonly IBitacoraService _bitacoraService;

        public ListarDesglosesModel(IDesgloseService desgloseService, IBitacoraService bitacoraService)
        {
            _desgloseService = desgloseService;
            _bitacoraService = bitacoraService;
        }

        public string IdSolicitud { get; set; } = string.Empty;
        public List<Desglose> Desgloses { get; set; } = new();
        public int PaginaActual { get; set; } = 1;
        public int TotalPaginas { get; set; }
        public int TamanoPagina { get; } = 10;

        public async Task<IActionResult> OnGetAsync(string id, int pagina = 1)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Mensaje"] = "ID de solicitud inválido.";
                return RedirectToPage("/Solicitudes/Index");
            }

            IdSolicitud = id;
            PaginaActual = pagina;

            var resultado = await _desgloseService.ObtenerDesglosesPaginadosAsync(IdSolicitud, PaginaActual, TamanoPagina);

            TotalPaginas = (int)Math.Ceiling(resultado.Total / (double)TamanoPagina);
            if (TotalPaginas < 1) TotalPaginas = 1;

            Desgloses = resultado.Desgloses.ToList();

            var idUsuarioEjecutor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                await _bitacoraService.RegistrarAccionAsync(
                    idUsuarioEjecutor,
                    $"Consulta paginada de desgloses de la solicitud {IdSolicitud}",
                    new { Pagina = PaginaActual, DesglosesMostrados = Desgloses.Count }
                );
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(string id_desglose, string id_solicitud)
        {
            var idUsuarioEjecutor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "Error al identificar al usuario.";
                return RedirectToPage(new { id = id_solicitud });
            }

            var eliminado = await _desgloseService.EliminarAsync(id_desglose, idUsuarioEjecutor);

            TempData["Mensaje"] = eliminado
                ? "Desglose eliminado correctamente."
                : "No se pudo eliminar el desglose. Puede estar relacionado con otras tablas.";

            return RedirectToPage(new { id = id_solicitud });
        }
    }
}
