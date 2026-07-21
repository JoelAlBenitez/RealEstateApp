using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.DTOs.Api.PropertyTypes
{
   
    public sealed class SavePropertyTypeApiRequestDto
    {
        [Required(ErrorMessage = "El nombre del tipo de propiedad es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es requerida.")]
        [StringLength(250, ErrorMessage = "La descripción no puede exceder los 250 caracteres.")]
        public string Description { get; set; } = null!;
    }
}
