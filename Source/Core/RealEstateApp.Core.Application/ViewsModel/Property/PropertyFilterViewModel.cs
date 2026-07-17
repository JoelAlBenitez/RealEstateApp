using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Property
{
    public sealed class PropertyFilterViewModel : IValidatableObject
    {
        [Range(0, double.MaxValue, ErrorMessage = "El precio mínimo no puede ser menor a 0.")]
        public decimal? MinPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio máximo no puede ser menor a 0.")]
        public decimal? MaxPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad de habitaciones no puede ser menor a 0.")]
        public int? Bedrooms { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad de baños no puede ser menor a 0.")]
        public int? Bathrooms { get; set; }

        public int? IdTypeProperty { get; set; }
        public List<TypePropertyViewModel>? TypePropery { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice.Value > MaxPrice.Value)
            {
                yield return new ValidationResult(
                    "El precio mínimo no puede ser mayor que el precio máximo.",
                    new[] { nameof(MinPrice), nameof(MaxPrice) });
            }
        }
    }
}
