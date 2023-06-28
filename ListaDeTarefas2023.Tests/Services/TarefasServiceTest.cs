using ListaDeTarefas2023.DAL;
using ListaDeTarefas2023.Services;
using Microsoft.EntityFrameworkCore;
using ListaDeTarefas2023.Domain.DTO;
using ListaDeTarefas2023.Domain.Entity;

namespace ListaDeTarefas2023.Tests.Services
{
    public class TarefasServiceTest : IDisposable
    {
        /// <summary>
        /// DbContext é a camada de acesso ao banco
        /// </summary>
        private readonly AppDbContext _dbContext; //_dbContext is an instance of the AppDbContext class, which represents
                                                  //the database context for accessing the database.
        /// <summary>
        /// Service que iremos testar
        /// </summary>
        private readonly TarefasService _service; //_service is an instance of the AlbunsService class, which is the

        /// <summary>
        /// Aqui preparamos os testes
        /// </summary>
        /// 
        //construtor:
        public TarefasServiceTest()
        {
            // Criando banco em memória
            var options = new DbContextOptionsBuilder<AppDbContext>()
                // Guid.NewGuid().ToString(): Garantindo a criação de um banco novo
                //  a cada execução de teste, evitando a existência de dados não inseridos durante os testes
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            // Criamos as instâncias que vamos usar nos testes
            _dbContext = new AppDbContext(options);
            _service = new TarefasService(_dbContext);
            // the constructor initializes the _dbContext field with the database context, and initializes the _service
            // field with a new instance of the AlbunsService class.

            //No construtor criamos um banco de dados em memória do EFCore e usamos o campo _dbContext para armazená-lo 
            //Além disso cada vez que um teste é executado um novo nome aleatório é gerado Guid.NewGuid().ToString()
            //Por fim, instanciamos o _service que vamos testar
        }

        //Execução dos testes: 
        [Fact]
        public void Quando_PassadaTarefaValida_DeveCadastrar_E_Retornar()
        {
            // Preparando entrada
            var request = new TarefaCreateRequest()
            {
                Título = "Tarefa 4",
                Descrição = "Descrição tarefa 4",
                Prioridade = 1
            };

            // Executando
            var retorno = _service.CadastrarNovaTarefa(request);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.Título, request.Título);
            Assert.Equal(retorno.ObjetoRetorno.Descrição, request.Descrição);
            Assert.Equal(retorno.ObjetoRetorno.Prioridade, request.Prioridade);
            //Tests the creation of a valid task and verifies if the returned task matches the input.
        }

