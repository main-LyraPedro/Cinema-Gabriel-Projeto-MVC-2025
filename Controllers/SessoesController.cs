using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaGabriel.Data;
using CinemaGabriel.Models;
using CinemaGabriel.ViewModels;

namespace CinemaGabriel.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão de sessões.
    /// FUNCIONALIDADES: CRUD completo + filtros por filme e data
    /// AUTORIZAÇÃO:
    /// - Index/Details: Todos os utilizadores autenticados
    /// - Create/Edit/Delete: Apenas Admin
    /// </summary>
    public class SessoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SessoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sessoes - Lista sessões com filtros opcionais
        [Authorize]
        public async Task<IActionResult> Index(int? filmeId, DateTime? data)
        {
            var query = _context.Sessoes
                .Include(s => s.Filme) // Carrega dados do filme relacionado
                .AsQueryable();

            // Filtro por filme
            if (filmeId.HasValue)
            {
                query = query.Where(s => s.FilmeId == filmeId.Value);
                ViewBag.FilmeSelecionado = await _context.Filmes.FindAsync(filmeId.Value);
            }

            // Filtro por data
            if (data.HasValue)
            {
                var dataInicio = data.Value.Date;
                var dataFim = dataInicio.AddDays(1);
                query = query.Where(s => s.Horario >= dataInicio && s.Horario < dataFim);
                ViewBag.DataSelecionada = data.Value;
            }

            var sessoes = await query
                .OrderBy(s => s.Horario) // Ordena por horário
                .ToListAsync();

            // Lista de filmes para o filtro
            ViewBag.Filmes = await _context.Filmes
                .OrderBy(f => f.Titulo)
                .ToListAsync();

            return View(sessoes);
        }

        // GET: Sessoes/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessao = await _context.Sessoes
                .Include(s => s.Filme)
                .Include(s => s.Reservas) // Carrega reservas
                    .ThenInclude(r => r.User) // Carrega dados dos utilizadores
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sessao == null)
            {
                return NotFound();
            }

            return View(sessao);
        }

        // GET: Sessoes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create(int? filmeId)
        {
            var viewModel = new SessaoViewModel();

            // Se vier de um filme específico, pré-seleciona
            if (filmeId.HasValue)
            {
                viewModel.FilmeId = filmeId.Value;
            }

            CarregarFilmes();
            return View(viewModel);
        }

        // POST: Sessoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(SessaoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // VALIDAÇÃO DE NEGÓCIO: Horário deve ser futuro
                if (viewModel.Horario <= DateTime.Now)
                {
                    ModelState.AddModelError("Horario", "O horário deve ser no futuro.");
                    CarregarFilmes();
                    return View(viewModel);
                }

                var sessao = new Sessao
                {
                    Horario = viewModel.Horario,
                    Sala = viewModel.Sala,
                    Preco = viewModel.Preco,
                    FilmeId = viewModel.FilmeId
                };

                _context.Add(sessao);
                await _context.SaveChangesAsync();

                TempData["Sucesso"] = "Sessão criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            CarregarFilmes();
            return View(viewModel);
        }

        // GET: Sessoes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessao = await _context.Sessoes
                .Include(s => s.Filme)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sessao == null)
            {
                return NotFound();
            }

            var viewModel = new SessaoViewModel
            {
                Id = sessao.Id,
                Horario = sessao.Horario,
                Sala = sessao.Sala,
                Preco = sessao.Preco,
                FilmeId = sessao.FilmeId,
                FilmeTitulo = sessao.Filme?.Titulo
            };

            CarregarFilmes();
            return View(viewModel);
        }

        // POST: Sessoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, SessaoViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var sessao = await _context.Sessoes.FindAsync(id);
                    if (sessao == null)
                    {
                        return NotFound();
                    }

                    sessao.Horario = viewModel.Horario;
                    sessao.Sala = viewModel.Sala;
                    sessao.Preco = viewModel.Preco;
                    sessao.FilmeId = viewModel.FilmeId;

                    _context.Update(sessao);
                    await _context.SaveChangesAsync();

                    TempData["Sucesso"] = "Sessão atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessaoExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            CarregarFilmes();
            return View(viewModel);
        }

        // GET: Sessoes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessao = await _context.Sessoes
                .Include(s => s.Filme)
                .Include(s => s.Reservas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sessao == null)
            {
                return NotFound();
            }

            return View(sessao);
        }

        // POST: Sessoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sessao = await _context.Sessoes.FindAsync(id);
            
            if (sessao != null)
            {
                // Cascade delete elimina automaticamente as reservas relacionadas
                _context.Sessoes.Remove(sessao);
                await _context.SaveChangesAsync();

                TempData["Sucesso"] = "Sessão eliminada com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SessaoExists(int id)
        {
            return _context.Sessoes.Any(e => e.Id == id);
        }

        /// <summary>
        /// Carrega lista de filmes para o SelectList (dropdown).
        /// </summary>
        private void CarregarFilmes()
        {
            ViewBag.Filmes = new SelectList(_context.Filmes.OrderBy(f => f.Titulo), "Id", "Titulo");
        }
    }
}