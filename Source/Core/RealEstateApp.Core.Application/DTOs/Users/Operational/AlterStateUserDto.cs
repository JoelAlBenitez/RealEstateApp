namespace RealEstateApp.Core.Application.DTOs.Users.Operational
{
    public sealed class AlterStateUserDto
    {
        public required string Id { get; set; }
        public required bool State {  get; set; }
    }
}
