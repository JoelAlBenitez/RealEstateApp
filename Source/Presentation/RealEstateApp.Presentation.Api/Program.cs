using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Presentation.Api.Helpers;
using RealStateApp.IOC;
using System.Text.Json.Serialization;
using RealEstateApp.Infraestructure.Identity.RegistrationAndConfiguration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//AddAppiVersioningExtension

#region composition root
builder.Services.AddApplicationDepdencies();

builder.Services.AddInfraestructurePersistence(builder.Configuration);
builder.Services.AddInfraestructrueShared(builder.Configuration);
builder.Services.AddWebApiServicesIdentity(builder.Configuration);
builder.Services.AddDependenciesCommon();
builder.Services.AddDependenciesWebApi();

builder.Services.AddScoped<IUserSession, UserSession>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//.....
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // quitar esta confiracion mas adelante cuando se trabaje con la Api
builder.Services.AddHealthChecks();

//builder.Services.AddAppiVersioningExtension();
//builder.Services.AddSwaggerExtension();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

#endregion
var app = builder.Build();

#region seed datas -> users and roles
await app.Services.GenerateDataSeedUsers();

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseHealthChecks("/health");

app.MapControllers();

app.Run();
