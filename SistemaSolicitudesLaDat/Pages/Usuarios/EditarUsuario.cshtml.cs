using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Usuarios;
using SistemaSolicitudesLaDat.Pages.Models;
using SistemaSolicitudesLaDat.Service.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace SistemaSolicitudesLaDat.Pages.Usuarios
{
    [Authorize]
    public class EditarUsuarioModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;

        public EditarUsuarioModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public ListarUsuariosModel Usuario { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var usuarioEntidad = await _usuarioService.GetByIdAsync(id);
            if (usuarioEntidad == null)
            {
                return RedirectToPage("ListarUsuarios");
            }

            Usuario = new ListarUsuariosModel
            {
                Id_Usuario = usuarioEntidad.Id_Usuario,
                Nombre_Usuario = usuarioEntidad.Nombre_Usuario,
                Nombre_Completo = usuarioEntidad.Nombre_Completo,
                Correo_Electronico = usuarioEntidad.Correo_Electronico,
                Estado = usuarioEntidad.Estado.ToString()
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var usuarioActualizar = new Usuario
            {
                Id_Usuario = Usuario.Id_Usuario,
                Nombre_Usuario = Usuario.Nombre_Usuario,
                Nombre_Completo = Usuario.Nombre_Completo,
                Correo_Electronico = Usuario.Correo_Electronico,
                Estado = Enum.Parse<EstadoUsuario>(Usuario.Estado)
            };

            // Obtener ID del usuario autenticado para registrar la acción en la bitácora
            var idUsuarioEjecutor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "Error al verificar credenciales del usuario, consulte al departamento de TI.";
                return RedirectToPage("/Usuarios/ListarUsuarios");
            }

            var actualizado = await _usuarioService.UpdateAsync(usuarioActualizar, idUsuarioEjecutor);

            if (actualizado == 1)
                TempData["Mensaje"] = "Usuario actualizado correctamente.";
            else
                TempData["Mensaje"] = "Error al actualizar el usuario.";

            return RedirectToPage("ListarUsuarios");
        }

    }
}
