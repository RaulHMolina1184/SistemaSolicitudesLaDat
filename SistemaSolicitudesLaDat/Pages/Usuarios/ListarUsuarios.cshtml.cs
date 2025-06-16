using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Usuarios;
using SistemaSolicitudesLaDat.Pages.Models;
using SistemaSolicitudesLaDat.Service.Abstract;

namespace SistemaSolicitudesLaDat.Pages.Usuarios
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IBitacoraService _bitacoraService;

        public IndexModel(IUsuarioService usuarioService, IBitacoraService bitacoraService)
        {
            _usuarioService = usuarioService;
            _bitacoraService = bitacoraService;
        }

        public List<ListarUsuariosModel> Usuarios { get; set; } = new();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }

        private const int TamanoPagina = 10;

        public async Task<IActionResult> OnGetAsync(int pagina = 1)
        {
            var usuariosEntidad = await _usuarioService.GetUsuariosPaginadosAsync(pagina, TamanoPagina);
            var total = await _usuarioService.CuentaUsuariosAsync();

            TotalPaginas = (int)Math.Ceiling(total / (double)TamanoPagina);
            PaginaActual = pagina;

            Usuarios = usuariosEntidad.Select(u => new ListarUsuariosModel
            {
                Id_Usuario = u.Id_Usuario,
                Nombre_Usuario = u.Nombre_Usuario,
                Nombre_Completo = u.Nombre_Completo,
                Correo_Electronico = u.Correo_Electronico,
                Estado = u.Estado.ToString()
            }).ToList();

            // Registrar bitácora
            var idUsuarioEjecutor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                await _bitacoraService.RegistrarAccionAsync(
                    idUsuarioEjecutor,
                    "Consulta paginada de usuarios",
                    new { Pagina = pagina, UsuariosMostrados = Usuarios.Count }
                );
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(string id_usuario)
        {
            var idUsuarioEjecutor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "No se pudo obtener el usuario autenticado.";
                return RedirectToPage();
            }

            int resultado = await _usuarioService.DeleteAsync(id_usuario, idUsuarioEjecutor);

            TempData["Mensaje"] = resultado == 1
                ? "Usuario eliminado correctamente."
                : "No se pudo eliminar el usuario. Puede estar relacionado con otros registros.";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCambiarEstadoAsync(string id_usuario, string nuevo_estado)
        {
            if (string.IsNullOrWhiteSpace(id_usuario) || string.IsNullOrWhiteSpace(nuevo_estado))
            {
                TempData["Mensaje"] = "Datos incompletos para cambiar el estado.";
                return RedirectToPage();
            }

            var usuario = await _usuarioService.GetByIdAsync(id_usuario);
            if (usuario == null)
            {
                TempData["Mensaje"] = "Usuario no encontrado.";
                return RedirectToPage();
            }

            if (!Enum.TryParse<EstadoUsuario>(nuevo_estado, out var estadoConvertido))
            {
                TempData["Mensaje"] = "Estado no válido.";
                return RedirectToPage();
            }

            var estadoAnterior = usuario.Estado;
            usuario.Estado = estadoConvertido;

            var idUsuarioEjecutor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUsuarioEjecutor))
            {
                TempData["Mensaje"] = "No se pudo obtener el ID del usuario autenticado.";
                return RedirectToPage();
            }

            var actualizado = await _usuarioService.UpdateAsync(usuario, idUsuarioEjecutor);

            if (actualizado == 1)
            {
                await _bitacoraService.RegistrarAccionAsync(
                    idUsuarioEjecutor,
                    $"Cambio de estado de usuario",
                    new
                    {
                        Id_Usuario = usuario.Id_Usuario,
                        Nombre_Usuario = usuario.Nombre_Usuario,
                        Estado_Anterior = estadoAnterior.ToString(),
                        Estado_Nuevo = nuevo_estado
                    }
                );
                TempData["Mensaje"] = $"Estado cambiado a {nuevo_estado}.";
            }
            else
            {
                TempData["Mensaje"] = "Error al cambiar el estado.";
            }

            return RedirectToPage();
        }
    }
}
