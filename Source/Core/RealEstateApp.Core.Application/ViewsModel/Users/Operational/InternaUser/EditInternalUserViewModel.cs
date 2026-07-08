using RealEstateApp.Core.Application.ViewsModel.Users.Operational.Base;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.InternaUser{
    public sealed class EditInternalUserViewModel : EditBaseUserViewModel, IValidatableObject
    {
        [Required(ErrorMessage = "Debe  ingresar un numero de identitdad valido sin guiones")]
        [StringLength(11, ErrorMessage = "El numero de identidad debe tener 11 digitos sin guiines", MinimumLength = 11)]
        [Display(Name = "Cedula")]
        public required string IdCard { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre de usuario valido")]
        [StringLength(25, ErrorMessage = "El nombre de usuario no debe superar los 25 caracteres ni tener menos de 5", MinimumLength = 5)]
        [Display(Name = "Nombre de usuario")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Debe ingresar un correo electronico valido")]
        [StringLength(254, ErrorMessage = "El correo electronico no puede tener una longitud mayor a 254 caracteres ni ser menor a 12 caracteres", MinimumLength = 12)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electronico")]
        public required string Email  { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword),ErrorMessage = "Las contraseñas deben coincidir")]
        public string? ConfirmNewPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool password = !string.IsNullOrWhiteSpace(NewPassword);
            bool confirm = !string.IsNullOrWhiteSpace(ConfirmNewPassword);
            if(password != confirm)
            {
                yield return new ValidationResult("La nueva password y su contraseña deben coincidir",
                    new[]
                    {
                        nameof(NewPassword),
                        nameof(ConfirmNewPassword)
                    }
                   );
            }
        }
    }
}
