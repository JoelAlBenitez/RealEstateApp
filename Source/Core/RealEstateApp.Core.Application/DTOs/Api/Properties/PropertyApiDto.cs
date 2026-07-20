namespace RealEstateApp.Core.Application.DTOs.Api.Properties
{
    public sealed class PropertyApiDto
    {
        public int Id { get; set; }
        public required string Code { get; set; }

        // Nombres de catálogos (tipo de propiedad, tipo de venta y mejoras),
        // ya integrados desde los mantenimientos del administrador.
        public string? PropertyType { get; set; }
        public string? SaleType { get; set; }
        public List<string> Improvements { get; set; } = new();

        public decimal Price { get; set; }
        public double Size { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public required string Description { get; set; }

        public string? AgentName { get; set; }
        public required string AgentId { get; set; }

        public required string Status { get; set; }
    }
}
