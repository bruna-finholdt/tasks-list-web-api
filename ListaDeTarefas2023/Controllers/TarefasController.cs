using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ListaDeTarefas2023.Domain.DTO;
using ListaDeTarefas2023.Domain.Entity;
using ListaDeTarefas2023.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaDeTarefas2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        //usando o TarefasService via injeção de dependência:
        private readonly TarefasService tarefaService;
        public TarefasController(TarefasService tarefaService)
        {
            this.tarefaService = tarefaService;
        }

        [HttpGet]
        public IEnumerable<Tarefa>? Get()
        {
            return tarefaService?.ListarTodos();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var retorno = tarefaService.PesquisarPorId(id);
            if (retorno.Sucesso)
            {
                return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return NotFound(retorno);
            }
        }

        [HttpGet("nome/{nome}")]
        public IActionResult GetByNome(string nome)
        {
            var retorno = tarefaService.PesquisarPorNome(nome);
            if (retorno.Sucesso)
            {
                return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return NotFound(retorno);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] TarefaCreateRequest postModel) //FromBody para indicar q o corpo da req deve
                                                                            //ser mapeado para o modelo

        //usa-se IActionResult no retorno pois aqui podemos ter um retorno específico para cada cenário: valid/not valid
        //os de cima foram alterados com os passo-a-passo e tb usam essa interface agora pois tb possuem cenários de 
        //return especificos
        {
            //validação modelo de entrada
            if (ModelState.IsValid)//modelState é o objeto que guarda o estado de validação do modelo de entrada, ou seja
                                   //a validação dos parametros do metodo
            {
                var retorno = tarefaService.CadastrarNovaTarefa(postModel);
                if (!retorno.Sucesso)
                {
                    return BadRequest(retorno.Mensagem);
                }
                else
                {
                    return Ok(retorno);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }


        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TarefaUpdateRequest putModel)
        {
            if (ModelState.IsValid)
            {
                var retorno = tarefaService.Editar(id, putModel);
                if (!retorno.Sucesso)
                {
                    return BadRequest(retorno.Mensagem);
                }
                else
                {
                    return Ok(retorno.ObjetoRetorno);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var retorno = tarefaService.Deletar(id);
            if (!retorno.Sucesso)
            {
                return BadRequest(retorno.Mensagem);
            }
            else
            {
                return Ok();
            }
        }
    }
}
