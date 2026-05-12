// Guia.cs — o "molde" de um guia de instalação

using System.ComponentModel.DataAnnotations;

namespace InfraManager.Models
{
    public class Guia
    {
       
        public int Id { get; set; }
       
        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; } = string.Empty;

        public string? ConteudoTexto { get; set; }

        public string? ArquivoPdf { get; set; }

        public string Autor { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
