using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HankoSpa.Models
{
    public class RolPermission
    {
        public int RolId { get; set; }
        public Rol Rol { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }

    }
}