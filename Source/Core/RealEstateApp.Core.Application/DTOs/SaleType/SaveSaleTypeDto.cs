namespace RealEstateApp.Core.Application.DTOs.SaleType
{
    public sealed record SaveSaleTypeDto
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
