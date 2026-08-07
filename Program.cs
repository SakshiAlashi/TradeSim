using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradeSim.Data;
using TradeSim.Models.Domain;
using TradeSim.Services;
using TradeSim.Services.Providers;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services
    .AddAuthentication("TradeSimCookie")
    .AddCookie("TradeSimCookie", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TradeSimDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TradeSimConnection"));
});

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MarketService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IMarketDataProvider, MockMarketProvider>();

builder.Services.AddScoped<PasswordHasher<User>>();


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

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Dashboard}/{id?}")
    .WithStaticAssets();


app.Run();
