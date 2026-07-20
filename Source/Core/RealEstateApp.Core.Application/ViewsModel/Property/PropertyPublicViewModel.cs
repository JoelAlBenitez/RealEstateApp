namespace RealEstateApp.Core.Application.ViewsModel.Property
{
    public sealed class PropertyPublicViewModel
    {
        public required int Id { get; set; }
        public required string FirtsImage { get; set; }
        public required string Code { get; set; }
        public required string TypeProperty { get; set; }
        public required string TypeSale { get; set; }
        public required decimal Price { get; set; }
        public required int NumberOfBedrooms { get; set; }
        public required int NumberOfBathrooms { get; set; }
        public required double Size { get; set; }
    }
}
