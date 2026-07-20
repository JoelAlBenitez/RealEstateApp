using RealEstateApp.Core.Application.Contracts.Api;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Application.Services.Api;
using RealEstateApp.Infraestructure.Identity.RegistrationAndConfiguration;
using RealEstateApp.Presentation.Api.Extensions;
using RealEstateApp.Presentation.Api.Helpers;
using RealStateApp.IOC;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

#region composition root
builder.Services.AddApplicationDepdencies();
builder.Services.AddInfraestructurePersistence(builder.Configuration);
builder.Services.AddInfraestructrueShared(builder.Configuration);
builder.Services.AddWebApiServicesIdentity(builder.Configuration);
builder.Services.AddDependenciesCommon(builder.Configuration);
builder.Services.AddDependenciesWebApi();

builder.Services.AddScoped<IUserSession, UserSession>();
builder.Services.AddScoped<IAgentApiService, AgentApiService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddApiVersioningExtension();
builder.Services.AddSwaggerExtension();
#endregion



var app = builder.Build();

#region seed datas -> users and roles
await app.Services.GenerateDataSeedUsers();
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExtension(app);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/health");

app.MapControllers();

await app.RunAsync();
