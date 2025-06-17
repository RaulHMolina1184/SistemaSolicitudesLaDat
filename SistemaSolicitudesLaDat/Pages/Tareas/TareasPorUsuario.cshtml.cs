using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaSolicitudesLaDat.Entities.Tareas;
using SistemaSolicitudesLaDat.Repository.Reportes;
using SistemaSolicitudesLaDat.Repository.Usuarios;

namespace SistemaSolicitudesLaDat.Pages.Reportes
{
    public class TareasPorUsuarioModel : PageModel
    {
        private readonly ReporteRepository _reporteRepo;
        private readonly UsuarioRepository _usuarioRepo;

        public TareasPorUsuarioModel(ReporteRepository reporteRepo, UsuarioRepository usuarioRepo)
        {
            _reporteRepo = reporteRepo;
            _usuarioRepo = usuarioRepo;
        }

        [BindProperty(SupportsGet = true)]
        public string IdUsuario { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public int Mes { get; set; } = DateTime.Today.Month;

        [BindProperty(SupportsGet = true)]
        public int Anio { get; set; } = DateTime.Today.Year;

        public List<SelectListItem> Usuarios { get; set; } = new();

        public TareaReporteResultado Reporte { get; set; } = new();

        public bool BusquedaRealizada { get; set; } = false;

        public async Task OnGetAsync()
        {
            var usuariosDb = await _usuarioRepo.GetAllAsync();
            Usuarios = usuariosDb
                .Select(u => new SelectListItem { Value = u.Id_Usuario, Text = u.Nombre_Completo })
                .ToList();

            if (!string.IsNullOrEmpty(IdUsuario))
            {
                Reporte = await _reporteRepo.ObtenerTareasPorUsuarioYMes(IdUsuario, Mes, Anio);
                BusquedaRealizada = true;
            }
        }
    }
}