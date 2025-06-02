using IdentityDemo.Context;
using IdentityDemo.Infrastructure.Identity;
using IdentityDemo.Models;
using IdentityDemoSysteam.Dtos;
using IdentityDemoSysteam.Infrastructure.GoogleEmail;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Şifre kuralları:
    options.Password.RequireDigit = true;            //Rakam zorunlu
    options.Password.RequireLowercase = true;        //Küçük harf zorunlu
    options.Password.RequireUppercase = true;        //Büyük harf zorunlu
    options.Password.RequireNonAlphanumeric = false; //Özel karakter zorunlu değil
    options.Password.RequiredLength = 6;             //Minimum uzunluk (örnek: 6)
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddErrorDescriber<CustomIdentityErrorDescriber>() //Türkçeleştirme burada
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;

    // Güvenlik ayarları
    options.Cookie.HttpOnly = true; // ❗ JS tarafından erişilemez (XSS koruması)
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // ❗ Her zaman HTTPS üzerinden gönderilsin
    options.Cookie.SameSite = SameSiteMode.Strict; // ❗ 3rd-party çerez saldırılarına karşı koruma
    options.Cookie.Name = "Accounting.Auth"; // Cookie adı özelleştirildi

    // Varsayılan şemayı açıkça belirt
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.MaxAge = TimeSpan.FromMinutes(60); // Alternatif tanım
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings")); 
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
