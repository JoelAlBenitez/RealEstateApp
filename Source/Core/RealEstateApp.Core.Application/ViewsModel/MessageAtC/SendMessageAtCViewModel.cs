using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewsModel.MessageAtC
{
    public class SendMessageAtCViewModel
    {
        public int PropertyId { get; set; }
        public string? AgentId { get; set; }

        [Required(ErrorMessage = "Debe escribir un mensaje antes de enviarlo.")]
        [StringLength(500, ErrorMessage = "El mensaje no puede exceder los 500 caracteres.")]
        public required string Content { get; set; }
    }
}
