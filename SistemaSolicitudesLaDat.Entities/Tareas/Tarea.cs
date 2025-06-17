using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Entities.Tareas
{
    public class Tarea
    {
        public string IdTarea { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal HorasInvertidas { get; set; }
        public string IdUsuario { get; set; } = string.Empty;
        public string IdSolicitud { get; set; } = string.Empty;

        // Extras para mostrar
        public string? NombreUsuario { get; set; }
        public string? TituloSolicitud { get; set; }
    }
}

