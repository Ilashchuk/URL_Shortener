using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data;
using URL_Shortener.Services.AccountServices;
using URL_Shortener.Services.AlgorithmServices;
using URL_Shortener.Services.URLsServices;
using URL_Shortener.Services.UsersServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<URL_Shortener_Context>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Account/Login");
    });

builder.Services.AddScoped<IUsersCntrollerService, UsersCntrollerService>();
builder.Services.AddScoped<IUrlsControllerService, UrlsControllerService>();
builder.Services.AddScoped<IShortenerAlgorithmService, ShortenerAlgorithmService>();
builder.Services.AddScoped<IAccountControllerService, UsersCntrollerService>();

//IAccountControllerService

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
