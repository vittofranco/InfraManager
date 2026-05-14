// EquipamentosController.cs — controla tudo sobre equipamentos
// Segue o mesmo padrão do TarefasController:
// cada função responde a uma URL e faz uma ação no banco.

using InfraManager.Data;
using InfraManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.Controllers
{
    public class EquipamentosController : Controller
    {
        // conexão com o banco de dados
        private readonly AppDbContext _context;

        // construtor: o sistema injeta a conexão com o banco automaticamente
        public EquipamentosController(AppDbContext context)
        {
            _context = context;
        }

        // responde a: GET /Equipamentos
        // lista apenas os equipamentos EM ABERTO (não concluídos e não condenados)
        // o parâmetro "busca" vem da URL: /Equipamentos?busca=12345
        public async Task<IActionResult> Index(string busca)
        {
            // busca só os equipamentos que ainda estão em manutenção
            var equipamentos = _context.Equipamentos
                .Where(e => !e.Concluido && !e.Condenado)
                .AsQueryable();

            // se o usuário digitou algo no campo de busca, filtra pelo patrimônio
            // Contains verifica se o patrimônio contém o texto digitado
            if (!string.IsNullOrEmpty(busca))
                equipamentos = equipamentos.Where(e => e.Patrimonio.Contains(busca));

            return View(await equipamentos.OrderByDescending(e => e.DataEntrada).ToListAsync());
        }

        // CREATE (GET)
        // responde a: GET /Equipamentos/Create
        // abre o formulário vazio para cadastrar um novo equipamento
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        // responde a: POST /Equipamentos/Create
        // roda quando o usuário preenche o formulário e clica em "Cadastrar"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Equipamento equipamento)
        {
            // verifica se já existe um equipamento com esse patrimônio no banco
            // AnyAsync retorna true se encontrar pelo menos um resultado
            bool patrimonioJaExiste = await _context.Equipamentos
                .AnyAsync(e => e.Patrimonio == equipamento.Patrimonio);

            if (patrimonioJaExiste)
            {
                ModelState.AddModelError("Patrimonio", "Já existe um equipamento com esse patrimônio.");
                return View(equipamento);
            }

            if (ModelState.IsValid)
            {
                _context.Equipamentos.Add(equipamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(equipamento);
        }

        // EDIT (GET)
        // responde a: GET /Equipamentos/Edit/5
        // abre o formulário já preenchido com os dados do equipamento
        public async Task<IActionResult> Edit(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento == null)
                return NotFound();

            return View(equipamento);
        }

        // EDIT (POST)
        // responde a: POST /Equipamentos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Equipamento formulario)
        {
            if (id != formulario.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                // busca o equipamento atual do banco
                var equipamento = await _context.Equipamentos.FindAsync(id);

                if (equipamento == null)
                    return NotFound();

                // atualiza só os campos editáveis — campos como Concluido,
                // DataConclusao, Condenado etc. não são sobrescritos
                equipamento.Tipo        = formulario.Tipo;
                equipamento.Localizacao = formulario.Localizacao;
                equipamento.Descricao   = formulario.Descricao;
                equipamento.Gravidade   = formulario.Gravidade;
                equipamento.Responsavel = formulario.Responsavel;
                equipamento.DataEntrada = formulario.DataEntrada;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(formulario);
        }

        // DETALHES
        // responde a: GET /Equipamentos/Detalhes/5
        // mostra todos os dados de um equipamento específico
        public async Task<IActionResult> Detalhes(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento == null)
                return NotFound();

            return View(equipamento);
        }

        // DELETE (GET)
        // responde a: GET /Equipamentos/Delete/5
        // mostra a tela de confirmação antes de excluir
        public async Task<IActionResult> Delete(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento == null)
                return NotFound();

            return View(equipamento);
        }

        // DELETE (POST)
        // responde a: POST /Equipamentos/Delete/5
        // executa a exclusão após confirmação do usuário
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento != null)
            {
                _context.Equipamentos.Remove(equipamento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // CONCLUIR (POST)
        // responde a: POST /Equipamentos/Concluir/5
        // marca o equipamento como concluído e registra data e hora
        // é POST porque altera dados no banco — nunca GET para isso
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Concluir(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento == null)
                return NotFound();

            equipamento.Concluido = true;
            // DateTime.Now registra data E hora
            equipamento.DataConclusao = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // HISTORICO (GET)
        // responde a: GET /Equipamentos/Historico
        // lista os equipamentos com manutenção concluída com sucesso
        public async Task<IActionResult> Historico()
        {
            var concluidos = await _context.Equipamentos
                .Where(e => e.Concluido)
                .OrderByDescending(e => e.DataConclusao)
                .ToListAsync();

            return View(concluidos);
        }

        // CONDENAR (GET)
        // responde a: GET /Equipamentos/Condenar/5
        // abre o formulário de condenação com os dados do equipamento já preenchidos
        public async Task<IActionResult> Condenar(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento == null)
                return NotFound();

            return View(equipamento);
        }

        // CONDENAR (POST)
        // responde a: POST /Equipamentos/Condenar/5
        // salva os dados da condenação e muda o status do equipamento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Condenar(int id, string? marcaEquipamento, string? motivoCondenacao, string? reposicaoPatrimonio)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento == null)
                return NotFound();

            equipamento.Condenado            = true;
            equipamento.DataCondenacao        = DateTime.Now;
            equipamento.MarcaEquipamento      = marcaEquipamento;
            equipamento.MotivoCondenacao      = motivoCondenacao;
            equipamento.ReposicaoPatrimonio   = reposicaoPatrimonio;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Condenados));
        }

        // CONDENADOS (GET)
        // responde a: GET /Equipamentos/Condenados
        // lista todos os equipamentos que receberam baixa no patrimônio
        public async Task<IActionResult> Condenados()
        {
            var condenados = await _context.Equipamentos
                .Where(e => e.Condenado)
                .OrderByDescending(e => e.DataCondenacao)
                .ToListAsync();

            return View(condenados);
        }
    }
}
