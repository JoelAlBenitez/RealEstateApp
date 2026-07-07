namespace RealEstateApp.Core.Application.DTOs.Users.Operational.Base
{
    public abstract record RegisterUserDto
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string ProfileImg {  get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string ConfirmPassword { get; set; }
        public required int TypeUser { get; set; }
    }
}
