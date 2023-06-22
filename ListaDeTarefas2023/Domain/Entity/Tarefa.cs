using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ListaDeTarefas2023.Domain.Entity
{
    [Table("Tarefas")]
    public class Tarefa
    {
        [Key]
        public int IdTarefa { get; set; }
        public string? Título { get; set; }
        public string? Descrição { get; set; }

        [DefaultValue(false)]
        public bool Concluído { get; set; }
        public int Prioridade { get; set; }
    }
}
