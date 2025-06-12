using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Usuarios;
using SistemaSolicitudesLaDat.Models; 
using SistemaSolicitudesLaDat.Service.Abstract;
using System.Text;

namespace SistemaSolicitudesLaDat.Pages.Usuarios
{
    public class AgregarUsuarioModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEncriptadoService _encriptadoService;

        public AgregarUsuarioModel(IUsuarioService usuarioService, IEncriptadoService encriptadoService)
        {
            _usuarioService = usuarioService;
            _encriptadoService = encriptadoService;
        }

        [BindProperty]
        public NuevoUsuarioFormModel UsuarioForm { get; set; } // Para usar el modelo de formulario

        public string Mensaje { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page(); // Verifica si el formulario cumple con las validaciones ([Required], [EmailAddress], etc.). Si hay errores, recarga la misma página para mostrar mensajes.

            var (cifrada, tag, nonce) = _encriptadoService.Encriptar(Encoding.UTF8.GetBytes(UsuarioForm.Contrasenia));

            var nuevoUsuario = new Usuario
            {
                NombreUsuario = UsuarioForm.NombreUsuario,
                NombreCompleto = UsuarioForm.NombreCompleto,
                CorreoElectronico = UsuarioForm.Correo,
                ContraseniaCifrada = cifrada,
                TagAutenticacion = tag,
                Nonce = nonce,
                Estado = Enum.Parse<EstadoUsuario>(UsuarioForm.Estado, ignoreCase: true)
            };

            await _usuarioService.InsertAsync(nuevoUsuario); // Para insercion de usuario
            return RedirectToPage("/Usuarios/AgregarUsuario");
        }
    }
}