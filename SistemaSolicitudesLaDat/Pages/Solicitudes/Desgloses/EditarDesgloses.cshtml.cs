using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using SistemaSolicitudesLaDat.Entities.Desgloses;
using SistemaSolicitudesLaDat.Service.Abstract;
using System.Security.Claims;
using System.Globalization;

namespace SistemaSolicitudesLaDat.Pages.Solicitudes.Desgloses
{
    [Authorize]
    public class EditarDesglosesModel : PageModel
    {
        private readonly IDesgloseService _desgloseService;
        private readonly IImpuestoService _impuestoService;

        public EditarDesglosesModel(IDesgloseService desgloseService, IImpuestoService impuestoService)
        {
            _desgloseService = desgloseService;
            _impuestoService = impuestoService;
        }

        [BindProperty]
        public Desglose Desglose { get; set; } = new();

        public List<SelectListItem> Meses { get; set; } = new();
        public List<SelectListItem> Ivas { get; set; } = new();
        public Dictionary<string, decimal> IvaPorcentajes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var desglose = await _desgloseService.GetByIdAsync(id);
            if (desglose == null) return NotFound();

            Desglose = desglose;
            await CargarListasAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var listaIvas = await _impuestoService.ObtenerTodosIVAAsync();
            IvaPorcentajes = listaIvas.ToDictionary(i => i.Id_iva, i => i.monto_iva);

            if (!ModelState.IsValid)
            {
                await CargarListasAsync();
                return Page();
            }

            if (!IvaPorcentajes.ContainsKey(Desglose.id_iva))
            {
                ModelState.AddModelError("Desglose.id_iva", "Seleccione un IVA válido.");
                await CargarListasAsync();
                return Page();
            }

            Desglose.total = Desglose.monto * (1 + IvaPorcentajes[Desglose.id_iva]);

            var idUsuarioEjecutor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "Error al verificar el usuario. Intente nuevamente.";
                return RedirectToPage("ListarDesgloses", new { id = Desglose.id_solicitud });
            }

            await _desgloseService.UpdateAsync(Desglose, idUsuarioEjecutor);

            TempData["Mensaje"] = "Desglose actualizado correctamente.";
            return RedirectToPage("ListarDesgloses", new { id = Desglose.id_solicitud });
        }


        private async Task CargarListasAsync()
        {
            Meses = new List<SelectListItem>
            {
                new("Enero", "1"),
                new("Febrero", "2"),
                new("Marzo", "3"),
                new("Abril", "4"),
                new("Mayo", "5"),
                new("Junio", "6"),
                new("Julio", "7"),
                new("Agosto", "8"),
                new("Septiembre", "9"),
                new("Octubre", "10"),
                new("Noviembre", "11"),
                new("Diciembre", "12")
            };

            var ivas = await _impuestoService.ObtenerTodosIVAAsync();
            Ivas = ivas.Select(i => new SelectListItem
            {
                Text = $"{i.monto_iva * 100:F0}%",
                Value = i.Id_iva
            }).ToList();

            IvaPorcentajes = ivas.ToDictionary(i => i.Id_iva, i => i.monto_iva);
        }
    }
}
