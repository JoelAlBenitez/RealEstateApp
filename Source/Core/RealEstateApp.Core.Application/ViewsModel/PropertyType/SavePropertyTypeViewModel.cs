using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.PropertyType
{
    public sealed class SavePropertyTypeViewModel
    {
        public int? Id { get; set; }
        
        [Required(ErrorMessage = "Debe completar todos los campos requeridos.")]
        [Display(Name = "Nombre del tipo de propiedad")]
        public required string Name { get; set; }
        
        [Required(ErrorMessage = "Debe completar todos los campos requeridos.")]
        [Display(Name = "Descripción")]
        public required string Description { get; set; }
    }
}
