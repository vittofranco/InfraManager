// Program.cs — o arquivo de PARTIDA do sistema
// e o primeiro arquivo que roda quando você aperta F5.
// ele configura tudo antes do sistema começar a funcionar.
// a "chave geral" do sistema.

using InfraManager.Data;
using Microsoft.EntityFrameworkCore;

// cria o "construtor" do sistema — ele vai montar tudo antes de ligar
var builder = WebApplication.CreateBuilder(args);

// conecta o sistema ao banco de dados MySQL
// ele vai ler as informações de conexão (senha, nome do banco, etc.)
// do arquivo appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// diz pro sistema que vamos usar o padrão MVC
// (Controllers que controlam as ações + Views que mostram as telas)
builder.Services.AddControllersWithViews();

// aqui o sistema termina de ser "montado" e fica pronto pra usar
var app = builder.Build();

// se o sistema NÃO estiver em modo de desenvolvimento (ou seja, estiver em produção),
// mostra uma tela de erro amigável em vez de uma tela técnica feia
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// redireciona automaticamente de HTTP para HTTPS (conexão segura)
app.UseHttpsRedirection();

// liga o sistema de rotas — é o que faz a URL /Tarefas chamar o TarefasController
app.UseRouting();

// liga o sistema de permissões (quem pode acessar o quê)
app.UseAuthorization();

// habilita arquivos estáticos — sem isso, o sistema não consegue
// servir os PDFs que ficam salvos na pasta wwwroot/uploads/
app.UseStaticFiles();

app.MapStaticAssets();

// define a rota padrão do sistema: colinha 
// {controller} = qual controller vai responder (ex: Tarefas, Guias)
// {action}     = qual ação dentro do controller (ex: Index, Create)
// {id?}        = número opcional no final da URL (ex: /Tarefas/Edit/5)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// liga o sistema — a partir daqui ele começa a responder às requisições
app.Run();
