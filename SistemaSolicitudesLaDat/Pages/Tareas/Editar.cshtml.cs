using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Tareas;
using SistemaSolicitudesLaDat.Service.Tareas;

namespace SistemaSolicitudesLaDat.Pages.Tareas
{
    public class EditarModel : PageModel
    {
        private readonly TareaService _tareaService;

        public EditarModel(TareaService tareaService)
        {
            _tareaService = tareaService;
        }

        [BindProperty]
        public Tarea Tarea { get; set; } = new();

        public Tarea? TareaAnterior { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Console.WriteLine($"LLEGÓ ID: {id}");
            var tarea = await _tareaService.ObtenerPorId(id);
            if (tarea == null)
            {
                Console.WriteLine("TAREA NO ENCONTRADA");
                return RedirectToPage("Listar");
            }

            Tarea = tarea;
            Console.WriteLine("TAREA CARGADA: " + Tarea.Descripcion);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var anterior = await _tareaService.ObtenerPorId(Tarea.IdTarea);
            var actualizado = await _tareaService.Editar(Tarea, anterior!, User.Identity!.Name!);

            if (actualizado)
                return RedirectToPage("Listar", new { idSolicitud = Tarea.IdSolicitud });

            ModelState.AddModelError("", "No se pudo actualizar la tarea.");
            return Page();
        }
    }
}