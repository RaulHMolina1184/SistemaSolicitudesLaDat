using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Service.Abstract;
using SistemaSolicitudesLaDat.Entities.Representantes;

namespace SistemaSolicitudesLaDat.Pages.Representantes
{
    public class AgregarRepresentanteModel : PageModel
    {
        private readonly IRepresentanteService _representanteService;

        public AgregarRepresentanteModel(IRepresentanteService representanteService)
        {
            _representanteService = representanteService;
        }

        [BindProperty]
        public Representante Representante { get; set; } = new Representante();

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

            await _representanteService.CrearAsync(Representante, idUsuarioEjecutor);

            TempData["Mensaje"] = "Representante agregado exitosamente.";
            return RedirectToPage("/Representantes/ListarRepresentantes");
        }

    }
}
