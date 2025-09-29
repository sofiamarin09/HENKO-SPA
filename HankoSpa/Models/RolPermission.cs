using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HankoSpa.Models
{
    public class RolPermission
    {
        // Clave primaria compuesta: se configura en AppDbContext
        public int CustomRolId { get; set; }
        public int PermissionId { get; set; }

        // Relaciones de navegación (opcional pero recomendado)
        [ForeignKey("CustomRolId")]
        public virtual CustomRol CustomRol { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}
