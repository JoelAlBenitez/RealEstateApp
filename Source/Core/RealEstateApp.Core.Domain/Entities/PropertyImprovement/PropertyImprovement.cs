namespace RealEstateApp.Core.Domain.Entities
{
    public class PropertyImprovement
    {
        public int PropertyId { get; set; }
        public int ImprovementId { get; set; }

        // Navigation Properties
        public Property? Property { get; set; }
        public Improvement? Improvement { get; set; }
    }
}
