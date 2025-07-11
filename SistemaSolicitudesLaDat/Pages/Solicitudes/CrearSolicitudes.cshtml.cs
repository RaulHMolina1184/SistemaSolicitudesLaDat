using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Solicitudes;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaSolicitudesLaDat.Service.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace SistemaSolicitudesLaDat.Pages.Solicitudes
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ISolicitudService _solicitudService;
        private readonly IRepresentanteService _representanteService;
        private readonly IEstadoSolicitudService _estadoSolicitudService;

        public CreateModel(ISolicitudService solicitudService, IRepresentanteService representanteService, IEstadoSolicitudService estadoSolicitudService)
        {
            _solicitudService = solicitudService;
            _representanteService = representanteService;
            _estadoSolicitudService = estadoSolicitudService;
        }

        [BindProperty]
        public Solicitud Solicitud { get; set; } = new();

        public List<SelectListItem> Representantes { get; set; } = new();

        public List<SelectListItem> EstadosSolicitud { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // No necesitas cargar lista de estados
            var estadoNueva = await _estadoSolicitudService.ObtenerPorIdAsync("EST001");
            if (estadoNueva == null)
            {
                TempData["MensajeError"] = "No existe el estado inicial 'NUEVA' en la base de datos.";
                return RedirectToPage("/Solicitudes/ListarSolicitudes");
            }

            Solicitud.estado_solicitud = estadoNueva.id_estado;
            Solicitud.EstadoNombre = estadoNueva.Estado;

            await CargarListasAsync(); // Solo para representantes

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarListasAsync();
                return Page();
            }

            if (string.IsNullOrEmpty(Solicitud.id_representante))
            {
                ModelState.AddModelError("Solicitud.id_representante", "Debe seleccionar un representante.");
                await CargarListasAsync();
                return Page();
            }

            // Forzar que el estado sea siempre "EST001"
            Solicitud.estado_solicitud = "EST001";

            var idUsuarioEjecutor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "Error al verificar credenciales del usuario.";
                return RedirectToPage("/Solicitudes/ListarSolicitudes");
            }

            await _solicitudService.CrearAsync(Solicitud, idUsuarioEjecutor);

            TempData["Mensaje"] = "Solicitud creada exitosamente.";
            return RedirectToPage("/Solicitudes/ListarSolicitudes");
        }
        private async Task CargarListasAsync()
        {
            var reps = await _representanteService.ObtenerTodosAsync();
            Representantes = reps.Select(r => new SelectListItem
            {
                Value = r.id_representante.ToString(),
                Text = r.nombre
            }).ToList();
            Representantes.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione representante --" });

            var estados = await _estadoSolicitudService.ObtenerTodosAsync();
            EstadosSolicitud = estados.Select(e => new SelectListItem
            {
                Value = e.id_estado,
                Text = e.Estado
            }).ToList();
            EstadosSolicitud.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione estado --" });
        }
    }
}
