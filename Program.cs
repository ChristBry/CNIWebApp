using Microsoft.EntityFrameworkCore;
using CNIWebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddRazorPages();

// 1. RÉCUPÉRATION DE LA CHAÎNE DE CONNEXION (depuis appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. CONFIGURATION DU SERVICE DE BASE DE DONNÉES
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AutoriserTout",
        policy =>
        {
            policy.AllowAnyOrigin() // Autorise n'importe quel site à appeler l'API
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Ajout du support MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("AutoriserTout");

app.UseRouting();

app.UseAuthorization();

// 5. DÉFINITION DE LA ROUTE PAR DÉFAUT (Essentiel pour MVC)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// app.MapRazorPages();

app.Run();
