using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSolicitudesLaDat.Entities.Representantes
{
    public class Representante
    {
        [Key]
        [Column("id_representante")]
        [StringLength(10)]
        public string id_representante { get; set; } = string.Empty;

        [Required]
        [Column("nombre")]
        [StringLength(100)]
        public string nombre { get; set; } = string.Empty;

        [Required]
        [Column("email")]
        [StringLength(100)]
        public string email { get; set; } = string.Empty;
    }
}
