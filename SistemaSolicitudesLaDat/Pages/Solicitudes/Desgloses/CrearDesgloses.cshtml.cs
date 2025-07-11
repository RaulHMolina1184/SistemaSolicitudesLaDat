using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Desgloses;
using SistemaSolicitudesLaDat.Service.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace SistemaSolicitudesLaDat.Pages.Solicitudes.Desgloses
{
    [Authorize]
    public class CrearDesglosesModel : PageModel
    {
        private readonly IDesgloseService _desgloseService;
        private readonly IImpuestoService _impuestoService;

        public CrearDesglosesModel(IDesgloseService desgloseService, IImpuestoService impuestoService)
        {
            _desgloseService = desgloseService;
            _impuestoService = impuestoService;
        }

        [BindProperty]
        public Desglose Desglose { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string IdSolicitud { get; set; } = string.Empty;

        public List<SelectListItem> Meses { get; set; } = new();
        public List<SelectListItem> Ivas { get; set; } = new();
        public Dictionary<string, decimal> IvaPorcentajes { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (string.IsNullOrEmpty(Desglose.id_solicitud))
                Desglose.id_solicitud = IdSolicitud;

            CargarMeses();

            var listaIvas = await _impuestoService.ObtenerTodosIVAAsync();

            Ivas = listaIvas.Select(iva => new SelectListItem
            {
                Text = $"{iva.monto_iva * 100:F0}%",
                Value = iva.Id_iva
            }).ToList();

            IvaPorcentajes = listaIvas.ToDictionary(iva => iva.Id_iva, iva => iva.monto_iva);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var listaIvas = await _impuestoService.ObtenerTodosIVAAsync();
            IvaPorcentajes = listaIvas.ToDictionary(iva => iva.Id_iva, iva => iva.monto_iva);

            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            if (!IvaPorcentajes.ContainsKey(Desglose.id_iva))
            {
                ModelState.AddModelError("Desglose.id_iva", "Seleccione un IVA válido.");
                await OnGetAsync();
                return Page();
            }

            Desglose.id_solicitud = IdSolicitud;

            var idUsuarioEjecutor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "Error al verificar el usuario. Intente nuevamente.";
                return RedirectToPage("/Solicitudes/Desgloses/ListarDesgloses", new { id = IdSolicitud });
            }

            if (IvaPorcentajes.TryGetValue(Desglose.id_iva, out decimal ivaPorcentaje))
            {
                Desglose.total = Desglose.monto * (1 + ivaPorcentaje);
            }

            await _desgloseService.InsertAsync(Desglose, idUsuarioEjecutor);

            TempData["Mensaje"] = "Desglose creado exitosamente.";
            return RedirectToPage("/Solicitudes/Desgloses/ListarDesgloses", new { id = IdSolicitud });
        }

        private void CargarMeses()
        {
            Meses = new List<SelectListItem>
            {
                new SelectListItem("Enero", "1"),
                new SelectListItem("Febrero", "2"),
                new SelectListItem("Marzo", "3"),
                new SelectListItem("Abril", "4"),
                new SelectListItem("Mayo", "5"),
                new SelectListItem("Junio", "6"),
                new SelectListItem("Julio", "7"),
                new SelectListItem("Agosto", "8"),
                new SelectListItem("Septiembre", "9"),
                new SelectListItem("Octubre", "10"),
                new SelectListItem("Noviembre", "11"),
                new SelectListItem("Diciembre", "12")
            };
        }
    }
}
