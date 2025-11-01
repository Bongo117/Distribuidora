// --- Imports necesarios ---
using Microsoft.AspNetCore.Identity; // <--- ESTA ES LA LÍNEA QUE FALTABA
using Microsoft.EntityFrameworkCore;
using Distribuidora.Data;     // Para que encuentre el ApplicationDbContext
using Distribuidora.Models;    // Para que encuentre la clase Usuario

var builder = WebApplication.CreateBuilder(args);

// --- SECCIÓN DE CONFIGURACIÓN DE SERVICIOS ---

// 1. Lee la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Registra el DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

// 3. Registra Identity (Esta es la línea 19, que ahora funcionará)
// ESTE ES EL BUENO: Usa "AddDefaultIdentity" (para la UI) y le suma ".AddRoles"
builder.Services.AddDefaultIdentity<Usuario>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // <-- Le decimos a la "default" que agregue roles
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 4. Agrega el servicio de controladores y vistas
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// --- FIN DE SERVICIOS ---

var app = builder.Build(); // Construye la aplicación

// --- SECCIÓN DE "MIDDLEWARE" (El orden importa) ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 5. AÑADE ESTOS DOS (en este orden)
app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // <-- Necesario para las vistas de Login

// --- SECCIÓN PARA "SEMBRAR" DATOS (justo antes de app.Run()) ---

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Un error ocurrió durante el sembrado de la BD.");
    }
}

// --- FIN DE LA SECCIÓN DE SEMBRADO ---

app.Run(); 