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

            
            string[] roleNames = { "Administrador", "Supervisor", "Empleado" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            
            var adminUser = await userManager.FindByEmailAsync("admin@dis.com");
            if (adminUser == null)
            {
                adminUser = new Usuario
                {
                    UserName = "admin@dis.com",
                    Email = "admin@dis.com",
                    EmailConfirmed = true 
                };
                
                var result = await userManager.CreateAsync(adminUser, "Admin123!"); 
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrador");
                }
            }
            
            
            var empleadoUser = await userManager.FindByEmailAsync("empleado@dis.com");
            if (empleadoUser == null)
            {
                empleadoUser = new Usuario
                {
                    UserName = "empleado@dis.com",
                    Email = "empleado@dis.com",
                    EmailConfirmed = true
                };
               
                var result = await userManager.CreateAsync(empleadoUser, "Empleado123!"); 
                if (result.Succeeded)
                {
                    
                    await userManager.AddToRoleAsync(empleadoUser, "Empleado");
                }
            }

            
            var supervisorUser = await userManager.FindByEmailAsync("supervisor@dis.com");
            if (supervisorUser == null)
            {
                supervisorUser = new Usuario
                {
                    UserName = "supervisor@dis.com",
                    Email = "supervisor@dis.com",
                    EmailConfirmed = true
                };
                
                var result = await userManager.CreateAsync(supervisorUser, "Super123!");
                if (result.Succeeded)
                {
                    
                    await userManager.AddToRoleAsync(supervisorUser, "Supervisor");
                }
            }
        }
    }
}