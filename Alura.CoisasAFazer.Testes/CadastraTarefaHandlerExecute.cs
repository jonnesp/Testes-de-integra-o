using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore.InMemory;
using Moq;
using Microsoft.Extensions.Logging;

namespace Alura.CoisasAFazer.Testes
{
    public class CadastraTarefaHandlerExecute
    {
        [Fact]
        public void DadaTarefaComInformacoesValidasDeveIncluirNoBanco()
        {
            //Arranje
            var comando = new CadastraTarefa("Estudar Xunit",
                new Categoria("Estudo"), new DateTime(2019, 12, 31));

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext").Options;

            var mock = new Mock<ILogger<CadastraTarefaHandler>>();

            var contexto = new DbTarefasContext(options);

            var repo = new RepositorioTarefa(contexto);

            var handler = new CadastraTarefaHandler(repo, mock.Object);
            //Act
            handler.Execute(comando);


            //Assert
            var tarefa = repo.ObtemTarefas(t => t.Titulo == "Estudar Xunit").FirstOrDefault();

            Assert.NotNull(tarefa);


        }


        [Fact]
        public void QuandoExceptionForLancadaResultadoDeveSerFalso()
        {

            //Arranje
            var comando = new CadastraTarefa("Estudar Xunit",
                new Categoria("Estudo"), new DateTime(2019, 12, 31));

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext").Options;





            var mock = new Mock<IRepositorioTarefas>();
            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>())).Throws(new Exception("Houve um erro ao incluir tarefas"));
            var repo = mock.Object;
            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();
            var handler = new CadastraTarefaHandler(repo, mockLogger.Object);
            //Act
            CommandResult resultado = handler.Execute(comando);

            Assert.False(resultado.IsSuccess);
        }

        //[Fact]
        //public void QuandoExceptionForLancadaDeveLogarAMensagemDaExcecao()
        //{

        //    var mensagemDeErroEsperada = "Houve um erro na inclusão de tarefas";
        //    var excecaoEsperada = new Exception(mensagemDeErroEsperada);

        //    var comando = new CadastraTarefa("Estudar Xunit", new Categoria("Estudo"), new DateTime(2019, 12, 31));

        //    var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();




        //    var mock = new Mock<IRepositorioTarefas>();

        //    mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
        //        .Throws(excecaoEsperada);

        //    var repo = mock.Object;

        //    var handler = new CadastraTarefaHandler(repo, mockLogger.Object);

        //    //act
        //    CommandResult resultado = handler.Execute(comando);


        //    mockLogger.Verify(l =>
        //        l.Log(
        //            LogLevel.Error, //nível de log => LogError
        //            It.IsAny<EventId>(), //identificador do evento
        //            It.IsAny<object>(), //objeto que será logado
        //            excecaoEsperada,    //exceção que será logada
        //            It.IsAny<Func<object, Exception, string>>()
        //        ), //função que converte objeto+exceção >> string
        //        Times.Once());


        //}
        delegate void CapturaMensagemLog(LogLevel level,
            EventId eventid,
            object state, Exception exc, Func<object, Exception, string> function);
            

        //[Fact]
        //public void DadaTarefasComINfoValidasDeveLogar()
        //{
        //    var mensagemDeErroEsperada = "Houve um erro na inclusão de tarefas";
        //    var excecaoEsperada = new Exception(mensagemDeErroEsperada);

        //    var comando = new CadastraTarefa("Estudar Xunit", new Categoria("Estudo"), new DateTime(2019, 12, 31));

        //    var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();
        //    LogLevel levelcapturado = LogLevel.Error;

        //    CapturaMensagemLog captura = (level, eventId, state, exception, func) =>
        //    {
        //        levelcapturado = level;

        //    };


        //    mockLogger.Setup(l =>
        //        l.Log(
        //            It.IsAny<LogLevel>(), //nível de log => LogError
        //            It.IsAny<EventId>(), //identificador do evento
        //            It.IsAny<object>(), //objeto que será logado
        //            It.IsAny<Exception>(),    //exceção que será logada
        //            It.IsAny<Func<object, Exception, string>>()
        //        )).Callback(captura);

        //    var mock = new Mock<IRepositorioTarefas>();

        //    mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
        //        .Throws(excecaoEsperada);

        //    var repo = mock.Object;

        //    var handler = new CadastraTarefaHandler(repo, mockLogger.Object);

        //    //act
        //    CommandResult resultado = handler.Execute(comando);

        //    //assert
        //    Assert.Equal(LogLevel.Information, levelcapturado);
        //}

    }



    //public static class LoggerTestingExtensions
    //{
    //    public static void LogError(this ILogger logger, string message)
    //    {
    //        logger.Log(
    //            LogLevel.Error,
    //            0,
    //            Arg.Is<FormattedLogValues>(v => v.ToString() == message),
    //            Arg.Any<Exception>(),
    //            Arg.Any<Func<object, Exception, string>>());
    //    }

    //}

}
