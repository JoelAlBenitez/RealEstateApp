using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.DTOs.Api.Improvements
{
  
    public sealed class SaveImprovementApiRequestDto
    {
        [Required(ErrorMessage = "El nombre de la mejora es requerido.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es requerida.")]
        public string Description { get; set; } = null!;
    }
}
