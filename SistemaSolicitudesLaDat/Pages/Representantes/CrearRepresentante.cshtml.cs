using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Pages.Models;
using SistemaSolicitudesLaDat.Service.Abstract;
using SistemaSolicitudesLaDat.Entities.Representantes;

namespace SistemaSolicitudesLaDat.Pages.Representantes
{
    public class CrearRepresentanteModel : PageModel
    {
        private readonly IRepresentanteService _representanteService;
        private readonly IBitacoraService _bitacoraService;

        public CrearRepresentanteModel(IRepresentanteService representanteService, IBitacoraService bitacoraService)
        {
            _representanteService = representanteService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public RepresentanteFormularioModel Formulario { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["Mensaje"] = "Por favor, complete todos los campos correctamente.";
                return Page();
            }

            var nuevo = new Representante
            {
                IdRepresentante = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(),
                Nombre = Formulario.Nombre.Trim(),
                Email = Formulario.Email.Trim()
            };

            TempData["Mensaje"] = $"Intentando crear: {nuevo.IdRepresentante}";

            var fueCreado = await _representanteService.CrearAsync(nuevo);

            if (!fueCreado)
            {
                TempData["Mensaje"] = "El correo ya está registrado.";
                return Page();
            }


            var idUsuario = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(idUsuario))
            {
                await _bitacoraService.RegistrarAccionAsync(
                    idUsuario,
                    "Creación de representante",
                    new { nuevo.IdRepresentante, nuevo.Nombre }
                );
            }

            TempData["Mensaje"] = "Representante creado correctamente.";
            return RedirectToPage("ListarRepresentantes");
        }
    }
}
