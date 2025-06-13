using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Pages.Models;
using SistemaSolicitudesLaDat.Service.Abstract;

namespace SistemaSolicitudesLaDat.Pages.Usuarios
{
    public class IndexModel : PageModel
    {
        public List<ListarUsuariosModel> Usuarios { get; set; } = new();

        private readonly IUsuarioService _usuarioService;

        public IndexModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task OnGetAsync()
        {
            var usuariosEntidad = await _usuarioService.GetAllAsync();

            Usuarios = usuariosEntidad.Select(u => new ListarUsuariosModel
            {
                Id_Usuario = u.Id_Usuario,
                Nombre_Usuario = u.Nombre_Usuario,
                Nombre_Completo = u.Nombre_Completo,
                Correo_Electronico = u.Correo_Electronico,
                Estado = u.Estado.ToString() 
            }).ToList();
        }
    }
}
