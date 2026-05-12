// Conhecimento.cs — o "molde" de um registro de conhecimento
// serve para registrar problemas que já foram resolvidos,
// junto com a solução, para consultar no futuro.

using System;

namespace InfraManager.Models
{
    public class Conhecimento
    {
        // número único que identifica cada registro no banco (patrimonio*
        public int Id { get; set; }

        // título do problema registrado 
        public string Titulo { get; set; } = string.Empty;

        // descrição detalhada do problema que aconteceu
        public string Problema { get; set; } = string.Empty;

        // como o problema foi resolvido
        public string Solucao { get; set; } = string.Empty;

        // informações extras — opcional
        public string? Observacoes { get; set; }

        // data em que o conhecimento foi registrado
        public DateTime DataRegistro { get; set; } = DateTime.Now;
    }
}
