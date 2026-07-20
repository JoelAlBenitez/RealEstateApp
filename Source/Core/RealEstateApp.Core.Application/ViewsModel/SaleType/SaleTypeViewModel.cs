namespace RealEstateApp.Core.Application.ViewsModel.SaleType
{
    public sealed class SaleTypeViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int PropertyCount { get; set; }
    }
}