        [Fact]
        public void Quando_PassadaTarefaInvalida_Deve_RetornarErro()
        {
            var mensagemEsperada = "A prioridade deve ser um valor entre 1 e 5";

            // Preparando entrada
            var request = new TarefaCreateRequest()
            {
                Título = "Tarefa 5",
                Descrição = "Descrição tarefa 5",
                // Prioridade inválida para provocar erro de validação
                Prioridade = 6
            };

            // Executando
            var retorno = _service.CadastrarNovaTarefa(request);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada); //Tests the creation of an invalid task and verifies if
                                                              //the returned result indicates an error with the expected
                                                              //error message.
        }

        [Fact]
        public void Quando_ChamadoListarTodos_Deve_RetornarTodos()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaTarefasStub();

            // Executa e cria objeto do tipo lista a partir da execução
            var retorno = new List<Tarefa>(_service.ListarTodos());

            // Validando resultados
            Assert.Equal(retorno.Count, lista.Count); //Tests the listing of all tasks and verifies if the number of
                                                      //returned tasks matches the number of tasks in the database.
        }
        [Fact]
        public void Quando_ChamadoPesquisaPorId_Com_IdExistente_Deve_RetornarTarefa()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaTarefasStub();

            // Executa com id que foi cadastrado no banco
            var retorno = _service.PesquisarPorId(lista[0].IdTarefa);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.IdTarefa, lista[0].IdTarefa);
            Assert.Equal(retorno.ObjetoRetorno.Título, lista[0].Título);
            Assert.Equal(retorno.ObjetoRetorno.Descrição, lista[0].Descrição);
            Assert.Equal(retorno.ObjetoRetorno.Prioridade, lista[0].Prioridade);
            Assert.Equal(retorno.ObjetoRetorno.Concluído, lista[0].Concluído);
            //Tests the search for a task by ID when the ID exists in the database and verifies if the returned task
            //matches the expected task.
        }
        [Fact]
        public void Quando_ChamadoPesquisaPorId_Com_IdNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaTarefasStub();
            var mensagemEsperada = "Tarefa não encontrada!";

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.PesquisarPorId(lista.Count + 1);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
            //Tests the search for a task by ID when the ID does not exist in the database and verifies if the returned
            //result indicates an error with the expected error message.
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorNome_Com_NomeExistente_Deve_RetornarAlbum()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaTarefasStub();

            // Executa com nome que foi cadastrado no banco
            var retorno = _service.PesquisarPorNome(lista[0].Título);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.IdTarefa, lista[0].IdTarefa);
            Assert.Equal(retorno.ObjetoRetorno.Título, lista[0].Título);
            Assert.Equal(retorno.ObjetoRetorno.Descrição, lista[0].Descrição);
            Assert.Equal(retorno.ObjetoRetorno.Prioridade, lista[0].Prioridade);
            Assert.Equal(retorno.ObjetoRetorno.Concluído, lista[0].Concluído);
            //Tests the search for a task by name when the name exists in the database and verifies if the returned
            //task matches the expected task.
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorNome_Com_NomeNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            ListaTarefasStub();
            var mensagemEsperada = "Tarefa não encontrada!";

            // Executa com nome que não foi cadastrado no banco
            var retorno = _service.PesquisarPorNome("Tarefa que não existe");

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
            //Tests the search for a task by name when the name does not exist in the database and verifies if the
            //returned result indicates an error with the expected error message.
        }

        [Fact]
        public void Quando_ChamadoEditar_Com_IdExistente_Deve_RetornarTarefaAtualizada()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaTarefasStub();

            // Preparando entrada
            var request = new TarefaUpdateRequest()
            {
                Prioridade = 4
            };

            // Executa com id que foi cadastrado no banco
            var retorno = _service.Editar(lista[0].IdTarefa, request);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.Prioridade, lista[0].Prioridade);//Verifica se a Prioridade da Tarefa
                                                                                //foi atualizada

            //Tests the editing of a task by ID when the ID exists in the database and verifies if the returned task
            //has been updated.                                                              
        }

        [Fact]
        public void Quando_ChamadoEditar_Com_IdNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaTarefasStub();
            var mensagemEsperada = "Tarefa não encontrada!";

            // Preparando entrada
            var request = new TarefaUpdateRequest();

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.Editar(lista.Count + 1, request);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
            //Tests the editing of a task by ID when the ID does not exist in the database and verifies if the returned
            //result indicates an error with the expected error message.
        }

        [Fact]
        public void Quando_ChamadoDeletar_Com_IdExistente_Deve_RetornarTarefaDeletada()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaTarefasStub();

            // Executa com id que foi cadastrado no banco
            var retorno = _service.Deletar(lista[0].IdTarefa);

            // Validando resultados
            Assert.True(retorno.ObjetoRetorno);
            // Verifica se existe um álbum a menos na base
            Assert.Equal(_dbContext.Tarefas.Count(), lista.Count - 1);//Verifica se existe uma tarefa a menos na base
        }

        [Fact]
        public void Quando_ChamadoDeletar_Com_IdNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaTarefasStub();
            var mensagemEsperada = "Tarefa não encontrada!";

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.Deletar(lista.Count + 1);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
            // Verifica se existe o mesmo número de álbuns na base
            Assert.Equal(_dbContext.Tarefas.Count(), lista.Count);//Verifica se existe o mesmo número de tarefas na
                                                                  //base
        }

        /// <summary>
        /// Stub Tarefas
        /// </summary>
        /// <returns>Conjunto de dados que mockamos para usar em testes</returns>
        private List<Tarefa> ListaTarefasStub()
        {
            // Dados para mock
            var lista = new List<Tarefa>()
            {
                new Tarefa()
                {
                    Título = "Tarefa 1",
                    Descrição = "Descrição tarefa 1",
                    Prioridade = 5,
                    //igual ao DTO de criação (ñ cria com atributo Concluido)
                },
                new Tarefa()
                {
                    Título = "Tarefa 2",
                    Descrição = "Descrição tarefa 2",
                    Prioridade = 3,
                }
            };

            // Salvamos os dados no banco
            _dbContext.AddRange(lista);
            _dbContext.SaveChanges();

            // Retornamos para usar nas validações
            return lista;
        }

        /// <summary>
        /// Método que é executado quando os testes são encerrados.
        /// O XUnit chama o método Dispose definido na interface IDisposable.
        /// </summary>
        public void Dispose()
        {
            // Garante que o banco usado nos testes foi deletado
            _dbContext.Database.EnsureDeleted();
            // Informa pro Garbage Collector que o objeto já foi limpo. Leia mais:
            // - https://docs.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1816
            // - https://stackoverflow.com/a/151244/7467989
            GC.SuppressFinalize(this);
        }//Finalização dos testes: O método Dispose é a implementação da interface IDisposable e é chamado ao
         //final dos testes para que o banco seja excluído.
         //A linha GC.SuppressFinalize(this) é uma recomendação da Microsoft.

    }
}
