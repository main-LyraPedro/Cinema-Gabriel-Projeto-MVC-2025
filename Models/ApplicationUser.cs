using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CinemaGabriel.Models
{
    /// <summary>
    /// Estende o IdentityUser com campos personalizados.
    /// Usado para autenticação e autorização com ASP.NET Identity.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Nome Completo")]
        [StringLength(200, ErrorMessage = "O nome não pode exceder 200 caracteres.")]
        public string? NomeCompleto { get; set; }

        [Display(Name = "Data de Registro")]
        public DateTime DataRegistro { get; set; } = DateTime.Now;

        // Relacionamento 1:N - Um Utilizador pode ter várias Reservas
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}