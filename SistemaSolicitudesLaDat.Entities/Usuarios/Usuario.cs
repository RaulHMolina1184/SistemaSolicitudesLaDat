namespace SistemaSolicitudesLaDat.Entities.Usuarios
{
    public class Usuario
    {
        public string UsuarioID { get; set; } // id_usuario
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string CorreoElectronico { get; set; }

        // Almacenar los datos cifrados correctamente
        public byte[] ContraseniaCifrada { get; set; } // Texto cifrado
        public byte[] TagAutenticacion { get; set; }   // Tag de autenticación (AES-GCM)
        public byte[] Nonce { get; set; }              // Nonce usado en la encriptación

        public EstadoUsuario Estado { get; set; } // Estado del usuario
        public int Encontrado { get; set; } // Indicador de si se encontró el usuario (1 = encontrado, 0 = no encontrado) desde SP
    }
}
