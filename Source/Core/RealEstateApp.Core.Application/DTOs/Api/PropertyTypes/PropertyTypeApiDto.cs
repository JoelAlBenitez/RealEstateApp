namespace RealEstateApp.Core.Application.DTOs.Api.PropertyTypes
{
    
    public sealed class PropertyTypeApiDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
