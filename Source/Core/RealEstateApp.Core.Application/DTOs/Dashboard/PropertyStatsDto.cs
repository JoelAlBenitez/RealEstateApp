namespace RealEstateApp.Core.Application.DTOs.Dashboard
{
    // DTO que encapsula las estadísticas de las propiedades consumidas de la base de datos
    public sealed record PropertyStatsDto
    {
        public required int AvailableCount { get; set; }
        public required int SoldCount { get; set; }
    }
}
