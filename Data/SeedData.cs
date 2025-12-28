using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaGabriel.Data
{
    /// <summary>
    /// Classe responsável por popular a base de dados com dados iniciais.
    /// Cria Roles (Admin, Cliente) e utilizadores padrão.
    /// BOA PRÁTICA: Seed automático garante que o sistema tem dados mínimos para funcionar.
    /// </summary>
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<CinemaGabriel.Models.ApplicationUser>>();

            // Criar roles se não existirem
            string[] roleNames = { "Admin", "Cliente" };
            
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    // Console.WriteLine removido - apenas para debug durante desenvolvimento
                }
            }

            // Criar utilizador Admin padrão
            var adminEmail = "admin@cinema.pt";
            var adminPassword = "Admin@123"; // ⚠️ ALTERAR EM PRODUÇÃO por motivos de segurança

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            
            if (adminUser == null)
            {
                var newAdmin = new CinemaGabriel.Models.ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    NomeCompleto = "Administrador do Sistema"
                };

                var createAdmin = await userManager.CreateAsync(newAdmin, adminPassword);
                
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }

            // Criar utilizador Cliente de exemplo
            var clienteEmail = "cliente@cinema.pt";
            var clientePassword = "Cliente@123";

            var clienteUser = await userManager.FindByEmailAsync(clienteEmail);
            
            if (clienteUser == null)
            {
                var newCliente = new CinemaGabriel.Models.ApplicationUser
                {
                    UserName = clienteEmail,
                    Email = clienteEmail,
                    EmailConfirmed = true,
                    NomeCompleto = "Cliente Exemplo"
                };

                var createCliente = await userManager.CreateAsync(newCliente, clientePassword);
                
                if (createCliente.Succeeded)
                {
                    await userManager.AddToRoleAsync(newCliente, "Cliente");
                }
            }
        }
    }
}