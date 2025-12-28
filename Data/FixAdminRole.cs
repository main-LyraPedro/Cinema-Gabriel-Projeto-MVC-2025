using Microsoft.AspNetCore.Identity;
using CinemaGabriel.Models;

namespace CinemaGabriel.Data
{
    public static class FixAdminRole
    {
        public static async Task Execute(IServiceProvider serviceProvider, string email)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            var user = await userManager.FindByEmailAsync(email);
            
            if (user != null)
            {
                // Remover role Cliente se existir
                if (await userManager.IsInRoleAsync(user, "Cliente"))
                {
                    await userManager.RemoveFromRoleAsync(user, "Cliente");
                    Console.WriteLine($"✅ Role 'Cliente' removida de {email}");
                }
                
                // Garantir que tem role Admin
                if (!await userManager.IsInRoleAsync(user, "Admin"))
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                    Console.WriteLine($"✅ Role 'Admin' adicionada a {email}");
                }
                else
                {
                    Console.WriteLine($"ℹ️ {email} já é Admin (sem role Cliente)");
                }
            }
        }
    }
}