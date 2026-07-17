namespace RealEstateApp.Core.Application.ViewsModel.Property
{
    public sealed class HomePropertiesViewModel
    {
        public required IReadOnlyCollection<PropertyPublicViewModel> Properties { get; set; }
        public required PropertyFilterViewModel Filter { get; set; }
        public required PropertySearchByCodeViewModel SearchByCode { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalPages { get; set; } = 1;
        public bool IsFiltered { get; set; }
        public bool IsAgentContext { get; set; }
        public string? AgentId { get; set; }    
        public string? AgentName { get; set; }
    }
}
