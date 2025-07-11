using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaSolicitudesLaDat.Entities.Solicitudes;
using SistemaSolicitudesLaDat.Service.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Pages.Solicitudes
{
    [Authorize]
    public class EditarSolicitudesModel : PageModel
    {
        private readonly ISolicitudService _solicitudService;
        private readonly IRepresentanteService _representanteService;
        private readonly IEstadoSolicitudService _estadoSolicitudService;

        public EditarSolicitudesModel(
            ISolicitudService solicitudService,
            IRepresentanteService representanteService,
            IEstadoSolicitudService estadoSolicitudService)
        {
            _solicitudService = solicitudService;
            _representanteService = representanteService;
            _estadoSolicitudService = estadoSolicitudService;
        }

        [BindProperty]
        public Solicitud Solicitud { get; set; } = new();

        public List<SelectListItem> Representantes { get; set; } = new();
        public List<SelectListItem> Estados { get; set; } = new();

        private async Task CargarListasAsync()
        {
            var reps = await _representanteService.ObtenerTodosAsync();
            Representantes = reps.Select(r => new SelectListItem
            {
                Value = r.id_representante,
                Text = r.nombre
            }).ToList();
            Representantes.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione representante --" });

            var estados = await _estadoSolicitudService.ObtenerTodosAsync();
            Estados = estados.Select(e => new SelectListItem
            {
                Value = e.id_estado,
                Text = e.Estado
            }).ToList();

            foreach (var item in Estados)
            {
                if (item.Value.Trim().ToUpper() == Solicitud.estado_solicitud?.Trim().ToUpper())
                {
                    item.Selected = true;
                    break;
                }
            }

            Estados.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione estado --" });
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var solicitud = await _solicitudService.ObtenerPorIdAsync(id);
            if (solicitud == null)
                return NotFound();

            Solicitud = solicitud;

            await CargarListasAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarListasAsync();
                return Page();
            }

            var idUsuarioEjecutor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "Error al verificar credenciales del usuario. Consulte con TI.";
                return RedirectToPage("/Solicitudes/ListarSolicitudes");
            }

            await _solicitudService.ActualizarAsync(Solicitud, idUsuarioEjecutor);

            TempData["Mensaje"] = "Solicitud actualizada exitosamente.";
            return RedirectToPage("/Solicitudes/ListarSolicitudes");
        }
    }
}
