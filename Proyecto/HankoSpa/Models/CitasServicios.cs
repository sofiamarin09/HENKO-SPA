using System.ComponentModel.DataAnnotations;

namespace HankoSpa.Models
{
    public class CitasServicios
    {
        [Key]
        public int Citas_ServiciosID { get; set; }

        public int CitasID { get; set; }
        public virtual Cita Cita { get; set; }

        public int ServicioID { get; set; }
        public virtual Servicio Servicio { get; set; }
    }
}