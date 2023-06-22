using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ListaDeTarefas2023.Domain.DTO
{
    public class TarefaUpdateRequest
    {
        [DefaultValue(false)]
        public bool Concluído { get; set; }

        [Range(1, 5, ErrorMessage = "A prioridade deve ser um valor entre 1 e 5")]
        public int? Prioridade { get; set; }
    }
}
