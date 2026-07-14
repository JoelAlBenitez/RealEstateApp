using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Infraestructure.Identity.RegistrationAndConfiguration;
using RealEstateApp.Presentation.WebApp.Helpers;
using RealStateApp.IOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region composition root
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationDepdencies();
builder.Services.AddInfraestructurePersistence(builder.Configuration);
builder.Services.AddInfraestructrueShared(builder.Configuration);
builder.Services.AddWebAppServicesIdentity(builder.Configuration);
builder.Services.AddDependenciesCommon();
builder.Services.AddDependenciesWebApp();
builder.Services.AddScoped<IUserSession,UserSession>();
builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();


#endregion
var app = builder.Build();
await app.Services.GenerateDataSeedUsers();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
