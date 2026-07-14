using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Infraestructure.Identity.RegistrationAndConfiguration;
using RealEstateApp.Presentation.WebApp.Helpers;
using RealStateApp.IOC;

var builder = WebApplication.CreateBuilder(args);

#region manager session
builder.Services.AddSession(opt =>
{
    opt.Cookie.HttpOnly = true;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    opt.Cookie.SameSite = SameSiteMode.Lax;
    opt.IOTimeout = TimeSpan.FromSeconds(60);
    opt.IdleTimeout = TimeSpan.FromMinutes(30);
});
#endregion


#region composition root
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationDepdencies();
builder.Services.AddInfraestructurePersistence(builder.Configuration);
builder.Services.AddInfraestructrueShared(builder.Configuration);
builder.Services.AddWebAppServicesIdentity(builder.Configuration);
builder.Services.AddDependenciesCommon();
builder.Services.AddDependenciesWebApp();

builder.Services.AddScoped<IUserSession,UserSession>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession();
#endregion

var app = builder.Build();
await app.Services.GenerateDataSeedUsers();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapStaticAssets();


app.MapControllerRoute( //cambiar page default
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
