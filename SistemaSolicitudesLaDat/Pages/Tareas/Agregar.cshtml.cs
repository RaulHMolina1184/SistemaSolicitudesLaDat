using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Tareas;
using SistemaSolicitudesLaDat.Service.Tareas;

namespace SistemaSolicitudesLaDat.Pages.Tareas
{
    public class AgregarModel : PageModel
    {
        private readonly TareaService _tareaService;

        public AgregarModel(TareaService tareaService)
        {
            _tareaService = tareaService;
        }

        [BindProperty(SupportsGet = true)]
        public string IdSolicitud { get; set; } = string.Empty;

        [BindProperty]
        public Tarea Tarea { get; set; } = new();

        public void OnGet()
        {
            Tarea.IdSolicitud = IdSolicitud;
            Tarea.Fecha = DateTime.Today;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Tarea.IdUsuario = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";

            Console.WriteLine($"DESCRIPCIÓN: {Tarea.Descripcion}");
            Console.WriteLine($"USUARIO: {Tarea.IdUsuario}");
            Console.WriteLine($"SOLICITUD: {Tarea.IdSolicitud}");
            Console.WriteLine($"TIEMPO: {Tarea.HorasInvertidas}");

            if (string.IsNullOrEmpty(Tarea.IdUsuario) || string.IsNullOrEmpty(Tarea.IdSolicitud) || string.IsNullOrEmpty(Tarea.Descripcion))
            {
                ModelState.AddModelError("", "Todos los campos son requeridos.");
                return Page();
            }

            var exito = await _tareaService.Crear(Tarea, Tarea.IdUsuario);

            if (exito)
                return RedirectToPage("Listar", new { idSolicitud = Tarea.IdSolicitud });

            ModelState.AddModelError("", "Error al guardar la tarea.");
            return Page();
        }
    }
}