using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Entities.Usuarios;
using SistemaSolicitudesLaDat.Service.Abstract;
using SistemaSolicitudesLaDat.Service.Encriptado;
using SistemaSolicitudesLaDat.Service.Usuarios;
using SistemaSolicitudesLaDat.Service.Encriptado;

namespace SistemaSolicitudesLaDat.Pages.Usuarios
{
    public class AgregarUsuarioModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;
        private readonly EncriptadoService _encriptadoService;

        public AgregarUsuarioModel(IUsuarioService usuarioService, EncriptadoService encriptadoService)
        {
            _usuarioService = usuarioService;
            _encriptadoService = encriptadoService;
        }

        [BindProperty]
        public Usuario Usuario { get; set; } = new Usuario();

        [BindProperty]
        public string Contrasenia { get; set; }

        public string Mensaje { get; set; }

        public void OnGet()
        {
            // Inicialización opcional
        }

    }
}
