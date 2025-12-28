using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaGabriel.Data;
using CinemaGabriel.Models;
using CinemaGabriel.ViewModels;

namespace CinemaGabriel.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão de reservas.
    /// Clientes podem criar e ver suas próprias reservas.
    /// Admins podem ver e gerir TODAS as reservas do sistema.
    /// BOA PRÁTICA: Controlo de acesso diferenciado por role.
    /// </summary>
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservas - Lista reservas (filtrada por role)
        public async Task<IActionResult> Index()
        {
            IQueryable<Reserva> query = _context.Reservas
                .Include(r => r.Sessao)
                    .ThenInclude(s => s.Filme) // ThenInclude: navegação encadeada
                .Include(r => r.User);

            // REGRA DE NEGÓCIO: Cliente vê só as suas, Admin vê todas
            if (User.IsInRole("Cliente"))
            {
                var userId = _userManager.GetUserId(User);
                query = query.Where(r => r.UserId == userId);
            }

            var reservas = await query
                .OrderByDescending(r => r.DataReserva)
                .ToListAsync();

            return View(reservas);
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Sessao)
                    .ThenInclude(s => s.Filme)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            // SEGURANÇA: Clientes só podem ver suas próprias reservas
            if (User.IsInRole("Cliente"))
            {
                var userId = _userManager.GetUserId(User);
                if (reserva.UserId != userId)
                {
                    return Forbid(); // HTTP 403
                }
            }

            return View(reserva);
        }

        // GET: Reservas/Create?sessaoId=X (Apenas Cliente)
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Create(int? sessaoId)
        {
            if (sessaoId == null)
            {
                TempData["Erro"] = "Sessão não especificada.";
                return RedirectToAction("Index", "Sessoes");
            }

            var sessao = await _context.Sessoes
                .Include(s => s.Filme)
                .FirstOrDefaultAsync(s => s.Id == sessaoId);

            if (sessao == null)
            {
                TempData["Erro"] = "Sessão não encontrada.";
                return RedirectToAction("Index", "Sessoes");
            }

            // VALIDAÇÃO DE NEGÓCIO 1: Sessão não pode ter passado
            if (sessao.Horario <= DateTime.Now)
            {
                TempData["Erro"] = "Não é possível reservar para sessões que já ocorreram.";
                return RedirectToAction("Details", "Sessoes", new { id = sessaoId });
            }

            // VALIDAÇÃO DE NEGÓCIO 2: Utilizador não pode ter reserva duplicada
            var userId = _userManager.GetUserId(User);
            var jaTemReserva = await _context.Reservas
                .AnyAsync(r => r.SessaoId == sessaoId && r.UserId == userId);

            if (jaTemReserva)
            {
                TempData["Erro"] = "Você já tem uma reserva para esta sessão.";
                return RedirectToAction("Details", "Sessoes", new { id = sessaoId });
            }

            // Preparar ViewModel com dados da sessão
            var viewModel = new ReservaViewModel
            {
                SessaoId = sessao.Id,
                FilmeTitulo = sessao.Filme?.Titulo,
                SessaoHorario = sessao.Horario,
                SessaoSala = sessao.Sala,
                SessaoPreco = sessao.Preco,
                FilmeCapa = sessao.Filme?.CaminhoImagem
            };

            return View(viewModel);
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Create(ReservaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var sessao = await _context.Sessoes
                    .Include(s => s.Filme)
                    .FirstOrDefaultAsync(s => s.Id == viewModel.SessaoId);

                if (sessao == null)
                {
                    TempData["Erro"] = "Sessão não encontrada.";
                    return RedirectToAction("Index", "Sessoes");
                }

                // Revalidar (proteção contra manipulação de formulário)
                if (sessao.Horario <= DateTime.Now)
                {
                    TempData["Erro"] = "Não é possível reservar para sessões que já ocorreram.";
                    return RedirectToAction("Details", "Sessoes", new { id = viewModel.SessaoId });
                }

                var userId = _userManager.GetUserId(User);
                var jaTemReserva = await _context.Reservas
                    .AnyAsync(r => r.SessaoId == viewModel.SessaoId && r.UserId == userId);

                if (jaTemReserva)
                {
                    TempData["Erro"] = "Você já tem uma reserva para esta sessão.";
                    return RedirectToAction("Details", "Sessoes", new { id = viewModel.SessaoId });
                }

                // Criar reserva
                var reserva = new Reserva
                {
                    DataReserva = DateTime.Now,
                    SessaoId = viewModel.SessaoId,
                    UserId = userId! // null-forgiving: userId sempre existe aqui
                };

                _context.Add(reserva);
                await _context.SaveChangesAsync();

                TempData["Sucesso"] = "Reserva realizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            // Se validação falhou, recarregar dados da sessão
            var sessaoReload = await _context.Sessoes
                .Include(s => s.Filme)
                .FirstOrDefaultAsync(s => s.Id == viewModel.SessaoId);

            if (sessaoReload != null)
            {
                viewModel.FilmeTitulo = sessaoReload.Filme?.Titulo;
                viewModel.SessaoHorario = sessaoReload.Horario;
                viewModel.SessaoSala = sessaoReload.Sala;
                viewModel.SessaoPreco = sessaoReload.Preco;
                viewModel.FilmeCapa = sessaoReload.Filme?.CaminhoImagem;
            }

            return View(viewModel);
        }

        // GET: Reservas/Delete/5 - Confirmação de cancelamento
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Sessao)
                    .ThenInclude(s => s.Filme)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            // SEGURANÇA: Clientes só podem cancelar suas próprias reservas
            if (User.IsInRole("Cliente"))
            {
                var userId = _userManager.GetUserId(User);
                if (reserva.UserId != userId)
                {
                    return Forbid();
                }
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5 - Confirmar cancelamento
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Sessao)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva != null)
            {
                // REGRA DIFERENCIADA POR ROLE
                if (User.IsInRole("Cliente"))
                {
                    var userId = _userManager.GetUserId(User);

                    // Cliente só cancela suas reservas
                    if (reserva.UserId != userId)
                    {
                        return Forbid();
                    }

                    // Cliente não pode cancelar se sessão já passou
                    if (reserva.Sessao?.Horario <= DateTime.Now)
                    {
                        TempData["Erro"] = "Não é possível cancelar reservas de sessões que já ocorreram.";
                        return RedirectToAction(nameof(Index));
                    }
                }

                // Admin pode cancelar qualquer reserva, a qualquer momento
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();

                TempData["Sucesso"] = "Reserva cancelada com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}