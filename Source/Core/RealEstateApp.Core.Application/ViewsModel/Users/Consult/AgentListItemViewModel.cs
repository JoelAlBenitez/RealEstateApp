using RealEstateApp.Core.Application.ViewsModel.Users.Consult.Base;

namespace RealEstateApp.Core.Application.ViewsModel.Users.Consult
{
    // ViewModel para el listado de agentes de la administración
    public sealed class AgentListItemViewModel : UserConsultViewModel
    {
        public required string Email { get; set; }
        public required int Properties { get; set; }
        public required bool IsActive { get; set; }
    }
}
