using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.PropertyType
{
    public sealed class EditPropertyTypeViewModel
    {
        [Required]
        public required int Id { get; set; }

        [Required(ErrorMessage = "Debe completar todos los campos requeridos.")]
        [StringLength(100, ErrorMessage = "El nombre no debe superar los 100 caracteres.")]
        [Display(Name = "Nombre del tipo de propiedad")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Debe completar todos los campos requeridos.")]
        [StringLength(500, ErrorMessage = "La descripción no debe superar los 500 caracteres.")]
        [Display(Name = "Descripción")]
        public required string Description { get; set; }
    }
}
