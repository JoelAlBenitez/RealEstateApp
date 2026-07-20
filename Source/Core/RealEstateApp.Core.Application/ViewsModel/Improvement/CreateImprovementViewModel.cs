using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Improvement
{
    // Supuesto de Diseño: El documento funcional no especifica límites de longitud para estos campos.
    public sealed class CreateImprovementViewModel
    {
        [Required(ErrorMessage = "Debe completar todos los campos requeridos.")]
        [StringLength(100, ErrorMessage = "El nombre no debe superar los 100 caracteres.")]
        [Display(Name = "Nombre de la mejora")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Debe completar todos los campos requeridos.")]
        [StringLength(250, ErrorMessage = "La descripción no debe superar los 250 caracteres.")]
        [Display(Name = "Descripción")]
        public required string Description { get; set; }
    }
}
