using System.ComponentModel.DataAnnotations;
using SistemaSolicitudesLaDat.Pages.Models;


namespace SistemaSolicitudesLaDat.Pages.Models;

public class RepresentanteFormularioModel
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
    public string Email { get; set; } = string.Empty;

    public string? IdRepresentante { get; set; }

}
