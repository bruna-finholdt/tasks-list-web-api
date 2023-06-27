using Microsoft.EntityFrameworkCore;
using ListaDeTarefas2023.DAL;
using ListaDeTarefas2023.Domain.DTO;
using ListaDeTarefas2023.Domain.Entity;
using ListaDeTarefas2023.Services.Base;
using ListaDeTarefas2023.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ListaDeTarefas2023.Services
{
    public class TarefasService
    {
        private readonly AppDbContext? _dbContext;

        public TarefasService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ServiceResponse<Tarefa> CadastrarNovaTarefa(TarefaCreateRequest model)
        {

            var novaTarefa = new Tarefa()
            {
                Título = model.Título,
                Descrição = model.Descrição,
                //Concluído = false,
                Prioridade = model.Prioridade.Value
            };

            _dbContext?.Add(novaTarefa);
            _dbContext?.SaveChanges();
            return new ServiceResponse<Tarefa>(novaTarefa);
        }

        public List<Tarefa> ListarTodos()
        {
            return _dbContext.Tarefas.ToList();
        }

        public ServiceResponse<Tarefa> PesquisarPorId(int id)
        {
            var resultado = _dbContext?.Tarefas?.FirstOrDefault(x => x.IdTarefa == id);

            if (resultado == null)
            {
                return new ServiceResponse<Tarefa>("Tarefa não encontrada!");
            }
            else
            {
                return new ServiceResponse<Tarefa>(resultado);
            }
        }

        public ServiceResponse<Tarefa> PesquisarPorNome(string nome)
        {
            var resultado = _dbContext?.Tarefas?.FirstOrDefault(x => x.Título == nome);

            if (resultado == null)
            {
                return new ServiceResponse<Tarefa>("Tarefa não encontrada!");
            }
            else
            {
                return new ServiceResponse<Tarefa>(resultado);
            }
        }

        public ServiceResponse<Tarefa> Editar(int id, TarefaUpdateRequest model)
        {
            var resultado = _dbContext?.Tarefas?.FirstOrDefault(x => x.IdTarefa == id);
            if (resultado == null)
            {
                return new ServiceResponse<Tarefa>("Tarefa não encontrada!");
            }

            //sendo encontrada => atualizando...
            resultado.Concluído = model.Concluído;
            resultado.Prioridade = model.Prioridade.Value;

            _dbContext.Tarefas.Add(resultado).State = EntityState.Modified; //_dbContext.Tarefas - Acessamos
                                                                            //o DBSet para conseguirmos manipular a entidade.
            _dbContext?.SaveChanges();
            //Sempre que fizermos qualquer alteração, teremos que executar _dbContext.SaveChanges(), para
            //"enviar" as alterações para o banco de dados.

            return new ServiceResponse<Tarefa>(resultado);
        }

        public ServiceResponse<bool> Deletar(int id)
        {
            var resultado = _dbContext?.Tarefas?.FirstOrDefault(x => x.IdTarefa == id);
            if (resultado == null)
            {
                return new ServiceResponse<bool>("Tarefa não encontrada!");
            }
            //sendo encontrada => deletando...
            _dbContext?.Tarefas?.Remove(resultado);
            _dbContext?.SaveChanges();
            //Sempre que fizermos qualquer alteração, teremos que executar _dbContext.SaveChanges(), para
            //"enviar" as alterações para o banco de dados.

            return new ServiceResponse<bool>(true);
        }

    }
}
