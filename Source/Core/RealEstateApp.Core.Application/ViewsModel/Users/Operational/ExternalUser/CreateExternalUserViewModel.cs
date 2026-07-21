using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.Base;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser
{
    public sealed class CreateExternalUserViewModel : CreateBaseUserViewModel
    {

        [Required(ErrorMessage = "El número de teléfono es requerido y debe cumplir con el formato de República Dominicana.")]
        [StringLength(10, ErrorMessage = "El número de teléfono debe tener exactamente 10 dígitos, sin guiones.", MinimumLength = 10)]
        [Phone(ErrorMessage = "Debe ingresar un número de teléfono válido.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Número de teléfono")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "El archivo seleccionado no tiene un formato de imagen válido.")]
        [Display(Name = "Imagen de perfil")]
        public required IFormFile ProfileImg { get; set; }
    }
}
