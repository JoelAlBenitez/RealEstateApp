namespace RealEstateApp.Core.Application.ViewsModel.Property
{
    public sealed class AgentDashboardViewModel : Common.IPaginatedViewModel
    {
        public required IReadOnlyCollection<PropertyCardViewModel> Properties { get; set; }
        public int TotalProperties { get; set; }
        public int AvailableProperties { get; set; }
        public int SoldProperties { get; set; }
        
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        int Common.IPaginatedViewModel.CurrentPage => Page;
        int Common.IPaginatedViewModel.TotalPages => TotalPages;
        int Common.IPaginatedViewModel.TotalItems => TotalProperties;
        int Common.IPaginatedViewModel.PageSize => PageSize;
    }
}
