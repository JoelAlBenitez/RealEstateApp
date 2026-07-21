using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser
{
    public sealed class ResetPasswordViewModel  : IValidatableObject
    {
        [Required(ErrorMessage = "Esta petición no es válida para este usuario. Intente de nuevo.")]
        public required string Id { get; set; }

        [Required(ErrorMessage = "Enlace inválido. La acción no pudo ser procesada.")]
        public required string Token { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida. Ingrese una de al menos 8 caracteres.")]
        [StringLength(100, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.", MinimumLength = 8)]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public required string NewPassword { get; set; }


        [Required(ErrorMessage = "La confirmación de contraseña es requerida.")]
        [StringLength(100, ErrorMessage = "La confirmación de contraseña debe tener al menos 8 caracteres.", MinimumLength = 8)]
        [Display(Name = "Confirmación de contraseña")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "La contraseña y la confirmación de contraseña no coinciden.")]
        public required string ConfirmNewPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool password = !string.IsNullOrWhiteSpace(NewPassword);
            bool confirm = !string.IsNullOrWhiteSpace(ConfirmNewPassword);
            if (password != confirm)
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
