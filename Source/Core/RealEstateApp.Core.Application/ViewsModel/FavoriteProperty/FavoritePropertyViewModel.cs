using RealEstateApp.Core.Application.ViewsModel.Property;

namespace RealEstateApp.Core.Application.ViewsModel.FavoriteProperty
{
    public class FavoritePropertyViewModel
    {
        public int Id { get; set; }
        public required string CustomerId { get; set; }
        public int PropertyId { get; set; }
        public PropertyCardViewModel? Property { get; set; }
    }
}
