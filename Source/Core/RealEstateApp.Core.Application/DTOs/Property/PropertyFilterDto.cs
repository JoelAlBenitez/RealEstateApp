namespace RealEstateApp.Core.Application.DTOs.Property
{
    public sealed class PropertyFilterDto
    {
        // public int? PropertyTypeId { get; set; }
        public decimal ? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
    }
}
