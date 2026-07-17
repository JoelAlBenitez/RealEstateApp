namespace RealEstateApp.Core.Application.ViewsModel.Improvement
{
    public sealed class ImprovementViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int PropertyCount { get; set; }
    }
}
