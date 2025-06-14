using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaSolicitudesLaDat.Service.Abstract;
using SistemaSolicitudesLaDat.Service.Login;
using System.Security.Claims;

namespace SistemaSolicitudesLaDat.Pages
{
    public class IndexModel : PageModel
    {
        private readonly LoginService _loginService;
        private readonly IBitacoraService _bitacoraService;

        public IndexModel(LoginService loginService, IBitacoraService bitacoraService)
        {
            _loginService = loginService;
            _bitacoraService = bitacoraService;
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
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id_Usuario),
                    new Claim(ClaimTypes.Name, usuario.Nombre_Usuario),
                    new Claim("NombreCompleto", usuario.Nombre_Completo),
                };

                var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identidad);

                // Iniciar sesión con cookies
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Registrar en bitácora
                await _bitacoraService.RegistrarAccionAsync(
                    usuario.Id_Usuario,
                    "Inicio de sesión",
                    new
                    {
                        usuario.Nombre_Usuario,
                        Fecha = DateTime.UtcNow
                    }
                );

                return RedirectToPage("/Principal");
            }

            Mensaje = "Usuario y/o contraseña incorrectos.";
            return Page();
        }

        public async Task OnGetAsync()
        {
            var returnUrl = Request.Query["returnUrl"].ToString();
            
            if (!User.Identity.IsAuthenticated && !string.IsNullOrEmpty(returnUrl))
            {
                MensajeInfo = "Por favor inicie sesión para utilizar el sistema.";

                try
                {
                   bool guardado = await _bitacoraService.RegistrarAccionAsync(
                        "anonimo",
                        "Intento de acceso no autorizado",
                        new
                        {
                            Ruta = returnUrl,
                            Fecha = DateTime.UtcNow,
                            IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Desconocida",
                            Navegador = Request.Headers["User-Agent"].ToString()
                        }
                    );

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Bitácora] Error al registrar: {ex.Message}");
                }
            }
        }

    }
}
