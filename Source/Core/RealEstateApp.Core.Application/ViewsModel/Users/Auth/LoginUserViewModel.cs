using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Auth
{
    public sealed class LoginUserViewModel
    {

        [Required(ErrorMessage = "Debe ingresar su correo o nombre de usuario y contraseña.")]
        [StringLength(254, ErrorMessage = "El valor ingresado supera la longitud máxima de caracteres.")]
        [Display(Name = "Correo o nombre de usuario")]
        public required string EmailOrNameUser { get; set; }

        [Required(ErrorMessage = "Debe ingresar su correo o nombre de usuario y contraseña.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public required string Password { get; set; }
    }
}
