using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Solicitudes;
using SistemaSolicitudesLaDat.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Pages.Solicitudes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ISolicitudService _solicitudService;
        private readonly IBitacoraService _bitacoraService;
        private readonly IEstadoSolicitudService _estadoSolicitudService;

        public IndexModel(
            ISolicitudService solicitudService,
            IBitacoraService bitacoraService,
            IEstadoSolicitudService estadoSolicitudService)
        {
            _solicitudService = solicitudService;
            _bitacoraService = bitacoraService;
            _estadoSolicitudService = estadoSolicitudService;
        }

        public List<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }

        private const int TamanoPagina = 5;

        public async Task<IActionResult> OnGetAsync(int pagina = 1)
        {
            var idUsuarioEjecutor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var idEstadoVencida = await _estadoSolicitudService.ObtenerIdPorNombreAsync("vencida");

            Console.WriteLine($"ID estado vencida obtenido: {idEstadoVencida}");

            if (!string.IsNullOrEmpty(idEstadoVencida))
            {
                var totalVencidas = await _solicitudService.MarcarSolicitudesVencidasAsync(idEstadoVencida);

                Console.WriteLine($"Filas actualizadas a vencidas: {totalVencidas}");

                if (!string.IsNullOrEmpty(idUsuarioEjecutor) && totalVencidas > 0)
                {
                    await _bitacoraService.RegistrarAccionAsync(
                        idUsuarioEjecutor,
                        "Marcado automático de solicitudes vencidas",
                        new { TotalVencidas = totalVencidas }
                    );
                }
            }
            else
            {
                Console.WriteLine("No se obtuvo ID para estado 'vencida'");
            }

            var resultado = await _solicitudService.ObtenerSolicitudesPaginadasAsync(pagina, TamanoPagina);

            TotalPaginas = (int)Math.Ceiling(resultado.Total / (double)TamanoPagina);
            if (TotalPaginas < 1) TotalPaginas = 1;

            PaginaActual = pagina;
            Solicitudes = resultado.Solicitudes.ToList();

            // Registrar acción de consulta
            if (!string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                await _bitacoraService.RegistrarAccionAsync(
                    idUsuarioEjecutor,
                    "Consulta paginada de solicitudes",
                    new { Pagina = pagina, SolicitudesMostradas = Solicitudes.Count }
                );
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(string id_solicitud)
        {
            var idUsuarioEjecutor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "Error al identificar al usuario.";
                return RedirectToPage();
            }

            var solicitud = new Solicitud { id_solicitud = id_solicitud };

            var eliminado = await _solicitudService.EliminarAsync(solicitud, idUsuarioEjecutor);

            TempData["Mensaje"] = eliminado
                ? "Solicitud eliminada correctamente."
                : "No se pudo eliminar la solicitud. Puede estar relacionada con otras tablas.";

            return RedirectToPage();
        }

        public string ObtenerColorFila(Solicitud solicitud)
        {
            if (!solicitud.fecha_ingreso.HasValue)
                return string.Empty;

            DateTime hoy = DateTime.Today;
            int diasDesdeIngreso = (hoy - solicitud.fecha_ingreso.Value.Date).Days;

            if (solicitud.EstadoNombre?.Trim().ToLower() == "vencida")
                return "table-danger";

            if (diasDesdeIngreso >= 5)
                return "table-danger";

            if (diasDesdeIngreso == 0)
                return "table-warning";

            return "table-success";
        }

    }
}
