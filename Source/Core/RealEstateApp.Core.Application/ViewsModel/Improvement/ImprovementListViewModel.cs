namespace RealEstateApp.Core.Application.ViewsModel.Improvement
{
    public sealed class ImprovementListViewModel
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int PropertyCount { get; set; }
    }
}
