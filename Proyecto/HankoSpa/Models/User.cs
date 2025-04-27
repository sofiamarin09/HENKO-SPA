using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace HankoSpa.Models
{
    public class User : IdentityUser
    {
        [Display(Name = "FirstName")]
        [Required(ErrorMessageResourceName = "RequiredField")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "LastName")]
        [Required(ErrorMessageResourceName = "RequiredField")]
        public string LastName { get; set; } = null!;

        [Display(Name = "UserType")]
        public UserType UserType { get; set; }

        [Display(Name = "User")]
        public string FullName => $"{FirstName} {LastName}";

        public virtual ICollection<Cita> Citas { get; set; }


    }
    public enum UserType
    {
        Cliente,
        Profesional,
        Administrador
    }
}
