using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.Base;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser
{
    public sealed class CreateExternalUserViewModel : CreateBaseUserViewModel
    {

        [Required(ErrorMessage = "El numero de telefono ingresado debe ser valido y cumplir con el formato de republica dominicana")]
        [StringLength(10, ErrorMessage = "El numero de telefono debe tener exactamente 10 caracteres sin guion", MinimumLength =10)]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numero telefonico")]

        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Debe ingresar una imagen de perfil valida para ser procesada (PNG, JPEG, JPG)")]
        [Display(Name = "Imagen de perfil")]

        public required IFormFile ProfileImg { get; set; }
    }
}
