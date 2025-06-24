using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities;
using SistemaSolicitudesLaDat.Entities.Solicitudes;
using SistemaSolicitudesLaDat.Service.Solicitudes;

namespace SistemaSolicitudesLaDat.Pages.Solicitudes
{
    public class ListarModel : PageModel
    {
        private readonly SolicitudService _solicitudService;

        public ListarModel(SolicitudService solicitudService)
        {
            _solicitudService = solicitudService;
        }

        public List<SolicitudResumen> Solicitudes { get; set; } = new();

        public async Task OnGetAsync()
        {
            Solicitudes = await _solicitudService.ObtenerResumenes();
        }
    }
}