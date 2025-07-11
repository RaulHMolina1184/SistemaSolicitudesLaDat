using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Solicitudes;
using SistemaSolicitudesLaDat.Service.Abstract;

namespace SistemaSolicitudesLaDat.Pages.Estados
{
    public class EditarModel : PageModel
    {
        private readonly IEstadoSolicitudService _estadoService;
        private readonly IBitacoraService _bitacoraService;

        public EditarModel(IEstadoSolicitudService estadoService, IBitacoraService bitacoraService)
        {
            _estadoService = estadoService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public EstadoSolicitud Estado { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var estado = await _estadoService.ObtenerPorIdAsync(id);
            if (estado == null)
                return NotFound();

            Estado = estado;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var idUsuarioEjecutor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "No se pudo verificar el usuario.";
                return RedirectToPage("/Estados/Index");
            }

            var actualizado = await _estadoService.ActualizarAsync(Estado, idUsuarioEjecutor);

            TempData["Mensaje"] = actualizado
                ? "Estado actualizado correctamente."
                : "No se pudo actualizar el estado.";

            return RedirectToPage("/Estados/Index");
        }
    }
}
