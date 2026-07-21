namespace RealEstateApp.Core.Application.ViewsModel.Common
{
    public sealed class ErrorPageViewModel
    {
        public required int Code { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
    }
}
