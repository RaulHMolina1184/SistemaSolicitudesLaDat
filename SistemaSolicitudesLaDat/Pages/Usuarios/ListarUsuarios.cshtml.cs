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
        private readonly IBitacoraService _bitacoraService;

        public IndexModel(IUsuarioService usuarioService, IBitacoraService bitacoraService)
        {
            _usuarioService = usuarioService;
            _bitacoraService = bitacoraService;
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

            // Obtener el ID del usuario autenticado
            var idUsuarioEjecutor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                await _bitacoraService.RegistrarAccionAsync(
                    idUsuarioEjecutor,
                    "Consulta de lista de usuarios",
                    new { TotalUsuarios = Usuarios.Count }
                );
            }

        }

        public async Task<IActionResult> OnPostEliminarAsync(string id_usuario)
        {
            if (string.IsNullOrEmpty(id_usuario))
            {
                TempData["Mensaje"] = "Error al eliminar el usuario.";
                return RedirectToPage();
            }

            // Obtener ID del usuario autenticado
            var idUsuarioEjecutor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "No se pudo obtener el ID del usuario autenticado.";
                return RedirectToPage();
            }

            int resultado = await _usuarioService.DeleteAsync(id_usuario, idUsuarioEjecutor);

            TempData["Mensaje"] = resultado == 1
                ? "Usuario eliminado correctamente."
                : "No se pudo eliminar el usuario. Puede estar relacionado con otros registros.";

            return RedirectToPage();
        }

    }
}
