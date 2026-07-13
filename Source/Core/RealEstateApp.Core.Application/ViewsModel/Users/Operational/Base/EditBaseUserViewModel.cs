using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Operational.Base
{
    public  class EditBaseUserViewModel
    {
        [Required(ErrorMessage = "Usuario no especificado, favor intente de nuevo")]
        public required string Id { get; set; }

        [Required(ErrorMessage = "Debee ingresar un nombre valido")]
        [StringLength(50, ErrorMessage = "Debe ingresar un nombre mayor a 0 caracteres y menor a 40", MinimumLength = 1)]
        [Display(Name = "Nombre")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Debe ingresar un apellido valido")]
        [StringLength(50, ErrorMessage = "Debe ingresar un apellido mayor a 0 caracteres y menor a 40", MinimumLength = 1)]
        [Display(Name = "Apellido")]
        public required string LastName { get; set; }

       
    }
}
