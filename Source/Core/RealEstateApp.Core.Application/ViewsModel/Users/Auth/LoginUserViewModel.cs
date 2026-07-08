using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Auth
{
    public sealed class LoginUserViewModel
    {

        [Required(ErrorMessage = "Ingrese su correo electronico o nombre de usuario.")]
        [StringLength(254, ErrorMessage = "El valor ingresado supera la longitud maxima de caracteres")]
        public required string EmailOrNameUser { get; set; }

        [Required(ErrorMessage = "Ingrese una contraseña valida")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
