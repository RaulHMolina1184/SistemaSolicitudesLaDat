using System.ComponentModel.DataAnnotations;

namespace SistemaSolicitudesLaDat.Models
{
    public class NuevoUsuarioFormModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string Contrasenia { get; set; }

        [Required(ErrorMessage = "Debe confirmar la contraseña.")]
        [Compare("Contrasenia", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContrasenia { get; set; } // ✅ Agregada correctamente

        [Required(ErrorMessage = "Debe seleccionar un estado.")]
        public string Estado { get; set; }
    }
}
