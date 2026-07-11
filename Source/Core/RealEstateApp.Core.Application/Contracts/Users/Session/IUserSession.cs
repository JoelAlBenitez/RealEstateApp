namespace RealEstateApp.Core.Application.DTOs.Users.Auth.Session
{
    public interface IUserSession
    {
        string GetUserName();
        string GetIdCurrentUser();
        List<string> GetRolesCurrentUser();
    }
}
