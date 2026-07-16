namespace RealEstateApp.Core.Application.ViewsModel.Property
{
    public sealed class PropertyFilterViewModel
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? IdTypeProperty { get; set; }
        public List<TypePropertyViewModel>? TypePropery { get; set; }
    }
}
