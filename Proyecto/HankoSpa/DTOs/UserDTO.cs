using System.ComponentModel.DataAnnotations;
using HankoSpa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HankoSpa.DTOs
{
    public class UserDTO
    {
        public string? Id { get; set; }

        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Nombre completo")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Documento")]
        [Required(ErrorMessage = "El documento es obligatorio")]
        public string Document { get; set; } = null!;


        [Display(Name = "Telefono")]
        [Required(ErrorMessage = "El telefono es obligatorio")]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El email es obligatorio")]
        public string Email { get; set; } = null!;


        [Display(Name = "Rol")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un rol")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public int CustomRolId { get; set; }

        public string? CustomRol { get; set; }

        public ICollection<Cita>? Citas { get; set; }

        public IEnumerable<SelectListItem> CustomRoles { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmPassword { get; set; }

    }
}
