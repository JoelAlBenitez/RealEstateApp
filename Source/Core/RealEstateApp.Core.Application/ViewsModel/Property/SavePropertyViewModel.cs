using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Property
{
    public class SavePropertyViewModel
    {
        public int Id { get; set; }
        public string? Code { get; set; }

        [Required(ErrorMessage = "El precio de la propiedad es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor mayor a cero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La descripción de la propiedad es requerida.")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres.")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "El tamaño de la propiedad es requerido.")]
        [Range(1, double.MaxValue, ErrorMessage = "El tamaño debe ser un valor mayor a cero.")]
        public decimal Size { get; set; }

        [Required(ErrorMessage = "La cantidad de habitaciones es requerida.")]
        [Range(0, 100, ErrorMessage = "Ingrese una cantidad de habitaciones válida (0-100).")]
        public int Bedrooms { get; set; }

        [Required(ErrorMessage = "La cantidad de baños es requerida.")]
        [Range(0, 100, ErrorMessage = "Ingrese una cantidad de baños válida (0-100).")]
        public int Bathrooms { get; set; }

        public string? AgentId { get; set; }
        public PropertyState Status { get; set; } = PropertyState.Available;

        [Required(ErrorMessage = "Debe seleccionar un tipo de propiedad.")]
        public int PropertyTypeId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de venta.")]
        public int SaleTypeId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos una mejora para la propiedad.")]
        public List<int> ImprovementIds { get; set; } = new();

        public List<IFormFile>? ImageFiles { get; set; }
        public List<string>? ExistingImageUrls { get; set; }
    }
}
