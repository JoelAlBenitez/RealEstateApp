namespace RealEstateApp.Core.Application.DTOs.FavoriteProperty
{
    public sealed class SaveFavoritePropertyDto
    {
        public int Id { get; set; }
        public string? CustomerId { get; set; }
        public int PropertyId { get; set; }
    }
}
