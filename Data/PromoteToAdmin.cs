using Microsoft.AspNetCore.Identity;
using CinemaGabriel.Models;

namespace CinemaGabriel.Data
{
    /// <summary>
    /// Classe utilitária para promover utilizadores a Admin.
    /// Usado no Seed para garantir que existe pelo menos um Admin.
    /// BOA PRÁTICA: Separar lógica de promoção do SeedData principal.
    /// </summary>
    public static class PromoteToAdmin
    {
        public static async Task Execute(IServiceProvider serviceProvider, string email)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            // Garantir que a role Admin existe
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            
            var user = await userManager.FindByEmailAsync(email);
            
            if (user != null)
            {
                var isInRole = await userManager.IsInRoleAsync(user, "Admin");
                
                if (!isInRole)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                    // Console.WriteLine removido - mensagens só para debug
                }
            }
        }
    }
}