using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.DTOs.Api.Agents
{
    public sealed class ChangeAgentStatusApiDto
    {
        [Required(ErrorMessage = "El estado enviado es requerido.")]
        public required bool Status { get; set; }
    }
}
