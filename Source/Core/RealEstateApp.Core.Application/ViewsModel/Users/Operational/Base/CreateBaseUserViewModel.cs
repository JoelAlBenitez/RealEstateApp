using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.Base
{
    public  class CreateBaseUserViewModel : IValidatableObject
    {

        [Required(ErrorMessage = "Debee ingresar un nombre valido")]
        [StringLength(40, ErrorMessage = "Debe ingresar un nombre mayor a 0 caracteres y menor a 40", MinimumLength = 1)]
        [Display(Name = "Nombre")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Debe ingresar un apellido valido")]
        [StringLength(40, ErrorMessage = "Debe ingresar un apellido mayor a 0 caracteres y menor a 40", MinimumLength = 1)]
        [Display(Name = "Apellido")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Debe ingresar un correo electronico valido")]
        [StringLength(254, ErrorMessage = "El correo electronico no puede tener una longitud mayor a 254 caracteres ni ser menor a 12 caracteres", MinimumLength = 12)]
        [EmailAddress]
        [Display(Name = "Correo Electronico")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida, la misma debe tener minimo 8 caracteres (1 mayuscula, 1 miniscula, un caracter especial)")]
        [StringLength(100, ErrorMessage = "Debe ingresar una contraseña valida de 8 caracteres minimo", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "La confirmacion de contraseña es requerida")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas deben coincidir")]
        [Display(Name = "Confirmacion de contraseña")]
        public required string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "El nombre de usuario requerido debe ingresar un nombre de usuario valido")]
        [DataType(DataType.Text)]
        [StringLength(25, ErrorMessage = "El nombre de usuario no debe ser  menor a 5 caracteres ni mayor a 25 caracteres ", MinimumLength = 5)]
        [Display(Name = "Nombre de usuario")]

        public required string NameUser { get; set; }

        [Required(ErrorMessage = "Debe indicar un tipo de usuario valido")]
        public required int TypeUser { get; set; }

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
