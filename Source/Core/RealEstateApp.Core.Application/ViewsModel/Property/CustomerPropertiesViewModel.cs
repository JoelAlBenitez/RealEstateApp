using System.Collections.Generic;

namespace RealEstateApp.Core.Application.ViewsModel.Property
{
    public sealed class CustomerPropertiesViewModel : Common.IPaginatedViewModel
    {
        public required IReadOnlyCollection<PropertyCardViewModel> Properties { get; set; }
        public PropertyFilterViewModel? Filter { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        int Common.IPaginatedViewModel.CurrentPage => Page;
        int Common.IPaginatedViewModel.TotalPages => TotalPages;
        int Common.IPaginatedViewModel.TotalItems => TotalItems;
        int Common.IPaginatedViewModel.PageSize => PageSize;
    }
}
