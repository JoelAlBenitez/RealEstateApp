using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser
{
    public sealed class ResendActivationEmailViewModel
    {
        [Required(ErrorMessage = "Debe ingresar un nombre de usuario válido.")]
        [StringLength(25, ErrorMessage = "El nombre de usuario debe tener entre 5 y 25 caracteres.", MinimumLength = 5)]
        [Display(Name = "Nombre de usuario")]

        public required string UserName { get; set; }

    }
}
