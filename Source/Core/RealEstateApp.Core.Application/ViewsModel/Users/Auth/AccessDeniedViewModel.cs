namespace RealEstateApp.Core.Application.ViewsModel.Users.Auth
{
    public sealed class AccessDeniedViewModel
    {
        public required string Message { get; set; }
        public required string HomeController { get; set; }
        public required string HomeAction { get; set; }
    }
}
