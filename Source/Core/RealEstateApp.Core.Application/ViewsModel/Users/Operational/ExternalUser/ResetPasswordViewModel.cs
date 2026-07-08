using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser
{
    public sealed class ResetPasswordViewModel  : IValidatableObject
    {
        [Required(ErrorMessage = "Error esta peticion no es valida para este usuario, favor intente de nuevo")]
        public required string Id { get; set; }

        [Required(ErrorMessage = "Error link invalido no puede ser procesada la accion")]
        public required string Token { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida favor ingrese una de al menos 8 caracteres")]
        [StringLength(100, ErrorMessage = "La contraseña debe contener minimo 8 caracteres", MinimumLength = 8)]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }


        [Required(ErrorMessage = "La conformacion de contraseña es requerida favor ingrese el valor pertinente")]
        [StringLength(100, ErrorMessage = "La confirmacion de contraseña debe contener minimo 8 caracteres", MinimumLength = 8)]
        [Display(Name = "Confirmacion de contraseña")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas deben coincidir.")]
        public required string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool password = !string.IsNullOrWhiteSpace(Password);
            bool confirm = !string.IsNullOrWhiteSpace(ConfirmPassword);
            if (password != confirm)
            {
                yield return new ValidationResult("La nueva password y su contraseña deben coincidir",
                    new[]
                    {
                        nameof(Password),
                        nameof(ConfirmPassword)
                    }
                   );
            }
        }
    }
}
