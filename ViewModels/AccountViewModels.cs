using System.ComponentModel.DataAnnotations;

namespace CinemaGabriel.ViewModels
{
    /// <summary>
    /// ViewModels para autenticação e registo.
    /// Separados do ApplicationUser por segurança e simplicidade.
    /// </summary>
    
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A password é obrigatória.")]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Password")]
        [Compare("Password", ErrorMessage = "As passwords não coincidem.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A password é obrigatória.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Lembrar-me")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}