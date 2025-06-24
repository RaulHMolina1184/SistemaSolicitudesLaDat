using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaSolicitudesLaDat.Entities.Tareas;
using SistemaSolicitudesLaDat.Repository.Reportes;
using SistemaSolicitudesLaDat.Repository.Solicitudes;

namespace SistemaSolicitudesLaDat.Pages.Reportes
{
    public class TareasPorSolicitudModel : PageModel
    {
        private readonly ReporteRepository _reporteRepo;
        private readonly SolicitudRepository _solicitudRepo;

        public TareasPorSolicitudModel(ReporteRepository reporteRepo, SolicitudRepository solicitudRepo)
        {
            _reporteRepo = reporteRepo;
            _solicitudRepo = solicitudRepo;
        }

        [BindProperty(SupportsGet = true)]
        public string IdSolicitud { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public int Mes { get; set; } = DateTime.Today.Month;

        [BindProperty(SupportsGet = true)]
        public int Anio { get; set; } = DateTime.Today.Year;

        public List<SelectListItem> Solicitudes { get; set; } = new();

        public TareaReporteResultado Reporte { get; set; } = new();

        public bool BusquedaRealizada { get; set; } = false;

        public async Task OnGetAsync()
        {
            var solicitudesDb = await _solicitudRepo.ObtenerTodasAsync();
            Solicitudes = solicitudesDb
                .Select(s => new SelectListItem { Value = s.Id_Solicitud, Text = s.Titulo_Solicitud })
                .ToList();

            if (!string.IsNullOrEmpty(IdSolicitud))
            {
                Reporte = await _reporteRepo.ObtenerTareasPorSolicitudYMes(IdSolicitud, Mes, Anio);
                BusquedaRealizada = true;
            }
        }
    }
}
