namespace RealEstateApp.Core.Application.DTOs.Users.Operational.Base
{
    public abstract record EditUserDto
    {
         public required string Id { get; set; }
         public required string Name { get; set; }
         public required string LastName { get; set; }
        
    }
}
