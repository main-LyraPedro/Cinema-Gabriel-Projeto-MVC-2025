using System.ComponentModel.DataAnnotations;

namespace CinemaGabriel.ViewModels
{
    /// <summary>
    /// ViewModel para criar/editar sessões.
    /// Simplifica o formulário, incluindo apenas dados necessários.
    /// </summary>
    public class SessaoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O horário é obrigatório.")]
        [Display(Name = "Horário")]
        public DateTime Horario { get; set; } = DateTime.Now.AddDays(1); // Padrão: amanhã

        [Required(ErrorMessage = "A sala é obrigatória.")]
        [StringLength(50, ErrorMessage = "O nome da sala não pode exceder 50 caracteres.")]
        [Display(Name = "Sala")]
        public string Sala { get; set; } = string.Empty;

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, 100.00, ErrorMessage = "O preço deve estar entre 0.01€ e 100.00€.")]
        [Display(Name = "Preço (€)")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O filme é obrigatório.")]
        [Display(Name = "Filme")]
        public int FilmeId { get; set; }

        // Propriedade auxiliar para exibição
        public string? FilmeTitulo { get; set; }
    }
}