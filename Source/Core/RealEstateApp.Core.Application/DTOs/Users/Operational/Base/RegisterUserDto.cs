namespace RealEstateApp.Core.Application.DTOs.Users.Operational.Base
{
    public  class RegisterUserDto
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string NameUser {  get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string ConfirmPassword { get; set; }
        public required int TypeUser { get; set; }
    }
}
