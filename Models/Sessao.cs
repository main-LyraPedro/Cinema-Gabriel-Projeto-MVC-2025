using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaGabriel.Models
{
    /// <summary>
    /// Model que representa uma sessão de exibição de filme.
    /// Relacionamento: Pertence a um Filme e pode ter várias Reservas.
    /// </summary>
    public class Sessao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O horário é obrigatório.")]
        [Display(Name = "Horário")]
        public DateTime Horario { get; set; }

        [Required(ErrorMessage = "A sala é obrigatória.")]
        [StringLength(50, ErrorMessage = "O nome da sala não pode exceder 50 caracteres.")]
        [Display(Name = "Sala")]
        public string Sala { get; set; } = string.Empty;

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, 100.00, ErrorMessage = "O preço deve estar entre 0.01€ e 100.00€.")]
        [Column(TypeName = "decimal(18,2)")] // Define tipo SQL preciso para valores monetários
        [Display(Name = "Preço (€)")]
        public decimal Preco { get; set; }

        // Foreign Key - Relacionamento N:1 com Filme
        [Required(ErrorMessage = "O filme é obrigatório.")]
        [Display(Name = "Filme")]
        public int FilmeId { get; set; }
        
        [ForeignKey("FilmeId")] // Navigation Property
        public Filme? Filme { get; set; }

        // Relacionamento 1:N - Uma Sessão pode ter várias Reservas
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}