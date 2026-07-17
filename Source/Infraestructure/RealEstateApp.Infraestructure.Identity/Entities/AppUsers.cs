using Microsoft.AspNetCore.Identity;

namespace RealEstateApp.Infraestructure.Identity.Entities
{
    public sealed class AppUsers : IdentityUser
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string ProfileImg {  get; set; }
        public string? IDCard {  get; set; }
        public required bool IsActive {  get; set; }
        public DateTimeOffset? BlockedEmailSending { get; set; }
        public required DateTimeOffset CreateAt { get; set; }
    }
}
