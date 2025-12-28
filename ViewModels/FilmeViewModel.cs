using System.ComponentModel.DataAnnotations;

namespace CinemaGabriel.ViewModels
{
    /// <summary>
    /// ViewModel para criar/editar filmes.
    /// Separa a lógica de apresentação do Model, permitindo upload de imagem.
    /// BOA PRÁTICA: ViewModels evitam expor Models diretamente nas Views.
    /// </summary>
    public class FilmeViewModel
    {
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

        [Display(Name = "Imagem do Filme")]
        public IFormFile? ImagemUpload { get; set; } // Recebe o ficheiro do formulário

        [Display(Name = "Imagem Atual")]
        public string? CaminhoImagemAtual { get; set; } // Mantém referência à imagem existente
    }
}