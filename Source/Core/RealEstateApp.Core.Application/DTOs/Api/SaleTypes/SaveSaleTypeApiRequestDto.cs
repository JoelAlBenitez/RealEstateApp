using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.DTOs.Api.SaleTypes
{
   
    public sealed class SaveSaleTypeApiRequestDto
    {
        [Required(ErrorMessage = "El nombre del tipo de venta es requerido.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es requerida.")]
        public string Description { get; set; } = null!;
    }
}
