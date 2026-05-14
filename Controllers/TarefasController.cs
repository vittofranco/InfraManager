// TarefasController.cs — controla tudo relacionado às tarefas
// cada função aqui responde a uma URL diferente.
// é o "cérebro" das tarefas: busca no banco, salva, edita e exclui.

using InfraManager.Data;
using InfraManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.Controllers
{
    public class TarefasController : Controller
    {
        // guarda a conexão com o banco de dados para usar nas funções abaixo
        private readonly AppDbContext _context;

        // construtor: quando o sistema cria esse controller,
        // ele injeta automaticamente a conexão com o banco aqui
        public TarefasController(AppDbContext context)
        {
            _context = context;
        }

        // responde a: GET /Tarefas
        // mostra a lista de todas as tarefas
        // o parâmetro "filtro" vem da URL: /Tarefas?filtro=pendentes
        public async Task<IActionResult> Index(string filtro)
        {
            // começa buscando todas as tarefas do banco
            var tarefas = _context.Tarefas.AsQueryable();

            // se o usuário escolheu um filtro, aplica o filtro na busca
            if (filtro == "pendentes")
                tarefas = tarefas.Where(t => !t.Concluida); // só as não concluídas

            else if (filtro == "concluidas")
                tarefas = tarefas.Where(t => t.Concluida);  // só as concluídas

            // ordena do mais recente pro mais antigo e envia para a View
            // "await" significa "espera terminar antes de continuar" (busca no banco pode demorar)
            return View(await tarefas.OrderByDescending(t => t.DataCriacao).ToListAsync());
        }

        // CREATE (GET)
        // responde a: GET /Tarefas/Create
        // apenas abre o formulário vazio para criar uma nova tarefa
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        // responde a: POST /Tarefas/Create
        // roda quando o usuário preenche o formulário e clica em "Salvar"
        // [HttpPost] significa que essa função só aceita envios de formulário
        // [ValidateAntiForgeryToken] é uma proteção de segurança automática
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tarefa tarefa)
        {
            // ModelState.IsValid verifica se todos os campos obrigatórios foram preenchidos
            if (ModelState.IsValid)
            {
                _context.Tarefas.Add(tarefa);  // adiciona a tarefa no banco
                _context.SaveChanges();         // confirma e salva no banco
                return RedirectToAction(nameof(Index)); // volta pra lista
            }

            // se algum campo obrigatório está vazio, volta pro formulário mostrando os erros
            return View(tarefa);
        }

        // EDIT (GET)
        // responde a: GET /Tarefas/Edit/5  (o 5 é o Id da tarefa)
        // abre o formulário já preenchido com os dados da tarefa para editar
        public async Task<IActionResult> Edit(int id)
        {
            // busca no banco a tarefa com esse Id
            var tarefa = await _context.Tarefas.FindAsync(id);

            // se não encontrou nenhuma tarefa com esse Id, retorna o bendito erro 404
            if (tarefa == null)
                return NotFound();

            // manda a tarefa encontrada para a tela de edição
            return View(tarefa);
        }

        // EDIT (POST)
        // responde a: POST /Tarefas/Edit/5
        // roda quando o usuário edita o formulário e clica em "Salvar Alterações"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tarefa tarefa)
        {
            // cerifica se o Id da URL bate com o Id que veio do formulário
            // (segurança para ninguém editar a tarefa errada)
            if (id != tarefa.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(tarefa);            // atualiza os dados no banco
                await _context.SaveChangesAsync();  // confirma e salva
                return RedirectToAction(nameof(Index));
            }

            return View(tarefa);
        }

        // DELETE (GET)
        // responde a: GET /Tarefas/Delete/5
        // mostra a tela de confirmação antes de excluir
        public async Task<IActionResult> Delete(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa == null)
                return NotFound();

            return View(tarefa);
        }

        // DELETE (POST)
        // responde a: POST /Tarefas/Delete/5
        // roda quando o usuário confirma a exclusão clicando em "Sim, excluir"
        // ActionName("Delete") faz essa função responder à mesma URL do GET acima
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            // só exclui se realmente encontrou a tarefa (proteção extra)
            if (tarefa != null)
            {
                _context.Tarefas.Remove(tarefa);    // marca para excluir
                await _context.SaveChangesAsync();  // confirma e remove do banco
            }

            return RedirectToAction(nameof(Index));
        }

        // CONCLUIR (POST)
        // responde a: POST /Tarefas/Concluir/5
        // marca uma tarefa como concluída
        // nunca use um link simples (<a href>) para alterar dados!
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Concluir(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa == null)
                return NotFound();

            // marca a tarefa como concluída e registra a data e hora
            tarefa.Concluida = true;
            tarefa.DataConclusao = DateTime.Now;

            await _context.SaveChangesAsync(); // salva a alteração no banco

            return RedirectToAction(nameof(Index));
        }
    }
}
