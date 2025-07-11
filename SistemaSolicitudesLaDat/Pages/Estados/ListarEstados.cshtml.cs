using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Solicitudes;
using SistemaSolicitudesLaDat.Service.Abstract;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SistemaSolicitudesLaDat.Pages.Estados
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IEstadoSolicitudService _estadoService;
        private readonly IBitacoraService _bitacoraService;

        public List<EstadoSolicitud> Estados { get; set; } = new();

        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        private const int TamanoPagina = 10;

        public IndexModel(IEstadoSolicitudService estadoService, IBitacoraService bitacoraService)
        {
            _estadoService = estadoService;
            _bitacoraService = bitacoraService;
        }

        public async Task<IActionResult> OnGetAsync(int pagina = 1)
        {
            // Suponiendo que tu servicio tiene método paginado, si no, habrá que implementarlo
            var resultado = await _estadoService.ObtenerEstadosPaginadosAsync(pagina, TamanoPagina);

            TotalPaginas = (int)Math.Ceiling(resultado.Total / (double)TamanoPagina);
            if (TotalPaginas < 1) TotalPaginas = 1;

            PaginaActual = pagina;
            Estados = resultado.Estados.ToList();

            var idUsuarioEjecutor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                await _bitacoraService.RegistrarAccionAsync(
                    idUsuarioEjecutor,
                    "Consulta paginada de estados de solicitud",
                    new { Pagina = pagina, EstadosMostrados = Estados.Count }
                );
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(string id_estado)
        {
            var idUsuarioEjecutor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "Error al identificar al usuario.";
                return RedirectToPage();
            }

            // Crea un objeto EstadoSolicitud con el id recibido
            var estado = new EstadoSolicitud { id_estado = id_estado };

            var eliminado = await _estadoService.EliminarAsync(estado, idUsuarioEjecutor);

            TempData["Mensaje"] = eliminado
                ? "Estado eliminado correctamente."
                : "No se puede eliminar un registro con datos relacionados.";

            if (eliminado)
            {
                await _bitacoraService.RegistrarAccionAsync(
                    idUsuarioEjecutor,
                    $"Eliminó estado de solicitud con ID {id_estado}",
                    null
                );
            }

            return RedirectToPage();
        }
    }
}
