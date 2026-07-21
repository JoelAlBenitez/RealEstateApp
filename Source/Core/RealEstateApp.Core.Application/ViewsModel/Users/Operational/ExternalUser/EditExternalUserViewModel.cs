using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.Base;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser
{
    public sealed class EditExternalUserViewModel : EditBaseUserViewModel
    {

        [Required(ErrorMessage = "El número de teléfono es requerido y debe cumplir con el formato de República Dominicana.")]
        [StringLength(10, ErrorMessage = "El número de teléfono debe tener exactamente 10 dígitos, sin guiones.", MinimumLength = 10)]
        [Phone(ErrorMessage = "Debe ingresar un número de teléfono válido.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Número de teléfono")]
        public required string PhoneNumber { get; set; }
        public required string ProfileImgCurrent {  get; set; }
        public IFormFile? NewProfileImage { get; set; }
      }
}
