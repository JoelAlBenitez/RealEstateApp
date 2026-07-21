using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.Base
{
    public  class EditBaseUserViewModel
    {
        [Required(ErrorMessage = "Usuario no especificado. Intente de nuevo.")]
        public required string Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre válido.")]
        [StringLength(50, ErrorMessage = "El nombre debe tener entre 1 y 50 caracteres.", MinimumLength = 1)]
        [Display(Name = "Nombre")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Debe ingresar un apellido válido.")]
        [StringLength(50, ErrorMessage = "El apellido debe tener entre 1 y 50 caracteres.", MinimumLength = 1)]
        [Display(Name = "Apellido")]
        public required string LastName { get; set; }


    }
}
