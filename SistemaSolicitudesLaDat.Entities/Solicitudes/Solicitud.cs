using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaSolicitudesLaDat.Entities.Solicitudes
{
    public class Solicitud
    {
        [Display(Name = "ID de Solicitud")]
        public string id_solicitud { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Consecutivo oficio de la solicitud")]
        public string consecutivo_oficio { get; set; } = string.Empty;

        [Display(Name = "Documento de respuesta")]
        public string? documento_respuesta { get; set; }

        [Display(Name = "Documento de inicio")]
        public string? documento_inicio { get; set; }

        [Display(Name = "Título de la solicitud")]
        public string titulo_solicitud { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        public string? descripcion { get; set; }

        [Display(Name = "Fecha de ingreso")]
        public DateTime? fecha_ingreso { get; set; }

        [Display(Name = "Fecha de inicio")]
        public DateTime? fecha_inicio { get; set; }

        [Display(Name = "Fecha de respuesta")]
        public DateTime? fecha_respuesta { get; set; }

        [Display(Name = "Observaciones")]
        public string? observaciones { get; set; }

        [Display(Name = "Estado de la solicitud")]
        public string? estado_solicitud { get; set; } 

        [Display(Name = "Representante")]
        public string? id_representante { get; set; }

        //public string? nombre { get; set; }
       [Display(Name = "Estado de la solicitud")]
       public string? EstadoNombre { get; set; }

        [Display(Name = "Representante")]

        public string? RepresentanteNombre { get; set; }
    }
}
