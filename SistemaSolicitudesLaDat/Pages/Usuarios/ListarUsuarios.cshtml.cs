using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Pages.Models;
using SistemaSolicitudesLaDat.Service.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace SistemaSolicitudesLaDat.Pages.Usuarios
{
    [Authorize]
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

        public async Task<IActionResult> OnPostEliminarAsync(string id_usuario)
        {
            if (string.IsNullOrEmpty(id_usuario))
            {
                TempData["Mensaje"] = "Error al eliminar el usuario.";
                return RedirectToPage();
            }

            int resultado = await _usuarioService.DeleteAsync(id_usuario);

            TempData["Mensaje"] = resultado == 1
                ? "Usuario eliminado correctamente."
                : "No se pudo eliminar el usuario. Puede estar relacionado con otros registros.";

            return RedirectToPage();
        }
    }
}
