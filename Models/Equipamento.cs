// Equipamento.cs — o "molde" de um equipamento em manutenção
//
// Define quais informações um Equipamento tem.
// Cada propriedade aqui vira uma coluna na tabela do banco.

using System.ComponentModel.DataAnnotations;

namespace InfraManager.Models
{
    public class Equipamento
    {
        // Múmero único que identifica cada equipamento no banco (gerado automaticamente)
        public int Id { get; set; }

        // Número de patrimônio — obrigatório e único por equipamento
        [Required(ErrorMessage = "O patrimônio é obrigatório.")]
        public string Patrimonio { get; set; } = string.Empty;

        // Tipo do equipamento (ex: Computador, Impressora)
        [Required(ErrorMessage = "O tipo é obrigatório.")]
        public string Tipo { get; set; } = string.Empty;

        // Setor onde o equipamento está (ex: Call Center, T.I)
        [Required(ErrorMessage = "A localização é obrigatória.")]
        public string Localizacao { get; set; } = string.Empty;

        // Descrição do problema — opcional
        public string? Descricao { get; set; }

        // Gravidade do problema: Baixa, Média ou Alta
        // Começa como Média por ser o valor mais comum
        public string Gravidade { get; set; } = "Média";

        // Nome do técnico responsável pelo equipamento
        [Required(ErrorMessage = "O responsável é obrigatório.")]
        public string Responsavel { get; set; } = string.Empty;

        // Data em que o equipamento entrou para manutenção
        public DateTime DataEntrada { get; set; } = DateTime.Today;

        // se o equipamento já foi concluído (manutenção finalizada)
        // começa como false — equipamento recém cadastrado ainda está em aberto
        public bool Concluido { get; set; } = false;

        // data em que a manutenção foi concluída — opcional,
        // pois só é preenchida quando o técnico clica em "Concluir"
        public DateTime? DataConclusao { get; set; }

        // começa false só vira true quando o usuário confiramar a condenação
        public bool Condenado { get; set; } = false;

        public string? MarcaEquipamento { get; set; }

        public string? MotivoCondenacao { get; set; }

        public string? ReposicaoPatrimonio { get; set; }

        public DateTime? DataCondenacao { get; set; }
    }
}
