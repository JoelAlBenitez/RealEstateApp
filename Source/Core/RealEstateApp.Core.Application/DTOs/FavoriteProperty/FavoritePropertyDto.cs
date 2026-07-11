using RealEstateApp.Core.Application.DTOs.Property;

namespace RealEstateApp.Core.Application.DTOs.FavoriteProperty
{
    public sealed class FavoritePropertyDto
    {
        public int Id { get; set; }
        public required string CustomerId { get; set; }
        public int PropertyId { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public PropertyDto? Property { get; set; }
    }
}
