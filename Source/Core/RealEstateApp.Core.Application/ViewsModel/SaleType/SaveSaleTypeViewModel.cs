using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.SaleType
{
    public sealed class SaveSaleTypeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de venta es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(250, ErrorMessage = "La descripción no puede exceder los 250 caracteres")]
        public string? Description { get; set; }
    }
}
