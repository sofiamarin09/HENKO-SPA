using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HankoSpa.Models
{
    public class RolPermission
    {
        public int CustomRolId { get; set; }
        public CustomRol CustomRol { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }

    }
}