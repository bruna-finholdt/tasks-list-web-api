using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ListaDeTarefas2023.Domain.DTO
{
    public class TarefaCreateRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "O título da tarefa é obrigatório!")]
        public string? Título { get; set; }
        public string? Descrição { get; set; }

        [Range(1, 5, ErrorMessage = "A prioridade deve ser um valor entre 1 e 5"), DefaultValue(5)]
        public int? Prioridade { get; set; }
    }
}
