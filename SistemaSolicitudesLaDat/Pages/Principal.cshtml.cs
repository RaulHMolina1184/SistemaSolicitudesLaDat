using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SistemaSolicitudesLaDat.Pages
{
    [Authorize]
    public class PrincipalModel : PageModel
    {
        public string NombreCompleto { get; set; }
        public void OnGet()
        {
            NombreCompleto = User.FindFirst("NombreCompleto")?.Value ?? "Usuario";
        }
    }
}
