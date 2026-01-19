using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Restaurantproject.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Connection_DB
builder.Services.AddDbContext<ApplicationDbContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"))
    );
#endregion

#region language
builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(option =>
{
    var supportedLanguage = new[]
    {
        new CultureInfo("En"),
        new CultureInfo("Ar")
    };
    option.DefaultRequestCulture = new RequestCulture("En");
    option.SupportedCultures = supportedLanguage;
    option.SupportedUICultures = supportedLanguage;
}
);
#endregion

builder.Services.AddIdentity<IdentityUser, IdentityRole>(
   option =>
   {
       option.Password.RequiredLength = 8;
       option.Password.RequireNonAlphanumeric = true;
       option.Password.RequireUppercase = true;
       option.User.RequireUniqueEmail = true;

   }
    ).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();

builder.Services.ConfigureApplicationCookie(
    option =>
    {
        option.AccessDeniedPath = "/User/AccessDenied";
        option.LoginPath = "/User/login";
        option.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
        option.Cookie.Name = "copkie";
        option.Cookie.HttpOnly = true;
        option.ExpireTimeSpan = TimeSpan.FromHours(1);


    }
    );


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

#region languages
var loc = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(loc!.Value);

#endregion

app.UseEndpoints(x=>
{
    x.MapControllerRoute(
          name: "Admin",
    pattern: "{area:exists}/{controller=Order}/{action=Orders}/{id?}");


    x.MapControllerRoute(
             name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}");
}

    );

app.Run();
