using System.ComponentModel.DataAnnotations;

namespace HankoSpa.DTOs
{
    public class CitasServiciosDTO
    {
        [Required(ErrorMessage = "El ID de la cita es obligatorio")]
        public int CitasID { get; set; }

        [Required(ErrorMessage = "El ID del servicio es obligatorio")]
        public int ServicioID { get; set; }
    }
}
