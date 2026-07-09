namespace RealEstateApp.Core.Application.DTOs.Users.BaseError
{
    public  class BaseErrors
    {
        public required List<string> Errors { get; set; }
        public required bool HasError { get; set; }
    }
}
