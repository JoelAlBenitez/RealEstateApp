using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Domain.Settings.JWT;
using RealEstateApp.Infraestructure.Identity.Context;
using RealEstateApp.Infraestructure.Identity.Entities;
using Newtonsoft.Json;
using System.Text;
using RealEstateApp.Core.Application.DTOs.Users.Auth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;


namespace RealEstateApp.Infraestructure.Identity.RegistrationAndConfiguration
{
    public static class RegistrationAndConfigurationsIdentityWebApi
    {
        #region Integration and configuration Identity framework web api
        public static void AddWebApiServicesIdentity(this IServiceCollection services,
            IConfiguration configuration
            )
        {

            GeneralConfiguration.AddGeneralConfiguration(services, configuration);
            #region Configurations
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            #endregion
            #region Configurations Options identity

            services.Configure<IdentityOptions>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
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

            #region IdentityCore
            services.AddIdentityCore<AppUsers>(opt =>
            {
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            })
              .AddRoles<IdentityRole>()
              .AddSignInManager()
              .AddEntityFrameworkStores<DbContextIdentityRealStateApp>()
              .AddTokenProvider<DataProtectorTokenProvider<AppUsers>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(12);
            });
            #endregion

            #region authentication

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2),
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"] ?? ""))
                };
                opt.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = af =>
                    {
                        af.NoResult();
                        af.Response.StatusCode = 500;
                        af.Response.ContentType = "text/plain";
                        return af.Response.WriteAsync(af.Exception.Message.ToString());
                    },
                    OnChallenge = c =>
                    {
                        c.HandleResponse();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new JwtResponseDto { HasError = true, Errors = "You are not Authorized" });
                        return c.Response.WriteAsync(result);
                    },
                    OnForbidden = c =>
                    {
                        c.Response.StatusCode = 403;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new JwtResponseDto { HasError = true, Errors = "You are not Authorized to access this resource" });
                        return c.Response.WriteAsync(result);
                    }
                };
            }).AddCookie(IdentityConstants.ApplicationScheme, opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(180);
            });

            services.Configure<SecurityStampValidatorOptions>(opt =>
            {
                opt.ValidationInterval = TimeSpan.FromMinutes(5);
            });
            services.AddScoped<ISecurityStampValidator, SecurityStampValidator<AppUsers>>();

            #endregion
        }

        #endregion
       
    }
}
