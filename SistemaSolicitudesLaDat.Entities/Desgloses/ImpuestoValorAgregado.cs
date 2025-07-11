using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Entities.Desgloses
{
    public class ImpuestoValorAgregado
    {
        [Key]
        [Column("id_iva")]
        [StringLength(10)]
        public string Id_iva { get; set; } = string.Empty;

        [Required]
        [Column("monto_iva")]
        public decimal monto_iva { get; set; }
    }
}

