using RealEstateApp.Core.Application.ViewsModel.Users.Operational.Base;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.InternaUser
{
    public sealed class CreateInternalUserViewModel : CreateBaseUserViewModel
    {

        [Required(ErrorMessage = "Debe ingresar una cédula válida.")]
        [StringLength(11, ErrorMessage = "La cédula debe tener 11 dígitos exactos, sin guiones.",  MinimumLength =11)]
        [Display(Name = "Cédula")]

        public required string IDCard { get; set; }
    }
}
