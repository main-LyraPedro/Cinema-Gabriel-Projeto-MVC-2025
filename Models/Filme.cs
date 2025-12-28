using System.ComponentModel.DataAnnotations;

namespace CinemaGabriel.Models
{
    /// <summary>
    /// Model que representa um filme no sistema.
    /// Contém informações básicas do filme e relacionamento com Sessões.
    /// </summary>
    public class Filme
    {
        [Key] // Define como chave primária
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(200, ErrorMessage = "O título não pode exceder 200 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O género é obrigatório.")]
        [StringLength(100, ErrorMessage = "O género não pode exceder 100 caracteres.")]
        [Display(Name = "Género")]
        public string Genero { get; set; } = string.Empty;

        [Required(ErrorMessage = "A duração é obrigatória.")]
        [Range(1, 500, ErrorMessage = "A duração deve estar entre 1 e 500 minutos.")]
        [Display(Name = "Duração (minutos)")]
        public int Duracao { get; set; }

        [Display(Name = "Sinopse")]
        [StringLength(1000, ErrorMessage = "A sinopse não pode exceder 1000 caracteres.")]
        public string? Sinopse { get; set; }

        [Display(Name = "Caminho da Imagem")]
        public string? CaminhoImagem { get; set; } // Armazena o caminho da imagem no servidor

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Relacionamento 1:N - Um Filme pode ter várias Sessões
        public ICollection<Sessao> Sessoes { get; set; } = new List<Sessao>();
    }
}