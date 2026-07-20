namespace RealEstateApp.Core.Application.ViewsModel.SaleType
{
    public sealed class SaleTypeListViewModel
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int PropertyCount { get; set; }
    }
}
