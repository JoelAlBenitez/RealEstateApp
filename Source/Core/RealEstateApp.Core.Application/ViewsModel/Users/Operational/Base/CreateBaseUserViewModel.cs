using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.Base
{
    public  class CreateBaseUserViewModel : IValidatableObject
    {

        [Required(ErrorMessage = "Debe ingresar un nombre válido.")]
        [StringLength(50, ErrorMessage = "El nombre debe tener entre 1 y 50 caracteres.", MinimumLength = 1)]
        [Display(Name = "Nombre")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Debe ingresar un apellido válido.")]
        [StringLength(50, ErrorMessage = "El apellido debe tener entre 1 y 50 caracteres.", MinimumLength = 1)]
        [Display(Name = "Apellido")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
        [StringLength(254, ErrorMessage = "El correo electrónico debe tener entre 12 y 254 caracteres.", MinimumLength = 12)]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
        [Display(Name = "Correo electrónico")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida y debe tener al menos 8 caracteres, incluyendo una mayúscula, una minúscula y un carácter especial.")]
        [StringLength(100, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "La confirmación de contraseña es requerida.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "La contraseña y la confirmación de contraseña no coinciden.")]
        [Display(Name = "Confirmación de contraseña")]
        public required string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [DataType(DataType.Text)]
        [StringLength(25, ErrorMessage = "El nombre de usuario debe tener entre 5 y 25 caracteres.", MinimumLength = 5)]
        [Display(Name = "Nombre de usuario")]

        public required string NameUser { get; set; }

        [Required(ErrorMessage = "Debe indicar un tipo de usuario válido.")]
        [Display(Name = "Tipo de usuario")]
        public required int TypeUser { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool password = !string.IsNullOrWhiteSpace(Password);
            bool confirm = !string.IsNullOrWhiteSpace(ConfirmPassword);
            if (password != confirm)
            {
                yield return new ValidationResult("La contraseña y la confirmación de contraseña no coinciden.",
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
