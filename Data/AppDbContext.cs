// AppDbContext.cs — a "ponte" entre o sistema e o banco de dados
// toda vez que o sistema precisar salvar, buscar, editar ou
// apagar alguma coisa no banco, ele passa por aqui.

using Microsoft.EntityFrameworkCore;
using InfraManager.Models;

namespace InfraManager.Data
{
    public class AppDbContext : DbContext
    {
        // construtor: recebe as configurações de conexão com o banco
        // (essas configurações vêm do appsettings.json)
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // cada linha abaixo representa uma TABELA no banco de dados.

        public DbSet<Tarefa> Tarefas { get; set; }           // tabela de tarefas
        public DbSet<Equipamento> Equipamentos { get; set; } // tabela de equipamentos
        public DbSet<Conhecimento> Conhecimentos { get; set; }// tabela de conhecimentos
        public DbSet<Guia> Guias { get; set; }               // tabela de guias de instalação
    }
}
