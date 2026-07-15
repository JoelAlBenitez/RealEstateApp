using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Infraestructure.Identity.Context;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Infraestructure.Identity.Errors;
using RealEstateApp.Infrastructure.Identity.EmailProvider;

namespace RealEstateApp.Infraestructure.Identity.RegistrationAndConfiguration
{
    public static class RegistrationAndConfigurationsIdentityWebApp
    {

        #region Integration and configuration Identity Framework web app
        public static void AddWebAppServicesIdentity(this IServiceCollection services,
            IConfiguration configuration)
        {

            GeneralConfiguration.AddGeneralConfiguration(services, configuration);

            #region Configurations Options identity

            services.Configure<IdentityOptions>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;

                //configurations password 
                opt.Password.RequireDigit = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 8;

                //...
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            });
            #endregion

            #region Authentication
            //authentication

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;

            }).AddCookie(IdentityConstants.ApplicationScheme, opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                opt.SlidingExpiration = true;
                opt.Cookie.HttpOnly = true;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.Cookie.SameSite = SameSiteMode.Lax;
                opt.AccessDeniedPath = "/Account/AccessDeniged";
                opt.LoginPath = "/Account/Login";
                opt.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Redirect("/Account/Login?expired=true");
                    return Task.CompletedTask;
                };
                opt.Events.OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync;
            });
            #endregion

            services.Configure<SecurityStampValidatorOptions>(opt =>
            {
                opt.ValidationInterval = TimeSpan.FromMinutes(5);
            });
            services.AddScoped<ISecurityStampValidator, SecurityStampValidator<AppUsers>>();
            // proveedor generarl para password

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(1); //este es el general de los restablecimientos de password |  
            });

            //proveedor especifico extendido para los emails de actividaciones

            //add elements identity
            #region IdentityCore
            services.AddIdentityCore<AppUsers>(opt =>
            {
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
                opt.Tokens.EmailConfirmationTokenProvider = "EmailConfirmProviderOptions";

            })
              .AddRoles<IdentityRole>()
              .AddSignInManager()
              .AddErrorDescriber<SpanishIdentityErrorDescriber>()
              .AddEntityFrameworkStores<DbContextIdentityRealStateApp>()
              .AddTokenProvider<EmailConfirmProviderTokensConfigurations>("EmailConfirmProviderOptions")
              .AddTokenProvider<DataProtectorTokenProvider<AppUsers>>(TokenOptions.DefaultProvider);
            #endregion

            //
        }

        #endregion

    }
}

 