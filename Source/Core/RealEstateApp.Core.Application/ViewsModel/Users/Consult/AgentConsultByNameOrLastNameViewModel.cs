using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Consult
{
    public sealed class AgentConsultByNameOrLastNameViewModel
    {
        [Required(ErrorMessage = "Debe ingresar un nombre valido.")]
        [StringLength(100, ErrorMessage = "Ingrese un nombre o apellido no mayor a 100 caracteres.")]
         public required string Name { get; set; }
    }
}
