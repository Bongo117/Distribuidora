using Microsoft.AspNetCore.Identity;
using Distribuidora.Models; 

namespace Distribuidora.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();

            // --- 1. Creación de Roles ---
            string[] roleNames = { "Administrador", "Supervisor", "Empleado" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // --- 2. Creación del Usuario Administrador ---
            var adminUser = await userManager.FindByEmailAsync("admin@dis.com");
            if (adminUser == null)
            {
                adminUser = new Usuario
                {
                    UserName = "admin@dis.com",
                    Email = "admin@dis.com",
                    EmailConfirmed = true 
                };
                //Clave: Admin123!
                var result = await userManager.CreateAsync(adminUser, "Admin123!"); 
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrador");
                }
            }
            
            // --- 3. CREACIÓN DEL USUARIO EMPLEADO (¡NUEVO!) ---
            var empleadoUser = await userManager.FindByEmailAsync("empleado@dis.com");
            if (empleadoUser == null)
            {
                empleadoUser = new Usuario
                {
                    UserName = "empleado@dis.com",
                    Email = "empleado@dis.com",
                    EmailConfirmed = true
                };
                //Clave: Empleado123!
                var result = await userManager.CreateAsync(empleadoUser, "Empleado123!"); 
                if (result.Succeeded)
                {
                    // ¡Le asignamos el rol Empleado!
                    await userManager.AddToRoleAsync(empleadoUser, "Empleado");
                }
            }
        }
    }
}