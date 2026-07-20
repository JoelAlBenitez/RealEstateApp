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
builder.Services.AddDependenciesCommon(builder.Configuration);
builder.Services.AddDependenciesWebApp();
builder.Services.AddDependenciesWebApi();
builder.Services.AddScoped<IUserSession,UserSession>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
#endregion

var app = builder.Build();

await app.Services.GenerateDataSeedUsers();
await app.Services.SeedSaleTypesAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/StatusCodeError", "?code={0}");

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
