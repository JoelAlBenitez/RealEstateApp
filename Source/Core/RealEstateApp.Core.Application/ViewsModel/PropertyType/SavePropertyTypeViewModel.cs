using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.PropertyType
{
    public sealed class SavePropertyTypeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de propiedad es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(250, ErrorMessage = "La descripción no puede exceder los 250 caracteres")]
        public string Description { get; set; }
    }
}
