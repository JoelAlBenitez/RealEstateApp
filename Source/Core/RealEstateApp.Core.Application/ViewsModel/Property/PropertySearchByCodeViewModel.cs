using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Property
{
    public sealed class PropertySearchByCodeViewModel 
    {
        [Required(ErrorMessage = "Favor ingresar un código para la propiedad a buscar.")]
        [StringLength(50, ErrorMessage = "Ingrese un código valido para proceder con la consulta.")]
        public required string Code { get; set; }
    }
}
