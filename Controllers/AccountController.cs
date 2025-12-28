using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CinemaGabriel.Models;
using CinemaGabriel.ViewModels;

namespace CinemaGabriel.Controllers
{
    /// <summary>
    /// Controller responsável por autenticação e autorização.
    /// Usa ASP.NET Identity para gestão de utilizadores.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Account/Login
        [AllowAnonymous] // Permite acesso sem autenticação
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login - Processar login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // PasswordSignInAsync: verifica email e password
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, 
                    model.Password, 
                    model.RememberMe, 
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Redirecionar para URL de retorno (se seguro) ou Home
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email ou password inválidos.");
                }
            }

            return View(model);
        }

        // GET: Account/Register
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register - Criar nova conta
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    NomeCompleto = model.NomeCompleto,
                    DataRegistro = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // REGRA: Novos utilizadores são sempre Clientes
                    await _userManager.AddToRoleAsync(user, "Cliente");

                    // Login automático após registo
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/AccessDenied - Página de acesso negado
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // ====================================================================
        // REMOVER APÓS CORRIGIR ROLES DO ADMIN (método temporário de debug)
        // ====================================================================
        /*
        [AllowAnonymous]
        public async Task<IActionResult> FixAdminRoles()
        {
            var user = await _userManager.FindByEmailAsync("admin@cinema.pt");
            
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var resultado = new System.Text.StringBuilder();
                
                resultado.AppendLine("ANTES:");
                resultado.AppendLine($"Roles: {string.Join(", ", roles)}");
                
                if (await _userManager.IsInRoleAsync(user, "Cliente"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Cliente");
                    resultado.AppendLine("✅ Role 'Cliente' REMOVIDA");
                }
                
                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    resultado.AppendLine("✅ Role 'Admin' ADICIONADA");
                }
                
                roles = await _userManager.GetRolesAsync(user);
                resultado.AppendLine("");
                resultado.AppendLine("DEPOIS:");
                resultado.AppendLine($"Roles: {string.Join(", ", roles)}");
                
                return Content(resultado.ToString(), "text/plain");
            }
            
            return Content("Utilizador não encontrado");
        }
        */
        // ====================================================================
    }
}