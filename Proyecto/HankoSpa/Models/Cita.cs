using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HankoSpa.Models
{
    public class Cita
    {
        [Key]
        public int CitaID { get; set; }

        [Required(ErrorMessage = "La fecha de la cita es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaCita { get; set; }

        [Required(ErrorMessage = "La hora de la cita es obligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan HoraCita { get; set; }

        [Required(ErrorMessage = "El estado de la cita es obligatorio")]
        public string EstadoCita { get; set; }
        
        // Relación muchos a muchos
        public virtual ICollection<CitasServicios> CitasServicios { get; set; }

        // Este campo no se mostrará en formularios, pero se puede asignar automáticamente en el backend
        //[ForeignKey("User")]
        public String? UsuarioID { get; set; }
        public virtual User? User { get; set; }
    }
}
