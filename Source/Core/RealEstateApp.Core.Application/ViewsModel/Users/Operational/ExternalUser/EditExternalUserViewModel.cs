using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.Base;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser
{
    public sealed class EditExternalUserViewModel : EditBaseUserViewModel
    {

        [Required(ErrorMessage = "El numero de telefono es requerido el mismo debe tener un formato de Republica Dominicana")]
        [StringLength(10, ErrorMessage = "El numero de telefono debe tener exactamente 10 caracteres sin guion", MinimumLength = 10)]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public required string PhoneNumber { get; set; }
        public required string ProfileImgCurrent {  get; set; }
        public IFormFile? NewProfileImage { get; set; }
      }
}
