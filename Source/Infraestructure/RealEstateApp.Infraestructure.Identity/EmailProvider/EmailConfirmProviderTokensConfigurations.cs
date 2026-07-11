using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Infrastructure.Identity.EmailProvider.Options;

namespace RealEstateApp.Infrastructure.Identity.EmailProvider
{
    public class EmailConfirmProviderTokensConfigurations :
        DataProtectorTokenProvider<AppUsers>
    {
        public EmailConfirmProviderTokensConfigurations(
            IDataProtectionProvider dataProtectionProvider, 
            IOptions<EmailConfirmProviderOptions> options,
            ILogger<DataProtectorTokenProvider<AppUsers>> logger) 
            : base(dataProtectionProvider, options, logger)
        {
        }
    }
}
