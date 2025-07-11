using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Solicitudes;
using SistemaSolicitudesLaDat.Service.Abstract;

namespace SistemaSolicitudesLaDat.Pages.Estados
{
    public class CrearEstadosModel : PageModel
    {
        private readonly IEstadoSolicitudService _estadoSolicitudService;

        public CrearEstadosModel(IEstadoSolicitudService estadoSolicitudService)
        {
            _estadoSolicitudService = estadoSolicitudService;
        }

        [BindProperty]
        public EstadoSolicitud EstadoSolicitud { get; set; } = new EstadoSolicitud();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var idUsuarioEjecutor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "Error al verificar credenciales del usuario. Consulte con TI.";
                return RedirectToPage("/Estados/Index");
            }

            await _estadoSolicitudService.CrearAsync(EstadoSolicitud, idUsuarioEjecutor);

            TempData["Mensaje"] = "Estado de solicitud agregado exitosamente.";
            return RedirectToPage("/Estados/Index");
        }
    }
}
