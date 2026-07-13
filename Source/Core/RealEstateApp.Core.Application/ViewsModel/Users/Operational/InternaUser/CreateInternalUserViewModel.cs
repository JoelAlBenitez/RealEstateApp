using RealEstateApp.Core.Application.ViewsModel.Users.Operational.Base;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.InternaUser
{
    public sealed class CreateInternalUserViewModel : CreateBaseUserViewModel
    {

        [Required(ErrorMessage = "Debe  ingresar una cedula valida")]
        [StringLength(11, ErrorMessage = "Debe ingresar una cedula de 11 digitos exactos sin guiones",  MinimumLength =11)]
        [Display(Name = "Cedula")]

        public required string IDCard { get; set; }
    }
}
