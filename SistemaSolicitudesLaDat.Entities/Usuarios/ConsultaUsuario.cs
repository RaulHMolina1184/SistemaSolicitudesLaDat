using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Entities.Usuarios
{
    public class ConsultaUsuario : Usuario // Hereda de Usuario para incluir propiedades adicionales específicas de la consulta
    {
        public int Encontrado { get; set; } // Indicador de si se encontró el usuario (1 = encontrado, 0 = no encontrado)
    }
}
