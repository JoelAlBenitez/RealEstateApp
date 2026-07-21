namespace RealEstateApp.Core.Application.DTOs.Dashboard
{
    public sealed record DashboardDto
    {
        public required int AvailableProperties { get; set; }
        public required int SoldProperties { get; set; }
        public required int ActiveAgentsCount { get; set; }
        public required int InactiveAgentsCount { get; set; }
        public required int ActiveClientsCount { get; set; }
        public required int InactiveClientsCount { get; set; }
        public required int ActiveDevelopersCount { get; set; }
        public required int InactiveDevelopersCount { get; set; }
    }
}
