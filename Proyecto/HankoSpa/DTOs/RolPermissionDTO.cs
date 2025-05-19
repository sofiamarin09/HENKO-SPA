namespace HankoSpa.DTOs
{
    public class RolPermissionDTO
    {
        public int CustomRolId { get; set; }
        public int PermissionId { get; set; }

        // Opcional: para mostrar nombres en vistas
        public string? NombreRol { get; set; }
        public string? NombrePermiso { get; set; }
        public string? Module { get; set; }
    }
}