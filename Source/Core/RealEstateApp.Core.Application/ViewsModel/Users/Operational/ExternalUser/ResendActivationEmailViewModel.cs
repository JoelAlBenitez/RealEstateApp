using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser
{
    public sealed class ResendActivationEmailViewModel
    {
        [Required(ErrorMessage = "Debe ingresar un nombre de usuario valido")]
        [StringLength(25, ErrorMessage = "Nombre de usuario de longitud invalida", MinimumLength = 5)]
        [Display(Name = "Nombre de usuario")]

        public required string UserName { get; set; }

    }
}
