using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Tareas;
using SistemaSolicitudesLaDat.Service.Tareas;
using Microsoft.AspNetCore.Mvc;

namespace SistemaSolicitudesLaDat.Pages.Tareas
{
    public class ListarModel : PageModel
    {
        private readonly TareaService _tareaService;

        public ListarModel(TareaService tareaService)
        {
            _tareaService = tareaService;
        }

        public List<Tarea> ListaTareas { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string IdSolicitud { get; set; } = string.Empty;

        public async Task OnGet()
        {
            if (!string.IsNullOrEmpty(IdSolicitud))
            {
                var tareas = await _tareaService.ListarPorSolicitud(IdSolicitud);
                ListaTareas = tareas.ToList();
            }
        }

        public async Task<IActionResult> OnPostEliminarAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Mensaje"] = "Error al eliminar la tarea.";
                return RedirectToPage(new { idSolicitud = IdSolicitud });
            }

            var tarea = await _tareaService.ObtenerPorId(id);
            if (tarea == null)
            {
                TempData["Mensaje"] = "La tarea no existe.";
                return RedirectToPage(new { idSolicitud = IdSolicitud });
            }

            IdSolicitud = tarea.IdSolicitud;

            var idUsuario = User.Identity!.Name!;
            bool eliminado = await _tareaService.Eliminar(id, tarea, idUsuario);

            TempData["Mensaje"] = eliminado
                ? "Tarea eliminada correctamente."
                : "No se pudo eliminar la tarea.";

            return RedirectToPage(new { idSolicitud = IdSolicitud });
        }
    }
}