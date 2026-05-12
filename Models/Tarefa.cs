using System;

namespace InfraManager.Models
{
    public class Tarefa
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataConclusao { get; set; }

        public bool Concluida { get; set; } = false;
    }
}
