namespace RealEstateApp.Core.Application.DTOs.Dashboard
{
    // DTO consolidante de las estadísticas generales del sistema para el Dashboard del Administrador
    public sealed record DashboardDto
    {
        public required int AvailableProperties { get; set; }
        public required int SoldProperties { get; set; }
        public required int ActiveAgents { get; set; }
        public required int InactiveAgents { get; set; }
        public required int ActiveClients { get; set; }
        public required int InactiveClients { get; set; }
        public required int ActiveDevelopers { get; set; }
        public required int InactiveDevelopers { get; set; }
    }
}
