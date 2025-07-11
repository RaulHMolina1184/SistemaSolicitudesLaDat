using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Representantes;
using SistemaSolicitudesLaDat.Service.Abstract;

namespace SistemaSolicitudesLaDat.Pages.Representantes
{
    public class EditarRepresentanteModel : PageModel
    {
        private readonly IRepresentanteService _representanteService;

        public EditarRepresentanteModel(IRepresentanteService representanteService)
        {
            _representanteService = representanteService;
        }

        [BindProperty]
        public Representante Representante { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var representante = await _representanteService.ObtenerPorIdAsync(id);
            if (representante == null)
            {
                return NotFound();
            }

            Representante = representante;
            return Page();
        }

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
                return RedirectToPage("/Representantes/ListarRepresentantes");
            }

            await _representanteService.ActualizarAsync(Representante, idUsuarioEjecutor);

            TempData["Mensaje"] = "Representante actualizado exitosamente.";
            return RedirectToPage("/Representantes/ListarRepresentantes");
        }
    }
}
