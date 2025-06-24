using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using SistemaSolicitudesLaDat.Entities.Representantes;
using SistemaSolicitudesLaDat.Service.Abstract;

namespace SistemaSolicitudesLaDat.Pages.Representantes
{
    [Authorize]
    public class EditarRepresentanteModel : PageModel
    {
        private readonly IRepresentanteService _representanteService;

        public EditarRepresentanteModel(IRepresentanteService representanteService)
        {
            _representanteService = representanteService;
        }

        [BindProperty]
        public Representante Representante { get; set; }



        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var entidad = await _representanteService.ObtenerPorIdAsync(id);
            if (entidad == null)
            {
                TempData["Mensaje"] = "Representante no encontrado.";
                return NotFound();
            }

            entidad.IdRepresentante = id.Trim(); // Fuerza el ID desde la URL
            Representante = entidad;

            TempData["Debug"] = $"Cargado ID: {Representante.IdRepresentante}";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            TempData["Debug"] = $"ID recibido: '{Representante.IdRepresentante}'";

            if (string.IsNullOrWhiteSpace(Representante.IdRepresentante))
            {
                TempData["Mensaje"] = "ID inválido.";
                return Page();
            }

            var resultado = await _representanteService.EditarAsync(Representante);
            TempData["Mensaje"] = resultado.Mensaje;

            return RedirectToPage("ListarRepresentantes");
        }
    }
}
