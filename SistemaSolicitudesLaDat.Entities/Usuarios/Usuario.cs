namespace SistemaSolicitudesLaDat.Entities.Usuarios
{
    public class Usuario
    {
        public string Id_Usuario { get; set; } // id_usuario
        public string Nombre_Usuario { get; set; }
        public string Nombre_Completo { get; set; }
        public string Correo_Electronico { get; set; }

        // Almacenar los datos cifrados correctamente
        public byte[] ContraseniaCifrada { get; set; } // Texto cifrado
        public byte[] TagAutenticacion { get; set; }   // Tag de autenticación (AES-GCM)
        public byte[] Nonce { get; set; }              // Nonce usado en la encriptación

        public EstadoUsuario Estado { get; set; } // Estado del usuario
        public int Encontrado { get; set; } // Indicador de si se encontró el usuario (1 = encontrado, 0 = no encontrado) desde SP
    }
}
