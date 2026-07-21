namespace RealEstateApp.Core.Application.ViewsModel.Property
{
    public sealed class PropertyDetailPublicConsult
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
        public decimal Size { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public required string TypePropertyName { get; set; }
        public required List<string> ImgUrls { get; set; }
        public required string TypeSalesName {  get; set; }
        public required List<string> Improvents { get; set; }
        public required string AgentName { get; set; }
        public required string AgentPhone { get; set; }
        public required string AgentEmail { get; set; }
        public required string AgentImg {  get; set; }  
    }
}
