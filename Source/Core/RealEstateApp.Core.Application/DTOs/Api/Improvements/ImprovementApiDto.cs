namespace RealEstateApp.Core.Application.DTOs.Api.Improvements
{
    
    public sealed class ImprovementApiDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
