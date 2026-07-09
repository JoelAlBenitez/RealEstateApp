namespace RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser.Base
{
    public abstract record BaseGetUserDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
    }
}
