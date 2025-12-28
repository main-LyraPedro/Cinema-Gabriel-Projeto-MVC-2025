using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaGabriel.Data;
using CinemaGabriel.Models;
using CinemaGabriel.ViewModels;

namespace CinemaGabriel.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão de filmes (CRUD completo).
    /// Autorização: Admins podem criar/editar/eliminar, todos podem visualizar.
    /// BOA PRÁTICA: Controllers enxutos, lógica de negócio nos métodos.
    /// </summary>
    public class FilmesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment; // Para acesso ao wwwroot

        public FilmesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Filmes - Lista todos os filmes (acessível a utilizadores autenticados)
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var filmes = await _context.Filmes
                .OrderByDescending(f => f.DataCadastro)
                .ToListAsync();
            
            return View(filmes);
        }

        // GET: Filmes/Details/5 - Detalhes de um filme específico
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include: carrega sessões relacionadas (Eager Loading)
            var filme = await _context.Filmes
                .Include(f => f.Sessoes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        // GET: Filmes/Create - Formulário de criação (apenas Admin)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Filmes/Create - Processar criação de filme
        [HttpPost]
        [ValidateAntiForgeryToken] // Proteção contra CSRF
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(FilmeViewModel viewModel)
        {
            if (ModelState.IsValid) // Validação server-side
            {
                var filme = new Filme
                {
                    Titulo = viewModel.Titulo,
                    Genero = viewModel.Genero,
                    Duracao = viewModel.Duracao,
                    Sinopse = viewModel.Sinopse,
                    DataCadastro = DateTime.Now
                };

                // Processar upload de imagem
                if (viewModel.ImagemUpload != null)
                {
                    string caminhoImagem = await SalvarImagem(viewModel.ImagemUpload);
                    filme.CaminhoImagem = caminhoImagem;
                }

                _context.Add(filme);
                await _context.SaveChangesAsync();

                TempData["Sucesso"] = "Filme criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Filmes/Edit/5 - Formulário de edição
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filmes.FindAsync(id);
            if (filme == null)
            {
                return NotFound();
            }

            // Mapear Model para ViewModel
            var viewModel = new FilmeViewModel
            {
                Id = filme.Id,
                Titulo = filme.Titulo,
                Genero = filme.Genero,
                Duracao = filme.Duracao,
                Sinopse = filme.Sinopse,
                CaminhoImagemAtual = filme.CaminhoImagem
            };

            return View(viewModel);
        }

        // POST: Filmes/Edit/5 - Processar edição
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, FilmeViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var filme = await _context.Filmes.FindAsync(id);
                    if (filme == null)
                    {
                        return NotFound();
                    }

                    // Atualizar propriedades
                    filme.Titulo = viewModel.Titulo;
                    filme.Genero = viewModel.Genero;
                    filme.Duracao = viewModel.Duracao;
                    filme.Sinopse = viewModel.Sinopse;

                    // Se foi feito upload de nova imagem
                    if (viewModel.ImagemUpload != null)
                    {
                        // Apagar imagem antiga
                        if (!string.IsNullOrEmpty(filme.CaminhoImagem))
                        {
                            ApagarImagem(filme.CaminhoImagem);
                        }

                        // Salvar nova imagem
                        filme.CaminhoImagem = await SalvarImagem(viewModel.ImagemUpload);
                    }

                    _context.Update(filme);
                    await _context.SaveChangesAsync();

                    TempData["Sucesso"] = "Filme atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmeExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Recarregar imagem atual se validação falhar
            viewModel.CaminhoImagemAtual = _context.Filmes.AsNoTracking()
                .Where(f => f.Id == id)
                .Select(f => f.CaminhoImagem)
                .FirstOrDefault();

            return View(viewModel);
        }

        // GET: Filmes/Delete/5 - Confirmação de eliminação
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filmes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        // POST: Filmes/Delete/5 - Confirmar eliminação
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filme = await _context.Filmes.FindAsync(id);
            
            if (filme != null)
            {
                // Apagar imagem física do servidor
                if (!string.IsNullOrEmpty(filme.CaminhoImagem))
                {
                    ApagarImagem(filme.CaminhoImagem);
                }

                _context.Filmes.Remove(filme);
                await _context.SaveChangesAsync();

                TempData["Sucesso"] = "Filme eliminado com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FilmeExists(int id)
        {
            return _context.Filmes.Any(e => e.Id == id);
        }

        #region Métodos Auxiliares para Gestão de Imagens

        /// <summary>
        /// Salva a imagem no servidor e retorna o caminho relativo.
        /// BOA PRÁTICA: Nome único (GUID) evita conflitos de nomes.
        /// </summary>
        private async Task<string> SalvarImagem(IFormFile imagem)
        {
            string pastaUploads = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "filmes");
            
            // Criar pasta se não existir
            if (!Directory.Exists(pastaUploads))
            {
                Directory.CreateDirectory(pastaUploads);
            }

            // Gerar nome único (GUID + extensão original)
            string nomeUnico = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
            string caminhoCompleto = Path.Combine(pastaUploads, nomeUnico);

            // Salvar ficheiro fisicamente
            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await imagem.CopyToAsync(stream);
            }

            // Retornar caminho relativo (usado no HTML)
            return "/uploads/filmes/" + nomeUnico;
        }

        /// <summary>
        /// Apaga uma imagem do servidor.
        /// </summary>
        private void ApagarImagem(string caminhoRelativo)
        {
            if (string.IsNullOrEmpty(caminhoRelativo))
                return;

            string caminhoCompleto = Path.Combine(_hostEnvironment.WebRootPath, caminhoRelativo.TrimStart('/'));
            
            if (System.IO.File.Exists(caminhoCompleto))
            {
                System.IO.File.Delete(caminhoCompleto);
            }
        }

        #endregion
    }
}