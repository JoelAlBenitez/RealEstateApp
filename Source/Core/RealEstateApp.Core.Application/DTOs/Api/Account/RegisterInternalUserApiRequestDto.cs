using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.DTOs.Api.Account
{
    public sealed class RegisterInternalUserApiRequestDto
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El apellido es requerido.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "La cédula es requerida.")]
        public required string IDCard { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "La confirmación de contraseña es requerida.")]
        [Compare(nameof(Password), ErrorMessage = "La contraseña y la confirmación de contraseña no coinciden.")]
        public required string ConfirmPassword { get; set; }
    }
}
