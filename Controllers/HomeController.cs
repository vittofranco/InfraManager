// HomeController.cs — controla a tela inicial do sistem
// um Controller é como um "garçom" do sistema:
// o usuário faz um pedido (acessando uma URL),
// o controller busca o que precisa e entrega a tela certa.
// ============================================================

using System.Diagnostics;
using InfraManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.Controllers
{
    public class HomeController : Controller
    {
        // quando o usuário acessa "/" ou "/Home",
        // essa função roda e mostra a tela inicial (Views/Home/Index.cshtml)
        public IActionResult Index()
        {
            return View();
        }

        // quando o usuário acessa "/Home/Privacy",
        // mostra a tela de privacidade (Views/Home/Privacy.cshtml)
        public IActionResult Privacy()
        {
            return View();
        }

        // quando acontece um erro no sistema, essa função roda
        // e mostra uma tela de erro amigável para o usuário
        // ResponseCache desliga o cache pra essa página — assim
        // o usuário sempre vê o erro real, nunca uma versão antiga salva
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
