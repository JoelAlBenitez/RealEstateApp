using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.DTOs.Api.PropertyTypes
{
   
    public sealed class SavePropertyTypeApiRequestDto
    {
        [Required(ErrorMessage = "El nombre del tipo de propiedad es requerido.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es requerida.")]
        public string Description { get; set; } = null!;
    }
}
