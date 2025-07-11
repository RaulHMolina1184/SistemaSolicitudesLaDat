using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Entities.Solicitudes
{
    public class EstadoSolicitud
    {
        [Key]
        [Column("id_estado")]
        [StringLength(10)]
        public string? id_estado { get; set; } 

        [Required]
        [Column("estado")]
        public string? Estado { get; set; } 
    }
}
