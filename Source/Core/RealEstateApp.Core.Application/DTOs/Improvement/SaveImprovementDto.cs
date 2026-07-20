namespace RealEstateApp.Core.Application.DTOs.Improvement
{
    public sealed record SaveImprovementDto
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
