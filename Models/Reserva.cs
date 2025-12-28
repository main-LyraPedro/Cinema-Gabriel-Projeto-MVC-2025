using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaGabriel.Models
{
    /// <summary>
    /// Model que representa uma reserva feita por um utilizador.
    /// Relacionamento: Pertence a uma Sessão e a um Utilizador (ApplicationUser).
    /// </summary>
    public class Reserva
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A data da reserva é obrigatória.")]
        [Display(Name = "Data da Reserva")]
        public DateTime DataReserva { get; set; } = DateTime.Now;

        // Foreign Key - Relacionamento N:1 com Sessão
        [Required(ErrorMessage = "A sessão é obrigatória.")]
        [Display(Name = "Sessão")]
        public int SessaoId { get; set; }
        
        [ForeignKey("SessaoId")]
        public Sessao? Sessao { get; set; }

        // Foreign Key - Relacionamento N:1 com ApplicationUser (Identity)
        [Required(ErrorMessage = "O utilizador é obrigatório.")]
        [Display(Name = "Utilizador")]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}