namespace RealEstateApp.Core.Application.DTOs.Api.SaleTypes
{
    
    public sealed class SaleTypeApiDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
