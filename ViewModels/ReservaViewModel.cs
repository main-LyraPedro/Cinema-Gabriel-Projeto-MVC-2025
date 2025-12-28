using System.ComponentModel.DataAnnotations;

namespace CinemaGabriel.ViewModels
{
    /// <summary>
    /// ViewModel para criar reservas.
    /// Agrupa informações da sessão para exibição na confirmação.
    /// </summary>
    public class ReservaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A sessão é obrigatória.")]
        [Display(Name = "Sessão")]
        public int SessaoId { get; set; }

        // Propriedades para exibição (não vão para o banco)
        public string? FilmeTitulo { get; set; }
        public DateTime? SessaoHorario { get; set; }
        public string? SessaoSala { get; set; }
        public decimal? SessaoPreco { get; set; }
        public string? FilmeCapa { get; set; }
    }
}