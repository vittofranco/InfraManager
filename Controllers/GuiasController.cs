// GuiasController.cs — controla tudo relacionado aos guias
// igual ao TarefasController, mas com uma diferença

using InfraManager.Data;
using InfraManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.Controllers
{
    public class GuiasController : Controller
    {
        // conexão com o banco de dados
        private readonly AppDbContext _context;

        // caminho da pasta onde os PDFs serão salvos no servidor
        private readonly string _uploadFolder;

        // construtor: recebe a conexão com o banco E o ambiente do servidor
        // IWebHostEnvironment nos dá informações sobre onde o sistema está rodando,
        // incluindo o caminho real da pasta wwwroot no computador
        public GuiasController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;

            // monta o caminho completo da pasta de uploads:
            // WebRootPath = caminho da pasta wwwroot no servidor
            // "uploads"   = subpasta dentro de wwwroot
            // resultado ex: C:\MeuProjeto\wwwroot\uploads
            _uploadFolder = Path.Combine(env.WebRootPath, "uploads");

            // cria a pasta "uploads" se ela ainda não existir
            // assim nunca vai dar erro de "pasta não encontrada" ao salvar um PDF
            Directory.CreateDirectory(_uploadFolder);
        }

        // INDEX
        // responde a: GET /Guias
        // lista todos os guias do banco, do mais recente pro mais antigo
        public async Task<IActionResult> Index()
        {
            var guias = await _context.Guias
                .OrderByDescending(g => g.DataCriacao)
                .ToListAsync();

            return View(guias);
        }

        // GET
        // responde a: GET /Guias/Create
        // apenas abre o formulário vazio para criar um novo guia
        public IActionResult Create()
        {
            return View();
        }

        // POST
        // responde a: POST /Guias/Create
        // roda quando o usuário preenche o formulário e clica em "Salvar Guia"
        // IFormFile? é o tipo usado para receber arquivos enviados pelo formulário
        // o "?" significa que o arquivo é opcional
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guia guia, IFormFile? arquivoPdf)
        {
            // CORREÇÃO: remove ArquivoPdf e ConteudoTexto da validação automática
            // porque esses campos são opcionais individualmente — a validação manual
            // está logo abaixo, verificando se pelo menos um dos dois foi preenchido
            ModelState.Remove("ArquivoPdf");
            ModelState.Remove("ConteudoTexto");

            // verifica se o usuário enviou algum arquivo
            if (arquivoPdf != null && arquivoPdf.Length > 0)
            {
                // pega a extensão do arquivo (ex: ".pdf", ".docx")
                var extensao = Path.GetExtension(arquivoPdf.FileName).ToLower();

                // se não for PDF, rejeita e mostra erro no formulário
                if (extensao != ".pdf")
                {
                    ModelState.AddModelError("ArquivoPdf", "Apenas arquivos PDF são permitidos.");
                    return View(guia);
                }

                // pega o nome original do arquivo de forma segura
                // (remove caracteres perigosos do nome)
                var nomeSeguro = Path.GetFileName(arquivoPdf.FileName);

                // monta o nome final: NomeDoAutor_NomeDoArquivo.pdf
                // replace(" ", "_") troca espaços por underline no nome do autor
                // se o autor estiver vazio, usa "sem_autor" para não quebrar o nome do arquivo
                var autorNome = string.IsNullOrWhiteSpace(guia.Autor) ? "sem_autor" : guia.Autor.Replace(" ", "_");
                var nomeArquivo = $"{autorNome}_{nomeSeguro}";

                // monta o caminho completo onde o arquivo será salvo
                var caminhoCompleto = Path.Combine(_uploadFolder, nomeArquivo);

                // abre um "stream" (fluxo) e copia o arquivo recebido pra dentro dele
                // usando "using" garante que o arquivo é fechado corretamente após salvar
                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await arquivoPdf.CopyToAsync(stream);
                }

                // salva APENAS O NOME do arquivo no banco de dados
                // O arquivo em si fica na pasta uploads — no banco só guardamos o nome
                // para saber qual arquivo buscar depois
                guia.ArquivoPdf = nomeArquivo;
            }

            // verifica se pelo menos texto OU pdf foi preenchido
            // IsNullOrWhiteSpace verifica se está vazio ou só tem espaços
            if (string.IsNullOrWhiteSpace(guia.ConteudoTexto) && string.IsNullOrWhiteSpace(guia.ArquivoPdf))
            {
                ModelState.AddModelError("", "Preencha o texto ou envie um PDF.");
                return View(guia);
            }

            if (ModelState.IsValid)
            {
                _context.Guias.Add(guia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(guia);
        }

        // ver
        // responde a: GET /Guias/Ver/5
        // abre a tela que mostra o conteúdo completo do guia
        public async Task<IActionResult> Ver(int id)
        {
            var guia = await _context.Guias.FindAsync(id);

            if (guia == null)
                return NotFound();

            return View(guia);
        }

        // DELETE (GET)
        // responde a: GET /Guias/Delete/5
        // mostra a tela de confirmação antes de excluir
        public async Task<IActionResult> Delete(int id)
        {
            var guia = await _context.Guias.FindAsync(id);

            if (guia == null)
                return NotFound();

            return View(guia);
        }

        // DELETE (POST)
        // responde a: POST /Guias/Delete/5
        // roda quando o usuário confirma a exclusão
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guia = await _context.Guias.FindAsync(id);

            if (guia != null)
            {
                // Se esse guia tinha um PDF, deleta o arquivo físico da pasta uploads também
                // não adianta só remover do banco — o arquivo ficaria "sobrando" na pasta
                if (!string.IsNullOrEmpty(guia.ArquivoPdf))
                {
                    // monta o caminho completo do arquivo
                    var caminho = Path.Combine(_uploadFolder, guia.ArquivoPdf);

                    // verifica se o arquivo ainda existe antes de tentar deletar
                    // (evita erro caso o arquivo já tenha sido apagado manualmente)
                    if (System.IO.File.Exists(caminho))
                        System.IO.File.Delete(caminho);
                }

                _context.Guias.Remove(guia);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
