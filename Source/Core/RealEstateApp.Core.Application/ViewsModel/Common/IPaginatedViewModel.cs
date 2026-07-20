namespace RealEstateApp.Core.Application.ViewsModel.Common
{
    public interface IPaginatedViewModel
    {
        int CurrentPage { get; }
        int TotalPages { get; }
        int TotalItems { get; }
        int PageSize { get; }
    }
}
