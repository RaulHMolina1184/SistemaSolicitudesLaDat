namespace SistemaSolicitudesLaDat.Pages.Models
{
    public class ListarUsuariosModel
    {
        public string Id_Usuario { get; set; }
        public string Nombre_Usuario { get; set; }
        public string Nombre_Completo { get; set; }
        public string Correo_Electronico { get; set; }
        public string Estado { get; set; } // o usar un enum si lo tenés mapeado
    }
}
