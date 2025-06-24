using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Entities.Tareas
{
    public class TareaReporte
    {
        public string Usuario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal HorasInvertidas { get; set; }
        public string ConsecutivoOficio { get; set; } = string.Empty;
        public string TituloSolicitud { get; set; } = string.Empty;
    }

    public class TareaReporteResultado
    {
        public List<TareaReporte> Tareas { get; set; } = new();
        public decimal TotalHoras { get; set; }
    }
}

