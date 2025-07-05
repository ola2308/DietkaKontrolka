using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using KontrolaNawykow.Models;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja Azure SQL Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new Exception("Brak ConnectionString w pliku konfiguracji!");
    }
    Console.WriteLine("✅ Połączenie z Azure SQL Database skonfigurowane");
    options.UseSqlServer(connectionString);
});

// Konfiguracja CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("https://localhost:7169", "http://localhost:7169")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Konfiguracja uwierzytelniania cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "KontrolaNawykowAuth";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

// Konfiguracja sesji
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// MVC i Razor Pages
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Recipes/Add");
    options.Conventions.AuthorizePage("/Diet/Index");
    options.Conventions.AuthorizePage("/Profile/Index");
    options.Conventions.AuthorizePage("/Profile/Setup");
});

var app = builder.Build();

// Automatyczne tworzenie bazy danych na Azure
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        Console.WriteLine("🔄 Sprawdzanie połączenia z Azure SQL Database...");

        // Utworzenie bazy danych jeśli nie istnieje
        if (context.Database.EnsureCreated())
        {
            Console.WriteLine("✅ Baza danych została utworzona na Azure SQL!");
        }
        else
        {
            Console.WriteLine("✅ Połączono z istniejącą bazą danych na Azure SQL!");
        }

    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Błąd połączenia z bazą danych: {ex.Message}");
        throw;
    }
}

// Konfiguracja błędów
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowLocalhost");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Mapowanie tras
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
app.MapRazorPages();

// Przekierowania
app.MapGet("/", async context =>
{
    context.Response.Redirect("/Account/Login");
});

app.MapGet("/Diet", async context =>
{
    context.Response.Redirect("/Diet/Index");
});

app.MapGet("/Recipes", async context =>
{
    context.Response.Redirect("/Recipes/Add");
});

Console.WriteLine("🚀 APLIKACJA STARTUJE Z AZURE SQL DATABASE");
Console.WriteLine("📊 Dostępne funkcje:");
Console.WriteLine("   - Rejestracja i logowanie użytkowników");
Console.WriteLine("   - Przepisy z ocenami");
Console.WriteLine("   - Plany posiłków");
Console.WriteLine("   - Lista zakupów");
Console.WriteLine("   - Zadania ToDo");
Console.WriteLine("   - Nawyki");
Console.WriteLine("   - Statystyki");
Console.WriteLine("   - Lodówka");
Console.WriteLine("=====================================");

app.Run();