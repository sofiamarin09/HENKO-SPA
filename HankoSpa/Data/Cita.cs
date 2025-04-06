using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HankoSpa.Models
{
    public class Cita
    {
        [key]
        public int CitaId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaCita { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan HoraCita { get; set; }

        [Required]
        public string EstadoCita { get; set; }

        public int UsuarioID { get; set; }

        public virtual ICollection<CitasServicios> CitasServicios { get; set; }
    }
}