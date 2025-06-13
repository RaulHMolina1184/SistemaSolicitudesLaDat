using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Service.Login;

namespace SistemaSolicitudesLaDat.Pages
{
    public class IndexModel : PageModel
    {
        private readonly LoginService _loginService;

        public IndexModel(LoginService loginService)
        {
            _loginService = loginService;
        }

        [BindProperty]
        public string NombreUsuario { get; set; }

        [BindProperty]
        public string Contrasenia { get; set; }

        public string Mensaje { get; set; }
        public string MensajeInfo { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var usuario = _loginService.Login(NombreUsuario, Contrasenia);

            if (usuario != null)
            {
                // Crear los claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre_Usuario),
                    new Claim("NombreCompleto", usuario.Nombre_Completo),
                };

                var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identidad);

                // Iniciar sesión con cookies
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToPage("/Principal");
            }

            Mensaje = "Usuario y/o contraseña incorrectos.";
            return Page();
        }

        public void OnGet(string? returnUrl = null)
        {
            if (!User.Identity.IsAuthenticated && !string.IsNullOrEmpty(returnUrl))
            {
                MensajeInfo = "Por favor inicie sesión para utilizar el sistema.";
            }
        }

    }
}
