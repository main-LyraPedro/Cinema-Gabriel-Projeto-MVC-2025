using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CinemaGabriel.Data;
using CinemaGabriel.Models;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// CONFIGURAÇÃO DE SERVIÇOS
// ============================================================================

// Configuração do DbContext com SQLite
// BOA PRÁTICA: Connection string vem de appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do ASP.NET Identity
// Sistema de autenticação e autorização integrado
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Requisitos de password (personalizáveis)
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configuração de Cookies de autenticação
// Define rotas para login, logout e acesso negado
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Adicionar suporte a MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ============================================================================
// SEED DE DADOS INICIAIS
// Executado automaticamente no arranque da aplicação
// BOA PRÁTICA: Garante que roles e utilizadores padrão existem
// ============================================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Criar Roles (Admin, Cliente) e utilizadores padrão
        await CinemaGabriel.Data.SeedData.Initialize(services);
        
        // Promover utilizador específico a Admin
        await CinemaGabriel.Data.PromoteToAdmin.Execute(services, "admin@cinema.pt");
        
        // ====================================================================
        // CORREÇÃO: Garantir que Admin NÃO tem role Cliente
        // Este bloco pode ser removido após primeira execução
        // ====================================================================
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var adminUser = await userManager.FindByEmailAsync("admin@cinema.pt");
        
        if (adminUser != null)
        {
            // Remover role Cliente se existir (problema corrigido anteriormente)
            if (await userManager.IsInRoleAsync(adminUser, "Cliente"))
            {
                await userManager.RemoveFromRoleAsync(adminUser, "Cliente");
                // Console.WriteLine removido - apenas para debug
            }
        }
        // ====================================================================
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao executar Seed de dados.");
    }
}

// ============================================================================
// CONFIGURAÇÃO DO PIPELINE HTTP
// Define como os requests são processados
// ============================================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HTTP Strict Transport Security
}

app.UseHttpsRedirection(); // Redirecionar HTTP para HTTPS
app.UseStaticFiles(); // Servir ficheiros estáticos (CSS, JS, imagens)

app.UseRouting(); // Ativar routing

// IMPORTANTE: Ordem correta
app.UseAuthentication(); // Identificar utilizador (quem é?)
app.UseAuthorization();  // Verificar permissões (pode fazer?)

// Definir rota padrão (Controller/Action/Id)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();