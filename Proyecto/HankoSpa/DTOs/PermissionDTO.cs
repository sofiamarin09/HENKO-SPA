using System.ComponentModel.DataAnnotations;

namespace HankoSpa.DTOs
{
    public class PermissionDTO
    {
        public int PermisoId { get; set; }

        [Required(ErrorMessage = "El nombre del permiso es obligatorio")]
        [StringLength(50)]
        public string NombrePermiso { get; set; } = null!;

        [Required(ErrorMessage = "El módulo del permiso es obligatorio")]
        [StringLength(50)]
        public string Module { get; set; } = null!;
    }
}
