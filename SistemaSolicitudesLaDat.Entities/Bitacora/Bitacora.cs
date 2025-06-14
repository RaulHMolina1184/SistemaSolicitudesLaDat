using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Entities.Bitacora
{
    public class Bitacora
    {
        public string IdBitacora { get; set; }  // CHAR(10)
        public DateTime Fecha { get; set; }  // DATE
        public string UsuarioEjecutor { get; set; }  // CHAR(10)
        public string DescripcionAccion { get; set; }  // VARCHAR(100)
        public string ListadoAcciones { get; set; }  // JSON almacenado como string
        public string IdSolicitud { get; set; }  // CHAR(10)
    }

}
