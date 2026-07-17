namespace RealEstateApp.Core.Application.ViewsModel.Users.Consult
{
    public sealed class AgentsHomeViewModel
    {
        public required IReadOnlyCollection<AgentViewModel> Agents { get; set; }
        public required AgentConsultByNameOrLastNameViewModel Consult { get; set; }
    }
}
