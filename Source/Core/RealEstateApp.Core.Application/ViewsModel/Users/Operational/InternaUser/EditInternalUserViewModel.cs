using RealEstateApp.Core.Application.ViewsModel.Users.Operational.Base;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.InternaUser{
    public sealed class EditInternalUserViewModel : EditBaseUserViewModel, IValidatableObject
    {
        [Required(ErrorMessage = "Debe ingresar un número de identidad válido, sin guiones.")]
        [StringLength(11, ErrorMessage = "El número de identidad debe tener 11 dígitos, sin guiones.", MinimumLength = 11)]
        [Display(Name = "Cédula")]
        public required string IdCard { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre de usuario válido.")]
        [StringLength(25, ErrorMessage = "El nombre de usuario debe tener entre 5 y 25 caracteres.", MinimumLength = 5)]
        [Display(Name = "Nombre de usuario")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
        [StringLength(254, ErrorMessage = "El correo electrónico debe tener entre 12 y 254 caracteres.", MinimumLength = 12)]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo electrónico")]
        public required string Email  { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmación de contraseña")]
        [Compare(nameof(NewPassword),ErrorMessage = "La contraseña y la confirmación de contraseña no coinciden.")]
        public string? ConfirmNewPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool password = !string.IsNullOrWhiteSpace(NewPassword);
            bool confirm = !string.IsNullOrWhiteSpace(ConfirmNewPassword);
            if(password != confirm)
            {
                yield return new ValidationResult("La contraseña y la confirmación de contraseña no coinciden.",
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
