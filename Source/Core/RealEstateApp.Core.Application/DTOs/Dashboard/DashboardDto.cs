namespace RealEstateApp.Core.Application.DTOs.Dashboard
{
    public sealed record DashboardDto
    {
        public required int AvailableProperties { get; set; }
        public required int SoldProperties { get; set; }
        public required int AgentsCount { get; set; }
        public required int ClientsCount { get; set; }
        public required int DevelopersCount { get; set; }
        public required bool ShowingActive { get; set; }
    }
}
