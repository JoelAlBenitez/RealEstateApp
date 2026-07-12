using Microsoft.AspNetCore.Identity;

namespace RealEstateApp.Infrastructure.Identity.EmailProvider.Options
{
    public class EmailConfirmProviderOptions : DataProtectionTokenProviderOptions
    {
        public EmailConfirmProviderOptions() {

           
            TokenLifespan = TimeSpan.FromHours(24);
        }
    }

}
