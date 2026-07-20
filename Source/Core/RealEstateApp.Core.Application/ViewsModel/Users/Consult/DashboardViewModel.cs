namespace RealEstateApp.Core.Application.ViewsModel.Users.Consult
{
    // Indicadores generales del Home del Administrador
    public sealed class DashboardViewModel
    {
        public required int AvailableProperties { get; set; }
        public required int SoldProperties { get; set; }
        public required int ActiveAgents { get; set; }
        public required int InactiveAgents { get; set; }
        public required int ActiveClients { get; set; }
        public required int InactiveClients { get; set; }
        public required int ActiveDevelopers { get; set; }
        public required int InactiveDevelopers { get; set; }
        public bool ShowActive { get; set; } = true;
    }
}
