using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Entities.Desgloses
{
    public class Desglose
    {
        [Key]
        [Column("id_desglose")]
        [StringLength(10)]
        public string id_desglose { get; set; } = string.Empty;

        [Required]
        [Column("mes")]
        public byte mes { get; set; }

        [Required]
        [Range(2000, 2100, ErrorMessage = "El año debe estar entre 2000 y 2100.")]
        [Column("anio")]
        public int anio { get; set; }

        [Required]
        [Range(1, 24, ErrorMessage = "Las horas deben estar entre 1 y 24.")]
        [Column("horas")]
        public byte horas { get; set; }

        [Required]
        [Column("monto")]
        public decimal monto { get; set; }

        [Required]
        [Column("total")]
        public decimal total { get; set; }

        [Column("observaciones")]
        [StringLength(500)]
        public string? observaciones { get; set; }

        [Required]
        [Column("porcentaje_cobro")]
        public decimal porcentaje_cobro { get; set; }

        [Required]
        [Column("id_solicitud")]
        [StringLength(10)]
        public string id_solicitud { get; set; } = string.Empty;

        [Required]
        [Column("id_iva")]
        [StringLength(10)]
        public string id_iva { get; set; } = string.Empty;

        [NotMapped]
        public ImpuestoValorAgregado? Impuesto { get; set; }
    }
}
