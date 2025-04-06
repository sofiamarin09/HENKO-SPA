using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HankoSpa.Models
{
    public class Servicio
    {
        public int ServicioId { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreServicio { get; set; }

        [StringLength(500)]
        public string DescripcionServicio { get; set; }

        public virtual ICollection<CitasServicios> CitasServicios { get; set; }

    }
}